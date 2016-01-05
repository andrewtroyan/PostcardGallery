$(document).ready(function () {
    var postcardId = $('#postcard-id').text();
    var username = $('#username').text();

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
                            commentJSON.fullname = username;
                            console.log(commentJSON.created);
                            success(commentJSON);
                        }
                    });
                },
                timeFormatter: function (time) {
                    return new Date(time).format('dd/mm/yyyy hh:MM:ss');
                },
                getComments: function (success, error) {
                    success(data);
                }
            });
        }
    });
});