<?
include("cargar_includes.inc.php");

$in = new input($_POST);

$datos = new datos();
$datos->cogeDatosFormulario($in);
$datos->calcularPareja();

//Comprovem dades
$res = $datos->comprobacioDades();

if($res=="Valides"){ 
	//echo "s�n valides";
	//Miro si existeix, si ja s'havia registrat
	if(!$datos->mirarExisteixRegistrat()){
		if(!$datos->mirarExisteixComACompany()){
			if(!$datos->mirarParejaExisteixRegistrat()){
				if(!$datos->mirarParejaExisteixComACompany()){
					//	echo "No existeix";
						$datos->mirarExclusion();
						$datos->calcularPuntuacion();						
						$datos->creaMD5();
						if($datos->insertaDatos()){
							
								if($datos->pareja==1)$datos->enviarMail();//}else $respuesta = "<response valid='false'><mensaje>No se ha podido enviar el e-mail de confirmaci�n a tu acompa�ante. Por favor, vuelve a intentarlo m�s tarde.</mensaje></response>"; 
								$respuesta = "<response valid='true'><mensaje></mensaje></response>";
	
						}else{ $respuesta = "<response valid='false'><mensaje>Se ha producido un error en la inserci�n del registro en la base de datos. Por favor, int�ntalo m�s tarde.</mensaje></response>";} 			
	

				}else $respuesta = "<response valid='false'><mensaje>El e-mail del acompa�ante ya ha sido registrado como acompa�ante.</mensaje></response>"; 
			}else $respuesta = "<response valid='false'><mensaje>El e-mail del acompa�ante ya ha sido registrado como participante.</mensaje></response>"; 
		}else $respuesta = "<response valid='false'><mensaje>Ya has sido registrado como acompa�ante. Mira tu buz�n de correo.</mensaje></response>"; 
	}else $respuesta = "<response valid='false'><mensaje>Este e-mail ya ha sido registrado. Por favor, comprueba que lo hayas introducido correctamente.</mensaje></response>"; 

}else $respuesta = "<response valid='false'><mensaje>El campo \"$res\" es incorrecto.Por favor, modif�calo para continuar.</mensaje></response>"; 

//echo $respuesta;
xml_write($respuesta);

?>