<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="issue_history.aspx.cs" Inherits="CallBaseMock.partials.issue_history" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Issue History</title>
    <link rel="stylesheet" href="../styles/master.css" />
    <link rel="stylesheet" href="../styles/popout.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="commentDetails">
    <label>Current Comment Details:</label> 
        <label><asp:Label ID="lblCommentDate" runat="server" Text="Comment Date"></asp:Label></label>
    
    </div>
    <asp:TextBox ID="txtCurrentComment" CssClass="textareaStyle" runat="server" Width="450" Height="120"
                        TextMode="MultiLine" Enabled="false" />

    <div class="commentDetails">
    <label>Comment History for Ticket #
        <asp:Label ID="lblTicket" runat="server" Text="Placehold"></asp:Label> Click on line to see more details.</label>
    </div>
    <div class="commentTableDiv">

        <asp:ObjectDataSource ID="odsCommentHistory" runat="server" 
            SelectMethod="GetCommentHistory" TypeName="DataAccess.InboundDB">
            <SelectParameters>
                <asp:SessionParameter Name="ticketnumber" SessionField="TicketNumber" 
                    Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:GridView ID="gvCommentHistory" runat="server" AutoGenerateColumns="False" 
            DataSourceID="odsCommentHistory" 
            onselectedindexchanged="gvCommentHistory_SelectedIndexChanged" 
            EmptyDataText="No comment history" SelectedRowStyle-BackColor="LightCyan">
            <Columns>
                <asp:CommandField ShowSelectButton="True" />
                <asp:BoundField  HeaderStyle-HorizontalAlign="Left" DataField="cah_date_edit" 
                    HeaderText="Date">
<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField HeaderStyle-HorizontalAlign="Left" DataField="cah_user_edit" 
                    HeaderText="User">
<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField HeaderStyle-HorizontalAlign="Left" DataField="cah_comments" 
                    HeaderText="Comment">
<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:BoundField>
            </Columns>
        </asp:GridView>
    
    </div>
    <div class="commentDetails">
    <label>Comment Details (from selected history line)</label>
    </div>
    <asp:TextBox ID="txtSelComment" CssClass="textareaStyle" runat="server" Width="450" Height="120"
                        TextMode="MultiLine" Enabled="false" />
    </form>
</body>
</html>
