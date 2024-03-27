<%@ Page Title="Contact Management" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" 
    CodeBehind="ContactManagement.aspx.cs" Inherits="CallBaseMock.ContactManagement"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="styles/contact.css" />
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
        <div id="issueZoomDiv" runat="server" visible="false" class="issueZoom">
            <asp:Button ID="btnCloseIssues" runat="server" Text="Close" 
                CssClass="closePopoutBtn" />
            <div class="commentDetails">
                <label>
                    <asp:Label ID="lblCurrentComment" runat="server" Text=""></asp:Label></label>
                <label>
                    <asp:Label ID="lblCommentDate" runat="server" Text="Comment Date"></asp:Label></label>
            </div>
            <asp:TextBox ID="txtCurrentComment" MaxLength="4000" CssClass="textareaStyle" runat="server" Width="450"
                Height="120" TextMode="MultiLine" Enabled="false" />
            <div class="commentDetails">
                <label>
                    <asp:Label ID="lblCommentHist" runat="server" Text=""></asp:Label>
                    <asp:Label ID="lblTicket" runat="server" Text=""></asp:Label>
                    <asp:Label ID="lblMoreDetails" runat="server" Text=""></asp:Label></label>
            </div>
            <div class="commentTableDiv">
                <asp:ObjectDataSource ID="odsCommentHistory" runat="server" SelectMethod="GetCommentHistory"
                    TypeName="DataAccess.InboundDB">
                    <SelectParameters>
                        <asp:SessionParameter Name="ticketnumber" SessionField="TicketNumber" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <asp:GridView ID="gvCommentHistory" runat="server" AutoGenerateColumns="False" DataSourceID="odsCommentHistory"
                    OnSelectedIndexChanged="gvCommentHistory_SelectedIndexChanged" EmptyDataText="No comment history"
                    SelectedRowStyle-BackColor="LightCyan">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkSelect" runat="server" CommandName="Select" Text='<%# setSelectText() %>' ></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderStyle-HorizontalAlign="Left" DataField="date_edit" HeaderText="Date">
                        </asp:BoundField>
                        <asp:BoundField HeaderStyle-HorizontalAlign="Left" DataField="cah_user_edit" HeaderText="User">
                        </asp:BoundField>
                        <asp:BoundField HeaderStyle-HorizontalAlign="Left" DataField="cah_comments" HeaderText="Comment">
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
            </div>
            <div class="commentDetails">
                <label>
                    <asp:Label ID="lblSelComment" runat="server" Text=""></asp:Label>
                </label>
            </div>
            <asp:TextBox ID="txtSelComment" MaxLength="4000" CssClass="textareaStyle" runat="server" Width="450"
                Height="120" TextMode="MultiLine" Enabled="false" />
        </div>
        <div id="answerZoomDiv" runat="server" visible="false" class="answerZoom">
            <asp:Button ID="btnAnswerClose" runat="server" Text="Close" 
                CssClass="closePopoutBtn" />
            <div class="commentDetails">
                <label>
                    <asp:Label ID="lblIssuesAZ" runat="server" Text=""></asp:Label></label>
            </div>
            <asp:TextBox ID="txtIssuesZ" MaxLength="4000" CssClass="textareaStyle" runat="server" Width="700"
                Height="320" TextMode="MultiLine" Enabled="false" />
            <br />
            <br />
            <div class="commentDetails">
                <label>
                    <asp:Label ID="lblAnswerAZ" runat="server" Text=""></asp:Label></label>
            </div>
            <asp:TextBox ID="txtAnswerZ" MaxLength="4000" CssClass="textareaStyle" runat="server" Width="700"
                Height="320" TextMode="MultiLine" Enabled="false" />
        </div>                
        
        <!--div class="breadcrumbDiv">
        <label>
            <asp:Label ID="lblBreadcrumb" runat="server" Text=""></asp:Label></label>
        </div-->
        <div class="inboundmain" runat="server" id="inboundDiv">
            <div class="actionbuttons">
                <asp:Button ID="btnNew" CssClass="actionbutton redbtn" runat="server" Text="New"
                     OnClick="btnNewClick" Visible="true"/>
                <asp:Button ID="btnReturn" CssClass="actionbutton redbtn" runat="server" Text="Return"
                     OnClientClick="JavaScript:window.history.back(1);return false;" Visible="false"/>
                <asp:Button ID="btnEdit" CssClass="actionbutton smallbluebtn" runat="server" Text="Edit"
                     OnClick="btnEditClick"/>
                <asp:Button ID="btnDelete" CssClass="actionbutton smallbluebtn" runat="server" Text="Delete"
                     OnClientClick="return confirmDelete()" OnClick="btnDelete_Click"/>
                <asp:Button ID="btnCancel" CssClass="actionbutton smallbluebtn" runat="server" Text="Cancel"
                    Enabled="false" OnClick="btnCancel_Click" />
                <asp:Button ID="btnSave" CssClass="actionbutton smallbluebtn" runat="server" Text="OK/Save"
                    OnClick="btnSaveClick" ValidationGroup="Group1" CausesValidation="false" />
                <asp:Button ID="btnFind" CssClass="actionbutton smallbluebtn" runat="server" Text="Find"
                     OnClick="btnFind_Click" />
                <label for="bodyContent_txtInqNumber" style="display: none">
                    Label placeholder for textbox</label>
                <asp:TextBox ID="txtInqNumber" runat="server" Text="Account #" CssClass="findBox"></asp:TextBox>
                <asp:Button ID="btnFirst" CssClass="movementbutton" runat="server" Text="|<" OnClick="btnFirst_Click" Enabled="true"/><asp:Button
                    ID="btnPrev" CssClass="movementbutton" runat="server" Text="<" OnClick="btnPrev_Click" Enabled="true"/><asp:Button
                        ID="btnNext" CssClass="movementbutton" runat="server" Text=">" OnClick="btnNext_Click" Enabled="true"/>
                <asp:Button ID="btnLast" CssClass="movementbutton" runat="server" Text=">|" OnClick="btnLast_Click" Enabled="true"/>
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
                <label class="fontbold">
                    <asp:Label ID="lblCustDetails" runat="server" Text=""></asp:Label></label><br /><br />
                    <table class="custInfo">

                        <tr>
                            <td>
                                <label for="bodyContent_txtFirstName">
                                    <asp:Label ID="lblFirstName" runat="server" Text=""></asp:Label></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFirstName" CssClass="custboxesbig" runat="server" Enabled="false" >                           
                                </asp:TextBox>
                                <asp:requiredfieldvalidator ID="requiredFirstName" ValidationGroup="Group1"
                                                            ErrorText="Please Enter First Name!"
                                                            ControlToValidate="txtFirstName"
                                                            runat="server"/>     
                            </td>
                            <td>
                            </td>
                            <td>
                                <label for="bodyContent_txtLastName">
                                    <asp:Label ID="lblSurname" runat="server" Text=""></asp:Label></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtLastName" CssClass="custboxesbig" runat="server" Enabled="false" ></asp:TextBox>
                                <asp:requiredfieldvalidator ID="requiredLastName" ValidationGroup="Group1"
                                                            ErrorText="Please Enter Last Name!"
                                                            ControlToValidate="txtLastName"
                                                            runat="server"/>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label for="bodyContent_txtPick">
                                    <asp:Label ID="lblPickSal" runat="server" Text=""></asp:Label></label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlPick" runat="server" AppendDataBoundItems="true" CssClass="custboxesbig" Enabled="false">
                                    <asp:ListItem Text="" Value="" />
                                </asp:DropDownList>
                            </td>
                            <td>
                            </td>                            
                            <td>
                                <label for="bodyContent_txtSalutation">
                                    <asp:Label ID="lblSalutation" runat="server" Text=""></asp:Label></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSalutation" CssClass="custboxesbig" runat="server" Enabled="false" ></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label for="bodyContent_txtCompany">
                                    <asp:Label ID="lblCompany" runat="server" Text=""></asp:Label></label>
                            </td>
                            <td>
                                    <asp:TextBox ID="txtCompany" CssClass="custboxesbig" runat="server" Enabled="false" ></asp:TextBox>
                            </td>
                            <td>
                            </td>
                            <td>
                                <label for="bodyContent_txtAddress1">
                                    <asp:Label ID="lblAdress1" runat="server" Text=""></asp:Label></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAddress1" CssClass="custboxesbig" runat="server" Enabled="false" ></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label for="bodyContent_txtJobTitle">
                                    <asp:Label ID="lblJobTilte" runat="server" Text=""></asp:Label></label>
                            </td>
                            <td>
                                    <asp:TextBox ID="txtJobTilte" CssClass="custboxesbig" runat="server" Enabled="false" ></asp:TextBox>
                            </td>
                            <td>
                            </td>
                            <td>
                                <label for="bodyContent_txtAddress2">
                                    <asp:Label ID="lblAddress2" runat="server" Text=""></asp:Label></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAddress2" CssClass="custboxesbig" runat="server" Enabled="false" ></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label for="bodyContent_txtPhone">
                                    <asp:Label ID="lblPhone" runat="server" Text=""></asp:Label></label>
                            </td>
                            <td>
                                    <asp:TextBox ID="txtPhone" CssClass="custboxesbig" runat="server" Enabled="false" ></asp:TextBox>
                            </td>
                            <td>
                            </td>
                            <td>
                                <label for="bodyContent_txtCity">
                                    <asp:Label ID="lblCity" runat="server" Text=""></asp:Label></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCity" CssClass="custboxesbig" runat="server" Enabled="false" ></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label for="bodyContent_txtFax">
                                    <asp:Label ID="lblFax" runat="server" Text=""></asp:Label></label>
                            </td>
                            <td>
                                    <asp:TextBox ID="txtFax" CssClass="custboxesbig" runat="server" Enabled="false" ></asp:TextBox>
                            </td>
                            <td>
                            </td>
                            <td>
                                <label for="bodyContent_txtProvCode">
                                    <asp:Label ID="lblProvCode" runat="server" Text=""></asp:Label></label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlProvCode" runat="server" AppendDataBoundItems="true" CssClass="custboxesbig" Enabled="false">
                                    <asp:ListItem Text="" Value="" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label for="bodyContent_txtEmail">
                                    <asp:Label ID="lblEmail" runat="server" Text=""></asp:Label></label>
                            </td>
                            <td>
                                    <asp:TextBox ID="txtEmail" CssClass="custboxesbig" runat="server" Enabled="false" ></asp:TextBox>
                            </td>
                            <td>
                            </td>
                            <td>
                                <label for="bodyContent_txtProvState">
                                    <asp:Label ID="lblProvState" runat="server" Text=""></asp:Label></label>
                            </td>
                            <td>
                                    <asp:TextBox ID="txtProvState" CssClass="custboxesbig" runat="server" Enabled="false" ></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label for="bodyContent_txtWWW">
                                    <asp:Label ID="lblWebURL" runat="server" Text=""></asp:Label></label>
                            </td>
                            <td>
                                    <asp:TextBox ID="txtWebURL" CssClass="custboxesbig" runat="server" Enabled="false" ></asp:TextBox>
                            </td>
                            <td>
                            </td>
                            <td>
                                <label for="bodyContent_txtPostalCode">
                                    <asp:Label ID="lblPostalCode" runat="server" Text=""></asp:Label></label>
                            </td>
                            <td>
                                    <asp:TextBox ID="txtPostalCode" CssClass="custboxesbig" runat="server" Enabled="false" ></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label for="bodyContent_txtLanguage">
                                    <asp:Label ID="lblLanguage" runat="server" Text=""></asp:Label></label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlLanguage" runat="server" AppendDataBoundItems="true" CssClass="custboxesbig" Enabled="false">
                                    <asp:ListItem Text="" Value="" />
                                </asp:DropDownList>
                            </td>
                            <td>
                            </td>
                            <td>
                                <label for="bodyContent_txtCountry">
                                    <asp:Label ID="lblCountry" runat="server" Text=""></asp:Label></label>
                            </td>
                            <td>
                                    <asp:TextBox ID="txtCountry" CssClass="custboxesbig" runat="server" Enabled="false" ></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label for="bodyContent_txtType">
                                    <asp:Label ID="lblType" runat="server" Text=""></asp:Label></label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlType" runat="server" AppendDataBoundItems="true" CssClass="custboxesbig" Enabled="false">
                                    <asp:ListItem Text="" Value="" />
                                </asp:DropDownList>
                            </td>
                            <td>
                            </td>
                            <td>
                                <label for="bodyContent_txtStatus">
                                    <asp:Label ID="lblStatus" runat="server" Text=""></asp:Label></label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlStatus" runat="server" AppendDataBoundItems="true" CssClass="custboxesbig" Enabled="false">
                                    <asp:ListItem Text="" Value="" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td></td><td></td><td></td>
                            <td>
                                <label for="bodyContent_txtDeliveryMode">
                                    <asp:Label ID="lblDeliveryMode" runat="server" Text=""></asp:Label></label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlDeliveryMode" runat="server" AppendDataBoundItems="true" CssClass="custboxesbig" Enabled="false">
                                    <asp:ListItem Text="" Value="" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <br /><br />
                    <label class="fontbold">
                        <asp:Label ID="lblAddInfo" runat="server" Text=""></asp:Label></label><br />
                    <div class="actionbuttons">
                        <asp:Button ID="btnClientActivity" CssClass="contactbuttons medbluebtn" runat="server" Text="CLIENT ACTIVITY"
                             OnClick="btnClientActivityClick"/>
                        <asp:Button ID="btnInboundOrders" CssClass="contactbuttons medbluebtn" runat="server" Text="INBOUND ORDERS"
                             OnClick="btnInboundOrdersClick"/>
                        <asp:Button ID="btnContactProjects" CssClass="contactbuttons medbluebtn" runat="server" Text="CONTACT PROJECTS"
                             OnClick="btnContactProjectsClick"/>
                   </div>
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