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
            $('#Description').val("");
            $('#Description').focus();
            update();
        });
        $('#btnClearFt').bind('click', function () {
            $('#Facilities').val("");
            $('#Facilities').focus();
            update();
        });
        $('#btnClearEv').bind('click', function () {
            $('#Events').val("");
            $('#Events').focus();
            update();
        });
        $('#btnClearPk').bind('click', function () {
            $('#Parking').val("");
            $('#Parking').focus();
            update();
        });
        $('#btnClearDt').bind('click', function () {
            $('#Details').val("");
            $('#Details').focus();
            update();
        });
    }); // close .each()
});