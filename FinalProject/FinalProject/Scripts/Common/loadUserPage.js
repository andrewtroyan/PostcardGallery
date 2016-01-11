$(document).ready(function () {
    if ($('#medals > center').children().length === 0) {
        $('#medals').css('background-color', 'transparent');
    }

    var pageSize = calculatePageSize();
    var userId = $('#data').data('user-id');
    $('#more-postcards-button').click(loadPostcards);
    var page = 0;
    var inCallback = false;
    function loadPostcards() {
        if (page > -1 && !inCallback) {
            inCallback = true;
            $.ajax({
                url: '/Postcard/GetPostcardsPage?userId=' + userId + "&pageSize=" +
                    pageSize + "&postcardPage=" + page,
                dataType: 'json',
                beforeSend: function () {
                    $('#loader').show();
                    $('#more-postcards-button').hide();
                },
                success: function (data) {
                    $('#loader').hide();
                    $('#more-postcards-button').show();
                    if (data.length != 0) {
                        page++;
                        var i;
                        for (i = 0; i < data.length; ++i) {
                            $('#images').append(
                            '<a href="/Postcard/GetPostcardPage?id=' + data[i].databaseId + '">' +
                                '<img src="' + data[i].imageUrl + '" alt="' + data[i].name + '"' +
                            '" title="' + data[i].name + '"' + ' style="margin:10px;height:150px;width:150px">' + '</a>'
                            );
                        }
                    }
                    else {
                        page = -1;
                        $('#more-postcards-button').fadeOut();
                    }
                    inCallback = false;
                }
            });
        }
    };

    function calculatePageSize(container)
    {
        var width = $(window).width();
        if (width <= 600)
            return 2;
        else if (width <= 1000)
            return 4;
        else if (width <= 1600)
            return 8;
        else
            return 16;
    }

    loadPostcards();
});