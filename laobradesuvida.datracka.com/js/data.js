var URI_BASE = "http://audi.comm.neuronics.es/audiclass/ServiciosNeuronics/";
var URI_PROXY = "proxyPHP/proxyGQ.php";
var token = "publicoAudiCumple2010.1";
var user = "cpproximity";
var password = "cp89ty";
var cargados = 0;
var errorColor = "#dc0039";
var resetColor = "#ABABAB";
var resetColorEspecial = "#ffffff";

//func para adquirir valores radio

function getCheckedValue(radioObj) {
	if(!radioObj)
		return "";
	var radioLength = radioObj.length;
	if(radioLength == undefined)
		if(radioObj.checked)
			return radioObj.value;
		else
			return "";
	for(var i = 0; i < radioLength; i++) {
		if(radioObj[i].checked) {
			return radioObj[i].value;
		}
	}
	return "";
}

function iniciarSesion(){
	var form = document.getElementsByTagName('form');

	var xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
	xml += "<peticion tipo=\"iniciarSesion\">";
	xml += 		"<encuesta token=\"" + token + "\" formatVersion=\"1\">";
	xml +=			"<usuario>" + user + "</usuario>";
	xml +=			"<password>" + password + "</password>";
	xml +=		"</encuesta>";
	xml += "</peticion>";
	
	obtenerSid("iniciarSesion.neu", xml);
}

function obtenerSid(petition, xml){
    var objHttp = getAjax();
    if(objHttp){
        if (objHttp.readyState==0 || objHttp.readyState==4){
            objHttp.open("POST", URI_PROXY, true);
            objHttp.setRequestHeader("Content-Type", "text/xml");	      
			objHttp.setRequestHeader("GQHost", URI_BASE + petition);
            objHttp.onreadystatechange = function(){
				if(objHttp.readyState==4){	
					if(objHttp.status == 200){
						var respuesta = objHttp.responseText;
						if (respuesta != ""){
							parseSidResponse(respuesta)
						}
					} 
				}
			}
            objHttp.send(xml);
        }
    }
}

function parseSidResponse(response){
	var doc;
    // Codigo para IE
    if (window.ActiveXObject){
        doc = new ActiveXObject("Microsoft.XMLDOM");
        doc.async = "false";
        doc.loadXML(response);
    } else {
        // Codigo para Mozilla, Firefox, Opera, etc.
        var parser = new DOMParser();
        doc = parser.parseFromString(response, "text/xml");
    }
	
    var x = doc.documentElement;
	var tipo = x.attributes.getNamedItem("resultado").nodeValue;
    var resultado = (tipo == "KO") ? false : true;
    
	var sid = 0;
	if(resultado){
		var lsid = doc.getElementsByTagName("sid");
    	if (lsid.length > 0) sid = lsid[0].firstChild.nodeValue;
	}
	
	document.getElementById("sid").value = sid;
	
	obtenerTodasRespuestasDominio();
	
}

function obtenerTodasRespuestasDominio(){
    var form = document.getElementsByTagName('form');	
    var aliases = document.getElementsByTagName('select');	
	var sid = document.getElementById("sid").value;
	
    for(var i = 0; i < aliases.length; i++){
        delay(250);
        
        var xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
        xml += "<peticion tipo=\"obtenerDominios\" sid=\"" +  sid + "\">";
		xml += 		"<encuesta token=\"" + form[0].id + "\" formatVersion=\"1\">";
		xml +=			"<alias>" + aliases[i].name + "</alias>";
		xml +=		"</encuesta>";
		xml += "</peticion>";

        obtenerRespuestasDominio("obtenerDominios.neu" , xml, aliases[i].name, "select");
    }
	
	//Obtener los dominios para crear los Checkbox Multiples
	var xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
	xml += "<peticion tipo=\"obtenerDominios\" sid=\"" +  sid + "\">";
	xml += 		"<encuesta token=\"" + form[0].id + "\" formatVersion=\"1\">";
	xml +=			"<alias>conductor_aficiones_1</alias>";
	xml +=		"</encuesta>";
	xml += "</peticion>";

	obtenerRespuestasDominio("obtenerDominios.neu" , xml, "conductor_aficiones_1", "multipleCheckBox");
	
	//Obtener los dominios para crear los Checkbox Multiples
	var xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
	xml += "<peticion tipo=\"obtenerDominios\" sid=\"" +  sid + "\">";
	xml += 		"<encuesta token=\"" + form[0].id + "\" formatVersion=\"1\">";
	xml +=			"<alias>conductor_lopdGeneral_1</alias>";
	xml +=		"</encuesta>";
	xml += "</peticion>";

	obtenerRespuestasDominio("obtenerDominios.neu" , xml, "conductor_lopdGeneral_1", "checkBox");
	

	
    
   	
	var interval = window.setInterval(function () {
        if(cargados >= 5){
            clearInterval(interval);
            obtenerEncuesta();
        }
    },2000);
    

}

function obtenerRespuestasDominio(petition, xml, inputName, inputType){
    var objHttp = getAjax();
    
    if(objHttp){
        if (objHttp.readyState==0 || objHttp.readyState==4){
            // Con el id de la respuesta, obtendremos el id del grupo asociado
            // del cual devoler las lista de respuestas
            objHttp.open("POST", URI_PROXY, true);
			objHttp.setRequestHeader("Content-Type", "text/xml");
			objHttp.setRequestHeader("GQHost", URI_BASE + petition);
            objHttp.onreadystatechange=
                function(){
                if(objHttp.readyState==4){
                    if(objHttp.status == 200){
                        var respuesta = objHttp.responseText;
                        var xmlDoc;

                        if(respuesta!=null && respuesta!=""){
                            try{ //Internet Explorer
                                xmlDoc = new ActiveXObject("Microsoft.XMLDOM");
                                xmlDoc.async = "false";
                                xmlDoc.loadXML(respuesta);
                            }catch(e){
                                try{ //Firefox, Mozilla, Opera, etc.
                                    parser = new DOMParser();
                                    xmlDoc = parser.parseFromString(respuesta, "text/xml");
                                }catch(e) {
                                    alert(e.message)
                                }
                            }													

                            // Carreguem les OPTIONS als seus combos
                            renderOptions(xmlDoc, inputName, inputType);
							cargados = cargados + 1;
                        }
                    }
                }
            }
            objHttp.send(xml);
        }
    }
}


function obtenerRespuestaDominioAsociado(selectPadre, selectHijo){
    var form = document.getElementsByTagName('form');
	var sid = document.getElementById("sid").value;
    
    while (selectHijo.firstChild) selectHijo.removeChild(selectHijo.firstChild);
    var newopc = document.createElement("option");
    newopc.setAttribute("value", "");
    var optionNode = document.createTextNode("...cargando...");
    newopc.appendChild(optionNode);
    selectHijo.appendChild(newopc);
    
	var xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
	xml += "<peticion tipo=\"obtenerDominios\" sid=\"" +  sid + "\">";
	xml += 		"<encuesta token=\"" + form[0].id + "\" formatVersion=\"1\">";
	xml +=			"<alias>" + selectPadre.name + "</alias>";
    xml +=  		"<id>" + selectPadre.value + "</id>";
	xml +=		"</encuesta>";
	xml += "</peticion>";

    obtenerRespuestasDominio("obtenerDominios.neu", xml, selectHijo.name, "select");
    
}

function obtenerRespuestaDominioAsociadoEspecial(selectPadre, selectHijo){
    var form = document.getElementsByTagName('form');
	var sid = document.getElementById("sid").value;
    if(selectPadre.value == "127067"){
        contenedor = document.getElementById("vfuturo_formaPagoDetalle_1_1_select");
    }else{
        contenedor = document.getElementById("vfuturo_formaPagoDetalle_1_2_select");
    }
    hijo = document.createElement("select");
    hijo.name = selectHijo;
    hijo.id = "vfuturo_formaPagoDetalle_1";
    //hijo.setAttribute('class','inpSelect');
    newopc = document.createElement("option");
	newopc.setAttribute("value", "0");
	optionNode = document.createTextNode("-- seleccione --");
	newopc.appendChild(optionNode);
	hijo.appendChild(newopc);
    contenedor.appendChild(hijo);
    while (hijo.firstChild) hijo.removeChild(hijo.firstChild);
    var newopc = document.createElement("option");
    newopc.setAttribute("value", "");
    var optionNode = document.createTextNode("...cargando...");
    newopc.appendChild(optionNode);
    hijo.appendChild(newopc);

	var xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
	xml += "<peticion tipo=\"obtenerDominios\" sid=\"" +  sid + "\">";
	xml += 		"<encuesta token=\"" + form[0].id + "\" formatVersion=\"1\">";
	xml +=			"<alias>" + selectPadre.name + "</alias>";
    xml +=  		"<id>" + selectPadre.value + "</id>";
	xml +=		"</encuesta>";
	xml += "</peticion>";

    obtenerRespuestasDominio("obtenerDominios.neu", xml, hijo.name, "select");

}

function delay(millis){
    var dini = new Date();
    var curdate = null;
    do {curdate = new Date();}
    while(curdate - dini < millis);
}

function getAjax(){	
	var ajax;
    if (window.XMLHttpRequest) { // IE7, Mozilla, Safari,...
        ajax = new XMLHttpRequest();
        if (ajax.overrideMimeType) {
            ajax.overrideMimeType('text/xml');
        }
    } else if (window.ActiveXObject) { // IE
        try {
            ajax = new ActiveXObject("Msxml2.XMLHTTP");
        } catch (e) {
            try {
                ajax = new ActiveXObject("Microsoft.XMLHTTP");
            } catch (e) {}
        }
    }
    return ajax;
}

function renderOptions(xmlDoc, idInput, inputType){

	if(inputType == "select"){
	    var valueOption, textOption, optionNode;
		var newopc, selectTag;
		var loptions, option;
		var nodeVal, nodeDesc;
	
	   var titleEng = new Array();
            titleEng['148814'] = ['Mr.']; 
            titleEng['148815'] = ['Mrs.']; 
	
	
		selectTag = document.getElementById(idInput);
		while (selectTag.firstChild) selectTag.removeChild(selectTag.firstChild);
	
		if (selectTag != null){
			newopc = document.createElement("option");
			newopc.setAttribute("value", "0");
			if($('#lng_en').length > 0)optionNode = document.createTextNode("-- select --");
			else optionNode = document.createTextNode("-- seleccione --");
			newopc.appendChild(optionNode);
			selectTag.appendChild(newopc);
			
			loptions = xmlDoc.getElementsByTagName("opcion");
		  
			for (ind=0; ind < loptions.length; ind++){
				option = loptions[ind];
	
				nodeVal = option.childNodes[0];   //-- valor
				valueOption = nodeVal.childNodes[0];
				nodeDesc = option.childNodes[1];   //-- description
                textOption = nodeDesc.childNodes[0];
	
				newopc = document.createElement("option");
				newopc.setAttribute("value", valueOption.nodeValue);
				if($('#lng_en').length > 0 && idInput == "conductor_sexo_1")optionNode = document.createTextNode(titleEng[valueOption.nodeValue]);
				else optionNode = document.createTextNode(textOption.nodeValue);
				newopc.appendChild(optionNode);
				selectTag.appendChild(newopc);
			}
		} else{
			alert("No existe Content: " + idInput);
		}
	}else if (inputType == "multipleCheckBox"){
	
        var option, radioNode, txtNode, divNode, labelNode;
		var fieldset = document.getElementById(idInput + "_div");
		if(fieldset != null){
		  var loptions = xmlDoc.getElementsByTagName("opcion");
		  var txtRadios = "";
		  var hobbies =  new Array;
                hobbies['145320'] = ['Motorsports'];
                hobbies['145321'] = ['Nature'];
                hobbies['145323'] = ['Cinema'];
                hobbies['145327'] = ['Gastronomy'];
                hobbies['145329'] = ['Golf'];
                hobbies['145330'] = ['Horse riding'];
                hobbies['145334'] = ['Reading'];
                hobbies['145335'] = ['Music'];
                hobbies['145337'] = ['Others'];
                hobbies['145340'] = ['Skiing'];
                hobbies['145341'] = ['Snowboarding'];
                hobbies['145342'] = ['Theatre'];
                hobbies['145343'] = ['Tennis'];
                hobbies['145345'] = ['Sailing'];
                hobbies['145346'] = ['Travel'];
		for (ind=0; ind < loptions.length; ind++){
				option = loptions[ind];

				nodeVal = option.childNodes[0];   //-- valor
				valueOption = nodeVal.childNodes[0];
				nodeDesc = option.childNodes[1];   //-- description
				textOption = nodeDesc.childNodes[0];
				
				if($('#lng_en').length > 0) {
                    //alert($('#lng_en').length);
                    txtRadios += "<div class='floatLeft clearfix w120px mb10px mr5px'><div class='floatLeft w20px'><input name='"+idInput+"' value ='"+valueOption.nodeValue+"' type='checkbox' class='' /></div><div class='floatLeft labelInt w85px'><label>"+ hobbies[valueOption.nodeValue] +"</label></div></div>";
                    
                }else{
                    txtRadios += "<div class='floatLeft clearfix w120px mb10px mr5px'><div class='floatLeft w20px'><input name='"+idInput+"' value ='"+valueOption.nodeValue+"' type='checkbox' class='' /></div><div class='floatLeft labelInt w85px'><label>"+textOption.nodeValue+"</label></div></div>";    
                }

				
            }
            fieldset.innerHTML = txtRadios;
		
		}
		}else if (inputType == "checkBox"){

        var option, radioNode, txtNode, divNode, labelNode;
		var fieldset = document.getElementById(idInput + "_div");
		if(fieldset != null){
		  var loptions = xmlDoc.getElementsByTagName("opcion");
		  var txtRadios = "";
        if(idInput == "conductor_lopdGeneral_1"){
    		for (ind=0; ind < loptions.length; ind++){
    				option = loptions[ind];
    
    				nodeVal = option.childNodes[0];   //-- valor
    				valueOption = nodeVal.childNodes[0];
    				nodeDesc = option.childNodes[1];   //-- description
    				textOption = nodeDesc.childNodes[0];
    
    				txtRadios += "<input name='"+idInput+"' name='"+idInput+"' value ='"+valueOption.nodeValue+"' type='checkbox' class='' />";
    				break;
                }
        }
            fieldset.innerHTML = txtRadios;

		}
			
     }else if(inputType == "radio"){
		var option, radioNode, txtNode, divNode, labelNode;
		var fieldset = document.getElementById(idInput + "_div");
		if(fieldset != null){
		
			var loptions = xmlDoc.getElementsByTagName("opcion");
			var txtRadios = "";
			
                for (ind=0; ind < loptions.length; ind++){
				option = loptions[ind];

				nodeVal = option.childNodes[0];   //-- valor
				valueOption = nodeVal.childNodes[0];
				nodeDesc = option.childNodes[1];   //-- description
				textOption = nodeDesc.childNodes[0];

				txtRadios += "<div class='radio clearfix'><div class='floatLeft'><input type='radio' name ='"+idInput+"' value ='"+valueOption.nodeValue+"'></div><div class='floatLeft' style='padding-top: 7px;'><label>"+textOption.nodeValue+"</label></div></div>";

				/*radioNode.setAttribute("type", "radio");
				radioNode.setAttribute("value", valueOption.nodeValue);
				radioNode.setAttribute("id", idInput);
				radioNode.setAttribute("name", idInput);

				txtNode = document.createTextNode(textOption.nodeValue);

				divNode = document.createElement("div");
				divNode.setAttribute("class", "radio");
				divNode.setAttribute("className", "radio");

				labelNode = document.createElement("label");
				labelNode.appendChild(txtNode);

				divNode.appendChild(radioNode);
				divNode.appendChild(labelNode);

				fieldset.appendChild(divNode);*/
			
            }
            
			fieldset.innerHTML = txtRadios;
		}
	}
	
	//Cufon.replace('label');
}


function obtenerEncuesta(){
	var form = document.getElementsByTagName('form');
	var sid = document.getElementById("sid").value;
	//var codeClient = document.getElementById("codeClient").value;
		
	var xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
	xml += "<peticion tipo=\"obtenerEncuesta\" sid=\"" +  sid + "\">";

    /*if (clicked_on == 'pedidos') {
        xml += 		"<encuesta token=\"Prelanzamiento_A5SPB.1\" formatVersion=\"1\">";
    } else if (clicked_on == 'informacion') { */
        xml += 		"<encuesta token=\"" + form[0].id + "\" formatVersion=\"1\">";
    //}
//	xml +=			"<code>" + codeClient + "</code>";
	xml +=		"</encuesta>";
	xml += "</peticion>";
	
	obtenerDatosEncuesta("obtenerEncuesta.neu", xml);
}

function obtenerDatosEncuesta(petition, xml){
    var objHttp = getAjax();
    if(objHttp){
        if (objHttp.readyState==0 || objHttp.readyState==4){
            objHttp.open("POST", URI_PROXY, true);
            objHttp.setRequestHeader("Content-Type", "text/xml");	      
			objHttp.setRequestHeader("GQHost", URI_BASE + petition);
            objHttp.onreadystatechange=
                function(){
					if(objHttp.readyState==4){	
							if(objHttp.status == 200){
								var respuesta = objHttp.responseText; 
								parseEncuesta(respuesta);
							} 
					}
            }
            objHttp.send(xml);
        }
    }
}

function parseEncuesta(respuesta){

	//var codeClient = document.getElementById("codeClient").value;
	// Codigo para IE
    if (window.ActiveXObject){
        var doc = new ActiveXObject("Microsoft.XMLDOM");
        doc.async = "false";
        doc.loadXML(respuesta);
    } else {
        // Codigo para Mozilla, Firefox, Opera, etc.
        var parser = new DOMParser();
        var doc = parser.parseFromString(respuesta, "text/xml");
    }
	
	var encuestaList = doc.getElementsByTagName("encuesta");
	
	if(encuestaList != null && encuestaList.length > 0){		
		var encuesta = encuestaList[0];
		
		//Obtenemos el idContacto obtenido por precarga
		var idContacto = encuesta.attributes.getNamedItem("idContacto").nodeValue;
		//document.getElementById("codeClient").value = idContacto;
        
        for(var i = 0; i < encuesta.childNodes.length; i++){
			node = encuesta.childNodes[i];
			var nodeForm = eval("document.forms[0]." + node.nodeName);
			var indexSelect = 0;
			if(nodeForm.type == "select-one"){
				for(var j = 0; j < nodeForm.length; j++){
                    // switch (clicked_on) {
                    //    case 'informacion':
                            if(nodeForm.options[j].value == node.childNodes[0].nodeValue){
                                indexSelect = j;
                                //nodeForm.options[j].selected = true;
                            }
  
				}
				document.getElementById(node.nodeName).selectedIndex = indexSelect;
				indexSelect = 0;
			}else if(nodeForm.type == "checkbox" || nodeForm.type == "radio"){
				for(var j = 0; j < nodeForm.length; j++){
					if(nodeForm[j].value == node.childNodes[0].nodeValue){
						nodeForm[j].checked = true;
					}
				}
			}else{
				nodeForm.value = node.childNodes[0].nodeValue;
			}
		}
	
		//Campos especiales
		if(document.getElementById("conductor_FechaNac_1").value != ""){
			var fechaNacimiento = document.getElementById("conductor_FechaNac_1").value.split("/");
			document.getElementById("diaNac").value = fechaNacimiento[0];
			document.getElementById("mesNac").value = fechaNacimiento[1];
			document.getElementById("anoNac").value = fechaNacimiento[2];
		}	
	}
		//Ocultar el Loading
	//document.getElementById("loading").style.display = "none";
    $("div#loading").fadeOut(300,function(){
        $(this).remove();

        $("#outerWrapper").fadeIn(300);
    });
	//showSelects();
}

function enviarDatos(form){
	//Mostramos mensaje de envio de datos
	//document.getElementById("enviandoDatos").style.display = "block";
	if(resetear) resetStyles();
	generarDatosPrevios();
    var xml = generarEncuesta(form);

    sendXml("guardarEncuesta.neu", xml, funcionErrores, funcionOk);


}

function generarDatosPrevios(){
	//Fecha de nacimiento
	var dia = document.getElementById("diaNac").value;
	var mes = document.getElementById("mesNac").value;
	var ano = document.getElementById("anoNac").value;
	if(dia != "" && mes != "" && ano != "") document.getElementById("conductor_FechaNac_1").value = dia + "/" + mes + "/" + ano;
}

function generarEncuesta(form) {
	var sid = document.getElementById("sid").value;
	//var codeClient = document.getElementById("codeClient").value;
	//if(codeClient == 0) codeClient = "";
	
	var xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
    xml += "<peticion tipo=\"guardarEncuesta\" sid=\"" + sid + "\">";
	/*if(codeClient != 0)	xml += 		"<encuesta token=\"" + form.id + "\" idContacto=\"" + codeClient+ "\" formatVersion=\"1\">";
	else */xml += 		"<encuesta token=\"" + form.id + "\" idContacto=\"\" formatVersion=\"1\">";
	      
    for (i=0; i<form.length; i++) {
    	if(form.elements[i].disabled == false){
            if (form.elements[i].type == "select-multiple") {
                var sel = form.elements[i];
                var opt_selected = new Array();
                var index = 0;
                for (var j=0; j < sel.options.length;j++){
                    if (sel.options[j].selected && sel.options[j].value != "0"){
                        xml += "<" + form.elements[i].name + " numContestacion=\"" + index + "\">" + sel.options[j].value + "</" + form.elements[i].name + ">";
                        index++;
                    }
                }
            } else {
                if (form.elements[i].type == "text" || form.elements[i].type == "textarea" || form.elements[i].type == "hidden") {
					if(/*form.elements[i].name != "codeClient" && */form.elements[i].name != "sid" && form.elements[i].name != "diaNac" && form.elements[i].name != "mesNac" && form.elements[i].name != "anoNac"){
						if (form.elements[i].value != null && form.elements[i].value.length > 0)
							xml += "<" + form.elements[i].name + ">" + getCDataValue(form.elements[i].value) + "</" + form.elements[i].name + ">";
					}
                } else if ((form.elements[i].type == "checkbox" || form.elements[i].type == "radio") && form.elements[i].checked && form.elements[i].name != 'q_two') {
	                    	xml += "<" + form.elements[i].name + ">" + form.elements[i].value + "</" + form.elements[i].name + ">";
	            } else if (form.elements[i].type == "select-one") {
                    var sel = form.elements[i];
                    if (sel.selectedIndex > -1 && sel.options[sel.selectedIndex].value.length > 0 && sel.options[sel.selectedIndex].value != "0"){
                        xml += "<" + form.elements[i].name + ">" + sel.options[sel.selectedIndex].value + "</" + form.elements[i].name + ">";
                    } 
                }
            }
    	}
    }
    xml += 		"</encuesta>";
	xml += "</peticion>";
    
    return xml;
}

function getCDataValue(str){

    if(str.indexOf("<")!=-1 || str.indexOf(">")!=-1 || str.indexOf("&")!=-1){
        return "<![CDATA[" + str + "]]>";
    }else{
        return str;
    }
}

function sendXml(petition, xml, funcionErrores, funcionOk){
    var objHttp = getAjax();
    if(objHttp){
        if (objHttp.readyState==0 || objHttp.readyState==4){
            objHttp.open("POST", URI_PROXY, true);
            objHttp.setRequestHeader("Content-Type", "text/xml");	      
			objHttp.setRequestHeader("GQHost", URI_BASE + petition);
            objHttp.onreadystatechange=
                function(){
					if(objHttp.readyState==4){	
							if(objHttp.status == 200){
								var respuesta = objHttp.responseText;
								parseErrores(respuesta, funcionErrores, funcionOk)
							} 
					}
            }
            objHttp.send(xml);
        }
    }
}

function parseErrores(errores, funcionErrores, funcionOk){
    // Codigo para IE
    if (window.ActiveXObject){
        var doc = new ActiveXObject("Microsoft.XMLDOM");
        doc.async = "false";
        doc.loadXML(errores);
    } else {
        // Codigo para Mozilla, Firefox, Opera, etc.
        var parser = new DOMParser();
        var doc = parser.parseFromString(errores, "text/xml");
    }
	
    var x = doc.documentElement;
    
    var tagGrabacion = doc.getElementsByTagName("grabacionEncuesta")[0];
	
	if(tagGrabacion != null){
		
		var token = tagGrabacion.attributes.getNamedItem("token").nodeValue;
		var status = tagGrabacion.attributes.getNamedItem("status").nodeValue;
			
		var aliases = new Array();
	
		if (status == 'OK'){
		  var idContacto = tagGrabacion.attributes.getNamedItem("idContacto").nodeValue;
			// La encuesta se ha guardado correctamente
			if (funcionOk){
				funcionOk(idContacto);
			}
		} else {
			if (funcionErrores){
				funcionErrores(doc, token);
			}
		}
	}else{
		document.getElementById("errores").innerHTML = "<p>Se ha producido un error interno. Intentelo de nuevo m&aacute;s tarde, gracias.</p>";
		//Cufon.replace("#content p");
	}
}

function funcionOk(data){

            

        //$.get('./despedida.php',function(data){
        //$('div#forms_container').html(data);
			//Cufon.replace("#despedida p");
			//Cufon.replace("#footer p");
		  //$('div#errores').html("Su formulario se envió con éxito!");
		    //setMaxDigits(19);
            if($('#lng_en').length > 0)lng = 'ENG';
            else lng = 'ES';
            //key = new RSAKeyPair("6aa86c39bbe678f7f7967a587a1149", "","8b46ab8a951615d07b66bdd2420f8b1");
            //$.get("demo.php", {data}, function(laKey){
            $.get("encriptador.php?encrypt="+data, {}, function(laKey){
                name = $('#conductor_nombre_1').val();                 
                surname = $('#conductor_apellido1_1').val();
                //alert(data);
                //alert(laKey);
                window.location = "http://laobradesuvida.audiclass.com?id_contacto="+laKey+"&procedencia=FORMULARIO&idioma="+lng+"&token=publicoAudiCumple2010.1&name="+name+"&surname="+surname;                 
            },'text');
            
            
           /* key = new RSAKeyPair("MFwwDQYJKoZIhvcNAQEBBQADSwAwSAJBANngTC5Eg9xY9b4XspnYuYbesnNhU7xe3B4kY6f5T2apJIat0RYUojAygGeLXfHmxbiSQN7PsZFZx00o8sJ3DocCAwEAAQ==", "","8b46ab8a951615d07b66bdd2420f8b1");
            
            laputakey = encryptedString(key,'GQ_32010595');
            alert(laputakey);
		    window.location = "http://laobradesuvida.audiclass.com?id_contacto="+laputakey+"&procedencia=FORMULARIO&idioma=es&token=publicoAudiCumple2010.1"
		  
		   */
		//id_contacto(encriptado)
		//procedencia=FORMULARIO
		//token=publicoAudiCumple2010.1
		//idioma=[en|es]

//http://laobradesuvida.audiclass.com?id_contacto=xxxxxxx&procedencia=FORMULARIO&idioma=en&token=publicoAudiCumple2010.1
          //alert(data);
          //window.location = "/";	
			
    //});    
    
    
}

function funcionErrores(doc, token){
	
    // El intento de grabacion del cuestionario ha generado errores
    var cuestionario  = doc.getElementsByTagName("errorCuestionario");
    var dominio       = doc.getElementsByTagName("errorValidacionRespuestaDominio");
    var cuestion      = doc.getElementsByTagName("errorValidacionCuestionDesconocida");
    var errorLibre    = doc.getElementsByTagName("errorValidacionContestacionLibre");			
    var longitud      = doc.getElementsByTagName("errorLongitudContestacionLibre");
    var multiplicidad = doc.getElementsByTagName("errorValidacionNumeroRespuestas");		
		
    procesarErrores(cuestionario, token, "Error en la definicion del cuestionario");
    procesarErrores(dominio, token, "Error en los dominios de respuesta");
    procesarErrores(cuestion, token, "Error de cuestion desconocida");
    procesarErrores(errorLibre, token, "Error de formato del texto");
    procesarErrores(longitud, token, "Error de longitud del texto");
    procesarErrores(multiplicidad, token, "Error de numero de respuestas validas");
}


// Esta funcion, a partir de una lista errores en nodos DOM, obtiene el elemento del formulario
// en el que se encuentra la causa del error
// Parametros: errores -> Array de elementos DOM conteniendo un error de algun tipo de los definidos
//var global pel control d'estils amb els errors

var resetear = false;
function procesarErrores(errores, token, texto){
	var isErrores = false;
	
    if (errores.length > 0){
		
        for (i = 0; i < errores.length; i++){
            var error = errores[i];
			var alias = error.getElementsByTagName("alias");
						
			if (alias != null && alias.length > 0){
				
				var nombreCampo = alias[0].firstChild.nodeValue;
				
				//Cambiar format campos con errores
				var nodeError = document.getElementById(nombreCampo);
				if (nodeError != null && nombreCampo != "particular_fechaNac_1" && nombreCampo != "particular_sexo_1" && nombreCampo != "particular_provincia_1" && nombreCampo != "vactual_marca_1" && nombreCampo != "vactual_modelo_1" && nombreCampo != "vactual_carroceria_1" && nombreCampo != "vfuturo_marca_1" && nombreCampo != "vfuturo_modelo_1" && nombreCampo != "vfuturo_marca_2" && nombreCampo != "vfuturo_modelo_2" && nombreCampo != "vfuturo_marca_3" && nombreCampo != "vfuturo_modelo_3"){
					cambiarFormatoError(nombreCampo);
					isErrores = true;
				}else if(nombreCampo == "particular_sexo_1" || nombreCampo == "particular_provincia_1" || nombreCampo == "vactual_marca_1" || nombreCampo == "vactual_modelo_1" || nombreCampo == "vactual_carroceria_1" || nombreCampo == "vfuturo_marca_1" || nombreCampo == "vfuturo_modelo_1" || nombreCampo == "vfuturo_marca_2" || nombreCampo == "vfuturo_modelo_2" || nombreCampo == "vfuturo_marca_3" || nombreCampo == "vfuturo_modelo_3"){
					cambiarFormatoError(nombreCampo + "_select");
					isErrores = true;
				}else if(nombreCampo == "particular_fechaNac_1"){
					cambiarFormatoError("diaNac");
					cambiarFormatoError("mesNac");
					cambiarFormatoError("anoNac");
					isErrores = true;
				}
			}
        }
    }
	if(isErrores){
	   if($('#lng_en').length > 0)document.getElementById("errores").innerHTML = "<p>You must fill all the fields above</p>";  
	   else document.getElementById("errores").innerHTML = "<p>Debe rellenar todos los campos.</p>";					
	}
	//Cufon.replace('#content p'); //Para reemplazar las fuentes

	//Ocultamos el mensaje de envio de datos
	document.getElementById("enviandoDatos").style.display = "none";

}

// Modifiquem l'estil dels camps que tenen error
function cambiarFormatoError(elemento) {
	
	document.getElementById(elemento).style.borderColor = errorColor;
  
    resetear = true;
}

function resetStyles(){

	var inputs = document.getElementsByTagName("input");
	var selects = document.getElementsByTagName("select");
    var textareas = document.getElementsByTagName("textarea");

	
	document.getElementById("errores").innerHTML ="";
	//radios	
	document.getElementById("conductor_aficiones_1_div").style.border = "0 solid";

	//telefonos
    //document.getElementById("telefono_div").style.borderColor = resetColorEspecial;
    
    for(i=0;i<inputs.length;i++){

		if(inputs[i].type == "text"){
            if(document.getElementById(inputs[i].id) != null) document.getElementById(inputs[i].id).style.borderColor = resetColor;
        }

	}

	for(i=0;i<selects.length;i++){
		    //alert(selects.length);
            if(document.getElementById(selects[i].id) != null) document.getElementById(selects[i].id /*+ "_select"*/).style.borderColor = resetColor;
	

    }

	for(i=0;i<textareas.length;i++){
		if(document.getElementById(textareas[i].id) != null) document.getElementById(textareas[i].id).style.borderColor = resetColor;
	}
	

}

