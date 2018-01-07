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
        $("#fillinfo").addClass("error");
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
                $("#divError").html(response.message, 'Error');
                $("#divError").addClass("error");
            }
        },
        error: function () {
            $("#divError").error('Failed!', 'Error');
        }
    });
});

jQuery("#cancel").click(function (e) {
    e.preventDefault();
    var data = {};
    data["CashKey"] = $("#cashkey").val();

    var url = $("#cancelUrl");
    $.ajax({
        url: url,
        data: data,
        type: 'POST',
        success: function (response) {
            if (response.result !== undefined) {
                window.location = response.result;
            } else {
                toastr.error('Something went wrong! Please try again later.', 'Error');
            }
        },
        error: function () {
            toastr.error('Failed!', 'Error');
        }
    });
});