$(document).ready(function () {
    var setCheckoutNowBtn = function (del, min) {
        var subTl = parseFloat($("#subtotal").text().replace('$', ''));
        var orderMin = parseFloat(min);
        if (del == "pickup") {
            $("#CheckoutBtn").show("slow");
        }
        else if ((subTl < orderMin)) {
            $("#CheckoutBtn").hide();
            //$("#CheckoutNow").hide();
        }
        else {
            $("#CheckoutBtn").show();
            //$("#CheckoutNow").show();
        }
    };

    $('select.productQty').bind('change', function () {
        var thisQty = $(this);
        var selectedValue = $(this).val();
        var itemId = $(this).next().val();
        var prodSub = '#proSubt-' + itemId;
        var postData = '{"itemId":"' + itemId + '","qty":"' + selectedValue + '"}';
        $("#CartContent").animate({ opacity: 0.3 }, "200");
        $.ajax({
            url: '/Cart/UpdateQuantity',
            type: 'POST',
            data: postData,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (xhr) {
                //$("#CartQty").text(xhr.cartitems);
                //$("#CartSum").text(xhr.subtotal);
                if (xhr.timeexp == "timeout") {
                    $("#CartContent").animate({ opacity: 1.0 }, "200");
                    location.href = "/Home";
                }
                var cpChoice = $("#CouponChoice");
                //cpChoice.hide();
                //cpChoice.show();
                //cpChoice.html("<option value='21'>21</option>");
                //cpChoice.append("<option value='22' selected='selected'>22</option>");
                cpChoice.html(xhr.couponDD);
                //cpChoice.children().remove().end().append('<option value="1">1</option><option value="2">2</option><option value="3">3</option>');
                //$("select#CouponChoice option[value='2']").remove();
                if (xhr.isDelivery == "delivery") {
                    $("select#DriverTip option:eq(0)").text("Tip the driver 15% of subtotal");
                }
                else {
                    $("select#DriverTip option:eq(0)").text("Tip? No, not this time");
                }
                $("select#DriverTip option:eq(1)").remove();
                $("select#DriverTip option:eq(1)").remove();
                $("select#DriverTip option:eq(1)").remove();
                $("select#DriverTip option:eq(1)").remove();
                $("select#DriverTip option:eq(1)").remove();
                $("select#DriverTip option:eq(1)").remove();
                $("select#DriverTip option:eq(1)").remove();
                $("select#DriverTip option:eq(1)").remove();
                $("select#DriverTip option:eq(1)").remove();
                $("select#DriverTip option:eq(1)").remove();
                $("select#DriverTip option:eq(1)").remove();

                $("select#DriverTip option:eq(1)").remove();
                $("select#DriverTip option:eq(1)").remove();
                $("select#DriverTip option:eq(1)").remove();
                $("select#DriverTip option:eq(1)").remove();
                $("select#DriverTip option:eq(1)").remove();
                $("select#DriverTip option:eq(1)").remove();
                $("select#DriverTip option:eq(1)").remove();
                $("select#DriverTip option:eq(1)").remove();
                $("select#DriverTip option:eq(1)").remove();
                $("select#DriverTip option:eq(1)").remove();

                $("select#DriverTip option:eq(0)").after(xhr.tip15);
                $("select#DriverTip option:eq(1)").after(xhr.tip16);
                $("select#DriverTip option:eq(2)").after(xhr.tip17);
                $("select#DriverTip option:eq(3)").after(xhr.tip18);
                $("select#DriverTip option:eq(4)").after(xhr.tip19);
                $("select#DriverTip option:eq(5)").after(xhr.tip20);
                $("select#DriverTip option:eq(6)").after(xhr.tip21);
                $("select#DriverTip option:eq(7)").after(xhr.tip22);
                $("select#DriverTip option:eq(8)").after(xhr.tip23);
                $("select#DriverTip option:eq(9)").after(xhr.tip24);
                $("select#DriverTip option:eq(10)").after(xhr.tip25);

                $("select#DriverTip option:eq(11)").after(xhr.inttip0);
                $("select#DriverTip option:eq(12)").after(xhr.inttip1);
                $("select#DriverTip option:eq(13)").after(xhr.inttip2);
                $("select#DriverTip option:eq(14)").after(xhr.inttip3);
                $("select#DriverTip option:eq(15)").after(xhr.inttip4);
                $("select#DriverTip option:eq(16)").after(xhr.inttip5);
                $("select#DriverTip option:eq(17)").after(xhr.inttip6);
                $("select#DriverTip option:eq(18)").after(xhr.inttip7);
                $("select#DriverTip option:eq(19)").after(xhr.inttip8);
                $("select#DriverTip option:eq(20)").after(xhr.inttip9);
                $('#DriverTip').val('0.00');
                if (xhr.couponshow == "OK") {
                    cpChoice.show();
                }
                else {
                    cpChoice.hide();
                }
                var cb = $("#CheckoutBtn");
                if (xhr.btnshow == "show") {
                    cb.css('display','block');
                }
                else {
                    cb.css('display','none');
                }
                $("#subtotal").text(xhr.subtotal);
                $("#cartTotal").text(xhr.total);
                $("#tax").text(xhr.carttax);
                $(prodSub).text(xhr.productsubtotal);
                $("#bagitems").text(xhr.cartitems);
                $("#CartContent").animate({ opacity: 1.0 }, "200");
               // setCheckoutNowBtn(xhr.isDelivery, xhr.delMin);
            },
            error: function (xhr, textStatus, errorThrown) {
                $("#CartContent").animate({ opacity: 1.0 }, "200");
                location.href = "/Home";
            }
        });
    });
    $(".rm-item").bind('click', function () {
        var thisOne = $(this);
        var thisTitle = $(this).next().val();
        $.confirm({
            text: "Are you sure you want to remove the <b>" + thisTitle + "</b>?",
            confirm: function() {
                thisOne.next().next().click();
            },
            cancel: function() {
                // nothing to do
            },
            dialogClass: "dialogbox"
        });
    });
    $('select#CouponChoice').bind('change', function () {
        var thisQty = $(this);
        var selectedValue = $(this).val();             // var selId = $('#' + $(this).attr('id'));

        var postData = '{"coupon":"' + selectedValue + '"}';
        $("#CartContent").animate({ opacity: 0.3 }, "200");
        $.ajax({
            url: '/Cart/ProcessCoupon',
            type: 'POST',
            data: postData,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (xhr) {
                if (xhr.timeexp == "timeout") {
                    $("#CartContent").animate({ opacity: 1.0 }, "200");
                    location.href = "/Home";
                }
                //$("#CartQty").text(xhr.cartitems);
                //$("#CartSum").text(xhr.subtotal);
                //thisQty.append("<option value='21'>21</option>");
                //$("select.productQty").children().remove().end().append('<option value="1">1</option><option value="2">2</option><option value="3">3</option>');
                //$("select.productQty option[value='2']").remove();$("#history option:first").after("<option>sdsadas</option>");

                var cb = $("#CheckoutBtn");
                if (xhr.btnshow == "show") {
                    cb.show();
                }
                else {
                    cb.hide();
                }
                $("#subtotal").text(xhr.subtotal);
                $("#cartTotal").text(xhr.total);
                $("#tax").text(xhr.carttax);
                $("#CartContent").animate({ opacity: 1.0 }, "200");
               // setCheckoutNowBtn(xhr.isDelivery, xhr.delMin);
            },
            error: function (xhr, textStatus, errorThrown) {
                $("#CartContent").animate({ opacity: 1.0 }, "200");
                location.href = "/Home";
            }
        });
    });


    $('select#DriverTip').bind('change', function () {
        var thisQty = $(this);
        var selectedValue = $(this).val();             // var selId = $('#' + $(this).attr('id'));

        var postData = '{"tip":"' + selectedValue + '"}';
        $("#CartContent").animate({ opacity: 0.3 }, "200");
        $.ajax({
            url: '/Cart/ProcessTip',
            type: 'POST',
            data: postData,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (xhr) {
                if (xhr.timeexp == "timeout") {
                    $("#CartContent").animate({ opacity: 1.0 }, "200");
                    location.href = "/Home";
                }

                var cb = $("#CheckoutBtn");
                if (xhr.btnshow == "show") {
                    cb.show();
                }
                else {
                    cb.hide();
                }
                $("#cartTotal").text(xhr.carttotal);
                $("#CartContent").animate({ opacity: 1.0 }, "200");
                //setCheckoutNowBtn(xhr.isDelivery, xhr.delMin);
            },
            error: function (xhr, textStatus, errorThrown) {
                $("#CartContent").animate({ opacity: 1.0 }, "200");
                location.href = "/Home";
            }
        });
    });

    var IsRadioBtnChecked = function (groupobj) {
        var all_answered = true;
        groupobj.each(function () {
            var name = $(this).attr("name");
            if ($("input:radio[name=" + name + "]:checked").length == 0) {
                all_answered = false;
            }
        });
        return all_answered;
    };
    $("input:radio[name=DeliveryGroup]").bind('click', function () {
        var p = "true";
        if (IsRadioBtnChecked($("input:radio[name=DeliveryGroup]"))) {
            p = $("input:radio[name=DeliveryGroup]:checked").val();
        }
        var postData = '{"del":"' + p + '"}';
        $("#CartContent").animate({ opacity: 0.3 }, "200");
        $.ajax({
            url: '/Cart/UpdateDelivery',
            type: 'POST',
            data: postData,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (xhr) {
                if (xhr.timeexp == "timeout") {
                    $("#CartContent").animate({ opacity: 1.0 }, "200");
                    location.href = "/Home";
                }

                var cb = $("#CheckoutBtn");
                if (xhr.btnshow == "show") {
                    cb.show();
                }
                else {
                    cb.hide();
                }
                if (xhr.isDelivery == "delivery") {
                    $("select#DriverTip option:eq(0)").text("Tip the driver 15% of subtotal");
                }
                else {
                    $("select#DriverTip option:eq(0)").text("Tip? No, not this time");
                }
                $("select#DriverTip option:eq(0)").val(xhr.basevalue);
                $("select#DriverTip option:eq(12)").remove();
                $("select#DriverTip option:eq(12)").remove();
                $("select#DriverTip option:eq(12)").remove();
                $("select#DriverTip option:eq(12)").remove();
                $("select#DriverTip option:eq(12)").remove();
                $("select#DriverTip option:eq(12)").remove();
                $("select#DriverTip option:eq(12)").remove();
                $("select#DriverTip option:eq(12)").remove();
                $("select#DriverTip option:eq(12)").remove();
                $("select#DriverTip option:eq(12)").remove();
                $("select#DriverTip option:eq(12)").remove();

                $("select#DriverTip option:eq(11)").after(xhr.inttip0);
                $("select#DriverTip option:eq(12)").after(xhr.inttip1);
                $("select#DriverTip option:eq(13)").after(xhr.inttip2);
                $("select#DriverTip option:eq(14)").after(xhr.inttip3);
                $("select#DriverTip option:eq(15)").after(xhr.inttip4);
                $("select#DriverTip option:eq(16)").after(xhr.inttip5);
                $("select#DriverTip option:eq(17)").after(xhr.inttip6);
                $("select#DriverTip option:eq(18)").after(xhr.inttip7);
                $("select#DriverTip option:eq(19)").after(xhr.inttip8);
                $("select#DriverTip option:eq(20)").after(xhr.inttip9);
                $("select#DriverTip").val(xhr.basevalue);
                //$("#CartQty").text(xhr.cartitems);
                //$("#CartSum").text(xhr.subtotal);
                $("#subtotal").text(xhr.subtotal);
                $("#cartTotal").text(xhr.total);
                $("#CartContent").animate({ opacity: 1.0 }, "200");
                setCharge(xhr.isDelivery, xhr.delFee);
            },
            error: function (xhr, textStatus, errorThrown) {
                $("#CartContent").animate({ opacity: 1.0 }, "200");
                location.href = "/Home";
            }
        });
    });
    var setCharge = function (del, fee) {
        if (del == "pickup") {
            $("#DelCharge").text("");
            $("#Fee").text("");
        }
        else {
            $("#DelCharge").text("Delivery Charge:");
            $("#Fee").text("$" + fee);
        }
    };
});