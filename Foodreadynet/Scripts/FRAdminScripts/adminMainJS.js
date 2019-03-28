
var onDoApproveBegin = function (item) {
    var divp = "#item" + item;
    $(divp).animate({ opacity: 0.5 }, "slow");
};
var onDoApproveSuccess = function (item) {
    var divp = "#item" + item;
    $(divp).animate({ opacity: 1.0 }, "slow");
};
var onDoApproveFailure = function (item) {
    var divp = "#item" + item;
    $(divp).animate({ opacity: 1.0 }, "slow");
};
var onFreeItemBegin = function (item) {
    var divp = "#item" + item;
    $(divp).animate({ opacity: 0.5 }, "slow");
};
var onFreeItemSuccess = function (item) {
    var divp = "#item" + item;
    $(divp).animate({ opacity: 1.0 }, "slow");
};
var onFreeItemFailure = function (item) {
    var divp = "#item" + item;
    $(divp).animate({ opacity: 1.0 }, "slow");
};