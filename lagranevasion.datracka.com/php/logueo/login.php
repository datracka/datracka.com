<?
include("cargar_includes.inc.php");


$in = new input($_POST);
$login = $in->get("login");
$pwd = $in->get("pwd");

//Miro si existeix
if(existeix($login,$pwd)){
	
	//Miro si ha acabat el registre
	if(acabat($login,$pwd)) $datos = "<dades><mensaje>Usuario Registrado</mensaje></dades>"; 
	else $datos = retornaDades($login,$pwd);
	
}else{
	//Creo l'usuari
	$datos = creaUsuari($login,$pwd);
}

//echo $datos;
xml_write($datos);
?>