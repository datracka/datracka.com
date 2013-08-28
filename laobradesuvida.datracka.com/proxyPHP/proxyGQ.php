<?php

$gq_url = false;


$headers = array();
// Buscamos la cabecera con la URL del GQ
foreach ($_SERVER as $k => $v){
	if (substr($k, 0, 5) == "HTTP_"){
		$k = str_replace('_', ' ', substr($k, 5));
		$k = str_replace(' ', '-', ucwords(strtolower($k)));
		$headers[$k] = $v;
		//echo "strpos ".$k."-".$v;
		if (strpos($k, 'Gqhost') !== false){
			$gq_url = $v;		
		}
	}
}

if (!$gq_url){
	// La peticion no tenia una cabecera con nombre GQHost
    header("HTTP/1.0 400 Bad Request");
    echo "No se ha establecido el Request Header GQHost con la URL completa al Gestor de Cuestionarios";
    exit();
}else if(strpos($gq_url,'.neuronics.') === false && strpos($gq_url,'.cpdata.') === false){
	// Se obtuvo la URL del GQ, pero no esta en un dominio *.neuronics.* ni *.cpdata.*
	header("HTTP/1.0 400 Bad Request");
    echo "No se permite que la URL completa al Gestor de Cuestionarios este en un dominio que no sea *.neuronics.* o *.cpdata.*";
    exit();
}

// Abrimos la sesion CURL
$session = curl_init($gq_url);

//Configuración del proxy
//curl_setopt($session, CURLOPT_PROXY, "proxy.bcn.bbdo2k.es");
//curl_setopt($session, CURLOPT_PROXY, "proxybcn.bbdo.local");
//curl_setopt($session, CURLOPT_PROXYPORT, 8080); 

if ($_SERVER['REQUEST_METHOD']==='POST') {
	curl_setopt ($session, CURLOPT_POST, true);
	curl_setopt ($session, CURLOPT_POSTFIELDS, $HTTP_RAW_POST_DATA);
	$header_array = Array( "Content-Type: text/xml");
	curl_setopt ($session, CURLOPT_HTTPHEADER, $header_array);
	curl_setopt ($session, CURLOPT_CUSTOMREQUEST, "POST");
}

// No devolver las cabeceras HTTP; devolver los contenidos de la llamada
curl_setopt($session, CURLOPT_HEADER, false);
curl_setopt($session, CURLOPT_RETURNTRANSFER, true);

// Hacemos la llamada
$xml = curl_exec($session);

// Devolvemos XML, asi que establecemos el content-type adecuado
header("Content-Type: text/xml");

echo $xml;
curl_close($session);