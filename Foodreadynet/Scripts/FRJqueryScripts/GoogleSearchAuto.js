var placeSearch, autocomplete;
var componentForm = {
    street_number: 'short_name',
    route: 'long_name',
    locality: 'long_name',
    administrative_area_level_1: 'short_name',
    country: 'long_name',
    postal_code: 'short_name'
};
var componentResult = {
    street_number: '',
    route: '',
    locality: '',
    administrative_area_level_1: '',
    country: '',
    postal_code: ''
};

function initialize() {
    // Create the autocomplete object, restricting the search
    // to geographical location types.
    autocomplete = new google.maps.places.Autocomplete(
        /** @type {HTMLInputElement} */(document.getElementById('autocomplete')),
        { types: ['geocode'], componentRestrictions: { country: "us" } });
    // When the user selects an address from the dropdown,
    // populate the address fields in the form.
    google.maps.event.addListener(autocomplete, 'place_changed', function () {

        $('#gosearch').attr("disabled", "true");
        fillInAddress();
    });
}

// [START region_fillform]
function fillInAddress() {
    // Get the place details from the autocomplete object.
    var place = autocomplete.getPlace();

    for (var component in componentForm) {
        document.getElementById(component).value = '';
        document.getElementById(component).disabled = false;
    }

    // Get each component of the address from the place details
    // and fill the corresponding field on the form.
    for (var i = 0; i < place.address_components.length; i++) {
        var addressType = place.address_components[i].types[0];
        if (componentForm[addressType]) {
            var val = place.address_components[i][componentForm[addressType]];
            componentResult[addressType] = val;

        }
    }
    var street = componentResult["street_number"] + " " + componentResult["route"];
    var city = componentResult["locality"];
    var state = componentResult["administrative_area_level_1"];
    var zipcode = componentResult["postal_code"];
    var strCtSt = street + "," + city + "," + state;
    location.href = "/Restaurants" + "?street=" + street + "&address=" + strCtSt + "&city=" + city + "&zip=" + zipcode;
  }
// [END region_fillform]

// [START region_geolocation]
// Bias the autocomplete object to the user's geographical location,
// as supplied by the browser's 'navigator.geolocation' object.
function geolocate() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (position) {
            var geolocation = new google.maps.LatLng(
                position.coords.latitude, position.coords.longitude);
            var circle = new google.maps.Circle({
                center: geolocation,
                radius: position.coords.accuracy
            });
            autocomplete.setBounds(circle.getBounds());
        });
    }
}
// [END region_geolocation] 
$(function () {
    var street = "";
    var city = "";
    var state = "";
    var zipcode = "";

    $("#autocomplete").bind('focus', function () {
        $("#msg").hide();
    });
    //check if n is an integer 
    function isInt(n) {
        if (+n) {
            return +n == n && !(n % 1) && n > 0;
        }
        return false;
    }

    $('#gosearch').bind('click', function () {
        var trimauto = $.trim($("#autocomplete").val());
        if (trimauto == "" || trimauto == "Your street,city,state zipcode") {
            $("#msg").hide();
            return false;
        }
        var value = trimauto.split(",");
        if (value.length < 3 || value.length > 4) {
            $("#msg").show("slow");
            return false;
        }
        if ($.trim(value[0]).length==0 || $.trim(value[1]).length==0 || $.trim(value[2]).length==0 ) {
            $("#msg").show("slow");
            return false;
        }
        if (value.length == 3) {
            var z = $.trim(value[2]);
            var zc =$.trim(z.substr(z.length - 5, 5))
            if (isInt(zc) && zc.length == 5) {
                street = $.trim(value[0]);
                city = $.trim(value[1]);
                state = $.trim(z.substr(0, z.length - 5));
                zipcode = zc;
                var strCtSt = street + "," + city + "," + state;
                $('#gosearch').attr("disabled", "true");
                location.href = "/Restaurants" + "?address=" + strCtSt + "&city=" + city + "&zip=" + zipcode;
            }
            else {
                $("#msg").show("slow");
                return false;
            }
        }
        else {
            var z = $.trim(value[3]);
            var zc = $.trim(z.substr(z.length - 5, 5))
            if (isInt(zc) && zc.length == 5) {
                street = $.trim(value[0]);
                city = $.trim(value[1]);
                state = $.trim(value[2]);
                zipcode = zc;
                var strCtSt = street + "," + city + "," + state;
                $('#gosearch').attr("disabled", "true");
                location.href = "/Restaurants" + "?address=" + strCtSt + "&city=" + city + "&zip=" + zipcode;
            }
            else {
                $("#msg").show("slow");
                return false;
            }
        }

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
window.onload = initialize;