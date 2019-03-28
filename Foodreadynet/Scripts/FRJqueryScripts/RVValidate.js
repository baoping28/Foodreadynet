$(document).ready(function () {
    var messageBox = $("#Message");
    messageBox.val("Please note that not all requests can be accommodated.");
    messageBox.keydown(function () {
        if (messageBox.val() == this.title) {
            messageBox.val("");
        }
    });
    messageBox.focus(function () {
        messageBox.removeClass("defaultZipText");
        messageBox.addClass("onFocusZipText");
    });
    messageBox.blur(function () {
        messageBox.removeClass("onFocusZipText");
        messageBox.addClass("defaultZipText");
        if ($.trim(messageBox.val()) == "") {
            messageBox.val(this.title);
        }
    });
    $("#rvSubmit").bind("click", function () {
        var mv = messageBox.val();
        if (mv == "Please note that not all requests can be accommodated.") {
            messageBox.val("");
        }
    });
    $("#rvform").validate({
        rules: {
            FirstName: {
                required: true,
                minlength: 2
            },
            LastName: {
                required: true,
                minlength: 2
            },
            Phone: {
                required: true,
                digits: true,
                minlength: 10
            },
            Email: {
                required: true,
                email: true
            },
            messages: {
                FirstName: {
                    required: "Please enter your first name.",
                    minlength: "You must enter 2 or more letters."
                },
                LastName: {
                    required: "Please enter your last name.",
                    minlength: "You must enter 2 or more letters."
                },
                Phone: {
                    required: "Please enter a  phone number",
                    digits: "Please use only numbers.",
                    minlength: "You must enter 10 or more digits."
                },
                Email: {
                    required: "Please enter your email address.",
                    email: "Please enter a valid email addresse."
                }
            }
        }
    });
});