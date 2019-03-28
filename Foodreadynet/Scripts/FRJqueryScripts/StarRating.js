$(document).ready(function () {
    $("#txtPenname").focus();

    $("#txtPenname").bind('focus', function () {
        $("#txtPenname").next().hide();
    });
    $("#txtPenname").bind('blur', function () {
        if ($.trim($("#txtPenname").val()).length == 0) {
            $("#txtPenname").next().text('*').css('color', 'red').show();
        }
        else {
            $("#txtPenname").next().hide();
        }
    });

    $("#txtTitle").bind('focus', function () {
        $("#txtTitle").next().hide();
    });
    $("#txtTitle").bind('blur', function () {
        if ($("#txtTitle").val().length == 0) {
            $("#txtTitle").next().text('*').css('color', 'red').show();
        }
        else {
            $("#txtTitle").next().hide();
        }
    });

    $("#txtaBody").bind('focus', function () {
        $("#areatext").hide();
    });
    $("#txtaBody").bind('blur', function () {
        if ($.trim($("#txtaBody").val()).length == 0) {
            $("#areatext").text('*').css('color', 'red').show();
        }
        else {
            $("#areatext").hide();
        }
    });

    $(".star").mouseover(function () {
        var span = $(this).parent("span");
        var newRating = $(this).attr("value");
        setRating1(span, newRating);
    });
    $(".star").mouseout(function () {
        var span = $(this).parent("span");
        var rating = span.attr("rating");
        setRating2(span, rating);
    });
    function setRating1(span, rating) {
        span.find('img').each(function () {
            var value = parseFloat($(this).attr("value"));
            var imgSrc = $(this).attr("src");
            if (value <= rating) {
                $(this).attr("src", imgSrc.replace("ratingEmpty.png", "ratingStar.png").replace("ratingHalf.png", "ratingStar.png"));
            }
            else {
                $(this).attr("src", imgSrc.replace("ratingStar.png", "ratingEmpty.png").replace("ratingHalf.png", "ratingEmpty.png"));
            }
        });
    }
    function setRating2(span, rating) {
        span.find('img').each(function () {
            var value = parseFloat($(this).attr("value"));
            var imgSrc = $(this).attr("src");

            var diff = value - rating;
            if (value <= rating) {
                $(this).attr("src", imgSrc.replace("ratingEmpty.png", "ratingStar.png").replace("ratingHalf.png", "ratingStar.png"));
            }
            else if (diff > 0.25 && diff < 1.0) {
                $(this).attr("src", imgSrc.replace("ratingStar.png", "ratingHalf.png").replace("ratingEmpty.png", "ratingHalf.png"));
            }
            else if (diff > 0.0 && diff <= 0.25) {
                $(this).attr("src", imgSrc.replace("ratingEmpty.png", "ratingStar.png").replace("ratingHalf.png", "ratingStar.png"));
            }
            else {

                $(this).attr("src", imgSrc.replace("ratingStar.png", "ratingEmpty.png").replace("ratingHalf.png", "ratingEmpty.png"));
            }
        });
    }

    $(".star").click(function () {
        var span = $(this).parent("span");
        var newRating = $(this).attr("value");
        var postID = span.attr("post");
        var spanText = span.children("span");
        var penname = $.trim($("#txtPenname").val());
        var title = $.trim($("#txtTitle").val());
        var comment = $.trim($("#txtaBody").val());

        var pass = true;
        if (penname.length <= 0) {
            $("#txtPenname").next().text('*').css('color', 'red').show();
            pass = false;
        }

        if (title.length <= 0) {
            $("#txtTitle").next().text('*').css('color', 'red').show();
            pass = false;
        }
        if (comment.length <= 0) {
            $("#areatext").text('*').css('color', 'red').show();
            pass = false;
        }
        if (!pass) { return false; }
        var postData = '{"penname":"' + penname + '","title":"' + title + '","id":"' + postID + '","comment":"' + comment + '","rating":"' + newRating + '"}';
        if (confirm("You are rating the restaurant " + newRating + " stars. Do you want to cast your vote ?")) {
            $.ajax({
                url: '/Reviews/SetRating',
                type: 'POST',
                data: postData,
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function (obj) {
                    spanText.text(""); //modify the text
                    // span.attr("rating", obj.Result.Rating); //set the rating attribute
                    // setRating1(span, obj.Result.Rating); //update the display
                    $("#result").text(obj.Message).show("slow");
                }
            });
        }
    });

});
