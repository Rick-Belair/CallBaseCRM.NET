<%@ Page Title="Inbound Stats" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true"
    CodeBehind="inbound.aspx.cs" Inherits="CallBaseMock.inbound" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="styles/inbound.css" />
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
                <asp:Button ID="btnInbound" CssClass="btn menuBtn inboundLink" runat="server" Text="Inbound Stats"
                    OnClick="btnInbound_Click" /><asp:Button ID="btnKnowledge" CssClass="btn menuBtn knowledgeLink"
                        runat="server" Text="KnowledgeBase" OnClick="btnKnowledge_Click" /><asp:Button ID="btnProduct"
                            CssClass="btn menuBtn productLink" runat="server" Text="Product Listing" OnClick="btnProduct_Click" /><asp:Button
                                ID="btnOrderStatus" CssClass="btn menuBtn orderLink" runat="server" Text="Order Status"
                                OnClick="btnOrderStatus_Click" /><asp:Button ID="btnReports" CssClass="btn menuBtn reportLink"
                                    runat="server" Text="Reports" OnClick="btnReports_Click" />
            </div>
            <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="actionbutton smallbluebtn closeBtn"
                Visible="false" OnClick="btnClose_Click" />
        </div>
        <div id="issueZoomDiv" runat="server" visible="false" class="issueZoom">
            <asp:Button ID="btnCloseIssues" runat="server" Text="Close" OnClick="btnCloseIssues_Click"
                CssClass="closePopoutBtn" />
            <div class="commentDetails">
                <label>
                    <asp:Label ID="lblCurrentComment" runat="server" Text=""></asp:Label></label>
                <label>
                    <asp:Label ID="lblCommentDate" runat="server" Text="Comment Date"></asp:Label></label>
            </div>
            <asp:TextBox ID="txtCurrentComment" MaxLength="4000" CssClass="textareaStyle" runat="server" Width="450"
                Height="120" TextMode="MultiLine" Enabled="false"  onKeyUp="javascript:lenCheck(this, 4000);" />
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
            <asp:Button ID="btnAnswerClose" runat="server" Text="Close" OnClick="btnAnswerClose_Click"
                CssClass="closePopoutBtn" />
            <div class="commentDetails">
                <label>
                    <asp:Label ID="lblIssuesAZ" runat="server" Text=""></asp:Label></label>
            </div>
            <asp:TextBox ID="txtIssuesZ" MaxLength="4000" CssClass="textareaStyle" runat="server" Width="700"
                Height="320" TextMode="MultiLine" Enabled="false" onKeyUp="javascript:lenCheck(this, 4000);" />
            <br />
            <br />
            <div class="commentDetails">
                <label>
                    <asp:Label ID="lblAnswerAZ" runat="server" Text=""></asp:Label></label>
            </div>
            <asp:TextBox ID="txtAnswerZ" MaxLength="4000" CssClass="textareaStyle" runat="server" Width="700"
                Height="320" TextMode="MultiLine" Enabled="false" onKeyUp="javascript:lenCheck(this, 4000);" />
        </div>
        <div id="clientSearchDiv" class="clientSearch" runat="server" visible="false">
            <div class="clientTableDiv">
                <label class="fontbold" runat="server" id="lblSelClientMessage">
                    </label>
                <asp:ObjectDataSource ID="odsClientSearch" runat="server" SelectMethod="SearchCustomers"
                    TypeName="DataAccess.InboundDB">
                    <SelectParameters>
                        <asp:SessionParameter Name="condition" SessionField="ClientSearch" Type="String" />
                        <asp:SessionParameter Name="accountFlag" SessionField="AccountFlag" Type="Boolean" />
                        <asp:SessionParameter Name="postFlag" SessionField="PostFlag" Type="Boolean" />
                        <asp:SessionParameter Name="lastFlag" SessionField="LastFlag" Type="Boolean" />
                        <asp:SessionParameter Name="orgFlag" SessionField="OrgFlag" Type="Boolean" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <asp:GridView ID="gvClientSearch" runat="server" AutoGenerateColumns="False" DataSourceID="odsClientSearch" class="gv_ClientSearch"
                    EmptyDataText="No results" SelectedRowStyle-BackColor="LightCyan" DataKeyNames="c_rec_no">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkSelect" runat="server" CommandName="Select" Text='<%# setSelectText() %>' ></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderStyle-HorizontalAlign="Left" DataField="c_postal_code" HeaderText="Postal Code" />
                        <asp:BoundField HeaderStyle-HorizontalAlign="Left" DataField="c_firstname_intl" HeaderText="First Name" />
                        <asp:BoundField HeaderStyle-HorizontalAlign="Left" DataField="c_surname" HeaderText="Surname" />
                        <asp:BoundField HeaderStyle-HorizontalAlign="Left" DataField="c_organization" HeaderText="Organization" />
                        <asp:BoundField HeaderStyle-HorizontalAlign="Left" DataField="c_telephone" HeaderText="Phone" />
                        <asp:TemplateField HeaderText="Account">
                            <ItemTemplate>
                                <asp:LinkButton ID="accountSelect" runat="server" CSSClass="blueLink" CommandName="c_rec_no" Text='<%# Bind("c_rec_no") %>' 
                                OnClick="goToClient" CommandArgument='<%# Eval("c_rec_no") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <br />
            &nbsp;<asp:Button ID="btnClientCancel" runat="server" Text="Cancel" OnClick="btnClientCancel_Click" />
            &nbsp;
            <asp:Button ID="btnClientOK" runat="server" Text="Ok" OnClick="btnClientOK_Click" />
        </div>
        <div id="promotionDiv" class="promoDiv" runat="server" visible="false">
            <div class="promoTableDiv">
                <label class="fontbold" runat="server" id="lblSelPromo">
                    </label>
                <asp:ObjectDataSource ID="odsPromo" runat="server" SelectMethod="GetSources" TypeName="DataAccess.InboundDB">
                    <SelectParameters>
                        <asp:SessionParameter Name="language" SessionField="PageLanguage" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <asp:GridView ID="gvPromotions" runat="server" AutoGenerateColumns="False" DataSourceID="odsPromo"
                    SelectedRowStyle-BackColor="LightCyan" DataKeyNames="PROMOTION_CODE" CssClass="promoTable">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkSelect" runat="server" CommandName="Select" Text='<%# setSelectText() %>' ></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderStyle-HorizontalAlign="Left" DataField="PROM_DEFINITION" HeaderText="Promotion" />
                        <asp:BoundField HeaderStyle-HorizontalAlign="Left" DataField="PROMOTION_CODE" HeaderText="Code" />
                    </Columns>
                </asp:GridView>
            </div>
            <br />
            &nbsp;<asp:Button ID="btnPromoCancel" runat="server" Text="Cancel" OnClick="btnPromoCancel_Click" />
            &nbsp;
            <asp:Button ID="btnPromoOK" runat="server" Text="Ok" OnClick="btnPromoOK_Click" />
        </div>
        <div id="shippingNotesDiv" class="shippingDiv" runat="server" visible="false">
            <asp:Button ID="btnCloseShipping" runat="server" Text="Close" CssClass="closePopoutBtn"
                OnClick="btnCloseShipping_Click" />
            <div class="shippingNotes">
                <label>
                    <asp:Label ID="lblShipNotes" runat="server" Text=""></asp:Label>
                    <asp:Label ID="lblOrderNum1" runat="server" Text=""></asp:Label>
                </label>
            </div>
            <asp:TextBox ID="txtShipNotes" MaxLength="1500" CssClass="textareaStyle" runat="server" Width="700"
                Height="120" TextMode="MultiLine" Enabled="false" />
            <br />
            <br />
            <div class="shippingNotes">
                <label>
                    <asp:Label ID="lblShipDetails" runat="server" Text=""></asp:Label>
                    <asp:Label ID="lblOrderNum2" runat="server" Text=""></asp:Label>
                </label>
            </div>
            <asp:TextBox ID="txtShipDetails" MaxLength="1500" CssClass="textareaStyle" runat="server" Width="700"
                Height="120" TextMode="MultiLine" Enabled="false" />
            <br />
        </div>
        <!--div class="breadcrumbDiv">
        <label>
            <asp:Label ID="lblBreadcrumb" runat="server" Text=""></asp:Label></label>
        </div-->
        <div class="inboundmain" runat="server" id="inboundDiv">
            <div class="actionbuttons">
                <asp:Button ID="btnNew" CssClass="actionbutton redbtn" runat="server" Text="New Enquiry"
                    OnClick="btnNew_Click" />
                <asp:Button ID="btnEdit" CssClass="actionbutton smallbluebtn" runat="server" Text="Edit"
                    OnClick="btnEdit_Click" />
                <asp:Button ID="btnDelete" CssClass="actionbutton smallbluebtn" runat="server" Text="Delete"
                    OnClick="btnDelete_Click" OnClientClick="return confirmDelete()" />
                <asp:Button ID="btnCancel" CssClass="actionbutton smallbluebtn" runat="server" Text="Cancel"
                    Enabled="false" OnClick="btnCancel_Click" />
                <asp:Button ID="btnSave" CssClass="actionbutton smallbluebtn" runat="server" Text="OK/Save"
                    Enabled="false" OnClick="btnSave_Click" ValidationGroup="RequiredFields" CausesValidation="false" />
                <asp:Button ID="btnFind" CssClass="actionbutton smallbluebtn" runat="server" Text="Find"
                    OnClick="btnFind_Click" />
                <label for="bodyContent_txtInqNumber" style="display: none">
                    Label placeholder for textbox</label>
                <asp:TextBox ID="txtInqNumber" runat="server" Text="Ticket #" CssClass="findBox"></asp:TextBox>
                <asp:Button ID="btnFirst" CssClass="movementbutton" runat="server" Text="|<" OnClick="btnFirst_Click" /><asp:Button
                    ID="btnPrev" CssClass="movementbutton" runat="server" Text="<" OnClick="btnPrev_Click" /><asp:Button
                        ID="btnNext" CssClass="movementbutton" runat="server" Text=">" OnClick="btnNext_Click" />
                <asp:Button ID="btnLast" CssClass="movementbutton" runat="server" Text=">|" OnClick="btnLast_Click" />
            </div>
            <div class="leftside floatleft">
                <label for="bodyContent_ddlLine" class="fontbold">
                    <asp:Label ID="lblIncLine" runat="server" Text=""></asp:Label><span class="mandatory">*</span>
                </label>
                <br />
                <asp:DropDownList ID="ddlLine" CssClass="leftsideBoxes" Enabled="false" runat="server" Visible="true"
                    ValidationGroup="RequiredFields">
                    <asp:ListItem></asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rqLine" runat="server" ErrorMessage="" ControlToValidate="ddlLine"
                    ValidationGroup="RequiredFields" />
                <label for="bodyContent_imgbtnScript" class="scriptLabel">
                    <asp:Label ID="lblScript" runat="server" Text="Label"></asp:Label></label>
                <asp:ImageButton ID="imgbtnScript" runat="server" ImageUrl="images/b_arrow2.gif"
                    AlternateText="Script button" OnClick="imgbtnScript_Click" />
                <br />
                <fieldset>
                    <legend>
                        <label for="bodyContent_rblLanguage" class="fontbold">
                            <asp:Label ID="lblLanguage" runat="server" Text=""></asp:Label><span class="mandatory">*</span>
                        </label>
                    </legend>
                    <asp:RadioButtonList ID="rblLanguage" runat="server" Enabled="false" ValidationGroup="RequiredFields">
                        <asp:ListItem>English</asp:ListItem>
                        <asp:ListItem>French</asp:ListItem>
                        <asp:ListItem>Other</asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:RequiredFieldValidator ID="rqLanguage" runat="server" ErrorMessage="" ValidationGroup="RequiredFields"
                        ControlToValidate="rblLanguage" />
                        <label for="bodyContent_txtLanguage" style="display: none;">
                    Label placeholder for textbox</label>
                <asp:TextBox ID="txtLanguage" MaxLength="25" CssClass="txtLang smallText" runat="server" Enabled="false"></asp:TextBox>
                </fieldset>
                
                <fieldset>
                    <legend>
                        <label for="bodyContent_rblCommunication" class="fontbold">
                            <asp:Label ID="lblCommunication" runat="server" Text=""></asp:Label><span class="mandatory">*</span>
                        </label>
                    </legend>
                    <asp:RadioButtonList ID="rblCommunication" runat="server" RepeatColumns="2" Enabled="false"
                        ValidationGroup="RequiredFields">
                        <asp:ListItem>Phone</asp:ListItem>
                        <asp:ListItem>In Person</asp:ListItem>
                        <asp:ListItem>Mail</asp:ListItem>
                        <asp:ListItem>Fax</asp:ListItem>
                        <asp:ListItem>Voice Mail</asp:ListItem>
                        <asp:ListItem>E-Mail</asp:ListItem>
                        <asp:ListItem>Exhibits</asp:ListItem>
                        <asp:ListItem>Web</asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:RequiredFieldValidator ID="rqCommunication" runat="server" ErrorMessage="" ValidationGroup="RequiredFields"
                        ControlToValidate="rblCommunication" />
                </fieldset>
                <fieldset>
                    <legend>
                        <label for="bodyContent_rblGender" class="fontbold">
                            <asp:Label ID="lblGender" runat="server" Text="Label"></asp:Label><span class="mandatory">*</span>
                        </label>
                    </legend>
                    <asp:RadioButtonList ID="rblGender" Enabled="false" runat="server" RepeatColumns="2"
                        ValidationGroup="RequiredFields">
                        <asp:ListItem>Male</asp:ListItem>
                        <asp:ListItem>Unknown</asp:ListItem>
                        <asp:ListItem>Female</asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:RequiredFieldValidator ID="rqGender" runat="server" ErrorMessage="" ValidationGroup="RequiredFields"
                        ControlToValidate="rblGender" />
                </fieldset>
                <br />
                <label for="bodyContent_ddlProvState" class="fontbold">
                    <asp:Label ID="lblProvState" runat="server" Text=""></asp:Label><span class="mandatory">*</span>
                </label>
                <br />
                <asp:DropDownList ID="ddlProvState" Enabled="false" runat="server" CssClass="leftsideBoxes"
                    ValidationGroup="RequiredFields" OnSelectedIndexChanged="ddlProvState_SelectedIndexChanged"
                    AutoPostBack="true">
                    <asp:ListItem></asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rqProvState" runat="server" ErrorMessage="" ValidationGroup="RequiredFields"
                    ControlToValidate="ddlProvState" />
                <label for="bodyContent_ddlSource" class="fontbold">
                    <asp:Label ID="lblSource" runat="server" Text=""></asp:Label>
                </label>
                <br />
                <asp:DropDownList ID="ddlSource" Enabled="false" runat="server" CssClass="leftsideBoxes">
                    <asp:ListItem></asp:ListItem>
                </asp:DropDownList>
                <label for="bodyContent_ddlCust" class="fontbold">
                    <asp:Label ID="lblCustomer" runat="server" Text=""></asp:Label><span class="mandatory">*</span>
                </label>
                <br />
                <asp:DropDownList ID="ddlCust" Enabled="false" runat="server" CssClass="leftsideBoxes"
                    ValidationGroup="RequiredFields">
                    <asp:ListItem></asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rqCust" runat="server" ErrorMessage="" ValidationGroup="RequiredFields"
                    ControlToValidate="ddlCust" />
                <label for="bodyContent_ddlClass" class="fontbold">
                    <asp:Label ID="lblAboriginal" runat="server" Text=""></asp:Label><span class="mandatory">*</span>
                </label>
                <br />
                <asp:DropDownList ID="ddlClass" Enabled="false" runat="server" CssClass="leftsideBoxes"
                    ValidationGroup="RequiredFields">
                    <asp:ListItem Value="0" Text=""> </asp:ListItem>
                    <asp:ListItem Value="1">Aboriginal</asp:ListItem>
                    <asp:ListItem Value="2">Non Aboriginal</asp:ListItem>
                    <asp:ListItem Value="3">No Reply</asp:ListItem>
                    <asp:ListItem Value="4">Unknown</asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rqClass" runat="server" ErrorMessage="" ValidationGroup="RequiredFields"
                    ControlToValidate="ddlClass" />
                <asp:Button ID="btnQA" CssClass="greybutton biggergrey" runat="server" Text="Q&amp;A/Program Info Referral/Research"
                    OnClick="btnKnowledge_Click" />
                    <p id="checkMarkText" runat="server" visible="false">&#10004;</p>
                <asp:Button ID="btnSubject" runat="server" Text="Subject" CssClass="greybutton" OnClick="btnSubject_Click" />
                <br />
                <span class="mandatory">
                    <asp:Label ID="lblMandatory" runat="server" Text=""></asp:Label>
                    *</span>
            </div>
            <div class="floatleft issueDiv">
                <label class="fontbold" for="bodyContent_txtIssues">
                    <asp:Label ID="lblIssues" runat="server" Text=""></asp:Label></label>
                <label for="bodyContent_imgbtnIssueHistory" style="display: none;">
                    Label placeholder for image</label>
                <asp:ImageButton ID="imgbtnIssueHistory" runat="server" ImageUrl="images/i_history.gif"
                    AlternateText="Zoom/View History button" OnClick="imgbtnIssueHistory_Click" />
                <br />
                <asp:TextBox ID="txtIssues" MaxLength="4000" CssClass="textareaStyle" runat="server" Width="100%" Height="210"
                    TextMode="MultiLine" Enabled="false" onKeyUp="javascript:lenCheck(this, 4000);" />
                <div class="orderStatus">
                    <label class="fontbold" for="bodyContent_ddlOrderStatus">
                        <asp:Label ID="lblOrderStatus" runat="server" Text=""></asp:Label></label>
                    &nbsp;
                    <label for="bodyContent_imgbtnOrderStatus" style="display: none;">
                        Label placeholder for image</label>
                    <asp:ImageButton ID="imgbtnOrderStatus" runat="server" ImageUrl="images/i_history.gif"
                        AlternateText="Zoom/View History button" OnClientClick="ShowOrderHistory()" />
                    <br />
                    <asp:DropDownList ID="ddlOrderStatus" Enabled="false" Width="100%" runat="server">
                    </asp:DropDownList>
                    <br />
                    <label class="fontbold">
                        <asp:Label ID="lblCallBack" runat="server" Text=""></asp:Label></label>
                    <label for="bodyContent_imgbtnCalendar" style="display: none;">
                        Label placeholder for image</label>
                    <asp:ImageButton ID="imgbtnCalendar" CssClass="calendarbtn" runat="server" ImageUrl="images/i_calendar2.gif"
                        AlternateText="Calendar Button" Enabled="false" OnClick="imgbtnCalendar_Click" />
                    <div class="calendarDiv">
                        <asp:Calendar ID="calCallbackDate" runat="server" Visible="false" OnSelectionChanged="calCallbackDate_SelectionChanged"
                            OtherMonthDayStyle-ForeColor="GrayText"></asp:Calendar>
                    </div>
                    <br />
                    <label for="bodyContent_txtDate" style="display: none;">
                        Label placeholder for textbox</label>
                    <asp:TextBox ID="txtDate" Width="100" Height="20" runat="server" Enabled="false"></asp:TextBox>
                    <label for="bodyContent_txtTime" style="display: none;">
                        Label placeholder for textbox</label>
                    <asp:TextBox ID="txtTime" MaxLength="5" Width="40" Height="20" runat="server" Enabled="false"></asp:TextBox>
                    <br />
                    <table class="orderTable">
                        <tr>
                            <td>
                                <label for="bodyContent_txtOwner">
                                    <asp:Label ID="lblOwner" runat="server" Text=""></asp:Label></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtOwner" Enabled="false" CssClass="orderBoxes" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label for="bodyContent_txtInputDate">
                                    <asp:Label ID="lblInputDate" runat="server" Text=""></asp:Label></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtInputDate" Enabled="false" CssClass="orderBoxes" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label for="bodyContent_txtInitialAgent">
                                    <asp:Label ID="lblInitialAgent" runat="server" Text=""></asp:Label></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtInitialAgent" Enabled="false" CssClass="orderBoxes" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label for="bodyContent_txtEditAgent">
                                    <asp:Label ID="lblEditAgent" runat="server" Text=""></asp:Label></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtEditAgent" Enabled="false" CssClass="orderBoxes" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label for="bodyContent_txtEditDate">
                                    <asp:Label ID="lblEditDate" runat="server" Text="Label"></asp:Label></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtEditDate" Enabled="false" CssClass="orderBoxes" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="answerDiv">
                    <label class="fontbold" for="bodyContent_txtAnswer">
                        <asp:Label ID="lblAnswer" runat="server" Text=""></asp:Label></label>&nbsp;
                    <label for="bodyContent_imgbtnAnswer" style="display: none;">
                        Label placeholder for image</label>
                    <asp:ImageButton ID="imgbtnAnswer" runat="server" ImageUrl="images/i_history.gif"
                        AlternateText="Zoom/View History button" OnClick="imgbtnAnswer_Click" />
                    <br />
                    <asp:TextBox ID="txtAnswer" MaxLength="4000" CssClass="textareaStyle" runat="server" Width="100%" Height="105"
                        TextMode="MultiLine" Enabled="false" onKeyUp="javascript:lenCheck(this, 4000);" />
                </div>
            </div>
            <div class="floatleft shippingInfo" id="shippingDiv" runat="server">
                <label class="fontbold">
                    <asp:Label ID="lblCustDetails" runat="server" Text=""></asp:Label></label>
                <hr class="dashedbottom" />
                <table class="custInfo">
                    <tr>
                        <td>
                            <label for="bodyContent_txtAccount">
                                <asp:Label ID="lblCustomerAccount" runat="server" Text=""></asp:Label></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAccount" CssClass="custboxes" runat="server" Enabled="false"></asp:TextBox>
                            <label for="bodyContent_imgbtnZoomAccount" style="display: none;">
                                Label placeholder for image</label>
                            <asp:ImageButton ID="imgbtnZoomAccount" ImageUrl="images/i_zoom2.gif" runat="server"
                                AlternateText="Zoom button" Enabled="false" />
                        </td>
                        <td>
                            <label for="bodyContent_txtPhone">
                                <asp:Label ID="lblPhone" runat="server" Text="Label"></asp:Label></label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="bodyContent_txtPostZip">
                                <asp:Label ID="lblPostZip" runat="server" Text=""></asp:Label></label>
                        </td>
                        <td>
                            <asp:TextBox MaxLength="20" ID="txtPostZip" CssClass="custboxes" runat="server" Enabled="false"></asp:TextBox>
                            <label for="bodyContent_imgbtnZoomPost" style="display: none;">
                                Label placeholder for image</label>
                            <asp:ImageButton ID="imgbtnZoomPost" ImageUrl="images/i_zoom2.gif" runat="server"
                                AlternateText="Zoom button" Enabled="false" OnClick="imgbtnZoomPost_Click" />
                        </td>
                        <td>
                            <asp:TextBox MaxLength="25" ID="txtPhone" CssClass="custboxes" runat="server" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="bodyContent_txtFirst">
                                <asp:Label ID="lblFirstName" runat="server" Text="Label"></asp:Label></label>
                        </td>
                        <td>
                            <asp:TextBox MaxLength="20" ID="txtFirst" Enabled="false" CssClass="custboxes largerCustBox" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            <label for="bodyContent_txtFax">
                                <asp:Label ID="lblFax" runat="server" Text=""></asp:Label></label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="bodyContent_txtLast">
                                <asp:Label ID="lblSurname" runat="server" Text="Label"></asp:Label></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLast" MaxLength="30" CssClass="custboxes largerCustBox" runat="server" Enabled="false"></asp:TextBox>
                            <label for="bodyContent_imgbtnZoomLast" style="display: none;">
                                Label placeholder for image</label>
                            <asp:ImageButton ID="imgbtnZoomLast" ImageUrl="images/i_zoom2.gif" runat="server"
                                AlternateText="Zoom button" OnClick="imgbtnZoomLast_Click" Enabled="false" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtFax" MaxLength="30" Enabled="false" CssClass="custboxes" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="bodyContent_txtOrganization">
                                <asp:Label ID="lblOrganization" runat="server" Text=""></asp:Label></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtOrganization" MaxLength="100" CssClass="custboxes largerCustBox" runat="server"
                                Enabled="false"></asp:TextBox>
                            <label for="bodyContent_imgbtnZoomOrganization" style="display: none;">
                                Label placeholder for image</label>
                            <asp:ImageButton ID="imgbtnZoomOrganization" ImageUrl="images/i_zoom2.gif" runat="server"
                                AlternateText="Zoom button" OnClick="imgbtnZoomOrganization_Click" Enabled="false" />
                        </td>
                        <td>
                            <label for="bodyContent_txtEmail">
                                <asp:Label ID="lblEmail" runat="server" Text=""></asp:Label></label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmail" MaxLength="100" Enabled="false" CssClass="custboxes largerCustBox" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="bodyContent_ddlProvState2">
                                <asp:Label ID="lblProvState2" runat="server" Text=""></asp:Label></label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlProvState2" Enabled="false" runat="server" CssClass="custboxes">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <label for="bodyContent_txtCountry">
                                <asp:Label ID="lblCountry" runat="server" Text=""></asp:Label></label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="bodyContent_txtCity">
                                <asp:Label ID="lblCity" runat="server" Text=""></asp:Label></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCity" MaxLength="30" Enabled="false" CssClass="custboxes" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCountry" MaxLength="50" Enabled="false" CssClass="custboxes" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="bodyContent_txtAddress">
                                <asp:Label ID="lblAddress" runat="server" Text=""></asp:Label></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAddress" MaxLength="100" Enabled="false" CssClass="custboxes largerCustBox" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            <label for="bodyContent_txtAddress2" style="display: none;">
                                Label placeholder for textbox</label>
                            <asp:TextBox ID="txtAddress2" MaxLength="50" Enabled="false" CssClass="custboxes largerCustBox"
                                runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="floatleft orderDetails" id="orderDetailsDiv" runat="server">
                <div class="orderbuttons">
                <label class="fontbold">
                    <asp:Label ID="lblOrderDelivery" runat="server" Text=""></asp:Label></label>
                     &nbsp;
                      <asp:Button ID="btnSendEmail" runat="server" Text="email!" CssClass="orderbutton medbluebtn"
                        OnClick="btnSendEmail_Click" Enabled="true" />
                        </div>
                <hr class="dashedbottom" />
                <table class="orderInfo">
                    <tr>
                        <td>
                            <label for="bodyContent_ddlDelivery">
                                <asp:Label ID="lblDeliveryMode" runat="server" Text=""></asp:Label></label>
                        </td>
                        <td colspan="3">
                            <asp:DropDownList ID="ddlDelivery" Enabled="false" CssClass="orderboxes" runat="server" Width="75%">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                        </td>

                        
                    </tr>
                    <tr>
                        <td>
                            <label for="bodyContent_txtBackOrder">
                                <asp:Label ID="lblBackOrder" runat="server" Text=""></asp:Label></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBackOrder" Enabled="false" CssClass="orderboxes" runat="server"></asp:TextBox>
                        </td>
                        <!--
                        <td>
                            <label for="bodyContent_txtReady">
                                Ready to Ship</label>
                        </td>
                        <td>
                            <%--<asp:TextBox ID="txtReady" Enabled="false" CssClass="orderboxes" runat="server"></asp:TextBox>--%>
                        </td-->
                        <td>
                            <label for="bodyContent_txtDelivered">
                                <asp:Label ID="lblDelivered" runat="server" Text=""></asp:Label></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDelivered" Enabled="false" CssClass="orderboxes" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <div class="orderbuttons">
                    <asp:Button ID="btnShipNotes" runat="server" Text="Shipping Notes" CssClass="orderbutton medbluebtn"
                        OnClick="btnShipNotes_Click" />
                    <asp:Button ID="btnAddItem" runat="server" Text="Add Item" CssClass="orderbutton medbluebtn"
                        OnClick="btnProduct_Click" Enabled="false" />
                    <asp:Button ID="btnAddElectronic" runat="server" Text="Add Electronic Item" CssClass="orderbutton medbluebtn"
                        OnClick="btnAddElectronic_Click" Enabled="false" />

                </div>
                <div class="productDiv">
                    <asp:Table ID="tblProducts" runat="server" CssClass="productTable">
                        <asp:TableHeaderRow>
                            <asp:TableHeaderCell></asp:TableHeaderCell>
                            <asp:TableHeaderCell ID="thcProduct" runat="server"></asp:TableHeaderCell>
                            <asp:TableHeaderCell ID="thcDesc" runat="server"></asp:TableHeaderCell>
                            <asp:TableHeaderCell ID="thcQTY" runat="server"></asp:TableHeaderCell>
                            <asp:TableHeaderCell ID="thcProcess" runat="server"></asp:TableHeaderCell>
                            <asp:TableHeaderCell ID="thcDate" runat="server"></asp:TableHeaderCell>
                            <asp:TableHeaderCell ID="thcSale" runat="server"></asp:TableHeaderCell>
                            <asp:TableHeaderCell ID="thcGST" runat="server"></asp:TableHeaderCell>
                            <asp:TableHeaderCell ID="thcPST" runat="server"></asp:TableHeaderCell>
                            <asp:TableHeaderCell ID="thcTotal" runat="server"></asp:TableHeaderCell>
                            <asp:TableHeaderCell ID="thcKit" runat="server"></asp:TableHeaderCell>
                            <asp:TableHeaderCell ID="thcWH" runat="server"></asp:TableHeaderCell>
                            <asp:TableHeaderCell ID="thcWHStatus" runat="server"></asp:TableHeaderCell>
                            <asp:TableHeaderCell ID="thcNotes" runat="server"></asp:TableHeaderCell>
                            <asp:TableHeaderCell ID="thcFormat" runat="server"></asp:TableHeaderCell>
                        </asp:TableHeaderRow>
                    </asp:Table>
                </div>
                <table border="0" cellpadding="0">
                    <tr>
                        <td>
                            <label for="bodyContent_txtProductLines">
                                <asp:Label ID="lblProductLines" runat="server" Text=""></asp:Label></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtProductLines" CssClass="smallText" Enabled="false" Width="40"
                                Height="15" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            <label for="bodyContent_txtTotalQTY">
                                <asp:Label ID="lblTotalQty" runat="server" Text=""></asp:Label></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTotalQTY" CssClass="smallText" Enabled='false' Width="40" Height="15"
                                runat="server"></asp:TextBox>
                        </td>
                        <td>
                            <label for="bodyContent_txtTotalPrice">
                                <asp:Label ID="lblTotalPrice" runat="server" Text=""></asp:Label></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTotalPrice" CssClass="smallText" Width="60" Enabled="false" Height="15"
                                runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr><td>&nbsp;</td></tr>
                </table>
            </div>
            <div class="floatleft confirmStatus" id="confirmStatusDiv" runat="server" visible="false">
                <label class="fontbold">
                    <asp:Label ID="lblConfirmStatus" runat="server" Text=""></asp:Label></label>
                <hr class="dashedbottom" />
                <br />
                <label>
                <asp:Label ID="lblCurrentStatusMessage" runat="server" Text=""></asp:Label> 
                    <asp:Label CssClass="fontbold" ID="lblCurStatus" runat="server" Text=""></asp:Label>
                </label>
                <br />
                <br />

                <fieldset>
                        <legend>
                            <label class="fontbold">
                                <asp:Label ID="lblSelStatusMessage" runat="server" Text=""></asp:Label></label></legend>
                        <asp:RadioButtonList ID="rblFinalStatus" runat="server" Enabled="true">
                        </asp:RadioButtonList>
                    </fieldset>
                    <br />
                    <br />
                <asp:Button ID="btnSaveStatus" CssClass="actionbutton smallbluebtn saveStatusBtn" runat="server" Text="" 
                    onclick="btnSaveStatus_Click" />
            </div>
        </div>
        <div class="productContent" runat="server" id="productSectionDiv" visible="false">
            <div class="productTop">
                <table>
                    <tr>
                        <td>
                            <label for="bodyContent_txtClientInfo" class="leftProductlbl fontbold">
                                <asp:Label ID="lblClientProd" runat="server" Text="Label"></asp:Label></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtClientInfo" runat="server" CssClass="productTxt " Width="300px"
                                Enabled="false"></asp:TextBox>
                        </td>
                        <td>
                            <label for="bodyContent_txtTicketNum" class="fontbold">
                                <asp:Label ID="lblTicketNumProd" runat="server" Text=""></asp:Label></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTicketNum" runat="server" CssClass="productTxt" Width="80px"
                                Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="bodyContent_txtSearch" class="leftProductlbl fontbold">
                                <asp:Label ID="lblSearchProd" runat="server" Text=""></asp:Label></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtSearch" runat="server" CssClass="productTxt" Width="350px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="btnProductSearch" runat="server" Text="Search" OnClick="btnProductSearch_Click" />
                        </td>
                    </tr>
                </table>
                <label>
                    <asp:Label ID="lblDisplayInfo" runat="server" Text="" CssClass="countLabels" /><asp:Label
                        ID="lblProductCount" runat="server" Text="" /></label>
            </div>
            <asp:ObjectDataSource ID="odsInventory" runat="server" SelectMethod="GetInventory"
                TypeName="DataAccess.InboundDB">
                <SelectParameters>
                    <asp:SessionParameter DefaultValue="EN" Name="language" SessionField="PageLanguage"
                        Type="String" />
                    <asp:SessionParameter DefaultValue="KIO,BC3," Name="usergroup" SessionField="UserAccess"
                        Type="String" />
                    <asp:ControlParameter ControlID="txtSearch" DefaultValue="All" Name="condition" PropertyName="Text"
                        Type="String" />
                    <asp:SessionParameter DefaultValue="" Name="products" SessionField="InventoryCodes"
                        Type="String" />
                    <asp:ControlParameter ControlID="ddlWH" DefaultValue="0" Name="sWH" PropertyName="SelectedValue"
                        Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:ObjectDataSource ID="odsInventoryElectronic" runat="server" SelectMethod="GetInventoryElectronic"
                TypeName="DataAccess.InboundDB">
                <SelectParameters>
                    <asp:SessionParameter DefaultValue="EN" Name="language" SessionField="PageLanguage"
                        Type="String" />
                    <asp:SessionParameter DefaultValue="KIO,BC3," Name="usergroup" SessionField="UserAccess"
                        Type="String" />
                    <asp:ControlParameter ControlID="txtSearch" DefaultValue="All" Name="condition" PropertyName="Text"
                        Type="String" />
                    <asp:SessionParameter DefaultValue="" Name="products" SessionField="InventoryCodes"
                        Type="String" />
                    <asp:ControlParameter ControlID="ddlWH" DefaultValue="0" Name="sWH" PropertyName="SelectedValue"
                        Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <div class="productTableDiv">
                <asp:GridView ID="gvInventory" runat="server" CssClass="productTable" AutoGenerateColumns="False"
                    OnRowDataBound="gvInventory_RowDataBound" DataSourceID="odsInventory" OnDataBound="gvInventory_DataBound">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:TextBox ID="txtProdQuantity" runat="server" Enabled="false" Width="30" Height="15"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Part #" DataField="INVENTORY_CODE" />
                        <asp:TemplateField HeaderText="Title">
                            <ItemTemplate>
                                <asp:Label ID="lblTitle" runat="server" Text='<%# shortenLabel(Eval("INVENTORY_DEF").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="WH0" Visible="false">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblWH0" Text='<%# Bind("WH0") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="WH1" Visible="false">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblWH1" Text='<%# Bind("WH1") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="WH2" Visible="false">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblWH2" Text='<%# Bind("WH2") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="WH3" Visible="false">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblWH3" Text='<%# Bind("WH3") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="WH4" Visible="false">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblWH4" Text='<%# Bind("WH4") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="WH5" Visible="false">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblWH5" Text='<%# Bind("WH5") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="WH6" Visible="false">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblWH6" Text='<%# Bind("WH6") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="WH7" Visible="false">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblWH7" Text='<%# Bind("WH7") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="WH8" Visible="false">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblWH8" Text='<%# Bind("WH8") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="WH9" Visible="false">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblWH9" Text='<%# Bind("WH9") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="OtherWHs">
                            <ItemTemplate>
                                <asp:Label ID="lblOtherWH" runat="server" Text=''></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="QTY on Hand" DataField="TOTAL_QTY" />
                        <asp:BoundField HeaderText="CMTD" DataField="CMTD" />
                        <asp:BoundField HeaderText="Status" DataField="PROD_STATUS_DEF" />
                        <asp:BoundField HeaderText="Branch" DataField="INVENTORY_BRANCH" />
                        <asp:BoundField HeaderText="Price" DataField="PRICE" />
                        <asp:TemplateField HeaderText="Kit Status">
                            <ItemTemplate>
                                <asp:Label ID="lblProdKit" runat="server" Text='<%# kitLabel(int.Parse(Eval("INVENTORY_KIT").ToString())) %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:GridView ID="gvInventoryElectronic" runat="server" CssClass="productTable" AutoGenerateColumns="False"
                    OnRowDataBound="gvInventoryElectronic_RowDataBound" DataSourceID="odsInventoryElectronic" OnDataBound="gvInventoryElectronic_DataBound">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkProdSelectE" runat="server" Enabled="true" Width="30" Height="15"></asp:CheckBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Part #" DataField="INVENTORY_CODE" />
                        <asp:TemplateField HeaderText="Title">
                            <ItemTemplate>
                                <asp:Label ID="lblTitleE" runat="server" Width="500" Text='<%# shortenLabel2(Eval("INVENTORY_DEF").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div class="productActions">
                <asp:DropDownList ID="ddlWH" runat="server" Width="200">
                    <asp:ListItem>All WHs</asp:ListItem>
                </asp:DropDownList>
                <asp:Button ID="btnDisplayAll" runat="server" Text="Display All" OnClick="btnDisplayAll_Click" />
                <asp:Button ID="btnHot" runat="server" Text="Hot/New Products" OnClick="btnHot_Click" />
                <asp:Button ID="btnAddToOrder" runat="server" Text="Add To Order" OnClick="btnAddToOrder_Click"
                    Enabled="false" />
            </div>
        </div>
        <div class="knowledgeContent" id="knowledgeSection" runat="server" visible="false">
            <div class="knowledgeTop">
                <table>
                    <tr>
                        <td>
                            <label for="bodyContent_txtClientInfoKB" class="leftKnowledgelbl fontbold">
                                <asp:Label ID="lblClientKB" runat="server" Text=""></asp:Label></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtClientInfoKB" runat="server" CssClass="knowledgeTxt" Width="250px"
                                Enabled="false"></asp:TextBox>
                        </td>
                        <td>
                        </td>
                        <td>
                            <label for="bodyContent_txtTicketNumKB" class="fontbold">
                                <asp:Label ID="lblTicketNumKB" runat="server" Text=""></asp:Label></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTicketNumKB" runat="server" CssClass="knowledgeTxt" Width="80px"
                                Enabled="false"></asp:TextBox>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="bodyContent_ddlLineKB" class="leftKnowledgelbl fontbold">
                                <asp:Label ID="lblCampaignKB" runat="server" Text=""></asp:Label></label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlLineKB" runat="server" Width="250px" Enabled="false" Font-Size="12px">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="bodyContent_txtSearchKB" class="leftKnowledgelbl fontbold">
                                <asp:Label ID="lblSearchForKB" runat="server" Text=""></asp:Label></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSearchKB" runat="server" CssClass="knowledgeTxt" Width="250px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="btnSearchKB" runat="server" Text="Search" OnClick="btnSearchKB_Click" />
                        </td>
                        <td colspan="3">
                            <asp:Button ID="btnListAll" runat="server" Text="List All" OnClick="btnListAll_Click" />
                            <asp:Button ID="btnShowSelected" runat="server" Text="Show Selected" OnClick="btnShowSelected_Click" />
                        </td>
                    </tr>
                </table>
                <label>
                    <asp:Label ID="lblKBCount" runat="server" Text="" CssClass="countLabels"></asp:Label></label>
            </div>
            <div class="knowledgeTableDiv">
                <asp:Table ID="tblKB" runat="server" CssClass="knowledgeTable">
                    <asp:TableHeaderRow runat="server" ID="tblKBHeader">
                        <asp:TableHeaderCell></asp:TableHeaderCell>
                        <asp:TableHeaderCell></asp:TableHeaderCell>
                        <asp:TableHeaderCell ID="thcSubjKB" runat="server">Subject</asp:TableHeaderCell>
                        <asp:TableHeaderCell ID="thcFirstKB" runat="server">First Name</asp:TableHeaderCell>
                        <asp:TableHeaderCell ID="thcLastKB" runat="server">Last Name</asp:TableHeaderCell>
                        <asp:TableHeaderCell ID="thcPhoneKB" runat="server">Telephone #</asp:TableHeaderCell>
                        <asp:TableHeaderCell ID="thcEmailKB" runat="server">Email</asp:TableHeaderCell>
                        <asp:TableHeaderCell ID="thcDescKB" runat="server">Description</asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                </asp:Table>
                <asp:ObjectDataSource ID="osdKB" runat="server" SelectMethod="GetKnowledgeRecords"
                    TypeName="DataAccess.InboundDB">
                    <SelectParameters>
                        <asp:SessionParameter DefaultValue="EN" Name="language" SessionField="PageLanguage"
                            Type="String" />
                        <asp:ControlParameter ControlID="txtSearchKB" Name="condition" PropertyName="Text"
                            Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <asp:GridView ID="gvKB" runat="server" AutoGenerateColumns="False" DataSourceID="osdKB"
                    DataKeyNames="ref_rec_no" CssClass="knowledgeTable" Visible="False" OnRowCommand="gvKB_RowCommand"
                    OnDataBound="gvKB_DataBound" GridLines="Horizontal">
                    <Columns>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImageButton" runat="server" CausesValidation="False" CommandName="Select"
                                    ImageUrl="images/i_zoom.gif" CommandArgument='<%# Eval("ref_rec_no") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelected" runat="server" Enabled="false" Text='<%# Eval("ref_rec_no") %>'
                                    AutoPostBack="true" OnCheckedChanged="chkSelected_CheckedChanged" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField Visible="False" DataField="ref_rec_no" HeaderText="ref_rec_no" />
                        <asp:TemplateField HeaderText="Subject">
                            <ItemTemplate>
                                <asp:Label ID="lblSubject" runat="server" Text='<%# shortenLabel(Eval("ref_subject").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="First Name" DataField="ref_first_name" />
                        <asp:BoundField HeaderText="Last Name" DataField="ref_surname" />
                        <asp:BoundField HeaderText="Telephone #" DataField="ref_tele_no" />
                        <asp:BoundField HeaderText="Email" DataField="ref_email" />
                        <asp:TemplateField HeaderText="Description">
                            <ItemTemplate>
                                <asp:Label ID="lblDescription" runat="server" Text='<%# shortenLabel(Eval("ref_desc").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div class="knowledgeBottom">
                <table class="knowledgeTableBottom">
                    <tr>
                        <td>
                            <label class="fontbold" for="bodyContent_txtIssueKB">
                                <asp:Label ID="lblIssuesKB" runat="server" Text=""></asp:Label></label>
                            <asp:ImageButton ID="imgbtnIssuesKB" runat="server" ImageUrl="images/i_history.gif"
                                AlternateText="Zoom/View History button" OnClick="imgbtnIssueHistory_Click" />
                        </td>
                        <td>
                        </td>
                        <td>
                            <label class="fontbold" for="bodyContent_txtAnswerKB">
                                <asp:Label ID="lblAnswerKB" runat="server" Text=""></asp:Label></label>
                            <asp:ImageButton ID="img" runat="server" ImageUrl="images/i_history.gif" AlternateText="Zoom/View History button"
                                OnClick="imgbtnAnswer_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtIssuesKB" runat="server" MaxLength="4000" TextMode="MultiLine" CssClass="textareaStyle"
                                Height="75" Width="360" Enabled="false" onKeyUp="javascript:lenCheck(this, 4000);"></asp:TextBox>
                        </td>
                        <td>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAnswerKB" MaxLength="4000" runat="server" TextMode="MultiLine" CssClass="textareaStyle"
                                Height="75" Width="360" Enabled="false"  onKeyUp="javascript:lenCheck(this, 4000);"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnHighlightSearch" runat="server" Text="Search on highlighted text"
                                OnClientClick="setHighlightedText()" OnClick="btnSearchKB_Click" Font-Size="12px" />
                            &nbsp;<label class="fontbold regionLabel" for="bodyContent_txtOther">
                                <asp:Label ID="lblOtherRegion" runat="server" Text="" ></asp:Label></label>
                        </td>
                        <td>
                        </td>
                        <td>
                            <asp:TextBox ID="txtOther" MaxLength="100" runat="server" CssClass="knowledgeTxt" Width="360" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <div class="knowledgeMediumDiv">
                    <table class="knowledgeTableBottom">
                        <tr>
                            <td>
                                <label for="bodyContent_txtDateReceived">
                                    <asp:Label ID="lblDateReceivedKB" runat="server" Text=""></asp:Label></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDateReceived" runat="server" CssClass="knowledgeTxt" Width="120"
                                    Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label for="bodyContent_txtDateToKB">
                                    <asp:Label ID="lblCallbackKB" runat="server" Text=""></asp:Label></label><label for="bodyContent_txtTimeToKB"></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDateToKB" runat="server" CssClass="knowledgeTxt" Width="80" Font-Size="12px"
                                    Enabled="false"></asp:TextBox>
                                <asp:ImageButton ID="imgbtnCal2" CssClass="calendarbtn" runat="server" ImageUrl="images/i_calendar2.gif"
                                    AlternateText="Calendar Button" Enabled="false" OnClick="imgbtnCal2_Click" />
                                <div class="calendarDiv">
                                    <asp:Calendar ID="calDateToKB" runat="server" Visible="false" OtherMonthDayStyle-ForeColor="GrayText"
                                        OnSelectionChanged="calDateToKB_SelectionChanged"></asp:Calendar>
                                </div>
                                <asp:TextBox ID="txtTimeToKB" MaxLength="5" runat="server" CssClass="knowledgeTxt" Width="40" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="knowledgeMediumDiv">
                    <table class="knowledgeTableBottom">
                        <tr style="width:80px;">
                            <td>
                                <label for="bodyContent_ddlOrderKB">
                                    <asp:Label ID="lblOrderStatusKB" runat="server" Text=""></asp:Label></label>
                            </td>
                            <td style="width:200px">
                                <asp:DropDownList ID="ddlOrderKB" runat="server"  
                                    style="width: 195px;
                                    height: 35px;
                                    white-space: initial;" 
                                    Enabled="false" Font-Size="Smaller">
                                </asp:DropDownList>
                                <%--CssClass="knowledgeTxt"--%>
                            </td>
                        </tr> 
                        <tr>                            
                            <td>
                                <label for="bodyContent_txtResponseDate">
                                    <asp:Label ID="lblResponseTimeKB" runat="server" Text="Label"></asp:Label></label><label
                                        for="bodyContent_txtResponseTime"></label>
                            </td>
                            <td style="white-space:nowrap;">
                                <asp:TextBox ID="txtResponseDate" runat="server" CssClass="knowledgeTxt" Font-Size="12px"
                                    Width="90px" Enabled="false"></asp:TextBox>
                                <asp:ImageButton ID="imgbtnCal3" CssClass="calendarbtn" runat="server" ImageUrl="images/i_calendar2.gif"
                                    AlternateText="Calendar Button" Enabled="false" OnClick="imgbtnCal3_Click" />
                                <div class="calendarDiv">
                                    <asp:Calendar ID="calResponseDate" runat="server" Visible="false" OtherMonthDayStyle-ForeColor="GrayText"
                                        OnSelectionChanged="calResponseDate_SelectionChanged"></asp:Calendar>
                                </div>
                                <asp:TextBox MaxLength="5" ID="txtResponseTime" runat="server" CssClass="knowledgeTxt" Width="50px"
                                    Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="knowledgeSmallDiv">
                    <fieldset>
                        <legend>
                            <label>
                                <asp:Label ID="lblResearchKB" runat="server" Text=""></asp:Label></label></legend>
                        <asp:RadioButtonList ID="rblResearch" runat="server" Enabled="false">
                            <asp:ListItem Value="0"> &lt; 10 Min.</asp:ListItem>
                            <asp:ListItem Value="1"> &gt; 10 Min.</asp:ListItem>
                        </asp:RadioButtonList>
                    </fieldset>
                    <asp:TextBox ID="txtResearchTime" runat="server" CssClass="knowledgeTxt" Width="30"
                        Enabled="false"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="kbDetailsContent" runat="server" id="kbDetailsSection" visible="false">
            <div class="kbDetailsTop" runat="server" id="kbdTop">
                <label class="fontbold" for="txtCampaign">
                    <asp:Label ID="lblCampaignKBD" runat="server" Text=""></asp:Label></label>
                <br />
                <asp:TextBox ID="txtCampaign" CssClass="knowledgeTxt" Width="70" runat="server" Enabled="false"></asp:TextBox>
                <asp:TextBox ID="txtTele" runat="server" Width="100" CssClass="knowledgeTxt" Enabled="false"></asp:TextBox>
                &nbsp;
                <asp:CheckBox ID="chkActive" runat="server" Enabled="false" />
            </div>
            <div class="usageDiv">
                <label class="fontbold">
                    <asp:Label ID="lblUsageKBD" runat="server" Text=""></asp:Label></label>
                <br />
                <asp:CheckBox ID="chkUsage" runat="server" Enabled="false" OnCheckedChanged="chkUsage_CheckedChanged" />
            </div>
            <div runat="server" id="kbdTicket" visible="false" class="kbDetailsTop kbdTicketDiv">
                <label class="fontbold">
                    <asp:Label ID="lblTicketNumKBD" runat="server" Text=""></asp:Label></label>
                <asp:TextBox ID="txtKBDTicketNum" runat="server" Enabled="false" CssClass="knowledgeTxt"></asp:TextBox>
            </div>
            <div class="refInfo" runat="server" id="kbdRef">
                <label>
                    <asp:Label ID="lblDateEditKBD" runat="server" Text=""></asp:Label>
                    <asp:Label ID="lblEditDateKBD" runat="server" Text=""></asp:Label></label>
                <br />
                <label>
                    <asp:Label ID="lblDateInputKBDText" runat="server" Text=""></asp:Label>
                    <asp:Label ID="lblInputDateKBDVal" runat="server" Text=""></asp:Label></label>
                <br />
                <label>
                    <asp:Label ID="lblOperatorKBD" runat="server" Text=""></asp:Label>
                    <asp:Label ID="lblOperator" runat="server" Text=""></asp:Label></label>
                <br />
                <label>
                    <asp:Label ID="lblReferralKBD" runat="server" Text=""></asp:Label>
                    <asp:Label ID="lblReferral" runat="server" Text=""></asp:Label></label>
            </div>
            <br />
            <div class="kbDetailsBottom" runat="server" id="kbdBottom">
                <table>
                    <tr>
                        <td>
                            <label class="fontbold" for="bodyContent_txtKBSubject">
                                <asp:Label ID="lblSubjKBD" runat="server" Text="" Width="105"></asp:Label>
                            </label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtKBSubject" runat="server" TextMode="MultiLine" Height="75" Width="500"
                                CssClass="textareaStyle kbBottomTxt" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label class="fontbold" for="bodyContent_txtKBSubjectFR">
                                <asp:Label ID="lblSubjFrKBD" runat="server" Text="" Width="105"></asp:Label>
                            </label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtKBSubjectFR" Enabled="false" runat="server" TextMode="MultiLine"
                                Height="75" Width="500" CssClass="textareaStyle kbBottomTxt"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div style="width: 105px">
                                <label class="fontbold" for="bodyContent_txtKBDesc">
                                    <asp:Label ID="lblDescKBD" runat="server" Text=""></asp:Label>
                                    <asp:ImageButton ID="imgBtnZoom" ImageUrl="images/i_zoom2.gif" runat="server" AlternateText="Zoom button"
                                        OnClick="imgBtnZoom_Click" />
                                </label>
                            </div>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtKBDesc" Enabled="false" runat="server" TextMode="MultiLine" Height="100"
                                Width="500" CssClass="textareaStyle kbBottomTxt"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label class="fontbold">
                                <asp:Label ID="lblTypeKBD" runat="server" Text=""></asp:Label></label>
                        </td>
                        <td colspan="3">
                            <asp:CheckBox ID="chkReferral" Enabled="false" runat="server" Text="" />
                            <asp:CheckBox ID="chkQA" Enabled="false" runat="server" Text="" />
                            <asp:CheckBox ID="chkProgram" Enabled="false" runat="server" Text=" Program Info" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label class="fontbold" for="bodyContent_txtKBDivision">
                                <asp:Label ID="lblDivisionKBD" runat="server" Text=""></asp:Label></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtKBDivision" runat="server" Enabled="false"  CssClass="kbDetailTxt"></asp:TextBox>
                            <label class="fontbold" for="bodyContent_txtKBKeyword">
                                <asp:Label ID="lblKeywordKBD" runat="server" Text=""></asp:Label></label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtKBKeyword" runat="server" Enabled="false"  CssClass="kbDetailTxt"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label class="fontbold" for="bodyContent_txtKBFirst">
                                <asp:Label ID="lblFirstKBD" runat="server" Text=""></asp:Label></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtKBFirst" Enabled="false" runat="server" CssClass="kbDetailTxt"></asp:TextBox>
                            <label class="fontbold" for="bodyContent_txtKBLast">
                                <asp:Label ID="lblLastKBD" runat="server" Text=""></asp:Label></label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtKBLast" Enabled="false" runat="server" CssClass="kbDetailTxt"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label class="fontbold" for="bodyContent_txtKBCompany">
                                <asp:Label ID="lblCompanyKBD" runat="server" Text=""></asp:Label></label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtKBCompany" Enabled="false" runat="server" CssClass="kbBiggerTxt"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label class="fontbold" for="bodyContent_txtKBPhone">
                                <asp:Label ID="lblPhoneKBD" runat="server" Text=""></asp:Label></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtKBPhone" Enabled="false" runat="server" CssClass="kbDetailTxt"></asp:TextBox>
                            <label class="fontbold" for="bodyContent_txtKBFax">
                                <asp:Label ID="lblFaxKBD" runat="server" Text=""></asp:Label></label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtKBFax" Enabled="false" runat="server" CssClass="kbDetailTxt"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label class="fontbold" for="bodyContent_txtKBAddress">
                                <asp:Label ID="lblAddressKBD" runat="server" Text=""></asp:Label></label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtKBAddress" Enabled="false" runat="server" CssClass="kbBiggerTxt"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label class="fontbold" for="bodyContent_txtKBCityProv">
                                <asp:Label ID="lblCityProvKBD" runat="server" Text=""></asp:Label></label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtKBCityProv" Enabled="false" runat="server" CssClass="kbBiggerTxt"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label class="fontbold" for="bodyContent_txtKBPostalCode">
                                <asp:Label ID="lblPostalKBD" runat="server" Text=""></asp:Label></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtKBPostalCode" Enabled="false" runat="server" CssClass="kbDetailTxt"></asp:TextBox>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label class="fontbold" for="bodyContent_txtKBEmail">
                                <asp:Label ID="lblEmailKBD" runat="server" Text=""></asp:Label></label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtKBEmail" Enabled="false" runat="server" CssClass="kbBiggerTxt"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label class="fontbold" for="bodyContent_txtKBWeb">
                                <asp:Label ID="lblWebKBD" runat="server" Text=""></asp:Label></label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtKBWeb" Enabled="false" runat="server" CssClass="kbBiggerTxt"></asp:TextBox>
                        </td>
                        <td>
                            <a runat="server" href="#" id="lnkWebSite" />
                        </td>
                    </tr>
                </table>
            </div>
            <div runat="server" id="kbdZoomTable" visible="false" class="kbDetailsBottom">
                <table>
                    <tr>
                        <td colspan="2">
                            <label class="fontbold">
                                <asp:Label ID="lblSubjQuesKBD" runat="server" Text=""></asp:Label></label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:TextBox ID="txtKBDZoomSubject" Enabled="false" runat="server" TextMode="MultiLine"
                                Height="100" Width="600" CssClass="textareaStyle kbBottomTxt"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <label class="fontbold">
                                <asp:Label ID="lblDescDetKBD" runat="server" Text=""></asp:Label></label>&nbsp;
                            &nbsp;<asp:Button ID="btnShowLang" runat="server" Text="" OnClick="btnShowLang_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:TextBox ID="txtKBDZoomDetails" Enabled="false" runat="server" TextMode="MultiLine"
                                Height="350" Width="600" CssClass="textareaStyle kbBottomTxt"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="bodyContent_txtKBDZoomWeb" style="display: none">
                                <asp:Label ID="lblWebKBDZ" runat="server" Text=""></asp:Label></label><asp:TextBox
                                    ID="txtKBDZoomWeb" Enabled="false" runat="server" CssClass="kbBiggerTxt"></asp:TextBox>
                        </td>
                        <td>
                            <a runat="server" href="#" id="lnkZoomWeb" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="subjectContent" runat="server" id="subjectSection" visible="false">
            <div class="subjectTop">
                <table>
                    <tr>
                        <td>
                            <label for="bodyContent_txtClientInfoSubj" class="leftProductlbl fontbold">
                                <asp:Label ID="lblSubjClient" runat="server" Text=""></asp:Label></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtClientInfoSubj" runat="server" CssClass="productTxt " Width="300px"
                                Enabled="false"></asp:TextBox>
                        </td>
                        <td>
                            <label for="bodyContent_txtTicketNumSubj" class="fontbold">
                                <asp:Label ID="lblSubjTicketNum" runat="server" Text=""></asp:Label></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTicketNumSubj" runat="server" CssClass="productTxt" Width="80px"
                                Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <label class="countLabels">
                    <asp:Label ID="lblSubjMessage" runat="server" Text=""></asp:Label></label>
            </div>
            <div class="subjectTableDiv">
                <label class="fontbold">
                    <asp:Label ID="lblSubjList" runat="server" Text=""></asp:Label></label>
                <asp:ObjectDataSource ID="odsSubj" runat="server" SelectMethod="GetSubjects" TypeName="DataAccess.InboundDB">
                    <SelectParameters>
                        <asp:SessionParameter DefaultValue="EN" Name="language" SessionField="PageLanguage"
                            Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <asp:GridView ID="gvSubjects" runat="server" AutoGenerateColumns="False" DataSourceID="odsSubj"
                    CssClass="subjectTable" OnDataBound="gvSubjects_DataBound">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelectedSubj" runat="server" Enabled="false" AutoPostBack="true" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="subject" HeaderText="Subject" />
                        <asp:BoundField DataField="subject_code" HeaderText="Code" />
                    </Columns>
                </asp:GridView>
            </div>
            <table class="subjectBottom">
                <tr>
                    <td>
                        <label class="fontbold">
                            <asp:Label ID="lblOtherSubj" runat="server" Text=""></asp:Label></label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtOtherSubject" runat="server" Width="200" Height="20"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Button ID="btnContinue" runat="server" Text="Continue" CssClass="continueBtn medbluebtn"
                            OnClick="btnContinue_Click" />
                    </td>
                </tr>
            </table>
        </div>
        <div class="orderStatusContent" id="orderStatusSection" runat="server" visible="false">
            <div class="orderStatusTop">
                <table class="orderStatusTable">
                    <tr>
                        <td colspan="6">
                            <label class="fontbold">
                                <asp:Label ID="lblSearchExp" runat="server" Text=""></asp:Label></label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label class="fontbold">
                                <asp:Label ID="lblOrderSearch" runat="server" Text=""></asp:Label></label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCriteria" runat="server" Height="20" Width="80" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlCriteria_SelectedIndexChanged">
                                <asp:ListItem Value="Incomplete" Text=""></asp:ListItem>
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                            <asp:TextBox ID="txtSelectAllHiddenFlag" runat="server" Visible="false"></asp:TextBox>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                            <label class="fontbold">
                                <asp:Label ID="lblOrderGroup" runat="server" Text=""></asp:Label></label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlGroup" runat="server" Width="250px">
                                <asp:ListItem Text="All" Value=""></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label class="fontbold">
                                <asp:Label ID="lblLowValue" runat="server" Text=""></asp:Label></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLowVal" runat="server" Height="20" Width="80" Font-Size="12px"></asp:TextBox>
                            <asp:ImageButton ID="imgbtnCal4" CssClass="calendarbtn" runat="server" ImageUrl="images/i_calendar2.gif"
                                AlternateText="Calendar Button" Visible="false" OnClick="imgbtnCal4_Click" />
                            <div class="calendarDiv">
                                <asp:Calendar ID="calLowVal" runat="server" Visible="false" OtherMonthDayStyle-ForeColor="GrayText"
                                    OnSelectionChanged="calLowVal_SelectionChanged"></asp:Calendar>
                            </div>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                            <label class="fontbold">
                                <asp:Label ID="lblStatusOrder" runat="server" Text="Label"></asp:Label></label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlStatus" runat="server" Width="250">
                                <asp:ListItem Text="All" Value=""></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label class="fontbold">
                                <asp:Label ID="lblHighVal" runat="server" Text=""></asp:Label></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtHighVal" runat="server" Height="20" Width="80" Font-Size="12px"></asp:TextBox>
                            <asp:ImageButton ID="imgbtnCal5" CssClass="calendarbtn" runat="server" ImageUrl="images/i_calendar2.gif"
                                AlternateText="Calendar Button" Visible="false" OnClick="imgbtnCal5_Click" />
                            <div class="calendarDiv">
                                <asp:Calendar ID="calHighVal" runat="server" Visible="false" OtherMonthDayStyle-ForeColor="GrayText"
                                    OnSelectionChanged="calHighVal_SelectionChanged"></asp:Calendar>
                            </div>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                        <td>
                            <asp:Button ID="btnOrderSearch" runat="server" Text="Submit" OnClick="btnOrderSearch_Click" />
                            &nbsp;
                            <asp:Button ID="btnShowIncomplete" runat="server" Text="Show All Incomplete" OnClick="btnShowIncomplete_Click" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="orderStatusLabel">
                <label class="fontbold">
                    <asp:Label ID="lblOrderStatusRecords" runat="server" Text=""></asp:Label></label></div>
            <div class="orderStatusMiddle">
                <asp:ObjectDataSource ID="odsOrderStatus" runat="server" SelectMethod="GetOrderStatusRecords"
                    TypeName="DataAccess.InboundDB">
                    <SelectParameters>
                        <asp:SessionParameter DefaultValue="EN" Name="language" SessionField="PageLanguage"
                            Type="String" />
                        <asp:ControlParameter ControlID="ddlCriteria" Name="searchType" PropertyName="SelectedValue"
                            Type="String" />
                        <asp:ControlParameter ControlID="txtLowVal" Name="lowVal" PropertyName="Text" Type="String" />
                        <asp:ControlParameter ControlID="txtHighVal" Name="highVal" PropertyName="Text" Type="String" />
                        <asp:ControlParameter ControlID="ddlStatus" Name="status" PropertyName="SelectedValue"
                            Type="String" />
                        <asp:ControlParameter ControlID="ddlGroup" Name="group" PropertyName="SelectedValue"
                            Type="String" />
                        <asp:ControlParameter ControlID="txtSelectAllHiddenFlag" Name="selectAllIncompleteFlag" PropertyName="Text"
                            Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <asp:GridView ID="gvOrderStatus" AllowSorting="True" runat="server" AutoGenerateColumns="False"
                    CssClass="orderRecordsTable" DataSourceID="odsOrderStatus" GridLines="None" SortedAscendingHeaderStyle-CssClass="sortasc"
                    SortedDescendingHeaderStyle-CssClass="sortdesc" OnDataBound="gvOrderStatus_DataBound">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkTicket" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ticket #" SortExpression="S_REC_NO">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkTicketNum" runat="server" Text='<%# Bind("S_REC_NO") %>' OnClick="lnkTicketNum_Click"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Date In" DataField="S_DATE_IN" SortExpression="S_DATE_INPUT" />
                        <asp:BoundField HeaderText="Group-User" DataField="GRP" SortExpression="GRP" />

                        <asp:TemplateField HeaderText="Customer Name" SortExpression="CNAME">
                            <ItemTemplate>
                                <div style="width: 80px; overflow: hidden; white-space: nowrap; text-overflow: ellipsis">
                                    <%#Eval("CNAME") %>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField HeaderText="Assigned To" DataField="S_USER_EDIT" SortExpression="EDIT_USER" />
                        <asp:BoundField HeaderText="Order Status" DataField="ORDER_STATUS_DESC" SortExpression="ORDER_STATUS_DESC" />
                        <asp:BoundField HeaderText="Date To Call Back" DataField="DATE_TO_CALLBACK" SortExpression="S_DATE_TO_CALLBACK" />
                        
                        <asp:BoundField HeaderText="Status Date" DataField="STATUS_DATE" SortExpression="S_ORDER_STATUS_DATE" />
                        
                        <asp:BoundField HeaderText="Products Ordered" DataField="SPRODUCT" SortExpression="SPRODUCT" />
                        <asp:BoundField HeaderText="Shipped" DataField="SDATESENT" SortExpression="SDATESENT" />
                        <asp:BoundField HeaderText="Question" DataField="SCOMMENTS" SortExpression="SCOMMENTS" />
                        <asp:BoundField HeaderText="Answer" DataField="SOPENANSWER" SortExpression="SOPENANSWER" />
                    </Columns>
                </asp:GridView>
            </div>
            <div class="orderStatusBottom">
                <div class="assignToDiv" id="assignToSection">
                    <asp:ObjectDataSource ID="odsUsers" runat="server" SelectMethod="GetUsers" TypeName="DataAccess.InboundDB">
                    </asp:ObjectDataSource>
                    <label class="fontbold">
                        <asp:Label ID="lblOrderAssignTo" runat="server" Text=""></asp:Label>
                    </label>
                    &nbsp; &nbsp;
                    <asp:DropDownList ID="ddlUser" runat="server" Width="250px" Height="20px" DataSourceID="odsUsers"
                        DataTextField="u_name" DataValueField="u_id">
                    </asp:DropDownList>
                    &nbsp;
                    <asp:Button ID="btnAssign" runat="server" Text="Assign" OnClick="btnAssign_Click" />
                </div>
                <br />
                <div class="orderStatusbtnDiv">
                    <asp:Button ID="btnPrintReport" runat="server" Text="Print Report" CssClass="orderStatusBtn medbluebtn"
                        OnClick="btnPrintReport_Click" />
                </div>
            </div>
        </div>
    </div>
    <script src="scripts/inbound.js" type="text/javascript"></script>
    <script type="text/javascript">
        function confirmDelete() {
            var message = '<%= DelMessage %>';
            if (confirm(message)) {
                return true;
            }
            else
                return false;
        }

        function lenCheck(txtElement, iLen) {

            var message = "";

            if (txtElement.id.indexOf("Answer") >= 0) {
                message = '<%= AnsToManyCharsMessage %>';
            } else {
				message = '<%= IssToManyCharsMessage %>';
			}
            
			var txtElementObj = $("[id='" + txtElement.id + "']");
            var txtElementObjValue = txtElementObj.val().replace(/(\r\n|\n|\r)/g, "  ");
            var txtElementObjOrgValue = txtElementObj.val();

			if (txtElementObjValue.length > iLen) {
                alert(message);
                txtElementObjOrgValue = txtElementObjOrgValue.substring(0, iLen - 1);
                txtElementObjOrgValue = txtElementObjOrgValue.slice(0, -1);
				txtElementObj.val(txtElementObjOrgValue);
            }
           
            return;
			
        }
	</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footerContent" runat="server">
</asp:Content>
