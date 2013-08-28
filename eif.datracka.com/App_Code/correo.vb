Imports Microsoft.VisualBasic
Imports System.Net.Mail
Imports System.Configuration.ConfigurationManager

Public Class correo
#Region "constructor"

    Public Sub New()
        '
    End Sub

#End Region
#Region "tareas"
    Public Function EnviarEmail(ByVal sFrom As String, ByVal sPara As String, ByVal sAsunto As String, ByVal sbody As String, ByVal sHtml As Boolean) As String
        Dim correo As New MailMessage
        correo.From = New System.Net.Mail.MailAddress(sFrom)
        correo.To.Add(sPara)
        correo.Subject = sAsunto
        correo.Body = sbody
        correo.IsBodyHtml = sHtml
        correo.Priority = System.Net.Mail.MailPriority.Normal
        Dim smtp As New System.Net.Mail.SmtpClient
        smtp.Host = AppSettings("smtp")
        smtp.Credentials = New System.Net.NetworkCredential(AppSettings("smtplogin"), AppSettings("smtpwd"))
        Try
            smtp.Send(correo)
            EnviarEmail = "Mensaje enviado satisfactoriamente"
        Catch ex As Exception
            EnviarEmail = "ERROR: " & ex.Message
        End Try
    End Function




    Public Function EnviaMailAmigo(ByVal amigo As String, ByVal guid As String) As Integer
        Dim nRespuesta As Integer = 1

        If Trim("" & amigo) <> "" Then

            EnviarEmail("info@eristoffinternativefestival.com", amigo, "invitación", "http://eristoff.bluemodus.com/test/index.html?mail=" & amigo & "&guid=" & guid, True)
        Else
            nRespuesta = -1
        End If

        Return nRespuesta

    End Function

#End Region
End Class
