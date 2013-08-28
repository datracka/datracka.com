<?php

include("cargar_includes.inc.php");
  
global $db; 
//$tipo=$_GET["tipo"];
$idioma=$_GET["idioma"];

//echo "Tipo: $tipo";
$tipos = array( 
					0 => 'provincias',
					1 => 'sectores'
);		
										
										
$out .="<combos>\n";									
foreach($tipos as $key => $value){								
  switch($value){
  	case "provincias":
	    $sql ="SELECT id_provincia,provincia FROM provincias ORDER BY provincia";
	    $result = $db->sql_query($sql);
	    $num_rows = $db->sql_numrows($result);
			$out.="<provincias>\n";
	    if($num_rows != 0){
        while($user = $db->sql_fetchrow($result)){
					$id_provincia=$user['id_provincia'];
					$provincia=mb_convert_encoding($user['provincia'],'UTF-8');
					/*if ($dec_ok=="La Coru?a" || $dec_ok=="La Coruña") {
					    $dec_ok="La Coru&#241;a";
					}*/
					$out.="<provincia data='$id_provincia' label='$provincia' />\n";
	      }
	    }
			$out.="</provincias>\n";
			break;			    	
/*	case "sectores":
			$sql ="SELECT id_sector,sector_".$idioma." FROM sectores ORDER BY sector_".$idioma;
	    $result = $db->sql_query($sql);
	    $num_rows = $db->sql_numrows($result);
			$out.="<sectores>\n";
	    if($num_rows != 0){
        while($user = $db->sql_fetchrow($result)){
					$id_sector=$user['id_sector'];
					$sector=mb_convert_encoding($user['sector_'.$idioma],'UTF-8');
					$out.="<sector data='$id_sector' label='$sector' />\n";
	      }
	    }
			$out.="</sectores>\n";
			break;			    		
	
  }  */ 
}
$out .="</combos>";	     
$out = "<?xml version=\"1.0\" encoding=\"UTF-8\"?".">\n".$out;
echo($out);
@$db->sql_close();
?>