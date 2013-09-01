// JavaScript Document

UserInterface = function(){
	
	var private = {
		_screen_width: null,
		_screen_height: null,
		_window_width: null,
		_window_height: null,
		flashVideo:false,
	}
	

	this.public = {		

   	    initWindow: function(){
			private._screen_width = $(window).width();
			private._screen_height = $(window).height();
		
			$("#infobackground").css({
				"width": private._screen_width,
				"height": private._screen_width,
				"min-height": private._screen_height,
			});
//			console.log("UserInterface initialized");
		},
		
		initMenu: function(){
		
		   //SIDEBAR -------------------------------------------------			
		   //thanks to gAlabnell www.gerardalbanell.com
		   //great developer
		   
			var offset = $("#menu").offset();
            var topPadding = 0;
            $(window).scroll(function() {
                if ($(window).scrollTop() > offset.top) {
                    $("#menu").stop().animate({
                        marginTop: $(window).scrollTop() + topPadding
                    } , 1000);
                } else {
                    $("#gadgets").stop().animate({
                        marginTop: 370
                    } , 1000);
                };
            });

		},
		
		viewVideo: function(div,video){
			
			if (private.flashVideo == false){ //solo entrará una vez
			 $(div).flash(
				{ src: video,
				  width: 600,
				  height: 500 },
				{ version: 10 }
   		 		);
			private.flashVideo = true;
			}
		},
		
		
		showVideo: function(_w,_h,div, video){

//			console.log("show video", (private._screen_width  / 2));

			private._window_width = _w;
			private._window_height = _h;
						
			var _left = (private._screen_width / 2) - (private._window_width / 2);
			var _top = $(document).scrollTop() + (private._screen_height  / 2) - (private._window_height/2);
							
			$(div).css({
				"left": _left,
				"top": _top 
			});
						
			$("#infobackground").fadeIn('fast',function(){
					$(div).fadeIn('slow');
			});
			
			document.getElementById("vidly-video").src = video;
			document.getElementById("vidly-video").play();
			console.log(video);
		},

		closeVideo: function (div){
	//		console.log("close video");
			$(div).fadeOut('fast',function(){
					$("#infobackground").fadeOut('slow');
			});
			
			document.getElementById("vidly-video").pause();
			document.getElementById("vidly-video").src = "";

		},
		
		initRelLinks : function(){
	/*  FOOTER LINKS  */

			$('ul#footer-social-links a')
				.mouseover(function(){
						var x;
						var y;
						var el = $(this);
						if($(this).css('backgroundPositionX')){
							x = el.css('backgroundPositionX').split('px')[0];
							y = el.css('backgroundPositionY').split('px')[0];
						}
						else{
							x = el.css('background-position').split(' ')[0].split('px')[0];
							y = el.css('background-position').split(' ')[1].split('px')[0];
						}
						
					var bgPosition = '('+ x +'px '+ (parseInt(y-25)) +'px)';
					
						el.stop().animate({backgroundPosition : bgPosition}, {duration:500});
						  
					})
					
				.mouseout(function(){
						var x;
						var y;
						var el = $(this);
						if($(this).css('backgroundPositionX')){
							x = el.css('backgroundPositionX').split('px')[0];
							y = el.css('backgroundPositionY').split('px')[0];
						}
						else{
							x = el.css('background-position').split(' ')[0].split('px')[0];
							y = el.css('background-position').split(' ')[1].split('px')[0];
						}
						
						var bgPosition = '('+ x +'px 0px)';
				
						el.stop().animate({backgroundPosition : bgPosition}, {duration:500});
					});
			
			
		}

}
	
};