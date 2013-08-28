Imports System.Xml
Imports System.Web
Imports System.Net
Imports System.Configuration.ConfigurationManager


Partial Class services_creamanada
    Inherits System.Web.UI.Page


    ' inicializamos el objeto de datos
    Dim da As New datos
    Dim intRespuesta As Integer = 0
    Dim mimail As New correo
    Dim f As New fichero

    Dim stMail As String = ""


    Dim stNom_grupo As String = ""
    Dim stGuidgrupo As String = ""
    Dim nUserId As Integer = 0
    Dim stAmigo1 As String = ""
    Dim stAmigo2 As String = ""
    Dim stAmigo3 As String = ""
    Dim stAmigo4 As String = ""
    Dim stAmigo5 As String = ""


    Dim nRespuesta1 As Integer = 0
    Dim nRespuesta2 As Integer = 0

    Dim nRespuestaM As Integer = 0

    Dim intRespuestaG As Integer = 0

    Dim stRespuestaDMS As String = ""
    Dim stGroupGUID As String = ""


    Dim correo As New correo

    Dim stError As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load



        ' Compruebo que el nombre del grupo no existe, si viene informado

        If Trim("" & Request.Form("guidgroup")) <> "" And Trim("" & Request.Form("user")) <> "" And Trim("" & Request.Form("nom_grupo")) <> "" Then
            ' Cargo variables que vienen del formulario
            InicializarVariables()

            ' compruebo nombre de grupo
            If CompruebaGrupo(stNom_grupo) Then

                intRespuestaG = 1

                ' Cambio el tipo del blogger a 1, que es líder
                nRespuesta1 = da.UpdateTipoBlogger(nUserId, 1)

                ' si el cambio se ha dado correctamente
                If nRespuesta1 > 0 Then

                    ' Cambio el tipo del grupo a 1, que es manada
                    nRespuesta2 = da.UpdateGrupo(stGuidgrupo, stNom_grupo, 1)
                    If nRespuesta2 = 0 Then
                        stError = "No se ha podido cambiar el tipo de manada"
                    End If
                Else
                    stError = "No se ha podido cambiar el tipo de usuario"
                End If
            Else
                stError = "El nombre de la manada ya existe"
            End If

        Else
            stError = "Faltan datos"
        End If


        ' enviamos invitaciones si no hay error
        If stError = "" Then
            GestionaManada()
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
        nUserId = CType(Request.Form("user"), Integer)
        stNom_grupo = SaneadorSQL.Sanea("" & Request.Form("nom_grupo"), 200)
        stGuidgrupo = SaneadorSQL.Sanea("" & Request.Form("guidgroup"), 50)
        stAmigo1 = SaneadorSQL.Sanea("" & Request.Form("amigo1"), 200)
        stAmigo2 = SaneadorSQL.Sanea("" & Request.Form("amigo2"), 200)
        stAmigo3 = SaneadorSQL.Sanea("" & Request.Form("amigo3"), 200)
        stAmigo4 = SaneadorSQL.Sanea("" & Request.Form("amigo4"), 200)
        stAmigo5 = SaneadorSQL.Sanea("" & Request.Form("amigo5"), 200)


    End Sub

    Function CambiaTipoBlogger(ByVal codblogger As Integer, ByVal tipoblogger As Integer) As Integer
        Dim nRespuesta As Integer = 0
        Dim b As New blogger
        b.pk_cod_blogger = codblogger
        b.fk_cod_tipo_blogger = tipoblogger
        b.fk_newuserId = 0
        b.Email = "#"
        b.Blog = "#"
        b.Url_blog = "#"
        b.Aviso = "#"
        b.Estado = "#"

        b.Fecha_Mod = Date.Now()
        Try
            'nRespuesta = da.UpdateTipoBlogger(b, "update")
        Catch ex As Exception
            'nRespuesta = 0
        End Try

        Return nRespuesta
    End Function

    Function CompruebaGrupo(ByVal nomgrupo As String) As Integer

        Dim stNomGrupo As String = ""

        ' leemos los datos en la base de datos, si hay error, los mandamos vacíos
        Try
            stNomGrupo = da.GetExisteGrupo(nomgrupo)
        Catch ex As Exception
            stNomGrupo = ""
        End Try

        If Trim("" & stNomGrupo) = "" Then
            Return 1
        Else
            Return 0
        End If

    End Function

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
