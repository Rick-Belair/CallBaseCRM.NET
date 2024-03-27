<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClientActivityForm.aspx.cs" Inherits="CallBaseMock.ClientActivityForm" 
MasterPageFile="~/Site1.Master" Title="Contact Activity Form"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
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
                     OnClientClick="JavaScript:window.history.back(1);return false;" Visible="true"/>
                <asp:Button ID="btnNew" CssClass="actionbutton smallbluebtn" runat="server" Text="New"
                     OnClick="btnNewClick" Visible="true"/>                
                <asp:Button ID="btnEdit" CssClass="actionbutton smallbluebtn" runat="server" Text="Edit"
                     OnClick="btnEditClick"/>
                <asp:Button ID="btnDelete" CssClass="actionbutton smallbluebtn" runat="server" Text="Delete"
                     OnClientClick="return confirmDelete()" OnClick="btnDelete_Click"/>
                <asp:Button ID="btnCancel" CssClass="actionbutton smallbluebtn" runat="server" Text="Cancel"
                    Enabled="false" OnClick="btnCancel_Click" />
                <asp:Button ID="btnSave" CssClass="actionbutton smallbluebtn" runat="server" Text="OK/Save"
                    OnClick="btnSaveClick" ValidationGroup="Group1" CausesValidation="false" />
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
                                <asp:Label ID="lblItem" runat="server" CssClass="custlogaccount fontbold" Text=""></asp:Label>
                        </td>                       
                        <td>                        
                            <asp:Label ID="txtItem" CssClass="txtlabelLogs" runat="server" Text="" ></asp:Label>
                        </td>
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
                <table id="Table1" runat="server" style="border-collapse: collapse; border:1px solid black;">
                    <tr>
                        <td colspan="4">
                            <asp:Label ID="lblCommFor" runat="server" CssClass="txtlabelLogs fontbold" Text=""></asp:Label>
                        </td>
                        <td></td><td></td>
                    </tr>
                    <tr><td colspan="6"></td></tr>
                    <tr>
                        <td colspan="4">
                                  <cc1:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
                                        </cc1:ToolkitScriptManager>
                            <table id="comm_tracking" class="comm_tracking" runat="server">
                                <tr>
                                    <td>
                                        <label for="bodyContent_txtStatus">
                                            <asp:Label ID="lblType" runat="server" Text="Type"></asp:Label></label>                                        
                                    </td>
                                    <td></td>
                                    <td>
                                        <asp:DropDownList ID="ddlType" runat="server" AppendDataBoundItems="true" CssClass="custboxesbig" Enabled="true">                                           
                                        </asp:DropDownList>                                        
                                    </td>
                                    <td></td><td></td><td></td>
                                    <td>
                                        <label for="bodyContent_txtStatus">
                                            <asp:Label ID="lblDateDone" runat="server" Text=""></asp:Label></label>                                           
                                    </td>
                                    <td></td>
                                    <td>
                                        <asp:TextBox ID="txtDate" runat="server" CssClass="custboxesbig2" ReadOnly="true"></asp:TextBox>                                       
                                    </td>                                    
                                    <td>
                                        <asp:ImageButton ID="imgPopup" ImageUrl="images/calendar.png" ImageAlign="Bottom" runat="server" />
                                        <cc1:CalendarExtender ID="Calendar1" PopupButtonID="imgPopup" runat="server" TargetControlID="txtDate"
                                            Format="dd-MMM-yyyy">
                                        </cc1:CalendarExtender>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <label for="bodyContent_txtStatus">
                                            <asp:Label ID="lblPriority" runat="server" Text="Type"></asp:Label></label>                                        
                                    </td>
                                    <td></td>
                                    <td>
                                        <asp:DropDownList ID="ddlPriority" runat="server" AppendDataBoundItems="true" CssClass="custboxesbig" Enabled="true">
                                            <asp:ListItem Text="" Value="" />
                                        </asp:DropDownList>                                        
                                    </td>
                                    <td colspan="8"></td>
                                </tr>
                                <tr>
                                    <td>
                                        <label for="bodyContent_txtStatus">
                                            <asp:Label ID="lblStatus" runat="server" Text="Type"></asp:Label></label>                                        
                                    </td>
                                    <td></td>
                                    <td>
                                        <asp:DropDownList ID="ddlStatus" runat="server" AppendDataBoundItems="true" CssClass="custboxesbig" Enabled="true">
                                            <asp:ListItem Text="" Value="" />
                                        </asp:DropDownList>                                        
                                    </td>
                                    <td colspan="8"></td>
                                </tr>
                                <tr>
                                    <td>
                                        <label for="bodyContent_txtStatus">
                                            <asp:Label ID="lblSubject" runat="server" Text="Type"></asp:Label></label>                                        
                                    </td>
                                    <td></td>
                                    <td colspan="8">
                                        <asp:DropDownList ID="ddlSubject" runat="server" AppendDataBoundItems="true" CssClass="custboxesbig1" Enabled="true">
                                            <asp:ListItem Text="" Value="" />
                                        </asp:DropDownList>                                        
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>                                  
                                    </td>
                                    <td></td>
                                    <td colspan="8">
                                        <asp:TextBox ID="txtSubject" runat="server" CssClass="custboxesbig1" ReadOnly="true"></asp:TextBox>                                   
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <label for="bodyContent_txtStatus">
                                            <asp:Label ID="lblContactsProjects" runat="server" Text="Type"></asp:Label></label>                                        
                                    </td>
                                    <td></td>
                                    <td colspan="8">
                                        <asp:DropDownList ID="ddlContactsProjects" runat="server" AppendDataBoundItems="true" CssClass="custboxesbig1" Enabled="true">
                                            <asp:ListItem Text="" Value="" />
                                        </asp:DropDownList>                                        
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <label for="bodyContent_txtStatus">
                                            <asp:Label ID="lblAllContactsProjects" runat="server" Text="Type"></asp:Label></label>                                        
                                    </td>
                                    <td></td>
                                    <td colspan="8">
                                        <asp:DropDownList ID="ddlAllContactsProjects" runat="server" AppendDataBoundItems="true" CssClass="custboxesbig1" Enabled="true">
                                            <asp:ListItem Text="" Value="" />
                                        </asp:DropDownList>                                        
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>                            
                                    </td>
                                    <td></td>
                                    <td colspan="2"><asp:Button ID="btnFormLetter" runat="server" Text="Form Letter" CssClass="actionbuttons_a" OnClick="btnFormLetterClick" Visible="true" Enabled="true" /></td>
                                    <td></td>
                                    <td colspan="4"><asp:Button ID="btnProgramInfo" runat="server" Text="Program Info" CssClass="actionbuttons_a" OnClick="btnProgramInfoClick" Visible="true" Enabled="true" /></td>
                                    <td>                             
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <label for="bodyContent_txtStatus">
                                            <asp:Label ID="lblDetails" runat="server" Text="Details/Text"></asp:Label></label>                                        
                                    </td>
                                    <td></td>
                                    <td colspan="8">
                                        <asp:TextBox ID="txtDetails" runat="server" CssClass="custboxesbig3" TextMode="MultiLine" ReadOnly="true"></asp:TextBox>                                       
                                    </td>
                                    <td></td>
                                </tr>
                            </table>
                        </td>
                        <td></td>
                        <td>
                            <asp:Label ID="lblOutboundActivity" runat="server" CssClass="txtlabelLogs fontbold" Text=""></asp:Label>
                            <div id="Div1" class="divprojects_1" runat="server">
                                 <asp:TreeView id="tree_view" runat="server" Width="150%" ShowLines="true">
                                        <HoverNodeStyle Font-Underline="True" ForeColor="#6666AA" />
                                        <ParentNodeStyle Font-Bold="False" />
                                        <SelectedNodeStyle BackColor="#B5B5B5" Font-Underline="False" HorizontalPadding="0px"
                                            VerticalPadding="0px" />
                                 </asp:TreeView>
                            </div>                           
                        </td>
                    </tr>
                </table>                                   
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function confirmDelete() {
            var message = '<%= DelMessage %>';
            if (confirm(message)) {
                return true;
            }
            else
                return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footerContent" runat="server">
</asp:Content>

