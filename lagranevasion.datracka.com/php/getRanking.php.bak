<?
include("cargar_includes.inc.php");


$in = new input($_POST);
$nombre = $in->get("nombre");
$puntuacion = $in->get("puntuacion");

$respuesta = setRanking($nombre,$puntuacion);

xml_write($respuesta);
?>