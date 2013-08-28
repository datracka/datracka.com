Imports Microsoft.VisualBasic

Public Class funciones
    Public Function TimeCodeMaker(ByVal Time As DateTime) As String
        Return Format(Time.Year Mod 10000, "0000") + "-" + Format(Time.Month, "00") + "-" + Format(Time.Day, "00")
    End Function
End Class
