<?
include("cargar_includes.inc.php");


$in = new input($_POST);

$md5 = $in->get("md5");
$idioma = $in->get("idioma");
$fromName = $in->get("fromName");
$fromEmail = $in->get("fromEmail");
$toName = $in->get("toName");
$toEmail = $in->get("toEmail");


if(check_email_mx($fromEmail)){
	if(check_email_mx($toEmail)){
		
			$datos=enviar_mail($md5,$idioma,$fromName,$fromEmail,$toName,$toEmail);
				
	}else{$datos="result=KO";}
}else{$datos="result=KO";}

echo $datos;
//xml_write($datos);
?>