<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="report_19c.aspx.cs" Inherits="CallBaseMock.partials.report_19c" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link rel="stylesheet" href="../styles/popout.css" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <h2 class="textCenter">
        <asp:Label ID="lblRepHeader" runat="server" Text=""></asp:Label></h2> <%--Need to create label--%>
    <h3 class="textCenter">
        <asp:Label ID="lblReportTitle" runat="server" Text=""></asp:Label></h3> <%--Need to create label--%>
    <br />
    <div class="floatLeft">
        <span class="reportText">
            <asp:Label ID="lblPrintedBy" runat="server" Text=""></asp:Label> </span>
        <asp:Label ID="lblUser" runat="server" Text="" CssClass="reportText"></asp:Label>
    </div>
    <asp:Label ID="lblDate" runat="server" Text="" CssClass="reportText floatRight"></asp:Label>
    <br />
    <div runat="server" id="hasRowsDiv">
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
                        <span class="reportText fontBold">
                            <asp:Label ID="lblDateRangeText" runat="server" Text=""></asp:Label> </span>
                    </td>
                    <td>
                        <asp:Label ID="lblDateRange" runat="server" Text="" CssClass="reportText"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <span class="reportText fontBold">
                            <asp:Label ID="lblCampaigns" runat="server" Text=""></asp:Label> </span>
                    </td>
                    <td>
                        <asp:Label ID="lblCampaign" runat="server" Text="" CssClass="reportText"></asp:Label>
                    </td>
                </tr>
                <tr id="filterRow" runat="server">
                    <td>
                        <asp:Label ID="lblFilter" runat="server" Text="" CssClass="reportText fontBold"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblVals" runat="server" Text="" CssClass="reportText"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <span class="reportText fontBold">
                            <asp:Label ID="lblOrderStatuses" runat="server" Text=""></asp:Label> </span>
                    </td>
                    <td>
                        <asp:Label ID="lblSelStatus" runat="server" Text="" CssClass="reportText"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <span class="reportText fontBold">
                            <asp:Label ID="lblGroups" runat="server" Text=""></asp:Label> </span>
                    </td>
                    <td>
                        <asp:Label ID="lblSelGroup" runat="server" Text="" CssClass="reportText"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <hr />
        <table class="statusReportTable">
            <tr>
                <td>
                    <span class="reportText fontBold"><asp:Label ID="lblOpenTicketsText" runat="server" Text=""></asp:Label></span>
                </td>
                <td>
                    <asp:Label ID="lblOpenTickets" runat="server" Text="" CssClass="reportText"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <span class="reportText fontBold"><asp:Label ID="lblDoneTicketsText" runat="server" Text=""></asp:Label> &nbsp;</span><%--Need to create label--%></td>
                <td>
                    <asp:Label ID="lblDoneTickets" runat="server" Text="" CssClass="reportText"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <span class="reportText fontBold"><asp:Label ID="lblCompTimeText" runat="server" Text=""></asp:Label> &nbsp;</span><%--Need to create label--%></td>
                <td>
                    <asp:Label ID="lblCompTime" runat="server" Text="" CssClass="reportText"></asp:Label>
                    <span class="reportText"><asp:Label ID="lbl24Clock" runat="server" Text=""></asp:Label></span>
                </td>
            </tr>
        </table>
        <hr />
        <asp:Table ID="tblReport" runat="server" CssClass="statusReportTable blackText">
            <asp:TableHeaderRow CssClass="fontBold">
                <asp:TableCell>Assigned To <br />
                /Edit Agent</asp:TableCell>
                <asp:TableCell>Original <br />
                Agent</asp:TableCell>
                <asp:TableCell>Ticket Number</asp:TableCell>
                <asp:TableCell>Date Input</asp:TableCell>
                <asp:TableCell>Current Status</asp:TableCell>
                <asp:TableCell>Date Assigned</asp:TableCell> <%--Need to create label--%>
                <asp:TableCell>Date to Call <br /> <%--Need to create label--%>
                Client Back</asp:TableCell>
                <asp:TableCell>Date Completed</asp:TableCell> <%--Need to create label--%>
                <asp:TableCell>Total Hours</asp:TableCell> <%--Need to create label--%>
                <asp:TableCell>Total Days</asp:TableCell> <%--Need to create label--%>
            </asp:TableHeaderRow> 
        </asp:Table>
    </div>
    <br />
     <asp:Label ID="lblReportNum" runat="server" CssClass="floatRight"></asp:Label>
    </form>
</body>
</html>
