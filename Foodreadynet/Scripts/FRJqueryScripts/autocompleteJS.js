$(function () {
    $("input#keyword").autocomplete({
        source: '@Url.Action("KeyWordsList", "Restaurants")',
        minLength: 1,
        select: function (event, ui) {
            keys = ui.item.value;
            if (keys == "Enter search keyword here") {
                keys = "";
            }
            if ($.trim(keys).length > 50) return;
            location.href = "/Restaurants/SearchList" + "?keyword=" + keys;
        }
    });
});