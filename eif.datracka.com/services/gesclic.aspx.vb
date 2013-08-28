﻿
Imports System.Xml
Imports System.Web
Imports System.Net
Imports System.Configuration.ConfigurationManager

Partial Class gestionclick
    Inherits System.Web.UI.Page

    Dim da As New datos
    Dim stGuid As String = ""
    Dim nRespuesta As Integer = 0

    Dim stRespuesta As String = ""


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        stGuid = Trim("" & Request.QueryString("guid"))

        If stGuid <> "" Then
            If CompruebaCookie(stGuid) > 0 Then
                ' nRespuesta = da.InsRanking(stGuid)
                'stRespuesta = nRespuesta
                'EnviaCookie(stGuid)
            Else
                stRespuesta = "-2"
            End If

        Else
            stRespuesta = "-1"
        End If


        ' si viene del flash respondemos
        ' si no enviamos
        If Trim("" & Request.QueryString("fla")) = "1" Then
            Response.Write(stRespuesta)
        Else
            Response.Redirect(AppSettings("urlClick"))
            Response.End()
        End If



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
            Return 0
        End If

    End Function

    Sub EnviaCookie(ByVal guid As String)
        Response.Cookies(guid).Value = "1"
        Response.Cookies(guid).Expires = "01/01/2025"
    End Sub
End Class
