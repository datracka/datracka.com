Imports System.Xml
Imports System.Web
Imports System.Net
Imports System.Configuration.ConfigurationManager
Partial Class Default2
    Inherits System.Web.UI.Page

    Dim nRespuesta As Integer = 0

    Dim stDatos As String = ""
    Dim stUserID As String = ""
    Dim da As New datos

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Trim("" & Request.Form("email")) <> "" And Trim("" & Request.Form("password")) <> "" Then
            Dim strResultado As String = ""
            Dim strVariables As String = ""
            Dim dms As New dms

            ' montamos variables a enviar
            strVariables = _
                "<email>" & Request.Form("email") & "</email>" & _
                "<password>" & Request.Form("password") & "</password>"

            ' enviamos XML y cargamos el resultado
            strResultado = dms.FunEnviaXML(strVariables, "CheckConsumerLogin")

            ' leemos la respuesta
            'Response.Write(strResultado)
            nRespuesta = SubLeeXML(strResultado)
        Else
            nRespuesta = -1
        End If

        ' damos la salida
        Response.Write("<salida><estado>" & nRespuesta & "</estado><error>")

        If nRespuesta < 1 Then
            Response.Write("El nombre de usuario o contraseña no son correctos" & "</error>")
        Else
            Response.Write("</error>")
            LeeBlogger(Request.Form("email"))
        End If


        Response.Write("</salida>")

    End Sub

    Sub LeeBlogger(ByVal stEmail As String)

        Dim strResultado As String = ""
        Dim strVariables As String = ""
        Dim dms As New dms

        ' montamos variables a enviar
        strVariables = _
            "<consumerEmail>" & stEmail & "</consumerEmail>"


        ' enviamos XML y cargamos el resultado
        strResultado = dms.FunEnviaXML(strVariables, "GetConsumerProfile")

        ' leemos el blogger en el DMS
        nRespuesta = SubLeeXMLBlogger(strResultado)

        ' leemos los datos en la base de datos
        If stUserID <> "" Then
            LeeBloggerSQL(stUserID)
        End If

        ' exportamos datos
        If Trim("" & stDatos) <> "" Then
            Response.Write("<datos>" & stDatos & "</datos>")
        End If


    End Sub


    Function SubLeeXML(ByVal prXML As String) As Integer
        Dim vrXml As System.IO.StringReader = New System.IO.StringReader(prXML)
        Dim PaginaXML As New XmlTextReader(vrXml)
        Dim EtiquetaActual As String = ""
        Dim stResultado As String = ""

        Do While (PaginaXML.Read())
            EtiquetaActual = PaginaXML.Name
            If EtiquetaActual = "CheckConsumerLoginResult" Then stResultado = PaginaXML.ReadElementContentAsString
        Loop

        If Trim("" & stResultado) = "true" Then
            Return 1
        Else
            Return -1
        End If

    End Function

    Function SubLeeXMLBlogger(ByVal prXML As String) As Integer

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


        stDatos = Replace(stDatos, "<GetConsumerProfileResponse>", "")
        stDatos = Replace(stDatos, "</GetConsumerProfileResponse>", "")
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

    Function LeeBloggerSQL(ByVal id As Integer) As Integer
        Dim dt1 As New Data.DataTable

        ' leemos los datos en la base de datos, si hay error, los mandamos vacíos
        Try
            dt1 = da.GetBloggerTotal(id)
            'datos propios
            With dt1.Rows(0)

                stDatos = stDatos & "<groupguid>" & CType(.Item("guid_grupo"), String) & "</groupguid>"
                stDatos = stDatos & "<miembros>" & CType(da.GetMiembrosGrupoByGuid(CType(.Item("guid_grupo"), String)), Integer) & "</miembros>"
                stDatos = stDatos & "<blog>" & CType(.Item("gls_blog"), String) & "</blog>"
                stDatos = stDatos & "<urlblog>" & CType(.Item("gls_url_blog"), String) & "</urlblog>"
                stDatos = stDatos & "<manada>" & CType(.Item("nom_grupo"), String) & "</manada>"
                stDatos = stDatos & "<tipoblogger>" & CType(.Item("fk_cod_tipoblogger"), String) & "</tipoblogger>"
                stDatos = stDatos & "<tipogrupo>" & CType(.Item("fk_tipo_grupo"), String) & "</tipogrupo>"
            End With

        Catch ex As Exception
            stDatos = stDatos & "<groupguid></groupguid>"
            stDatos = stDatos & "<miembros></miembros>"
            stDatos = stDatos & "<blog></blog>"
            stDatos = stDatos & "<urlblog></urlblog>"
            stDatos = stDatos & "<manada></manada>"
            stDatos = stDatos & "<tipoblogger></tipoblogger>"
            stDatos = stDatos & "<tipogrupo></tipogrupo>"

        End Try



    End Function

End Class
