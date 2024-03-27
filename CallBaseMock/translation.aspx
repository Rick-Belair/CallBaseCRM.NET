<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true"
    CodeBehind="translation.aspx.cs" Inherits="CallBaseMock.translation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="styles/wcms.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContent" runat="server">
    <div class="wcmsContainer">
        <div class="wcmsTop">
        </div>
        <div class="wcmsContent">
            <asp:ObjectDataSource ID="odsLabels" runat="server" SelectMethod="GetLabels" TypeName="Business.Workflows.WCMSManager"
                UpdateMethod="UpdateLabel">
                <SelectParameters>
                    <asp:ControlParameter ControlID="ddlPage" Name="page" PropertyName="SelectedValue"
                        Type="String" />
                    <asp:ControlParameter ControlID="txtSearch" Name="condition" PropertyName="Text"
                        Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <div class="wcmsInner">
                <table>
                    <tr>
                        <td>
                            Page Name:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlPage" CssClass="controlStyle" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Search:
                        </td>
                        <td>
                            <asp:TextBox ID="txtSearch" runat="server" CssClass="controlStyle"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Button ID="btnGo" runat="server" Text="Go" />
                        </td>
                    </tr>
                </table>
                <div class="labelDiv">
                    <asp:GridView ID="gvLabels" runat="server" CssClass="labelTable" AllowPaging="True"
                        DataSourceID="odsLabels" AutoGenerateColumns="False">
                        <Columns>
                            <asp:CommandField ShowEditButton="True" ItemStyle-Wrap="false"/>
                            <asp:TemplateField HeaderText="Code" ItemStyle-Wrap="false">
                                <ItemTemplate>
                                    <asp:Label ID="lang_code" runat="server" Text='<%# Bind("lang_code") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="English Description">
                                <ItemTemplate>
                                    <%# Eval("lang_en")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="lang_en" runat="server" Text='<%# Bind("lang_en")%>'></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="French Description">
                                <ItemTemplate>
                                    <%# Eval("lang_fr")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="lang_fr" runat="server" Text='<%# Bind("lang_fr")%>'></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footerContent" runat="server">
</asp:Content>
