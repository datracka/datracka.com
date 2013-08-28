<?
include("cargar_includes.inc.php");

$in = new input($_POST);

$datos = new datos($in);
$exclusion = $datos->mirarExclusion();
$puntuacion = $datos->calcularPuntuacion();
$acabat = $datos->mirarAcabat($_POST);
$respuesta = $datos->insertaDatos($exclusion,$puntuacion,$acabat);

echo $respuesta;

?>