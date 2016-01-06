$(document).ready(function () {
    $(document).ready(function () {
        $('#styleOptions').styleSwitcher({
            hasPreview: true,
            defaultThemeId: 'black-white',
            fullPath: '/Content/Themes/',
            cookie: {
                expires: 30,
                isManagingLoad: true
            }
        });
    });
});