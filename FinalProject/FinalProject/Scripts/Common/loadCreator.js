var templateButton = $('#drop-button');

$(templateButton).css('-webkit-animation', 'wiggle 2s linear infinite');

$(templateButton).click(function () {
    $(this).css('-webkit-animation', 'none');    
});