
Imports System.Xml
Imports System.Web
Imports System.Net
Imports System.Configuration.ConfigurationManager
Imports System.Data

Partial Class votos5914
    Inherits System.Web.UI.Page

    Dim da As New datos
    Dim stGuid As String = ""
    Dim nRespuesta As Integer = 0

    Dim stRespuesta As String = ""
    Dim stIp As String = ""

    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        ' definimos variables
        stIp = Request.UserHostAddress
        stGuid = Trim("" & Request.QueryString("guid"))

        If stGuid <> "" And Request.Browser.Cookies = True Then

            ' comprobamos que no haya cookie
            If CompruebaCookie(stGuid) = 1 Then

                ' enviamos cookie, para evitar que sea otra cosa, o que no acepte cookies
                If (da.GetIPGrupo(stIp, stGuid)) = "1" Then
                    nRespuesta = da.InsRanking(stGuid)
                    stRespuesta = nRespuesta
                    EnviaCookie(stGuid)
                End If


            Else
                stRespuesta = "-2"
            End If

        Else
            stRespuesta = "-1"
        End If


        ' si viene del flash respondemos
        ' si no enviamos

        Server.Transfer("gracias.html")
        'Response.Redirect(AppSettings("urlClick"))
        Response.End()




    End Sub

    Function CompruebaCookie(ByVal sGuid As String) As Integer
        Dim stCookie As String = ""

        Dim I As Integer
        For I = 0 To Request.Cookies.Count - 1
            If Request.Cookies.Item(I).Name = sGuid Then
                stCookie = Request.Cookies.Item(I).Value
            End If
        Next

        If Trim("" & stCookie) = "" Then
            Return 1
        Else
            Return CType(stCookie, Integer)
        End If

    End Function

    Function EnviaCookie(ByVal guid As String) As Boolean
        Response.Cookies(guid).Value = "99"
        Response.Cookies(guid).Expires = "01/01/2025"
    End Function
End Class
