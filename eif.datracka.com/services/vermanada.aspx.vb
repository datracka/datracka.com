Imports System.Xml
Imports System.Web
Imports System.Net
Imports System.Configuration.ConfigurationManager

Partial Class vermanada
    Inherits System.Web.UI.Page
    Dim da As New datos
    Dim dms As New dms
    Dim nRespuesta As Integer = 0
    Dim nRespuestaRk As Integer = 0
    Dim stGuid As String = ""

    ' Datos genarales de la manada
    Dim stManada As String = ""
    Dim stPuntuacion As String = "<puntuacion>0</puntuacion>"
    Dim stPosicion As String = "<posicion>0</posicion>"

    Dim stDatos As String = ""
    Dim stUserID As String = ""



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' si viene el parámetro

        If Trim("" & Request.Form("guidgroup")) <> "" Then
            stGuid = Request.Form("guidgroup")
            nRespuestaRk = LeeRanking(stGuid)
            nRespuesta = LeeManada(stGuid)
        Else


        End If


        Response.Write("<salida><estado>" & nRespuesta & "</estado><error>")

        If nRespuestaRk < 1 Then
            Response.Write("Error de lectura en el ranking" & "</error>")
        ElseIf nRespuesta < 1 Then
            Response.Write("Error de lectura de la manada" & "</error>")
        ElseIf stDatos = "" Then
            Response.Write("No se ha encontrado el código del grupo" & "</error>")
        Else
            Response.Write("</error><datos>")

            Response.Write(stManada)
            Response.Write(stPuntuacion)
            Response.Write(stPosicion)

            If stDatos <> "" Then
                Response.Write("<bloggers>" & stDatos & "</bloggers>")
            End If

            Response.Write("</datos>")

        End If


        Response.Write("</salida>")


    End Sub

    Function LeeRanking(ByVal guid As String) As Integer
        Dim dt1 As New Data.DataTable
        Dim nResultado As Integer = 0

        Dim resGuid As String = ""

        Dim nTipoGrupo As Integer = 0

        nTipoGrupo = CType(da.GetTipoGrupoByGuid(guid), Integer)
 

        ' leemos los datos en la base de datos, si hay error, los mandamos vacíos
        Try
            dt1 = da.GetRankingManada(nTipoGrupo)

            'recorremos los votos y buscamos la posición 
            For i As Integer = 0 To dt1.Rows.Count - 1
                resGuid = CType(dt1.Rows(i).Item("guid_grupo"), String)

                If Trim("" & resGuid) = Trim(stGuid) Then
                    stPuntuacion = "<puntuacion>" & CType(dt1.Rows(i).Item("num_votos"), String) & "</puntuacion>"
                    stPosicion = "<posicion>" & CType(i + 1, String) & "</posicion>"
                End If

            Next

            nResultado = 1

        Catch ex As Exception

            stDatos = ""
            nResultado = 0
        End Try

        Return nResultado

    End Function

    Function LeeManada(ByVal guid As String) As Integer
        Dim dt1 As New Data.DataTable
        Dim nResultado As Integer = 0
        Dim strResultado As String = ""
        Dim strVariables As String = ""

        ' leemos los datos en la base de datos, si hay error, los mandamos vacíos
        Try
            dt1 = da.GetManada(guid)

            'datos 
            For i As Integer = 0 To dt1.Rows.Count - 1
                stManada = "<manada>" & CType(dt1.Rows(i).Item("nom_grupo"), String) & "</manada>"

                stDatos = stDatos & "<blogger><user>" & CType(dt1.Rows(i).Item("fk_cod_blogger"), Integer) & "</user>"
                stDatos = stDatos & "<tipo>" & CType(dt1.Rows(i).Item("fk_cod_tipoblogger"), String) & "</tipo>"

                ' enviamos XML y cargamos el resultado
                strVariables = "<consumerID>" & CType(dt1.Rows(i).Item("fk_cod_blogger"), String) & "</consumerID>"
                strResultado = dms.FunEnviaXML(strVariables, "GetConsumerProfileByConsumerID")

                SubLeeXMLBlogger(strResultado)


                stDatos = stDatos & "<blog>" & CType(dt1.Rows(i).Item("gls_blog"), String) & "</blog>"
                stDatos = stDatos & "<urlblog>" & CType(dt1.Rows(i).Item("gls_url_blog"), String) & "</urlblog>"
                stDatos = stDatos & "</blogger>"
            Next

            nResultado = 1

        Catch ex As Exception
            stManada = "<manada>" & "" & "</manada>"
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
                    stDatos = stDatos & (PaginaXML.Value)

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
        stDatos = Replace(stDatos, "<ConsumerID>", "<user>")
        stDatos = Replace(stDatos, "</ConsumerID>", "</user>")
        stDatos = Replace(stDatos, "<Password>", "")

        ' Si está vacío
        If Trim("" & stDatos) = "Empty Element" Then
            stDatos = ""
        End If



    End Sub

End Class
