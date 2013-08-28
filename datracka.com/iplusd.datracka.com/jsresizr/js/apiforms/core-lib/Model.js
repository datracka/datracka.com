document.write("<script type=\"text/javascript\" src=\"js/jquery/jquery.xml2json.pack.js\"></script"+">");

var Model = function (){
	
	//-- Public elements
	this.public = {
		
		iniciarSesion: function(oController){
			var xml = private.convertToXml(private.optIniciarSesion, "", "");
			private.gateway(oController, xml, private.optIniciarSesion.actionName);
		},
		obtenerDominios: function(oController, sid){
			var xml = private.convertToXml(private.optDominios, sid, "");
			private.gateway(oController, xml, private.optDominios.actionName);
		},
		obtenerDominio: function(oController, sid, alias){
			private.optDominios.nodes = [{alias:"alias", value:alias}];
			var xml = private.convertToXml(private.optDominios, sid, "");
			private.gateway(oController, xml, private.optDominios.actionName);
		},
		obtenerDominioAsociado: function(oController, sid, alias, valor){
			private.optDominios.nodes = [{alias: "alias", value: alias}, {alias: "id", value: valor}];
			var xml = private.convertToXml(private.optDominios, sid, "");
			private.gateway(oController, xml, private.optDominios.actionName);
		},
		obtenerDefinicion: function(oController, sid){
			var xml = private.convertToXml(private.optDefinicionCuestionario, sid, "");
			private.gateway(oController, xml, private.optDefinicionCuestionario.actionName);
		},
		guardarEncuesta: function(oController, sid, encuesta, idContacto){
			private.optGuardarEncuesta.nodes = encuesta;
			xml = private.convertToXml(private.optGuardarEncuesta, sid, idContacto);
			private.gateway(oController, xml, private.optGuardarEncuesta.actionName);
		},
		obtenerEncuesta: function(oController, sid, pinCode){
			private.optObtenerEncuesta.nodes = [{alias: "particular_pincode_1", value: pinCode}];
			xml = private.convertToXml(private.optObtenerEncuesta, sid, "");
			private.gateway(oController, xml, private.optObtenerEncuesta.actionName);
		},
		consultarRestricciones: function(oController, sid, restricciones){
			private.optConsultarRestricciones.nodes = restricciones;
			xml = private.convertToXml(private.optConsultarRestricciones, sid, "");
			private.gateway(oController, xml, private.optConsultarRestricciones.actionName);
		},
		
		obtenerReglas: function(oController) {
			$.ajax({
				url: "./xml/reglas.xml",
				dataType: "xml",
				cache: false,
				async: false,
				error: function(xhr, textStatus, errorThrown){
					oController.setReglas(null);
				},
				success: function(data) {
					var reglas = $.xml2json(data);
					oController.setReglas(reglas);
				}
			});
		},
		obtenerTipoDominios: function(oController) {
			$.ajax({
				url: "./xml/visualizacion.xml",
				dataType: "xml",
				cache: false,
				async: false,
				error: function(xhr, textStatus, errorThrown){
					oController.setTipoDominios(null);
				},
				success: function(data) {
					var tipos = $.xml2json(data);
					oController.setTipoDominios(tipos);
				}
			});
		}
	}
	
	//-- Private elements
	var private = {
		optIniciarSesion: {
				rootTagName: 'peticion',
				actionName: 'iniciarSesion',
				nodes: [{alias:"usuario", value:CONSTANTS.USUARIO}, {alias:"password", value:CONSTANTS.PASSWORD}]
		},
		optDominios: {
				rootTagName: 'peticion',
				actionName: 'obtenerDominiosValor',
				nodes: [{alias:"alias", value:"*"}]
		},
		optDominioAsociado: {
				rootTagName: 'peticion',
				actionName: 'obtenerDominiosValor',
				nodes: [{alias:"alias", value:""}, {alias:"id", value:""}]
		},
		optGuardarEncuesta: {
				rootTagName: 'peticion',
				actionName: 'guardarEncuestaValorRestriccion',
				nodes: []
		},
		optDefinicionCuestionario: {
				rootTagName: 'peticion',
				actionName: 'definicionCuestionario',
				nodes: []
		},
		optObtenerEncuesta: {
				rootTagName: 'peticion',
				actionName: 'buscarEncuestasValor',
				nodes: []
		},
		optConsultarRestricciones: {
				rootTagName: 'peticion',
				actionName: 'consultarRestricciones',
				nodes: []
		},
		convertToXml : function(json, sid, idContacto){
			
				var xmlTag = "<" + json.rootTagName + " tipo=\"" + json.actionName + "\"";
				if(sid != "") xmlTag += " sid=\"" + sid + "\"";
				xmlTag += ">";
				
				xmlTag += "<encuesta token=\"" + ENCUESTA.TOKEN + "\" idContacto=\"" + idContacto + "\" formatVersion=\"1\""
				if(json.actionName == "buscarEncuestasValor") xmlTag += " pagina=\"1\" resultadosPagina=\"1\"";
				xmlTag += ">";
				
				$.each(json.nodes, function(i,node){
				xmlTag += "<" + node.alias + ">" + node.value + "</" + node.alias + ">";
				});
				
				xmlTag += "</encuesta>";
				xmlTag += "</peticion>";
				
				return xmlTag;
		},
		gateway: function(oController, xml, peticion){
			
			$.ajax({
				async: false,
				cache: false,
				data: xml,
				url: CONSTANTS.URL, 
				type: CONSTANTS.TYPE,
				dataType: "xml",
				contentType: "application/xhtml+xml",
				
				beforeSend: function(xhr) {
					xhr.setRequestHeader("Content-Type", "text/xml");
					xhr.setRequestHeader("GQHost", "http://" + CONSTANTS.GQHost + "/ServiciosNeuronics/" + peticion + ".neu");
				},
			
				error: function(xhr, textStatus, errorThrown){
					//pendiente tratamiento de errores!!!
					//console.log("Pasó lo siguiente: " + xhr + " - " + textStatus + " - " + errorThrown);
					oController.setRespuesta(null);
				},
			
				success: function(data){
	
					var xml;
					if(typeof data == 'string'){
						xml = new ActiveXObject('Microsoft.XMLDOM');
						xml.async = false;
						xml.loadXML(data);
					} else {
						xml = data;
					}
					
					var jsonObject = $.xml2json(xml);
					oController.setRespuesta(jsonObject);
				}
			
			});
		} //-- gateway
			
	}
	
}