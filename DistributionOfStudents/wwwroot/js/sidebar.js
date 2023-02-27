$('a').not('.home').each(function () {
    if (window.location.pathname == '/' || window.location.pathname.includes('Home')) {
        $('.home').addClass('active');
    }
    else {
        if (window.location.pathname.includes($(this).attr('href'))) {
            $(this).addClass('active');
        }
    }
});