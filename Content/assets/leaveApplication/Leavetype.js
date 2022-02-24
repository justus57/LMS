$(function () {
    $('#UploadDiv').hide();
    //get leave selected details
    $("#Leave_Type").change(function () {

        $('#LeaveDaysEntitled').val('');
        $('#LeaveDaysTaken').val('');
        $('#LeaveBalance').val('');
        $('#LeaveAccruedDays').val('');
        $('#LeaveOpeningBalance').val('');

        console.log($('#Leave_Type').val());
        var leavetype = $('#Leave_Type').val();
        jQuery.ajax({
            url: 'LeaveApplication/GetLeaveDetails',
            type: "POST",
            data: { param1: leavetype },
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (response) {

                if (response != null) {
                    //console.log(JSON.stringify(response)); //it comes out to be string 

                    //we need to parse it to JSON
                    var data = $.parseJSON(response);

                    //set fields values
                    $('#LeaveDaysEntitled').val(data.LeaveDaysEntitled);
                    $('#LeaveDaysTaken').val(data.LeaveDaysTaken);
                    $('#LeaveBalance').val(data.RemainingDays);
                    $('#LeaveAccruedDays').val(data.AccruedDays);
                    $('#LeaveOpeningBalance').val(data.OpeningBalance);
                }
            }
        });
    });

});