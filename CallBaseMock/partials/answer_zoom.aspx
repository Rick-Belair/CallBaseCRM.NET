<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="answer_zoom.aspx.cs" Inherits="CallBaseMock.partials.answer_zoom" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Answer</title>
    <link rel="stylesheet" href="../styles/master.css" />
    <link rel="stylesheet" href="../styles/popout.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="commentDetails">
        <label>
            Issues/Questions:</label>
    </div>
    <asp:TextBox ID="txtIssues" CssClass="textareaStyle" runat="server" Width="700" Height="320"
        TextMode="MultiLine" />
        <br />
        <br />
    <div class="commentDetails">
        <label>
            Answer:</label>
    </div>
    <asp:TextBox ID="txtAnswer" CssClass="textareaStyle" runat="server" Width="700" Height="320"
        TextMode="MultiLine" />
    </form>
</body>
</html>
