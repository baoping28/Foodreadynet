$(function () {
    // Stick the #nav to the top of the window
    var nav = $('#cd-cart');
    var nav1 = $('#menuHeader');
        var $w = $(window);
        $w.scroll(function () {
            var navH = $('#cd-cart');
            var wHeight = $w.height()-50;
            if (navH.height() > wHeight) {
                nav.css({
                    top: '8px'
                });
            }
            else {
                nav.css({
                    top: '26px'
                })
            }

            if ($(this).scrollTop() > 76) {
                nav.css({ top: '8px' })
            } else {
                nav.css({ top: '26px' })
            }

            if ($(this).scrollTop() > nav1.position().top) {
                nav1.css({ top: '58px', width: $('#col-left').width() })
            } else {
                nav1.css({ top: '0px' })
            }
        });

        $(window).resize(function () {
            nav1.css({width: $('#col-left').width() })
        });
});
