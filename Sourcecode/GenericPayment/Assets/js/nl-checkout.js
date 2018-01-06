jQuery("#agree").click(function (e) {
    e.preventDefault();
    var data = {};
    data["CashKey"] = $("#cashkey").val();
    data["OptionPayment"] = $("option_payment").is(":checked").val();
    data["BankCode"] = $("bankcode").is(":checked").val();

    $.ajax({
        url: '/paypal/AgreeToPay',
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

jQuery("#cancel").click(function (e) {
    e.preventDefault();
    var data = {};
    data["CashKey"] = $("#cashkey").val();

    $.ajax({
        url: '/paypal/canceltopay',
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