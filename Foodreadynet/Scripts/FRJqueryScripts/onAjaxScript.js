function getDirections(maplink) {
    var winName = "BizLocation";
    var ShelpWindow = window.open(maplink, winName, 'scrollbars=yes,width=1000,height=600 left=100 top=100 resizable=yes');
}

var onAjaxBegin = function () {
    $("Div#loading").show();
    $("#filterBody").animate({ opacity: 0.1 }, "fast");
};
var onAjaxSuccess = function () {
    $("DIV#loading").hide();
    $("#filterBody").animate({ opacity: 1.0 }, "fast");
};
var onAjaxCompleted = function () {
    $("DIV#loading").hide();
    $("#filterBody").animate({ opacity: 1.0 }, "fast");
};
var onAjaxFailed = function () {
    $("DIV#loading").hide();
    $("#filterBody").animate({ opacity: 1.0 }, "fast");
};
var onNewBegin = function () {
    $("Div#loading").show();
    $("#filterBody").animate({ opacity: 0.1 }, "fast");
};
var onNewSuccess = function () {
    $("DIV#loading").hide();
    $("#filterBody").animate({ opacity: 1.0 }, "fast");
};
var onNewCompleted = function () {
    $("DIV#loading").hide();
    $("#filterBody").animate({ opacity: 1.0 }, "fast");
};
var onNewFailed = function () {
    $("DIV#loading").hide();
    $("#filterBody").animate({ opacity: 1.0 }, "fast");
};



var onAddBegin = function () {
   // $("Div#loading").show();
    //$("#navFloating").animate({ opacity: 0.1 }, "fast");
};
var onAddSuccess = function () {
    //$("DIV#loading").hide();
    $("#divAddPopup").popup_me({ centered: true });
};
var onAddCompleted = function () {
    //$("DIV#loading").hide();
    $("#divAddPopup").popup_me({ centered: true });
};
var onAddFailed = function () {
   // $("DIV#loading").hide();
    // $("#navFloating").animate({ opacity: 1.0 }, "fast");
    location.href = "/Home";
};

var onWinBegin = function () {
    // $("Div#loading").show();
    $("#winPopup").trigger('close');
};
var onWinSuccess = function () {
    //$("DIV#loading").hide();
    $("#winPopup").popup_me({ centered: true });
};
var onWinCompleted = function () {
    //$("DIV#loading").hide();
    $("#winPopup").popup_me({ centered: true });
};
var onWinFailed = function () {
    // $("DIV#loading").hide();
    // $("#navFloating").animate({ opacity: 1.0 }, "fast");
    location.href = "/Home";
};

var onRv2Begin = function () {
    // $("Div#loading").show();
    //$("#navFloating").animate({ opacity: 0.1 }, "fast");
};
var onRv2Success = function () {
    //$("DIV#loading").hide();
    $("#rvPopup").popup_me({ centered: true });
};
var onRv2Completed = function () {
    //$("DIV#loading").hide();
    $("#rvPopup").popup_me({ centered: true });
};
var onRv2Failed = function () {
    // $("DIV#loading").hide();
    // $("#navFloating").animate({ opacity: 1.0 }, "fast");
    location.href = "/Home";
};

var onAddedBegin = function () {
    // $("Div#loading").show();
    //$("#navFloating").animate({ opacity: 0.1 }, "fast");
    $("#divAddPopup").trigger('close');
};
var onAddedSuccess = function () {
    //$("DIV#loading").hide();
    $("#divAddPopup").trigger('close');
};
var onAddedCompleted = function () {
    //$("DIV#loading").hide();
    $("#divAddPopup").trigger('close');
};
var onAddedFailed = function () {
    // $("DIV#loading").hide();
    $("#divAddPopup").trigger('close');
    location.href = "/Home";
};


var onHotelBegin = function () {
    // $("Div#loading").show();
    //$("#navFloating").animate({ opacity: 0.1 }, "fast");
    $("#hotelPopup").trigger('close');
};
var onHotelSuccess = function () {
    //$("DIV#loading").hide();
    $("#hotelPopup").popup_me({ centered: true });
};
var onHotelCompleted = function () {
    //$("DIV#loading").hide();
    $("#hotelPopup").popup_me({ centered: true });
};
var onHotelFailed = function () {
    // $("DIV#loading").hide();
    $("#hotelPopup").trigger('close');
};

var onSubMenuBegin = function () {
    // $("Div#loading").show();
    $("#menucontent").animate({ opacity: 0.1 }, "fast"); ;
};
var onSubMenuSuccess = function () {
    //$("DIV#loading").hide();
    $("#menucontent").animate({ opacity: 1.0 }, "fast");
};
var onSubMenuCompleted = function (link) {
    //$("DIV#loading").hide();
    $("#menucontent").animate({ opacity: 1.0 }, "fast");
    $("#menuHeader ul li a").each(function () {
        $(this).removeClass('selected');
    });
    $("#" + link).addClass('selected');
};
var onSubMenuFailed = function () {
    // $("DIV#loading").hide();
    $("#menucontent").animate({ opacity: 1.0 }, "fast");
};

var onRemoveBegin = function () {
    $("#CartContent").animate({ opacity: 0.3 }, "slow");
};
var onRemoveSuccess = function () {
    $("#bagitems").text($("#hitems").val());
    $("#CartContent").show("slow").animate({ opacity: 1.0 }, "slow");
};
var onRemoveCompleted = function () {

    $("#bagitems").text($("#hitems").val());
    $("#CartContent").animate({ opacity: 1.0 }, "slow");

};
var onRemoveFailed = function () {

};

var onCheckBegin = function () {
    // $("Div#loading").show();
    //$("#navFloating").animate({ opacity: 0.1 }, "fast");
};
var onCheckSuccess = function () {
    //$("DIV#loading").hide();
    $("#divCheckPopup").popup_me({ centered: true });
};
var onCheckCompleted = function () {
    //$("DIV#loading").hide();
    //$("#divCheckPopup").popup_me({ centered: true });
};
var onCheckFailed = function () {
    // $("DIV#loading").hide();
    // $("#navFloating").animate({ opacity: 1.0 }, "fast");
    location.href = "/Home";
};


var onJoinBegin = function () {
    $("#joinForm").animate({ opacity: 0.3 }, "slow");
    //$("#navFloating").animate({ opacity: 0.1 }, "fast");
};
var onJoinSuccess = function () {
    //$("DIV#loading").hide();
    $("#joinForm").animate({ opacity: 1.0 }, "fast");
};
var onJoinCompleted = function () {
    //$("DIV#loading").hide();
    $("#joinForm").animate({ opacity: 1.0 }, "fast");
};
var onJoinFailed = function () {
    // $("DIV#loading").hide();
    $("#joinForm").animate({ opacity: 1.0 }, "fast");
};

var onChatBegin = function () {
    $("#chatNameCheck").html("");
    $("#chatEmailCheck").html("");
    if ($("#chatUsername").val() == "") {
        $("#chatNameCheck").html("Please enter your name");
        return false;
    }
    else if ($("#chatEmail").val() == "") {
        $("#chatEmailCheck").html("Please enter your email address");
        return false;
    }
};
var onChatSuccess = function () {
   
};
var onChatCompleted = function () {
   
};
var onChatFailed = function () {

};


var onLoadBegin = function () {
    $("#divAddPopup").trigger('close');
};
var onLoadSuccess = function () {
    $("#divAddPopup").popup_me({ centered: true });
};
var onLoadCompleted = function () {
    $("#divAddPopup").popup_me({ centered: true });
};
var onLoadFailed = function () {
    location.href = "/Home";
};

var onRvBegin = function () {
    $("#rvPopup").trigger('close');
};
var onRvSuccess = function () {
    $("#rvPopup").popup_me({ centered: true });
};
var onRvCompleted = function () {
    $("#rvPopup").popup_me({ centered: true });
};
var onRvFailed = function () {
    location.href = "/Home";
};

var onGiftBegin = function () {
    $("Div#GCloading").show();
};
var onGiftSuccess = function () {
    $("DIV#GCloading").hide();
};
var onGiftCompleted = function () {
    $("DIV#GCloading").hide();
};
var onGiftFailed = function () {
    $("DIV#GCloading").hide();
};

var onOrderBegin = function () {
    // $("Div#loading").show();
    //$("#navFloating").animate({ opacity: 0.1 }, "fast");
};
var onOrderSuccess = function () {
    //$("DIV#loading").hide();
    $("#divOrderPopup").popup_me({ centered: true });
};
var onOrderCompleted = function () {
    //$("DIV#loading").hide();
    $("#divOrderPopup").popup_me({ centered: true });
};
var onOrderFailed = function () {
    // $("DIV#loading").hide();
    // $("#navFloating").animate({ opacity: 1.0 }, "fast");
    location.href = "/Home";
}
var onReviewBegin = function (item) {
    var divp = "#item" + item;
    $(divp).animate({ opacity: 0.5 }, "slow");
};
var onReviewSuccess = function (item) {
    var divp = "#item" + item;
    $(divp).animate({ opacity: 1.0 }, "slow");
};
var onReviewFailure = function (item) {
    var divp = "#item" + item;
    $(divp).animate({ opacity: 1.0 }, "slow");
};

var onShareBegin = function () {
    
};
var onShareSuccess = function () {
    $("#CartContent").show("slow").animate({ opacity: 1.0 }, "slow");
};
var onShareCompleted = function () {

    $("#CartContent").animate({ opacity: 1.0 }, "slow");

};
var onShareFailed = function () {

};

var onFreshBegin = function () {
};
var onFreshSuccess = function () {
    $("DIV#otherscart").slideDown();
};
var onFreshCompleted = function () {

    $("DIV#otherscart").slideDown();

};
var onFreshFailed = function () {

};

var onFreshbossBegin = function () {
};
var onFreshbossSuccess = function () {
    $("DIV#otherscart").slideDown();
};
var onFreshbossCompleted = function () {

    $("DIV#otherscart").slideDown();

};
var onFreshbossFailed = function () {

};

var onChangeBegin = function () {
    
};
var onChangeSuccess = function () {
    $("#divAddPopup").trigger('close');
};
var onChangeCompleted = function () {
    $("#divAddPopup").trigger('close');
};
var onChangeFailed = function () {
};

var onDriverBegin = function (item) {
};
var onDriverSuccess = function (item) {
    var divp = "#item" + item;
    $(divp).text("Done!");
};
var onDriverFailure = function (item) {
};



var onMissedBegin = function () {

};
var onMissedSuccess = function () {
    $("#missed-count").text($("#getcount").val());
};
var onMissedCompleted = function () {

};
var onMissedFailed = function () {
};