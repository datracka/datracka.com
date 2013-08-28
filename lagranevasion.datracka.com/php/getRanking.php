<?
include("cargar_includes.inc.php");


$in = new input($_POST);
$num = $in->get("num");

$respuesta = getRanking($num);

xml_write($respuesta);
?>