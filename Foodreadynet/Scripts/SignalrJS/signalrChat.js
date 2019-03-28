$(function () {
    // Reference the auto-generated proxy for the hub.
    var chat = $.connection.chatHub;
    // Create a function that the hub can call back to display messages.Note: In JavaScript the reference to the server class and its members is in camel case.
    chat.client.chatMessageToPage = function (name, message) {
        // Add the message to the page.
        $('#discussion').prepend('<li><strong>' + htmlEncode(name)  + '</strong>: ' + htmlEncode(message) + '</li>'); 
    };
    chat.client.addNewMessageToPage = function (name, message) {
        // Add the message to the page.
        $('#adminpannel').prepend('<li>' + htmlEncode(message) + '</li>');
    };
    // Set initial focus to message input box.
    $('#message').focus();
    // Start the connection.
    $.connection.hub.start().done(function () {
        $('#sendmessage').click(function () {
            // Call the Send method on the hub.
            chat.server.sendChatMessage($('#displayname').val(), $('#message').val());
            // Clear text box and reset focus for next comment.
            $('#message').val('').focus();
        });
        $('#discussion').prepend('<li>Hi ' + $('#displayname').val() + ', just a moument, we will be right with you!</li>');
        var str = "Name: " + $('#displayname').val() + " Email: " + $('#displayemail').val() + " Question: " + $('#displayquestion').val();
        chat.server.send($('#displayadmin').val(), str);
    });
});
// This optional function html-encodes messages for display in the page.
function htmlEncode(value) {
    var encodedValue = $('<div />').text(value).html();
    return encodedValue;
}