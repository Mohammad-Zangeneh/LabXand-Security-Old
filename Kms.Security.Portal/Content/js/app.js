/*****
* CONFIGURATION
*/

//Main navigation
$.navigation = $('nav > ul.nav');

$.panelIconOpened = 'icon-arrow-up';
$.panelIconClosed = 'icon-arrow-down';

//Default colours
$.brandPrimary = '#20a8d8';
$.brandSuccess = '#4dbd74';
$.brandInfo = '#63c2de';
$.brandWarning = '#f8cb00';
$.brandDanger = '#f86c6b';

$.grayDark = '#2a2c36';
$.gray = '#55595c';
$.grayLight = '#818a91';
$.grayLighter = '#d1d4d7';
$.grayLightest = '#f8f9fa';

'use strict';

/****
* MAIN NAVIGATION
*/

$(document).ready(function ($) {



    // Add class .active to current link
    $.navigation.find('a').each(function () {
        var cUrl = window.location.href;// String(window.location).split('?')[0];

        if (cUrl.substr(cUrl.length - 1) == '#') {
            cUrl = cUrl.slice(0, -1);
        }

        if ($($(this))[0].href == cUrl) {
            $(this).addClass('active');

            $(this).parents('ul').add(this).each(function () {
                $(this).parent().addClass('open');
            });
        }
        else {
            $(this).remove('active');
        }
    });

    // Dropdown Menu
    $.navigation.on('click', 'a', function (e) {
        //morsa
        $.navigation.find('a').each(function () {
            $(this).removeClass('active'); 
        });

        var cUrl = window.location.href
        if (($($(this))[0].href.substr(cUrl.indexOf("#"))).length >= 2) {
            document.body.scrollTop = 0;
            document.documentElement.scrollTop = 0;
            $(this).addClass('active');
        }

        var aTags = $.navigation.find('li');
        var temp = null;
        for (var i = 0 ; i < aTags.length; i++) {
            var test = aTags[i].classList;
            if (test.value == "nav-item nav-dropdown open" && (aTags[i] != $(this).parent()[0]) && aTags[i].innerHTML.search($(this).parent()[0].id) < 0) {
                aTags[i].classList.remove('open');
                temp = aTags[i];
            }
        }
        if ($.ajaxLoad) {
            e.preventDefault();
        }
        if ($(this).hasClass('nav-dropdown-toggle')) {//&& $(this).parent()!= temp) {
            $(this).parent().toggleClass('open');
            resizeBroadcast();
        }
    });

    function resizeBroadcast() {

        var timesRun = 0;
        var interval = setInterval(function () {
            timesRun += 1;
            if (timesRun === 5) {
                clearInterval(interval);
            }
            window.dispatchEvent(new Event('resize'));
        }, 62.5);
    }

    /* ---------- Main Menu Open/Close, Min/Full ---------- */
    $('.sidebar-toggler').click(function () {
        $('body').toggleClass('sidebar-hidden');
        resizeBroadcast();
    });

    $('.sidebar-minimizer').click(function () {
        $('body').toggleClass('sidebar-minimized');
        resizeBroadcast();
    });

    $('.brand-minimizer').click(function () {
        $('body').toggleClass('brand-minimized');
    });

    $('.aside-menu-toggler').click(function () {
        $('body').toggleClass('aside-menu-hidden');
        resizeBroadcast();
    });

    $('.mobile-sidebar-toggler').click(function () {
        $('body').toggleClass('sidebar-mobile-show');
        resizeBroadcast();
    });

    $('.sidebar-close').click(function () {
        $('body').toggleClass('sidebar-opened').parent().toggleClass('sidebar-opened');
    });

    /* ---------- Disable moving to top ---------- */
    $('a[href="#"][data-top!=true]').click(function (e) {
        e.preventDefault();
    });

});

/****
* CARDS ACTIONS
*/

$(document).on('click', '.card-actions a', function (e) {
    e.preventDefault();

    if ($(this).hasClass('btn-close')) {
        $(this).parent().parent().parent().fadeOut();
    } else if ($(this).hasClass('btn-minimize')) {
        var $target = $(this).parent().parent().next('.card-body');
        if (!$(this).hasClass('collapsed')) {
            $('i', $(this)).removeClass($.panelIconOpened).addClass($.panelIconClosed);
        } else {
            $('i', $(this)).removeClass($.panelIconClosed).addClass($.panelIconOpened);
        }

    } else if ($(this).hasClass('btn-setting')) {
        $('#myModal').modal('show');
    }

});

function capitalizeFirstLetter(string) {
    return string.charAt(0).toUpperCase() + string.slice(1);
}

function init(url) {

    /* ---------- Tooltip ---------- */
    $('[rel="tooltip"],[data-rel="tooltip"]').tooltip({ "placement": "bottom", delay: { show: 400, hide: 200 } });

    /* ---------- Popover ---------- */
    $('[rel="popover"],[data-rel="popover"],[data-toggle="popover"]').popover();

}
