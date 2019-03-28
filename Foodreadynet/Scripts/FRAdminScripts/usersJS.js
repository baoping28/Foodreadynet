$(document).ready(function () {

    $("input#autocomplete").autocomplete({
        source: "/Admin/UserNames",
        minLength: 1,
        select: function (event, ui) {
            var postItem = ui.item.value;
            var postData = '{"userSearch":"' + postItem + '"}';
            $.ajax({
                url: '/Admin/GetUser',
                type: 'POST',
                data: postData,
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function (obj) {
                    $("#sUserName").text(obj.uUserName);
                    $("#sCreateDate").text(obj.uCreateDate);
                    $("#sChangedDate").text(obj.uChangedDate);
                    $("#sEmailConfirmed").text(obj.uEmailConfirmed);
                    $("#sLockoutEnabled").text(obj.uLockoutEnabled);
                    $("#sRoles").text(obj.uRoles);
                    $("#username").val(obj.uUserName);
                    $("#trUserInfo").css('background-color', 'gray').show("slow");
                    // location.href = "/Book/ProductDetails/2";
                }
            });
        }
    });
});