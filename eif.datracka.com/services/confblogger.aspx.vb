Imports System.Configuration.ConfigurationManager


Partial Class confblogger
    Inherits System.Web.UI.Page
    Dim dm As New dms


    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        ' aquí el alta...
        Dim nBlogger As Integer = 0
        nBlogger = CType("0" & Request.QueryString("id"), Integer)

        ' si viene el código del blogger, comenzamos el proceso
        If nBlogger <> 0 Then
            Label1.Text = "Actualizando el usuario " & nBlogger
            'consigo datos 
            Dim b As New blogger(CType(nBlogger, Integer))
            Label1.Text = AltaBloggerDms(b)

        Else
            Label1.Text = "ERROR: No se han indicado datos de usuario"
        End If

    End Sub



    Function AltaBloggerDms(ByVal bb As blogger) As String
        Dim stXML As String = ""
        Dim stRespuesta As String = ""

        ' montamos la cadena del xml
        stXML = stXML & "<profile>"
        stXML = stXML & "<FirstName>" & bb.Firstname & "</FirstName>"
        stXML = stXML & "<LastName>" & bb.Lastname & "</LastName>"
        stXML = stXML & "<MiddleName>" & bb.Middlename & "</MiddleName>"
        stXML = stXML & "<Email>" & bb.Email & "</Email>"
        stXML = stXML & "<DateOfBirth>" & TimeCodeMaker(CType(bb.DateOfBirth, Date)) & "</DateOfBirth>"
        stXML = stXML & "<City>" & bb.City & "</City>"
        stXML = stXML & "<Password>" & bb.Password & "</Password>"
        stXML = stXML & "<EmailOptIn>false</EmailOptIn>"
        stXML = stXML & "<isFkock>false</isFkock>"
        stXML = stXML & "<CustomID>88</CustomID>"
        stXML = stXML & "<BlogName>1</BlogName>"
        stXML = stXML & "<BlogUrl>1</BlogUrl>"
        stXML = stXML & "</profile>"
        stXML = stXML & "<sourceCampaignID>88</sourceCampaignID>"
        stXML = stXML & "<sendEmailToConsumer>false</sendEmailToConsumer>"
        stXML = stXML & "<source>ExternalOnlineSource</source>"

        stRespuesta = dm.FunEnviaXML(stXML, "AddConsumerProfile")

        Return (stRespuesta)

    End Function

    Function TimeCodeMaker(ByVal Time As DateTime) As String
        Return Format(Time.Year Mod 10000, "0000") + "-" + Format(Time.Month, "00") + "-" + Format(Time.Day, "00")
    End Function

    
End Class
