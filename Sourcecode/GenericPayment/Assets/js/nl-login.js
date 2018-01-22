$(document).ready(function () {

    $("#login").click(function (e) {
        e.preventDefault();

        var data = {};
        data["username"] =  $("#txtUsername").val();
        data["password"] =  $("#txtPassword").val();

        var url = $("#loginUrl").val();

        $.ajax({
            url: url,
            data: data,
            type: 'POST',
            success: function (response) {
                if (response && response.result == "") {
                    window.location = response.url;

                } else {
                    var authMessage = $("#authMessage");
                    authMessage.removeClass("hide");
                    authMessage.html(response.result);
                }

            },
            error: function () {
                alert("Error", 'Something went wrong! Please try again later.');
            }
        });
    });
});
