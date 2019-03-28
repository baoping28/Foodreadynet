$(function () {
    // Stick the #nav to the top of the window
    var nav = $('#snavFloating');
    var wp = $('#swrap');
    var cc = $('#menucontent');
    var navHomeY = nav.offset().top;
    var isFixed = false;
    var $w = $(window);
    $w.scroll(function () {
        var scrollTop = $w.scrollTop();
        var shouldBeFixed = scrollTop > navHomeY;
        var navH = $('#snavFloating');
        var wHeight = $w.height();
        if (navH.height() > wHeight) {
            nav.css({
                position: 'absolute',
                top: 122,
                left: nav.offset().left,
                width: nav.width(),
                zIndex: 8888,
                overflowY: 'visible'
            });
        }
        else if (shouldBeFixed && !isFixed) {
            cc.css({
                height: wp.height() + 100

            });
            nav.css({
                position: 'fixed',
                top: 0,
                left: nav.offset().left,
                width: nav.width(),
                overflowY: 'visible'
            });
            isFixed = true;
        }
        else if (!shouldBeFixed && isFixed) {
            nav.css({
                position: 'static'
            });
            isFixed = false;
        }
    });


    $(window).resize(function () {
        // alert(wp.scrollLeft());
        var navH = $('#snavFloating');
        var $ww = ($(window).width() - 960) / 2;
        navH.css({
            left: $ww + cc.width() + 8
        });
    });


});
