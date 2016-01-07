$(document).ready(function () {
    var postcardId = $('#data').data('postcard-id');
    var username = $('#data').data('username');

    $('#rating').rating({
        'size' : 'sm',
        'showClear': false,
        'showCaption': false
    });

    $('#rating').on('rating.change', function (event, value, caption) {
        $.ajax({
            url: '/Postcard/RatePostcard?postcardId=' + postcardId + '&rating=' +
                value,
            dataType: 'json',
            beforeSend: function () {
                $('#rating-loader').show();
            },
            success: function (data) {
                $('#rating-loader').hide();
                $('#rate-success').show();
                $('#average-rating').text(data.averageRating);
                setTimeout(function () { $('#rate-success').fadeOut(); }, 1000);               
            }
        });
    })

    $.ajax({
        url: '/Data/GetComments?postcardId=' + postcardId,
        dataType: 'json',
        beforeSend: function () {
            $('#comments-loader').show();
        },
        success: function (data) {
            $('#comments-loader').hide();
            $('#comments-section').comments({
                profilePictureURL: '/Content/img/user.png',
                enableReplying: false,
                enableEditing: false,
                roundProfilePictures: true,
                newestText: $('#data').data('newest-text'),
                oldestText: $('#data').data('oldest-text'),
                popularText: $('#data').data('popular-text'),
                sendText: $('#data').data('send-text'),
                textareaPlaceholderText: $('#data').data('textarea-placeholder-text'),
                noCommentsText: $('#data').data('no-comments-text'),
                youText: username,
                upvoteComment: function (commentJSON, success, error) {
                    if (commentJSON.user_has_upvoted) {
                        $.ajax({
                            dataType: 'json',
                            url: '/Comment/Like?commentId=' + commentJSON.id,
                            success: function (data) {
                                success(commentJSON);
                            }
                        });
                    } else {
                        $.ajax({
                            dataType: 'json',
                            url: '/Comment/Dislike?commentId=' + commentJSON.id,
                            success: function (data) {
                                success(commentJSON);
                            }
                        });
                    }
                },
                postComment: function(commentJSON, success, error) {
                    $.ajax({
                        dataType: 'json',
                        url: '/Comment/PostComment?postcardId=' + postcardId +
                            '&value=' + commentJSON.content + '&creationTime=' +
                            commentJSON.created,
                        success: function (data) {
                            commentJSON.id = data.id;
                            success(commentJSON);
                        }
                    });
                },
                timeFormatter: function (time) {
                    return new Date(time).format('dd/mm/yyyy HH:MM:ss');
                },
                getComments: function (success, error) {
                    success(data);
                }
            });
        }
    });
});