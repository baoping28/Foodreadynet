$(document).ready(function () {

    $("DIV#roomnumer").next().slideUp();
    $("DIV#pproomnumer").next().slideUp();

    $("DIV.ctgTitle").each(function () {
        $(this).bind('click', function () {
            var thisTitle = $(this);
            var thisBody = thisTitle.next();
            thisBody.slideToggle('slow');
        });
    });
    
    $("DIV#roomnumer").bind('click', function () {
        var thisTitle = $(this);
        var thisBody = thisTitle.next();
        thisBody.slideToggle('slow');
    });
    $("DIV#pproomnumer").bind('click', function () {
        var thisTitle = $(this);
        var thisBody = thisTitle.next();
        thisBody.slideToggle('slow');
    });
});