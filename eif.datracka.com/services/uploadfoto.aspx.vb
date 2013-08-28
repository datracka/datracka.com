Imports System.Configuration.ConfigurationManager
Imports System.Data
Imports System.IO
Imports System.Xml
Imports System.Web
Imports System.Net
Imports System.Text

Partial Class services_uploadfoto
    Inherits System.Web.UI.Page

    ' Genericos
    Private dmsClient As New DMSServiceClient
    Private strBaseLocation As String = Server.MapPath(AppSettings("rutaFotoUpload"))
    Private campaingId As Integer = AppSettings("campaingIDFotoUpload")
    Private customFieldFotoUpload As Integer = AppSettings("customFieldFotoUpload")
    Private credentials As New BacardiWS.DMS.ServiceCredentials

    ' Datos del blogger
    Private strEmail As String = ""
    Private dateOfBirth As New Date
    Private strName As String = ""
    Private strLastName As String = ""
    Private strMiddleName As String = ""
    Private strCity As String = ""
    Private boolOptin As Boolean = True
    Private intConsumerId As Integer = 0

    ' Datos Campaña
    Private strCesionFoto As String = ""
    Private filePicture As HttpPostedFile
    Private ExtensionesPermitidas() As String = {".jpg"}
    Private PesoMaximo As Integer = 512000


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            Response.ContentType = "text/xml"
            Response.ContentEncoding = Encoding.UTF8

            InitVariables()

            Dim result As Boolean = AddToCampaing()
            If result Then
                result = UploadPicture()
                If result Then
                    Response.Write("<respuesta estado='ok'><mensaje>Se han guardado los datos correctamente</mensaje></respuesta>")
                End If
            End If

        Catch exc As Exception
            Response.Write("<respuesta estado='ko'><mensaje>Se ha producido un error en el servidor. Por favor, intentalo de nuevo. Motivo: " & exc.Message & "</mensaje></respuesta>")
        Finally
            dmsClient.Close()
            Response.Flush()
        End Try

    End Sub

    Function ExtensionOK(ByVal nomfile As String) As Boolean
        Dim extension As String = nomfile.Substring(nomfile.IndexOf("."))

        'Si no hay extensiones se aceptan todas
        If ExtensionesPermitidas.Length = 0 Then Return True

        'Si las hay se comprueban
        For i As Integer = 0 To ExtensionesPermitidas.Length - 1
            If extension = ExtensionesPermitidas(i) Then
                Return True
            End If
        Next

        Return False
    End Function

    Function IsEristoffFile(ByVal fileName As String) As Boolean
        Dim result As Boolean = False
        Dim posColetilla As Integer = fileName.IndexOf("_")
        If posColetilla > 0 Then

            Dim coletilla As String = fileName.Substring(0, posColetilla)
            If coletilla = "eif" Then
                result = True
            End If
        End If

        Return result
    End Function

    Private Sub InitVariables()

        strEmail = CStr("" & Request.Form("email"))
        If Not (Request.Form("dateBirth") Is Nothing) Then
            dateOfBirth = CDate("" & Request.Form("dateBirth"))
        End If
        strName = CStr("" & Request.Form("firstName"))
        strLastName = CStr("" & Request.Form("lastName"))
        strMiddleName = CStr("" & Request.Form("middleName"))
        strCity = CStr("" & Request.Form("city"))
        If Not (Request.Form("lopd") Is Nothing) Then
            boolOptin = True
            strCesionFoto = "Si"
        Else
            boolOptin = False
            strCesionFoto = "No"
        End If

        Dim files As HttpFileCollection = Request.Files
        If Not (files Is Nothing) Then
            If files.Count > 0 Then
                filePicture = files.Get(0)
            End If
        End If

        credentials.AppID = AppSettings("AppID")
        credentials.SOAPKey = AppSettings("SoapKey")

    End Sub

    Private Function AddToCampaing() As Boolean
        Dim result As Boolean = False
        Dim profile As Bacardi.DMS.Core.Interfaces.ConsumerProfile = dmsClient.GetConsumerProfile_1(credentials, strEmail)

        If profile Is Nothing Then
            profile = New Bacardi.DMS.Core.Interfaces.ConsumerProfile
        End If

        profile.FirstName = strName
        profile.LastName = strLastName
        profile.MiddleName = strMiddleName
        profile.City = strCity
        profile.DateOfBirth = dateOfBirth
        profile.Email = strEmail
        profile.EmailOptIn = boolOptin

        Dim responseAdd As Bacardi.DMS.Core.Interfaces.AddConsumerProfileResponse = dmsClient.AddConsumerProfile_1(credentials, profile, campaingId, Bacardi.DMS.Core.Interfaces.ConsumerSourceType.WebsiteOptInForm, True, "")
        result = responseAdd.SuccessFlag
        intConsumerId = responseAdd.NewConsumerID

        If result Then
            Dim responseSetField As Bacardi.DMS.Core.Remote.CampaignFieldDataResponse = dmsClient.SetCampaignFieldValue_1(credentials, strEmail, customFieldFotoUpload, strCesionFoto)

            If responseSetField = Bacardi.DMS.Core.Remote.CampaignFieldDataResponse.ConsumerIsNotInCampaign Then
                Response.Write("<respuesta estado='ko'><mensaje>El consumidor no está asociado a la campaña</mensaje></respuesta>")
                result = False
            ElseIf responseSetField = Bacardi.DMS.Core.Remote.CampaignFieldDataResponse.InvalidCampaignCustomField Then
                Response.Write("<respuesta estado='ko'><mensaje>Campo personalizado no válido</mensaje></respuesta>")
                result = False
            ElseIf responseSetField = Bacardi.DMS.Core.Remote.CampaignFieldDataResponse.InvalidConsumer Then
                Response.Write("<respuesta estado='ko'><mensaje>Consumidor no válido</mensaje></respuesta>")
                result = False
            ElseIf responseSetField = Bacardi.DMS.Core.Remote.CampaignFieldDataResponse.OK Then
                result = True
            End If
        Else
            If responseAdd.ResponseMessage = Bacardi.DMS.Core.Interfaces.AddConsumerProfileMessage.ConsumerAlreadyExists Then
                Response.Write("<respuesta estado='ko'><mensaje>Usuario ya registrado</mensaje></respuesta>")
                result = False
            ElseIf Not (responseAdd.ResponseMessage = Bacardi.DMS.Core.Interfaces.AddConsumerProfileMessage.ConsumerAlreadyExists) And Not (responseAdd.ResponseMessage = Bacardi.DMS.Core.Interfaces.AddConsumerProfileMessage.OK) Then
                Response.Write("<respuesta estado='ko'><mensaje>Formato de los campos invalido</mensaje></respuesta>")
                result = False
            End If
        End If

        Return result

    End Function

    Private Function UploadPicture() As Boolean
        Dim result As Boolean = False

        If Not (filePicture Is Nothing) Then
            'Response.Write("<fichero><nombre>" & filePicture.FileName.ToString & "</nombre><extension>" & filePicture.FileName.Substring(filePicture.FileName.LastIndexOf(".")) & "</extension><fileType>" & filePicture.ContentType.ToString & "</fileType><peso>" & filePicture.ContentLength.ToString & "</peso></fichero>")
            If Len(filePicture.FileName) > 0 And filePicture.ContentLength <= PesoMaximo And ExtensionOK(filePicture.FileName) And filePicture.ContentType = "application/octet-stream" And IsEristoffFile(filePicture.FileName) Then
                Try
                    Dim fileName As String = intConsumerId.ToString & filePicture.FileName.Substring(filePicture.FileName.LastIndexOf("."))
                    filePicture.SaveAs(strBaseLocation & "\" & fileName)
                    result = True
                    'Response.Write("<resultado>File " & strBaseLocation & "\" & fileName & " uploaded successfully</reultado>")
                Catch ex As Exception
                    result = True
                    'Response.Write("<respuesta estado='ko'><mensaje>Error al guardar la imagen. Motivo: " & ex.Message & "</mensaje></respuesta>")
                End Try
            Else
                result = False
                Response.Write("<respuesta estado='ko'><mensaje>La imagen proporcionada es incorrecta o tiene un tamaño superior a 500KB o no es un fichero JPG válido</mensaje></respuesta>")
            End If
        Else
            result = False
            Response.Write("<respuesta estado='ko'><mensaje>No se ha proporcionado ninguna imagen</mensaje></respuesta>")
        End If

        Return result

    End Function

End Class
