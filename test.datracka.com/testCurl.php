<?php
/**
 * Created by JetBrains PhpStorm.
 * User: vfayos
 * Date: 28.03.13
 * Time: 11:11
 * To change this template use File | Settings | File Templates.
 */

// Call the API

$curl_opts = array(
    CURLOPT_TIMEOUT => 30,
    CURLOPT_POST => true,
);


$curl = curl_init("testRequest.php");

echo "A" . $response;
