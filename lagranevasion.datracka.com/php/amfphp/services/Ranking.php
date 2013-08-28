<?php

include("../../inc/mysql.inc.php");

class Ranking {

	var $db;
	
	function Ranking(){
		$this->$db = new sql_db("localhost", "lagranev_lagrane", "lagran12", "lagranev_lagranevasion", false);
	}
	
	function getRanking($num){
		$sql = "SELECT * FROM ranking ORDER BY puntuacion,fecha ASC LIMIT 0,$num";
		$result = $this->$db->sql_query($sql,true);
		if($result){
		 	while($resultrow = $this->$db->sql_fetchrow($result)){
				$resrow["nombre"]=$resultrow["nombre"];
				$resrow["puntuacion"]=$resultrow["puntuacion"];
		 		$res[]=$resrow;
			}
			return $res;
		}else return -1;
	}
	
	function setRanking($nom,$punt,$email){
		if($nom != "" && $punt != "" && $email != ""){
			$sql = "INSERT INTO ranking(nombre,puntuacion,email,fecha) VALUES('$nom','$punt','$email', NOW())";
			$res = $this->$db->sql_query($sql);
			if ($res){
					$sql = "SELECT count(*)total FROM ranking WHERE puntuacion<=$punt AND fecha<=NOW()";
					$res = $this->$db->sql_query($sql);
					$resultrow = $this->$db->sql_fetchrow($res);
					
				  return $resultrow["total"]; 
			}else return -1; 
		}else return -2;
	}
}

?>