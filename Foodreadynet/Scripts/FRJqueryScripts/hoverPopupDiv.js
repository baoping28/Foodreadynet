$(function () {
    var moveLeft = 20;
    var moveDown = 10;
    $('div#usage').next().slideUp();
   // $('input#gdialogue').next().slideUp();
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

    $('DIV.popupdiv').each(function () {
        $(this).click(function (e) {
            $(this).hide();
        });
    });

   // $('DIV.populink').each(function () {
    //    $(this).mousemove(function (e) {
    //        $(this).next().css('top', e.pageY + moveDown).css('left', e.pageX + moveLeft).css('position', 'absolute');
    //    });
    //});

    $('#popuprvClose').click(function (e) {
        $("#rvPopup").trigger('close');
    });

    $('div#usage').bind('click', function () {
        var thisBody = $(this).next();
        thisBody.slideToggle();
    });
   // $('input#gdialogue').bind('click', function () {
   //     var thisBody = $(this).next();
    //    thisBody.slideToggle();
    // });
    var container = $("#chatcontainer");
    $("input#gdialog").click(function (event) {
    // Prevent the default event.
    event.preventDefault();

    // Toggle the slide based on its current
    // visibility.
    if (container.is(":visible")) {

        // Hide - slide up.
        container.slideUp();

    }
    else {
        container.slideDown();
    }

    });
    $("input#closechat").click(function (event) {
        // Prevent the default event.
        event.preventDefault();
        $("#chatcontainer").slideUp();

    });
});