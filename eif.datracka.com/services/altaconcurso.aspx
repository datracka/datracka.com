﻿<%@ Page Language="VB" AutoEventWireup="false" CodeFile="altaconcurso.aspx.vb" Inherits="services_altaconcurso" %>
<%@ Register Assembly="Lanap.BotDetect" Namespace="Lanap.BotDetect" TagPrefix="BotDetect" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
    <head>
        <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
        <title>&iquest;Quieres ir al Eristoff Internative Festival?</title>
        
        <link type="text/css" rel="stylesheet" href="css/reset.css" />
        <link type="text/css" rel="stylesheet" href="css/estilos.css" />
        
		<script type="text/javascript" language="javascript">
            function enviar(){
                var form = document.forms[0];
                   
			   if(form.CodeTextBox.value == ""){
                    alert("Debe introducir el texto de la imagen");
                    form.CodeTextBox.focus();
                    return false;
				}else if(!form.lopd.checked){
					alert("Debe aceptar las condiciones legales");
					return false;
                }else if((form.name.value == "")&&(form.email.value != "")){
					alert("Debe introducir su nombre");
                    form.name.focus();
                    return false;
				} else if((form.name.value != "")&&(form.email.value == "")){
					alert("Debe introducir su email");
                    form.email.focus();
                    return false;
				} else {
                    form.submit();
                    return true;
                }
            }
			
			function openPopup(_html, _name, _widht, _height ) {
				window.open(_html, "window", "width="+_widht+",height="+_height+",toolbar=no,directories=no,menubar=no,status=no,scrollbars=1;");
			}
			
        </script>     
        <style type="text/css">
        .boton222 {

	   background-image:url(img/btEnviar.gif);
	   height:20px;
	   width:55px;
        }
        </style>   
    </head>
    <body>
    	<div id="content">
	    	<div id="cabecera"></div>
            <div id="form">
            	<p><img src="img/txtQuiresIr.jpg" alt="¿Quieres ir al Eristoff Internative Festival?" /></p>
                <form name="formulario" method="post"  runat="server" onsubmit="return enviar();">
                	<input type="hidden" name="dateofbirth" value="01/01/1978" />
                    <input type="hidden" name="url" value="http://www.eristoffinternativefestival.com/services/gracias.html" />
                    <input type="hidden" name="guid" value="<%= Request.Querystring("guid") %>" />
                	<BotDetect:Captcha ID="SampleCaptcha" runat="server"  />
					<label for="CodeTextBox"><img src="img/txtImagen.gif" alt="Email" />&nbsp;
						<asp:TextBox ID="CodeTextBox" runat="server" Width="100" MaxLength = "5" ></asp:TextBox> 
						<asp:Label ID="MessageCorrectLabel" runat="server" Text="Label" Width="100"></asp:Label>
					</label>
						
					<label for="texto">
						<img src="img/txtDejaEmail.jpg" alt="Deja tu email" />
                    </label>	
						
					<label for="name"><img src="img/txtNombre.gif" alt="Nombre" />&nbsp;
                     <asp:TextBox ID="name" runat="server" MaxLength = "128"></asp:TextBox>
                    </label>
                    <label for="email"><img src="img/txtEmail.gif" alt="Email" />&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:TextBox ID="email" runat="server" MaxLength="128"></asp:TextBox>
                    </label>
                    
					
                   <label><input type="checkbox" name="lopd" value="true" class="chk" />&nbsp;<a href="javascript:openPopup('http://www.eristoffinternativefestival.com/popup/terminos_legales.html','Terminos Legales','350','250')" title="Enviar"><img src="img/txtLegales.gif" border="0" /></a></label>
					
            <p><asp:Button ID="ValidateButton" runat="server" CssClass="boton222" Width="55" Height="20" /></p>
					

                	<p><img src="img/txtIrWeb.jpg" alt="Ir a la web de Eristoff Internativ Festival" />&nbsp;<a href="http://www.eristoffinternativefestival.com"><img src="img/btnIrWeb.jpg" alt="boton" border="0" /></a></p>
                </form>
            </div>
    	</div>
    </body>
</html>

