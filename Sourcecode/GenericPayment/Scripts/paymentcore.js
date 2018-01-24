$(document).ready(function () {
    var spinner = new Spinner({
        lines: 11, // The number of lines to draw
        length: 33, // The length of each line
        width: 9, // The line thickness
        radius: 45, // The radius of the inner circle
        scale: 1, // Scales overall size of the spinner
        corners: 1, // Corner roundness (0..1)
        color: '#ffff80', // CSS color or array of colors
        fadeColor: 'transparent', // CSS color or array of colors
        opacity: 0.25, // Opacity of the lines
        rotate: 0, // The rotation offset
        direction: 1, // 1: clockwise, -1: counterclockwise
        speed: 1, // Rounds per second
        trail: 60, // Afterglow percentage
        fps: 20, // Frames per second when using setTimeout() as a fallback in IE 9
        zIndex: 2e9, // The z-index (defaults to 2000000000)
        className: 'spinner', // The CSS class to assign to the spinner
        top: '50%', // Top position relative to parent
        left: '50%', // Left position relative to parent
        shadow: 'none', // Box-shadow for the lines
        position: 'absolute' // Element positioning
    }).spin(document.getElementById("loadingMask")); 

    $(document).bind("ajaxSend", function () {
        $("#loadingMask").show();
    }).bind("ajaxComplete", function () {
        $("#loadingMask").fadeOut();
    }).ajaxError(function (event, response, settings, thrownError) {
        $("#loadingMask").fadeOut();
    });

    $("#loadingMask").fadeOut();

    $("#logout").click(function(e) {
        e.preventDefault();

        window.location = $("#logoutUrl").val();
    });

    $("#english").click(function (e) {
        e.preventDefault();

        var data = { lang: "en-US" };
        var url = $("#changeLanguageUrl").val();

        $.ajax({
            url: url,
            data: data,
            type: 'POST',
            success: function (response) {
                if (response && response.result) {
                    window.location.reload();
                }

            },
            error: function () {
                alert("Error", 'Something went wrong! Please try again later.');
            }
        });
    });

    $("#vietnam").click(function (e) {
        e.preventDefault();

        var data = { lang: "vi-VN" };
        var url = $("#changeLanguageUrl").val();

        $.ajax({
            url: url,
            data: data,
            type: 'POST',
            success: function (response) {
                if (response && response.result) {
                    window.location.reload();
                }

            },
            error: function () {
                alert("Error", 'Something went wrong! Please try again later.');
            }
        });
    });
});
