<?
require_once("cargar_includes.inc.php");

$in = new input($_GET);

$accion=$in->get("accion");
$num_actual=$in->get("na");
$num_p=10;


if($accion=="buscar"){
	$aux=array();
	$aux["id"]=$in->get("id");
	$aux["edad"]=$in->get("edad");
	$aux["foto"]=$in->get("foto");	
	$aux["pareja"]=$in->get("pareja");	
	$aux["estado_auto"]=$in->get("estado_auto");	
	$aux["puntuacion_auto"]=$in->get("puntuacion_auto");
	$aux["estado"]=$in->get("estado");
	$aux["puntuacion"]=$in->get("puntuacion");				
	$sesion_consultas["mod_tal_consultar"]=$aux;
}
?>
<html>
<head>
<link rel="stylesheet" type="text/css" href="css/styles.css">
<SCRIPT>
 function enviar(){
    f = document.forms["formulario"];
    f.submit() ;
 }
 
 
</SCRIPT>
</head>
<body>
   <!-- contenido-->
   <form name="formulario">
   <h1>Listado de registrados</h1>
   <h2>Criterios de busqueda</h2>
   <INPUT type="hidden" name="accion" value="buscar">
   <table width="90%">
   <tr><td align="center" class="destacado">Referencia</td><td align="center" class="destacado">Edad</td><td align="center" class="destacado">Foto</td><td align="center" class="destacado">Pareja</td><td align="center" class="destacado">Estado_auto</td><td align="center" class="destacado">Puntuacion_auto</td><td align="center" class="destacado">Estado</td><td align="center" class="destacado">Puntuacion</td></tr>
   <tr>
	   <td align="center"><INPUT type="text" name="id" size="4" maxlength="4" ></td>
	   <td align="center"><SELECT name="edad">
		   <OPTION value=""></OPTION>
		   <OPTION value="0-10">0-10</OPTION>
		   <OPTION value="10-18">10-18</OPTION>
		   <OPTION value="18-30">18-30</OPTION>
		   <OPTION value="30-40">30-40</OPTION>
		   <OPTION value="40-50">40-50</OPTION>
		   <OPTION value="50-60">50-60</OPTION>
		   <OPTION value="60-70">60-70</OPTION>
		   <OPTION value="70-80">70-80</OPTION>
		   <OPTION value="80-90">80-90</OPTION>
		   <OPTION value="90-100">90-100</OPTION>
	   </SELECT></td>
	   
	   <td align="center">
			<SELECT name="foto">
				<OPTION></OPTION>
				<OPTION value="si">Si</OPTION>
				<OPTION value="no">No</OPTION>
			</SELECT>
	   </td> 
		<td align="center">
			<SELECT name="pareja">
				<OPTION></OPTION>
				<OPTION value="si">Si</OPTION>
				<OPTION value="no">No</OPTION>
			</SELECT>
		</td>
		<td align="center">
			<SELECT name="estado_auto">
				<OPTION></OPTION>
				<OPTION value="0">Aceptado</OPTION>
				<OPTION value="1">Excluido</OPTION>
			</SELECT>
		</td>
		<td align="center">
			<SELECT name="puntuacion_auto">
		   <OPTION value=""></OPTION>
		   <OPTION value="0-10">0-10</OPTION>
		   <OPTION value="10-20">10-20</OPTION>
		   <OPTION value="20-30">20-30</OPTION>
		   <OPTION value="30-40">30-40</OPTION>
		   <OPTION value="40-50">40-50</OPTION>
		   <OPTION value="50-60">50-60</OPTION>
		   <OPTION value="60-70">60-70</OPTION>
		   <OPTION value="70-80">70-80</OPTION>
		   <OPTION value="80-90">80-90</OPTION>
		   <OPTION value="90-100">90-100</OPTION> 
			</SELECT>
		</td>
		<td align="center">
			<SELECT name="estado">
				<OPTION></OPTION>
				<OPTION value="Aceptado">Aceptado</OPTION>
				<OPTION value="Excluido">Excluido</OPTION>
				<OPTION value="Finalista">Finalista</OPTION>
				<OPTION value="Pendiente">Pendiente</OPTION>
			</SELECT>
		</td>
		<td align="center">
			<SELECT name="puntuacion">
				<OPTION></OPTION>
				<? for($i=0;$i<=100;$i++){?>
			   <OPTION value="<?=$i?>"><?=$i?></OPTION>
			  <? $i+=9;} ?>  
			</SELECT>
		</td>								
	</tr> 
	<tr>
   <td colspan="8" align="right"><INPUT class="btn" type="button" name="buscar" value="buscar" onclick="enviar()"></td>
  </tr>	
	</table>
	</form>
   
   <h2>Resultados</h2>
   <TABLE border="0" width="90%">
   <tr>
   	<td class="destacado">Ref.</td><td class="destacado">Nombre</td><td class="destacado">Profesión</td><td class="destacado">Foto</td><td class="destacado">Fecha Nacimiento</td><td class="destacado">Pareja</td><td class="destacado">Estado_auto</td><td class="destacado">Puntuacion_auto</td><td class="destacado">Estado</td><td class="destacado">Puntuacion</td><td class="destacado">Comentarios</td><td class="destacado">Info</td>
   </tr>
   <?
   	$max=buscar_talentos(true,$sesion_consultas["mod_tal_consultar"],0,0);
//echo "max=".$max;
	if($max<=0)
		echo "<tr><td colspan='6'  align='center'>Talentos no encontrados<td></tr>";
	else{
		if($num_actual<1)
			$num_actual=1;		
		else if($num_actual>$max)
			$num_actual=$max;
	
		$res=@buscar_talentos(false,$sesion_consultas["mod_tal_consultar"],$num_actual,$num_p);	
		//echo "max=".$max." act=".$num_actual;
		for($i=0;$i<count($res);$i++){
			$aux=$res[$i];
		?>
		<tr>
			<td  align="right"><?=$aux["id"]?></td>
			<td ><?=$aux["nombre"]?></td>
			<td ><?=$aux["profesion"]?></td>
			<td ><img src="<?="../img/".substr($aux["foto"], 0, 2)."/".$aux["foto"]."/imagen.jpg"?>" width="100px" height="100px"></td>
			<td ><?=$aux["edad"]?></td>
			<td ><?=$aux["pareja"]?></td>
			<td class="Rojo" ><? if($aux["estado_auto"]==0)echo "Aceptado";else echo "Excluido";?></td>
			<td class="Rojo" ><?=$aux["puntuacion_auto"]?></td>  
			<td class="Rojo" ><?=$aux["estado"]?></td>
			<td class="Rojo" ><?=$aux["puntuacion"]?></td> 
			<td class="comentarios"><?=$aux["comentarios"]?></td> 		 	
			<td><a href="gestor_detallado.php?id=<?=$aux["id"]?>" target="_blank">Más info</a></td>
		</tr>	
		<?
		}
		?><tr><td valign="bottom" align="right" colspan="6">
		<? if($num_actual- $num_p>=1){?>
		<A href="gestor.php?na=<?=$num_actual- $num_p ?>&accion=<?=$accion?>
			<?
				foreach($sesion_consultas["mod_tal_consultar"] as $key => $value){
					echo "&".$key."=".$value;
				}
		  ?>
			"> < </A>
		<?}?>
		<?=$num_actual?> al <?if($num_actual-1+$num_p < $max) 
			echo $num_actual+$num_p-1; else echo $max;?>/<?=$max?> 
			<?if($num_actual-1+$num_p < $max){?>
		<A href="gestor.php?na=<?=$num_actual + $num_p ?>&accion=<?=$accion?>
			<?
				foreach($sesion_consultas["mod_tal_consultar"] as $key => $value){
					echo "&".$key."=".$value;
				}
		  ?>
			"> ></A>
		<?}?>
		</td><td>&nbsp;</td></tr><?
	}
   ?>  
   </table>
</body>
</html>
