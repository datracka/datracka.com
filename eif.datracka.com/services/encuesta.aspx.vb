Imports System.Xml
Imports System.Web
Imports System.Net
Imports System.Configuration.ConfigurationManager

Partial Class services_encuesta
    Inherits System.Web.UI.Page

    Dim da As New datos
    Dim id_encuesta As String = ""
    Dim nVoto As Integer = 0
    Dim nRespuesta As Integer = 0

    Dim stRespuesta As String = ""


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        id_encuesta = Trim("" & Request.QueryString("e"))
        nVoto = CType(Trim("0" & Request.QueryString("v")), Integer)

        ' si nos llega la variable de la encuesta y se quiere votar
        If id_encuesta <> "" And nVoto < 9 And nVoto > 0 Then
            stRespuesta = da.ManejaEncuesta(id_encuesta, nVoto)
            If stRespuesta = "1" Then
                nRespuesta = 1
            End If
        ElseIf nVoto = 0 Then
            nRespuesta = 1
        End If

        Response.Write("<salida><estado>" & nRespuesta & "</estado><error>")
        If nRespuesta = 0 Then
            If stRespuesta = "0" Then
                Response.Write("Error en la inserción")
            End If

            If stRespuesta = "" Then
                Response.Write("No hay datos")
            End If
        End If


        Response.Write("</error><datos>")
        If id_encuesta <> "" Then
            Response.Write(LeeEncuesta(id_encuesta))
        End If
        Response.Write("</datos></salida>")

    End Sub


    Function LeeEncuesta(ByVal id_encuesta As String) As String
        Dim dt1 As New Data.DataTable
        Dim nResultado As Integer = 0

        Dim stDatos As String = ""


        ' leemos los datos en la base de datos, si hay error, los mandamos vacíos
        Try
            dt1 = da.GetEncuestaData(id_encuesta)



            ' si hay datos
            If Trim(CType("" & dt1.Rows(0).Item("res1"), String)) <> "" Then
                stDatos = stDatos & "<res1>" & CType("" & dt1.Rows(0).Item("res1"), String) & "</res1>"
                stDatos = stDatos & "<res2>" & CType("" & dt1.Rows(0).Item("res2"), String) & "</res2>"
                stDatos = stDatos & "<res3>" & CType("" & dt1.Rows(0).Item("res3"), String) & "</res3>"
                stDatos = stDatos & "<res4>" & CType("" & dt1.Rows(0).Item("res4"), String) & "</res4>"
                stDatos = stDatos & "<res5>" & CType("" & dt1.Rows(0).Item("res5"), String) & "</res5>"
                stDatos = stDatos & "<res6>" & CType("" & dt1.Rows(0).Item("res6"), String) & "</res6>"
                stDatos = stDatos & "<res7>" & CType("" & dt1.Rows(0).Item("res7"), String) & "</res7>"
                stDatos = stDatos & "<res8>" & CType("" & dt1.Rows(0).Item("res8"), String) & "</res8>"
                nRespuesta = 1
                stRespuesta = "1"

            Else
                stDatos = ""

            End If



        Catch ex As Exception
            stDatos = ""

        End Try

        Return stDatos

    End Function

End Class
