
Partial Class services_captcha
    Inherits System.Web.UI.Page

    Protected Sub Page_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.PreRender

        ' initial page setup
        If (Not IsPostBack) Then

            'set control text
            ValidateButton.Text = "Validate"
            MessageCorrectLabel.Text = "Correct!"
            MessageIncorrectLabel.Text = "Incorrect!"

            'these messages are shown only after validation
            MessageCorrectLabel.Visible = False
            MessageIncorrectLabel.Visible = False
        End If

        CodeTextBox.Attributes.Add("onkeyup", "this.value = this.value.toLowerCase();")

        If (IsPostBack) Then
            'validate the input code, and show the appropriate message 
            Dim code As String = CodeTextBox.Text.Trim().ToUpper()

            If (SampleCaptcha.Validate(code)) Then
                MessageCorrectLabel.Visible = True
                MessageIncorrectLabel.Visible = False
            Else
                MessageCorrectLabel.Visible = False
                MessageIncorrectLabel.Visible = True
            End If

            'clear previous user code input
            CodeTextBox.Text = ""
        End If

    End Sub

   
End Class
