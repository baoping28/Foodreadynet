function updateClock() {
    var bizid = $("#biz-id").val();
        var postData = '{"id":"' + bizid + '"}';
        $.ajax({
            url: '/Restaurants/GetBizTime',
            type: 'POST',
            data: postData,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (xhr) {
                $("#clock").html(xhr.result);
            }
        });
}
$(document).ready(function () {
    setInterval('updateClock()', 1000);
});