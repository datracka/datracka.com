Imports System.Xml
Imports System.Web
Imports System.Net
Imports System.Configuration.ConfigurationManager
Partial Class getblogger
    Inherits System.Web.UI.Page
    Dim nRespuesta As Integer = 0
    Dim stDatos As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Trim("" & Request.Form("email")) <> "" Then
            Dim strResultado As String = ""
            Dim strVariables As String = ""
            Dim dms As New dms

            ' montamos variables a enviar
            strVariables = _
                "<consumerEmail>" & Request.Form("email") & "</consumerEmail>"


            ' enviamos XML y cargamos el resultado
            strResultado = dms.FunEnviaXML(strVariables, "GetConsumerProfile")


            ' leemos la respuesta
            'Response.Write(strResultado)
            'Response.End()

            nRespuesta = SubLeeXML(strResultado)
        Else
            nRespuesta = -1
        End If

        ' damos la salida
        Response.Write("<salida><estado>" & nRespuesta & "</estado><error>")

        If nRespuesta < 1 Then
            Response.Write("Usuario no existe o e-mail no indicado")
        End If

        Response.Write("</error>")

        If Trim("" & stDatos) <> "" Then
            Response.Write ("<datos>" & stDatos & "</datos>")
        End If

        Response.Write("</salida>")

    End Sub


    Function SubLeeXML(ByVal prXML As String) As Integer

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
            EtiquetaActual = PaginaXML.Name


            ' stContenido = PaginaXML.ReadElementContentAsString


            Select Case PaginaXML.NodeType

                ' if Element, display its name
                Case XmlNodeType.Element

                    ' increase tab depth
                    stDatos = stDatos & ("<" & PaginaXML.Name & ">")

                    ' if empty element, decrease depth
                    If PaginaXML.IsEmptyElement Then
                        stDatos = stDatos & ("Empty Element")
                    End If


                Case XmlNodeType.Text ' if Text, display it
                    stDatos = stDatos & (PaginaXML.Value)

 

                    ' if EndElement, display it and decrement depth
                Case XmlNodeType.EndElement
                    stDatos = stDatos & ("</" & PaginaXML.Name & ">")


            End Select


        Loop


        stDatos = Replace(stDatos, "<GetConsumerProfileResponse>", "")
        stDatos = Replace(stDatos, "</GetConsumerProfileResponse>", "")

        ' Si está vacío
        If Trim("" & stDatos) = "Empty Element" Then
            stDatos = ""
        End If

        If Trim("" & stDatos) <> "" Then
            Return 1
        Else
            Return -1
        End If

    End Function
End Class
