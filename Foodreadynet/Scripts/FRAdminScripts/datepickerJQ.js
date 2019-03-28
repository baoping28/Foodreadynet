$(document).ready(function () {
    var pickerOpts = {
        //appendText: "mm/dd/yyyy",
        defaultDate: "+5",
        showOtherMonths: true,
        changeMonth: true,
        changeYear: true,
        yearRange: "-80:+10"
        // showOn: "button",
        //  buttonImage: "../Images/calendar.png",
        //  buttonImageOnly: true
    };
    $('input#StartDate').datepicker(pickerOpts);
    $('input#EndDate').datepicker(pickerOpts);
    $('input#CloseDay').datepicker(pickerOpts);


    $('input#vFromDate').datepicker(pickerOpts);
    $('input#vToDate').datepicker(pickerOpts);
});