Imports System.Xml
Imports System.Web
Imports System.Net
Imports System.Configuration.ConfigurationManager
Partial Class countgroup
    Inherits System.Web.UI.Page
    Dim nRespuesta As Integer = 0
    Dim nDatos As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Trim("" & Request.Form("guidgroup")) <> "" Then
            Dim strResultado As String = ""
            Dim strVariables As String = ""
            Dim dms As New dms
            Dim da As New datos

            ' montamos variables a enviar
            nDatos = da.GetMiembrosGrupoByGuid(Request.Form("guidgroup"))
            nRespuesta = 1

        Else
            nRespuesta = 0
        End If

        ' damos la salida
        Response.Write("<salida><estado>" & nRespuesta & "</estado><error>")


        Response.Write("</error>")

        Response.Write("<datos>" & nDatos & "</datos>")


        Response.Write("</salida>")

    End Sub

End Class
