Imports System.Configuration.ConfigurationManager
Imports System.Guid
Imports System.Data
Imports System.IO
Imports System.Xml

Partial Class altablogger
    Inherits System.Web.UI.Page

    ' genéricos
    Dim dm As New dms
    Dim fn As New funciones
    Dim f As New fichero


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
    Dim stMail As String = ""

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

    Dim stError = 0
    Dim setRespuesta As String = ""
    Dim stGUID As String = ""
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
        b.Fecha_Alta = Date.Now()
        b.Fecha_Mod = Date.Now()

        ' COMPROBAMOS QUE NO EXISTE EL NOMBRE DEL GRUPO

        intRespuesta = -1

        If CompruebaGrupo(stNom_grupo) Then

            ' #AHORA COMPROBAMOS EL BLOG

            If CompruebaBlog(stUrl_blog) > 0 Then
                ' #Damos de alta al blogger en el DMS
                intRespuesta = SubLeeXML(AltaBloggerDms(b))

                If intRespuesta = 0 And stRespuestaDMS1 = "ConsumerAlreadyExists" Then
                    '#update usuario nuevos campos más nuevo password.
                    If SubLeeXMLUpdate(UpdateBloggerDms(b)) = 1 And SubLeeXMLRemindPass(RemindPassword(b)) = 1 Then
                        intRespuesta2 = 1
                    End If
                    If intRespuesta2 > 0 Then
                        '#añadimos usuario a la campaña correspondiente
                        intRespuesta3 = SubLeeXMLCampaign(UpdateCampaignDms(stEmail))
                        '#onbtenemos el guid del usuario ya registrado para base de datos local
                        obtenerGUID(b.Email)
                    End If

                End If

                ' #SI HEMOS RECIBIDO RESPUESTA DEL ALTA CORRECTA DEL BLOGGER, INSERTAMOS EL RESTO DE DATOS
                If intRespuesta > 0 Or (intRespuesta2 > 0 And intRespuesta3 > 0) Then

                    ' #borramos los datos personales del blogger para crearlo en la base de datos
                    b.Email = ""
                    b.Password = ""
                    b.Firstname = ""
                    b.Lastname = ""
                    b.Middlename = ""
                    b.City = ""
                    b.Estado = "1"
                    b.fk_newuserId = Integer.Parse(stGUID)

                    nProv = da.InsUpdBlogger(b, "INSERT")


                    '# CREAMOS EL GRUPO primero creamos un código GUID
                    Dim g As Guid = Guid.NewGuid()
                    stGroupGUID = g.ToString

                    '# Insertamos el grupo
                    intRespuestaG = da.InsGrupo(intEsManada, stGUID, stGroupGUID, stNom_grupo, Date.Now(), 1)

                    '# Asociamos el blogger al grupo
                    da.InsBlogGrupo(b.fk_newuserId, intRespuestaG, Date.Now(), 1)

                    '# enviamos el mail al blogger
                    ' MailABlogger(stEmail)

                End If

            End If

        End If

        '#Manejamos respuesta de errores
        If intRespuesta = -1 Then '#No ha pasado la primera comprobación ERROR
            setRespuesta = "La URL de este blog ya existe"
            stError = 0
        ElseIf intRespuesta = 0 And intRespuesta2 = 0 Then
            setRespuesta = "Ha habido un error con la modificación de datos o del password."
            stError = 0
        ElseIf intRespuesta = 0 And intRespuesta2 = 1 And intRespuesta3 = 0 Then
            'Error en el registro al añadirlo a la campaña ERROR
            setRespuesta = "El usuario no se ha añadido a la campaña"
            stError = 0
        ElseIf intRespuesta = 0 And intRespuesta2 = 1 And intRespuesta3 = 1 Then
            'Error en el registro al añadirlo a la campaña ERROR
            setRespuesta = "El usuario ha sido añadido a la campaña"
            stError = 1
        ElseIf intRespuesta > 0 Then 'Registrado por primera vez o añadido a la campaña. OK
            setRespuesta = "El usuario ha sido registrado"
            stError = 1
        End If


        ' Generamos el Mensaje
        SalidaMensajeSimple(stError, setRespuesta)


    End Sub

    

    Sub obtenerGUID(ByVal stEmail As String)
        SubLeeXMLGUID(dm.FunEnviaXML("<consumerEmail>" & stEmail & "</consumerEmail>", "GetConsumerProfile"))
    End Sub

    Sub SubLeeXMLGUID(ByVal prXML As String)

        prXML = Replace(prXML, "<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""><soap:Body>", "")
        prXML = Replace(prXML, "</soap:Body></soap:Envelope>", "")
        prXML = Replace(prXML, "<GetConsumerProfileResult>", "")
        prXML = Replace(prXML, "</GetConsumerProfileResult>", "")

        Dim vrXml As System.IO.StringReader = New System.IO.StringReader(prXML)
        Dim PaginaXML As New XmlTextReader(vrXml)
        Dim EtiquetaActual As String = ""
        Dim stResultado As String = ""
        Dim stContenido As String = ""

        Do While (PaginaXML.Read())

            Select Case PaginaXML.NodeType
                Case XmlNodeType.Element
                    EtiquetaActual = PaginaXML.Name
                Case XmlNodeType.Text
                    If EtiquetaActual = "ConsumerID" Then
                        stGUID = stGUID & (PaginaXML.Value)
                    End If
            End Select
            
        Loop

    End Sub

    Sub SalidaMensajeSimple(ByVal stError As Integer, ByVal stRespuesta As String)
        Response.Write("<salida><estado>" & stError & "</estado><error>" & stRespuesta & "</error><debug>" & intRespuesta & intRespuesta2 & intRespuesta3 & "</debug></salida>")
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
            'ElseIf nAmigo1 = "-1" Then
            '   nEstado = 0
            '  stErrorsal = "El e-mail del amigo 1 ya existe"
            'ElseIf nAmigo2 = "-1" Then
            '   nEstado = 0
            '  stErrorsal = "El e-mail del amigo 2 ya existe"
            'ElseIf nAmigo3 = "-1" Then
            '   nEstado = 0
            '  stErrorsal = "El e-mail del amigo 3 ya existe"
            'ElseIf nAmigo4 = "-1" Then
            '   nEstado = 0
            '  stErrorsal = "El e-mail del amigo 4 ya existe"
            'ElseIf nAmigo5 = "-1" Then
            '   nEstado = 0
            '  stErrorsal = "El e-mail del amigo 5 ya existe"
            'ElseIf nRespuestaG = "-1" Then
            '   nEstado = 0
            '  stErrorsal = "Ya existe un grupo con estas características"
            'ElseIf nRespuestaG = "0" Then
            '   nEstado = 0
            '  stErrorsal = "Error en la inserción del grupo"
        End If
        Response.Write("<salida><estado>" & nEstado & "</estado><error>" & stErrorsal & "</error>")

        If nEstado > 0 Then
            Response.Write("<datos><user>" & nBlogger & "</user><groupguid>" & sGrupo & "</groupguid></datos>")
        End If

        Response.Write("</salida>")


    End Sub



    Sub GestionaManada()
        ' para cada amigo comprobamos si existe ese e-mail
        ' si no existe, le enviamos invitación

        stMail = f.Lee(Server.MapPath("../mailings/invitacion/index1.html"))


        intRespuesta1 = CompruebaAmigo(stAmigo1, stMail)
        intRespuesta2 = CompruebaAmigo(stAmigo2, stMail)
        intRespuesta3 = CompruebaAmigo(stAmigo3, stMail)
        intRespuesta4 = CompruebaAmigo(stAmigo4, stMail)
        intRespuesta5 = CompruebaAmigo(stAmigo5, stMail)



    End Sub


    Function CompruebaAmigo(ByVal amigo As String, ByVal body As String) As Integer
        Dim nRespuesta As Integer = 1


        If Trim("" & amigo) <> "" Then


            body = Replace(body, "[[LINK1]]", AppSettings("dominio2") & "index.html?mail=" & amigo & "&guid=" & stGroupGUID)
            body = Replace(body, "[[LINK2]]", AppSettings("dominio2"))
            body = Replace(body, "[[NOMBRE]]", stFirstname)
            body = Replace(body, """img/", """" & AppSettings("dominio") & "mailings/invitacion/img/")

            'correo.EnviarEmail("info@eristoffinternativefestival.com", amigo, "invitación", "http://eristoff.bluemodus.com/test/index.html?mail=" & amigo & "&guid=" & stGroupGUID, True)
            correo.EnviarEmail("info@eristoffinternativefestival.com", amigo, stFirstname & " te envía esta invitación", body, True)
        Else
            nRespuesta = -1
        End If

        Return nRespuesta

    End Function

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


    Sub MailABlogger(ByVal stemailb As String)
        Dim stBody As String = ""
        Dim stUrl As String = ""
        Dim stSubject As String = "Bienvenido/a"

        If intEsManada = 1 Then
            stUrl = "bienvenida_manada"
            stSubject = "¡Bienvenido/a a la manada!"
        Else
            stUrl = "bienvenida_lobo"
            stSubject = "Bienvenido/a al Eristoff Internative Festival"
        End If

        stBody = f.Lee(Server.MapPath("../mailings/" & stUrl & "/index.html"))

        stBody = Replace(stBody, "[[NOMBRE]]", stFirstname)
        stBody = Replace(stBody, """img/", """" & AppSettings("dominio") & "/mailings/" & stUrl & "/img/")


        If Trim("" & stemailb) <> "" Then
            correo.EnviarEmail("info@eristoffinternativefestival.com", stemailb, stSubject, stBody, True)
        End If
    End Sub

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

#Region "webservices"

    Function RemindPassword(ByVal bb As blogger) As String
        Dim stXML As String = ""
        Dim stRespuesta As String = ""

        ' montamos la cadena del xml

        stXML = stXML & "<consumerEmail>" & bb.Email & "</consumerEmail>"
        stXML = stXML & "<generatePassword>false</generatePassword>"
        stXML = stXML & "<newPassword>" & bb.Password & "</newPassword>"
        stXML = stXML & "<marketID>36</marketID>"


        stRespuesta = dm.FunEnviaXML(stXML, "ResetConsumerPassword")
        stRespuestaDMS = stXML & stRespuesta
        Return (stRespuesta)

    End Function

    Function SubLeeXMLRemindPass(ByVal prXML As String) As Integer
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

            End Select
        Loop
        If stBolResult = "true" Then
            Return 1
        Else
            Return 0
        End If
    End Function

    Function UpdateBloggerDms(ByVal bb As blogger) As String
        Dim stXML As String = ""
        Dim stRespuesta As String = ""

        'montamos la cadena del xml
        stXML = stXML & "<profile>"
        stXML = stXML & "<FirstName>" & bb.Firstname & "</FirstName>"
        stXML = stXML & "<LastName>" & bb.Lastname & "</LastName>"
        stXML = stXML & "<MiddleName>" & bb.Middlename & "</MiddleName>"
        stXML = stXML & "<Email>" & bb.Email & "</Email>"
        stXML = stXML & "<DateOfBirth>" & fn.TimeCodeMaker(CType(bb.DateOfBirth, Date)) & "</DateOfBirth>"
        stXML = stXML & "<City>" & bb.City & "</City>"
        stXML = stXML & "<Password>2222a</Password>"
        stXML = stXML & "<EmailOptIn>true</EmailOptIn>"
        stXML = stXML & "</profile>"



        stRespuesta = dm.FunEnviaXML(stXML, "UpdateConsumerProfile")
        stRespuestaDMS = stXML & stRespuesta
        Return (stRespuesta)

    End Function

    Function UpdateCampaignDms(ByVal Email As String) As String
        Dim stXML As String = ""
        Dim stRespuesta As String = ""

        ' montamos la cadena del xml
        stXML = stXML & "<campaignID>" & AppSettings("campaingID") & "</campaignID>"
        stXML = stXML & "<consumerEmail>" & Email & "</consumerEmail>"



        stRespuesta = dm.FunEnviaXML(stXML, "AddConsumerToCampaign")
        stRespuestaDMS = stXML & stRespuesta
        Return (stRespuesta)

    End Function

    Function SubLeeXMLCampaign(ByVal prXML As String) As Integer
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
                    If EtiquetaActual = "AddConsumerToCampaignResult" Then stBolResult = PaginaXML.Value
            End Select

        Loop



        If stBolResult = "true" Then
            Return 1
        Else
            Return 0
        End If
    End Function

    Function SubLeeXMLUpdate(ByVal prXML As String) As Integer
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
                    If EtiquetaActual = "ResponseMessage" Then stRespuestaDMS2 = PaginaXML.Value
            End Select


        Loop



        If stBolResult = "true" Then
            Return 1
        Else
            Return 0
        End If
    End Function

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
                    If EtiquetaActual = "NewConsumerID" Then
                        nResultado = 1 '#resultado para logica de negocio
                        stGUID = PaginaXML.Value
                    End If

            End Select


        Loop



        If stBolResult = "true" Then
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

    Function SubLeeXMLAmigo(ByVal prXML As String) As String
        Dim vrXml As System.IO.StringReader = New System.IO.StringReader(prXML)
        Dim PaginaXML As New XmlTextReader(vrXml)
        Dim EtiquetaActual As String = ""
        Dim stResultado As String = ""

        Do While (PaginaXML.Read())
            EtiquetaActual = PaginaXML.Name
            If EtiquetaActual = "Email" Then stResultado = PaginaXML.ReadElementContentAsString
        Loop

        Return stResultado

    End Function

#End Region


End Class
