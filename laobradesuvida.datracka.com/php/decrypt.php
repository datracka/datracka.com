<?php

//if(isset($_GET['id'])) $id = $_GET['id']; else $id = "0";
//echo "we received: ". $id . "<br/>";

function decrypt($id){

	$keyDataPrivate = file_get_contents('private_key.pem');
	if(!$keyPrivate =openssl_pkey_get_private($keyDataPrivate)) echo "error getting private<br>";
	//desencriptamos
	$encrypted= base64_decode($id);
	openssl_private_decrypt($encrypted,$newsource,$keyPrivate);

	return $newsource;
}

?>

