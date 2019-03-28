var chat = $.connection.chatHub;
var groupid = "";
var personname = "";
var bossname = "";
var bizname = "";
$(function () {

    groupid = $("#groupid").val();
    personname = $("#personname").val();
    bossname = $("#bossname").val();
    bizname = $("#bizname").val();
    $.connection.hub.start(function () {
        chat.server.joinGroup(groupid);
    }).done(function () {
        $('input#sendgroupmessage').click(function (event) {
            event.preventDefault();
            // Call the Send method on the hub.
            chat.server.sendGroupMessage(groupid, personname, $('#grouparea').val());
            // Clear text box and reset focus for next comment.
            $('#grouparea').val('').focus();
        });
    });

    chat.client.groupMessageToPage = function (name, message) {
        // Add the message to the page.
        $('#groupchat').prepend('<li>' + htmlEncode(name + ": " + message) + '</li>');
    };

    chat.client.messageToPage = function (name, itemid, quantity, productTitle, itemTotal, subtotal, bizName, tax, driverTip, globalSubtotal, globalTotal, btnShow) {
        beep();
        // Add the message to the page.
        var n = "#my-" + name;
        var m = "#sub-" + name;
        var k = "tr-" + itemid + name;
        var c = "trc-" + name;
        t = "#" + k;
        if ($(n).length == 0) {
            $("#others").prepend('<tr id="' + k + '" class="' + c + '" ><td>' + name + '</td><td>' + quantity + '</td><td>' + productTitle + '</td><td>' + bizName + '</td><td align="right"> $' + itemTotal + '</td></tr>');
            //$(m).html(subtotal);
        } else {
            $(n).after('<tr id="' + k + '"><td>' + quantity + '</td><td colspan="3">' + productTitle + '</td><td align="right"> $' + itemTotal + '</td></tr>');
            $(m).html('$' + subtotal);
        }
        $('#globalsubtotal').html('$' + globalSubtotal);
        $('#tax').html('$' + tax);
        $('#drivertip').html(driverTip);
        $('#cartTotal').html('$' + globalTotal);

        var cb = $("#CheckoutBtn");
        if (btnShow == "show" && personname == bossname) {
            cb.css('display', 'block');
        }
        else {
            cb.css('display', 'none');
        }
        if ($('#emptycart').length != 0) {
            $('#emptycart').html('');
        }
    };
    chat.client.removeToPage = function (name, itemid, subtotal, bizName, tax, driverTip, globalSubtotal, globalTotal, btnShow) {
        beep();
        // Add the message to the page.
        var m = "#sub-" + name;
        var t = "#tr-" + itemid + name;
        if ($(t).length != 0) {
            $(t).remove();
            $(m).html('$' + subtotal);
        }
        $('#globalsubtotal').html('$' + globalSubtotal);
        $('#tax').html('$' + tax);
        $('#drivertip').html(driverTip);
        $('#cartTotal').html('$' + globalTotal);

        var cb = $("#CheckoutBtn");
        if (btnShow == "show" && personname == bossname) {
            cb.css('display', 'block');
        }
        else {
            cb.css('display', 'none');
        }
    };
    chat.client.updatedToPage = function (name, itemid, quantity, title, itemSubtotal, subtotal, bizName, tax, driverTip, globalSubtotal, globalTotal, btnShow) {
        beep();
        // Add the message to the page.
        var n = "#my-" + name;
        var m = "#sub-" + name;
        var t = "#tr-" + itemid + name;
        if ($(t).length != 0) {
            if ($(n).length == 0) {
                $(t).html('<td>' + name + '</td><td>' + quantity + '</td><td>' + title + '</td><td>' + bizName + '</td><td align="right">' + itemSubtotal + '</td>');
            } else {
                $(t).html('<td>' + quantity + '</td><td colspan="3">' + title + '</td><td align="right">' + itemSubtotal + '</td>');
            }
            $(m).html(subtotal);
        }
        $('#globalsubtotal').html(globalSubtotal);
        $('#tax').html(tax);
        $('#drivertip').html(driverTip);
        $('#cartTotal').html(globalTotal);

        var cb = $("#CheckoutBtn");
        if (btnShow == "show" && personname == bossname) {
            cb.css('display', 'block');
        }
        else {
            cb.css('display', 'none');
        }
    };

    chat.client.deliveryToPage = function (name, isDelivery, delFee, driverTip, globalTotal, btnShow) {
        beep();
        // Add the message to the page.
        $('#drivertip').html(driverTip);
        $('#cartTotal').html(globalTotal);

        var cb = $("#CheckoutBtn");
        cb.css('display', 'none');
        setCharge(isDelivery, delFee);
    };

    chat.client.DoneMessageToPage = function (name) {
        beep();
        // Add the message to the page.
        var m = "#done-" + name;
        if ($(m).length != 0) {
            $(m).text('Done!');
        }
    };

    chat.client.kickToPage = function (name, tax, driverTip, globalSubtotal, globalTotal, btnShow) {
        // Add the message to the page.
        if (name == personname) {
            location.href = "/Home";
        }
        beep();
        var cl = ".trc-" + name;
        var tb = "#tb-" + pname;
        if ($(tb).length != 0) {
            $(tb).remove();
        }
        $(cl).each(function () {
            $(this).remove();
        });

        $("#globalsubtotal").text(xhr.globalsubtotal);
        $("#cartTotal").text(xhr.total);
        $("#tax").text(xhr.carttax);
        $('#drivertip').html(xhr.drivertip);
    };

    var beforechang = "";
    $('select.productQty').focus(function () { beforechang = $(this).val(); }).bind('change', function () {
        var thisQty = $(this);
        var selectedValue = $(this).val(); // var selId = $('#' + $(this).attr('id')); var itemId = $(this).next().val();
        var itemId = $(this).next().val();
        var prodSub = '#proSubt-' + itemId;
        var postData = '{"itemId":"' + itemId + '","qty":"' + selectedValue + '"}';
        $("#CartContent").animate({ opacity: 0.3 }, "fast");
        $.ajax({
            url: '/Group/UpdateQuantity',
            type: 'POST',
            data: postData,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (xhr) {
                if (xhr.timeexp == "timeout") {
                    $("#CartContent").animate({ opacity: 1.0 }, "fast");
                    location.href = "/Home";
                }
                if (xhr.canupdatequantity == "n") {
                    alert("You can not update quantity now, because either cart is locked for checking out or your order is over the budget");
                    thisQty.val(beforechang); thisQty.blur();
                    $("#CartContent").animate({ opacity: 1.0 }, "fast");
                    return false;
                }

                var cb = $("#CheckoutBtn");
                if (xhr.btnshow == "show" && xhr.isboss == "yes") {
                    cb.css('display', 'block');
                }
                else {
                    cb.css('display', 'none');
                }
                $("#subtotal").text(xhr.subtotal);
                $("#globalsubtotal").text(xhr.globalsubtotal);
                $("#cartTotal").text(xhr.total);
                $("#tax").text(xhr.carttax);
                $('#drivertip').html(xhr.drivertip);
                $(prodSub).text(xhr.productsubtotal);
                $("#CartContent").animate({ opacity: 1.0 }, "fast");
                // $("DIV#otherscart").slideUp();
                // setCheckoutNowBtn(xhr.isDelivery, xhr.delMin);
                chat.server.sendUpdatedMessage(groupid, personname, xhr.itemid, xhr.qutty, xhr.producttitle, xhr.productsubtotal, xhr.subtotal, bizname, xhr.carttax, xhr.drivertip, xhr.globalsubtotal, xhr.total, xhr.btnshow);
            },
            error: function (xhr, textStatus, errorThrown) {
                $("#CartContent").animate({ opacity: 1.0 }, "fast");
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
        $("#CartContent").animate({ opacity: 0.3 }, "fast");
        $.ajax({
            url: '/Group/UpdateDelivery',
            type: 'POST',
            data: postData,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (xhr) {
                if (xhr.timeexp == "timeout") {
                    $("#CartContent").animate({ opacity: 1.0 }, "fast");
                    location.href = "/Home";
                }
                //   if (xhr.sharelocked == "yes") {
                //      $("#CartContent").animate({ opacity: 1.0 }, "200");
                //      $("#locktitle").show();
                //      return false;
                //  }
                //  $("#locktitle").hide();

                var cb = $("#CheckoutBtn");
                if (xhr.btnshow == "show" && xhr.isboss == "yes") {
                    cb.css('display', 'block');
                }
                else {
                    cb.css('display', 'none');
                }
                $('#drivertip').html(xhr.drivertip);
                $("#cartTotal").text(xhr.globaltotal);
                $("#CartContent").animate({ opacity: 1.0 }, "fast");
                setCharge(xhr.isdelivery, xhr.delfee);
                chat.server.sendDeliveryMessage(groupid, personname, xhr.isdelivery, xhr.delfee, xhr.drivertip, xhr.globaltotal, xhr.btnshow);

            },
            error: function (xhr, textStatus, errorThrown) {
                $("#CartContent").animate({ opacity: 1.0 }, "fast");
                location.href = "/Home";
            }
        });
    });
    $('input#grouplock').bind('click', function () {
        var postData = '{}';
        $.ajax({
            url: '/Group/ChangeLock',
            type: 'POST',
            data: postData,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (xhr) {
                if (xhr.islocked == "y") {
                    $("input#grouplock").val("Unlock the cart");
                }
                else {
                    $("input#grouplock").val("Lock the cart");
                }
            },
            error: function (xhr, textStatus, errorThrown) {
            }
        });
    });
    $('input#iamdone').bind('click', function () {
        var postData = '{}';
        $.ajax({
            url: '/Group/IamDone',
            type: 'POST',
            data: postData,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (xhr) {
                chat.server.sendDoneMessage(groupid, personname);
                location.href = "/Home";
            },
            error: function (xhr, textStatus, errorThrown) {
            }
        });
    });

    $('input#kickperson').bind('click', function () {
        var thisQty = $(this);
        var pname = $(this).next().val();
        var r = confirm("Do you want to remove the person from group cart?");
        if (r == true) {
            var postData = '{"id":"' + pname + '"}';
            $("#CartContent").animate({ opacity: 0.3 }, "fast");
            $.ajax({
                url: '/Group/RemovePerson',
                type: 'POST',
                data: postData,
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function (xhr) {
                    if (xhr.islocked == "yes") {
                        alert("You can not remove the person because the group cart is locked.");
                        $("#CartContent").animate({ opacity: 1.0 }, "fast");
                        return false;
                    }

                    var cb = $("#CheckoutBtn");
                    if (xhr.btnshow == "show" && personname == bossname) {
                        cb.css('display', 'block');
                    }
                    else {
                        cb.css('display', 'none');
                    }
                    var cl = ".trc-" + name;
                    var tb = "#tb-" + pname;
                    if ($(tb).length != 0) {
                        $(tb).remove();
                    }
                    $(cl).each(function () {
                        $(this).remove();
                    });

                    $("#globalsubtotal").text(xhr.globalsubtotal);
                    $("#cartTotal").text(xhr.total);
                    $("#tax").text(xhr.carttax);
                    $('#drivertip').html(xhr.drivertip);
                    $("#CartContent").animate({ opacity: 1.0 }, "fast");
                    chat.server.sendKickMessage(groupid, pname, xhr.carttax, xhr.drivertip, xhr.globalsubtotal, xhr.total, xhr.btnshow);
                },
                error: function (xhr, textStatus, errorThrown) {
                    $("#CartContent").animate({ opacity: 1.0 }, "fast");
                    location.href = "/Home";
                }
            });
        } else {
            return false;
        }
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

// This optional function html-encodes messages for display in the page.
function htmlEncode(value) {
    var encodedValue = $('<div />').text(value).html();
    return encodedValue;
}

var onGroupAddBegin = function () {
    $("#popupModal1").modal('hide');
};
var onGroupAddSuccess = function () {
    $('#cart-btn').click();
    if ($('#g-canadditem').val() == "y") {
        $("#popupModal1").modal('hide');
        chat.server.sendMessage(groupid, personname, $('#g-itemid').val(), $('#g-quantity').val(), $('#g-productTitle').val(), $('#g-itemTotal').val(), $('#g-subtotal').val(), bizname, $('#g-tax').val(), $('#g-drivertip').val(), $('#g-globalSubtotal').val(), $('#g-globalTotal').val(), $('#g-btnShow').val());
    }
    else {
        alert("You can not add item now, because either cart is locked for checking out or your order is over the budget"); return false;
    }
};
var onGroupAddCompleted = function () {
    $('#cart-btn').click();
    if ($('#g-canadditem').val() == "y") {
        $("#popupModal1").modal('hide');
        chat.server.sendMessage(groupid, personname, $('#g-itemid').val(), $('#g-quantity').val(), $('#g-productTitle').val(), $('#g-itemTotal').val(), $('#g-subtotal').val(), bizname, $('#g-tax').val(), $('#g-drivertip').val(), $('#g-globalSubtotal').val(), $('#g-globalTotal').val(), $('#g-btnShow').val());
    }
    else {
        alert("You can not add item now, because either cart is locked for checking out or your order is over the budget"); return false;
    }
};
var onGroupAddFailed = function () {
    $("#popupModal1").modal('hide');
    location.href = "/Home";
};

var onGroupRemoveBegin = function () {
    $("#CartContent").animate({ opacity: 0.3 }, "fast");
};
var onGroupRemoveSuccess = function () {
    if ($('#r-canRemoveItem').val() == "y") {
        chat.server.sendRemoveMessage(groupid, personname, $('#r-itemid').val(), $('#r-subtotal').val(), bizname, $('#r-tax').val(), $('#r-drivertip').val(), $('#r-globalSubtotal').val(), $('#r-globalTotal').val(), $('#r-btnShow').val());
    }
    else {
        alert("You can not remove the item now, because the cart is locked for checking out.");
        $("#CartContent").animate({ opacity: 1.0 }, "fast");
        return false;
    }
    $("#CartContent").animate({ opacity: 1.0 }, "fast");
};
var onGroupRemoveCompleted = function () {
    if ($('#r-canRemoveItem').val() == "y") {
        chat.server.sendRemoveMessage(groupid, personname, $('#r-itemid').val(), $('#r-subtotal').val(), bizname, $('#r-tax').val(), $('#r-drivertip').val(), $('#r-globalSubtotal').val(), $('#r-globalTotal').val(), $('#r-btnShow').val());
    }
    else {
        alert("You can not remove the item now, because the cart is locked for checking out.");
        $("#CartContent").animate({ opacity: 1.0 }, "fast");
        return false;
    }
    $("#CartContent").animate({ opacity: 1.0 }, "fast");
};
var onGroupRemoveFailed = function () {
    location.href = "/Home";
};
function beep() {
    var snd = new Audio("data:audio/wav;base64,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qJrx4KOQjahZxWbcZgztj2c49nKmkId44S71j0c8eV9yDK6uPRzx5X18eDvjvQ6yKo9ZSS6l//8elePK/Lf//IInrOF/FvDoADYAGBMGb7FtErm5MXMlmPAJQVgWta7Zx2go+8xJ0UiCb8LHHdftWyLJE0QIAIsI+UbXu67dZMjmgDGCGl1H+vpF4NSDckSIkk7Vd+sxEhBQMRU8j/12UIRhzSaUdQ+rQU5kGeFxm+hb1oh6pWWmv3uvmReDl0UnvtapVaIzo1jZbf/pD6ElLqSX+rUmOQNpJFa/r+sa4e/pBlAABoAAAAA3CUgShLdGIxsY7AUABPRrgCABdDuQ5GC7DqPQCgbbJUAoRSUj+NIEig0YfyWUho1VBBBA//uQZB4ABZx5zfMakeAAAAmwAAAAF5F3P0w9GtAAACfAAAAAwLhMDmAYWMgVEG1U0FIGCBgXBXAtfMH10000EEEEEECUBYln03TTTdNBDZopopYvrTTdNa325mImNg3TTPV9q3pmY0xoO6bv3r00y+IDGid/9aaaZTGMuj9mpu9Mpio1dXrr5HERTZSmqU36A3CumzN/9Robv/Xx4v9ijkSRSNLQhAWumap82WRSBUqXStV/YcS+XVLnSS+WLDroqArFkMEsAS+eWmrUzrO0oEmE40RlMZ5+ODIkAyKAGUwZ3mVKmcamcJnMW26MRPgUw6j+LkhyHGVGYjSUUKNpuJUQoOIAyDvEyG8S5yfK6dhZc0Tx1KI/gviKL6qvvFs1+bWtaz58uUNnryq6kt5RzOCkPWlVqVX2a/EEBUdU1KrXLf40GoiiFXK///qpoiDXrOgqDR38JB0bw7SoL+ZB9o1RCkQjQ2CBYZKd/+VJxZRRZlqSkKiws0WFxUyCwsKiMy7hUVFhIaCrNQsKkTIsLivwKKigsj8XYlwt/WKi2N4d//uQRCSAAjURNIHpMZBGYiaQPSYyAAABLAAAAAAAACWAAAAApUF/Mg+0aohSIRobBAsMlO//Kk4soosy1JSFRYWaLC4qZBYWFRGZdwqKiwkNBVmoWFSJkWFxX4FFRQWR+LsS4W/rFRb/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////VEFHAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAU291bmRib3kuZGUAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAMjAwNGh0dHA6Ly93d3cuc291bmRib3kuZGUAAAAAAAAAACU=");
    snd.play();
}