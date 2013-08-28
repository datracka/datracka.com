/**
 * Created with JetBrains PhpStorm.
 * User: datracka
 * Date: 20/10/12
 * Time: 12:03
 * To change this template use File | Settings | File Templates.
 */

$(function() {
    $('a.moveTo').bind('click',function(event){
        var $anchor = $(this);

        $('html, body').stop().animate({
            scrollTop: $($anchor.attr('href')).offset().top + 2
        }, 500,'easeInOutExpo');
        /*
         if you don't want to use the easing effects:
         $('html, body').stop().animate({
         scrollTop: $($anchor.attr('href')).offset().top
         }, 1000);
         */
        event.preventDefault();
    });
});