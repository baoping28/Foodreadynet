$(document).ready(function () {

    $("DIV#gcroomnumer").next().slideUp();
    $("DIV#gcroomnumer").bind('click', function () {
        var thisTitle = $(this);
        var thisBody = thisTitle.next();
        thisBody.slideToggle('slow');
    });
});