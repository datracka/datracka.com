Imports System.Configuration.ConfigurationManager
Imports System.Guid
Imports System.Data
Imports System.IO
Imports System.Xml

Partial Class updblogger
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
    Dim da As New datos
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
            b.pk_cod_blogger = 0
            b.fk_cod_tipo_blogger = 1
            b.fk_newuserId = 0
            b.Email = stEmail
            b.Password = stPassword

            b.Firstname = stFirstname
            b.Lastname = stLastname
            b.Middlename = stMiddlename
            b.DateOfBirth = dDateOfBirth
            b.City = stCity
            b.Fecha_Alta = Date.Now()
            b.Fecha_Mod = Date.Now()



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
        stPassword = SaneadorSQL.Sanea("" & Request.Form("password"), 20)
        stFirstname = SaneadorSQL.Sanea("" & Request.Form("firstname"), 50)
        stLastname = SaneadorSQL.Sanea("" & Request.Form("lastname"), 50)
        stMiddlename = SaneadorSQL.Sanea("" & Request.Form("middlename"), 50)
        dDateOfBirth = CDate("" & Request.Form("dateofbirth").ToString())
        stCity = SaneadorSQL.Sanea("" & Request.Form("city"), 200)



    End Sub



#Region "webservices"
    Function UpdBloggerDms(ByVal bb As blogger) As String
        Dim stXML As String = ""
        Dim stRespuesta As String = ""

        ' montamos la cadena del xml
        stXML = stXML & "<profile>"
        stXML = stXML & "<FirstName>" & bb.Firstname & "</FirstName>"
        stXML = stXML & "<LastName>" & bb.Lastname & "</LastName>"
        stXML = stXML & "<MiddleName>" & bb.Middlename & "</MiddleName>"
        stXML = stXML & "<Email>" & bb.Email & "</Email>"
        stXML = stXML & "<DateOfBirth>" & fn.TimeCodeMaker(CType(bb.DateOfBirth, Date)) & "</DateOfBirth>"
        stXML = stXML & "<City>" & bb.City & "</City>"
        stXML = stXML & "<Password>" & bb.Password & "</Password>"
        'stXML = stXML & "<EmailOptIn>true</EmailOptIn>"
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
