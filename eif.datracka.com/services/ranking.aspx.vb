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


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Trim("" & Request.Form("tipogrupo")) <> "" Then
            nTipogrupo = CType(Request.Form("tipogrupo"), Integer)
        End If


        nRespuesta = LeeRanking(nTipogrupo)

        Response.Write("<salida><estado>" & nRespuesta & "</estado><error>")

        If nRespuesta < 1 Then
            Response.Write("Error de lectura del ranking" & "</error>")
        Else
            Response.Write("</error><datos>")

            Response.Write(stConcierto)
            Response.Write(stProvincia)
            Response.Write(stSala)
            Response.Write(stFecha)

            If stDatos <> "" Then
                Response.Write("<ranking>" & stDatos & "</ranking>")
            End If

            Response.Write("</datos>")

        End If


        Response.Write("</salida>")


    End Sub
    Function LeeRanking(ByVal tipogrupo As Integer) As Integer
        Dim dt1 As New Data.DataTable
        Dim nResultado As Integer = 0

        Dim stGroupant As String = ""

        ' leemos los datos en la base de datos, si hay error, los mandamos vacíos
        Try
            dt1 = da.GetRanking(tipogrupo)

            'datos 
            For i As Integer = 0 To dt1.Rows.Count - 1
                stConcierto = "<concierto>" & CType(dt1.Rows(i).Item("nom_concierto"), String) & "</concierto>"
                stProvincia = "<provincia>" & CType(dt1.Rows(i).Item("gls_provincia"), String) & "</provincia>"
                stSala = "<sala>" & CType(dt1.Rows(i).Item("gls_sala"), String) & "</sala>"
                stFecha = "<fecha>" & CType(dt1.Rows(i).Item("gls_fecha"), String) & "</fecha>"

                If stGroupant <> CType(dt1.Rows(i).Item("guid_grupo"), String) Then
                    If stGroupant <> "" Then
                        stDatos = stDatos & "</users></grupo>"
                    End If

                    stDatos = stDatos & "<grupo><manada>" & CType(dt1.Rows(i).Item("nom_grupo"), String) & "</manada>"
                    stDatos = stDatos & "<groupguid>" & CType(dt1.Rows(i).Item("guid_grupo"), String) & "</groupguid>"
                    stDatos = stDatos & "<tipo>" & CType(dt1.Rows(i).Item("fk_tipo_grupo"), String) & "</tipo>"
                    stDatos = stDatos & "<votos>" & CType(dt1.Rows(i).Item("num_votos"), String) & "</votos><users>"

                    stDatos = stDatos & "<user><id>" & CType(dt1.Rows(i).Item("fk_cod_blogger"), String) & "</id>"
                    stDatos = stDatos & "<blog>" & CType(dt1.Rows(i).Item("gls_blog"), String) & "</blog>"
                    stDatos = stDatos & "<url>" & CType(dt1.Rows(i).Item("gls_url_blog"), String) & "</url>"
                    stDatos = stDatos & "<tipouser>" & CType(dt1.Rows(i).Item("fk_cod_tipoblogger"), String) & "</tipouser></user>"

                    stGroupant = CType(dt1.Rows(i).Item("guid_grupo"), String)
                Else
                    stDatos = stDatos & "<user><id>" & CType(dt1.Rows(i).Item("fk_cod_blogger"), String) & "</id>"
                    stDatos = stDatos & "<blog>" & CType(dt1.Rows(i).Item("gls_blog"), String) & "</blog>"
                    stDatos = stDatos & "<url>" & CType(dt1.Rows(i).Item("gls_url_blog"), String) & "</url>"
                    stDatos = stDatos & "<tipouser>" & CType(dt1.Rows(i).Item("fk_cod_tipoblogger"), String) & "</tipouser></user>"
                End If



            Next

            If stDatos <> "" Then
                stDatos = stDatos & "</users></grupo>"
            End If

            nResultado = 1

        Catch ex As Exception
            stConcierto = "<concierto>" & "" & "</concierto>"
            stProvincia = "<provincia>" & "" & "</provincia>"
            stSala = "<sala>" & "" & "</sala>"
            stFecha = "<fecha>" & "" & "</fecha>"
            stDatos = ""
            nResultado = 0
        End Try

        Return nResultado

    End Function
End Class
