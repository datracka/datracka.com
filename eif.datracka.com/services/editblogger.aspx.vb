Imports System.Xml
Imports System.Web
Imports System.Net
Imports System.Configuration.ConfigurationManager

Partial Class editblogger
    Inherits System.Web.UI.Page

    ' genéricos
    Dim dm As New dms
    Dim fn As New funciones


    ' Datos del blogger
    Dim stEmail As String = ""
    Dim stPassword As String = ""
    Dim stBlog As String = ""
    Dim stUrl_blog As String = ""
    Dim stAviso As String = ""
    Dim stFirstname As String = ""
    Dim stLastname As String = ""
    Dim stMiddlename As String = ""
    Dim dDateOfBirth As New Date
    Dim stCity As String = ""

    ' Datos de la manada
    Dim intEsManada As Integer = 2
    Dim stNom_grupo As String = ""
    Dim stAmigo1 As String = ""
    Dim stAmigo2 As String = ""
    Dim stAmigo3 As String = ""
    Dim stAmigo4 As String = ""
    Dim stAmigo5 As String = ""

    ' inicializamos el objeto de datos
    Dim da As New datos
    Dim intRespuesta As Integer = 0
    Dim intRespuesta1 As Integer = 0
    Dim intRespuesta2 As Integer = 0
    Dim intRespuesta3 As Integer = 0
    Dim intRespuesta4 As Integer = 0
    Dim intRespuesta5 As Integer = 0
    Dim intRespuestaG As Integer = 0

    Dim stRespuestaDMS As String = ""
    Dim stRespuestaDMS1 As String = ""
    Dim stRespuestaDMS2 As String = ""
    Dim stRespuestaDMS3 As String = ""
    Dim stRespuestaDMS4 As String = ""
    Dim stRespuestaDMS5 As String = ""

    Dim nProv As Integer = 0

    Dim stGroupGUID As String = ""

    Dim correo As New correo




    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' Cargo variables que vienen del formulario
        InicializarVariables()


        ' CREAMOS EL BLOGGER
        ' guardamos en la base de datos el blogger principal
        ' es de tipo blogger 1
        ' es un tipo de consulta INSERT
        ' no tiene pk
        ' no tiene consumerID del DMS, ponemos a 0
        ' fecha de alta es la fecha actual
        Dim b As New blogger
        b.pk_cod_blogger = 0
        b.fk_cod_tipo_blogger = 1
        b.fk_newuserId = 0
        b.Email = stEmail
        b.Password = stPassword
        b.Blog = stBlog
        b.Url_blog = stUrl_blog
        b.Aviso = stAviso
        b.Firstname = stFirstname
        b.Lastname = stLastname
        b.Middlename = stMiddlename
        b.DateOfBirth = dDateOfBirth
        b.City = stCity
        b.Fecha_Mod = Date.Now()

        ' damos la salida
        Response.Write("<salida><estado>1</estado><error></error><datos></datos></salida>")


    End Sub

    Sub InicializarVariables()

        ' datos generales del blogger
        stEmail = SaneadorSQL.Sanea("" & Request.Form("email"), 200)
        stPassword = SaneadorSQL.Sanea("" & Request.Form("password"), 20)
        stBlog = SaneadorSQL.Sanea("" & Request.Form("blog"), 100)
        stUrl_blog = SaneadorSQL.Sanea("" & Request.Form("urlblog"), 255)
        stAviso = SaneadorSQL.Sanea("" & Request.Form("aviso"), 1)
        stFirstname = SaneadorSQL.Sanea("" & Request.Form("firstname"), 50)
        stLastname = SaneadorSQL.Sanea("" & Request.Form("lastname"), 50)
        stMiddlename = SaneadorSQL.Sanea("" & Request.Form("middlename"), 50)
        dDateOfBirth = CDate("" & Request.Form("dateofbirth").ToString())
        stCity = SaneadorSQL.Sanea("" & Request.Form("city"), 200)

        ' vemos si es manada
        ' en ese caso, rellenamos los datos
        ' si no es manada, el nombre del grupo es el del blog
        If Request.Form("esmanada") = "1" Then
            intEsManada = 1
            stNom_grupo = SaneadorSQL.Sanea("" & Request.Form("nom_grupo"), 200)
            stAmigo1 = SaneadorSQL.Sanea("" & Request.Form("amigo1"), 200)
            stAmigo2 = SaneadorSQL.Sanea("" & Request.Form("amigo2"), 200)
            stAmigo3 = SaneadorSQL.Sanea("" & Request.Form("amigo3"), 200)
            stAmigo4 = SaneadorSQL.Sanea("" & Request.Form("amigo4"), 200)
            stAmigo5 = SaneadorSQL.Sanea("" & Request.Form("amigo5"), 200)
        Else
            stNom_grupo = SaneadorSQL.Sanea(stBlog, 200)
        End If

    End Sub
End Class
