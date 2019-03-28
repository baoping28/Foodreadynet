$(document).ready(function () {
    $("DIV.slideTitle").each(function () {
        $(this).next().slideUp();
    });

    $(".help").each(function () {
        $(this).next().slideUp();
    });

    $(".giftHelp").each(function () {
        $(this).next().slideUp();
    });
    var imgPlus = '../../Content/Images/minusSign.png';
    var imgMinus = '../../Content/Images/PlusSign.png';
    $(".productItem:even").css("background-color", "#f6f6f6");
    $(".productItem:odd").css("background-color", "#ffffff");
    $('.productItem').mouseover(function () { $(this).css('background-color', '#eeeeee') });
    $('.productItem:even').mouseout(function () { $(this).css('background-color', '#f6f6f6') });
    $('.productItem:odd').mouseout(function () { $(this).css('background-color', '#ffffff') });

    $("DIV.ctgTitle").each(function () {
        $(this).bind('click', function () {
            var thisTitle = $(this);
            var thisBody = thisTitle.next();
            var thisImg = thisTitle.children('img');
            thisBody.slideToggle('fast');

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
    $("DIV.slideTitle").each(function () {
        $(this).bind('click', function () {
            var thisTitle = $(this);
            var thisBody = thisTitle.next();
            var thisImg = thisTitle.children('img');
            thisBody.slideToggle('fast');

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
    $(".help").each(function () {
        $(this).bind('click', function () {
            var thisTitle = $(this);
            var thisBody = thisTitle.next();
            var thisImg = thisTitle.children('img');
            thisBody.slideToggle('fast');

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
    $(".giftHelp").each(function () {
        $(this).bind('click', function () {
            var thisTitle = $(this);
            var thisBody = thisTitle.next();
            var thisImg = thisTitle.children('img');
            thisBody.slideToggle('fast');

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
});