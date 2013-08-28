<?
include("cargar_includes.inc.php");

$in = new input($_POST);

$datos = new datos();
$datos->cogeDatosEnviaAmigo($in);

//Comprovem dades
$res = $datos->comprobacioDadesEnviaAmigo();
if($res=="Valides"){
		$datos->insertaDatosEnviarAmigo();
		if($datos->enviarMails()) $respuesta = "<response valid='true'><mensaje>El envio se ha hecho correctamente.</mensaje></response>";
		else $respuesta = "<response valid='false'><mensaje>No se ha podido hacer el envio. Por favor, vuelve a intentarlo más tarde.</mensaje></response>"; 

}else $respuesta = "<response valid='false'><mensaje>El campo \"$res\" es incorrecto.Por favor, modifícalo.</mensaje></response>"; 


//echo $respuesta;
xml_write($respuesta);

?>