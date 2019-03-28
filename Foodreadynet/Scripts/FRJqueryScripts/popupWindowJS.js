
$(function () {
    $('#btnHow').click(function (e) {
        $("#divPopup").popup_me({ centered: true });
        e.preventDefault();
    });
    $('#popupAddClose').click(function (e) {
        $("#divAddPopup").trigger('close').slow();
    });

    $('#popupClose').click(function (e) {
        $("#divPopup").trigger('close').slow();
    });

    $('img#chat').click(function (e) {
        $("#divChatPopup").popup_me({ centered: true });
    });
    $('#popupChatClose').click(function (e) {
        $("#divChatPopup").trigger('close');
    });
    $('#contactChat').click(function (e) {
        $("#divChatPopup").popup_me({ centered: true });
    });
    $('#chatwithus').bind('click', function () {
        $("#divChatPopup").popup_me({ centered: true });
        return false;
    });

    $('a#synchronous').click(function (e) {
        $("#partyPopup").popup_me({ centered: true });
    });
    /*
    $('a#combined').click(function (e) {
        $("#partyPopup").popup_me({ centered: true });
    });
    */
    $('a#grouplink').click(function (e) {
        $("#partyPopup").popup_me({ centered: true });
    });
    $('#partyClose').click(function (e) {
        $("#partyPopup").trigger('close');
    });

    $('#winClose').click(function (e) {
        $("#winPopup").trigger('close');
    });
    $('#hotelClose').click(function (e) {
        $("#hotelPopup").trigger('close');
    });
    $('img#sharedChatBtn').click(function (e) {
        $("#sendkeyPopup").popup_me({ centered: true });
    });
    $('#sendkeyClose').click(function (e) {
        $("#sendkeyPopup").trigger('close');
    });
});