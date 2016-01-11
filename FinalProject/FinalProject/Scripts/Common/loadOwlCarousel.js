$(document).ready(function () {
    var client = new Dropbox.Client({ token: $('#data').data('token') });

    var resizeCarouselItems = function () {
        $('#cloud-container').css('height', $(window).innerHeight() * 2 / 3);
        $('#top-container').css('min-height', $(window).innerHeight() * 2 / 3);
        $('#lately-added-container').css('min-height', $(window).innerHeight() * 2 / 3);
    };

    $(window).resize(resizeCarouselItems);

    $("#main-carousel").owlCarousel({
        singleItem: true,
        autoHeight: true,
        beforeInit: resizeCarouselItems,
        afterUpdate: function () {
            $(window).trigger('resize');
        }
    });

    $.ajax({
        url: '/Data/GetHashTags',
        dataType: 'json',
        beforeSend: function () {
            $('#cloud-loader').show();
        },
        success: function (data) {
            $('#cloud-loader').hide();
            $('#cloud-area').jQCloud(data, {
                autoResize: true
            });
        }
    });

    $.ajax({
        url: '/Data/GetTopPostcards?amount=5',
        dataType: 'json',
        beforeSend: function () {
            $('#top-loader').show();
        },
        success: function (data) {
            $('#top-loader').hide();
            var container = $('#top-postcards');
            var i;
            for (i = 0; i < data.length; ++i) {
                container.append(
                '<a href="/Postcard/GetPostcardPage?id=' + data[i].databaseId + '">' +
                    '<img src="' + data[i].imageUrl + '" alt="' + data[i].name + '"' +
                '" title="' + data[i].name + '"' + ' style="margin:10px;width:150px;height:150px">' + '</a>'
                );
            }
        }
    });

    $.ajax({
        url: '/Data/GetLatelyAddedPostcards?amount=10',
        dataType: 'json',
        beforeSend: function () {
            $('#lately-added-loader').show();
        },
        success: function (data) {
            $('#lately-added-loader').hide();
            var container = $('#lately-added-postcards');
            var i;
            for (i = 0; i < data.length; ++i)
            {
                container.append(
                '<a href="/Postcard/GetPostcardPage?id=' + data[i].databaseId + '">' +
                    '<img src="' + data[i].imageUrl + '" alt="' + data[i].name + '"' +
                '" title="' + data[i].name + '"' + ' style="margin:10px;width:150px;height:150px">' + '</a>'
                );
            }
        }
    });
});