/* ------ Show and Hide Navigation ------ */

$(function () {

    //change nav on load
    showHideNav();

    $(window).scroll(function () {

        //change nav on scroll
        showHideNav();

    });

    function showHideNav(isMobile = false) {
        
        if ($(window).scrollTop() > 20 || isMobile) {
            //show white navigation bar
            $('header').addClass('sticky-header');
            $('.logo-wrapper img').attr('src', './Content/imgs/icons/logo.png');
            $('.navigation-wrapper li button').removeClass('white-btn');
            $('.navigation-wrapper li button').addClass('rblue-btn');
            if(!$('#mobile-navigation-wrapper').is(':visible')){
                $('.menu-btn').find('img').attr('src', './Content/imgs/icons/bars-solid-rblue.png');
            } else {
                $('.menu-btn').find('img').attr('src', './Content/imgs/icons/times-solid-rblue.png');
            }

        } else {

            //hide white navigation bar
            $('header').removeClass('sticky-header');
            $('.logo-wrapper img').attr('src', './Content/imgs/icons/top-logo.png');
            $('.navigation-wrapper li button').removeClass('rblue-btn');
            $('.navigation-wrapper li button').addClass('white-btn');
            $('.menu-btn').find('img').attr('src', './Content/imgs/icons/bars-solid-white.png');
        }

    }
    
    $('.menu-btn').on('click',function() {
        
        $('#mobile-navigation-wrapper').slideToggle(200, function() {
            
            if(!$('header').hasClass('sticky-header')){
                showHideNav(true);
            } else {
            showHideNav(false);
            }
            
        });
        $('body').toggleClass('disable-scroll');
    });

});     