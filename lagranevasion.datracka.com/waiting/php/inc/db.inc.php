<?php

$dbhost=$propiedades["host"];
$dbuname=$propiedades["login"];
$dbpass=$propiedades["password"];
$dbname=$propiedades["database"];

function error_msg($error){
    echo "<script Language=\"Javascript\">\n";
    echo "<!--\n";
    echo "alert(\"".$error."\");\n";
    echo "//--></script>\n";
}

$db = new sql_db($dbhost, $dbuname, $dbpass, $dbname, false);
if(!$db->db_connect_id) {
    error_msg("No puedo conectarme a la base de datos");
}

$db2 = new sql_db($dbhost, $dbuname, $dbpass, $dbname, false);
if(!$db2->db_connect_id) {
    error_msg("No puedo conectarme a la base de datos");
}

?>