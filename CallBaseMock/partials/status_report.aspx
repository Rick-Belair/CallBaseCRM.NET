<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="status_report.aspx.cs"
    Inherits="CallBaseMock.partials.status_report" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" href="../styles/popout.css" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <h2 class="textCenter">
        <asp:Label ID="lblRepHeader" runat="server" Text="Label"></asp:Label></h2>
    <h3 class="textCenter">
        <asp:Label ID="lblStatusHeader" runat="server" Text=""></asp:Label></h3>
    <br />
    <div>
        <h3 class="floatLeft">
            <asp:Label ID="lblReportTitle" runat="server" Text=""></asp:Label></h3>
        <div class="floatRight">
            <asp:Label ID="lblDate" runat="server" Text="" CssClass="reportText"></asp:Label>
            <br />
            <asp:Label ID="lblUser" runat="server" Text="" CssClass="reportText"></asp:Label>
        </div>
    </div>
    <br />
    <div runat="server" id="hasRowsDiv">
        <br />
        <hr />
        <div>
            <span class="floatRight fontItalic reportText">
                <asp:Label ID="lblProtectedData" runat="server" Text=""></asp:Label></span>
            <table class="statusReportTable">
                <tr>
                    <td>
                        <span class="reportText fontBold">
                            <asp:Label ID="lblCurrUser" runat="server" Text=""></asp:Label> </span>
                    </td>
                    <td>
                        <asp:Label ID="lblUserName" runat="server" Text="" CssClass="reportText"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblCriteria" runat="server" Text="" CssClass="reportText fontBold"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblVals" runat="server" Text="" CssClass="reportText"></asp:Label>
                    </td>
                </tr>
                <tr id="groupRow" runat="server">
                    <td>
                        <span class="reportText fontBold">
                            <asp:Label ID="lblGroupText" runat="server" Text=""></asp:Label> </span>
                    </td>
                    <td>
                        <asp:Label ID="lblGroup" runat="server" Text="" CssClass="reportText"></asp:Label>
                    </td>
                </tr>
                <tr id="statusRow" runat="server">
                    <td>
                        <span class="reportText fontBold">
                            <asp:Label ID="lblStatusText" runat="server" Text=""></asp:Label> </span>
                    </td>
                    <td>
                        <asp:Label ID="lblStatus" runat="server" Text="" CssClass="reportText"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <hr />
        <asp:Table ID="tblOrderStatus" runat="server" CssClass="statusReportTable alignLeft">
        </asp:Table>
        <br />
        <div class="textCenter greyBack">
            <asp:Label ID="lblNumRows" runat="server" Text="" CssClass="reportText"></asp:Label></div>
        <br />
        <div>
            <asp:Label ID="lblDate2" runat="server" Text="" CssClass="reportText floatLeft"></asp:Label>
            <span class="floatRight fontItalic reportText">
                <asp:Label ID="lblProtectedData2" runat="server" Text=""></asp:Label></span>
        </div>
    </div>
    <div runat="server" id="noRowsDiv" visible="false">
        <asp:Label ID="lblNoRows" runat="server" Text="No records" CssClass="reportText"></asp:Label>
    </div>
    </form>
</body>
</html>
