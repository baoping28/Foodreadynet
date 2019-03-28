$(document).ready(function () {
    var chat = $.connection.chatHub;
    // Start the connection.
    $.connection.hub.start();

    $("input#gocheckout").bind('click', function () {
        $("DIV#loading").show();
        $(this).attr('disabled', true);
        var postData = '{}';
        $.ajax({
            url: '/Checkout/ProcessCheckout',
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
                    $("#bagitems").text("");
                    chat.server.sendOrder(xhr.username,xhr.biztitle,xhr.orderId, xhr.dataResult);
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

    $('select#DriverTip').bind('change', function () {
        var thisQty = $(this);
        var selectedValue = $(this).val();             // var selId = $('#' + $(this).attr('id'));

        var postData = '{"tip":"' + selectedValue + '"}';
        $("#cartTotal").animate({ opacity: 0.3 }, "200");
        $.ajax({
            url: '/Cart/ProcessTip',
            type: 'POST',
            data: postData,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (xhr) {
                if (xhr.timeexp == "timeout") {
                    $("#cartTotal").animate({ opacity: 1.0 }, "200");
                    location.href = "/Home";
                }
                $("#cartTotal").text(xhr.carttotal);
                $("#cartTotal").animate({ opacity: 1.0 }, "200");
                //setCheckoutNowBtn(xhr.isDelivery, xhr.delMin);
            },
            error: function (xhr, textStatus, errorThrown) {
                $("#cartTotal").animate({ opacity: 1.0 }, "200");
                location.href = "/Home";
            }
        });
    });

    $("input#giftcheckout").bind('click', function () {
        var gcid = $("#GCGiftcardID").val();
        var gcbalance = $("#GCBalance").val();
        var gcuserid = $("#GCUserID").val();
        var postData = '{"gcid":"' + gcid + '","balance":"' + gcbalance + '","userid":"' + gcuserid + '"}';
        $("DIV#giftloading").show();
        $(this).attr('disabled', true);
        $.ajax({
            url: '/Checkout/ProcessGiftCheckout',
            type: 'POST',
            data: postData,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (xhr) {
                $("DIV#giftloading").hide();
                $(this).attr('disabled', false);
                $("#popupModal2").modal('hide');
                $("#divsuccess").html($("#giftpopsuccess").html());
                //$("#divsuccess").html(xhr.test);
                $("#complete").hide();
                $("#divsuccess").show("slow");
                $(".checkoutTitle").text("Thank you for your ordering!");
                $("#bagitems").text("");
                chat.server.sendOrder(xhr.username, xhr.biztitle, xhr.orderId, xhr.dataResult);
            },
            error: function (xhr, textStatus, errorThrown) {
                $("DIV#giftloading").hide();
                $(this).attr('disabled', false);
                $("#popupModal2").modal('hide');
                $("#divsuccess").html("").hide();
                $("#complete").hide();
                $("#divfailed").show();
            }
        });
    });

    $("input#giftonlycheckout").bind('click', function () {
        var gcid = $("#GCGiftcardID").val();
        var gcbalance = $("#GCBalance").val();
        var gcuserid = $("#GCUserID").val();
        var postData = '{"gcid":"' + gcid + '","balance":"' + gcbalance + '","userid":"' + gcuserid + '"}';
        $("DIV#giftonlyloading").show();
        $(this).attr('disabled', true);
        $.ajax({
            url: '/Checkout/ProcessGiftOnlyCheckout',
            type: 'POST',
            data: postData,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (xhr) {
                $("DIV#giftonlyloading").hide();
                $(this).attr('disabled', false);
                $("#popupModal2").modal('hide');
                $("#divsuccess").html($("#giftpopsuccess").html());
                //$("#divsuccess").html(xhr.test);
                $("#complete").hide();
                $("#divsuccess").show("slow");
                $(".checkoutTitle").text("Thank you for your ordering!");
                $("#bagitems").text("");
                chat.server.sendOrder(xhr.username, xhr.biztitle, xhr.orderId, xhr.dataResult);
            },
            error: function (xhr, textStatus, errorThrown) {
                $("DIV#giftonlyloading").hide();
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
            url: '/Checkout/ProcessSchedule',
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

    $('select#PPScheduleDate').bind('change', function () {
        var sd = $('select#PPScheduleDate');
        var st = $('select#PPScheduleTime');
        var osh = $('#PPorderSchedule');
        DoScheduleAjaxUpdate(sd, st, osh);
    });
    $('select#PPScheduleTime').bind('change', function () {
        var sd = $('select#PPScheduleDate');
        var st = $('select#PPScheduleTime');
        var osh = $('#PPorderSchedule');
        DoScheduleAjaxUpdate(sd, st, osh);
    });


    $('select#GCScheduleDate').bind('change', function () {
        var sd = $('select#GCScheduleDate');
        var st = $('select#GCScheduleTime');
        var osh = $('#GCorderSchedule');
        DoScheduleAjaxUpdate(sd, st, osh);
    });
    $('select#GCScheduleTime').bind('change', function () {
        var sd = $('select#GCScheduleDate');
        var st = $('select#GCScheduleTime');
        var osh = $('#GCorderSchedule');
        DoScheduleAjaxUpdate(sd, st, osh);
    });

});