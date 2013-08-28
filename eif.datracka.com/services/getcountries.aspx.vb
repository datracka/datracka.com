Imports System.Xml
Imports System.Web
Imports System.Net
Imports System.Configuration.ConfigurationManager

Partial Class getcountries
    Inherits System.Web.UI.Page

    Dim nRespuesta As Integer = 0

    Dim stDatos As String = ""
    Dim stUserID As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim strResultado As String = ""
        Dim strVariables As String = ""
        Dim dms As New dms

        ' enviamos XML y cargamos el resultado
        strResultado = dms.FunEnviaXMLGDF("", "GetCountryListAllArray_v1")


        ' leemos la respuesta
        'Response.Write(strResultado)
        nRespuesta = SubLeeXMLCountries(strResultado)



        ' damos la salida
        Response.Write("<salida><estado>" & nRespuesta & "</estado><error>")

        If nRespuesta < 1 Then
            Response.Write("Error" & "</error>")

        Else
            Response.Write("</error>")
            Response.Write("<datos><countries>" & stDatos & "</countries></datos>")
        End If


        Response.Write("</salida>")

    End Sub


    Function SubLeeXMLCountries(ByVal prXML As String) As Integer

        prXML = Replace(prXML, "<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""><soap:Body>", "")
        prXML = Replace(prXML, "</soap:Body></soap:Envelope>", "")
        prXML = Replace(prXML, "<GetCountryListAllArray_v1Response xmlns=""https://webservices.bacardi.com/gdfGateway/GDF.asmx"">", "")
        prXML = Replace(prXML, "</GetCountryListAllArray_v1Response>", "")

        'Response.Write(prXML)
        'Response.End()


        Dim vrXml As System.IO.StringReader = New System.IO.StringReader(prXML)
        Dim PaginaXML As New XmlTextReader(vrXml)
        Dim EtiquetaActual As String = ""
        Dim stResultado As String = ""
        Dim stContenido As String = ""



        Do While (PaginaXML.Read())
            ' EtiquetaActual = PaginaXML.Name


            ' stContenido = PaginaXML.ReadElementContentAsString


            Select Case PaginaXML.NodeType

                ' if Element, display its name
                Case XmlNodeType.Element
                    EtiquetaActual = PaginaXML.Name
                    ' increase tab depth
                    stDatos = stDatos & ("<" & EtiquetaActual & ">")

                    ' if empty element, decrease depth
                    If PaginaXML.IsEmptyElement Then
                        stDatos = stDatos & ("")
                    End If




                Case XmlNodeType.Text ' if Text, display it
                    stDatos = stDatos & (PaginaXML.Value)

                    If EtiquetaActual = "ConsumerID" Then
                        stUserID = PaginaXML.Value
                    End If

                    ' if EndElement, display it and decrement depth
                Case XmlNodeType.EndElement
                    stDatos = stDatos & ("</" & PaginaXML.Name & ">")


            End Select


        Loop


        stDatos = Replace(stDatos, "<GetCountryListAllArray_v1Result>", "")
        stDatos = Replace(stDatos, "</GetCountryListAllArray_v1Result>", "")
        stDatos = Replace(stDatos, "<ConsumerID>", "<user>")
        stDatos = Replace(stDatos, "</ConsumerID>", "</user>")
        stDatos = Replace(stDatos, "<Password>", "")

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
