$(document).ready(function () {
    // find the "sameAsShipping" checkbox and listen for the change event
    $('#sameAsShipping').bind('change', function () {
        if (this.checked) {
            // find all text inputs inside billingInfo, disable them, then cycle through each one
            $('#paymentInfo input:text').not('input#CardNumber').not('input#SecurityCode').each(function (i) {
                // find the shipping input that corresponds to this billing input
                var valueFromShippingInput =
$('#shippingInfo input:text:eq(' + i + ')').val();
                // set the billing value with the shipping text value
                $(this).val(valueFromShippingInput);
            }); // close each()
        } else {
            $('#paymentInfo input:text').not('input#CardNumber').not('input#SecurityCode').each(function () {
                // find the shipping input that corresponds to this billing input
                $(this).val("");
            }); // close each()
        }
    }).trigger('change'); // close change() then trigger it

    $('#mSameAsShipping').bind('change', function () {
        if (this.checked) {
            // find all text inputs inside billingInfo, disable them, then cycle through each one
            $('#paymentInfo input:text').not('input#GCCardNumber').not('input#GCSecurityCode').each(function (i) {
                // find the shipping input that corresponds to this billing input
                var valueFromShippingInput =
$('#GCshippingInfo input:text:eq(' + i + ')').val();
                // set the billing value with the shipping text value
                $(this).val(valueFromShippingInput);
            }); // close each()
        } else {
            $('#paymentInfo input:text').not('input#GCCardNumber').not('input#GCSecurityCode').each(function () {
                // find the shipping input that corresponds to this billing input
                $(this).val("");
            }); // close each()
        }
    }).trigger('change'); // close change() then trigger it

    $('#sameAsLStart').bind('click', function () {
        if (this.checked) {
            var valuelMondayStart = $('select#LMonStart').val();
            $('select.lstart').each(function () {
                // set the billing value with the shipping text value
                $(this).val(valuelMondayStart);
            }); // close each()
        } else {
            $('select.lstart').each(function () {
                // find the shipping input that corresponds to this billing input
                $(this).val("");
            }); // close each()
        }
    }).trigger('change'); // close change() then trigger it


    $('input#gcsameAsShipping').bind('change', function () {
        if (this.checked) {
            // find all text inputs inside billingInfo, disable them, then cycle through each one
            $('#GCpaymentInfo input:text').not('input#GCCardNumber').not('input#GCSecurityCode').each(function (i) {
                // find the shipping input that corresponds to this billing input
                var valueFromShippingInput =
$('#GCshippingInfo input:text:eq(' + i + ')').val();
                // set the billing value with the shipping text value
                $(this).val(valueFromShippingInput);
            }); // close each()
        } else {
            $('#GCpaymentInfo input:text').not('input#GCCardNumber').not('input#GCSecurityCode').each(function () {
                // find the shipping input that corresponds to this billing input
                $(this).val("");
            }); // close each()
        }
    }).trigger('change'); // close change() then trigger it




    $('#sameAsLClose').bind('click', function () {
        if (this.checked) {
            var valuelMondayClose = $('select#LMonClose').val();
            $('select.lclose').each(function () {
                // set the billing value with the shipping text value
                $(this).val(valuelMondayClose);
            }); // close each()
        } else {
            $('select.lclose').each(function () {
                // find the shipping input that corresponds to this billing input
                $(this).val("");
            }); // close each()
        }
    }).trigger('change'); // close change() then trigger it

    $('#sameAsDStart').bind('click', function () {
        if (this.checked) {
            var valueMondayStart = $('select#MonStart').val();
            $('select.dstart').each(function () {
                // set the billing value with the shipping text value
                $(this).val(valueMondayStart);
            }); // close each()
        } else {
            $('select.dstart').each(function () {
                // find the shipping input that corresponds to this billing input
                $(this).val("");
            }); // close each()
        }
    }).trigger('change'); // close change() then trigger it

    $('#sameAsDClose').bind('click', function () {
        if (this.checked) {
            var valueMondayClose = $('select#MonClose').val();
            $('select.dclose').each(function () {
                // set the billing value with the shipping text value
                $(this).val(valueMondayClose);
            }); // close each()
        } else {
            $('select.dclose').each(function () {
                // find the shipping input that corresponds to this billing input
                $(this).val("");
            }); // close each();
        }
    }).trigger('change'); // close change() then trigger it
});