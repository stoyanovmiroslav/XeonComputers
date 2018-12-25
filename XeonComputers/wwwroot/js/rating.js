$.fn.stars = function () {
    return $(this).each(function () {
        var val = parseFloat($(this).html());
        var size = Math.max(0, (Math.min(5, val))) * 16;
        var $span = $('<span />').width(size);
        $(this).html($span);
    });
}
$(function () {
    $('span.stars').stars();
});