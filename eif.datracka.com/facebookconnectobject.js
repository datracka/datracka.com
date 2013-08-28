//Local Variable with Flash Content id. default = flashcontent
var as_swf_name = "flashcontent";

//Initialize Facebook
function fbInit(pAsSwfName,pApi_key,pReceiver){
	as_swf_name = pAsSwfName;
	FB.init(pApi_key,pReceiver);
}

//JavaScript Connect methods
function login(){
	FB.Connect.requireSession( onLoginHandler );
}

//Event Handlers
function onLoginHandler(){
	flashCallBack( "onLogIn" );
}

//Method to dispatch an Event to Flash
function flashCallBack ( func ) {
	if( arguments.length > 1 ){
		document[as_swf_name][func]( Array.prototype.slice.call(arguments).slice(1)[0]);
	}else{
		document[as_swf_name][func]();
	}
}

function hola(str){
	alert(str);
}