function isArray(o) {
	return Object.prototype.toString.call(o) === '[object Array]';
}

function isChecked(input){
	for(var i = 0; i < input.length; i++){
		if(input[i].checked) return true;
	}

	return false;
}

function textChecked(input){
	for(var i = 0; i < input.length; i++){
		if(input[i].checked) return radioButton[i].value;
	}

	return "";
}

function indexChecked(input){
	for(var i = 0; i < input.length; i++){
		if(input[i].checked) return i;
	}

	return -1;
}

function getIndexValue(sel, valor){
	var index = -1;
	for(var i=0; i < sel.length; i++){
		if(sel.options[i].value == valor) index = i;
	}

	return index;
}

function getIndexText(sel, valor){
	var index = -1;
	for(var i=0; i < sel.length; i++){
		if(sel.options[i].text == valor) index = i;
	}

	return index;
}