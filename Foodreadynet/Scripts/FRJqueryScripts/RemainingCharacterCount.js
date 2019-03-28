$(document).ready(function () {

    $('.remaining').each(function () {
        // find and store the count readout and the related textarea/input field
        var $count = $('.count', this);
        var $input = $(this).prev();
        // .text() returns a string, multiply by 1 to make it a number (for math)
        var maximumCount = $count.text() * 1;
        // update function is called on keyup, paste and input events
        var update = function () {
            var before = $count.text() * 1;
            var now = maximumCount - $input.val().length;
            // check to make sure users haven't exceeded their limit
            if (now < 0) {
                var str = $input.val();
                $input.val(str.substr(0, maximumCount));
                now = 0;
            }
            // only alter the DOM if necessary
            if (before != now) {
                $count.text(now);
            }
        };
        // listen for change (see discussion below)
        $input.bind('input keyup paste', function () { setTimeout(update, 0) });
        // call update initially, in case input is pre-filled
        update();

        $('#btnClear').bind('click', function () {
            $('#Instructions').val("");
            $('#Instructions').focus();
            update();
        });
        $('#reviewClear').bind('click', function () {
            $('#txtaBody').val("");
            $('#txtaBody').focus();
            update();
        });
        $('#messageClear').bind('click', function () {
            $('#Message').val("");
            $('#Message').focus();
            update();
        });
        $('#btnGiftClear').bind('click', function () {
            $('#GCInstructions').val("");
            $('#GCInstructions').focus();
            update();
        });
        $('#btnMsgClear').bind('click', function () {
            $('#Message').val("");
            $('#Message').focus();
            update();
        });
    }); // close .each()

    $("input:checkbox[class=cbtopping]").each(function () {
        $(this).bind('click', function () {
            var mn = $("input#maxnum").val();
            var mnt = parseInt(mn);
            var leng = $("input:checkbox[class=cbtopping]:checked").length;
            if (leng >= mnt) {

                $("input:checkbox[class=cbtopping]").each(function () {
                    if (!($(this).is(':checked'))) {
                        $(this).attr("disabled", true);
                    }
                });
            }
            else {
                $("input:checkbox[class=cbtopping]").each(function () {
                    $(this).attr("disabled", false);
                });
            }
        });

    });
});