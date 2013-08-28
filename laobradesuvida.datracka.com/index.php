<?php

	include("./php/includes.inc.php");
	
	function curPageURL() {
		 $pageURL = 'http';
		 if ($_SERVER["HTTPS"] == "on") {$pageURL .= "s";}
		 $pageURL .= "://";
		 if ($_SERVER["SERVER_PORT"] != "80") {
		  $pageURL .= $_SERVER["SERVER_NAME"].":".$_SERVER["SERVER_PORT"].$_SERVER["REQUEST_URI"];
		 } else {
		  $pageURL .= $_SERVER["SERVER_NAME"].$_SERVER["REQUEST_URI"];
		 }
		 return $pageURL;
	}
/*
$key = 'asdijfhaiusdfhasodifuhasdifuhasd';
$iv = substr( md5(mt_rand(),true), 0, 8 );
$data  = $_GET[id_contacto];
*/

//echo "MMMM".rawurldecode($_GET[id_contacto])."MMMM";


	/* recuperamos parametros por GET */
	//if(isset($_GET['id_contacto'])) $id_contacto = $_GET['id_contacto'];else $id_contacto = "0";
	if(isset($_GET['id_contacto'])) {
	
		$idParseado = str_replace('porciento','%',$_GET['id_contacto']);
		if ($_GET[procedencia] == "FACEBOOK"){
			$idParseado = rawurldecode($idParseado);
			//echo "viene de facebook, decode: ".$idParseado."<br />";
		}
		//echo "ID PARSEADO: ".$idParseado."<br />";
		//echo "De Facebook: ".$idParseado."<br />";
		$id_contacto = decrypt($idParseado);
		//echo "ID CONTACTO decrypt: ".$id_contacto."<br />";
		$idCodificado1 = rawurldecode($idParseado);
		//echo "decode: ".$idCodificado1."<br />";
		$idCodificado = rawurlencode($idCodificado1);
		//echo "encode: ".$idCodificado."<br />";	
		//$idCodificadoHtml = htmlentities($idCodificado1);
		//echo "html: ".$idCodificadoHtml."<br />";
		//echo "Test Codigo: ".rawurlencode("@-_.*+/")."<br />";
				
	} else {
		$id_contacto = "0";
	}
	if(isset($_GET['procedencia'])) $procedencia = $_GET['procedencia'];else $procedencia = "BANNER";
	if(isset($_GET['idioma'])) $idioma = $_GET['idioma'];else $idioma = "es";
	if(isset($_GET['token'])) $token = $_GET['token'];else $token = "cualificacionAudiclass.2";
	//if(isset($_GET['urlfb'])) $urlfb = $_GET['urlfb'];else 
	

	if ($procedencia == "BANNER"){ //si procedencia banner montamos URL para FB
		$urlfb = $contextPath."?id_contacto=".$id_contacto."&procedencia=FACEBOOK&idioma=".$idioma."&token=cualificacionAudiclass.2";
	}
	if ($procedencia == "FACEBOOK"){ //
//	http://localhost/laobradetuvida?id_contacto=1234455&procedencia=FACEBOOK&idioma=en&token=cualificacionAudiclass.2
		$urlfb = curPageURL();	// obtenemos url de FB
	}




// FACEBOOK METAS

if (isset($_GET[name]) && isset($_GET[surname])){
	//Metas personalizados para facebook
	$title = "La obra de la vida de ".$_GET[name]." ".$_GET[surname];
	$descName = $_GET[name];
	
	//venimos de Facebook, recodificamos la variable
	$idCodificado = str_replace('porciento','%',$_GET[id_contacto]);
	
}else{
	//metas por defecto
	$title = "La obra de su vida";
	$descName = "usted";
}
if (isset($_GET[s])&&(strtoupper($_GET[s]) == "SRA")){
	$unicoTxt = "la hacen &uacute;nica";
}else{
	$unicoTxt = "le hacen &uacute;nico";
}



//Hacemos la url dinámica y se la pasamos al flash, para no tener que modificar el swf en función del server

$urlName = "http://".$_SERVER[SERVER_NAME].$_SERVER[PHP_SELF];

?>
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en" lang="en">
<head>
<title><?php echo $title; ?></title>
<meta http-equiv="Content-Type" content="text/html; charset=ISO-8859-1" />
<meta name="title" content="<?php echo $title; ?>" />
<meta name="description" content="Esta es la pieza que Audi ha creado para <?php echo $descName; ?>, basada en los detalles de su vida que <?php echo $unicoTxt;?>. Una melod&iacute;a personalizada que t&uacute; tambi&eacute;n puedes vivir. " />
<link rel="image_src" href="facebookImage.jpg" / >

<script language="JavaScript" type="text/javascript">

function facebookCall(url){
	var w = 700;
	var h = 500;
	var myleft=(screen.width)?(screen.width-w)/2:100;mytop=(screen.height)?(screen.height-h)/2:100;
	var gotoUrl = url;
	var settings="width=" + w + ",height=" + h + ",left=" + myleft + ",scrollbars=no,location=no,directories=no,status=no,menubar=no,toolbar=no,resizable=no";
	win=window.open(gotoUrl,'',settings);
	win.focus();
}


<!--
//v1.7
// Flash Player Version Detection
// Detect Client Browser type
// Copyright 2005-2008 Adobe Systems Incorporated.  All rights reserved.
var isIE  = (navigator.appVersion.indexOf("MSIE") != -1) ? true : false;
var isWin = (navigator.appVersion.toLowerCase().indexOf("win") != -1) ? true : false;
var isOpera = (navigator.userAgent.indexOf("Opera") != -1) ? true : false;
function ControlVersion()
{
	var version;
	var axo;
	var e;
	// NOTE : new ActiveXObject(strFoo) throws an exception if strFoo isn't in the registry
	try {
		// version will be set for 7.X or greater players
		axo = new ActiveXObject("ShockwaveFlash.ShockwaveFlash.7");
		version = axo.GetVariable("$version");
	} catch (e) {
	}
	if (!version)
	{
		try {
			// version will be set for 6.X players only
			axo = new ActiveXObject("ShockwaveFlash.ShockwaveFlash.6");
			
			// installed player is some revision of 6.0
			// GetVariable("$version") crashes for versions 6.0.22 through 6.0.29,
			// so we have to be careful. 
			
			// default to the first public version
			version = "WIN 6,0,21,0";
			// throws if AllowScripAccess does not exist (introduced in 6.0r47)		
			axo.AllowScriptAccess = "always";
			// safe to call for 6.0r47 or greater
			version = axo.GetVariable("$version");
		} catch (e) {
		}
	}
	if (!version)
	{
		try {
			// version will be set for 4.X or 5.X player
			axo = new ActiveXObject("ShockwaveFlash.ShockwaveFlash.3");
			version = axo.GetVariable("$version");
		} catch (e) {
		}
	}
	if (!version)
	{
		try {
			// version will be set for 3.X player
			axo = new ActiveXObject("ShockwaveFlash.ShockwaveFlash.3");
			version = "WIN 3,0,18,0";
		} catch (e) {
		}
	}
	if (!version)
	{
		try {
			// version will be set for 2.X player
			axo = new ActiveXObject("ShockwaveFlash.ShockwaveFlash");
			version = "WIN 2,0,0,11";
		} catch (e) {
			version = -1;
		}
	}
	
	return version;
}
// JavaScript helper required to detect Flash Player PlugIn version information
function GetSwfVer(){
	// NS/Opera version >= 3 check for Flash plugin in plugin array
	var flashVer = -1;
	
	if (navigator.plugins != null && navigator.plugins.length > 0) {
		if (navigator.plugins["Shockwave Flash 2.0"] || navigator.plugins["Shockwave Flash"]) {
			var swVer2 = navigator.plugins["Shockwave Flash 2.0"] ? " 2.0" : "";
			var flashDescription = navigator.plugins["Shockwave Flash" + swVer2].description;
			var descArray = flashDescription.split(" ");
			var tempArrayMajor = descArray[2].split(".");			
			var versionMajor = tempArrayMajor[0];
			var versionMinor = tempArrayMajor[1];
			var versionRevision = descArray[3];
			if (versionRevision == "") {
				versionRevision = descArray[4];
			}
			if (versionRevision[0] == "d") {
				versionRevision = versionRevision.substring(1);
			} else if (versionRevision[0] == "r") {
				versionRevision = versionRevision.substring(1);
				if (versionRevision.indexOf("d") > 0) {
					versionRevision = versionRevision.substring(0, versionRevision.indexOf("d"));
				}
			}
			var flashVer = versionMajor + "." + versionMinor + "." + versionRevision;
		}
	}
	// MSN/WebTV 2.6 supports Flash 4
	else if (navigator.userAgent.toLowerCase().indexOf("webtv/2.6") != -1) flashVer = 4;
	// WebTV 2.5 supports Flash 3
	else if (navigator.userAgent.toLowerCase().indexOf("webtv/2.5") != -1) flashVer = 3;
	// older WebTV supports Flash 2
	else if (navigator.userAgent.toLowerCase().indexOf("webtv") != -1) flashVer = 2;
	else if ( isIE && isWin && !isOpera ) {
		flashVer = ControlVersion();
	}	
	return flashVer;
}
// When called with reqMajorVer, reqMinorVer, reqRevision returns true if that version or greater is available
function DetectFlashVer(reqMajorVer, reqMinorVer, reqRevision)
{
	versionStr = GetSwfVer();
	if (versionStr == -1 ) {
		return false;
	} else if (versionStr != 0) {
		if(isIE && isWin && !isOpera) {
			// Given "WIN 2,0,0,11"
			tempArray         = versionStr.split(" "); 	// ["WIN", "2,0,0,11"]
			tempString        = tempArray[1];			// "2,0,0,11"
			versionArray      = tempString.split(",");	// ['2', '0', '0', '11']
		} else {
			versionArray      = versionStr.split(".");
		}
		var versionMajor      = versionArray[0];
		var versionMinor      = versionArray[1];
		var versionRevision   = versionArray[2];
        	// is the major.revision >= requested major.revision AND the minor version >= requested minor
		if (versionMajor > parseFloat(reqMajorVer)) {
			return true;
		} else if (versionMajor == parseFloat(reqMajorVer)) {
			if (versionMinor > parseFloat(reqMinorVer))
				return true;
			else if (versionMinor == parseFloat(reqMinorVer)) {
				if (versionRevision >= parseFloat(reqRevision))
					return true;
			}
		}
		return false;
	}
}
function AC_AddExtension(src, ext)
{
  if (src.indexOf('?') != -1)
    return src.replace(/\?/, ext+'?'); 
  else
    return src + ext;
}
function AC_Generateobj(objAttrs, params, embedAttrs) 
{ 
  var str = '';
  if (isIE && isWin && !isOpera)
  {
    str += '<object ';
    for (var i in objAttrs)
    {
      str += i + '="' + objAttrs[i] + '" ';
    }
    str += '>';
    for (var i in params)
    {
      str += '<param name="' + i + '" value="' + params[i] + '" /> ';
    }
    str += '</object>';
  }
  else
  {
    str += '<embed ';
    for (var i in embedAttrs)
    {
      str += i + '="' + embedAttrs[i] + '" ';
    }
    str += '> </embed>';
  }
  document.write(str);
}
function AC_FL_RunContent(){
  var ret = 
    AC_GetArgs
    (  arguments, ".swf", "movie", "clsid:d27cdb6e-ae6d-11cf-96b8-444553540000"
     , "application/x-shockwave-flash"
    );
  AC_Generateobj(ret.objAttrs, ret.params, ret.embedAttrs);
}
function AC_SW_RunContent(){
  var ret = 
    AC_GetArgs
    (  arguments, ".dcr", "src", "clsid:166B1BCA-3F9C-11CF-8075-444553540000"
     , null
    );
  AC_Generateobj(ret.objAttrs, ret.params, ret.embedAttrs);
}
function AC_GetArgs(args, ext, srcParamName, classid, mimeType){
  var ret = new Object();
  ret.embedAttrs = new Object();
  ret.params = new Object();
  ret.objAttrs = new Object();
  for (var i=0; i < args.length; i=i+2){
    var currArg = args[i].toLowerCase();    
    switch (currArg){	
      case "classid":
        break;
      case "pluginspage":
        ret.embedAttrs[args[i]] = args[i+1];
        break;
      case "src":
      case "movie":	
        args[i+1] = AC_AddExtension(args[i+1], ext);
        ret.embedAttrs["src"] = args[i+1];
        ret.params[srcParamName] = args[i+1];
        break;
      case "onafterupdate":
      case "onbeforeupdate":
      case "onblur":
      case "oncellchange":
      case "onclick":
      case "ondblclick":
      case "ondrag":
      case "ondragend":
      case "ondragenter":
      case "ondragleave":
      case "ondragover":
      case "ondrop":
      case "onfinish":
      case "onfocus":
      case "onhelp":
      case "onmousedown":
      case "onmouseup":
      case "onmouseover":
      case "onmousemove":
      case "onmouseout":
      case "onkeypress":
      case "onkeydown":
      case "onkeyup":
      case "onload":
      case "onlosecapture":
      case "onpropertychange":
      case "onreadystatechange":
      case "onrowsdelete":
      case "onrowenter":
      case "onrowexit":
      case "onrowsinserted":
      case "onstart":
      case "onscroll":
      case "onbeforeeditfocus":
      case "onactivate":
      case "onbeforedeactivate":
      case "ondeactivate":
      case "type":
      case "codebase":
      case "id":
        ret.objAttrs[args[i]] = args[i+1];
        break;
      case "width":
      case "height":
      case "align":
      case "vspace": 
      case "hspace":
      case "class":
      case "title":
      case "accesskey":
      case "name":
      case "tabindex":
        ret.embedAttrs[args[i]] = ret.objAttrs[args[i]] = args[i+1];
        break;
      default:
        ret.embedAttrs[args[i]] = ret.params[args[i]] = args[i+1];
    }
  }
  ret.objAttrs["classid"] = classid;
  if (mimeType) ret.embedAttrs["type"] = mimeType;
  return ret;
}
// -->
</script>
<style type="text/css">
<!--
body {
	margin-left: 0px;
	margin-top: 0px;
	margin-right: 0px;
	margin-bottom: 0px;
	background-color: #333;
}
-->
</style></head>
<body bgcolor="#ffffff">



<table width="100%" height="100%" border="0" align="center" cellpadding="0" cellspacing="0">
  <tr>
    <td align="center" valign="middle"><script language="JavaScript" type="text/javascript">
	AC_FL_RunContent(
		'codebase', 'http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=10,0,0,0',
		'width', '900',
		'height', '570',
		'src', 'la_obra_de_tu_vida',
		'quality', 'high',
		'pluginspage', 'http://www.adobe.com/go/getflashplayer',
		'align', 'middle',
		'play', 'true',
		'loop', 'true',
		'scale', 'showall',
		'wmode', 'window',
		'devicefont', 'false',
		'id', 'la_obra_de_tu_vida',
		'bgcolor', '#ffffff',
		'name', 'la_obra_de_tu_vida',
		'menu', 'true',
		'allowFullScreen', 'false',
		'allowScriptAccess','sameDomain',
		'movie', 'la_obra_de_tu_vida',
		'FlashVars', 'id_contacto=<?php echo $id_contacto; ?>&idCodificado=<?php echo $idCodificado; ?>&token=<?php  echo $token; ?>&idioma=<?php echo $idioma; ?>&procedencia=<?php echo $procedencia; ?>&urlName=<?php echo $urlName; ?>',
		'salign', ''
		); //end AC code
    </script>
      <noscript>
      <object classid="clsid:d27cdb6e-ae6d-11cf-96b8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=10,0,0,0" width="900" height="570" id="la_obra_de_tu_vida" align="middle">
        <param name="allowScriptAccess" value="sameDomain" />
        <param name="allowFullScreen" value="false" />
        <param name="movie" value="la_obra_de_tu_vida.swf" />
        <param name="quality" value="high" />
        <param name="bgcolor" value="#ffffff" />
        <param name="FlashVars" value="id_contacto=<?php echo $id_contacto; ?>&idCodificado=<?php echo $idCodificado; ?>&token=<?php echo $token; ?>&idioma=<?php echo $idioma; ?>&procedencia=<?php echo $procedencia; ?>&urlName=<?php echo $urlName; ?>">
        <embed src="la_obra_de_tu_vida.swf" width="900" height="570" align="middle" quality="high" bgcolor="#ffffff" name="la_obra_de_tu_vida" allowScriptAccess="sameDomain" allowFullScreen="false" type="application/x-shockwave-flash" pluginspage="http://www.adobe.com/go/getflashplayer" flashvars="animacion=1" />      
</object>
    </noscript></td>
  </tr>
</table>

<!-- Script de google analitycs -->
	<script type="text/javascript">
		var gaJsHost = (("https:" == document.location.protocol) ? "https://ssl." : "http://www.");
		document.write(unescape("%3Cscript src='" + gaJsHost + "google-analytics.com/ga.js' type='text/javascript'%3E%3C/script%3E"));
		</script>
		<script type="text/javascript">
		try {
		var pageTracker = _gat._getTracker("UA-1534016-20");
		pageTracker._trackPageview();
		} catch(err) {}
	</script>
<!-- fin script de google analitycs -->
<?php //echo $id_contacto; ?></br>
<?php //echo $_GET['id_contacto']; ?></br>
</body>
</html>