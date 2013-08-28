<?
include("cargar_includes.inc.php");


$in = new input($_POST);
$mail = $in->get("mail");

puntua($mail);

?>