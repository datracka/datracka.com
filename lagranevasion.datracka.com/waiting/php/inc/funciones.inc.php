<?


function check_email_mx($email) { 
	if( (preg_match('/(@.*@)|(\.\.)|(@\.)|(\.@)|(^\.)/', $email)) || 
		(preg_match('/^.+\@(\[?)[a-zA-Z0-9\-\.]+\.([a-zA-Z]{2,3}|[0-9]{1,3})(\]?)$/',$email)) ) { 
		$host = explode('@', $email);
		if(checkdnsrr($host[1].'.', 'MX') ) return true;
		if(checkdnsrr($host[1].'.', 'A') ) return true;
		if(checkdnsrr($host[1].'.', 'CNAME') ) return true;
	}
	return false;
} 

function insertaUsuario($nombre,$email){

	global $db;

	$sql="INSERT INTO usuarios(nombre,email,fecha) VALUES('$nombre','$email',NOW())";
	$res=$db->sql_query($sql);
	
	return $res;
}

function retornaPelicula($md5){
	global $db;
	
//Retorno xml
	$sql = "SELECT * FROM peliculas WHERE md5='$md5'";
	$result = $db->sql_query($sql,true);
	$num_rows = $db->sql_numrows($result);

	if($num_rows != 0){	
		$res = $db->sql_fetchrow($result);
		$datos = $res['xml'];
	}else{
		$xml = "<node>\n</node>"; 
		$datos = $xml;
	}
		
return $datos;	
}	

function borrarPelicula($md5){
	global $db;

	$sql="DELETE FROM peliculas WHERE md5='$md5'";	
	$res=$db->sql_query($sql);
	return $res;
}

function enviar_mail($md5,$idioma,$nombre,$email,$nombreAmigo,$emailAmigo){
	global $db;		

	if($idioma == "es")$subject="te envia una sorpresa";
	
		ob_start();
		readfile("./email/".$idioma."/index_email.html");
		$html=ob_get_contents();
		ob_end_clean();
		
		$html=eregi_replace("__IDIOMA__",$idioma,$html);
		$html=eregi_replace("__URL_BASE__",$md5,$html);
		$html=eregi_replace("__NOMBRE__",$nombre,$html);
		$html=eregi_replace("__NOMBRE_AMIGO__",$nombreAmigo,$html);

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
  $mail->PluginDir = "inc/includes_php_mailer/";
  
  $mail->From = $email;
  $mail->FromName = $nombre;
	$mail->AddReplyTo($email, $nombre);
	$mail->Sender = $email;  
	$mail->AddAddress($emailAmigo,$nombreAmigo);
  $mail->Subject = $nombre." ".$subject;
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
 
  if($exito)	$datos="result=OK";
	else	$datos="result=KO";
		
return $datos;   
}

?>