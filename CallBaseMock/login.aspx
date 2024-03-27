<%@ Page Title="CallBase Login Page" Language="C#" MasterPageFile="~/Site1.Master"
    AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="CallBaseMock.login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link rel="stylesheet" href="styles/login.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContent" runat="server">
    <div class="logincontent">
    <span class="greybold headerText" >
        <asp:Label ID="lblAANDCHeader" runat="server" Text="Public Enquiries AANDC"></asp:Label></span>
            <div class="menulinks">
                <img src="images/b_arrow2.gif" alt="arrow image" />&nbsp;<asp:LinkButton ID="lnkLanguage" runat="server" onclick="lnkLanguage_Click" />
            </div>
        <div class="logintop">
        </div>
        <div class="loginmiddle">
        </div>
        <div class="loginbottom">
            <table class="loginTable">
                <tr>
                    <td>
                        <label for="bodyContent_txtUserName" class="fontbold">
                            <asp:Label ID="lblUserName" runat="server" Text=""></asp:Label></label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtUserName" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label for="bodyContent_txtPassword" class="fontbold">
                            <asp:Label ID="lblPassword" runat="server" Text=""></asp:Label></label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btnLogin btn" OnClick="btnLogin_Click" />
                        <asp:LinkButton ID="lnkForgot" runat="server">Forgot Password?</asp:LinkButton>
                    </td>
                </tr>
            </table>
            <asp:Label ID="lblError" CssClass="loginError" ForeColor="Red" runat="server" Text=""></asp:Label>
        </div>
    </div>
</asp:Content>
