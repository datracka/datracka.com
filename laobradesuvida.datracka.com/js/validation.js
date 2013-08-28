var no_letra = /\D/g;
var no_numero = /\d/g;

function isName(text){
	//var regExp = /^\s*[A-Za-zÑñÇçáéíóúäëïöüàèìòùâêîôû·ÁÉÍÓÚÄËÏÖÜÀÈÌÒÙÂÊÎÔÛªº\.]+(\s)?/;
	var regExp = /^\s*[A-Za-zÑñÇçáéíóúäëïöüàèìòùâêîôû·ÁÉÍÓÚÄËÏÖÜÀÈÌÒÙÂÊÎÔÛªº\.\- ]+\s*$/;
	return regExp.test(text);
}

function isLatinCharset(text){
	//var regExp = /^\s*[A-Za-zÑñÇçáéíóúäëïöüàèìòùâêîôû·ÁÉÍÓÚÄËÏÖÜÀÈÌÒÙÂÊÎÔÛªº\.]+(\s)?/;
	var regExp = /^\s*[A-Za-zÑñÇçáéíóúäëïöüàèìòùâêîôû·ÁÉÍÓÚÄËÏÖÜÀÈÌÒÙÂÊÎÔÛªº\. ]+\s*$/;
	return regExp.test(text);
}


function isNumberSymbol(text) {
    var regExp = /^\s*[0-9A-Za-zÑñÇçáéíóúäëïöüàèìòùâêîôû·ÁÉÍÓÚÄËÏÖÜÀÈÌÒÙÂÊÎÔÛªº\.]+(\s)?/;
	return regExp.test(text);    
}

function isNumber(text){
	var regExp =/^\d+$/;
	return regExp.test(text);
}

function isEmail(text){
	var regExp = /^([0-9a-zA-Z]+[-._+&amp;])*[0-9a-zA-Z]+@([-0-9a-zA-Z]+[.])+[a-zA-Z]{2,6}$/;
	return regExp.test(text);
}

function isDate(text){
	var regExp = /^((0[1-9]|[12][0-9]|3[01])\/(0[1-9]|1[0-2])\/(19[0-9][0-9]|2[0-9][0-9][0-9]))$/;
	return regExp.test(text);
}

function isDia(text){
	var regExp = /^(0[1-9]|[12][0-9]|3[01])$/;
	return regExp.test(text);
}
function isMes(text){
	var regExp = /^(0[1-9]|1[0-2])$/;
	return regExp.test(text);
}
function isAno(text){
	var regExp = /^(19[0-9][0-9]|2[0-9][0-9][0-9])$/;
	return regExp.test(text);
}

function isTelefono(text){
	var regExp = /^[9]{1}[0-9]{8}$/;
	return regExp.test(text);
}

function isMovil(text){
	var regExp = /^[6]{1}[0-9]{8}$/;
	return regExp.test(text);
}

function isAnoNoSuperiorActual(text){
	var result = isNumber(text);
	if(result && text.length == 4){
		var currentTime = new Date();
		var currentYear = currentTime.getFullYear();
		
		var year = parseInt(text);
		
		if(year <= currentYear && year > 1929) return true;
		else return false;
	}else{
		return false;
	}
}

function isMatricula(text){
	//var regExp = /^(\d{4}[A-Z]{3})|([A-Z]{1,2}\d{4}[A-Z]{1,2})$/;
	//var regExp = /^([A-Z]{0,2})\d{4}[A-Z]{1,3}$/;
	var regExp = /^^(((VI|AB|A|AL|AV|BA|PM|IB|B|BU|CC|CA|CS|CR|CO|C|CU|GE|GI|GR|GU|SS|H|HU|J|LE|L|LO|LR|LU|M|MA|MU|NA|OR|OU|O|P|GC|PO|SA|TF|S|SG|SE|SO|T|TE|TO|V|VA|BI|ZA|Z|CE|ML)\d{4}(?!.*(AA|EE|II|OO|UU))[A-Z]{1,2})|([E]\d{4}(?!.*(A|E|I|O|U|Q))[A-Z]{3})|(\d{4}(?!.*(A|E|I|O|U|Q))[A-Z]{3})|((vi|ab|a|al|av|ba|pm|ib|b|bu|cc|ca|cs|cr|co|c|cu|ge|gi|gr|gu|ss|h|hu|j|le|l|lo|lr|lu|m|ma|mu|na|or|ou|o|p|gc|po|sa|tf|s|sg|se|so|t|te|to|v|va|bi|za|z|ce|ml)\d{4}(?!.*(aa|ee|ii|oo|uu))[a-z]{1,2})|([e]\d{4}(?!.*(a|e|i|o|u|q))[a-z]{3})|(\d{4}(?!.*(a|e|i|o|u|q))[a-z]{3}))$/;
	return regExp.test(text);
}

function isCP(text){
    var regExp = /^([0-9]{5})$/;
	return regExp.test(text);
}


function checkForm(){
	var form = document.forms[0];
	var isErrors = false;
	var isErrorLOPD = false;
	
	resetStyles();

	if(form.conductor_sexo_1.options[form.conductor_sexo_1.selectedIndex].value == 0){
	   //alert(errorColor);
	    //document.getElementById("conductor_sexo_1_select").style.border = "1px solid";
		document.getElementById("conductor_sexo_1").style.borderColor = errorColor;
		isErrors =true;
	}
		
	if(form.conductor_nombre_1.value == ""){
		document.getElementById("conductor_nombre_1").style.borderColor = errorColor;
		isErrors = true;
	}
    if(form.conductor_nombre_1.value != "" && !isName(form.conductor_nombre_1.value)){
		document.getElementById("conductor_nombre_1").style.borderColor = errorColor;
		isErrors = true;
	}
	
	if(form.conductor_apellido1_1.value == ""){
		document.getElementById("conductor_apellido1_1").style.borderColor = errorColor;
		isErrors = true;
	}
    if(form.conductor_apellido1_1.value != "" && !isName(form.conductor_apellido1_1.value)){
		document.getElementById("conductor_apellido1_1").style.borderColor = errorColor;
		//$("#conductor_apellido1_1").addClass("errorStyle");
		//alert($("#particular_apellido1_1").val());
		isErrors = true;
	}
	
    if(form.diaNac.value == "" || form.mesNac.value == "" || form.anoNac.value == "") {
        document.getElementById("diaNac").style.borderColor = errorColor;
        document.getElementById("mesNac").style.borderColor = errorColor;
        document.getElementById("anoNac").style.borderColor = errorColor;
		isErrors = true;    
    }
    
	if(form.diaNac.value != "" && (form.diaNac.value.length < 2 || !isNumber(form.diaNac.value) || !isDia(form.diaNac.value))){
		document.getElementById("diaNac").style.borderColor = errorColor;
		isErrors = true;
	}
	if(form.mesNac.value != "" && (form.mesNac.value.length < 2 || !isNumber(form.mesNac.value) || !isMes(form.mesNac.value))){
		document.getElementById("mesNac").style.borderColor = errorColor;
		isErrors = true;
	}
	if(form.anoNac.value != "" && (form.anoNac.value.length < 4 || !isNumber(form.anoNac.value) || !isAno(form.anoNac.value) || !isAnoNoSuperiorActual(form.anoNac.value))){
		document.getElementById("anoNac").style.borderColor = errorColor;
		isErrors = true;
	}

	if(form.conductor_provincia_1.options[form.conductor_provincia_1.selectedIndex].value == 0){
		document.getElementById("conductor_provincia_1").style.borderColor = errorColor;
		isErrors = true;
	}

	if(form.vcomprado_modelo_1.options[form.vcomprado_modelo_1.selectedIndex].value == 0){
		document.getElementById("vcomprado_modelo_1").style.borderColor = errorColor;
		isErrors = true;
	}
	
	if(getCheckedValue(form.conductor_aficiones_1) == "" ){
		document.getElementById("conductor_aficiones_1_div").style.border = "1px solid";
		document.getElementById("conductor_aficiones_1_div").style.borderColor = errorColor;
		isErrors = true;
	}
	                
	
	if(!form.conductor_lopdGeneral_1.checked){
		isErrorLOPD = true;
	}else{
		isErrorLOPD = false;
	}
	
	if(isErrors && isErrorLOPD){
		if($('#lng_en').length > 0)document.getElementById("errores").innerHTML = "<p>You must accept the general conditions.<br/>Please check the marked fields and remember to accept the general condition</p>";
		else document.getElementById("errores").innerHTML = "<p>Por favor, revise los campos marcados. <br />Y recuerde que debe aceptar las condiciones generales</p>";
		//Cufon.replace("#content p");
	}else if(isErrors && !isErrorLOPD){
		if($('#lng_en').length > 0)document.getElementById("errores").innerHTML = "<p>Please check the marked fields and remember to accept the general condition</p>";
		else document.getElementById("errores").innerHTML = "<p>Por favor, revise los campos marcados.</p>";
		//Cufon.replace("#content p");		
	}else if(!isErrors && isErrorLOPD){
		if($('#lng_en').length > 0)document.getElementById("errores").innerHTML = "<p>You must accept the general conditions</p>";
		else document.getElementById("errores").innerHTML = "<p>Debe aceptar las condiciones generales</p>";
		//Cufon.replace("#content p");
	/*}else if(!isErrors && !isErrorLOPD && !isPaso2){
		document.getElementById("errores").innerHTML = "<p>Si completa todos los campos podremos atenderle con mayor eficacia.</p>";
		Cufon.replace("#content p");
		isPaso2 = true; */
	}
    else{       
        if($('#lng_en').length > 0)document.getElementById("errores").innerHTML = "<p>Sending data...</p>";
		else document.getElementById("errores").innerHTML = "<p>Enviando datos...</p>";
	
		//Cufon.replace("#content p");		
		enviarDatos(form);
	}
}