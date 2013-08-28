<?php
	
	include("./cargar_includes.inc.php");

if ($_FILES['Filedata']['name']) {
	
	$in = new input($_GET,DEBUG);
	$nom = strtolower(addslashes($in->get("nom")));
	$ext = strtolower(addslashes($in->get("ext")));

	$md5 = strtolower(addslashes($in->get("md5")));
	/*$md5 = $nom.$ext.rand().time();
	$md5= md5($md5);*/	
	
	$uploadDir = 'tmp/';
	$uploadFile = $uploadDir.$md5.$ext;

	move_uploaded_file($_FILES['Filedata']['tmp_name'], $uploadFile);
	chmod($uploadFile,0777);

//Propietats thumbmnail****************************************

	$TamMax = 200;
	//set the quality of the JPEG image (0-100)
	$quality = 80;
	#set the name of the thumbnail file
	if($ext==".jpg")$filename_thumb = $md5 . ".jpg";
	if($ext==".jpeg")$filename_thumb = $md5 . ".jpeg";
	if($ext==".JPG")$filename_thumb = $md5 . ".JPG";
	if($ext==".JPEG")$filename_thumb = $md5 . ".JPEG";
	//In order to create the thumbnail we must work out the relative height of the image. We are using a fixed width of 100 pixels and the height will be in proportion to the width.
	#get the uploaded image size
	$imagesize = @getimagesize($uploadFile);
	#width of uploaded image
	$imagesize_w = $imagesize[0];
	#height of uploaded image
	$imagesize_h = $imagesize[1];
	
//Mantenim Escalatge***************************************	
	if($imagesize_w == $imagesize_h){$width = $TamMax;$height = $TamMax;}
	if($imagesize_w > $imagesize_h){
		$width = $TamMax;
		$height = (100 / ($imagesize_w / $width)) * .01;
		$height = @round($imagesize_h * $height);
	}
	if($imagesize_w < $imagesize_h){
		$height = $TamMax;
		$width = (100 / ($imagesize_h / $height)) * .01;
		$width = @round($imagesize_w * $width);
	}	

//Guardar thumbmnail****************************************

	//The next step is to create the thumbnail image from the original image, using the width, height, and quality settings.
	#create new instance of the image
	if($ext==".jpg" || $ext==".jpeg" || $ext==".JPG" || $ext==".JPEG")$image = ImageCreateFromJPEG($uploadFile);
	if ($image) {	
		//Creem path
		$uploadDirThumb = "img/";	
		$nivel=substr($md5, 0, 2);
		if(!file_exists($uploadDirThumb."$nivel")){
			mkdir($uploadDirThumb."$nivel",0777);
			@chmod($uploadDirThumb."$nivel",0777);
		}
		$pathThumb=$uploadDirThumb."$nivel/$md5";
		if(!file_exists($pathThumb)){
			mkdir($pathThumb,0777);
			@chmod($pathThumb,0777);
		}

		#create the blank thumbnail image
		$thumb = @ImageCreateTrueColor($width, $height);
		#resize the image into the thumbnail image
		@ImageCopyResampled($thumb, $image, 0, 0, 0, 0, $width, $height, $imagesize_w, $imagesize_h);
		#save the thumbnail using the filename and quality
	if($ext==".jpg" || $ext==".jpeg" || $ext==".JPG" || $ext==".JPEG")imagejpeg($thumb,$pathThumb."/imagen.jpg", $quality);
	}else{
		echo 'Error creating thumbnail image';
	}	
	
	
//Borro foto no reescalada
	unlink($uploadFile);
//Permisos Thumbnail
	chmod ($pathThumb."/imagen.jpg", 0777);
	
	echo "md5=".$md5;
}

 ?>