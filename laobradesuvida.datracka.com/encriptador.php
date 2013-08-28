<?php

    $string = $_GET['encrypt'];
    $passphrase="";
    
    $keyDataPublic = file_get_contents('public_key.pem');
    
    //obtenemos clave privada y publica
    if(!$keyPublic = openssl_pkey_get_public($keyDataPublic)) echo "error getting public<br>";
    
    //encriptamos
    openssl_public_encrypt($string, $encrypted, $keyPublic);
    
    echo urlencode(base64_encode($encrypted));
    //echo $string;

?>