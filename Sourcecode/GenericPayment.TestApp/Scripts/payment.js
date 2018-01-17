jQuery("#btnProceed").click(function (e) {
    e.preventDefault();
    var data = {};
    data["invoiceno"] = $("#txtInvoice").val();
    data["currency"] = "vnd";
    data["total"] = $("#txtAmount").val();
    data["hashkey"] = $("#txtHashKey").val();
    data["gateway"] = $("#txtGateway").val();

    $.ajax({
        url: 'http://thanhtoan.hebevn.com/payment/GeneratePaykey',
        data: data,
        type: 'POST',
        success: function (response) {
            if (response != "") {
                window.location = 'http://thanhtoan.hebevn.com/nganluong/index?invoiceNo=' + data["invoiceno"] + '&paykey=' + response;
            } else {
                alert("Error", "Failed to connect payment gateway");
            }
        },
        error: function () {
            alert("Error", "time out to connect payment gateway");
        }
    });
});

