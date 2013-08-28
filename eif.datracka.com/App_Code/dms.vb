
Imports System.Xml
Imports System.Web
Imports System.Net
Imports System.Configuration.ConfigurationManager

Public Class dms

    Function FunEnviaXML(ByVal prCadena As String, ByVal prMetodo As String) As String
        Dim objHTTP As New MSXML.XMLHTTPRequest
        Dim strEnvelope As String
        Dim strReturn As String
        Dim objReturn As New MSXML.DOMDocument




        'Create the SOAP Envelope
        strEnvelope = _
            "<?xml version=""1.0"" encoding=""utf-8""?>" & _
            "<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" " & _
            "xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">" & _
            "<soap:Body>" & _
            "<" & prMetodo & " xmlns=""https://webservices.bacardi.com/crm/"">" & _
            "<siteID>" & AppSettings("siteID") & "</siteID>" & _
            "<siteKey>" & AppSettings("siteKey") & "</siteKey>" & _
            prCadena & _
            "</" & prMetodo & ">" & _
            "</soap:Body>" & _
            "</soap:Envelope>"



        'Set up to post to the server
        objHTTP.open("post", AppSettings("urlDms"), False, "", "")

        objHTTP.setRequestHeader("Content-Type", "text/xml; charset=utf-8")


        'Make the SOAP call
        objHTTP.send(strEnvelope)

        'Get the return envelope
        strReturn = objHTTP.responseText
        FunEnviaXML = strReturn

    End Function

    Function FunEnviaXMLGDF(ByVal prCadena As String, ByVal prMetodo As String) As String
        Dim objHTTP As New MSXML.XMLHTTPRequest
        Dim strEnvelope As String
        Dim strReturn As String
        Dim objReturn As New MSXML.DOMDocument




        'Create the SOAP Envelope
        strEnvelope = _
            "<?xml version=""1.0"" encoding=""utf-8""?>" & _
            "<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" " & _
            "xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">" & _
            "<soap:Body>" & _
            "<" & prMetodo & " xmlns=""https://webservices.bacardi.com/gdfGateway/GDF.asmx"">" & _
            "<siteID>" & AppSettings("siteID") & "</siteID>" & _
            "<siteKey>" & AppSettings("siteKey") & "</siteKey>" & _
            prCadena & _
            "</" & prMetodo & ">" & _
            "</soap:Body>" & _
            "</soap:Envelope>"



        'Set up to post to the server
        objHTTP.open("post", AppSettings("urlGdf"), False, "", "")

        objHTTP.setRequestHeader("Content-Type", "text/xml; charset=utf-8")
        objHTTP.setRequestHeader("SOAPAction", "https://webservices.bacardi.com/gdfGateway/GDF.asmx/" & prMetodo)



        'Make the SOAP call
        objHTTP.send(strEnvelope)

        'Get the return envelope
        strReturn = objHTTP.responseText
        FunEnviaXMLGDF = strReturn

    End Function

End Class
