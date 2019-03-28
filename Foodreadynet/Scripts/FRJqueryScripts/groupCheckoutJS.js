$(document).ready(function () {
    var chat = $.connection.chatHub;
    // Start the connection.
    $.connection.hub.start();

    $("input#gogroupcheckout").bind('click', function () {
        $("DIV#loading").show();
        $(this).attr('disabled', true);
        var postData = '{}';
        $.ajax({
            url: '/GroupCheckout/ProcessGroupCheckout',
            type: 'POST',
            data: postData,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (xhr) {
                if (xhr.result == "success") {
                    $("DIV#loading").hide();
                    $(this).attr('disabled', false);
                    $("#popupModal2").modal('hide');
                    $("#divsuccess").html($("#popsuccess").html());
                    //$("#divsuccess").html(xhr.test);
                    $("#complete").hide();
                    $("#divsuccess").show("slow");
                    $(".checkoutTitle").text("Thank you for your ordering!");
                    for (var i = 0; i < xhr.orderResult.length; i++) {
                        chat.server.sendOrder(xhr.orderResult[i].UserName, xhr.orderResult[i].BizTitle, xhr.orderResult[i].OrderId, xhr.orderResult[i].OrderResult);
                    }
                }
                else {
                    $("DIV#loading").hide();
                    $(this).attr('disabled', false);
                    $("#popupModal2").modal('hide');
                    $("#divsuccess").html("").hide();
                    $("#complete").hide();
                    $("#divfailed").show();
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                $("DIV#loading").hide();
                $(this).attr('disabled', false);
                $("#popupModal2").modal('hide');
                $("#divsuccess").html("").hide();
                $("#complete").hide();
                $("#divfailed").show();
            }
        });
    });

    var DoScheduleAjaxUpdate = function (vsd, vst, vosh) {
        var schdl = vsd.val();
        var delivtime = vst.val();
        var postData = '{"schedule":"' + schdl + '","time":"' + delivtime + '"}';
        $.ajax({
            url: '/GroupCheckout/ProcessGroupSchedule',
            type: 'POST',
            data: postData,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (xhr) {
                var os = vosh;
                if (xhr.result == "failed") {
                    var s = "<span  style='font-size:12px; color:Red;'> The restaurant is not open at your scheduled time <br/> (" + xhr.schTime + ", " + xhr.schDate + "). Please schedule for a different time.</span>";
                    os.html(s).show();
                }
                else {
                    os.html("").hide();
                }
            },
            error: function (xhr, textStatus, errorThrown) {
            }
        });
    };

    $('select#ScheduleDate').bind('change', function () {
        var sd = $('select#ScheduleDate');
        var st = $('select#ScheduleTime');
        var osh = $('#orderSchedule');
        DoScheduleAjaxUpdate(sd, st, osh);
    });
    $('select#ScheduleTime').bind('change', function () {
        var sd = $('select#ScheduleDate');
        var st = $('select#ScheduleTime');
        var osh = $('#orderSchedule');
        DoScheduleAjaxUpdate(sd, st, osh);
    });

});