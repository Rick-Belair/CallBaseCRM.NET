<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="order_history.aspx.cs" Inherits="CallBaseMock.partials.order_history" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Order History</title>
      <link rel="stylesheet" href="../styles/master.css" />
      <link rel="stylesheet" href="../styles/popout.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="orderTableDiv">
        <asp:Table ID="tblOrderHistory" runat="server">
        <asp:TableHeaderRow>
        <asp:TableHeaderCell>Order Status</asp:TableHeaderCell>
        <asp:TableHeaderCell>Date</asp:TableHeaderCell>
        <asp:TableHeaderCell>User</asp:TableHeaderCell>
        </asp:TableHeaderRow>
        </asp:Table>
    </div>
    </form>
</body>
</html>
