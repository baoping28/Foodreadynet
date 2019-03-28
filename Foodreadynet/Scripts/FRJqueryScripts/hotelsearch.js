$(document).ready(function () {
    $("Div#loading").hide();
    $("#btngo").removeAttr('disabled');

    $('a.hotelsearch').each(function () {
        $(this).click(function (e) {
            $("Div#loading").show();
            $('#btngo').attr("disabled", "true");
        });
    });
});