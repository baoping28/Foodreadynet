$(document).ready(function () {
    // Get a reference to the container.
    var container = $("#container");

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
    $("#slide1Close").click(
function (event) {
    // Prevent the default event.
    event.preventDefault();
    var container = $("#container");
    // Toggle the slide based on its current
    // visibility.
    if (container.is(":visible")) {

        // Hide - slide up.
        container.slideUp();

    }
});
    $('.navdropdown').hover(function () {
        // When the event is triggered, grab the current element 'this' and
        // find it's children '.sub_navigation' and display/hide them
        $(this).find('.main_nvg').slideToggle();
    });

    $("#r-discount").click(
function (event) {
    // Prevent the default event.
    event.preventDefault();
    $("#r-offer").click();
});
});