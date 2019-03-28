$(function () {
    var pusher = new window.Pusher('53760f442521f2983fd7');
    var channel = pusher.subscribe('sharechat_channel');
    channel.bind('sharemessage_received', function (data) {
        var senderid = data.sendcartid;
        var myid = $('#sharemessages').attr('sharecartid');
        if (senderid==myid) {
        $("#sharemessages").prepend('<li>' + data.timestamp + "/" + data.user + '>  ' + data.message + '</li>');
        }
    });

    $('#sharechatSubmitMessage').bind('click', function () {
        $.post("/Share/ShareChat", { talkMessage: $('#sharechatMessage').val(),
            talkUsername: $('#sharemessages').attr('shareitemid'),
            senderCartCode: $('#sharemessages').attr('sharecartid')
        });
    });
});