$(document).ready(function () {
    var postcardId = $('#data').data('postcard-id');
    var username = $('#data').data('username');
    var userId = $('#data').data('user-id');

    var notificationHub = $.connection.notificationHub;

    notificationHub.client.addComment = function (data) {
        $('#comment-list').prepend(parseToComment(userId, JSON.parse(data)));
    }

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
                            commentJSON.userId = userId;
                            notificationHub.server.onNewComment(JSON.stringify(commentJSON), postcardId);
                            success(commentJSON);
                        }
                    });
                },
                timeFormatter: function (time) {
                    return new Date(time).format('dd/mm/yyyy HH:MM:ss');
                },
                getComments: function (success, error) {
                    success(data);
                    $.connection.hub.start().done(function () {
                        notificationHub.server.joinGroup(postcardId);
                    });
                }
            });
        }
    });

    $(window).unload(function () {
        notificationHub.server.leaveGroup(postcardId);
    });

    var parseToComment = function(currentUserId, jsonData) {
        var isLiked = jsonData.userId === currentUserId && jsonData.user_has_upvoted === true ? ' highlight-font' : '';
        return '<li data-id="' + jsonData.id + '" class="comment">' +
            '<div class="comment-wrapper">' +
            '<img src="' + jsonData.profile_picture_url + '" class="profile-picture round"/>' +
            '<time data-original="' + jsonData.created + '">' + new Date(jsonData.created).format('dd/mm/yyyy HH:MM:ss') + '</time>' +
            '<div class="name">' + jsonData.fullname + '</div>' +
            '<div class="wrapper">' +
            '<div class="content">' + jsonData.content + '</div>' +
            '<span class="actions">' +
            '<button class="action upvote' + isLiked + '">' +
            '<span class="upvote-count">' + jsonData.upvote_count + '</span>' +
            '<i class="fa fa-thumbs-up"></i>' +
            '</button>' +
            '</span>' +
            '</div>' +
            '</div>' +
            '<ul class="child-comments"></ul>' +
            '</li>';
    }

    $('.own').click(function () {
        alert('hi!');
    })
});