<%@ Page Language="VB" AutoEventWireup="false" CodeFile="captcha.aspx.vb" Inherits="services_captcha" %>

<%@ Register Assembly="Lanap.BotDetect" Namespace="Lanap.BotDetect" TagPrefix="BotDetect" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Página sin título</title>
       <link type='text/css' rel='Stylesheet' href='StyleSheet.css' />
</head>
<body>
    <form id="form1" runat="server">
    <fieldset id="Preview">
        <legend><span id="PreviewLegend">CAPTCHA Preview</span></legend>
        <div id="PromptDiv">
            <span id="Prompt">Type the characters you see in the picture</span>
        </div>
        <div id="CaptchaDiv">
            <BotDetect:Captcha ID="SampleCaptcha" runat="server" />
        </div>
        <div id="ValidationDiv">
            <asp:TextBox ID="CodeTextBox" runat="server"></asp:TextBox>
            <asp:Button ID="ValidateButton" runat="server" />
            <asp:Label ID="MessageCorrectLabel" runat="server"></asp:Label>
            <asp:Label ID="MessageIncorrectLabel" runat="server"></asp:Label>
        </div>
    </fieldset>
    <div id="Note">
        <span>NOTE: the Trial version will use "LANAP" instead of a random code in 50% of renderings.</span>
    </div>
    </form>
</body>
</html>
