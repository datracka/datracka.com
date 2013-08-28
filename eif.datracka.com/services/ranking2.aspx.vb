Imports System.Xml
Imports System.Web
Imports System.Net
Imports System.Configuration.ConfigurationManager

Partial Class ranking
    Inherits System.Web.UI.Page
    Dim da As New datos
    Dim nRespuesta As Integer = 0
    Dim stDatos As String = ""

    ' Datos genarales del concierto
    Dim stConcierto As String = ""
    Dim stProvincia As String = ""
    Dim stSala As String = ""
    Dim stFecha As String = ""

    Dim nTipogrupo As Integer = 0
    Dim stUserID As String = ""

    Dim dms As New dms


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Trim("" & Request.Form("tipogrupo")) <> "" Then
            nTipogrupo = CType(Request.Form("tipogrupo"), Integer)
        End If


        nRespuesta = LeeRanking(nTipogrupo)
        Response.ContentType = "text/xml"
        Response.Write("<?xml version=""1.0"" encoding=""windows-1252""?>" & vbCrLf)
        'Response.Write("<salida><error>")

        If nRespuesta < 1 Then
            'Response.Write("Error de lectura del ranking" & "</error>")
        Else
            'Response.Write("</error><datos>")

            'Response.Write(stConcierto)
            'Response.Write(stProvincia)
            'Response.Write(stSala)
            'Response.Write(stFecha)


            stDatos = Replace(stDatos, vbCrLf, "")
            stDatos = Replace(stDatos, Chr(10), "")
            stDatos = Replace(stDatos, Chr(13), "")

            stDatos = Replace(stDatos, "><", ">" & vbCrLf & "<")

            If stDatos <> "" Then
                Response.Write("<ranking>" & stDatos & "</ranking>")
            End If

            'Response.Write("</datos>")

        End If


        'Response.Write("</salida>")


    End Sub
    Function LeeRanking(ByVal tipogrupo As Integer) As Integer
        Dim dt1 As New Data.DataTable
        Dim nResultado As Integer = 0
        Dim strResultado As String = ""
        Dim strVariables As String = ""

        Dim stGroupant As String = ""

        ' leemos los datos en la base de datos, si hay error, los mandamos vacíos
        Try
            dt1 = da.GetRanking(tipogrupo)

            'datos 
            For i As Integer = 0 To dt1.Rows.Count - 1
         

                If stGroupant <> CType(dt1.Rows(i).Item("guid_grupo"), String) Then
                    If stGroupant <> "" Then
                        stDatos = stDatos & "</users></grupo>"
                    End If

                    stDatos = stDatos & "<grupo><manada><![CDATA[ " & Replace(Replace(CType(dt1.Rows(i).Item("nom_grupo"), String), "[", "-"), "]", "-") & " ]]></manada>"
                    stDatos = stDatos & "<groupguid><![CDATA[ " & CType(dt1.Rows(i).Item("guid_grupo"), String) & " ]]></groupguid>"
                    stDatos = stDatos & "<tipo><![CDATA[ " & CType(dt1.Rows(i).Item("fk_tipo_grupo"), String) & " ]]></tipo>"
                    stDatos = stDatos & "<votos><![CDATA[ " & CType(dt1.Rows(i).Item("num_votos"), String) & " ]]></votos><users>"


                    stDatos = stDatos & "<user><id><![CDATA[ " & CType(dt1.Rows(i).Item("fk_cod_blogger"), String) & " ]]></id>"
                    ' enviamos XML y cargamos el resultado
                    strVariables = "<consumerID>" & CType(dt1.Rows(i).Item("fk_cod_blogger"), String) & "</consumerID>"
                    strResultado = dms.FunEnviaXML(strVariables, "GetConsumerProfileByConsumerID")

                    SubLeeXMLBlogger(strResultado)



                    stDatos = stDatos & "<blog><![CDATA[ " & Replace(Replace(CType(dt1.Rows(i).Item("gls_blog"), String), "[", "-"), "]", "-") & " ]]></blog>"
                    stDatos = stDatos & "<url><![CDATA[ " & CType(dt1.Rows(i).Item("gls_url_blog"), String) & " ]]></url>"
                    stDatos = stDatos & "<tipouser><![CDATA[ " & CType(dt1.Rows(i).Item("fk_cod_tipoblogger"), String) & " ]]></tipouser></user>"

                    stGroupant = CType(dt1.Rows(i).Item("guid_grupo"), String)
                Else
                    stDatos = stDatos & "<user><id><![CDATA[ " & CType(dt1.Rows(i).Item("fk_cod_blogger"), String) & " ]]></id>"
                    ' enviamos XML y cargamos el resultado
                    strVariables = "<consumerID>" & CType(dt1.Rows(i).Item("fk_cod_blogger"), String) & "</consumerID>"
                    strResultado = dms.FunEnviaXML(strVariables, "GetConsumerProfileByConsumerID")

                    SubLeeXMLBlogger(strResultado)
                    stDatos = stDatos & "<blog><![CDATA[ " & CType(dt1.Rows(i).Item("gls_blog"), String) & " ]]></blog>"
                    stDatos = stDatos & "<url><![CDATA[ " & CType(dt1.Rows(i).Item("gls_url_blog"), String) & " ]]></url>"
                    stDatos = stDatos & "<tipouser><![CDATA[ " & CType(dt1.Rows(i).Item("fk_cod_tipoblogger"), String) & " ]]></tipouser></user>"
                End If



            Next

            If stDatos <> "" Then
                stDatos = stDatos & "</users></grupo>"
            End If

            nResultado = 1

        Catch ex As Exception
            stConcierto = "<concierto><![CDATA[ " & "" & " ]]></concierto>"
            stProvincia = "<provincia><![CDATA[ " & "" & " ]]></provincia>"
            stSala = "<sala><![CDATA[ " & "" & " ]]></sala>"
            stFecha = "<fecha><![CDATA[ " & "" & " ]]></fecha>"
            stDatos = ""
            nResultado = 0
        End Try

        Return nResultado

    End Function


    Sub SubLeeXMLBlogger(ByVal prXML As String)

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
                    stDatos = stDatos & "<![CDATA[ " & (PaginaXML.Value) & " ]]>"

                    If EtiquetaActual = "ConsumerID" Then
                        stUserID = PaginaXML.Value
                    End If

                    ' if EndElement, display it and decrement depth
                Case XmlNodeType.EndElement
                    stDatos = stDatos & ("</" & PaginaXML.Name & ">")


            End Select


        Loop

        stDatos = Replace(stDatos, "<GetConsumerProfileByConsumerIDResponse>", "")
        stDatos = Replace(stDatos, "<GetConsumerProfileByConsumerIDResult>", "")
        stDatos = Replace(stDatos, "</GetConsumerProfileByConsumerIDResponse>", "")
        stDatos = Replace(stDatos, "</GetConsumerProfileByConsumerIDResult>", "")


        stDatos = Replace(stDatos, "<GetConsumerProfileResponse>", "")
        stDatos = Replace(stDatos, "</GetConsumerProfileResponse>", "")
        stDatos = Replace(stDatos, "<ConsumerID>", "<user1>")
        stDatos = Replace(stDatos, "</ConsumerID>", "</user1>")
        stDatos = Replace(stDatos, "<Password>", "")


        ' Si está vacío
        If Trim("" & stDatos) = "Empty Element" Then
            stDatos = ""
        End If



    End Sub


End Class
