<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true"
    CodeBehind="LegacyIE.aspx.cs" Inherits="CallBaseMock.LegacyIE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="styles/login.css" />
    <style>
        .messageDiv
        {
            margin-top: 15px;
            border: 1px solid #94BFE9;
            background-color: #FFFFFF;
            height: 125px;
            width: 800px;
        }
        p
        {
            text-align: center;
            margin: 0px 55px 0px 55px;
            color: Red;
            font-size: 12px;
        }
        .smallerText
        {
            font-size: 11px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContent" runat="server">
    <div class="logincontent">
        <div class="logintop">
        </div>
        <div class="loginmiddle">
        </div>
        <div class="loginbottom">
            <p id="message" runat="server">
            </p>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footerContent" runat="server">
</asp:Content>
