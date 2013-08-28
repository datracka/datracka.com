document.write("<script type=\"text/javascript\" src=\"js/apiforms/ext-lib/cplib.js\"></script"+">");

function Render(defCuestionario, valEncuesta, defReglas, defTipoDominios, defDominios, oController){
	//-- Public elements
	this.public = {
		getHtmlForm : function(){private.renderForm(); return private.htmlForm;},
		validateForm: function(cuestiones){
			
			var errores = new Array();
			
			$(".error").removeClass("error");
			$(".lblError").removeClass("lblError");
						
			if(cuestiones != null){
				for(var i = 0; i < cuestiones.length; i++){
					var cuestion = cuestiones[i];
					if(cuestion.validador != ""){
						var regExp = new RegExp(cuestion.validador);
						if($("#" + cuestion.alias).val() != "" && !regExp.test($("#" + cuestion.alias).val())){
							errores.push(cuestion.alias);
						}
					}
				}
			}
			
			var requireds = $("input.required[type!=checkbox][type!=radio]");
			$.each(requireds, function(i, cuestion){
				if($(cuestion).val() == ""){
					errores.push($(cuestion).attr("id"));
				}
			});
			
			requireds = $("input.required[type=checkbox]");
			$.each(requireds, function(i, cuestion){
				var name = $(cuestion).attr("name");
				if($("input[name=" + name + "]:checked").val() == null){
					errores.push($(cuestion).attr("id"));
				}
			});
			
			requireds = $("input.required[type=radio]");
			$.each(requireds, function(i, cuestion){
				var name = $(cuestion).attr("name");
				if($("input[name=" + name + "]:checked").val() == null){
					errores.push($(cuestion).attr("id"));
				}
			});
			
			requireds = $("select.required");
			$.each(requireds, function(i, cuestion){
				if($(cuestion).val() == ""){
					errores.push($(cuestion).attr("id"));
				}
			});
			
			return errores;
		}
	}
	
	//-- Private elements
	var private = {
		cuestionario : defCuestionario,
		encuesta: valEncuesta,
		reglas: defReglas,
		tipoDominios: defTipoDominios,
		dominios: defDominios,
		controller: oController,
		htmlForm : null,
		renderForm : function(){
			if(private.cuestionario != null){
				//console.log(private.cuestionario);
				//console.log(private.dominios);
				//console.log(private.encuesta);
								
				private.htmlForm = document.createElement("form");
				private.htmlForm.setAttribute("id", private.cuestionario.encuestas.encuesta.token);
				private.htmlForm.setAttribute("name", private.cuestionario.encuestas.encuesta.token);
				private.htmlForm.setAttribute("onsubmit", "javascript:validateForm(); return false;");
				private.htmlForm.setAttribute("method", "post");
				private.htmlForm.setAttribute("action", "javascript:validateForm();");
				
				if(isArray(private.cuestionario.encuestas.encuesta.secciones.seccion)){
					for(var i = 0; i < private.cuestionario.encuestas.encuesta.secciones.seccion.length; i++){
						var seccionData = private.cuestionario.encuestas.encuesta.secciones.seccion[i];
						private.renderSeccion(seccionData, private.htmlForm);
					}
				}else{
					var seccionData = private.cuestionario.encuestas.encuesta.secciones.seccion;
					private.renderSeccion(seccionData, private.htmlForm);
				}
				
				var submitButton = document.createElement("input");
				submitButton.setAttribute("type", "submit");
				submitButton.setAttribute("id", "submitButton");
				submitButton.setAttribute("name", "submitButton");
				submitButton.setAttribute("value", "Enviar");
				
				var dSubmit = document.createElement("div");
				dSubmit.setAttribute("id", "d_submitButton");
				
				dSubmit.appendChild(submitButton);
				
				private.htmlForm.appendChild(dSubmit);
				
			}
		},
		renderSeccion : function(seccionData, form){
			var seccion = document.createElement("fieldset");
			seccion.setAttribute("id", "seccion_" + seccionData.numSeccion);
			seccion.setAttribute("class", "seccion");
			
			var lgTituloSeccion = document.createElement("legend");
			lgTituloSeccion.setAttribute("id", "tituloSeccion_" + seccionData.numSeccion);
			lgTituloSeccion.setAttribute("class", "tituloSeccion");
			
			var txTituloSeccion = document.createTextNode(seccionData.titulo);
			lgTituloSeccion.appendChild(txTituloSeccion);
			
			seccion.appendChild(lgTituloSeccion);
			
			if(isArray(seccionData.preguntas.pregunta)){
			
				for(var j = 0; j < seccionData.preguntas.pregunta.length; j++){
					var preguntaData = seccionData.preguntas.pregunta[j];
					private.renderPregunta(preguntaData, seccion);
				}
			}else{
				var preguntaData = seccionData.preguntas.pregunta;
				private.renderPregunta(preguntaData, seccion);
			}
			
			form.appendChild(seccion);
		},
		renderPregunta : function(preguntaData, seccion){
			var pregunta = document.createElement("div");
			pregunta.setAttribute("id", "pregunta_" + preguntaData.numPregunta);
			pregunta.setAttribute("class", "pregunta");
			
			var pTituloPregunta = document.createElement("p");
			pTituloPregunta.setAttribute("id", "tituloPregunta_" + preguntaData.numPregunta);
			pTituloPregunta.setAttribute("class", "tituloPregunta");
			
			var txTituloPregunta = document.createTextNode(preguntaData.enunciado);
			pTituloPregunta.appendChild(txTituloPregunta);
			
			pregunta.appendChild(pTituloPregunta);
			
			if(isArray(preguntaData.cuestiones.cuestion)){						
				for(var k = 0; k < preguntaData.cuestiones.cuestion.length; k++){
					var cuestionData = preguntaData.cuestiones.cuestion[k];
					private.renderCuestion(cuestionData, pregunta);
				}
				
			}else{
				var cuestionData = preguntaData.cuestiones.cuestion;
				private.renderCuestion(cuestionData, pregunta);
			}
			
			seccion.appendChild(pregunta);
		},
		renderCuestion : function(cuestionData, pregunta){
			
			private.controller.public.getCuestiones().push({"alias": cuestionData.alias, "validador": cuestionData.expresionRegular});
			
			var divCuestion = document.createElement("div");
			divCuestion.setAttribute("id", "c_" + cuestionData.alias);
			divCuestion.setAttribute("class", "cuestion");
			
			var lblCuestion = document.createElement("label");
			lblCuestion.setAttribute("id", "lbl_" + cuestionData.alias);
			lblCuestion.setAttribute("for", cuestionData.alias);
			
			var txtCuestion = document.createTextNode(cuestionData.enunciado);
			lblCuestion.appendChild(txtCuestion);
			
			divCuestion.appendChild(lblCuestion);
			
			if(cuestionData.tipoInput == "select"){
				
				if (private.tipoDominios[cuestionData.alias]){
					var tipo = private.tipoDominios[cuestionData.alias].tipo;
					
					if(tipo == "radio" || tipo == "checkbox"){					
						var dom = null;
						for(var i = 0; i < private.dominios.length; i ++){
							var domi = private.dominios[i];
							if(domi.alias == cuestionData.alias){ 
								dom = domi;
								break;
							}
						}
						
						if(dom != null){
						
							$.each(dom.opcion, function(i, inputNode){
								var checked = false;
								if(private.encuesta && typeof(private.encuesta) != "undefined" && private.encuesta != null && private.encuesta != "") if(private.encuesta[cuestionData.alias] && typeof(private.encuesta[cuestionData.alias]) != "undefined" && private.encuesta[cuestionData.alias] == inputNode.valor) checked = true;
								
								var input = null;
								if(document.all && !window.opera && document.createElement){ // For no DOM2 compilant (IE6 and IE7)
									var temp = "<input ";
									temp += "type=\"" + tipo + "\" ";
									temp += "name=\"" + dom.alias + "\" ";
									temp += "id=\"" + dom.alias + "_" + inputNode.valor + "\" ";
									temp += "value=\"" + inputNode.valor + "\" ";
									if(checked) temp += "checked=\"checked\" ";
									temp += " />"
									input = document.createElement(temp);
																		
								}else if(document.createElement && document.createTextNode){
									input = document.createElement("input");
									input.setAttribute("type", tipo);
									input.setAttribute("name", dom.alias);
									input.setAttribute("id", dom.alias + "_" + inputNode.valor);
									input.setAttribute("value", inputNode.valor);
									if(checked) input.setAttribute("checked", "checked");
								}
								
								divCuestion.appendChild(input);
								
								var lblInput = document.createElement("label");
								lblInput.setAttribute("for", dom.alias);
								lblInput.setAttribute("id", "lbl_" + dom.alias + "_" + inputNode.valor);
								
								var txtLblInput = document.createTextNode(inputNode.descripcion);
								lblInput.appendChild(txtLblInput);
								
								private.addAttributesCuestion(cuestionData, input, lblInput, divCuestion);
								
								divCuestion.appendChild(lblInput);
							});
						}else{
							private.addAttributesCuestion(cuestionData, null, null, divCuestion);
						}												
						
					}else if(tipo == "hidden"){
						var inpCuestion = document.createElement("input");
						inpCuestion.setAttribute("type", "hidden");
						inpCuestion.setAttribute("name", cuestionData.alias);
						inpCuestion.setAttribute("id", cuestionData.alias);
						
						if(private.encuesta && typeof(private.encuesta) != "undefined" && private.encuesta != null && private.encuesta != "") if(private.encuesta[cuestionData.alias] && typeof(private.encuesta[cuestionData.alias]) != "undefined") $(inpCuestion).val(private.encuesta[cuestionData.alias]);
						
						private.addAttributesCuestion(cuestionData, inpCuestion, lblCuestion, divCuestion);
						
						divCuestion.appendChild(inpCuestion);
					}
				}else{
					var selCuestion = document.createElement("select");
					
					selCuestion.setAttribute("name", cuestionData.alias);
					selCuestion.setAttribute("id", cuestionData.alias);
					
					private.addAttributesCuestion(cuestionData, selCuestion, lblCuestion, divCuestion);
					
					/** Option por defecto **/
					selCuestion.options.add(new Option(" " , ""));
					
					var dom = null;
					for(var i = 0; i < private.dominios.length; i ++){
						var domi = private.dominios[i];
						if(domi.alias == cuestionData.alias){ 
							dom = domi;
							break;
						}
					}
					
					if(dom != null){
						/** Resto de options **/
						$.each(dom.opcion, function(i, optionNode){
							selCuestion.options.add(new Option(optionNode.descripcion , optionNode.valor));
						});
					}
					
					if(private.encuesta && typeof(private.encuesta) != "undefined" && private.encuesta != null && private.encuesta != "") if(private.encuesta[cuestionData.alias] && typeof(private.encuesta[cuestionData.alias]) != "undefined") $(selCuestion).val(private.encuesta[cuestionData.alias]);
					
					divCuestion.appendChild(selCuestion);
					
				}
								
			}else if(cuestionData.tipoInput == "text" || cuestionData.tipoInput == "textarea"){
				var inpCuestion = document.createElement("input");
				if(private.tipoDominios[cuestionData.alias]){
					inpCuestion.setAttribute("type", private.tipoDominios[cuestionData.alias].tipo);
				}else{
					inpCuestion.setAttribute("type", "text");
				}
				inpCuestion.setAttribute("name", cuestionData.alias);
				inpCuestion.setAttribute("id", cuestionData.alias);
				if(private.encuesta && typeof(private.encuesta) && private.encuesta != null && private.encuesta != "") if(private.encuesta[cuestionData.alias] && typeof(private.encuesta[cuestionData.alias]) != "undefined") inpCuestion.setAttribute("value", private.encuesta[cuestionData.alias]);
				
				private.addAttributesCuestion(cuestionData, inpCuestion, lblCuestion, divCuestion);
				
				divCuestion.appendChild(inpCuestion);				
			}
			
			pregunta.appendChild(divCuestion);
		},
		addAttributesCuestion : function(cuestionData, cuestion, label, contenedor){
			
			if(cuestion != null && label != null){
				if(cuestionData.obligatoria == "S"){
					if(!private.tipoDominios[cuestionData.alias]) $(label).text($(label).text() + " *");
					$(cuestion).addClass("required");
				}
				if(cuestionData.tipoInput == "text" || cuestionData.tipoInput == "textarea") cuestion.setAttribute("maxlength", cuestionData.max);
				
				if(cuestionData.tipoInput == "select" && cuestionData.max > 1 && !private.tipoDominios[cuestionData.alias]){ 
					cuestion.setAttribute("multiple", "multiple");
					cuestion.setAttribute("size", cuestionData.max);
				}
				
				if(cuestionData.aliasAsociado != "") $(cuestion).click(function(){
					var dominio = private.controller.public.obtenerDominioAsociado(this);
					//console.log(dominio);
					private.renderDominio(dominio);
				});
			}
			
			private.addReglasCuestion(cuestionData, cuestion, label, contenedor);
			
		},
		addReglasCuestion : function(cuestionData, cuestion, label, contenedor){
			
			var alias = "";
			if(typeof cuestionData == "string") alias = cuestionData;
			if(typeof cuestionData == "object") alias = cuestionData.alias;
			
			if(private.reglas.estadoInicial[alias]){
				var estadoInicial = private.reglas.estadoInicial[alias];
				
				if(isArray(estadoInicial.reglas.regla)){
					for(var i = 0; i < estadoInicial.reglas.regla.length; i++){
						var tipo = estadoInicial.reglas.regla[i].tipo;
						private.parseRegla(tipo, cuestion, label, contenedor);
					}
				}else{
					var tipo = estadoInicial.reglas.regla.tipo;
					private.parseRegla(tipo, cuestion, label, contenedor);
				}
				
				if(estadoInicial.defaultValue != "" && cuestion != null) $(cuestion).val(estadoInicial.defaultValue);
			}
			
			if(private.reglas.flujo[alias]){
				var regla = private.reglas.flujo[alias];
				$(cuestion).change(function(){
					private.validateReglas(alias);
				});
			}
		},
		parseRegla: function(tipo, cuestion, label, contenedor){						
			if(tipo == "required"){ 
				$(cuestion).addClass("required");
			}
			if(tipo == "norequired") $(cuestion).removeClass("required");
			
			if(tipo == "hidden"){
				$(label).removeClass("visible");
				$(cuestion).removeClass("visible");
				$(contenedor).removeClass("visible");

				$(contenedor).addClass("hidden");				 
				$(label).addClass("hidden");
				$(cuestion).addClass("hidden");
			}
			if(tipo == "visible"){
				$(contenedor).addClass("hidden");				
				$(label).removeClass("hidden");
				$(cuestion).removeClass("hidden");
				
				$(label).addClass("visible");
				$(cuestion).addClass("visible");
				$(contenedor).addClass("visible");				
			}			
		},
		validateReglas: function(id){
			var valorSeleccionado = "";
			
			var defReglas = private.reglas.flujo[id];
			if(isArray(defReglas.valores.valor)){
				for(var i = 0; i < defReglas.valores.valor.length; i++){
					var valor = defReglas.valores.valor[i];
					
					
					if (private.tipoDominios[id] && (private.tipoDominios[id].tipo == "checkbox" || private.tipoDominios[id].tipo == "radio")){
						valorSeleccionado = $("[name=" + id + "]:checked").val();
					}else{
						valorSeleccionado = $("[name=" + id + "]").val();
					}
										
					if(valorSeleccionado == valor.valueText){
						private.validateAfectados(valor.afectados.afectado);
						break;
					}else if(valorSeleccionado != valor.valueText && valor.valueText == "*"){
						private.validateAfectados(valor.afectados.afectado);
					}
				}
			}else{
				if (private.tipoDominios[id] && (private.tipoDominios[id].tipo == "checkbox" || private.tipoDominios[id].tipo == "radio")){
					valorSeleccionado = $("[name=" + id + "]:checked").val();
				}else{
					valorSeleccionado = $("[name=" + id + "]").val();
				}
				
				if(valorSeleccionado == defReglas.valores.valor.valueText){
					private.validateAfectados(defReglas.valores.valor.afectados.afectado);
				}else if(defReglas.valores.valor.valueText == "*"){
					private.validateAfectados(defReglas.valores.valor.afectados.afectado);
				}
			}
		},
		validateAfectados: function(defAfectados){
			if(isArray(defAfectados)){
				for(var i = 0; i < defAfectados.afectado.length; i++){
					var afectado = defAfectados.afectado[i];
					private.validateAfectado(afectado);
				}
			}else{
				private.validateAfectado(defAfectados);
			}
		},
		validateAfectado: function(defAfectado){
			if(isArray(defAfectado.reglas.regla)){
				for(var i = 0; i < defAfectado.reglas.regla.length; i++){
					var regla = defAfectado.reglas.regla[i];
					private.parseAfectado(defAfectado, regla);
				}
			}else{
				var regla = defAfectado.reglas.regla;
				private.parseAfectado(defAfectado, regla);
			}
		},
		parseAfectado: function(defAfectado, regla){
			var afectado = null;
			var lblAfectado = null;
			var contenedor = $("#c_" + defAfectado.id);
			
			if (private.tipoDominios[defAfectado.id]){
				var tipo = private.tipoDominios[defAfectado.id].tipo;
				if(tipo == "radio" || tipo == "checkbox"){
					afectado = $("input[name=" + defAfectado.id + "]");
					lblAfectado = $("label.contains('" + defAfectado.id + "')");
				}else if(tipo == "select"){
					afectado = $("select[name=" + defAfectado.id + "]");
					lblAfectado = $("#lbl_" + defAfectado.id + "]");
				}
			}else{
				afectado = $("#" + defAfectado.id);
				lblAfectado = $("#lbl_" + defAfectado.id + "]");
			}
									
			private.parseRegla(regla.tipo, afectado, lblAfectado, contenedor);
			
		},
		renderDominio: function(defDominio){
			if(defDominio == null || defDominio.resultado == "KO"){
				//tratar errores
				return;
			}else{
				var alias = defDominio.dominio.alias;
				if (private.tipoDominios[alias]){
					var tipo = private.tipoDominios[alias].tipo;
					
					if(tipo == "radio" || tipo == "checkbox"){
						var lblContiguo = document.getElementById("lbl_" + alias);
						
						var divCuestion = document.getElementById("c_" + alias);
						if(divCuestion != null){
							$(divCuestion).empty();
						}else{
							divCuestion = document.createElement("div");
							divCuestion.setAttribute("id", "c_" + alias);
							
							$(lblContiguo).after(divCuestion);
						}
						
						$.each(defDominio.dominio.opcion, function(i, inputNode){
							var input = null;
							if(document.all && !window.opera && document.createElement){ // For no DOM2 compilant (IE6 and IE7)
								var temp = "<input ";
								temp += "type=\"" + tipo + "\" ";
								temp += "name=\"" + alias + "\" ";
								temp += "id=\"" + alias + "_" + inputNode.valor + "\" ";
								temp += "value=\"" + inputNode.valor + "\" ";
								temp += " />"
								input = document.createElement(temp);
																	
							}else if(document.createElement && document.createTextNode){
								input = document.createElement("input");
								input.setAttribute("type", tipo);
								input.setAttribute("name", alias);
								input.setAttribute("id", alias + "_" + inputNode.valor);
								input.setAttribute("value", inputNode.valor);
							}
							
							divCuestion.appendChild(input);						
							
							var lblInput = document.createElement("label");
							lblInput.setAttribute("for", alias);
							lblInput.setAttribute("id", "lbl_" + alias + "_" + inputNode.valor);
							
							var txtLblInput = document.createTextNode(inputNode.descripcion);
							lblInput.appendChild(txtLblInput);
							
							divCuestion.appendChild(lblInput);
						});
					}
				}else{
									
					var resSelect = document.getElementById(alias);
					while (resSelect.firstChild) resSelect.removeChild(resSelect.firstChild);
					
					/** Option por defecto **/
					resSelect.options.add(new Option(" " , ""));
					
					/** Resto de options **/
					$.each(defDominio.dominio.opcion, function(i, optionNode){
						resSelect.options.add(new Option(optionNode.descripcion , optionNode.valor));
					});
				}
			}
		}
	}
}