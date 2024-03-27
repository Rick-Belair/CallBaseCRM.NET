<%@ Page Title="Reports" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true"
    CodeBehind="report.aspx.cs" Inherits="CallBaseMock.report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="styles/report.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContent" runat="server">
    <div class="reportContent">
        <div class="reportTop" id="topDiv" runat="server">
            <span class="greybold headerText">
                <asp:Label ID="lblAANDCHeader" runat="server" Text="Public Enquiries AANDC"></asp:Label></span>
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
                    OnClick="btnInbound_Click" />
            </div>
        </div>
        <div class="breadcrumbDiv">
        <label>
            <asp:Label ID="lblReports" runat="server" Text=""></asp:Label>  
            <asp:Label ID="lblRepNum" runat="server" Text=""></asp:Label> 
            <asp:Label ID="lblRepName" runat="server" Text=""></asp:Label></label>
        </div>
        <div class="reportmain">
            <div class="reportCriteria">
                <table class="firstTable">
                    <tr>
                        <td>
                            <label class="greybold">
                                <asp:Label ID="lblReport" runat="server" Text="Report"></asp:Label></label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DropDownList ID="ddlReport" runat="server" CssClass="reportBoxes" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlReport_SelectedIndexChanged">
                                <asp:ListItem Value="a">19a </asp:ListItem>
                                <asp:ListItem Value="b">19b</asp:ListItem>
                                <asp:ListItem Value="c">19c</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td colspan="2">
                            <label class="greybold">
                                <asp:Label ID="lblDateRange" runat="server" Text=""></asp:Label></label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>
                                <asp:Label ID="lblFrom" runat="server" Text=""></asp:Label></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDateFrom" runat="server" CssClass="reportBoxes" Enabled="false"></asp:TextBox>
                            <label for="bodyContent_imgbtnCalFrom" style="display: none;">
                                Label placeholder for image</label>
                            <asp:ImageButton ID="imgbtnCalFrom" CssClass="calendarbtn" runat="server" ImageUrl="images/i_calendar2.gif"
                                AlternateText="Calendar Button" OnClick="imgbtnCalFrom_Click" />
                            <div class="calendarDiv">
                                <asp:Calendar ID="calDateFrom" runat="server" Visible="false" OnSelectionChanged="calDateFrom_SelectionChanged"
                                    OtherMonthDayStyle-ForeColor="GrayText"></asp:Calendar>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblTo" runat="server" Text="Label"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDateTo" runat="server" CssClass="reportBoxes" Enabled="false"></asp:TextBox>
                            <label for="bodyContent_imgbtnCalTo" style="display: none;">
                                Label placeholder for image</label>
                            <asp:ImageButton ID="imgbtnCalTo" CssClass="calendarbtn" runat="server" ImageUrl="images/i_calendar2.gif"
                                AlternateText="Calendar Button" OnClick="imgbtnCalTo_Click" />
                            <div class="calendarDiv">
                                <asp:Calendar ID="calDateTo" runat="server" Visible="false" OnSelectionChanged="calDateTo_SelectionChanged"
                                    OtherMonthDayStyle-ForeColor="GrayText"></asp:Calendar>
                            </div>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <label class="greybold">
                                <asp:Label ID="lblReportLayout" runat="server" Text="Label"></asp:Label></label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:RadioButton ID="rbDetailed" runat="server" Text="Detailed" Enabled="false" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:RadioButton ID="rbSummary" runat="server" Text="Summary" Enabled="false" Checked="true" />
                        </td>
                    </tr>
                </table>
                <table class="lastTable">
                    <tr>
                        <td>
                            <label class="greybold">
                                <asp:Label ID="lblFilterBy" runat="server" Text=""></asp:Label></label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DropDownList ID="ddlFilter" runat="server" CssClass="reportBoxes" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlFilter_SelectedIndexChanged" Width="200">
                                <asp:ListItem Value="KB">KnowledgeBase List</asp:ListItem>
                                <asp:ListItem Value="Prog">Program Info List</asp:ListItem>
                                <asp:ListItem Value="QA">Q &amp; A List</asp:ListItem>
                                <asp:ListItem Value="Ref">Referral List</asp:ListItem>
                                <asp:ListItem Value="Subj">Subject List</asp:ListItem>
                            </asp:DropDownList>
                            <br />
                            <br />
                            <asp:Button ID="btnPrintReport2" runat="server" Text="Print Report" CssClass="medbluebtn printbutton"
                            OnClick="btnPrintReport_Click" />


                        </td>
                    </tr>
                </table>
            </div>
            <div class="reportLists">

                
                <div class="campaignDiv" style="max-width:150px">
                    <label class="greybold">
                        <asp:Label ID="lblCampaignList" runat="server" Text=""></asp:Label></label>
                    <asp:GridView ID="gvCampaign" runat="server" DataKeyNames="tele_no" ShowHeader="False"
                        AutoGenerateColumns="False">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkCampaign" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="tele_desc" />
                        </Columns>
                    </asp:GridView>
                </div>
                <div class="groupDiv" runat="server" id="groupSection" style="max-width:250px">
                    <label class="greybold">
                        <asp:Label ID="lblGroupList" runat="server" Text=""></asp:Label></label>
                    <asp:GridView ID="gvGroup" runat="server" DataKeyNames="own_code" ShowHeader="False"
                        AutoGenerateColumns="False">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkGroup" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="group_def" />
                        </Columns>
                    </asp:GridView>
                </div>
                <div class="knowledgeDiv" runat="server" id="knowledgeList" style="max-width:300px">
                    <label class="greybold">
                        <asp:Label ID="lblKBList" runat="server" Text=""></asp:Label></label>
                    <asp:GridView ID="gvKB" runat="server" DataKeyNames="ref_seq" ShowHeader="False"
                        AutoGenerateColumns="False">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkKnowledge" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="lblSubject" runat="server" Text='<%# shortenLabel(Eval("title").ToString()) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div id="refList" class="refDiv" runat="server" visible="false" style="max-width:150px">
                    <label class="greybold">
                        <asp:Label ID="lblRefList" runat="server" Text=""></asp:Label></label>
                    <asp:GridView ID="gvRef" runat="server" DataKeyNames="ref_seq" ShowHeader="False"
                        AutoGenerateColumns="False">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkRef" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="lblSubject" runat="server" Text='<%# shortenLabel(Eval("title").ToString()) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div id="qaList" class="qaDiv" runat="server" visible="false" style="max-width:150px">
                    <label class="greybold">
                        <asp:Label ID="lblQAList" runat="server" Text=""></asp:Label></label>
                    <asp:GridView ID="gvQA" runat="server" DataKeyNames="ref_seq" ShowHeader="False"
                        AutoGenerateColumns="False">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkQA" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="lblSubject" runat="server" Text='<%# shortenLabel(Eval("title").ToString()) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div id="progList" class="progDiv" runat="server" visible="false" style="max-width:150px">
                    <label class="greybold">
                        <asp:Label ID="lblProgInfoList" runat="server" Text=""></asp:Label></label>
                    <asp:GridView ID="gvProg" runat="server" DataKeyNames="ref_seq" ShowHeader="False"
                        AutoGenerateColumns="False">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkProg" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="lblSubject" runat="server" Text='<%# shortenLabel(Eval("title").ToString()) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div id="subjList" class="subjDiv" runat="server" visible="false" style="max-width:150px">
                    <label class="greybold">
                        <asp:Label ID="lblSubjList" runat="server" Text=""></asp:Label></label>
                    <asp:GridView ID="gvSubjects" runat="server" DataKeyNames="subject_code" ShowHeader="False"
                        AutoGenerateColumns="False">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSubj" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="lblSubject" runat="server" Text='<%# shortenLabel(Eval("subject").ToString()) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div class="orderStatusDiv" style="width: 600px; float: left; max-width:150px">
                    <label class="greybold">
                        <asp:Label ID="lblOrderStatusList" runat="server" Text=""></asp:Label></label>
                    <asp:GridView ID="gvStatus" runat="server" DataKeyNames="ORDER_STATUS_CODE" ShowHeader="False"
                        AutoGenerateColumns="False">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkStatus" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ORDER_STATUS_DESC" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <div class="btnDiv">
                <asp:Button ID="btnPrintReport" runat="server" Text="Print Report" CssClass="medbluebtn printbutton"
                    OnClick="btnPrintReport_Click" />
            </div>
        </div>
    </div>
    <script src="scripts/report.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footerContent" runat="server">
</asp:Content>
