

Public Class SaneadorSQL

    Public Shared Function Sanea(ByVal par As String, ByVal longitud As Integer) As String
        par = par.Trim(" ")
        par = par.Replace("'", "''")
        par = Mid(par, 1, longitud)
        Return par
    End Function


End Class

