<?
include("cargar_includes.inc.php");

$in = new input($_POST);

$datos = new datos();
$datos->cogeDatosMailInfo($in);

//Comprovem dades
$res = $datos->comprobacioDadesMailInfo();

if($res=="Valides"){ 
	if(!$datos->mirarExisteixRegistratInfo()){
		//if($datos->enviarMail()){
			
				if($datos->insertaDatosMailInfo())$respuesta = "<response valid='true'><mensaje>Se ha inscrito correctamente.</mensaje></response>";
				else $respuesta = "<response valid='false'><mensaje>Se ha producido un error en la inserci�n del registro en la base de datos. Por favor, int�ntalo m�s tarde.</mensaje></response>"; 			
	
		//}else $respuesta = "<response valid='false'><mensaje>No se ha podido enviar el e-mail. Por favor, vuelve a intentarlo m�s tarde.</mensaje></response>"; 
	}else $respuesta = "<response valid='false'><mensaje>Este e-mail ya ha sido registrado. Por favor, comprueba que lo hayas introducido correctamente.</mensaje></response>"; 
}else $respuesta = "<response valid='false'><mensaje>El campo \"$res\" es incorrecto.Por favor, modif�calo.</mensaje></response>"; 


//echo $respuesta;
xml_write($respuesta);

?>