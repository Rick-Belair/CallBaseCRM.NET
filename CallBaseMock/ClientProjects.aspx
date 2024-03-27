<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClientProjects.aspx.cs" Inherits="CallBaseMock.ClientProjects" 
MasterPageFile="~/Site1.Master" Title="Contact Projects"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="styles/projects.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContent" runat="server">
    <div class="inboundcontent" onkeydown="javascript:EnterKeyFilter();">
        <div class="inboundtop" id="topDiv" runat="server">
            <span class="greybold headerText"><asp:Label ID="lblAANDCHeader" runat="server" Text="Public Enquiries AANDC"></asp:Label></span>
            <div class="menulinks extraMargin">
                <!--img src="images/b_arrow2.gif" alt="arrow image" />&nbsp;<a href="#">Help</a-->
                <img src="images/b_arrow2.gif" alt="arrow image" />&nbsp;<asp:LinkButton ID="lnkLanguage"
                    runat="server" OnClick="lnkLanguage_Click" />
                <img src="images/b_arrow2.gif" alt="arrow image" />&nbsp;<asp:LinkButton ID="lnkLogOut"
                    runat="server" onclick="lnkLogOut_Click">Logout</asp:LinkButton>
                <!--img src="images/b_arrow2.gif" alt="arrow image" />&nbsp;<a href="#">Home</a-->
            </div>
            <div class="headerbuttons">
                <asp:Button ID="btnContactManagement" CssClass="btn menuBtn2 inboundLink" runat="server" Text="CONTACT MANAGEMENT"
                     OnClick="GoToContactManagement"/><asp:Button ID="btnInventory" CssClass="btn menuBtn1 knowledgeLink"
                        runat="server" Text="INVENTORY" /><asp:Button ID="btnReports"
                            CssClass="btn menuBtn1 productLink" runat="server" Text="REPORTS" /><asp:Button
                                ID="btnMaintenance" CssClass="btn menuBtn1 orderLink" runat="server" Text="MAINTENANCE"
                                 />
            </div>
            <asp:Button ID="btnClose1" runat="server" Text="Close" CssClass="actionbutton smallbluebtn closeBtn"
                Visible="false"  />
        </div>      
        
        <!--div class="breadcrumbDiv">
        <label>
            <asp:Label ID="lblBreadcrumb" runat="server" Text=""></asp:Label></label>
        </div-->
        <div class="inboundmain" runat="server" id="inboundDiv">
            <div class="actionbuttons">
                <asp:Button ID="btnReturn" CssClass="actionbutton redbtn" runat="server" Text="Return"
                     OnClick="GoToContactManagement" Visible="true"/>
            </div>
            <div class="floatleft contactLogs" id="ClientLogs" runat="server">
                <table class="clientLogsTable">
                    <tr>
                        <td>
                                <asp:Label ID="lblDateUsed" runat="server" CssClass="txtlabelLogs fontbold" Text=""></asp:Label>
                        </td>                       
                        <td>
                            <asp:Label ID="txtDateUsed" runat="server" CssClass="txtlabelLogs" Text=""></asp:Label>
                        </td>
                        <td></td>
                        <td>
                                <asp:Label ID="lblInserted" runat="server" CssClass="txtlabelLogs fontbold" Text=""></asp:Label>
                        </td>                        
                        <td>
                            <asp:Label ID="txtDateInput" runat="server" CssClass="txtlabelLogs" Text=""></asp:Label>
                        </td>
                        <td></td>
                        <td>
                                <asp:Label ID="lblCustomerAccount" runat="server" CssClass="custlogaccount fontbold" Text=""></asp:Label>
                        </td>                       
                        <td>                        
                            <asp:TextBox ID="txtAccount" CssClass="custlogaccount" runat="server" Enabled="false" ></asp:TextBox>
                        </td>                        
                    </tr>
                    <tr>
                        <td>
                                <asp:Label ID="lblAmended" runat="server" CssClass="txtlabelLogs fontbold" Text=""></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="txtAmended" runat="server" CssClass="txtlabelLogs" Text=""></asp:Label>
                        </td>
                        <td></td>
                        <td>
                                <asp:Label ID="lblInputBy" runat="server" CssClass="txtlabelLogs fontbold" Text=""></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="txtInputBy" runat="server" CssClass="txtlabelLogs" Text=""></asp:Label>
                        </td>
                        <td></td>
                        <td>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>
                                <asp:Label ID="lblEditBy" runat="server" CssClass="txtlabelLogs fontbold" Text=""></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="txtEditBy" runat="server" CssClass="txtlabelLogs" Text=""></asp:Label>
                        </td>
                        <td></td>
                        <td>
                                <asp:Label ID="lblGroup" runat="server" CssClass="txtlabelLogs fontbold" Text=""></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="txtGroup" runat="server" CssClass="txtlabelLogs" Text=""></asp:Label>
                        </td>
                        <td></td>
                        <td>
                        </td>
                        <td></td>
                    </tr>
                </table>
            </div>
            <div class="floatleft contactManagement" id="clientInfo" runat="server">
                <table class="parentTable" runat="server">
                    <tr>
                        <td>
                            <label class="fontbold">
                                <asp:Label ID="lblCEClientContactProjects" runat="server" Text=""></asp:Label></label>                                       
                        </td><td></td>
                        <td>
                        </td>
                        <td>
                            <label class="fontbold">
                                <asp:Label ID="lblCEAllContactProjectCodes" runat="server" Text=""></asp:Label></label>       
                        </td><td></td>
                    </tr>
                    <tr>                        
                        <td colspan="2" style="width:340px">
                        <div class="divprojects" runat="server">
                            <asp:Table ID="tblClientContactProjects" runat="server" Width="150%" CssClass="projectsTable"> 
                                <asp:TableRow>
                                    <asp:TableCell ColumnSpan="2">
                                        <label class="fontbold">
                                            <asp:Label ID="lblCode" runat="server" Text="Code"></asp:Label></label>      
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <label class="fontbold">
                                            <asp:Label ID="lblDefinition" runat="server" Text="Definition"></asp:Label></label>      
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <label class="fontbold">
                                            <asp:Label ID="lblDateIn" runat="server" Text="Date In"></asp:Label></label>   
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <label class="fontbold">
                                            <asp:Label ID="lblDateLastUsed" runat="server" Text="Date Last Used"></asp:Label></label>   
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </div>  
                        </td>
                        <td colspan="1" style="width:120px">
                            <div runat="server">
                            <table>
                                <tr>
                                    <td>
                                        <div class="actionbuttons">
                                            <asp:Button ID="btnAdd" CssClass="contactbuttons medbluebtn" runat="server" Text="<< Add <<"
                                                 OnClick="btnAddClick"/>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="actionbuttons">
                                            <asp:Button ID="btnRemove" CssClass="contactbuttons medbluebtn" runat="server" Text=">> Remove >>"
                                                 OnClick="btnRemoveClick"/>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            </div>
                        </td>
                        <td colspan="2" style="width:340px">
                            <div class="divprojects" runat="server">
                            <asp:Table ID="tblClientAllContactProjects" runat="server" Width="150%" CssClass="projectsTable"> 
                                <asp:TableRow>
                                    <asp:TableCell ColumnSpan="2">
                                        <label class="fontbold">
                                            <asp:Label ID="lblCode1" runat="server" Text="Code"></asp:Label></label>      
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <label class="fontbold">
                                            <asp:Label ID="lblDefinition1" runat="server" Text="Definition"></asp:Label></label>      
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                            </div>  
                        </td>
                    </tr>
                </table>
                                   
            </div>
        </div>
    </div>
    <script type="text/javascript">
        //declare global variables
        var rowsSelected = 0;
        //change the background color of a row when selected and
        //also count how many rows are selected
        function colorRow(srcElement) {
            var cb = event.srcElement;
            var curElement = cb;
            while (curElement && !(curElement.tagName == "TR")) {
                curElement = curElement.parentElement;
            }
            if (!(curElement == cb)) {
                if (cb.checked) {
                    curElement.style.backgroundColor = "gold";
                    rowsSelected = rowsSelected + 1;
                }
                else {
                    curElement.style.backgroundColor = "#eeeeee";
                    rowsSelected = rowsSelected - 1;
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footerContent" runat="server">
</asp:Content>
