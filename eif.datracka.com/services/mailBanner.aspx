<%@ Register TagPrefix="recaptcha" Namespace="Recaptcha" Assembly="Recaptcha" %>

<script runat=server>
    Sub btnSubmit_Click(ByVal sender As Object, ByVal e As EventArgs)
        If Page.IsValid Then
            lblResult.Text = "You Got It!"
            lblResult.ForeColor = Drawing.Color.Green
        Else
            lblResult.Text = "Incorrect"
            lblResult.ForeColor = Drawing.Color.Red
        End If
    End Sub
</script>


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
                
                if(form.name.value == ""){
                    alert("Debe introducir su nombre");
                    form.name.focus();
                }else if(form.email.value == ""){
                    alert("Debe introducir su email");
                    form.email.focus();
				}else if(!form.lopd.checked){
					alert("Debe aceptar las condiciones legales");
                }else{
                    form.submit();
                }
            }
			
			function openPopup(_html, _name, _widht, _height ) {
				window.open(_html, "window", "width="+_widht+",height="+_height+",toolbar=no,directories=no,menubar=no,status=no,scrollbars=1;");
			}
			
        </script>        
    </head>
    <body>
    <form runat="server">
    	<div id="content">
	    	<div id="cabecera"></div>
            <div id="form">
            	<p><img src="img/txtQuiresIr.jpg" alt="¿Quieres ir al Eristoff Internative Festival?" /></p>
                <p><img src="img/txtDejaEmail.jpg" alt="Deja tu email" /></p>
                <!--form name="formulario" method="post" action="http://www.eristoffinternativefestival.com/services/altaconcurso.aspx"-->
                	<input type="hidden" name="dateofbirth" value="01/01/1978" />
                    <input type="hidden" name="url" value="http://www.eristoffinternativefestival.com/services/gracias.html" />
                	<label for="name"><img src="img/txtNombre.gif" alt="Nombre" />&nbsp;
                    <input type="text" name="name" maxlength="128" /></label>
                    <label for="email"><img src="img/txtEmail.gif" alt="Email" />&nbsp;&nbsp;&nbsp;&nbsp;
                    <input type="text" name="email" maxlength="128" /></label>
                    <label><input type="checkbox" name="lopd" value="true" class="chk" />&nbsp;<a href="javascript:openPopup('http://www.eristoffinternativefestival.com/popup/terminos_legales.html','Terminos Legales','350','250')" title="Enviar"><img src="img/txtLegales.gif" border="0" /></a></label>
					 <p><asp:Label Visible="true" ID="lblResult" runat="server" />
</p>
					<p><recaptcha:RecaptchaControl ID="recaptcha" runat="server" PublicKey="6LdtMggAAAAAAMacq5Ho3rS6aljORkEfqIqc4dgy" PrivateKey="6LdtMggAAAAAAKk_H1XAfM_Lq3f00xRd_nOVURWq" /></p>
 <p><asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" /></p>
					<p><a href="javascript:enviar()" title="Enviar"><img src="img/btEnviar.gif" alt="Enviar" border="0" /></a></p>
                	<p><img src="img/txtIrWeb.jpg" alt="Ir a la web de Eristoff Internativ Festival" />&nbsp;<a href="http://www.eristoffinternativefestival.com"><img src="img/btnIrWeb.jpg" alt="boton" border="0" /></a></p>
                <!--/form-->
            </div>
    	</div></form>
    </body>
</html>
