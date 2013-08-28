<?
include("cargar_includes.inc.php");


$in = new input($_POST);

$nombre = $in->get("nombre");
$email = $in->get("email");


$datos="result=KO";
if(insertaUsuario($nombre,$email)) 
	$datos="result=OK&md5=".$md5;

echo $datos;

?>