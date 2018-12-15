$("#Search").autocomplete({
    source: "/Home/GetProduct",
    minLength: 3,
    select: function (event, ui) {
        window.location.href = ui.item.url;
    }
});
$('#Search').on('keypress', function (e) {
    if (e.which === 13) {
        window.location.href = '/Home/Index?searchString=' + $('#Search').val();
    }
});