<?

function get_talento($id){
	if($id>0)
	{
		$res=ejecutar("SELECT * , DATE_FORMAT( FROM_DAYS( TO_DAYS( NOW( ) ) - TO_DAYS( f_nacimiento ) ) , '%Y' ) +0 AS edad, DATE_FORMAT( f_nacimiento, '%d' ) f_nacimiento_dd, DATE_FORMAT( f_nacimiento, '%m' ) f_nacimiento_mm, DATE_FORMAT( f_nacimiento, '%Y' ) f_nacimiento_aaaa, DATE_FORMAT( f_recepcion, '%d' ) f_recepcion_dd, DATE_FORMAT( f_recepcion, '%m' ) f_recepcion_mm, DATE_FORMAT( f_recepcion, '%Y' ) f_recepcion_aaaa
			FROM talentos T
			INNER JOIN departamentos D ON T.id_departamento = D.id_departamento
			INNER JOIN areas A ON T.id_area = A.id_area
			INNER JOIN niveles N ON T.id_nivel = N.id_nivel
			INNER JOIN origenes_cv O ON T.id_origen_cv = O.id_origen_cv
			LEFT JOIN provincias P ON T.id_provincia = P.id_provincia
			INNER JOIN paises PA ON T.id_pais = PA.id_pais		
		WHERE id_talento=$id");
		return $res[0]	;
	}

}


function insertar_talento(){
	global $HTTP_POST_VARS,$sesion_id_usuario;
	extract($HTTP_POST_VARS,EXTR_PREFIX_ALL,"req");
	
	if($req_f_nacimiento_aaaa >0 && $req_f_nacimiento_mm >0 && $req_f_nacimiento_dd>0)
		$f_nacimiento=$req_f_nacimiento_aaaa."-".$req_f_nacimiento_mm."-".$req_f_nacimiento_dd;
	else
		$f_nacimiento="";
	
	if($req_id_talento>0)
	{
		$sql="UPDATE talentos SET nombre='$req_nombre', apellido1='$req_apellido1', apellido2='$req_apellido2', f_nacimiento='$f_nacimiento', id_departamento=$req_id_departamento, otro_departamento='$req_otro_departamento', id_area=$req_id_area, otra_area='$req_otra_area', id_nivel=$req_id_nivel, tiene_experiencia=$req_tiene_experiencia, trabajo_actual='$req_trabajo_actual', id_origen_cv=$req_id_origen_cv, fuente_externa_cv='$req_fuente_externa_cv', rec_nombre='$req_rec_nombre', rec_apellidos='$req_rec_apellidos', rec_empresa='$req_rec_empresa', rec_comentarios='$req_rec_comentarios', comentarios='$req_comentarios', id_pais='$req_id_pais', id_provincia='$req_id_provincia', poblacion='$req_poblacion', tel_fijo='$req_tel_fijo', tel_movil='$req_tel_movil', email='$req_email', f_mod=NOW(), f_recepcion='$req_f_recepcion_aaaa-$req_f_recepcion_mm-$req_f_recepcion_dd',  id_estado=$req_id_estado WHERE id_talento=$req_id_talento";
		ejecutar($sql);
		$id=$req_id_talento;
	}
	else
	{
		$sql="INSERT INTO talentos(nombre, apellido1, apellido2, f_nacimiento, id_departamento, otro_departamento, id_area, otra_area, id_nivel, tiene_experiencia, trabajo_actual, id_origen_cv, fuente_externa_cv, rec_nombre, rec_apellidos, rec_empresa, rec_comentarios, comentarios, id_pais, id_provincia, poblacion, tel_fijo, tel_movil, email, f_mod, f_recepcion, f_alta, id_usuario_alta, id_estado)  
		VALUES('$req_nombre', '$req_apellido1', '$req_apellido2', '$f_nacimiento', $req_id_departamento, '$req_otro_departamento', $req_id_area, '$req_otra_area', $req_id_nivel, $req_tiene_experiencia, '$req_trabajo_actual', $req_id_origen_cv, '$req_fuente_externa_cv', '$req_rec_nombre', '$req_rec_apellidos', '$req_rec_empresa', '$req_rec_comentarios','$req_comentarios', '$req_id_pais', '$req_id_provincia', '$req_poblacion', '$req_tel_fijo', '$req_tel_movil', '$req_email', NOW(), '$req_f_recepcion_aaaa-$req_f_recepcion_mm-$req_f_recepcion_dd', NOW(), $sesion_id_usuario, '$req_id_estado')";
		ejecutar($sql);
		$id =next_id();
	}
	return $id;
}

function borrar_talento(){
	global $HTTP_POST_VARS;
	extract($HTTP_POST_VARS,EXTR_PREFIX_ALL,"req");
	if($req_id_usuario>1)
	{
		$sql="DELETE FROM talentos WHERE id_talento=$req_id_talento";
		ejecutar($sql);
		
	}
	return $req_id_usuario;
}

function buscar_talentos($es_total,$criterios,$num_actual,$num_p){	
	@extract($criterios,EXTR_PREFIX_ALL,"req");
	$condicion=" 1=1 ";
	if(@$req_id!="")
		$condicion .=" AND id='$req_id' ";
		
	/*if(@$req_edad!=""){
		$a=split("-",$req_edad);
		$condicion .=" AND edad >= ".$a[0]." AND edad <=".$a[1];
	}*/
	if(@$req_edad!=""){
		$a=split("-",$req_edad);
		$condicion .=" AND DATE_FORMAT(FROM_DAYS(TO_DAYS(NOW())-TO_DAYS(edad)), '%Y')+0 >= ".$a[0]." AND DATE_FORMAT(FROM_DAYS(TO_DAYS(NOW())-TO_DAYS(edad)), '%Y')+0<=".$a[1];
	}	
	
	if(@$req_foto=="si") $condicion .=" AND foto!='' ";
	if(@$req_foto=="no") $condicion .=" AND foto='' ";
	if(@$req_pareja=="si") $condicion .=" AND pareja=1 ";
	if(@$req_pareja=="no") $condicion .=" AND pareja=0 ";	
	if(@$req_estado_auto!="") $condicion .=" AND estado_auto=$req_estado_auto ";
	if(@$req_puntuacion_auto!=""){
		$a=split("-",$req_puntuacion_auto);
		$condicion .=" AND puntuacion_auto >= ".$a[0]." AND puntuacion_auto <=".$a[1];
	}	
	if(@$req_estado!="")$condicion .=" AND estado='$req_estado' ";
	if(@$req_puntuacion!="")$condicion .=" AND puntuacion=$req_puntuacion ";	
	
	if($es_total==true)
	{	
		$sql="SELECT COUNT(*) total FROM usuaris WHERE $condicion ORDER BY puntuacion DESC,puntuacion_auto DESC";
		//echo "buscarTotal=".$sql;
		$res=ejecutar($sql);
		return @$res[0]["total"];
	}
	else
	{
		$sql="SELECT * FROM usuaris WHERE $condicion ORDER BY puntuacion DESC,puntuacion_auto DESC LIMIT ".($num_actual-1).",$num_p";
		//echo "BuscarLimitado=".$sql;
		return ejecutar($sql);
	}
	
}

function get_cv_talento($id){
	$sql="SELECT *,DATE_FORMAT(f_recepcion_cv,'%d') f_recepcion_cv_dd,DATE_FORMAT(f_recepcion_cv,'%m') f_recepcion_cv_mm,DATE_FORMAT(f_recepcion_cv,'%Y') f_recepcion_cv_aaaa,DATE_FORMAT(f_recepcion_cv,'%Y-%m-%d') f_recepcion_cv_aaaammdd,TTCV.tipo_cv FROM talentos_cv TCV INNER JOIN talentos_tipo_cv TTCV ON TCV.id_tipo_cv = TTCV.id_tipo_cv WHERE id_talento=$id ORDER BY f_recepcion_cv ASC";
	$res=ejecutar($sql);
	return $res;
}

function get_tipo_cv_talento(){
	$sql="SELECT * FROM talentos_tipo_cv ORDER BY tipo_cv";
	$res=ejecutar($sql);
	return $res;
}

function get_extension($file){
	$i = strrpos($file,'.');
	$ext = substr($file,$i+1);
	return $ext;
}

function limpiar_caracteres($cadena){
	$cadena=strtolower($cadena);
	$max=strlen($cadena);
	$nueva="";
	for($i=0;$i<$max;$i++){
		$c=substr($cadena,$i,1 );
		if(($c>='a' && $c<='z') || ($c>='0' && $c<='9') || $c=='.' ||  $c=='_' || $c=='-')
			$nueva.=$c;
		else
			$nueva.="_";
	}
	return $nueva;
}

function insertar_talento_cv($id_talento){
	global $HTTP_POST_VARS,$_FILES,$sesion_id_usuario;	
	$aux=$HTTP_POST_VARS;
	$idcv=0;
	for($i=0;isset($aux["cv_accion_$i"]);$i++){
		if($aux["cv_accion_$i"]=="modificado"){
			if($aux["id_tipo_cv_$i"]>0){
				if($aux["id_cv_$i"]>0)
				{
					$sql="UPDATE talentos_cv SET id_talento=$id_talento, id_tipo_cv=".$aux["id_tipo_cv_$i"].", descripcion_cv='".$aux["descripcion_cv_$i"]."', url_cv='".$aux["url_cv_$i"]."',  f_recepcion_cv='".$aux["f_recepcion_cv_aaaa_$i"]."-".$aux["f_recepcion_cv_mm_$i"]."-".$aux["f_recepcion_cv_dd_$i"]."', f_mod=NOW(), id_usuario=$sesion_id_usuario WHERE id_cv=".$aux["id_cv_$i"];
					ejecutar($sql);
					$idcv=$aux["id_cv_$i"];
				}
				else
				{
					$sql="INSERT INTO talentos_cv(id_talento, id_tipo_cv, descripcion_cv, url_cv,  f_recepcion_cv, f_alta, f_mod, id_usuario) 
				    VALUES($id_talento, ".$aux["id_tipo_cv_$i"].", '".$aux["descripcion_cv_$i"]."', '".$aux["url_cv_$i"]."', '".$aux["f_recepcion_cv_aaaa_$i"]."-".$aux["f_recepcion_cv_mm_$i"]."-".$aux["f_recepcion_cv_dd_$i"]."', NOW(), NOW(), $sesion_id_usuario)";
					ejecutar($sql);
					$idcv=next_id();
				}
				$aux_file=$_FILES["fichero_cv_".$i];
				if($aux_file["name"]!=""){
					//copia fichero
					if(!file_exists("talentos/$id_talento")){
						mkdir("talentos/$id_talento");
					}
					
					$file=limpiar_caracteres($idcv."_".$id_talento."_".$aux_file["name"]);
					
					move_uploaded_file($aux_file["tmp_name"],"talentos/$id_talento/".$file);
					$sql="UPDATE talentos_cv SET fichero_cv = '".$file."' WHERE id_cv=$idcv";
					ejecutar($sql);
				}
			}else{
				// borrar CV
				if($aux["id_cv_$i"]>0){
					$sql="SELECT id_talento,fichero_cv FROM talentos_cv WHERE id_cv=".$aux["id_cv_$i"];
					$res_aux=ejecutar($sql);
					if(count($res_aux)>0){
						$res_aux=$res_aux[0];
						@unlink("talentos/".$res_aux["id_talento"]."/".$res_aux["fichero_cv"]);
						$sql="DELETE FROM talentos_cv WHERE id_talento=".$res_aux["id_talento"]." AND id_cv=".$aux["id_cv_$i"];
						$res_aux=ejecutar($sql);
					}
				}
			}	
		}
	}
}

function cv_html($i,$aux_cv,$rec_tipo_cv){
	global $sesion_usuario;
	if($sesion_usuario["mod_tal_cv_mod"]!=1)
		return ver_cv_html($i,$aux_cv,$rec_tipo_cv);
	
	
	$max_tipo_cv=count($rec_tipo_cv);
?>
	<table>
	<tr><td class="destacado" valign="top">
	<INPUT TYPE="hidden" name="id_cv_<?=$i?>" value="<?=@$aux_cv["id_cv"]?>">
	<INPUT TYPE="hidden" name="cv_accion_<?=$i?>" value="">
	Fecha:</td><td><SELECT name="f_recepcion_cv_dd_<?=$i?>"  onchange="cambio_en_cv(<?=$i?>)">
<?
   for($d=1;$d<=31;$d++){
   	$selected="";
	if(($d==@$aux_cv["f_recepcion_cv_dd"]&& @$aux_cv["f_recepcion_cv_dd"]>0) || (@$aux_cv["f_recepcion_cv_dd"]=="" && $d == date('j'))) $selected=" selected ";
	?><option value="<?=$d?>" <?=$selected?>><?=$d?></option><?
   }
   ?>
   </SELECT> / <SELECT name="f_recepcion_cv_mm_<?=$i?>" onchange="cambio_en_cv(<?=$i?>)">
   <?
   for($m=1;$m<=12;$m++){
   	$selected="";
	if(($m==@$aux_cv["f_recepcion_cv_mm"] && @$aux_cv["f_recepcion_cv_mm"]>0)|| ($m==date('n') && @$aux_cv["f_recepcion_cv_mm"]=="")) $selected=" selected ";
	?><option value="<?=$m?>" <?=$selected?>><?=$m?></option><?
   }
   ?>
   </SELECT> / <SELECT name="f_recepcion_cv_aaaa_<?=$i?>" onchange="cambio_en_cv(<?=$i?>)">
   <?
   $max_aa=date("Y")+1;
   for($m=$max_aa;$m>=2005;$m--){
   	$selected="";
	if(($m==@$aux_cv["f_recepcion_cv_aaaa"] && @$aux_cv["f_recepcion_cv_aaaa"]>0) || ($m==date('Y') && @$aux_cv["f_recepcion_cv_aaaa"]=="")) $selected=" selected ";
	?><option value="<?=$m?>" <?=$selected?>><?=$m?></option><?
   }
   ?>
   </select></td><td class="destacado">Tipo:</td><td><SELECT name="id_tipo_cv_<?=$i?>" onchange="cambio_en_cv(<?=$i?>);cambia_tipo_cv(this,<?=$i?>)">
   <?if($sesion_usuario["mod_tal_cv_baja"]==1){?><OPTION value=""> vacio / eliminar</OPTION><?}?>
	<?for($j=0;$j<$max_tipo_cv;$j++){
	$selected="";
	if($rec_tipo_cv[$j]["id_tipo_cv"]==@$aux_cv["id_tipo_cv"]) $selected=" selected ";
	?>
	<OPTION VALUE="<?=$rec_tipo_cv[$j]["id_tipo_cv"]?>" <?=$selected?>><?=$rec_tipo_cv[$j]["tipo_cv"]?></OPTION>
	<?}?></SELECT> (*)</td></tr>
	<tr id="tr_cv_3_<?=$i?>"><td class="destacado">URL</td><td><?=@$aux_cv["url_cv"]?></td><td><b>Modificar URL</b></td><td><INPUT type="text" name="url_cv_<?=$i?>" value="<?=@$aux_cv["url_cv"]?>" onchange="cambio_en_cv(<?=$i?>)"/></td></tr>
	<tr id="tr_cv_1_<?=$i?>"><td class="destacado">FICHERO</td><td><?if(@$aux_cv["fichero_cv"]!=""){?><a href="talentos/<?=$aux_cv["id_talento"]?>/<?=$aux_cv["fichero_cv"]?>"><?=@$aux_cv["fichero_cv"]?></a><?}?></td><td><b>Modificar Fichero:</b></td><td><INPUT type="file" name="fichero_cv_<?=$i?>"  value="<?=@$aux_cv["url_cv"]?>" onchange="cambio_en_cv(<?=$i?>)"/></td></tr>

	<td valign="top" class="destacado" colspan="4">Descripción:</td></tr>
	<tr><td colspan="4" align="center"><TEXTAREA name="descripcion_cv_<?=$i?>" rows="3" cols="60" onchange="cambio_en_cv(<?=$i?>)"><?=@$aux_cv["descripcion_cv"]?></TEXTAREA></td></tr>
	<tr><td colspan="4">(*) Para eliminar un CV deje el tipo de CV vacio</td></tr>	
	<tr><td colspan="4"><hr></td></tr>	
	</table>
	<SCRIPT>
	//cambia_tipo_cv(document.forms["formulario"].id_tipo_cv_<?=$i?>,<?=$i?>);
	</SCRIPT>		
	<?
}

function ver_cv_html($i,$aux_cv,$rec_tipo_cv){
	$max_tipo_cv=count($rec_tipo_cv);
?>
	<table width="100%">
	<tr><td class="destacado" valign="top">
	Fecha:</td><td><?=@$aux_cv["f_recepcion_cv_dd"]?>/<?=@$aux_cv["f_recepcion_cv_mm"]?>/<?=@$aux_cv["f_recepcion_cv_aaaa"]?></td><td class="destacado">Tipo:</td><td><?=@$aux_cv["tipo_cv"]?></td></tr>
	<tr><td class="destacado">URL</td><td colspan="3"><?
		if($aux_cv["url_cv"]!=""){?>
		<a href="<?=$aux_cv["url_cv"]?>" target="_blank"><?=$aux_cv["url_cv"]?><!--IMG src="img/ico/url.gif" border="0"--></a>
		<?}?>
		</td></tr>
	<tr><td class="destacado">FICHERO</td><td colspan="3"><?
		if(get_extension($aux_cv["fichero_cv"])!=""){
		?><a href="talentos/<?=$aux_cv["id_talento"]?>/<?=$aux_cv["fichero_cv"]?>"><?=$aux_cv["fichero_cv"]?><!--IMG src="img/ico/<?=get_extension($aux_cv["fichero_cv"])?>.gif" border="0"--></a><?
		}

	?></td></tr>
	<td valign="top" class="destacado" colspan="4">Descripción:</td></tr>
	<tr><td colspan="4" align="center"><?=@nl2br($aux_cv["descripcion_cv"])?></td></tr>
	<tr><td colspan="4"><hr></td></tr>	
	</table>
	<?
}
?>