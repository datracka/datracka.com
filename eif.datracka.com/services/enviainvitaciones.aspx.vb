Imports System.Xml
Imports System.Web
Imports System.Net
Imports System.Configuration.ConfigurationManager
Partial Class enviainvitaciones
    Inherits System.Web.UI.Page
    ' inicializamos el objeto de datos
    Dim da As New datos
    Dim intRespuesta As Integer = 0
    Dim mimail As New correo
    Dim f As New fichero

    Dim stMail As String = ""


    Dim stGuidgrupo As String = ""
    Dim stFirstname As String = ""
    Dim nUserId As Integer = 0
    Dim stAmigo1 As String = ""
    Dim stAmigo2 As String = ""
    Dim stAmigo3 As String = ""
    Dim stAmigo4 As String = ""
    Dim stAmigo5 As String = ""

    Dim nRespuestaM As Integer = 0

    Dim correo As New correo

    Dim stError As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load



        ' Compruebo que el nombre del grupo no existe, si viene informado

        If Trim("" & Request.Form("guidgroup")) <> "" And Trim("" & Request.Form("nombre")) <> "" Then
            ' Cargo variables que vienen del formulario
            InicializarVariables()

            GestionaManada()

        Else
            stError = "Faltan datos"
        End If


        ' SALIDA DE TEXTO
        Response.Write("<salida><estado>")

        If stError <> "" Then
            Response.Write("0")
        Else
            Response.Write("1")
        End If

        Response.Write("</estado><error>" & stError & "</error>")
        Response.Write("<datos></datos></salida>")


    End Sub

    Sub InicializarVariables()
        ' datos generales del blogger
        stFirstname = SaneadorSQL.Sanea("" & Request.Form("nombre"), 50)
        stGuidgrupo = SaneadorSQL.Sanea("" & Request.Form("guidgroup"), 50)
        stAmigo1 = SaneadorSQL.Sanea("" & Request.Form("amigo1"), 200)
        stAmigo2 = SaneadorSQL.Sanea("" & Request.Form("amigo2"), 200)
        stAmigo3 = SaneadorSQL.Sanea("" & Request.Form("amigo3"), 200)
        stAmigo4 = SaneadorSQL.Sanea("" & Request.Form("amigo4"), 200)
        stAmigo5 = SaneadorSQL.Sanea("" & Request.Form("amigo5"), 200)
    End Sub




#Region "invitaciones"
    Sub GestionaManada()
        ' para cada amigo comprobamos si existe ese e-mail
        ' si no existe, le enviamos invitación

        stMail = f.Lee(Server.MapPath("../mailings/invitacion/index1.html"))
        nRespuestaM = EnviaInvitacion(Request.Form("amigo1"), stMail)
        nRespuestaM = EnviaInvitacion(Request.Form("amigo2"), stMail)
        nRespuestaM = EnviaInvitacion(Request.Form("amigo3"), stMail)
        nRespuestaM = EnviaInvitacion(Request.Form("amigo4"), stMail)
        nRespuestaM = EnviaInvitacion(Request.Form("amigo5"), stMail)
    End Sub

    Function EnviaInvitacion(ByVal amigo As String, ByVal body As String) As Integer
        Dim nRespuesta As Integer = 1

        Dim stFirstname As String = Trim("" & Request.Form("nombre"))

        If Trim("" & amigo) <> "" Then
            body = Replace(body, "[[LINK1]]", AppSettings("dominio2") & "index.html?mail=" & amigo & "&guid=" & stGuidgrupo)
            body = Replace(body, "[[LINK2]]", AppSettings("dominio2"))
            body = Replace(body, "[[NOMBRE]]", stFirstname)
            body = Replace(body, """img/", """" & AppSettings("dominio") & "mailings/invitacion/img/")
            correo.EnviarEmail("info@eristoffinternativefestival.com", amigo, stFirstname & " te envía esta invitación", body, True)
        Else
            nRespuesta = -1
        End If

        Return nRespuesta
    End Function

#End Region




End Class
