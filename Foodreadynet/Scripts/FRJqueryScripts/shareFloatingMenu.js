$(function () {
    // Stick the #nav to the top of the window
    var nav = $('#wrap');
    var $w = $(window);
    $w.scroll(function () {
        var navH = $('#wrap');
        var wHeight = $w.height();
        if (navH.height() > wHeight) {
            nav.css({
                position: 'static',
                width: '450px',
                zIndex: '1'
            });
        }
        else {
            nav.css({
                position: 'fixed',
                width: '450px',
                zIndex: '1'
            })
        }
    });
});
