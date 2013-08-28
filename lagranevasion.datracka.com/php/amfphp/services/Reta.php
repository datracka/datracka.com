<?php

include_once("../../inc/includes_php_mailer/class.phpmailer.php");
include_once("../../inc/includes_php_mailer/class.smtp.php");

class Reta {
	
	function Reta(){}
	
	function setReta($nombre,$emailAmigo,$puntuacion,$idioma){
		
		$email = "info@lagranevasion.es";
		$nombreAmigo = $emailAmigo;
		$subject = array("es" =>  "Yo en ".$puntuacion." ¿y tú?",
	 									 "en" =>  "Yo en ".$puntuacion." ¿y tú?Inglés");
		
		if($idioma!="es" && $idioma!="en")
			$idioma="es";

		ob_start();
		readfile("../../email/reta/email_".$idioma.".html");
		$html=ob_get_contents();
		ob_end_clean();

		$html=eregi_replace("__NOMBRE__",$nombre,$html);
		$html=eregi_replace("__PUNTUACION__","".$puntuacion,$html);
	
	  $mail = new phpmailer();
	  $mail->Priority = 3;
		$mail->Encoding = "8bit";
		$mail->CharSet = "iso-8859-1";
	  $mail->Mailer = "smtp";
	  $mail->Host = "localhost";
	  $mail->Timeout=30;
		$mail->IsHTML(true);
		$mail->WordWrap = 0; 	      
		$mail->Port = 25;  
	  $mail->SMTPAuth = false;
	  //$mail->PluginDir = "inc/includes_php_mailer/";
	  
	  $mail->From = $email;
	  $mail->FromName = $nombre;
		$mail->AddReplyTo($email, $nombre);
		$mail->Sender = $email;  
	
		$mail->AddAddress($emailAmigo,$nombreAmigo);
	  $mail->Subject = $subject[$idioma];
	  $mail->Body = $html;
	  $mail->AltBody = "";

	  $exito = $mail->Send();
	
	  $intentos=1; 
	  while ((!$exito) && ($intentos < 5)) {
		sleep(5);
	     	//echo $mail->ErrorInfo;
	     	$exito = $mail->Send();
	     	$intentos=$intentos+1;	
	   }
	 
	  if($exito)	return true.$puntuacion;
		else	return false;

	} 		



}

?>