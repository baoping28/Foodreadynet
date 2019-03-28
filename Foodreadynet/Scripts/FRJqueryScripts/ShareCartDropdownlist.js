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
        var selectedValue = $(this).val(); // var selId = $('#' + $(this).attr('id')); var itemId = $(this).next().val();
        var itemId = $(this).next().val();
        var prodSub = '#proSubt-' + itemId;
        var postData = '{"itemId":"' + itemId + '","qty":"' + selectedValue + '"}';
        $("#CartContent").animate({ opacity: 0.3 }, "200");
        $.ajax({
            url: '/Group/UpdateQuantity',
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
               // if (xhr.sharelocked == "yes") {
                //    $("#CartContent").animate({ opacity: 1.0 }, "200");
                  //  $("#locktitle").show();
                  //  return false;
               // }
               // $("#locktitle").hide();
                var cb = $("#CheckoutBtn");
                if (xhr.btnshow == "show") {
                    cb.show();
                }
                else {
                    cb.hide();
                }
                $("#subtotal").text(xhr.subtotal);
                $("#globalsubtotal").text(xhr.globalsubtotal);
                $("#cartTotal").text(xhr.total);
                $("#tax").text(xhr.carttax);
                $(prodSub).text(xhr.productsubtotal);
                $("#CartContent").animate({ opacity: 1.0 }, "200");
                $("DIV#otherscart").slideUp();
                // setCheckoutNowBtn(xhr.isDelivery, xhr.delMin);
            },
            error: function (xhr, textStatus, errorThrown) {
                $("#CartContent").animate({ opacity: 1.0 }, "200");
                location.href = "/Home";
            }
        });
    });

    $('select#CouponChoice').bind('change', function () {
        var thisQty = $(this);
        var selectedValue = $(this).val();             // var selId = $('#' + $(this).attr('id'));

        var postData = '{"coupon":"' + selectedValue + '"}';
        $("#CartContent").animate({ opacity: 0.3 }, "200");
        $.ajax({
            url: '/Group/ProcessCoupon',
            type: 'POST',
            data: postData,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (xhr) {
                if (xhr.timeexp == "timeout") {
                    $("#CartContent").animate({ opacity: 1.0 }, "200");
                    location.href = "/Home";
                }
              //  if (xhr.sharelocked == "yes") {
               //     $("#CartContent").animate({ opacity: 1.0 }, "200");
                //    $("#locktitle").show();
                //    return false;
              //  }
              //  $("#locktitle").hide();
                var cb = $("#CheckoutBtn");
                if (xhr.btnshow == "show") {
                    cb.show();
                }
                else {
                    cb.hide();
                }
                $("#subtotal").text(xhr.subtotal);
                $("#globalsubtotal").text(xhr.globalsubtotal);
                $("#cartTotal").text(xhr.total);
                $("#tax").text(xhr.carttax);
                $("#CartContent").animate({ opacity: 1.0 }, "200");
                $("DIV#otherscart").slideUp();
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
            url: '/Group/ProcessTip',
            type: 'POST',
            data: postData,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (xhr) {
                if (xhr.timeexp == "timeout") {
                    $("#CartContent").animate({ opacity: 1.0 }, "200");
                    location.href = "/Home";
                }
             //   if (xhr.sharelocked == "yes") {
             //       $("#CartContent").animate({ opacity: 1.0 }, "200");
              //      $("#locktitle").show();
              //      return false;
              //  }
              //  $("#locktitle").hide();

                var cb = $("#CheckoutBtn");
                if (xhr.btnshow == "show") {
                    cb.show();
                }
                else {
                    cb.hide();
                }
                $("#subtotal").text(xhr.subtotal);
                $("#globalsubtotal").text(xhr.globalsubtotal);
                $("#cartTotal").text(xhr.total);
                $("#tax").text(xhr.carttax);
                $("#CartContent").animate({ opacity: 1.0 }, "200");
                $("DIV#otherscart").slideUp();
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
            url: '/Group/UpdateDelivery',
            type: 'POST',
            data: postData,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (xhr) {
                if (xhr.timeexp == "timeout") {
                    $("#CartContent").animate({ opacity: 1.0 }, "200");
                    location.href = "/Home";
                }
             //   if (xhr.sharelocked == "yes") {
              //      $("#CartContent").animate({ opacity: 1.0 }, "200");
              //      $("#locktitle").show();
              //      return false;
              //  }
              //  $("#locktitle").hide();

                var cb = $("input#CheckoutBtn");
                if (xhr.btnshow == "show") {
                    cb.show();
                }
                else {
                    cb.hide();
                }
                //$("#CartQty").text(xhr.cartitems);
                //$("#CartSum").text(xhr.subtotal);
                $("#subtotal").text(xhr.subtotal);
                $("#globalsubtotal").text(xhr.globalsubtotal);
                $("#cartTotal").text(xhr.total);
                $("#tax").text(xhr.carttax);
                $("#CartContent").animate({ opacity: 1.0 }, "200");
                setCharge(xhr.isDelivery, xhr.delFee);
                $("DIV#otherscart").slideUp();
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
    $('#popupCheckClose').click(function (e) {
        $("#divCheckPopup").trigger('close');
    });
});