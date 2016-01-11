$(document).ready(function () {
    $('.combobox').combobox();

    $('#file-opener').click(function () {
        $('#actual-file-opener').click();
    });

    var initializeCanvas = function (id) {
        var canvas = new fabric.Canvas('main-canvas');
        canvas.setDimensions({ width: 500, height: 500 });
        canvas.setBackgroundColor('white');
        return canvas;
    };

    var canvas = initializeCanvas('main-canvas');

    var client = new Dropbox.Client({ token: $('#data').data('token') });

    client.readFile($('#data').data('json-path'), function (error, data) {
        canvas.loadFromJSON(data);
        canvas.renderAll();
        var input;
        canvas.forEachObject(function (obj) {
            if (obj.type === "text") {
                obj.selectable = false;
                input = $('<input type="text" class="form-control" placeholder="' +
                    obj.text + '" value="' + obj.text + '">');
                $(input).keyup(function () {
                    obj.text = $(this).val();
                    canvas.renderAll();
                });
                $('#text-fields').prepend('<br />');
                $('#text-fields').prepend(input);
            }
        });
        animations[$('#data').data('template-name')](canvas);
    });

    $("[data-autocomplete-source]").each(function () {
        var target = $(this);
        target.autocomplete({ source: target.attr("data-autocomplete-source") });
    });

    $('#actual-file-opener').change(function (e) {
        var reader = new FileReader();
        reader.onload = function (event) {
            var imgAdd = new Image();
            imgAdd.onload = function () {
                var img = new fabric.Image(imgAdd);
                canvas.setBackgroundImage(img, canvas.renderAll.bind(canvas), {
                    scaleX: canvas.width / img.width,
                    scaleY: canvas.height / img.width
                });
            }
            imgAdd.src = event.target.result;
        }
        reader.readAsDataURL(e.target.files[0]);
    });

    var sendToServer = function (tempId, catId, postcardName, tags, imgPath, imgUrl, json) {
        $.ajax({
            url: '/Postcard/SavePostcard',
            type: 'POST',
            data: {
                templateId: tempId, categoryId: catId, name: postcardName,
                hashTags: tags, imagePath: imgPath, imageUrl: imgUrl, jsonPath: json
            },
            dataType: 'json',
            success: function (data) {
                $('#bottom-loader').hide();
                $('#save-success').fadeIn();
                setTimeout(function () { $('#save-success').fadeOut(); }, 2000);
            }
        });
    };

    $('#save-button').click(function () {
        if ($('#category-combobox').val().length === 0) {
            $('#category-required').fadeIn();
            setTimeout(function () { $('#category-required').fadeOut(); }, 2000);
            return;
        } else if ($('#name-input').val().length === 0) {
            $('#name-required').fadeIn();
            setTimeout(function () { $('#name-required').fadeOut(); }, 2000);
            return;
        } else {
            $('#save-button').hide();
            $('#bottom-loader').show();
            var tags = $('#tag-cloud li').map(function (i, n) {
                return $(n).text();
            }).get().join(',');
            var imageUrl, imagePath, jsonPath;
            client.writeFile('/img/' + (new Date()).getTime() + '.svg',
                canvas.toSVG({ viewBox: { x:175, y: 175, width: 150, height: 150 }}), function (error, stat) {
                    imagePath = stat.path;
                    client.makeUrl(imagePath, { download: true }, function (error, shareUrl) {
                        imageUrl = shareUrl.url;
                        client.writeFile('/json/postcards/' + (new Date()).getTime() + '.txt',
                        JSON.stringify(canvas), function (error, stat) {
                            jsonPath = stat.path;
                            sendToServer($('#data').data('template-id'), $('#category-combobox').val(),
                                $('#name-input').val(), tags, imagePath, imageUrl, jsonPath);
                        });
                    });
            });
        }
    });
});