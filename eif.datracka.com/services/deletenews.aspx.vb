Imports System.Configuration.ConfigurationManager
Imports System.Guid
Imports System.Data
Imports System.IO
Imports System.Xml

Partial Class services_deletenews
    Inherits System.Web.UI.Page
    ' genéricos
    Dim dm As New dms
    Dim fn As New funciones


    ' Datos del blogger
    Dim stEmail As String = ""
    Dim stPassword As String = ""
    Dim stFirstname As String = ""
    Dim stLastname As String = ""
    Dim stMiddlename As String = ""
    Dim dDateOfBirth As New Date
    Dim stCity As String = ""


    ' inicializamos el objeto de datos
    Dim intRespuesta As Integer = 0


    Dim stRespuestaDMS As String = ""


    Dim stError As String = ""


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load



        If Trim("" & Request.Form("email")) <> "" And Trim("" & Request.Form("DateOfBirth")) <> "" Then
            ' Cargo variables que vienen del formulario
            InicializarVariables()

            ' ACTUALIZAMOS EL BLOGGER
            ' guardamos en la base de datos el blogger principal
            ' es de tipo blogger 1
            ' tiene el estado 2, pendiente de validación de opt-in
            ' es un tipo de consulta INSERT
            ' no tiene pk
            ' no tiene consumerID del DMS, ponemos a 0
            ' fecha de alta es la fecha actual
            Dim b As New blogger

            b.Email = stEmail
            b.DateOfBirth = dDateOfBirth




            intRespuesta = SubLeeXML(UpdBloggerDms(b))
            ' SI HEMOS RECIBIDO RESPUESTA DEL ALTA CORRECTA DEL BLOGGER

            If intRespuesta = 0 Then


                stError = stRespuestaDMS

            End If
        Else
            stError = "Se debe indicar email y fecha de nacimiento"
        End If





        ' Generamos el Mensaje

        Response.Write("<salida><estado>" & intRespuesta & "</estado><error>" & stError & "</error>")


        Response.Write("<datos></datos></salida>")





    End Sub





    Sub InicializarVariables()

        ' datos generales del blogger
        stEmail = SaneadorSQL.Sanea("" & Request.Form("email"), 200)
        dDateOfBirth = CDate("" & Request.Form("dateofbirth").ToString())



    End Sub



#Region "webservices"
    Function UpdBloggerDms(ByVal bb As blogger) As String
        Dim stXML As String = ""
        Dim stRespuesta As String = ""

        ' montamos la cadena del xml
        stXML = stXML & "<profile>"

        stXML = stXML & "<Email>" & bb.Email & "</Email>"
        stXML = stXML & "<DateOfBirth>" & fn.TimeCodeMaker(CType(bb.DateOfBirth, Date)) & "</DateOfBirth>"
        stXML = stXML & "<EmailOptIn>false</EmailOptIn>"
        stXML = stXML & "</profile>"


        stRespuesta = dm.FunEnviaXML(stXML, "UpdateConsumerProfile")
        stRespuestaDMS = stXML & stRespuesta
        Return (stRespuesta)

    End Function

    Function SubLeeXML(ByVal prXML As String) As Integer
        Dim vrXml As System.IO.StringReader = New System.IO.StringReader(prXML)
        Dim PaginaXML As New XmlTextReader(vrXml)
        Dim EtiquetaActual As String = ""
        Dim nResultado As Integer = 0
        Dim stBolResult As String = ""

        Do While (PaginaXML.Read())


            Select Case PaginaXML.NodeType

                ' if Element, display its name
                Case XmlNodeType.Element
                    EtiquetaActual = PaginaXML.Name

                Case XmlNodeType.Text ' if Text, display it
                    If EtiquetaActual = "SuccessFlag" Then stBolResult = PaginaXML.Value
                    If EtiquetaActual = "ResponseMessage" Then stRespuestaDMS = PaginaXML.Value
            End Select


        Loop



        If stBolResult = "true" Then
            Return 1
        Else
            Return 0
        End If
    End Function





#End Region

End Class
