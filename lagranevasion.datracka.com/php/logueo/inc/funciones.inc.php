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

function existeix($login,$pwd){
	global $db;
	
	$sql = "SELECT * FROM usuaris WHERE login='$login' AND pwd='$pwd'";
	$result = $db->sql_query($sql,true);
	$num_rows = $db->sql_numrows($result);

	if($num_rows != 0)return true;	
	else return false;
}

function acabat($login,$pwd){
	global $db;
	
	$sql = "SELECT * FROM usuaris WHERE login='$login' AND pwd='$pwd' AND acabat=1";
	$result = $db->sql_query($sql,true);
	$num_rows = $db->sql_numrows($result);

	if($num_rows != 0)return true;	
	else return false;
}

function retornaDades($login,$pwd){
	global $db;

	$sql = "SELECT * FROM usuaris WHERE login='$login' AND pwd='$pwd'";
	$result = $db->sql_query($sql,true);
	$num_rows = $db->sql_numrows($result);

	if($num_rows != 0)$datos = generaXMLDades($db);
	else $datos = "<dades><mensaje>Error Interno</mensaje></dades>"; 
		
return $datos;	
}	

function creaUsuari($login,$pwd){

	$md5 = $login.$pwd.rand().time();
	$md5= md5($md5);
	
	$sql="INSERT INTO usuaris(login,pwd,fecha_insercion,fecha_ultim,md5) VALUES('$login','$pwd',NOW(),NOW(),'$md5')";
	$res=$db->sql_query($sql);
	
	$xml = "<dades>\n<mensaje>Usuario Nuevo</mensaje>\n<md5>$md5</md5>\n</dades>"; 
	$datos = $xml;

}

function generaXMLDades($db){
	
		$res = $db->sql_fetchrow($result);
		
		$md5 = $res['md5'];
		$nombreUTF8 = mb_convert_encoding($res['nombre'], "UTF-8","ISO-8859-1");
		$apellidosUTF8 = mb_convert_encoding($res['apellidos'], "UTF-8","ISO-8859-1");
		$dni = $res['dni'];
		$email = $res['email'];
		$telefono = $res['telefono'];
		$direccionUTF8 = mb_convert_encoding($res['direccion'], "UTF-8","ISO-8859-1");
		$fecha_nacimiento = $res['fecha_nacimiento'];
		$foto = $res['foto'];
		$provincia = $res['provincia'];
		$edad = $res['edad'];
		$disponibilidad = $res['disponibilidad'];
		$nivel_informatica = $res['nivel_informatica'];
		$trabajador = $res['trabajador'];
		$sector = $res['sector'];
		$horas_trabaja = $res['horas_trabaja'];
		$pareja = $res['pareja'];
		$nombre_parUTF8 = mb_convert_encoding($res['nombre_par'], "UTF-8","ISO-8859-1");
		$apellidos_parUTF8 = mb_convert_encoding($res['apellidos_par'], "UTF-8","ISO-8859-1");
		$dni_par = $res['dni_par'];
		$direccion_parUTF8 = mb_convert_encoding($res['direccion_par'], "UTF-8","ISO-8859-1");
		$email_par = $res['email_par'];
		$telefono_par = $res['telefono_par'];
		$provincia_par = $res['provincia_par'];
		$fecha_nacimiento_par = $res['fecha_nacimiento_par'];
		$abierta1 = $res['abierta1'];
		$abierta2 = $res['abierta2'];

		$out.="<datos>\n";
		$out.="<mensaje>Usuario Inacabado</mensaje>\n";
		$out.="<nombre>$nombreUTF8</nombre>\n";
		$out.="<apellidos>$apellidosUTF8</apellidos>\n";
		$out.="<dni>$dni</dni>\n";
		$out.="<email>$email</email>\n";
		$out.="<telefono>$telefono</telefono>\n";
		$out.="<direccion>$direccionUTF8</direccion>\n";
		$out.="<fecha_nacimiento>$fecha_nacimiento</fecha_nacimiento>\n";
		$out.="<foto>$foto</foto>\n";
		$out.="<provincia>$provincia</provincia>\n";
		$out.="<edad>$edad</edad>\n";
		$out.="<disponibilidad>$disponibilidad</disponibilidad>\n";
		$out.="<nivel_informatica>$nivel_informatica</nivel_informatica>\n";
		$out.="<trabajador>$trabajador</trabajador>\n";
		$out.="<sector>$sector</sector>\n";
		$out.="<horas_trabaja>$horas_trabaja</horas_trabaja>\n";
		$out.="<pareja>$pareja</pareja>\n";
		$out.="<nombre_par>$nombre_parUTF8</nombre_par>\n";
		$out.="<apellidos_par>$apellidos_parUTF8</apellidos_par>\n";
		$out.="<dni_par>$dni_par</dni_par>\n";
		$out.="<direccion_par>$direccion_parUTF8</direccion_par>\n";
		$out.="<email_par>$email_par</email_par>\n";
		$out.="<telefono_par>$telefono_par</telefono_par>\n";
		$out.="<provincia_par>$provincia_par</provincia_par>\n";
		$out.="<fecha_nacimiento_par>$fecha_nacimiento_par</fecha_nacimiento_par>\n";	
		$out.="<abierta1>$abierta1</abierta1>\n";
		$out.="<abierta2>$abierta2</abierta2>\n";
		$out.="</datos>\n\n";
	
return $out;
}


?>