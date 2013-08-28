<?php


function xml_write($datos){
	$datosUTF8 = mb_convert_encoding($datos, "UTF-8","ISO-8859-1");
  //$datosUTF8 = replace($datosUTF8);
  
   	echo '<?xml version="1.0" encoding="UTF-8" standalone="yes"?>';
    echo "$datosUTF8";
    //echo "$datos";
}

function replace($datos){
	
	$datos = eregi_replace("ä","a",$datos);
	$datos = eregi_replace("ë","e",$datos);
	$datos = eregi_replace("ï","i",$datos);
	$datos = eregi_replace("ö","o",$datos);
	$datos = eregi_replace("ü","u",$datos);
	return $datos;
}
?>