document.write("<script type=\"text/javascript\" src=\"js/apiforms/Constants.js\"></script"+">");
document.write("<script type=\"text/javascript\" src=\"js/apiforms/core-lib/Model.js\"></script"+">");
document.write("<script type=\"text/javascript\" src=\"js/apiforms/core-lib/Render.js\"></script"+">");


var Controller = function (){
	
	//-- Public elements
	this.public = {
		
		getSid: function(){ return private.sid;},
		setSid: function(sid){ private.sid = sid;},
		getRespuesta: function(){ return private.respuesta;},
		setRespuesta: function(respuesta){ private.respuesta = respuesta;},
		getReglas: function(){ return private.reglas;},
		setReglas: function(reglas){ private.reglas = reglas;},
		getTipoDominios: function(){ return private.tipoDominios;},
		setTipoDominios: function(tipos){ private.tipoDominios = tipos;},
		getDominios: function(){ return private.dominios;},
		setDominios: function(dominios){ private.dominios = dominios;},
		getCuestiones: function(){ return private.cuestiones;},
		setCuestiones: function(cuestiones){ private.cuestiones = cuestiones;},
		getEncuesta: function(){ return private.encuesta;},
		setEncuesta: function(encuesta){ private.encuesta = encuesta;},
		getIdContacto: function(){ return private.idContacto;},
		setIdContacto: function(idContacto){ private.idContacto = idContacto;},

		init: function(){
			this.obtenerReglas();
			this.obtenerTipoDominios();
			
			this.iniciarSesion();
			if(this.getSid != "") {
				this.obtenerDominios();
				
				if (private.dominios.resultado == "KO") { 
					this.setDominios(null);
				}else{
					//console.log(this.getDominios());
					this.setDominios(this.getDominios().dominios.dominio);
				}
			}
		},
		
		obtenerCuestionario: function(pinCode){
			var result = null;
			
			if(this.getSid() != ""){
				var defCuestionario = this.obtenerDefinicion();
				this.obtenerEncuesta(pinCode);
				
				if(defCuestionario != null){
					var render = new Render(defCuestionario, this.getEncuesta(), this.getReglas(), this.getTipoDominios(), this.getDominios(), oController);
					
					result = render.public.getHtmlForm();
				}
			}
			
			return result;
		},	
		validarCuestionario: function(){
			var result = null;
			
			if(this.getSid() != ""){
				var render = new Render();
				result = render.public.validateForm(this.getCuestiones());
			}
			
			return result;
		},
		
		iniciarSesion: function(){
			var oModel = new Model();
		 	oModel.public.iniciarSesion(this);
			if(this.getRespuesta() != null && this.getRespuesta().resultado == "OK") this.setSid(this.getRespuesta().sid);
		},
		obtenerDominios: function(){
			var oModel = new Model();
			oModel.public.obtenerDominios(this, this.getSid());
			
			if(this.getRespuesta() != null && this.getRespuesta().resultado == "OK") this.setDominios(this.getRespuesta());

			return this.getDominios();
		},
		obtenerDominio: function(alias){
			var oModel = new Model();
			oModel.public.obtenerDominio(this, this.getSid(), alias);
		},
		obtenerDominioAsociado: function(element){ 
			var result = null;
			
			if ($(element).val() != ""){
				var oModel = new Model();
				oModel.public.obtenerDominioAsociado(this, this.getSid(), $(element).attr("name"), $(element).val());
				
				result = this.getRespuesta();
			}
			
			return result;
		},
		obtenerDefinicion: function(){
			var oModel = new Model();
			oModel.public.obtenerDefinicion(this, this.getSid());
			
			var result = null;			
			if(this.getRespuesta() != null && this.getRespuesta().resultado == "OK")  result = this.getRespuesta();
			
			return result;
		},
		obtenerEncuesta: function(pinCode){ 
			var oModel = new Model();
			oModel.public.obtenerEncuesta(this, this.getSid(), pinCode);
	
			var result = null;			
			if(this.getRespuesta() != null && this.getRespuesta().resultado == "OK"){  
				result = this.getRespuesta().encuestas.encuesta;
				if(result && result != null && result.idContacto) this.setIdContacto(result.idContacto);
			}
			
			
			this.setEncuesta(result);
			
			return result;
		},
		guardarEncuesta: function(){
			
			if(this.getCuestiones() == null) return false;
			
			var fields = new Array();
			for(var i = 0; i < this.getCuestiones().length; i++){
				var cuestion = this.getCuestiones()[i];
				var valor = "";
								
				var tipoDomino = this.getTipoDominios()[cuestion.alias];
				if(tipoDomino && typeof(tipoDomino) != "undefined"){
					if(tipoDomino.tipo == "checkbox" || tipoDomino.tipo == "radio") valor = $("[name=" + cuestion.alias + "]:checked").val();
					else valor = $("[name=" + cuestion.alias + "]").val();
				}else{
					 valor = $("[name=" + cuestion.alias + "]").val();
				}
				
				if(valor && typeof(valor) != "undefined" && valor != null && valor != ""){
					fields.push({alias: cuestion.alias, value: valor});
				}
			}
			
			var oModel = new Model();
			oModel.public.guardarEncuesta(this, this.getSid(), fields, this.getIdContacto());
			
			if(this.getRespuesta().resultado == "KO") return false;
			else if(this.getRespuesta().resultado == "OK") return true;
			
		},
		consultarRestricciones: function(restricciones){
			var oModel = new Model();
			oModel.public.consultarRestricciones(this, this.getSid(), restricciones);
			
			var result = null;			
			if(this.getRespuesta() != null && this.getRespuesta().resultado == "OK")  result = this.getRespuesta().restriccion;
			
			return result;
		},
		obtenerReglas: function(){
			var oModel = new Model();
			oModel.public.obtenerReglas(this);
			
			return this.getReglas();
		},
		obtenerTipoDominios: function(){
			var oModel = new Model();
			oModel.public.obtenerTipoDominios(this);
			
			return this.getTipoDominios();
		}
	}
	
	//-- Private elements
	var private = {
		sid: "",
		respuesta: null,
		reglas: null,
		tipoDominios: null,
		dominios: null,
		cuestiones: new Array(),
		encuesta: null,
		idContacto: ""
	}
	
}