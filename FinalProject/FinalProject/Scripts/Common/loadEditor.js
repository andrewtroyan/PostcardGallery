$(document).ready(function () {
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

    var sendToServer = function (imgId, imgName, imgUrl) {
        $.ajax({
            url: '/Postcard/EditPostcard',
            type: 'POST',
            data: { id: imgId, name: imgName, imageUrl: imgUrl },
            dataType: 'json',
            success: function (data) {
                $('#bottom-loader').hide();
                $('#save-success').fadeIn();
                setTimeout(function () { $('#save-success').fadeOut(); }, 2000);
            }
        });
    };

    $('#save-button').click(function () {
        if ($('#name-input').val().length === 0) {
            $('#name-required').fadeIn();
            setTimeout(function () { $('#name-required').fadeOut(); }, 2000);
            return;
        } else {
            $('#save-button').hide();
            $('#bottom-loader').show();
            var tags = $('#tag-cloud li').map(function (i, n) {
                return $(n).text();
            }).get().join(',');
            var imageUrl, imagePath = $('#data').data('image-path'), jsonPath = 
                $('#data').data('json-path');
            client.writeFile(imagePath,
                canvas.toSVG({ viewBox: { x: 175, y: 175, width: 150, height: 150 } }), function (error, stat) {
                    client.makeUrl(imagePath, { download: true }, function (error, shareUrl) {
                        imageUrl = shareUrl.url;
                        client.writeFile(jsonPath, JSON.stringify(canvas), function (error, stat) {
                            sendToServer($('#data').data('image-id'), $('#name-input').val(), imageUrl);
                        });
                    });
                });
        }
    });
});