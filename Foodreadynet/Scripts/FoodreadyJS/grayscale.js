/*!
 * Start Bootstrap - Grayscale Bootstrap Theme (http://startbootstrap.com)
 * Code licensed under the Apache License v2.0.
 * For details, see http://www.apache.org/licenses/LICENSE-2.0.
 */

// jQuery to collapse the navbar on scroll

// hide #back-top first
$("#back-top").hide();
function showImage(imgPass) {
    var pass = "images/items/food" + imgPass + ".png";
    var tl = "";
    switch (imgPass) {
        case '1':
            tl = "Fried prawns";
            break;
        case '12':
            tl = "Crab meat puffs";
            break;
        case '23':
            tl = "Shanghai style smoke fish";
            break;
        case '16':
            tl = "Golden sauteed prawns";
            break;
        case '6':
            tl = "Chinese chicken salad";
            break;
        case '5':
            tl = "Hot and sour cucumber";
            break;
        case '43':
            tl = "Gai lan with oyster sauce";
            break;
        case '42':
            tl = "Bamboo fungus, black mushroom and bok choy";
            break;
        case '46':
            tl = "Braised tofo";
            break;
        case '59':
            tl = "General tsao chicken";
            break;
        case '50':
            tl = "Eggplant with salmon fish";
            break;
        case '70':
            tl = "Honey walnut prawns";
            break;

        case '78':
            tl = "Lobster with E-F Noodle";
            break;
        case '69':
            tl = "Kung pao fish fillet";
            break;
        case '67':
            tl = "Beijin duck";
            break;
        case '81':
            tl = "Steamed black bass";
            break;
        case '66':
            tl = "Spicy crab";
            break;
        case '38':
            tl = "Zhouzhuang Pork Knuckle";
            break;
        case '58':
            tl = "Prawns with hot muster sauce";
            break;
        case '60':
            tl = "Lemon chicken";
            break;
        case '27':
            tl = "Wor won ton soup";
            break;
        case '24':
            tl = "Chicken corn soup";
            break;
        case '62':
            tl = "Crispy chicken";
            break;
        case '61':
            tl = "Salt and peper fish fillet";
            break;

        case '54':
            tl = "Pan fried ginger beef";
            break;
        case '18':
            tl = "Sandon dumpling";
            break;
        case '10':
            tl = "Shanghai steamed dumpling";
            break;
        case '20':
            tl = "Green onion pancake";
            break;
        case '2':
            tl = "Sesame pancake";
            break;
        case '77':
            tl = "Orange peel fish fillet";
            break;
        case '100':
            tl = "Shanghai Gourmet";
            break;
        case '101':
            tl = "Shanghai Gourmet";
            break;
        case '102':
            tl = "Shanghai Gourmet";
            break;
        case '103':
            tl = "Shanghai Gourmet";
            break;
        case '104':
            tl = "Shanghai Gourmet";
            break;
    }
    $('#popupImg').attr("src", pass);
    $('#poptitle').text(tl);
    $("#popupModal").modal('show');
}
$(window).scroll(function () {
    if ($(".navbar").offset().top > 76) {
        $(".navbar-fixed-top").addClass("top-nav-collapse");
    } else {
        $(".navbar-fixed-top").removeClass("top-nav-collapse");
    }
    if ($(this).scrollTop() > 800) {
        $('#back-top').fadeIn();
    } else {
        $('#back-top').fadeOut();
    }
});

// jQuery for page scrolling feature - requires jQuery Easing plugin
$(function () {
    $("#loginForm #UserName").focus();
    var searchBox = $("div#auto1 > #keyword");
    searchBox.val("Search by keyword");

    var searchBox2 = $("div#auto2 > #keyword");

    searchBox.keydown(function () {
        if (searchBox.val() == this.title) {
            searchBox.val("");
        }
    });
    searchBox.blur(function () {
        if ($.trim(searchBox.val()) == "") {
            searchBox.val(this.title);
        }
    });
    $('#searchBtn').bind('click', function () {
        if ($.trim(searchBox.val()).length < 2 || $.trim(searchBox.val()) == "Search by keyword") {
            searchBox.focus();
            return false;
        }
    });
    $('#searchBtn2').bind('click', function () {
        if ($.trim(searchBox2.val()).length < 2) {
            searchBox2.focus();
            return false;
        }
    });
    searchBox.blur();

    $('a.page-scroll').bind('click', function (event) {
        var $anchor = $(this);
        $('html, body').stop().animate({
            scrollTop: $($anchor.attr('href')).offset().top
        }, 1500, 'easeInOutExpo');
        event.preventDefault();
    });

    // scroll body to 0px on click
    $('#back-top a').click(function () {
        $('body,html').animate({
            scrollTop: 0
        }, 800);
        return false;
    });

    $('.item >img').css('height', $(window).height())

    $(window).resize(function () {

        $('.item >img').css('height', $(window).height())

    });


    $("#btnClose").click(function () {
        $("#popupModal").modal('hide');
    });

    $("#topClose").click(function () {
        $("#popupModal").modal('hide');
    });

    $("#btnClose1").click(function () {
        $("#popupModal1").modal('hide');
    });

    $("#topClose1").click(function () {
        $("#popupModal1").modal('hide');
    });

    $("#btnClose2").click(function () {
        $("#popupModal2").modal('hide');
    });

    $("#topClose2").click(function () {
        $("#popupModal2").modal('hide');
    });
    $("#topClose3").click(function () {
        $("#popupModal3").modal('hide');
    });
    $("#btnClose3").click(function () {
        $("#popupModal3").modal('hide');
    });

    $(".dropdown-menu > li").mouseover(function () {
        $(this).css('backgroundColor', '#216796');
    }).mouseout(function () {
        $(this).css('backgroundColor', '#008cba');
  });

    $('#helpChat').bind('click', function () {
        $("#popupModal3").modal('show');
        return false;
    });
    $('#btn-chat').bind('click', function () {
        $("#popupModal3").modal('show');
        return false;
    })

});
function getDirections(maplink) {
    var winName = "BizLocation";
    var ShelpWindow = window.open(maplink, winName, 'scrollbars=yes,width=1000,height=600 left=100 top=100 resizable=yes');
}
// Closes the Responsive Menu on Menu Item Click
$('.navbar-collapse ul li a').click(function () {
    $('.navbar-toggle:visible').click();
});

// Google Maps Scripts
// When the window has finished loading create our google map below
/*
google.maps.event.addDomListener(window, 'load', init);

function init() {
    // Basic options for a simple Google Map
    // For more options see: https://developers.google.com/maps/documentation/javascript/reference#MapOptions
    var mapOptions = {
        // How zoomed in you want the map to start at (always required)
        zoom: 18,

        // The latitude and longitude to center the map (always required)
        center: new google.maps.LatLng(37.911379, -122.061887), // 


    };

    // Get the HTML DOM element that will contain your map 
    // We are using a div with id="map" seen below in the <body>
    var mapElement = document.getElementById('map');

    // Create the Google Map using out element and options defined above
    var map = new google.maps.Map(mapElement, mapOptions);

    // Custom Map Marker Icon - Customize the map-marker.png file to customize your icon
    // var image = 'img/map-marker.png';
    var myLatLng = new google.maps.LatLng(37.911379, -122.061887);
    var beachMarker = new google.maps.Marker({
        position: myLatLng,
        map: map
        // icon: image
    });
}
*/
var onGroupCheckBegin = function () {
    // $("#popupModal").modal('hide');
};
var onGroupCheckSuccess = function () {
    $("#popupModal").modal('show');
    $("#grouplock").val("Unlock the cart");
};
var onGroupCheckCompleted = function () {
    $("#popupModal").modal('show');
    $("#grouplock").val("Unlock the cart");
};
var onGroupCheckFailed = function () {
    location.href = "/Home";
};

var onPopupBegin = function () {
   // $("#popupModal").modal('hide');
};
var onPopupSuccess = function () {
    $("#popupModal").modal('show');
};
var onPopupCompleted = function () {
    $("#popupModal").modal('show');
};
var onPopupFailed = function () {
    location.href = "/Home";
};

var onRepopupBegin = function () {
};
var onRepopupSuccess = function () {
    $("#popupModal").modal('show');
};
var onRepopupCompleted = function () {
    $("#popupModal").modal('show');
};
var onRepopupFailed = function () {
    location.href = "/Home";
};

var onPopup1Begin = function () {
    // $("#popupModal1").modal('hide');
};
var onPopup1Success = function () {
    $("#popupModal1").modal('show');
};
var onPopup1Completed = function () {
    $("#popupModal1").modal('show');
};
var onPopup1Failed = function () {
    location.href = "/Home";
};

var onPopup2Begin = function () {
    // $("#popupModal2").modal('hide');
};
var onPopup2Success = function () {
    $("#popupModal2").modal('show');
};
var onPopup1Completed = function () {
    $("#popupModal2").modal('show');
};
var onPopup2Failed = function () {
    location.href = "/Home";
};

var onPopupChatBegin = function () {
    $("#chatNameCheck").html("");
    $("#chatEmailCheck").html("");
    if ($("#chatUsername").val() == "") {
        $("#chatNameCheck").html("Please enter your name");
        return false;
    }
    else if ($("#chatEmail").val() == "") {
        $("#chatEmailCheck").html("Please enter your email address");
        return false;
    }
};
var onPopupChatSuccess = function () {
    $("#popupModal3").modal('show');
};
var onPopupChatCompleted = function () {
    $("#popupModal3").modal('show');
};
var onPopupChatFailed = function () {
    location.href = "/Home";
};

var onPopupAddBegin = function () {
    $("#popupModal1").modal('hide');
};
var onPopupAddSuccess = function () {
    $("#popupModal1").modal('hide');
    $("#bagitems").text($("#hitems").val());
    $('#cart-btn').click();
};
var onPopupAddCompleted = function () {
    $("#popupModal1").modal('hide');
    $("#bagitems").text($("#hitems").val());
    $('#cart-btn').click();
};
var onPopupAddFailed = function () {
    $("#popupModal1").modal('hide');
    location.href = "/Home";
};