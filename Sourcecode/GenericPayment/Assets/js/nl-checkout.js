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