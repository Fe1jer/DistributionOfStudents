$('.ripple').not('.home').each(function () {
    if (window.location.pathname == '/' || window.location.pathname.includes('Home')) {
        $('.home').addClass('active');
    }
    else {
        var controller = window.location.pathname.split('/');
        if ($(this).attr('href').includes(controller[1])) {
            $(this).addClass('active');
        }
    }
});