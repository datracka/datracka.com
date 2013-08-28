<?
include_once("../../php/inc/propiedades.inc.php");
include_once("../../php/inc/mysql.inc.php");
include_once("../../php/inc/db.inc.php");
include_once("../../php/inc/funciones.inc.php");
include_once("../../php/inc/xml.inc.php");
include_once("../../php/inc/input_class.php");


	$in = new input($_GET,DEBUG);
	$nombre = $in->get("nombre");
	$nombreAmigo = $in->get("nombreAmigo");
	$md5 = $in->get("id");
?>

<html >

<title>Andalucia te quiere</title>

<body>

<p>
	<font color="003273" size="1" face="Verdana, Arial, Helvetica, sans-serif">
		Hola <?= $nombreAmigo ?>,</font>
</p>

<p><font color="003273" size="1" face="Verdana, Arial, Helvetica, sans-serif"><?= $nombre ?> quiere
    que veas esta película.</font></p>
		
<p><font color="003273" size="1" face="Verdana, Arial, Helvetica, sans-serif">Entra
      <a href="http://turismoandaluz.cp-proximity.com/alojamientos/desarrollo/abreMail.php?id=<?= $md5 ?>&idioma=es"><strong><font color="ff0000">aquí</font></strong></a> y disfruta de ella. ¿A qué esperas?</font></p>