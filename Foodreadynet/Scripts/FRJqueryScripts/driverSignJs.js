$(function () {
    $("#driverin").bind('click', function () {
        var trimname = $.trim($("#drivername").val());
        if (trimname == "" || trimname == "Drive name") {
            $("#msg").text("");
            return false;
        }
        var trimpsw = $.trim($("#driverpassword").val());
        if (trimpsw == "" || trimpsw == "Drive password") {
            $("#msg").text("");
            return false;
        }
        //$("DIV#loading").show();
        $(this).attr('disabled', true);
        $('#driverout').attr('disabled', true);
        var postData = '{"vName":"' + trimname + '","vPassword":"' + trimpsw + '"}';
        $.ajax({
            url: '/Driver/DriverIn',
            type: 'POST',
            data: postData,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',

            success: function (xhr) {
                $("#msg").text(xhr.note);
                if (xhr.btn != "lock")
                {
                    $("#driverin").attr('disabled', false);
                }
                $('#driverout').attr('disabled', false);
             },
            error: function (xhr, textStatus, errorThrown) {
                $("#msg").text("Failed sign in");
                $("#driverin").prop('disabled', false);
                $('#driverout').attr('disabled', false);
            }
        });
    });
    $("#driverout").bind('click', function () {
        var trimname = $.trim($("#drivername").val());
        if (trimname == "" || trimname == "Drive name") {
            $("#msg").text("");
            return false;
        }
        var trimpsw = $.trim($("#driverpassword").val());
        if (trimpsw == "" || trimpsw == "Drive password") {
            $("#msg").text("");
            return false;
        }
        //$("DIV#loading").show();
        $(this).attr('disabled', true);
        $('#driverin').attr('disabled', true);
        var postData = '{"vName":"' + trimname + '","vPassword":"' + trimpsw + '"}';
        $.ajax({
            url: '/Driver/DriverOut',
            type: 'POST',
            data: postData,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',

            success: function (xhr) {
                $("#msg").text(xhr.note);
                if (xhr.btn != "lock") {
                    $("#driverout").attr('disabled', false);
                }
                $('#driverin').attr('disabled', false);
            },
            error: function (xhr, textStatus, errorThrown) {
                $("#msg").text("Failed sign out");
                $("#driverout").prop('disabled', false);
                $('#driverin').attr('disabled', false);
            }
        });
    });
});