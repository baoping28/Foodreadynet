$(document).ready(function () {
    var chat = $.connection.chatHub;
    // Start the connection.
    $.connection.hub.start(function () {
    }).done(function () {
        var un = $("#p-username").val();
        var bt = $("#p-biztitle").val();
        var oi = $("#p-orderid").val();
        var ol = $("#p-orderlist").val();
        chat.server.sendOrder(un, bt, oi, ol);
    });
});