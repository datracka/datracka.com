<html>
<head><link rel="stylesheet" type="text/css" href="css/styles.css">
</head>
	<?
require_once("cargar_includes.inc.php");

$in = new input($_GET);

$id=$in->get("id");

$accion=$in->get("accion");
$estado=$in->get("estado");
$puntuacion=$in->get("puntuacion");
$comentarios=$in->get("comentarios");
if($accion=="guardar"){
	guardar($id,$estado,$puntuacion,$comentarios);
?>	
	<body>
	   <TABLE align="center" border="0" width="100%" height="100%">
			<tr align="center"> <td align="center"> GUARDADO! </td></tr>	
		</table>	
	</body></html>	
<?			
}else{
?>
<SCRIPT>
 function guardar(){
    f = document.forms["formulario"];
    f.submit() ;
 }
</SCRIPT>

<body>
	 
   <h2>Datos</h2>
   <TABLE border="0" width="100%">

   	<?	$sql="SELECT * FROM usuaris WHERE id=$id";
				$res=ejecutar($sql);
				$aux=$res[0];
		?>

		<tr class="destacado">Ref.</tr>	
		<tr valign="top" align="right"><?=$aux["id"]?></tr>
   	<tr class="destacado">Nombre</tr>
		<tr valign="top"><?=$aux["nombre"]?></tr>
		<tr class="destacado">Profesión</tr>
		<tr valign="top"><?=$aux["profesion"]?></tr>
		<tr class="destacado">Cp</tr>
		<tr valign="top"><?=$aux["cp"]?></tr>
		<tr class="destacado">Email</tr>
		<tr valign="top"><?=$aux["email"]?></tr>
		<tr class="destacado">Telefono</tr>
		<tr valign="top"><?=$aux["telefono"]?></tr>
		<tr class="destacado">Direccion</tr>
		<tr valign="top"><?=$aux["direccion"]?></tr>
		<tr class="destacado">Foto</tr>
		<tr valign="top"><img src="<?="../img/".substr($aux["foto"], 0, 2)."/".$aux["foto"]."/imagen.jpg"?>" width="100px" height="100px"></td>
		<tr class="destacado">Provincia</tr>
<? $res=ejecutar("SELECT id_provincia,provincia FROM provincias WHERE id_provincia=".$aux["provincia"]);?>
		<tr valign="top"><?=$res[0]["provincia"]?></tr>		
		<tr class="destacado">Edad</tr>
		<tr valign="top"><?=$aux["edad"]?></tr>
		
		<tr class="destacado">P1</tr>
		<tr valign="top"><?=$aux["p1"]?></tr>
		<tr class="destacado">P2</tr>
		<tr valign="top"><?=$aux["p2"]?></tr>
		<tr class="destacado">P3</tr>
		<tr valign="top"><?=$aux["p3"]?></tr>
		<tr class="destacado">P4</tr>
		<tr valign="top"><?=$aux["p4"]?></tr>
		<tr class="destacado">P5</tr>
		<tr valign="top"><?=$aux["p5"]?></tr>	
		
		<tr class="destacado">Pareja</tr>
		<tr valign="top"><?=$aux["pareja"]?></tr>
		<tr class="destacado">Nombre_par</tr>
		<tr valign="top"><?=$aux["nombre_par"]?></tr>
		<tr class="destacado">Email_par</tr>
		<tr valign="top"><?=$aux["email_par"]?></tr>
		<tr class="destacado">Telefono_par</tr>
		<tr valign="top"><?=$aux["telefono_par"]?></tr>
		<tr class="destacado">Abierta1</tr>
		<tr class="comentarios" valign="top"><?=$aux["abierta1"]?></tr>
   </table>
 
<form name="formulario">   
   <TABLE border="0" width="100%">
 		<tr class="destacado">Estado_auto</tr>		
		<tr class="Rojo" valign="top"><? if($aux["estado_auto"]==0)echo "Aceptado";else echo "Excluido";?></tr> 
		<tr class="destacado">Puntuacion_auto</tr>
		<tr class="Rojo" valign="top"><?=$aux["puntuacion_auto"]?></tr>
		<tr></br></tr>
		<tr class="destacado">Estado</tr>			
		<tr valign="top">
			<SELECT name="estado">
			   <OPTION value=""></OPTION>
			   <OPTION value="Aceptado" <? if($aux["estado"]=="Aceptado")echo "selected";?> >Aceptado</OPTION>
			   <OPTION value="Excluido" <? if($aux["estado"]=="Excluido")echo "selected";?> >Excluido</OPTION>
			   <OPTION value="Finalista" <? if($aux["estado"]=="Finalista")echo "selected";?> >Finalista</OPTION>
			   <OPTION value="Pendiente" <? if($aux["estado"]=="Pendiente")echo "selected";?> >Pendiente</OPTION>
			</SELECT>	
		</tr>	
		<tr class="destacado">Puntuacion</tr>
		<tr valign="top">
			<SELECT name="puntuacion">
				<OPTION value=""></OPTION>
				<? for($i=0;$i<=100;$i++){?>
			   <OPTION value="<?=$i?>" <? if($aux["puntuacion"]=="$i")echo "selected";?> ><?=$i?></OPTION>
			   <? $i+=9;} ?>  
			</SELECT>	
		</tr>	
		<tr class="destacado">Comentarios</tr>
		<tr valign="top">
			   <INPUT type="text" name="comentarios" value="<?=$aux["comentarios"] ?>"></OPTION>
		</tr>		
		<INPUT type="hidden" name="accion" value="guardar">	
		<INPUT type="hidden" name="id" value="<?=$id?>">	
		<tr valign="top">		
			<td><INPUT class="btn" type="button" name="enviar" value="Guardar" onclick="guardar()"></td>
			<td><INPUT class="btn" type="button" name="cancelar" value="Cancelar" onclick="javascript:window.close()"></td>
		</tr>					
   </table>
</form> 
</body>
</html>
<?
}
 
function guardar($id,$estado,$puntuacion,$comentarios){
	$res=ejecutar("UPDATE usuaris SET estado='$estado',puntuacion='$puntuacion',comentarios='$comentarios' WHERE id=$id");
}

?>
