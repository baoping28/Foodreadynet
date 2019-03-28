$(function () {
    var moveLeft = -140;
    var moveDown = -300;

    $('DIV.populink').each(function () {
        $(this).hover(function (e) {
            $(this).next().show();
            //.css('top', e.pageY + moveDown)
            //.css('left', e.pageX + moveLeft)
            //.appendTo('body');
        }, function () {
            $(this).next().hide();
        });
    });

    $('DIV.populink').each(function () {
        $(this).mousemove(function (e) {
            $(this).next().css('top', e.pageY + moveDown).css('left', e.pageX + moveLeft);
        });
    });

    $('#popupAddClose').click(function (e) {
        $("#divAddPopup").trigger('close');
    });
});