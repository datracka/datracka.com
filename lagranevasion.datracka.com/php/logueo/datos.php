<?
class datos{	
	var $md5;
	var $nombre;
	var $apellidos; 
	var $dni;
	var $email;
	var $telefono; 
	var $direccion;
	var $fecha_nacimiento;
	var $foto;
	var $provincia;
	var $edad;
	var $disponibilidad;
	var $nivel_informatica;
	var $trabajador;
	var $sector;
	var $horas_trabaja; 
	var $pareja;
	var $nombre_par;
	var $apellidos_par; 
	var $dni_par;
	var $direccion_par; 
	var $email_par;
	var $telefono_par;
	var $provincia_par; 
	var $fecha_nacimiento_par;
	var $abierta1;
	var $abierta2;
	var $puntuaciones = array("informatica" => array (0 => 0, 1 => 10, 2 => 20),
														"trabajador"  => array (0 => 20, 1 => 10, 2 => 0),
														"sector"  => array (0 => 0, 1 => 10, 2 => 20),
														"horas_trabajadas"  => array (0 => 0, 1 => 10, 2 => 20)
														);

	function datos($in){
		$this->md5 = $in->get("md5");
		$this->nombre = $in->get("nombre");
		$this->apellidos = $in->get("apellidos");
		$this->dni = $in->get("dni");
		$this->email = $in->get("email");
		$this->telefono = $in->get("telefono");
		$this->direccion = $in->get("direccion");
		$this->fecha_nacimiento = $in->get("fecha_nacimiento");
		$this->foto = $in->get("foto");
		$this->provincia = $in->get("provincia");
		$this->edad = $in->get("edad");
		$this->disponibilidad = $in->get("disponibilidad");
		$this->nivel_informatica = $in->get("nivel_informatica");
		$this->trabajador = $in->get("trabajador");
		$this->sector = $in->get("sector");
		$this->pareja = $in->get("pareja");
		$this->nombre_par = $in->get("nombre_par");
		$this->apellidos_par = $in->get("apellidos_par");
		$this->dni_par = $in->get("dni_par");
		$this->direccion_par = $in->get("direccion_par");
		$this->email_par = $in->get("email_par");
		$this->telefono_par = $in->get("telefono_par");
		$this->provincia_par = $in->get("provincia_par");
		$this->fecha_nacimiento_par = $in->get("fecha_nacimiento_par");
		$this->abierta1 = $in->get("abierta1");
		$this->abierta2 = $in->get("abierta2");	
	}

	function calcularPuntuacion(){
		$puntuacion = 0;
		$puntuacion += $this->puntuaciones["nivel_informatica"][$this->nivel_informatica]+
									 $this->puntuaciones["trabajador"][$this->trabajador]+
									 $this->puntuaciones["sector"][$this->sector]+
									 $this->puntuaciones["horas_trabajadas"][$this->horas_trabajadas];
		return $puntuacion;
	}
	
	function mirarExclusion(){

		if(strcmp($this->foto,"") || 
			!strcmp($this->provincia,30) ||
			 strcmp($this->trabajador,2)
			)$exclusion = 1;
		else	$exclusion = 0;
		
		return $exclusion;
	}
	function mirarAcabat($post){
		$acabat = 1;
		foreach ($post as $value){
			//echo $value;
			if(strcmp($value,""))$acabat = 0;
		}
		return $acabat;
	}	
	function insertaDatos($exclusion,$puntuacion,$acabat){
		global $db;
		
		$sql="UPDATE usuaris SET estado_auto=$exclusion,puntuacion=$puntuacion,acabat=$acabat WHERE md5='$this->md5'";	
		$res=$db->sql_query($sql);		
		echo $sql;
		if($res)$resposta = "resultat=ok";
		else $resposta = "resultat=ko";
		
		return $resposta;
	}
}
?>