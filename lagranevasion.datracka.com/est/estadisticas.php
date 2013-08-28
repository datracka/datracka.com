<?php
	include("./cargar_includes.inc.php");
	$gl=persistent_open("../php/ser/est_global.ser");
	
	function out_table($gl,$idioma,$solo_valores=0){
		$su="";
		if($idioma!="")
			$su="_$idioma";
		?>
 		<table>
 			<tr><?if(!$solo_valores){?><td valign="top">Home</td><?}?><td align="right"  valign="top">&nbsp;<?=$gl["INDEX".$su]?></td></tr>
 			<tr><?if(!$solo_valores){?><td valign="top">Emails enviados</td><?}?><td align="right"  valign="top">&nbsp;<?=$gl["EMAIL".$su]?></td></tr>
 		</table>
		<?	
	} 
	
	function out_table_idioma($gl){
	?>
	<table width="600" border="1">
		<tr>
			<td width="50%"></td>
				<td width="10%" align="center"><h3>ES</h3></td>
				<td width="10%" align="center"><h3>EN</h3></td>
				<td width="10%" align="center"><h3>DE</h3></td>
				<td width="10%" align="center"><h3>FR</h3></td>
				<td width="10%" align="center"><h3>IT</h3></td>
		</tr>
		<tr>
			<td>
	 		<table>
	 			<tr><td valign="top">Home&nbsp;</td></tr>
	 			<tr><td valign="top">Emails enviados&nbsp;</td></tr>
	 		</table>				
			</td>	
			<td align="right" valign="top"><?out_table($gl,"es",1);?>&nbsp;</td>
			<td align="right" valign="top"><?out_table($gl,"en",1);?>&nbsp;</td>
			<td align="right" valign="top"><?out_table($gl,"de",1);?>&nbsp;</td>
			<td align="right" valign="top"><?out_table($gl,"fr",1);?>&nbsp;</td>
			<td align="right" valign="top"><?out_table($gl,"it",1);?>&nbsp;</td>
		</tr>
	</table>
	<?
	}
	
 ?>
 <html>
 	<head>
 		<title>Estadísticas</title>
 	</head>
 	<body>
 		<h1>Total</h1>
 		<?
 		out_table($gl,"",0);
 		?>
 		<h2>Por idioma</h2>
		<? out_table_idioma($gl);?>
		<h1>Por Días</h1>
		<?
		$dir = "../php/ser/";
		$dh  = opendir($dir);
		$dias=array();
		while (false !== ($nombre_archivo = readdir($dh))) {
			if(strstr($nombre_archivo,'est_2')!==false)
   			$dias[] = $nombre_archivo;
		}
		rsort($dias);		
		
		for($i=0;$i<count($dias);$i++){
			$dia = ereg_replace("est_","", $dias[$i]);
			$dia = ereg_replace(".ser","", $dia);
			echo "<h2>$dia</h2>";
			$gl=persistent_open("../php/ser/".$dias[$i]);
			out_table($gl,"",0);
			out_table_idioma($gl);
		}
		?>
 	</body>
</html>