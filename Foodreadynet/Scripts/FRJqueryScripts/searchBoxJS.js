$(document).ready(function () {
    var searchZipBox = $("#autoZipcode");
    searchZipBox.val("Your Zip Code (Required)");

    var searchAddressBox = $("#autoAddress");
    searchAddressBox.val("Your Street, City, State (Required)");

    searchAddressBox.keydown(function () {
        if (searchAddressBox.val() == this.title) {
            searchAddressBox.val("");
        }
    });

    searchZipBox.keydown(function () {
        if (searchZipBox.val() == this.title) {
            searchZipBox.val("");
        }
    });

    searchZipBox.focus(function () {
        $("#geomsg").html("").hide("slow");
    });
    searchZipBox.blur(function () {
        if ($.trim(searchZipBox.val()) == "") {
            searchZipBox.val(this.title);
        }
        $("#geomsg").html("").hide("slow");
    });

    searchAddressBox.focus(function () {
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
    $("#goImg").bind("click", function () {
       // deliv = $("input[@@name='DeliveryGroup']:checked").val();
        addr = $("input#autoAddress").val();
        zip = $("#autoZipcode").val();
        if (addr == "Your Street, City, State (Required)") {
            addr = "";
        }
        if (addr == "") {
            $("#geomsg").html("Please enter your street, city, state.").show("slow"); return;
        }
        if (zip == "Your Zip Code (Required)") {
            zip = "";
        }

        var value = zip.split("---");
        zip = value[0];

        if ($.trim(zip).length != 5) { $("#geomsg").html("Invalid zip code! Please try again.").show("slow"); return; }

        var postData = '{"address":"' + addr + '","zipcode":"' + zip + '"}';
        $.ajax({
            url: '/Home/ValidateAddress',
            type: 'POST',
            data: postData,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                if (result.resp == "OK") {
                    location.href = "/Restaurants" + "?address=" + addr + "&zip=" + zip;
                }
                else {
                    $("#geomsg").html("Invalid address! Please try again.").show("slow");
                }
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
        $.ajax({
            url: '/Home/UpdateHistory',
            type: 'POST',
            data: postData,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (result) {

            }
        });;
    });
});