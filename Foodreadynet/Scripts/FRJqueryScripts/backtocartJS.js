$(document).ready(function () {
    $('img#backtoshare').bind('click', function () {
        var postData = '{}';
        $.ajax({
            url: '/Share/BackToCart',
            type: 'POST',
            data: postData,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (xhr) {
                if (xhr.timeexp == "timeout") {
                    location.href = "/Home";
                }
                $("#divAddPopup").trigger('close');
                $("#locktitle").hide();
                // setCheckoutNowBtn(xhr.isDelivery, xhr.delMin);
            },
            error: function (xhr, textStatus, errorThrown) {
                $("#divAddPopup").trigger('close');
            }
        });
    });
});