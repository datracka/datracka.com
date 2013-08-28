<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>Documento sin t&iacute;tulo</title>
</head>

<body>
<?php

$string = "624285";
echo "before to encrypt: ". $string ."<br>";

$passphrase="";

$keyDataPrivate = file_get_contents('private_key.pem');
$keyDataPublic = file_get_contents('public_key.pem');

//obtenemos clave privada y publica
if(!$keyPrivate =openssl_pkey_get_private($keyDataPrivate)) echo "error getting private<br>";
if(!$keyPublic = openssl_pkey_get_public($keyDataPublic)) echo "error getting public<br>";

//encriptamos
openssl_public_encrypt($string, $encrypted, $keyPublic);

$encryptedText= urlencode(base64_encode($encrypted));
echo "String crypt: ". $encryptedText;
//desencriptamos
openssl_private_decrypt($encrypted,$newsource,$keyPrivate);

echo "String decrypt: ". $newsource;

?>


</body>
</html>
