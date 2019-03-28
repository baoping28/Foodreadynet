$(function () {
    $("#divSharePopup").dialog({
        width: 500,
        minHeight: 200,
        modal: true
    });
    $("a#viewcart").hide();
    $(".ui-dialog-titlebar").hide();
    $('div#usage').next().slideUp();
    $('img#popupOK').bind('click', function () {
        var name = $("#personname").val();
        if ($.trim(name).length == 0) {
            $("#emptymsg").show();
            return;
        }
        else {
            $("#emptymsg").hide();
        }
        var postData = '{"personname":"' + name + '"}';
        $.ajax({
            url: '/Share/ShareInfo',
            type: 'POST',
            data: postData,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (xhr) {
                if (xhr.doneorder == "yes") {
                    location.href = "/Home";
                }
                if (xhr.result == "yes") {
                    $("#divSharePopup").dialog("close");
                    $("#myname").text(xhr.myname);
                }
            },
            error: function (xhr, textStatus, errorThrown) {

            }
        });
    });
    $("#personname").focus(function () {
        $("#emptymsg").hide();
    });

    $('div#usage').bind('click', function () {
            var thisBody = $(this).next();
            thisBody.slideToggle();
    });
});