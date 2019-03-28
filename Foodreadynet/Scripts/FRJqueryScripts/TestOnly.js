$(document).ready(function () {
    var imgPlus = '../../Content/Images/PlusSign.png';
    var imgMinus = '../../Content/Images/minusSign.png';

    var searchZipBox = $("#autoZipcode");
    if (searchZipBox.val() == "") {
        searchZipBox.val("Your Zip Code (Required)");
    }

    var searchAddressBox = $("#autoAddress");
    if (searchAddressBox.val() == "") {
        searchAddressBox.val("Your Street, City, State");
    }

    searchZipBox.focus(function () {
        if (searchZipBox.val() == this.title) {
            searchZipBox.val("");
        }
        $("#geomsg").html("").hide("slow");
    });
    searchZipBox.blur(function () {
        if ($.trim(searchZipBox.val()) == "") {
            searchZipBox.val(this.title);
        }
        $("#geomsg").html("").hide("slow");
    });

    searchAddressBox.focus(function () {
        if (searchAddressBox.val() == this.title) {
            searchAddressBox.val("");
        }
        $("#geomsg").html("").hide("slow");
    });
    searchAddressBox.blur(function () {
        if ($.trim(searchAddressBox.val()) == "") {
            searchAddressBox.val(this.title);
        }
        $("#geomsg").html("").hide("slow");
    });
    $("#autoZipcode").keydown(function (event) {
        // Allow: backspace, delete, tab, escape, and enter
        if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 ||
        // Allow: Ctrl+A
            (event.keyCode == 65 && event.ctrlKey === true) ||
        // Allow: home, end, left, right
            (event.keyCode >= 35 && event.keyCode <= 39)) {
            // let it happen, don't do anything
            return;
        }
        else {
            // Ensure that it is a number and stop the keypress
            if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
                event.preventDefault();
            }
        }
    });


    $('#resetPrice').click(function () {
        $("input:radio[name=Price]").prop('checked', false);
        DoAjaxUpdate();
    });

    $('#resetDistance').click(function () {
        $("input:radio[name=Distance]").prop('checked', false);
        DoAjaxUpdate();
    });

    $('#resetRating').click(function () {
        $("input:radio[name=Rating]").prop('checked', false);
        DoAjaxUpdate();
    });

    $('#resetBox').click(function () {
        $('input[type=checkbox]').each(function () {
            var checkedBox = $(this);
            this.checked = false;
            checkedBox.val(checkedBox.is(':checked') ? "checked" : "unchecked");
        });
        DoAjaxUpdate();
    });
    $("DIV.filterTitle").each(function () {
        $(this).bind('click', function () {
            var thisTitle = $(this);
            var thisBody = thisTitle.next();
            var thisImg = thisTitle.children('img');
            thisBody.slideToggle('slow');

            var toggleSrc = thisImg.attr('src');
            thisImg.removeAttr('src');
            if (toggleSrc == imgPlus) {
                thisImg.attr('src', imgMinus);
            }
            else {
                thisImg.attr('src', imgPlus);
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

    var DoAjaxUpdate = function () {
        var p = "0";
        if (IsRadioBtnChecked($("input:radio[name=Price]"))) {
            p = $("input:radio[name=Price]:checked").val();
        }

        var d = "0.0";
        if (IsRadioBtnChecked($("input:radio[name=Distance]"))) {
            d = $("input:radio[name=Distance]:checked").val();
        }

        var r = "0";
        if (IsRadioBtnChecked($("input:radio[name=Rating]"))) {
            r = $("input:radio[name=Rating]:checked").val();
        }

        var schdl = $('select#schedule').val();
        var cuis = $('select#CuisineDropDown').val();
        var delivtime = $('select#timeat').val();
        var postData = '{"schedule":"' + schdl + '","time":"' + delivtime + '","cuisine":"' + cuis + '","min":"' + p + '","dist":"' + d + '","rating":"' + r + '",';
        $("input[type='checkbox']").each(function () {
            postData = postData + '"' + $(this).attr("id") + '":"' + $(this).val() + '",';
        });
        postData = postData.slice(0, -1);
        postData = postData + '}';
        $("#filterBody").animate({ opacity: 0.1 }, "fast");
        $("Div#loading").show();
        $.ajax({
            url: '/Restaurants/FilterUpdate',
            type: 'POST',
            data: postData,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                if (result.timeexp == "timeout") {
                    $("DIV#loading").hide();
                    $("#filterBody").animate({ opacity: 1.0 }, "fast");
                    location.href = "/Home";
                }
                for (var j in result.featherOff) {
                    var dv = $('#tr-' + result.featherOff[j]);
                    dv.hide();
                }
                for (var i in result.featherOn) {
                    var dv = $('#tr-' + result.featherOn[i]);
                    var sp = $('#tr-' + result.featherOpen[i] + '  span:first-child');
                    var lt = $('#tr-' + result.featherOn[i] + '  span.localtime');
                    var ops = $('#tr-' + result.featherOn[i] + '  img.opensign');
                    ops.attr('src', 'Content/Images/closed.png');
                    lt.text(result.localOn[i]);
                    dv.removeClass('turnon');
                    dv.addClass('turnoff');
                    sp.removeClass('ontitle');
                    sp.addClass('offtitle');
                    dv.show();
                }

                for (var i in result.featherOpen) {
                    var dv = $('#tr-' + result.featherOpen[i]);
                    var sp = $('#tr-' + result.featherOpen[i] + '  span:first-child');
                    var sp = $('#tr-' + result.featherOpen[i] + '  span:first-child');
                    var lt = $('#tr-' + result.featherOpen[i] + '  span.localtime');
                    var ops = $('#tr-' + result.featherOpen[i] + '  img.opensign');
                    ops.attr('src', 'Content/Images/open.png');
                    lt.text(result.localOpen[i]);
                    dv.removeClass('turnoff');
                    dv.addClass('turnon');
                    sp.removeClass('offtitle');
                    sp.addClass('ontitle');
                }
                // var now = new Date();
                //vr aoutStr = now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds();
                $("#bizTotal").html(result.featherOnCount + "  restaurants in your area");
                $("#bizOn").html("<br/>" + result.featherOpenCount + " restaurants opening ");
                $("#bizOff").html(result.featherCloseCount + " restaurants closed ");
                $("DIV#loading").hide();
                $("#filterBody").animate({ opacity: 1.0 }, "fast");
                // alert(result.sch + " / " + result.dlvtime + " / " + result.mini + " / " + result.distc + " / " + result.rate + " / " + result.re1 + " / " + result.re2 + " / " + result.re3 + " / " + result.re4 + " / " + result.re5);
            },
            error: function (xhr, textStatus, errorThrown) {
                $("DIV#loading").hide();
                $("#filterBody").animate({ opacity: 1.0 }, "fast");
                location.href = "/Home";
            }
        });
    };

    $('A#searchGo').bind('click', function () {
        addr = $.trim($("input#autoAddress").val());
        zip = $("#autoZipcode").val();
        if ($.trim(zip).length != 5 || addr == "") return;
        cuis = $('select#CuisineDropDown').val();
        if (cuis == "All") {
            cuis = "";
        }
        if (addr == "Your Street, City, State") {
            addr = "";
        }
        if (zip == "Your Zip Code (Required)") {
            zip = "";
        }
        var postData = '{"address":"' + addr + '","zipcode":"' + zip + '","cuisine":"' + cuis + '"}';
        $.ajax({
            url: '/Home/ValidateAddress',
            type: 'POST',
            data: postData,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                if (result.resp == "OK") {
                    location.href = "/Restaurants?address=" + addr + "&zip=" + zip + "&cuisine=" + cuis;
                }
                else {
                    $("#geomsg").html("Invalid address! Please try again.").show("slow");
                }
            }
        });
    });

    $('select#CuisineDropDown').bind('change', function () {
        // if ($('select#CuisineDropDown').val().length == 0) {
        //     return false;
        // }
        DoAjaxUpdate();
    });
    $('select#schedule').bind('change', function () {
        DoAjaxUpdate();
    });
    $('select#timeat').bind('change', function () {
        DoAjaxUpdate();
    });

    $("input[type='checkbox']").bind('click', function () {
        var checkedBox = $(this);
        checkedBox.val(checkedBox.is(':checked') ? "checked" : "unchecked");
        DoAjaxUpdate();
    });



    $("input:radio[name=Price]").bind('click', function () {
        DoAjaxUpdate();
    });

    $("input:radio[name=Distance]").bind('click', function () {
        DoAjaxUpdate();
    });

    $("input:radio[name=Rating]").bind('click', function () {
        DoAjaxUpdate();
    });

});