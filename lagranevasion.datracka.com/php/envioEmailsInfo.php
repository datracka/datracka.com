<?
include("cargar_includes.inc.php");

$in = new input($_POST);

//Envio de mails de tabla usuarios
$table = "usuarios";

$datos = new datos();
$respuesta .= $datos->envioMailsInfo($table);


echo $respuesta;
//xml_write($respuesta);

?>