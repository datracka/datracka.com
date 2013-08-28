<?php

    $ch = curl_init("http://api.linkedin.com/v1/people/~/mailbox");
	
	$xml = "<?xml version='1.0' encoding='UTF-8'?><mailbox-item><recipients><recipient><person path='/people/email=datracka@hotmail.com'><first-name>Vicens</first-name><last-name>Fayos</last-name></person></recipient></recipients><subject>invitacion a conectar</subject><body>Por favor, aceptame</body>					  <item-content><invitation-request><connect-type>friend</connect-type></invitation-request></item-content></mailbox-item>";
 
  	curl_setopt($ch, CURLOPT_TIMEOUT, 180);
  
   //metodo post
	curl_setopt($ch, CURLOPT_POST, true);
	curl_setopt($ch, CURLOPT_POSTFIELDS, $xml);
	curl_setopt($ch, CURLOPT_HTTPHEADER, Array("Content-Type: text/xml; charset=UTF-8"));
	curl_setopt ($ch, CURLOPT_CUSTOMREQUEST, "POST");
	
	// no devolver las cabeceras HTTP; devolver los contenidos de la llamada
	curl_setopt($ch, CURLOPT_HEADER, false);
	curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
	
	//ejecutamos la llamada
    $data=curl_exec($ch);

	echo $data;
	
?>