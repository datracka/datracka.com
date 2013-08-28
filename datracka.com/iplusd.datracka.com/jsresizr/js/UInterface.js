var UInterface = function(){
	
	//private elementes
	var private = {
		p_vel:300, //velocidad de fadeins / fadeouts
		p_height: 600,
		p_seccion: "div#defecto",
		idUsuario: "",
		reserva: false, //true -> reserva ya efectuada por otro canal; false -> usuario puede reservar por la web
		limite:false, //true -> limite de reservas alcanzado ; false -> plazas disponibles
		asistencia: false,
		formularioOk: false,
		isUser: true,
		flashVideo: false //controla que el video solo se incruste una vez
	}
	
	//public elements
	this.public = {
		
		getIdUsuario: function(){ return private.idUsuario;},
		setIdUsuario: function(idUsuario){ private.idUsuario = idUsuario;},
		getReserva: function(){ return private.reserva;},
		setReserva: function(reserva){ private.reserva = reserva;},
		getLimite: function(){ return private.limite;},
		setLimite: function(limite){ private.limite = limite;},
		getAsistencia: function(){ return private.asistencia;},
		setAsistencia: function(asistencia){ private.asistencia = asistencia;},
		getFormularioOk: function(){ return private.formularioOk;},
		setFormularioOk: function(formularioOk){ private.formularioOk = formularioOk;},
		
		
		applyFonts: function(){
		
			Cufon.refresh();
			//console.log("aplicadas");

		},
		
		centerPage:function(element, height){
			p_height = height;
			var fixHeight = $(document).height();
            var outerWrapper = p_height;
            var top = fixHeight/2 - outerWrapper/2;
            $(element).css({"position" : "relative", "top" : top});
		},
				
		viewPage:function(contenedor, tagToget, vel){
				
				private.p_vel = vel;
				$(contenedor).fadeOut(private.p_vel, function(){
					$.get('./' + tagToget,function(data){
						$(contenedor).html(data,function(){});
						oU.public.applyFonts();
		
						$(contenedor).fadeIn(private.p_vel);
						
						if (tagToget == "home.html") private.p_seccion = "div#defecto";

					});
				});
				
				this.hideLoading();
		},
		
		displaySeccion: function(bg_div, bg_imagen, seccion, vel){
			
			
			if(private.p_seccion != seccion){
				//console.log("a"+private.p_seccion)
				var pp = private.p_seccion; //bug a investigar...
				$(bg_div).fadeOut(private.p_vel, function(){
					$(bg_div).css('background-image','url('+bg_imagen+')');
					$(pp).hide();
					$(seccion).show();
				});
				
				$(bg_div).fadeIn(private.p_vel);	
				
				private.p_seccion = seccion;
			}
		
			if(seccion == "div#apertura"){
				$("a#linkApertura").hide();
				$("a#linkJamie").show();
				$("a#linkA7").show();
			}else if(seccion == "div#jamie"){
				$("a#linkApertura").show();
				$("a#linkJamie").hide();
				$("a#linkA7").show();
			}else if(seccion == "div#a7"){
				$("a#linkApertura").show();
				$("a#linkJamie").show();
				$("a#linkA7").hide();
			}
			
			$("div#volver").show();
			$("p#btnConfirmacion").show();
	
			
		},
		
		swapLayers: function(layer1, layer2){
			$(layer1).fadeOut(private.p_vel, function(){$(layer2).fadeIn(private.p_vel);});
			//console.log(layer1, layer2);
			
		},
		
		/* quitar o poner fondo en un div.
		1 -> acción quitar fondo.
		0 -> acción poner fonfo
		*/
		
		swapBg: function(bg,quitarFondo,img){
			if (quitarFondo == 1){ //no queremos fondo.
				$(bg).css('background-image','none');
			}
			else{
				$(bg).css('background-image','url(img/'+img+')');
			}
		
		},
		
		skipVideo: function(){

			$('#videoContent').fadeOut(private.p_vel,function(){$('#content').fadeIn(private.p_vel);});

			oU.public.viewPage('div#header', 'header_home.html', 300);
			oU.public.viewPage('div#container', 'home.html', 300);
			
			oAnalytics = new Analytics();
			oAnaliycs.public.trackView('Home', 'acceso_home', 'Entradas del usuario a la home');
		
				
		},
		
		abrirPopup: function (url, w, h, sbars) {
	
			var migX = w/2;
			var migY = h/2;
		
			var x = screen.width/2 - migX;
			var y = screen.height/2 - migY;
		
			valors = 'width='+w+',height='+h+',left='+x+',top='+y+',scrollbars='+sbars+',resizable=no';
		
			window.open(url,'',valors);
		
		}, 
		
		mostrarDespedida: function(){
			//console.log("mostrar despedida");
			
			var formularioOk = this.getFormularioOk();
			var limite = this.getLimite();
			var asistencia = this.getAsistencia();
			
			//console.log (formularioOk, limite, asistencia);
			
			this.hideVolver();
			
			if (formularioOk == false){
				oU.public.viewPage('div#container', 'cartelas/cartela_error.html', 0);
			}else{
				if (limite == true){
					oU.public.viewPage('div#container', 'cartelas/cartela_plazas_ko.html', 0);
				}else{
					if (asistencia == true){
						oU.public.viewPage('div#container', 'cartelas/cartela_asiste.html', 0);
					}else{
						oU.public.viewPage('div#container', 'cartelas/cartela_no_asiste.html', 0);
					}
				}
			}
			
			oU.public.hideLoading();

		},

		showLoading: function(){
            $("body").append("<div id=\"loading\"><div id=\"loading-overlay\"></div><div id=\"loading-bkg\"></div></div>");
		},
		hideLoading: function(){
            $("div#loading").remove();
		},
		
		hideLogin: function(){
			$("div#loginPage").remove();
		},
		
		hideHome: function(){
			$("div#home").remove();
		},
		
		showVolver: function(){
			$('div#volver').show(); 
		},
		
		hideVolver: function(){
			$('div#volver').hide(); 
		},
		
		loadForm: function(){			
			
			
			this.hideHome();
			this.hideVolver();
			var reserva = this.getReserva();
			var limite = this.getLimite();
			var formularioOk = this.getFormularioOk();
			
			
			//console.log("reserva: " + reserva, "limite: "+ limite);
			
			if (formularioOk == false){
				if(reserva == true){
					oU.public.viewPage('div#container', 'form.html', 50);
				}else{
					if (limite == true){ //limite de reservas comletado
						oU.public.viewPage('div#container', 'cartelas/cartela_plazas_ko.html', 50);
					}else{ //usuario con permiso para entrar en el formulario
						oU.public.showLoading();
						oU.public.viewPage('div#container', 'form.html', 50);
					}
				}
			
			}else{ // formulario == true ya lo ha rellenado y por lo tanto no entramos
				oU.public.viewPage('div#container', 'cartelas/cartela_reserva_ko.html', 50);
			}
			
		

		},
		
		showPopup: function(popup){
			
		},
		
		hidePopup: function(popup){
			$(popup).hide();
		},

		prelogin: function(clave){
			if (clave != null && clave != ""){
				this.hideLogin();
				this.showLoading();
				setTimeout("oU.public.login('" + clave + "')", 1000);
			}else{
				oU.public.viewPage('div#container', 'cartelas/cartela_error_login.html', 300);
			}
		},
		
		viewVideo: function(div,video){
			
			if (private.flashVideo == false){ //solo entrará una vez
			 $(div).flash(
				{ src: video,
				  width: 458,
				  height: 257 },
				{ version: 8 }
   		 		);
			private.flashVideo = true;
		}
		},
		
		viewYotube: function(){
				$('div#jamie_videoclip div#videoBox').flash(
				{ height: 265, width: 440 },
				{ version: 8 },
				function(htmlOptions) {
					$this = $(this);
					htmlOptions.src = 'http://www.youtube.com/v/S0z1Mo7O6dE';
					$this.html($.fn.flash.transform(htmlOptions));						
				}
   	 		);
		},

		/* función responsable de la lógica de negocio de login
		 * return: 
		 	Si devuelve return true deja paso al siguiente modulo.
		 */		
		login: function(clave){
			
			
				this.setIdUsuario(clave);
				
				//Comprobacion de la clave
				var isOk = true;
				
				var controller = new Controller();
				controller.public.iniciarSesion();
				if(controller.public.getSid() != null && controller.public.getSid != ""){
					var encuesta = controller.public.obtenerEncuesta(clave);
					if(encuesta != null){
						// Comprobamos si ya ha realizado la reservar anteriormente
						var asistencia = encuesta["asistencia_evento_1"];
						if(asistencia && asistencia != null && asistencia != "") this.setReserva(true);
						else this.setReserva(false);
						
											
						// Comprovamos si quedan plazas para el concesionario asignado
						var concesion = encuesta["concesion_asignada_1"];
						if(concesion && concesion != null && concesion != ""){
							var restricciones = [{alias: "asistencia_evento_1", value: "SI"}, {alias: "concesion_asignada_1", value: concesion}];
							var restriccion = controller.public.consultarRestricciones(restricciones);
							//console.log("Limite: " + restriccion.limite + " - Actual: " + restriccion.numeroActual);

							if(parseInt(restriccion.numeroActual) < parseInt(restriccion.limite)) this.setLimite(false);
							else this.setLimite(true);
						}else{
							isOk = false;
						}
					}else{
						isOk = false;
						isUser = false; //Encuesta no existente, login incorrecto
					}
				}else{
					isOk = false; //No se ha podido iniciar sesion
				}
				
				//this.hideLoading();
				
				/*test plan */
				/* reserva anteriormente no efectuada y plazas disponibles - OK */
				//this.setReserva(false);	
				//this.setLimite(false);
				
				/* reserva anteriormente efectuada y plazas disponibles */
				//this.setReserva(true);	
				//this.setLimite(false);
				
				/* reserva anteriormente no efectuada y no plazas disponibles */
				//this.setReserva(false);	
				//this.setLimite(true);
				
				/* reserva anteriormente efectuada y no plazas disponibles */
				//this.setReserva(true);	
				//this.setLimite(true);
				
				//console.log(this.getReserva(), this.getLimite());

			
				if (isOk){
					//mostramos capa de video
					$('#content').hide();
					$('#videoContent').show();
					//$('#content').fadeOut(private.p_vel,function(){$('#videoContent').fadeIn(private.p_vel);});
					this.hideLoading();
				
				}
				else{ //false
					if(!isUser) this.viewPage('div#container', 'cartelas/cartela_error_nouser.html', 300);
					else this.viewPage('div#container', 'cartelas/cartela_error.html', 300);
				}
			
		}
	}
}