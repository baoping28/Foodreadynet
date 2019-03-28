jQuery(document).ready(function($){
	//if you change this breakpoint in the style.css file (or _layout.scss if you use SASS), don't forget to update this value as well
	var $L = 1200,
		$menu_navigation = $('#main-nav'),
		$cart_trigger = $('#cd-cart-trigger'),
		$hamburger_icon = $('#cd-hamburger-menu'),
		$lateral_cart = $('#cd-cart'),
		$shadow_layer = $('#cd-shadow-layer');

	//open lateral menu on mobile
	$hamburger_icon.on('click', function(event){
		event.preventDefault();
		//close cart panel (if it's open)
		$lateral_cart.removeClass('speed-in');
		toggle_panel_visibility($menu_navigation, $shadow_layer, $('body'));
	});

	//open cart
	$cart_trigger.on('click', function(event){
		event.preventDefault();
		//close lateral menu (if it's open)
		$menu_navigation.removeClass('speed-in');
		toggle_panel_visibility($lateral_cart, $shadow_layer, $('body'));
		document.documentElement.style.overflow = "auto";
	});
	$('#slidecart').on('click', function (event) {
	    event.preventDefault();

	    //close lateral menu (if it's open)
	    $menu_navigation.removeClass('speed-in');
	    toggle_panel_visibility($lateral_cart, $shadow_layer, $('body'));
	    document.documentElement.style.overflow = "auto";
	});

	$('#cart-btn').on('click', function (event) {
	    event.preventDefault();

	    //close lateral menu (if it's open)
	    if (!$('#cd-cart').hasClass('speed-in')) {
	        //$('#cd-cart').removeClass('speed-in');
	        toggle_panel_visibility($lateral_cart, $shadow_layer, $('body'));
	        document.documentElement.style.overflow = "auto";
	    }
	});

	//close lateral cart or lateral menu
	$shadow_layer.on('click', function(){
		$shadow_layer.removeClass('is-visible');
		// firefox transitions break when parent overflow is changed, so we need to wait for the end of the trasition to give the body an overflow hidden
		if( $lateral_cart.hasClass('speed-in') ) {
			$lateral_cart.removeClass('speed-in').on('webkitTransitionEnd otransitionend oTransitionEnd msTransitionEnd transitionend', function(){
				$('body').removeClass('overflow-hidden');
			});
			$menu_navigation.removeClass('speed-in');
		} else {
			$menu_navigation.removeClass('speed-in').on('webkitTransitionEnd otransitionend oTransitionEnd msTransitionEnd transitionend', function(){
				$('body').removeClass('overflow-hidden');
			});
			$lateral_cart.removeClass('speed-in');
		}
	});

	//move #main-navigation inside header on laptop
	//insert #main-navigation after header on mobile
	move_navigation( $menu_navigation, $L);
	$(window).on('resize', function(){
		move_navigation( $menu_navigation, $L);
		
		if( $(window).width() >= $L && $menu_navigation.hasClass('speed-in')) {
			$menu_navigation.removeClass('speed-in');
			$shadow_layer.removeClass('is-visible');
			$('body').removeClass('overflow-hidden');
		}

	});

	$('#slidecart').click();

	var container = $("#container");
    // Bind the link to toggle the slide.
	$("#viewcart").click(
function (event) {
    // Prevent the default event.
    event.preventDefault();
    if ($lateral_cart.length != 0) {
        $('#slidecart').click(); return false;
    }
    // Toggle the slide based on its current
    // visibility.
    if (container.is(":visible")) {

        // Hide - slide up.
        container.slideUp();

    } else {
        //do Jquery ajax
        var postData = '{}';
        $.ajax({
            url: '/Home/GetCart',
            type: 'POST',
            data: postData,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (xhr) {
                //$("#CartQty").text(xhr.cartitems);
                //$("#CartSum").text(xhr.subtotal);

                var cb = $("#btnCheckout");
                var subtotalDiv = $("#cartsubtotal");
                var biztitle = $("#bizname");
                var items = $("#cartitems");
                var ordertype = $("#ordertype");
                var ordermin = $("#ordermin");
                var details = $("#cartdetails");
                var bizaddress = $("#bizaddress");
                if (xhr.timeexp == "timeout") {
                    subtotalDiv.html("Subtotal: $0.00");
                    $("#inner2").show();
                    $("#inner1").hide();
                }
                else {
                    if (xhr.bizid == 0) {
                        $("#inner2").show();
                        $("#inner1").hide();
                    }
                    else {
                        $("#inner1").show();
                        $("#inner2").hide();

                        $("img#bizimage").attr('src', xhr.bizimg);
                        $("a#bizlink").attr('href', xhr.bizlink);
                        biztitle.html(xhr.biztitle);
                        bizaddress.html(xhr.bizaddress);

                        ordertype.html("<b>Order Type:</b> " + xhr.isDelivery);
                        if (xhr.isbizdelivery === "yes") {

                            if (xhr.isDelivery == "Delivery") {
                                ordermin.html("<b>Delivery Minimum:</b> " + xhr.deliverymin);
                            }
                            else {
                                ordermin.html("");
                            }
                        }
                        else {
                            ordertype.html("<b>Pick up only</b>");
                            ordermin.html("");
                        }
                        items.html("<b>Items in Bag:</b> " + xhr.cartitems);
                        subtotalDiv.html("<b>Subtotal:</b> " + xhr.subtotal);
                        $("a#cartdetails").attr('href', xhr.bizlink);

                    }
                }

                if (xhr.btnshow == "show") {
                    cb.show();
                }
                else {
                    cb.hide();
                }
                container.slideDown();

            },
            error: function (xhr, textStatus, errorThrown) {
                location.href = "/Home";
            }
        });

    }
}
);
	$("#slideClose").click(
function (event) {
    // Prevent the default event.
    event.preventDefault();

    // Toggle the slide based on its current
    // visibility.
    if (container.is(":visible")) {

        // Hide - slide up.
        container.slideUp();

    }
});







});

function toggle_panel_visibility ($lateral_panel, $background_layer, $body) {
	if( $lateral_panel.hasClass('speed-in') ) {
		// firefox transitions break when parent overflow is changed, so we need to wait for the end of the trasition to give the body an overflow hidden
		$lateral_panel.removeClass('speed-in').one('webkitTransitionEnd otransitionend oTransitionEnd msTransitionEnd transitionend', function(){
			$body.removeClass('overflow-hidden');
		});
		$background_layer.removeClass('is-visible');

	} else {
		$lateral_panel.addClass('speed-in').one('webkitTransitionEnd otransitionend oTransitionEnd msTransitionEnd transitionend', function(){
			$body.addClass('overflow-hidden');
		});
		$background_layer.addClass('is-visible');
	}
}

function move_navigation( $navigation, $MQ) {
	if ( $(window).width() >= $MQ ) {
		$navigation.detach();
		$navigation.appendTo('header');
	} else {
		$navigation.detach();
		$navigation.insertAfter('header');
	}
}