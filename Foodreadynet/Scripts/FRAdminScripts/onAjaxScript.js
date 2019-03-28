

var onAddBegin = function () {
    // $("Div#loading").show();
    //$("#navFloating").animate({ opacity: 0.1 }, "fast");
};
var onAddSuccess = function () {
    //$("DIV#loading").hide();
    $("#divAddPopup").popup_me({ centered: true });
};
var onAddCompleted = function () {
    //$("DIV#loading").hide();
    $("#divAddPopup").popup_me({ centered: true });
};
var onAddFailed = function () {
   // $("DIV#loading").hide();
    // $("#navFloating").animate({ opacity: 1.0 }, "fast");
    location.href = "/Home";
};