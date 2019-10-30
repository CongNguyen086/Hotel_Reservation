//=============================play/ pause slider===============
"use strict";
var myVideo = document.getElementById("video-demo");
function loadvideo(){
    myVideo.play();
}
function loadvideo1(){
    myVideo.play();
}
function loadvideo2(){
    myVideo.play();
}
function loadvideo3(){
    myVideo.play();
}
function playPause() {
    if (myVideo.paused)
        myVideo.play();
    else
        myVideo.pause();
}
function enableMute() {
    myVideo.muted = false;
}

function disableMute() {
    myVideo.muted = true;
}

//======================================================================================================================

jQuery(document).ready(function ($) {
    "use strict";
    //==================load page===============
    setTimeout(function() {
        jQuery('.load-page').hide();
    }, 1000);


    //====Light Gallery====
    lightGallery(document.getElementById('lightgallery'));

    //=====COUNTTER UP=====
    $('.counter').counterUp({
        delay: 10,
        time: 1000
    });

    //======================search menu=============================
    $('.uni-search-box').on('click', function (e) {
        if($(e.target).is('.toggle-form, .toggle-form i')){
            $('.form-search-wrapper').slideToggle();
        }
    });

//======================MENU MOBILES===================
    $('.mobile-menu-container .menu-mobile-nav').on('click', function (e) {
        if($(e.target).is('.icon-bar i')){
            $('#cssmenu').slideToggle();
            $('#cssmenu ul ul').hide();
            $('.vk-menu-mobile-nav.mobile-menu-container ul.navbar-nav').show();
        }
        if($(e.target).is('.icon-search i')){
            $('#cssmenu').slideToggle();
            $('.vk-menu-mobile-nav.mobile-menu-container ul.navbar-nav').hide();
            return true;
        }
    });
    $('.mobile-menu-container #cssmenu .uni-icon-close').on('click', function (e) {
        if($(e.target).is('span')){
            $('#cssmenu').hide(500);
        }
    });
    (function($) {

        $.fn.menumaker = function(options) {

            var cssmenu = $(this), settings = $.extend({
                title: "Menu",
                format: "dropdown",
                sticky: false
            }, options);

            return this.each(function() {
                cssmenu.find('li ul').parent().addClass('has-sub');

                var multiTg = function() {
                    cssmenu.find(".has-sub").prepend('<span class="submenu-button"></span>');
                    cssmenu.find('.submenu-button').on('click', function() {
                        $(this).toggleClass('submenu-opened');
                        $(this).toggleClass('active');
                        if ($(this).siblings('ul').hasClass('open')) {
                            $(this).siblings('ul').removeClass('open').hide();
                        }
                        else {
                            $(this).siblings('ul').addClass('open').show();
                        }
                    });
                };

                if (settings.format === 'multitoggle') multiTg();
                else cssmenu.addClass('dropdown');

                if (settings.sticky === true) cssmenu.css('position', 'fixed');

                var resizeFix = function() {
                    if ($( window ).width() > 992) {
                        cssmenu.find('ul').show();
                    }
                };
                resizeFix();
                return $(window).on('resize', resizeFix);

            });
        };
    })(jQuery);

    (function($){
        $(document).ready(function(){

            $(document).ready(function() {
                $("#cssmenu").menumaker({
                    title: "Menu",
                    format: "multitoggle"
                });

                $("#cssmenu").prepend("<div id='menu-line'></div>");

                var foundActive = false, activeElement, linePosition = 0, menuLine = $("#cssmenu #menu-line"), lineWidth, defaultPosition, defaultWidth;

                $("#cssmenu > ul > li").each(function() {
                    if ($(this).hasClass('active')) {
                        activeElement = $(this);
                        foundActive = true;
                    }
                });

                if (foundActive === false) {
                    activeElement = $("#cssmenu > ul > li").first();
                }

                defaultWidth = lineWidth = activeElement.width();

                defaultPosition = linePosition = activeElement.position().left;

                menuLine.css("width", lineWidth);
                menuLine.css("left", linePosition);

                $("#cssmenu > ul > li").on('mouseenter', function () {
                        activeElement = $(this);
                        lineWidth = activeElement.width();
                        linePosition = activeElement.position().left;
                        menuLine.css("width", lineWidth);
                        menuLine.css("left", linePosition);
                    },
                    function() {
                        menuLine.css("left", defaultPosition);
                        menuLine.css("width", defaultWidth);
                    });
            });
        });
    })(jQuery);


    //==============================btn load more=========================
    $(".vk-testimonials-content-item").slice(0, 6).show();
    $(".btn-more").on('click', function (e) {
        e.preventDefault();
        $(".vk-testimonials-content-item:hidden").slice(0, 3).slideDown();
        $('html,body').animate({
            scrollTop: $(this).offset().top -1000
        }, 1000);
    });

    // ==================play and pause==================
    $('button.vk-play').on('click', function (e) {
        if($(e.target).is(this)){
            $('button.vk-pause').show();
            $('button.vk-play').hide();
        }
    });
    $('button.vk-pause').on('click', function (e) {
        if($(e.target).is(this)){
            $('button.vk-play').show();
            $('button.vk-pause').hide();
        }
    });

    //====================mute and sound================
    $('button.vk-enablemute').on('click', function (e) {
        if($(e.target).is(this)){
            $('button.vk-mutesound').show();
            $('button.vk-enablemute').hide();
        }
    });
    $('button.vk-mutesound').on('click', function (e) {
        $(this).hide();
        if($(e.target).is(this)){
            $('button.vk-enablemute').show();
            $('button.vk-mutesound').hide();
        }
    });


    //==========Owl Carosel===========
    if(! $.isFunction('owlCarousel')){
        $('.vk-owl-three-dots').owlCarousel({
            loop:true,
            margin:30,
            nav:false,
            dots:true,
            responsive:{
                0:{
                    items:1
                },
                992:{
                    items:3
                }
            }
        });
        $('.vk-owl-one-item').owlCarousel({
            loop:true,
            nav:true,
            navText:false,
            dots:true,
            responsive:{
                0:{
                    items:1
                }
            }
        });

        $('#owl-slide-home').owlCarousel({
            loop:true,
            autoplayTimeout:6000,
            nav:true,
            navText:true,
            dots:false,
            items:1,
            autoHeight:true,
            autoWidth:false
        });
    }
    
    // ============================ACCORDION===================================

    $('.vk-accordion-default .vk-accordion-toggle-default , .vk-accordion-with-bg .vk-accordion-toggle-default').on('click', function (e) {
        if($(e.target).is('h4.vk-accordion-toggle-default')){
            $(this).next().slideToggle('600');
            $(".vk-accordion-content-default").not($(this).next()).slideUp('600');
            $(this).toggleClass('active').siblings().removeClass('active');
        }
    });

    // ======================================search================================
    $('.box-search-header').hide();
    $('.vk-icon-search').on('click', function (e) {
        if($(e.target).is(this)){
            $('.box-search-header').slideToggle();
        }
        if($(e.target).is('i')){
            $('.box-search-header').slideToggle();
        }
    });

    // ============================short code hover=================================
    $('.hover-short').on({
        mouseenter: function () {
            $('.show-hover-shortcodes').fadeIn();
        },
        mouseleave: function () {
            $('.show-hover-shortcodes').hide();
        }
    });
    $('.show-hover-shortcodes').on({
        mouseenter: function () {
            $(this).show();
        },
        mouseleave: function () {
            $(this).fadeOut();
        }
    });

    //======isotop=======
    $('.vk-main-iso').isotope({
        itemSelector: '.item',
        percentPosition:true,
        layoutMode: 'fitRows',
        masonry:{
            columnWidth:'.item'
        },
        return:true
    });
    $('.grid').isotope({
        itemSelector: '.grid-item',
        percentPosition:true,
        masonry:{
            columnWidth:'.grid-sizer'
        }
    });
    // Isotope click function
    $('.vk-iso-nav ul li').on('click', function (e) {
        if($(e.target).is(this)){
            $('.vk-iso-nav ul li').removeClass('active');
            $(this).addClass('active');

            var selector = $(this).attr('data-filter');
            $('.vk-main-iso').isotope({
                filter: selector
            });
            return false;
        }
    });
    $('.vk-iso-nav ul li').on('click', function (e) {
        if($(e.target).is(this)){
            $('.vk-iso-nav ul li').removeClass('active');
            $(this).addClass('active');
        }
        var selector = $(this).attr('data-filter');
        $('.grid').isotope({
            filter: selector
        });
        return false;
    });

    //=============Calendar=============
    moment.locale('tr');
    var date = new Date();
    var bugun = moment(date).format("DD/MM/YYYY");

    var date_input1=$('input[name="date1"]'); //our date input has the name "date"
    var date_input2=$('input[name="date2"]'); //our date input has the name "date"
    var container=$('.bootstrap-iso form').length>0 ? $('.bootstrap-iso form').parent() : "body";
    var options={
        container: container,
        todayHighlight: true,
        autoclose: true,
        format: 'dd/mm/yyyy',
        language: 'tr'
    };
    date_input1.val(bugun);
    date_input1.datepicker(options).on('focus', function(date_input){
    });
    date_input2.val(bugun);
    date_input2.datepicker(options).on('focus', function(date_input){
    });


    date_input1.change(function () {
        var deger = $(this).val();
    });
    date_input2.change(function () {
        var deger = $(this).val();
    });


    $('.date-check-in').find('#ti-calendar1').on('click', function(){

        if( !date_input1.data('datepicker').picker.is(":visible"))
        {
            date_input1.trigger('focus');
        } else {
        }
    });
    $('.date-check-out').find('#ti-calendar2').on('click', function(){

        if( !date_input2.data('datepicker').picker.is(":visible"))
        {
            date_input2.trigger('focus');
        } else {
        }
    });


    // =========shop loc giá=============
    $( "#slider-range" ).slider({
        range: true,
        min: 0,
        max: 1000,
        values: [ 75, 500 ],
        slide: function( event, ui ) {
            $( "#amount" ).val( "$" + ui.values[ 0 ] + " - $" + ui.values[ 1 ] );
        }
    });
    $( "#amount" ).val( "$" + $( "#slider-range" ).slider( "values", 0 ) +
        " - $" + $( "#slider-range" ).slider( "values", 1 ) );


    //=======cout so luong==========
    var $str = $('.vk-shop2-slide1-details-number').text();

    $('.vk-shop2-slide1-details-minus').on('click', function (e) {
        if($(e.target).is(this)){
            $str--;

            if ($str <= 1) {
                $str =1;
            }

            $('.vk-shop2-slide1-details-number').text($str);
        }

    });

    $('.vk-shop2-slide1-details-plus').on('click', function (e) {
        if($(e.target).is(this)){
            $str++;

            $('.vk-shop2-slide1-details-number').text($str);
        }
    });


    //======================select image==================================
    $('#img1').on('click', function (e) {
        if($(e.target).is('img')){
            $('.vk-shop2-slide1-img img:nth-child(1)').fadeIn();
            $('.vk-shop2-slide1-img img:nth-child(2)').hide();
            $('.vk-shop2-slide1-img img:nth-child(3)').hide();
        }
    });
    $('#img2').on('click', function (e) {
        if($(e.target).is('img')){
            $('.vk-shop2-slide1-img img:nth-child(2)').fadeIn();
            $('.vk-shop2-slide1-img img:nth-child(1)').hide();
            $('.vk-shop2-slide1-img img:nth-child(3)').hide();
        }
    });
    $('#img3').on('click', function (e) {
        if($(e.target).is('img')){
            $('.vk-shop2-slide1-img img:nth-child(3)').fadeIn();
            $('.vk-shop2-slide1-img img:nth-child(1)').hide();
            $('.vk-shop2-slide1-img img:nth-child(2)').hide();
        }
    });

    //====================shop cart=================================
    $(".ddd").on("click", function (e) {
        var $button = $(this);
        var oldValue = $button.closest('.sp-quantity').find("input.quntity-input").val();

        if ($button.text() == "+") {
            var newVal = parseFloat(oldValue) + 1;
        } else {
            // Don't allow decrementing below zero
            if (oldValue >= 1) {
                var newVal = parseFloat(oldValue) - 1;
            } else {
                newVal = 0;
            }
        }

        $button.closest('.sp-quantity').find("input.quntity-input").val(newVal);
        return false;
    });

    //------------------checkout-------------------
    $(".woocommerce-info ").on('click', function (e) {
        if($(e.target).is('.click-here-to-login')){
            $('.vk-form-woo-login').slideToggle();
            return false;
        }
        if($(e.target).is('.click-here-entry-code')){
            $('.vk-check-coupon').slideToggle();
            return false;
        }
    }) ;
    $('.vk-checkout-ship-address').on('click', function (e) {
       if($(e.target).is('.btn-checkbox-ship-address')){
           $('.checkout-ship-address-checkbox').slideToggle();
       }
    });
    $('.vk-checkout-creat-account').on('click', function (e) {
        if($(e.target).is('.checkbox-create-account')){
            $('.checkbox-create-account-form').slideToggle();
        }
    });


    // ===========================Select language====================
    function DropDown(el) {
        this.dd = el;
        this.dda=el;
        this.ddc=el;
        this.dde=el;
        this.placeholder = this.dd.children('span');
        this.opts = this.dd.find('.dropdown a');
        this.val = '';
        this.index = -1;
        this.initEvents();
    }
    DropDown.prototype = {
        initEvents : function() {
            var obj = this;

            obj.dd.on('click', function(event){
                $(this).toggleClass('active');
                return false;
            });

            obj.opts.on('click',function(){
                var opt = $(this);
                obj.val = opt.text();
                obj.index = opt.index();
                obj.placeholder.text(obj.val);
            });
        },
        getValue : function() {
            return this.val;
        },
        getIndex : function() {
            return this.index;
        }
    };

    $(function() {

        var dd = new DropDown( $('#dd') );
        var dda = new DropDown( $('#dda') );
        var ddc = new DropDown( $('#ddc') );
        var dde = new DropDown( $('#dde') );

    });


    //===========================menu scroll====================
    $(".uni-sticky").sticky({topSpacing:0});

    //=======booking hotel=========
    $('.form_datetime').datetimepicker({
        //language:  'fr',
        weekStart: 1,
        todayBtn:  1,
        autoclose: 1,
        todayHighlight: 1,
        startView: 2,
        forceParse: 0,
        showMeridian: 1
    });
    $('.form_date').datetimepicker({
        language:  'fr',
        weekStart: 1,
        todayBtn:  1,
        autoclose: 1,
        todayHighlight: 1,
        startView: 2,
        minView: 2,
        forceParse: 0
    });
    $('.form_time').datetimepicker({
        language:  'fr',
        weekStart: 1,
        todayBtn:  1,
        autoclose: 1,
        todayHighlight: 1,
        startView: 1,
        minView: 0,
        maxView: 1,
        forceParse: 0
    });

    //====================
    $('.ui.selection.dropdown').dropdown();
    $('.ui.menu .ui.dropdown').dropdown({
        on: 'hover'
    });

    //==============back to top===========

    $('body').append('<div id="toTop"><div class="btn btn-totop"><i class="fa fa-chevron-up" aria-hidden="true"></i></div></div>');
    $(window).on('scroll', function () {
        if ($(this).scrollTop() != 0) {
            $('#toTop').fadeIn();
        } else {
            $('#toTop').fadeOut();
        }
    });
    $('#toTop').on('click', function (e) {
        if($(e.target).is('.btn-totop')){
            $("html, body").animate({ scrollTop: 0 }, 600);
            return false;
        }
        if($(e.target).is('.btn-totop i')){
            $("html, body").animate({ scrollTop: 0 }, 600);
            return false;
        }
    });
});