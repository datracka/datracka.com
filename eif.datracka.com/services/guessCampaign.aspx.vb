Imports System.Configuration.ConfigurationManager
Imports System.Data
Imports System.IO
Imports System.Xml
Imports System.Web
Imports System.Net

Partial Class services_guessCampaign
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lbl1.Text = "siteID: " & AppSettings("siteID")
        lbl2.Text = "CampaignID: " & AppSettings("campaingID")
        lbl3.Text = "campaingIDNews: " & AppSettings("campaingIDNews")
        lbl4.Text = "campaingIDConcurso: " & AppSettings("campaingIDConcurso")
    End Sub

End Class
