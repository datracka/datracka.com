<?
include_once("../inc/propiedades.inc.php");
include_once("../inc/mysql.inc.php");
include_once("../inc/db.inc.php");
include_once("../inc/funciones.inc.php");
include_once("../inc/xml.inc.php");
include_once("../inc/input_class.php");

global $db,$db2;
	


?>
<html>
<body>
	<br><br><br><br><br>
	<table align="center" border=1>
<?
$sql = "SELECT DATE_FORMAT(ultimo_acceso,'%Y-%c') mes, COUNT(*) total FROM `calendari_usuari` WHERE entra=1
				GROUP BY DATE_FORMAT(ultimo_acceso,'%Y-%c')";
$result = $db->sql_query($sql,true);
		
?>
		<tr>
			<td>				
				<?
					while($res = $db->sql_fetchrow($result)){
							$mes = $res['mes'];
							$total = $res['total'];
							echo "Total usuaris han entrat $mes : $total <br>";
					}	
				?>						
			</td>
		</tr>			
<?
$sql = "SELECT * FROM calendari_usuari WHERE entra=1";
$result = $db->sql_query($sql,true);
$num_rows = $db->sql_numrows($result);
?>
		<tr>
			<td>
				Total usuaris han entrat:<?=$num_rows?>
			</td>
		</tr>	
<?
$sql = "SELECT DATE_FORMAT(fecha_envio,'%Y-%c') mes, COUNT(*) total FROM `calendari_email`
				GROUP BY DATE_FORMAT(fecha_envio,'%Y-%c')";
$result = $db->sql_query($sql,true);
		
?>
		<tr>
			<td>				
				<?
					while($res = $db->sql_fetchrow($result)){
							$mes = $res['mes'];
							$total = $res['total'];
							echo "Total E-mails enviats $mes : $total <br>";
					}	
				?>						
			</td>
		</tr>			
<?
$sql = "SELECT * FROM calendari_email";
$result = $db->sql_query($sql,true);
$num_rows = $db->sql_numrows($result);
?>				
		<tr>
			<td>
				Total E-mails enviats:<?=$num_rows?>
			</td>
		</tr>
<?
$sql = "SELECT DATE_FORMAT(fecha,'%Y-%c') mes, COUNT(*) total FROM `calendari_est` WHERE script='/regalo/php/imprime.php'
				GROUP BY DATE_FORMAT(fecha,'%Y-%c')";
$result = $db->sql_query($sql,true);
		
?>
		<tr>
			<td>				
				<?
					while($res = $db->sql_fetchrow($result)){
							$mes = $res['mes'];
							$total = $res['total'];
							echo "Total impresos $mes : $total <br>";
					}	
				?>						
			</td>
		</tr>					
<?
$sql = "SELECT * FROM calendari_est WHERE script='/regalo/php/imprime.php'";
$result = $db->sql_query($sql,true);
$num_rows = $db->sql_numrows($result);
?>				
		<tr>
			<td>
				Total impresos:<?=$num_rows?>
			</td>
		</tr>
<?
$sql = "SELECT DATE_FORMAT(fecha,'%Y-%c') mes, COUNT(*) total FROM `calendari_est` WHERE script='/regalo/php/clubclick.php'
				GROUP BY DATE_FORMAT(fecha,'%Y-%c')";
$result = $db->sql_query($sql,true);
		
?>
		<tr>
			<td>				
				<?
					while($res = $db->sql_fetchrow($result)){
							$mes = $res['mes'];
							$total = $res['total'];
							echo "Total accessos ClubPadres $mes : $total <br>";
					}	
				?>						
			</td>
		</tr>					
<?
$sql = "SELECT * FROM calendari_est WHERE script='/regalo/php/clubclick.php'";
$result = $db->sql_query($sql,true);
$num_rows = $db->sql_numrows($result);
?>				
		<tr>
			<td>
				Total accessos ClubPadres:<?=$num_rows?>
			</td>
		</tr>						
			
<?
$sql = "SELECT * FROM calendari_usuari WHERE entra=1";
$result = $db->sql_query($sql,true);
$num_rows = $db->sql_numrows($result);
if($num_rows == 0){	
}else {
?>
<tr>
	<td>	
<table align="center" border=1>
	
		<tr><td>Quins han entrat:</td><td>Emails han enviat:</td><td>Han impr�s:</td><td>Accedeixen a ClubPadres:</td></tr>										
<?	
		while($res = $db->sql_fetchrow($result)){
		$pwd = $res['pwd'];
		$id_usuari = $res['id'];
		
			?><tr><td><?
			echo $pwd;
			?></td><?
			
			$sql2 = "SELECT * FROM calendari_email WHERE id_usuari='$id_usuari'";
			$result2 = $db2->sql_query($sql2,true);
			$num_rows2 = $db2->sql_numrows($result2);
			?><td><? echo $num_rows2; ?></td><?
			
			$sql2 = "SELECT * FROM calendari_est WHERE id_usuari='$id_usuari' AND script='/regalo/php/imprime.php'";
			$result2 = $db2->sql_query($sql2,true);
			$num_rows2 = $db2->sql_numrows($result2);
			?><td><? echo $num_rows2; ?></td><?
				
			$sql2 = "SELECT * FROM calendari_est WHERE id_usuari='$id_usuari' AND script='/regalo/php/clubclick.php'";
			$result2 = $db2->sql_query($sql2,true);
			$num_rows2 = $db2->sql_numrows($result2);
			?><td><? echo $num_rows2; ?></td><?				
				
			?></tr><?
			

		}
?>	
				</table>					
			</td>
		</tr>
<?
}

?>
