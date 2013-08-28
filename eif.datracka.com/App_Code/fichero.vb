Imports Microsoft.VisualBasic
Imports System
Imports System.IO

Public Class fichero
#Region "constructor"

    Public Sub New()
        '
    End Sub

#End Region
#Region "funciones"
    Public Function Lee(ByVal nomfile As String) As String
        Dim fileName As String
        fileName = nomfile
        Try
            Dim stream As StreamReader
            stream = New StreamReader(fileName)
            Lee = CType(stream.ReadToEnd(), String)
        Catch exceptionCatch As IOException
            Lee = ""
        End Try
    End Function
#End Region
End Class
