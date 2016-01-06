﻿$(document).ready(function () {
    var pageSize = calculatePageSize();
    console.log(pageSize);
    var userId = $('#user-id').text();

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
                },
                success: function (data) {
                    console.log(data);
                    $('#loader').hide();
                    if (data.length != 0) {
                        page++;
                        var i;
                        for (i = 0; i < data.length; ++i) {
                            $('#images').append(
                            '<a href="/Postcard/GetPostcardPage?id=' + data[i].databaseId + '">' +
                                '<img src="' + data[i].thumbnailUrl + '" alt="' + data[i].name + '"' +
                            '" title="' + data[i].name + '"' + ' style="margin:10px">' + '</a>'
                            );
                        }
                    }
                    else {
                        console.log('fuck!');
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
            return 4;
        else if (width <= 1000)
            return 8;
        else if (width <= 1600)
            return 12;
        else
            return 16;
    }

    loadPostcards();

    $('#more-postcards-button').click(loadPostcards);
});