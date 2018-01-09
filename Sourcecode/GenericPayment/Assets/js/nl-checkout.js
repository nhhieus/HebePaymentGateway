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
});

jQuery("#agree").click(function (e) {
    e.preventDefault();
    var data = {};
    data["CashKey"] = $("#cashkey").val();
    data["OptionPayment"] = $("input:radio[name=option_payment]:checked").val();
    data["BankCode"] = $("input:radio[name=bankcode]:checked").val();
    data["FullName"] = $("#txtFullName").val();
    data["Email"] = $("#txtEmail").val();
    data["Phone"] = $("#txtPhone").val();

    if (data["FullName"] == "" || data["Email"] == "" || data["Phone"] == "") {
        $("#divError").removeClass("hide");
        $("#divError").html("Vui lòng nhập họ tên, email và số điện thoại.");
        return;
    }

    if (data["OptionPayment"] == "ATM_ONLINE" && data["BankCode"] == undefined) {
        $("#divError").removeClass("hide");
        $("#divError").html("Vui lòng chọn ngân hang nội đia để tiến hành thanh toán.");
        return;
    }

    var url = $("#agreeUrl").val();
    $.ajax({
        url: url,
        data: data,
        type: 'POST',
        success: function (response) {
            if (response.result && response.result.redirectUrl != "") {
                window.location = response.result;
            } else {
                $("#divError").removeClass("hide");
                $("#divError").html(response.message, 'Error');
            }
        },
        error: function () {
            alert("Error", 'Something went wrong! Please try again later.');
        }
    });
});

jQuery("#cancel").click(function (e) {
    e.preventDefault();
    var data = {};
    data["CashKey"] = $("#cashkey").val();

    var url = $("#cancelUrl").val();
    $.ajax({
        url: url,
        data: data,
        type: 'POST',
        success: function (response) {
            if (response.result !== undefined) {
                window.location = response.result;
            } else {
                alert("Error", 'Something went wrong! Please try again later.');
            }
        },
        error: function () {
            alert("Error", 'Failed! Please try again later.');
        }
    });
});