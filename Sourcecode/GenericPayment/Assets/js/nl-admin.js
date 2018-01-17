$(document).ready(function () {

    var today = new Date();
    var startDate = new Date(today.getFullYear() - 1, today.getMonth(), today.getDate());

    $('#fromDatePicker')
        .datepicker({
            format: 'dd/mm/yyyy',
            autoclose: true,
            startDate: startDate,
            endDate : today
        })
        .on('changeDate', function (e) {
            // Revalidate the date field
            //$('#eventForm').formValidation('revalidateField', 'date');
        });

    $('#toDatePicker')
        .datepicker({
            format: 'dd/mm/yyyy',
            autoclose: true,
            startDate: startDate,
            endDate: today
        })
        .on('changeDate', function (e) {
            // Revalidate the date field
            //$('#eventForm').formValidation('revalidateField', 'date');
        });

    startDate = new Date(today.getFullYear(), today.getMonth() - 1, today.getDate());
    today = new Date(today.getFullYear(), today.getMonth(), today.getDate());
    $("#fromDatePicker").datepicker('update', startDate);
    $("#toDatePicker").datepicker('update', today);


    $("#btnSearch").click(function (e) {
        e.preventDefault();

        var data = {};
        data["invoiceno"] = $("#invoiceno").val();
        data["paykey"] = $("#paykey").val();
        data["providertransref"] = $("#providertransref").val();
        data["fromdate"] = $("#fromdate").val();
        data["todate"] = $("#todate").val();

        var url = $("#searchUrl").val();
        $.ajax({
            url: url,
            data: data,
            type: 'POST',
            success: function (response) {
                var searchRef = $("#searchResult");
                searchRef.html(response);

            },
            error: function () {
                alert("Error", 'Something went wrong! Please try again later.');
            }
        });
    });
});

function viewRawData(data) {
    var modelHeader = $("#modelHeader");
    modelHeader.html("Payent Data");

    var modelBody = $("#modelBody");
    modelBody.html("<p>" + JSON.stringify(data) + "</p>");

    var myModal = $("#myModal");
    myModal.show();
}