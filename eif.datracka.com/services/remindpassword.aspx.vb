Imports System.Configuration.ConfigurationManager
Imports System.Data
Imports System.IO
Imports System.Xml
Partial Class services_remindpassword
    Inherits System.Web.UI.Page

    ' genéricos
    Dim dm As New dms

    ' Datos del blogger
    Dim stEmail As String = ""
    Dim stRespuestaDMS As String = ""
    Dim stRespuestaDMS1 As String = ""
    Dim intRespuesta As Integer = 0
    Dim stPassword As String = ""

    Dim correo As New correo
    Dim f As New fichero



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If Trim("" & Request.Form("email")) <> "" Then
            stEmail = CType("" & Request.Form("email"), String)

            If Trim("" & Request.Form("pwd")) <> "" Then
                stPassword = Request.Form("pwd")
            Else
                stPassword = GetSimpleUserPassword(10)
            End If



            'Response.Write(RemindPassword(stEmail))
            intRespuesta = SubLeeXML(RemindPassword(stEmail))

            If intRespuesta = 1 Then
                MailPassword(stEmail)
            End If

        End If


        ' damos la salida


        Response.Write("<salida><estado>" & intRespuesta & "</estado><error>" & stRespuestaDMS1 & "</error><datos></datos></salida>")


    End Sub

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

            End Select


        Loop



        If stBolResult = "true" Then
            Return 1
        Else
            Return 0
        End If
    End Function

    Function RemindPassword(ByVal Email As String) As String
        Dim stXML As String = ""
        Dim stRespuesta As String = ""

        ' montamos la cadena del xml

        stXML = stXML & "<consumerEmail>" & Email & "</consumerEmail>"
        stXML = stXML & "<generatePassword>false</generatePassword>"
        stXML = stXML & "<newPassword>" & stPassword & "</newPassword>"
        stXML = stXML & "<marketID>36</marketID>"


        stRespuesta = dm.FunEnviaXML(stXML, "ResetConsumerPassword")
        stRespuestaDMS = stXML & stRespuesta
        Return (stRespuesta)

    End Function

    Function GetSimpleUserPassword(Optional ByVal length As Int16 = 8) As String

        'Get the GUID

        Dim guidResult As String = System.Guid.NewGuid().ToString()

        'Remove the hyphens

        guidResult = guidResult.Replace("-", String.Empty)

        'Make sure length is valid

        If length <= 0 OrElse length > guidResult.Length Then

            Return guidResult

        Else

            'Return the first length bytes

            Return guidResult.Substring(0, length)

        End If

    End Function

    Sub MailPassword(ByVal stemailb As String)
        Dim stBody As String = ""
        Dim stUrl As String = ""
        Dim stSubject As String = "Recordatorio de contraseña"



        stBody = f.Lee(Server.MapPath("../mailings/recordar_password/index.html"))

        stBody = Replace(stBody, "[[PASSWORD]]", stPassword)
        stBody = Replace(stBody, """img/", """" & AppSettings("dominio") & "/mailings/recordar_password/img/")


        If Trim("" & stemailb) <> "" Then
            correo.EnviarEmail("info@eristoffinternativefestival.com", stemailb, stSubject, stBody, True)
        End If
    End Sub

End Class

