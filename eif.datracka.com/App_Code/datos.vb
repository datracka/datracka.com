Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager

Public Class datos

#Region "privados"

    Private _con As New SqlConnection(ConnectionStrings.Item("conexion").ConnectionString)

#End Region

#Region "propiedades"

    Public Property Con() As SqlConnection
        Get
            Return _con
        End Get
        Set(ByVal value As SqlConnection)
            _con = value
        End Set
    End Property

#End Region

#Region "constructor"

    Public Sub New()
        '
    End Sub

#End Region


#Region "blogger"
    ''' <summary>
    ''' Inserta un blogger o actualiza sus datos en función del primer parámetro
    ''' </summary>
    ''' <param name="tipoConsulta">"INSERT" o "UPDATE"</param>
    ''' <returns>1 o 0 o -1 en función de si resultado ok o no</returns>
    ''' <remarks></remarks>
    Public Function InsUpdBlogger(ByVal b As blogger, ByVal tipoConsulta As String) As Integer
        'selecciono el sp en función del tipo de consulta
        Dim cmd As New SqlCommand()
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = Con
        If tipoConsulta = "INSERT" Then
            cmd.CommandText = "sp_InsBlogger"
            cmd.Parameters.Add(New System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, 10, 0, Nothing, System.Data.DataRowVersion.Current, False, Nothing, "", "", ""))
            cmd.Parameters.AddWithValue("@ctr_fecha_alta", b.Fecha_Alta)
        Else
            cmd.CommandText = "sp_UpdBlogger"
            cmd.Parameters.AddWithValue("@pk_cod_blogger", b.pk_cod_blogger)
            cmd.Parameters.AddWithValue("@ctr_fecha_mod", b.Fecha_Mod)
        End If


        'los parametros son los mismos en ambos casos
        cmd.Parameters.AddWithValue("@fk_cod_tipoblogger", b.fk_cod_tipo_blogger)
        cmd.Parameters.AddWithValue("@fk_newuserId", b.fk_newuserId)
        cmd.Parameters.AddWithValue("@gls_email", b.Email)
        cmd.Parameters.AddWithValue("@gls_blog", b.Blog)
        cmd.Parameters.AddWithValue("@gls_url_blog", b.Url_blog)
        cmd.Parameters.AddWithValue("@gls_aviso", b.Aviso)
        cmd.Parameters.AddWithValue("@cod_estado", b.Estado)



        'insertando o actualizando
        Con.Open()
        Dim res As Integer
        Try
            cmd.ExecuteScalar()
            res = cmd.Parameters(0).Value
        Catch ex As Exception
            res = 0
        Finally
            _con.Close()
        End Try
        Return res
    End Function


    Public Function InsGrupo(ByVal fk_tipo_grupo As Integer, ByVal fk_cod_blogger As Integer, ByVal guid_grupo As String, ByVal nom_grupo As String, ByVal ctr_fecha As Date, ByVal cod_estado As String) As Integer
        'selecciono el sp en función del tipo de consulta
        Dim cmd As New SqlCommand()
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = Con

        cmd.CommandText = "sp_InsGrupo"
        cmd.Parameters.Add(New System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, 10, 0, Nothing, System.Data.DataRowVersion.Current, False, Nothing, "", "", ""))


        'los parametros son los mismos en ambos casos
        cmd.Parameters.AddWithValue("@fk_tipo_grupo", fk_tipo_grupo)
        cmd.Parameters.AddWithValue("@fk_cod_blogger", fk_cod_blogger)
        cmd.Parameters.AddWithValue("@guid_grupo", guid_grupo)
        cmd.Parameters.AddWithValue("@nom_grupo", nom_grupo)
        cmd.Parameters.AddWithValue("@ctr_fecha_alta", ctr_fecha)
        cmd.Parameters.AddWithValue("@cod_estado", cod_estado)


        'insertando o actualizando
        Con.Open()
        Dim res As Integer
        Try
            cmd.ExecuteScalar()
            res = cmd.Parameters(0).Value
        Catch ex As Exception
            res = 0
        Finally
            _con.Close()
        End Try
        Return res
    End Function


    Public Function InsBlogGrupo(ByVal fk_cod_blogger As Integer, ByVal fk_cod_grupo As Integer, ByVal ctr_fecha As Date, ByVal cod_estado As String) As Integer
        'selecciono el sp en función del tipo de consulta
        Dim cmd As New SqlCommand()
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = Con

        cmd.CommandText = "sp_InsBlogGrupo"
        cmd.Parameters.Add(New System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, 10, 0, Nothing, System.Data.DataRowVersion.Current, False, Nothing, "", "", ""))


        'los parametros son los mismos en ambos casos

        cmd.Parameters.AddWithValue("@fk_cod_blogger", fk_cod_blogger)
        cmd.Parameters.AddWithValue("@fk_cod_grupo", fk_cod_grupo)
        cmd.Parameters.AddWithValue("@ctr_fecha_alta", ctr_fecha)
        cmd.Parameters.AddWithValue("@cod_estado", cod_estado)


        'insertando o actualizando
        Con.Open()
        Dim res As Integer
        Try
            cmd.ExecuteScalar()
            res = cmd.Parameters(0).Value
        Catch ex As Exception
            res = 0
        Finally
            _con.Close()
        End Try
        Return res
    End Function

    Public Function UpdateTipoBlogger(ByVal fk_newuserId As Integer, ByVal fk_cod_tipoblogger As Integer) As Integer
        'selecciono el sp en función del tipo de consulta
        Dim cmd As New SqlCommand()
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = Con

        cmd.CommandText = "sp_UpdTipoBlogger"
        cmd.Parameters.Add(New System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, 10, 0, Nothing, System.Data.DataRowVersion.Current, False, Nothing, "", "", ""))


        'los parametros son los mismos en ambos casos
        cmd.Parameters.AddWithValue("@fk_newuserId", fk_newuserId)
        cmd.Parameters.AddWithValue("@fk_cod_tipoblogger", fk_cod_tipoblogger)




        'insertando o actualizando
        Con.Open()
        Dim res As Integer
        Try
            cmd.ExecuteScalar()
            res = cmd.Parameters(0).Value
        Catch ex As Exception
            res = 0
        Finally
            _con.Close()
        End Try
        Return res
    End Function

    Public Function GetBloggerPorId(ByVal id As Integer) As DataTable
        Dim da As New SqlDataAdapter
        Dim dt As New DataTable
        Dim cmd As New SqlCommand("select * from ta_blogger where pk_cod_blogger=@id", Con)
        cmd.Parameters.AddWithValue("@id", id)
        da.SelectCommand = cmd
        da.Fill(dt)
        Return dt
    End Function


    Public Function GetBloggerTotal(ByVal id As Integer) As DataTable
        Dim da As New SqlDataAdapter
        Dim dt As New DataTable
        Dim cmd As New SqlCommand("SELECT ta_blogger.fk_newuserId, ta_blogger.pk_cod_blogger, ta_blogger.fk_cod_tipoblogger,ta_grupos.fk_tipo_grupo, ta_grupos.pk_cod_grupo, ta_grupos.guid_grupo, ta_grupos.nom_grupo, ta_blogger.gls_blog, ta_blogger.gls_url_blog FROM ta_blogger INNER JOIN ta_blog_grupo on ta_blogger.fk_newuserId = ta_blog_grupo.fk_cod_blogger INNER JOIN ta_grupos ON ta_blog_grupo.fk_cod_grupo = ta_grupos.pk_cod_grupo  WHERE     (dbo.ta_blogger.fk_newuserId = @id)", Con)
        cmd.Parameters.AddWithValue("@id", id)
        da.SelectCommand = cmd
        da.Fill(dt)
        Return dt
    End Function

#End Region

#Region "votar"
    Public Function InsRanking(ByVal stguid As String) As Integer
        'selecciono el sp en función del tipo de consulta
        Dim cmd As New SqlCommand()
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = Con

        cmd.CommandText = "sp_InsRanking"
        cmd.Parameters.Add(New System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, 10, 0, Nothing, System.Data.DataRowVersion.Current, False, Nothing, "", "", ""))


        'los parametros son los mismos en ambos casos
        cmd.Parameters.AddWithValue("@guid_grupo", stguid)

        'insertando o actualizando
        Con.Open()
        Dim res As Integer
        Try
            cmd.ExecuteScalar()
            res = cmd.Parameters(0).Value
        Catch ex As Exception
            res = 0
        Finally
            _con.Close()
        End Try
        Return res
    End Function

    Public Function GetIPGrupo(ByVal stIp As String, ByVal guid As String) As String
        Dim da As New SqlDataAdapter
        Dim res As String = ""
        Dim nMin As Integer = 0
        Dim nRes1 As Integer = 0
        Dim cmd As New SqlCommand("SELECT fecha from ta_ipvoto where ip = @ip and guid_grupo=@guid_grupo", Con)

        cmd.Parameters.AddWithValue("@ip", stIp)
        cmd.Parameters.AddWithValue("@guid_grupo", guid)

        Con.Open()

        Try
            res = cmd.ExecuteScalar()
            _con.Close()

            If Trim("" & res) = "" Then
                nRes1 = InsIpVoto(stIp, guid)

                If nRes1 > 0 Then
                    res = "1"
                Else
                    res = "0"
                End If
                'res = "no hay" & nRes1
            Else
                nMin = TiempoDiff(res)
                If nMin > 4 Then
                    nRes1 = UpdIpVoto(stIp, guid)
                    If nRes1 > 0 Then
                        res = "1"
                    Else
                        res = "0"
                    End If
                    'res = "vota " & nMin & " - " & res & " - " & Now() & " - " & nRes1
                Else
                    res = "0"
                    'res = "no vota por tiempo " & nMin & " - " & res & " - " & Now()
                End If
            End If

        Catch ex As Exception
            'res = ex.Message
            res = "0"
            _con.Close()

            'Finally

        End Try
        Return res

    End Function

    Public Function InsIpVoto(ByVal stIp As String, ByVal guid As String) As Integer
        'selecciono el sp en función del tipo de consulta
        Dim cmd As New SqlCommand()
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = Con

        cmd.CommandText = "sp_InsIpVoto"
        cmd.Parameters.Add(New System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, 10, 0, Nothing, System.Data.DataRowVersion.Current, False, Nothing, "", "", ""))


        'los parametros son los mismos en ambos casos
        cmd.Parameters.AddWithValue("@ip", stIp)
        cmd.Parameters.AddWithValue("@guid_grupo", guid)
        cmd.Parameters.AddWithValue("@fecha", Now())


        'insertando o actualizando
        Con.Open()
        Dim res As Integer
        Try
            cmd.ExecuteScalar()
            res = cmd.Parameters(0).Value
        Catch ex As Exception
            res = 0
        Finally
            _con.Close()
        End Try
        Return res
    End Function

    Public Function UpdIpVoto(ByVal stIp As String, ByVal guid As String) As Integer
        'selecciono el sp en función del tipo de consulta
        Dim cmd As New SqlCommand()
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = Con

        cmd.CommandText = "sp_UpdIpVoto"
        cmd.Parameters.Add(New System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, 10, 0, Nothing, System.Data.DataRowVersion.Current, False, Nothing, "", "", ""))


        'los parametros son los mismos en ambos casos
        cmd.Parameters.AddWithValue("@ip", stIp)
        cmd.Parameters.AddWithValue("@guid_grupo", guid)
        cmd.Parameters.AddWithValue("@fecha", Now())


        'insertando o actualizando
        Con.Open()
        Dim res As Integer
        Try
            cmd.ExecuteScalar()
            res = cmd.Parameters(0).Value
        Catch ex As Exception
            res = 0
        Finally
            _con.Close()
        End Try
        Return res
    End Function


    Public Function TiempoDiff(ByVal dTimeOn As Date) As Integer

        Dim dTimeOff As Date = Now()
        Dim intTotalTimeWorked As Integer

        Dim strTimeon As String
        Dim strTimeoff As String


        'Format the dates.
        strTimeon = dTimeOn.ToString("hh:mm tt")
        strTimeoff = dTimeOff.ToString("hh:mm tt")
        intTotalTimeWorked = DateDiff(DateInterval.Minute, dTimeOn, dTimeOff)
        Return intTotalTimeWorked
    End Function

#End Region

#Region "ranking"
    Public Function GetRanking(ByVal tipogrupo As Integer) As DataTable
        Dim da As New SqlDataAdapter
        Dim dt As New DataTable
        Dim stSql As String = ""
        stSql = "SELECT ta_ranking.num_votos, ta_conciertos.nom_concierto, " & _
        "ta_conciertos.gls_provincia, ta_conciertos.gls_sala, ta_grupos.fk_tipo_grupo, " & _
        "ta_conciertos.gls_fecha, ta_grupos.nom_grupo, ta_grupos.guid_grupo, " & _
        "ta_blog_grupo.fk_cod_blogger, ta_blogger.gls_blog, " & _
        "ta_blogger.gls_url_blog, ta_blogger.fk_cod_tipoblogger " & _
        "FROM ta_ranking INNER JOIN " & _
        "ta_grupos ON ta_ranking.fk_cod_grupo = ta_grupos.pk_cod_grupo INNER JOIN " & _
        "ta_conciertos ON ta_ranking.fk_cod_concierto = ta_conciertos.pk_cod_concierto INNER JOIN " & _
        "ta_blog_grupo ON ta_blog_grupo.fk_cod_grupo = ta_grupos.pk_cod_grupo INNER JOIN " & _
        "ta_blogger ON ta_blog_grupo.fk_cod_blogger = ta_blogger.fk_newuserId " & _
        "WHERE (ta_conciertos.cod_estado = '1') AND (ta_grupos.cod_estado = '1') " & _
        "AND (ta_ranking.fk_cod_concierto = " & _
        "(SELECT TOP (1) pk_cod_concierto " & _
        "FROM ta_conciertos AS ta_conciertos_1 " & _
        "WHERE(cod_estado = 1) ORDER BY gls_fecha)) " & _
        " AND (ta_ranking.cod_estado = '1') "
        If tipogrupo <> 0 Then
            stSql = stSql & " and (ta_grupos.fk_tipo_grupo = '" & CType(tipogrupo, String) & "') "
        End If


        stSql = stSql & " ORDER BY ta_ranking.num_votos DESC, ta_grupos.fk_tipo_grupo"


        Dim cmd As New SqlCommand(stSql, Con)


        da.SelectCommand = cmd
        da.Fill(dt)
        Return dt
    End Function


    Public Function GetRankingManada(ByVal tipogrupo As Integer) As DataTable
        Dim da As New SqlDataAdapter
        Dim dt As New DataTable
        Dim stSql As String = ""
        stSql = "SELECT ta_ranking.fk_cod_grupo, ta_ranking.num_votos, ta_grupos.fk_tipo_grupo, ta_grupos.guid_grupo " & _
        "FROM ta_ranking INNER JOIN ta_grupos ON ta_ranking.fk_cod_grupo = ta_grupos.pk_cod_grupo "
        If tipogrupo <> 0 Then
            stSql = stSql & " WHERE  (ta_grupos.fk_tipo_grupo = '" & CType(tipogrupo, String) & "') "
        End If

        stSql = stSql & " ORDER BY ta_ranking.num_votos DESC, ta_grupos.fk_tipo_grupo"


        Dim cmd As New SqlCommand(stSql, Con)


        da.SelectCommand = cmd
        da.Fill(dt)
        Return dt
    End Function


#End Region

#Region "Manada"
    Public Function GetManada(ByVal guid As String) As DataTable
        Dim da As New SqlDataAdapter
        Dim dt As New DataTable
        Dim cmd As New SqlCommand("SELECT TOP (100) PERCENT ta_grupos.guid_grupo, ta_grupos.nom_grupo, ta_blog_grupo.fk_cod_blogger, " & _
        "ta_blogger.gls_blog, ta_blogger.gls_url_blog, ta_blogger.fk_cod_tipoblogger " & _
        "FROM ta_blog_grupo INNER JOIN " & _
        "ta_grupos ON ta_blog_grupo.fk_cod_grupo = ta_grupos.pk_cod_grupo INNER JOIN " & _
        "ta_blogger ON ta_blog_grupo.fk_cod_blogger = ta_blogger.fk_newuserId " & _
        "WHERE (ta_grupos.guid_grupo = @guid) " & _
        "ORDER BY ta_blogger.fk_cod_tipoblogger", Con)
        cmd.Parameters.AddWithValue("@guid", guid)

        da.SelectCommand = cmd
        da.Fill(dt)
        Return dt
    End Function
#End Region

#Region "grupos"
    Public Function UpdateGrupo(ByVal guid_grupo As String, ByVal nom_grupo As String, ByVal fk_tipo_grupo As Integer) As Integer
        'selecciono el sp en función del tipo de consulta
        Dim cmd As New SqlCommand()
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = Con

        cmd.CommandText = "sp_UpdTipoGrupo"
        cmd.Parameters.Add(New System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, 10, 0, Nothing, System.Data.DataRowVersion.Current, False, Nothing, "", "", ""))


        'los parametros son los mismos en ambos casos
        cmd.Parameters.AddWithValue("@guid_grupo", guid_grupo)
        cmd.Parameters.AddWithValue("@nom_grupo", nom_grupo)
        cmd.Parameters.AddWithValue("@fk_tipo_grupo", fk_tipo_grupo)




        'insertando o actualizando
        Con.Open()
        Dim res As Integer
        Try
            cmd.ExecuteScalar()
            res = cmd.Parameters(0).Value
        Catch ex As Exception
            res = 0
        Finally
            _con.Close()
        End Try
        Return res
    End Function
#End Region

#Region "comprobar"
    Public Function GetExisteGrupo(ByVal nomgrupo As String) As String
        Dim da As New SqlDataAdapter
        Dim res As String = ""
        Dim cmd As New SqlCommand("SELECT nom_grupo from ta_grupos where nom_grupo = @nomgrupo", Con)
       
        cmd.Parameters.AddWithValue("@nomgrupo", nomgrupo)

        Con.Open()

        Try
            res = CType(cmd.ExecuteScalar(), String)

        Catch ex As Exception
            res = ""
        Finally
            _con.Close()
        End Try
        Return res

    End Function


    Public Function GetMiembrosGrupoByGuid(ByVal guidgrupo As String) As Integer
        Dim da As New SqlDataAdapter
        Dim res As String = ""
        Dim cmd As New SqlCommand("SELECT COUNT(ta_blog_grupo.fk_cod_grupo) AS Expr1 FROM ta_blog_grupo INNER JOIN " & _
                      "ta_grupos ON ta_blog_grupo.fk_cod_grupo = ta_grupos.pk_cod_grupo " & _
                    "WHERE(ta_grupos.guid_grupo = @guid_grupo)", Con)

        cmd.Parameters.AddWithValue("@guid_grupo", guidgrupo)

        Con.Open()

        Try
            res = CType(cmd.ExecuteScalar(), Integer)

        Catch ex As Exception
            res = ""
        Finally
            _con.Close()
        End Try
        Return res

    End Function

    Public Function GetExisteGrupoByGuid(ByVal guidgrupo As String) As Integer
        Dim da As New SqlDataAdapter
        Dim res As String = ""
        Dim cmd As New SqlCommand("SELECT pk_cod_grupo from ta_grupos where guid_grupo = @guid_grupo", Con)

        cmd.Parameters.AddWithValue("@guid_grupo", guidgrupo)

        Con.Open()

        Try
            res = CType(cmd.ExecuteScalar(), Integer)

        Catch ex As Exception
            res = ""
        Finally
            _con.Close()
        End Try
        Return res

    End Function

    Public Function GetTipoGrupoByGuid(ByVal guidgrupo As String) As Integer
        Dim da As New SqlDataAdapter
        Dim res As String = ""
        Dim cmd As New SqlCommand("SELECT fk_tipo_grupo from ta_grupos where guid_grupo = @guid_grupo", Con)

        cmd.Parameters.AddWithValue("@guid_grupo", guidgrupo)

        Con.Open()

        Try
            res = CType(cmd.ExecuteScalar(), Integer)

        Catch ex As Exception
            res = ""
        Finally
            _con.Close()
        End Try
        Return res

    End Function

    Public Function GetPropietarioGrupoByGuid(ByVal guidgrupo As String) As Integer
        Dim da As New SqlDataAdapter
        Dim res As String = ""
        Dim cmd As New SqlCommand("SELECT fk_cod_blogger from ta_grupos where guid_grupo = @guid_grupo", Con)

        cmd.Parameters.AddWithValue("@guid_grupo", guidgrupo)

        Con.Open()

        Try
            res = CType(cmd.ExecuteScalar(), Integer)

        Catch ex As Exception
            res = ""
        Finally
            _con.Close()
        End Try
        Return res

    End Function

    Public Function GetExisteBlog(ByVal urlblog As String) As String
        Dim da As New SqlDataAdapter
        Dim res As String = ""
        Dim cmd As New SqlCommand("SELECT gls_url_blog from ta_blogger where gls_url_blog = @gls_url_blog", Con)

        cmd.Parameters.AddWithValue("@gls_url_blog", urlblog)

        Con.Open()

        Try
            res = CType(cmd.ExecuteScalar(), String)

        Catch ex As Exception
            res = ""
        Finally
            _con.Close()
        End Try
        Return res

    End Function
#End Region

#Region "concierto"
    Public Function InsImagenes(ByVal gls_imagen As String, ByVal cod_estado As String) As Integer
        'selecciono el sp en función del tipo de consulta
        Dim cmd As New SqlCommand()
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = Con

        cmd.CommandText = "sp_InsImagenes"
        cmd.Parameters.Add(New System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, 10, 0, Nothing, System.Data.DataRowVersion.Current, False, Nothing, "", "", ""))


        'los parametros son los mismos en ambos casos
        cmd.Parameters.AddWithValue("@gls_imagen", gls_imagen)
        cmd.Parameters.AddWithValue("@cod_estado", cod_estado)


        'insertando o actualizando
        Con.Open()
        Dim res As Integer
        Try
            cmd.ExecuteScalar()
            res = cmd.Parameters(0).Value
        Catch ex As Exception
            res = 0
        Finally
            _con.Close()
        End Try
        Return res
    End Function
#Region "imagenes"
    Public Function InsTexto(ByVal gls_texto As String, ByVal cod_estado As String) As Integer
        'selecciono el sp en función del tipo de consulta
        Dim cmd As New SqlCommand()
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = Con

        cmd.CommandText = "sp_InsTexto"
        cmd.Parameters.Add(New System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, 10, 0, Nothing, System.Data.DataRowVersion.Current, False, Nothing, "", "", ""))


        'los parametros son los mismos en ambos casos
        cmd.Parameters.AddWithValue("@gls_texto", gls_texto)
        cmd.Parameters.AddWithValue("@cod_estado", cod_estado)


        'insertando o actualizando
        Con.Open()
        Dim res As Integer
        Try
            cmd.ExecuteScalar()
            res = cmd.Parameters(0).Value
        Catch ex As Exception
            res = 0
        Finally
            _con.Close()
        End Try
        Return res
    End Function
    Public Function InsChat(ByVal gls_texto As String, ByVal cod_estado As String) As Integer
        'selecciono el sp en función del tipo de consulta
        Dim cmd As New SqlCommand()
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = Con

        cmd.CommandText = "sp_InsChat"
        cmd.Parameters.Add(New System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, 10, 0, Nothing, System.Data.DataRowVersion.Current, False, Nothing, "", "", ""))


        'los parametros son los mismos en ambos casos
        cmd.Parameters.AddWithValue("@gls_texto", gls_texto)
        cmd.Parameters.AddWithValue("@cod_estado", cod_estado)


        'insertando o actualizando
        Con.Open()
        Dim res As Integer
        Try
            cmd.ExecuteScalar()
            res = cmd.Parameters(0).Value
        Catch ex As Exception
            res = 0
        Finally
            _con.Close()
        End Try
        Return res
    End Function
#End Region
#End Region

#Region "encuesta"
    Public Function ManejaEncuesta(ByVal id_encuesta As String, ByVal nVoto As Integer) As String
        Dim da As New SqlDataAdapter
        Dim res As String = ""
        Dim nMin As Integer = 0
        Dim nRes1 As Integer = 0
        Dim cmd As New SqlCommand("SELECT res" & CType(nVoto, String) & " from ta_encuesta where id_encuesta = @id_encuesta", Con)

        cmd.Parameters.AddWithValue("@id_encuesta", id_encuesta)


        Con.Open()

        Try
            res = cmd.ExecuteScalar()
            _con.Close()

            If Trim("" & res) = "" Then
                nRes1 = InsEncuesta(id_encuesta, nVoto)

                If nRes1 > 0 Then
                    res = "1"
                Else
                    res = "0"
                End If
                'res = "no hay" & nRes1
            Else

                nRes1 = UpdEncuesta(id_encuesta, nVoto)
                If nRes1 > 0 Then
                    res = "1"
                Else
                    res = "0"
                End If
                'res = "vota " & nMin & " - " & res & " - " & Now() & " - " & nRes1

            End If

        Catch ex As Exception
            'res = ex.Message
            res = "0"
            _con.Close()

            'Finally

        End Try
        Return res

    End Function

    Public Function InsEncuesta(ByVal id_encuesta As String, ByVal nVoto As Integer) As Integer
        'selecciono el sp en función del tipo de consulta

        Dim res1 As Integer = 0
        Dim res2 As Integer = 0
        Dim res3 As Integer = 0
        Dim res4 As Integer = 0
        Dim res5 As Integer = 0
        Dim res6 As Integer = 0
        Dim res7 As Integer = 0
        Dim res8 As Integer = 0

        Select Case nVoto
            Case 1
                res1 = 1
            Case 2
                res2 = 1
            Case 3
                res3 = 1
            Case 4
                res4 = 1
            Case 5
                res5 = 1
            Case 6
                res6 = 1
            Case 7
                res7 = 1
            Case 8
                res8 = 1
        End Select



        Dim cmd As New SqlCommand()
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = Con


        cmd.CommandText = "sp_InsEncuesta"
        cmd.Parameters.Add(New System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, 10, 0, Nothing, System.Data.DataRowVersion.Current, False, Nothing, "", "", ""))


        'los parametros son los mismos en ambos casos
        cmd.Parameters.AddWithValue("@id_encuesta", id_encuesta)
        cmd.Parameters.AddWithValue("@res1", res1)
        cmd.Parameters.AddWithValue("@res2", res2)
        cmd.Parameters.AddWithValue("@res3", res3)
        cmd.Parameters.AddWithValue("@res4", res4)
        cmd.Parameters.AddWithValue("@res5", res5)
        cmd.Parameters.AddWithValue("@res6", res6)
        cmd.Parameters.AddWithValue("@res7", res7)
        cmd.Parameters.AddWithValue("@res8", res8)



        'insertando o actualizando
        Con.Open()
        Dim res As Integer
        Try
            cmd.ExecuteScalar()
            res = cmd.Parameters(0).Value
        Catch ex As Exception
            res = 0
        Finally
            _con.Close()
        End Try
        Return res
    End Function

    Public Function UpdEncuesta(ByVal id_encuesta As String, ByVal nVoto As Integer) As Integer
        'selecciono el sp en función del tipo de consulta
        Dim cmd As New SqlCommand()
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = Con

        cmd.CommandText = "sp_UpdEncuesta"
        cmd.Parameters.Add(New System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, 10, 0, Nothing, System.Data.DataRowVersion.Current, False, Nothing, "", "", ""))


        'los parametros son los mismos en ambos casos
        cmd.Parameters.AddWithValue("@id_encuesta", id_encuesta)
        cmd.Parameters.AddWithValue("@res", nVoto)


        'insertando o actualizando
        Con.Open()
        Dim res As Integer
        Try
            cmd.ExecuteScalar()
            res = cmd.Parameters(0).Value
        Catch ex As Exception
            res = 0
        Finally
            _con.Close()
        End Try
        Return res
    End Function


    Public Function GetEncuestaData(ByVal id_encuesta As String) As DataTable
        Dim da As New SqlDataAdapter
        Dim dt As New DataTable
        Dim stSql As String = ""
        stSql = "SELECT res1,res2,res3,res4,res5,res6,res7,res8 " & _
        "FROM ta_encuesta  WHERE  (id_encuesta = '" & id_encuesta & "') "
        

        Dim cmd As New SqlCommand(stSql, Con)


        da.SelectCommand = cmd
        da.Fill(dt)
        Return dt
    End Function
#End Region

End Class
