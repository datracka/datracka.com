Imports System.Configuration.ConfigurationManager
Imports System.Data
Imports System.IO
Imports System.Xml
Imports System.Web
Imports System.Net
Partial Class services_altaconcurso
    Inherits System.Web.UI.Page


    ' genéricos
    Dim dm As New dms
    Dim fn As New funciones


    ' Datos del blogger
    Dim stEmail As String = ""
    Dim stDateOfBirth As String = ""
    Dim stName As String = ""
    Dim stLopd As String = ""
    Dim stGuid As String = ""

    Dim intRespuesta As Integer = 0
    Dim intRespuesta2 As Integer = 0
    Dim intRespuesta3 As Integer = 0
    Dim stRespuestaDMS As String = ""
    Dim stRespuestaDMS1 As String = ""
    Dim stRespuestaDMS2 As String = ""
    Dim stRespuestaDMS3 As String = ""


    'lopd
    Dim stRespuestaLopd As String = ""
    Dim stRespuestaGuid As String = ""


    ' para el voto
    Dim da As New datos
    Dim nRespuesta As Integer = 0
    Dim stRespuesta As String = ""



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        ' initial page setup
        If (Not IsPostBack) Then

            'set control text
            MessageCorrectLabel.Text = "Incorrecto"
            'these messages are shown only after validation
            MessageCorrectLabel.Visible = False

        End If


        CodeTextBox.Attributes.Add("onkeyup", "this.value = this.value.toLowerCase();")
        ' si se remiten datos del formulario
        If (IsPostBack) Then

            Dim code As String = CodeTextBox.Text.Trim().ToUpper()

            ' si es correcto el código seguimos con el proceso, si no, nada....
            If (SampleCaptcha.Validate(code)) Then


                ' extraemos los campos
                stEmail = CType("" & Request.Form("email"), String)
                stDateOfBirth = CType(Request.Form("dateofbirth"), String)
                stName = CType("" & Request.Form("name"), String)
                stLopd = CType("" & Request.Form("lopd"), String)
                stGuid = CType("" & Request.Form("guid"), String)

                ' si vienen informado el email y la fecha de nacimiento
                If Trim(stEmail) <> "" And Trim(stDateOfBirth) <> "" Then

                    intRespuesta = SubLeeXML(AltaBloggerDms(stEmail, stDateOfBirth, stName, stLopd))

                    'Response.Write(intRespuesta & stRespuestaDMS1)

                    ' si no se ha dado de alta y el e-mail ya existe entonces actualizamos el usuario
                    ' y lo añadimos a la campaña correspondiente
                    If intRespuesta = 0 And stRespuestaDMS1 = "ConsumerAlreadyExists" Then
                        intRespuesta2 = SubLeeXMLUpdate(UpdateBloggerDms(stEmail, stDateOfBirth, stName, stLopd))

                        'Response.Write(intRespuesta2 & stRespuestaDMS2)

                        ' si lo hemos actualizado, entonces lo añadimos a la campaña
                        If intRespuesta2 > 0 Then
                            intRespuesta3 = SubLeeXMLCampaign(UpdateCampaignDms(stEmail))
                            'Response.Write("-" & intRespuesta3 & "-")

                        End If
                    End If


                End If

                ' variables para la salida
                Dim nEstado As Integer = 0
                Dim stError As String = ""

                ' MANEJAMOS ERRORES Y
                ' damos la salida
                If intRespuesta > 0 Then
                    ' si todo ha ido bien
                    nEstado = 1
                    stError = "OK"
                ElseIf intRespuesta = 0 And stRespuestaDMS1 <> "ConsumerAlreadyExists" Then
                    nEstado = 0
                    stError = stRespuestaDMS1
                ElseIf intRespuesta = 0 And stRespuestaDMS1 = "ConsumerAlreadyExists" And intRespuesta2 = 1 And intRespuesta3 = 1 Then
                    nEstado = 1
                    stError = "OK"
                ElseIf intRespuesta = 0 And stRespuestaDMS1 = "ConsumerAlreadyExists" And intRespuesta2 = 1 And intRespuesta3 = 0 Then
                    nEstado = 0
                    stError = "Usuario ya existente"
                ElseIf intRespuesta = 1 And intRespuesta2 = 0 Then
                    nEstado = 0
                    stError = stRespuestaDMS2
                End If


                ' si todo ha ido bien, grabamos LOPD
                If nEstado = 1 Then
                    stRespuestaLopd = SubLeeXMLlopd(GrabaLopd(stEmail, stLopd))
                End If

                If stRespuestaLopd <> "OK" Then
                    nEstado = 0
                    stError = "No se ha podido grabar LOPD"
                End If


                ' si todo ha ido bien, grabamos GUID
                If nEstado = 1 Then
                    stRespuestaGuid = SubLeeXMLGuid(GrabaGuid(stEmail, stGuid))
                End If

                If stRespuestaGuid <> "OK" Then
                    nEstado = 0
                    stError = "No se ha podido asociar al grupo"
                End If




                ' si nos indican URL redirigimos, si no salida por pantalla
                'If Trim("" & Request.Form("url")) <> "" Then
                'Response.Redirect(Request.Form("url"))
                'Response.End()
                'Else
                'Response.Write("<salida><estado>" & nEstado & "</estado><error>" & stError & "</error><datos></datos></salida>")
                ' End If

                Server.Transfer("votos5914.aspx?guid=" & stGuid)
                Response.End()

            Else
                MessageCorrectLabel.Visible = True
                CodeTextBox.Text = ""

            End If

        End If


    End Sub

#Region "lopd"
    Function GrabaLopd(ByVal Email As String, ByVal Lopd As String) As String
        Dim stXML As String = ""
        Dim stRespuesta As String = ""

        ' montamos la cadena del xml
        stXML = stXML & "<customField>"
        'stXML = stXML & "<CampaignCustomFieldID>104</CampaignCustomFieldID>"
        stXML = stXML & "<CampaignCustomFieldID>260</CampaignCustomFieldID>"
        stXML = stXML & "<ConsumerEmail>" & Email & "</ConsumerEmail>"
        stXML = stXML & "<FieldValue>" & Lopd & "</FieldValue>"
        stXML = stXML & "</customField>"



        stRespuesta = dm.FunEnviaXML(stXML, "AddCampaignFieldValue")
        stRespuestaDMS = stXML & stRespuesta
        Return (stRespuesta)

    End Function
    Function SubLeeXMLlopd(ByVal prXML As String) As String
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
                    If EtiquetaActual = "AddCampaignFieldValueResult" Then stBolResult = PaginaXML.Value

            End Select


        Loop

        Return stBolResult

    End Function
#End Region


#Region "guid"
    Function GrabaGuid(ByVal Email As String, ByVal Guid As String) As String
        Dim stXML As String = ""
        Dim stRespuesta As String = ""

        ' montamos la cadena del xml
        stXML = stXML & "<customField>"
        '   stXML = stXML & "<CampaignCustomFieldID>123</CampaignCustomFieldID>"
        stXML = stXML & "<CampaignCustomFieldID>261</CampaignCustomFieldID>"
        stXML = stXML & "<ConsumerEmail>" & Email & "</ConsumerEmail>"
        stXML = stXML & "<FieldValue>" & Guid & "</FieldValue>"
        stXML = stXML & "</customField>"



        stRespuesta = dm.FunEnviaXML(stXML, "AddCampaignFieldValue")
        stRespuestaDMS = stXML & stRespuesta
        Return (stRespuesta)

    End Function
    Function SubLeeXMLGuid(ByVal prXML As String) As String
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
                    If EtiquetaActual = "AddCampaignFieldValueResult" Then stBolResult = PaginaXML.Value

            End Select


        Loop

        Return stBolResult

    End Function
#End Region


    Function UpdateCampaignDms(ByVal Email As String) As String
        Dim stXML As String = ""
        Dim stRespuesta As String = ""

        ' montamos la cadena del xml
        stXML = stXML & "<campaignID>" & AppSettings("campaingIDConcurso") & "</campaignID>"
        stXML = stXML & "<consumerEmail>" & Email & "</consumerEmail>"



        stRespuesta = dm.FunEnviaXML(stXML, "AddConsumerToCampaign")
        stRespuestaDMS = stXML & stRespuesta
        Return (stRespuesta)

    End Function

    Function UpdateBloggerDms(ByVal Email As String, ByVal DateOfBirth As String, ByVal Name As String, ByVal Lopd As String) As String
        Dim stXML As String = ""
        Dim stRespuesta As String = ""

        ' montamos la cadena del xml
        stXML = stXML & "<profile>"
        stXML = stXML & "<Email>" & Email & "</Email>"
        stXML = stXML & "<DateOfBirth>" & fn.TimeCodeMaker(CType(DateOfBirth, Date)) & "</DateOfBirth>"
        stXML = stXML & "<FirstName>" & Name & "</FirstName>"
        stXML = stXML & "<EmailOptIn>true</EmailOptIn>"
        stXML = stXML & "</profile>"


        stRespuesta = dm.FunEnviaXML(stXML, "UpdateConsumerProfile")
        stRespuestaDMS = stXML & stRespuesta
        Return (stRespuesta)

    End Function

    Function AltaBloggerDms(ByVal Email As String, ByVal DateOfBirth As String, ByVal Name As String, ByVal Lopd As String) As String
        Dim stXML As String = ""
        Dim stRespuesta As String = ""

        ' montamos la cadena del xml
        stXML = stXML & "<profile>"
        stXML = stXML & "<Email>" & Email & "</Email>"
        stXML = stXML & "<DateOfBirth>" & fn.TimeCodeMaker(CType(DateOfBirth, Date)) & "</DateOfBirth>"
        stXML = stXML & "<FirstName>" & Name & "</FirstName>"
        stXML = stXML & "<EmailOptIn>true</EmailOptIn>"
        stXML = stXML & "</profile>"
        stXML = stXML & "<sourceCampaignID>" & AppSettings("campaingIDConcurso") & "</sourceCampaignID>"
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
End Class
