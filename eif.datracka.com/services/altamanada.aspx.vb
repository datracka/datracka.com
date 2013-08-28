Imports System.Configuration.ConfigurationManager
Imports System.Guid
Imports System.Data
Imports System.IO
Imports System.Xml
Partial Class altamanada
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
    Dim stGuid_grupo As String = ""


    ' inicializamos el objeto de datos
    Dim da As New datos
    Dim intRespuesta As Integer = 0
    Dim intRespuesta1 As Integer = 0
    Dim intRespuesta2 As Integer = 0
    Dim intRespuesta3 As Integer = 0
    Dim intRespuesta4 As Integer = 0
    Dim intRespuesta5 As Integer = 0
    Dim intRespuestaG As Integer = 0

    Dim nCod_grupo As Integer = 0

    Dim stRespuestaDMS As String = ""
    Dim stRespuestaDMS1 As String = ""


    Dim nProv As Integer = 0
    Dim stGroupGUID As String = ""
    Dim correo As New correo

    ' para el propietario del grupo
    Dim nPropID As Integer = 0
    Dim stPropMail As String = 0
    Dim stPropRuta As String = 0



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' Cargo variables que vienen del formulario
        InicializarVariables()

        ' CREAMOS EL BLOGGER
        ' guardamos en la base de datos el blogger principal
        ' es de tipo blogger 2
        ' es un tipo de consulta INSERT
        ' no tiene pk
        ' no tiene consumerID del DMS, ponemos a 0
        ' fecha de alta es la fecha actual
        Dim b As New blogger
        b.pk_cod_blogger = 0
        b.fk_cod_tipo_blogger = 2
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
        b.Fecha_Alta = Date.Now()
        b.Fecha_Mod = Date.Now()

        ' COMPROBAMOS QUE SI EXISTE EL GUID DEL GRUPO
        ' SI ES ASÍ LO CARGAMOS EN LA VARIABLE stGuid_grupo
        nCod_grupo = CompruebaGrupo(stGuid_grupo)

        If nCod_grupo > 0 Then

            ' AHORA COMPROBAMOS EL BLOG
            If CompruebaBlog(stUrl_blog) > 0 Then
                ' Damos de alta al blogger en el DMS
                intRespuesta = SubLeeXML(AltaBloggerDms(b))

                If intRespuesta = 0 And stRespuestaDMS1 = "ConsumerAlreadyExists" Then
                    stRespuestaDMS1 = "mierdaaaaaaaaaa"
                End If

                ' SI HEMOS RECIBIDO RESPUESTA DEL ALTA CORRECTA DEL BLOGGER, INSERTAMOS EL RESTO DE DATOS
                If intRespuesta > 0 Then

                    ' borramos los datos personales del blogger para crearlo en la base de datos
                    b.Email = ""
                    b.Password = ""
                    b.Firstname = ""
                    b.Lastname = ""
                    b.Middlename = ""
                    b.City = ""
                    b.Estado = "1"
                    b.fk_newuserId = intRespuesta

                    nProv = da.InsUpdBlogger(b, "INSERT")

                    ' Asociamos el blogger al grupo
                    da.InsBlogGrupo(b.fk_newuserId, nCod_grupo, Date.Now(), 1)

                    ' Obtenemos los datos del propietario del grupo 
                    nPropID = TraePropietario(stGuid_grupo)
                    GestionaPropietario(nPropID)


                End If
            Else
                intRespuesta = 0
                stRespuestaDMS1 = "La URL de este blog ya existe"
            End If

        Else
            intRespuesta = 0
            stRespuestaDMS1 = "El indicador del grupo no es correcto"
        End If





        ' Generamos el Mensaje
        SalidaMensaje(intRespuesta, intRespuesta1, intRespuesta2, intRespuesta3, intRespuesta4, intRespuesta5, intRespuestaG, stGroupGUID)







    End Sub




    Sub SalidaMensaje(ByVal nBlogger As Integer, ByVal nAmigo1 As Integer, ByVal nAmigo2 As Integer, ByVal nAmigo3 As Integer, ByVal nAmigo4 As Integer, ByVal nAmigo5 As Integer, ByVal nRespuestaG As Integer, ByVal sGrupo As String)
        Dim nEstado As Integer = 1
        Dim stErrorsal As String = ""

        If nBlogger = "-1" Then
            nEstado = 0
            stErrorsal = "Error en la inserción del blogger "
        ElseIf nBlogger = "0" Then
            nEstado = 0
            stErrorsal = stRespuestaDMS1

        End If
        Response.Write("<salida><estado>" & nEstado & "</estado><error>" & stErrorsal & "</error>")

        If nEstado > 0 Then
            Response.Write("<datos><user>" & nBlogger & "</user></datos>")
        End If

        Response.Write("</salida>")


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

        stGuid_grupo = SaneadorSQL.Sanea("" & Request.Form("guidgroup"), 200)


    End Sub



    Function CompruebaGrupo(ByVal guidgrupo As String) As Integer

        Dim nGuidGrupo As Integer = 0

        ' leemos los datos en la base de datos, si hay error, los mandamos vacíos
        Try
            nGuidGrupo = da.GetExisteGrupoByGuid(guidgrupo)
        Catch ex As Exception
            nGuidGrupo = 0

        End Try

        If nGuidGrupo > 0 Then
           Return nGuidGrupo
        Else
            Return 0
        End If

    End Function
    Function CompruebaBlog(ByVal urlblog As String) As Integer

        Dim stUrlBlog As String = ""

        ' leemos los datos en la base de datos, si hay error, los mandamos vacíos
        Try
            stUrlBlog = da.GetExisteBlog(urlblog)
        Catch ex As Exception
            stUrlBlog = ""
        End Try

        If Trim("" & stUrlBlog) = "" Then
            Return 1
        Else
            Return 0
        End If

    End Function

    Function TraePropietario(ByVal guidgrupo As String) As Integer

        Dim nGuidGrupo As Integer = 0

        ' leemos los datos en la base de datos, si hay error, los mandamos vacíos
        Try
            nGuidGrupo = da.GetPropietarioGrupoByGuid(guidgrupo)
        Catch ex As Exception
            nGuidGrupo = 0

        End Try

        If nGuidGrupo > 0 Then
            Return nGuidGrupo
        Else
            Return 0
        End If

    End Function

#Region "webservices"
    Function AltaBloggerDms(ByVal bb As blogger) As String
        Dim stXML As String = ""
        Dim stRespuesta As String = ""

        ' montamos la cadena del xml
        stXML = stXML & "<profile>"
        stXML = stXML & "<FirstName>" & bb.Firstname & "</FirstName>"
        stXML = stXML & "<LastName>" & bb.Lastname & "</LastName>"
        stXML = stXML & "<MiddleName>" & bb.Middlename & "</MiddleName>"
        stXML = stXML & "<Email>" & bb.Email & "</Email>"
        stXML = stXML & "<DateOfBirth>" & fn.TimeCodeMaker(CType(bb.DateOfBirth, Date)) & "</DateOfBirth>"
        stXML = stXML & "<City>" & bb.City & "</City>"
        stXML = stXML & "<Password>" & bb.Password & "</Password>"
        stXML = stXML & "<EmailOptIn>true</EmailOptIn>"
        'stXML = stXML & "<isFkock>false</isFkock>"
        'stXML = stXML & "<CustomID>88</CustomID>"
        'stXML = stXML & "<BlogName>1</BlogName>"
        'stXML = stXML & "<BlogUrl>1</BlogUrl>"
        stXML = stXML & "</profile>"
        stXML = stXML & "<sourceCampaignID>" & AppSettings("campaingID") & "</sourceCampaignID>"
        stXML = stXML & "<sendEmailToConsumer>false</sendEmailToConsumer>"
        stXML = stXML & "<source>ExternalOnlineSource</source>"

        stRespuesta = dm.FunEnviaXML(stXML, "AddConsumerProfile")
        stRespuestaDMS = stXML & stRespuesta
        Return (stRespuesta)

    End Function

    Function SubLeeXML(ByVal prXML As String) As Integer
        Dim vrXml As System.IO.StringReader = New System.IO.StringReader(prXML)
        Dim PaginaXML As New XmlTextReader(vrXml)
        Dim EtiquetaActual As String = ""
        Dim nResultado As Integer = 0
        Dim stBolResult As String = ""

        Do While (PaginaXML.Read())


            Select Case PaginaXML.NodeType

                ' if Element, display its name
                Case XmlNodeType.Element
                    EtiquetaActual = PaginaXML.Name

                Case XmlNodeType.Text ' if Text, display it
                    If EtiquetaActual = "SuccessFlag" Then stBolResult = PaginaXML.Value
                    If EtiquetaActual = "ResponseMessage" Then stRespuestaDMS1 = PaginaXML.Value
                    If EtiquetaActual = "NewConsumerID" Then nResultado = PaginaXML.Value
            End Select


        Loop



        If stBolResult = "true" And nResultado > 0 Then
            Return nResultado
        Else
            Return 0
        End If
    End Function


    Function ConsultaMailDms(ByVal b As String) As String
        Dim stXML As String = ""
        Dim stRespuesta As String = ""

        ' montamos la cadena del xml
        stXML = stXML & "<consumerEmail>" & b & "</consumerEmail>"
        stRespuesta = dm.FunEnviaXML(stXML, "GetConsumerProfile")

        Return (stRespuesta)

    End Function



#End Region

    Sub GestionaPropietario(ByVal UserId As Integer)


        Dim strResultado1 As String = ""
        Dim strVariables1 As String = ""

        ' montamos variables a enviar
        strVariables1 = _
            "<consumerID>" & CType(UserId, String) & "</consumerID>"


        ' enviamos XML y cargamos el resultado
        strResultado1 = dm.FunEnviaXML(strVariables1, "GetConsumerProfileByConsumerID")


        ' leemos la respuesta
        'Response.Write(strResultado)
        'Response.End()

        stPropMail = SubLeeXMLProp(strResultado1)

        ' si tenemos el e-mail, lo enviamos...
        If stPropMail <> "" Then
            EnviaInvitacion(stPropMail)
        End If


    End Sub

    Function SubLeeXMLProp(ByVal prXML As String) As String

        Dim vrXml As System.IO.StringReader = New System.IO.StringReader(prXML)
        Dim PaginaXML As New XmlTextReader(vrXml)
        Dim EtiquetaActual As String = ""
        Dim nResultado As Integer = 0
        Dim stBolResult As String = ""

        Do While (PaginaXML.Read())


            Select Case PaginaXML.NodeType

                ' if Element, display its name
                Case XmlNodeType.Element
                    EtiquetaActual = PaginaXML.Name

                Case XmlNodeType.Text ' if Text, display it
                    If EtiquetaActual = "Email" Then stBolResult = PaginaXML.Value

            End Select


        Loop

        Return stBolResult

    End Function

    Sub EnviaInvitacion(ByVal amigo As String)
        Dim nRespuesta As Integer = 1
        Dim f As New fichero
        Dim stMail As String = ""
        Dim body As String = ""

        body = f.Lee(Server.MapPath("../mailings/union_manada/index.html"))

        If Trim("" & amigo) <> "" Then
            body = Replace(body, "[[NOMBRE]]", stFirstname & " " & stLastname)
            body = Replace(body, """img/", """" & AppSettings("dominio") & "mailings/union_manada/img/")
            correo.EnviarEmail("info@eristoffinternativefestival.com", amigo, stFirstname & " " & stLastname & " se ha unido a tu manada", body, True)
        End If
    End Sub
End Class
