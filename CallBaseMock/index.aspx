<%@ Page Title="CallBase Homepage" Language="C#" MasterPageFile="~/Site1.Master"
    AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="CallBaseMock.index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link rel="stylesheet" href="styles/index.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="bodyContent" runat="server">
    <div class="indexcontent">
        <div class="indextop">
            <div id="menu">
                <div class="dropdown callbaseMenu">
                    <button class="btn dropdown-toggle menuBtn" type="button" id="callbaseLink" data-toggle="dropdown">
                        CallBase
                    </button>
                    <ul class="dropdown-menu" role="menu" aria-labelledby="callbaseLink">
                        <li role="presentation"><a role="menuitem" tabindex="-1" href="inbound.aspx">Inbound
                            Tracking</a></li>
                        <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Order Shipping</a></li>
                        <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Contact Management</a></li>
                        <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Web Catalog</a></li>
                    </ul>
                </div>
                <div class="dropdown">
                    <button class="btn dropdown-toggle menuBtn" type="button" id="infoLink" data-toggle="dropdown">
                        Info
                    </button>
                    <ul class="dropdown-menu" role="menu" aria-labelledby="infoLink">
                        <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Order Status</a></li>
                        <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Client Search</a></li>
                        <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Knowledgebase</a></li>
                        <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Inventory</a></li>
                        <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Publication Forms</a></li>
                        <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Web Inventory List</a></li>
                        <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Central Inventory</a></li>
                        <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Form Letters</a></li>
                        <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Back Orders</a></li>
                        <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Master Search</a></li>
                        <li role="presentation"><a role="menuitem" tabindex="-1" href="#">CIDM List</a></li>
                        <li role="presentation"><a role="menuitem" tabindex="-1" href="#">CIDM Request</a></li>
                        <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Task List</a></li>
                    </ul>
                </div>
                <div class="dropdown">
                    <button class="btn dropdown-toggle menuBtn" type="button" id="reportLink" data-toggle="dropdown">
                        Reports
                    </button>
                    <ul class="dropdown-menu" role="menu" aria-labelledby="reportLink">
                        <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Report R1a- Officer
                            Stats</a></li>
                        <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Report R1b- User Activity</a></li>
                        <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Report R2A - Stats
                            Overview</a></li>
                        <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Report R2B - Stats
                            Overview</a></li>
                        <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Report R3 - Research
                            Issues and Comments</a></li>
                        <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Report R4a - KnowledgeBase
                            Usage</a></li>
                        <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Report R5a - Q&A Summary</a></li>
                        <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Report R5b - Program
                            Info</a></li>
                        <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Report R5c - Referrals</a></li>
                        <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Report R6 - Any Product
                            Orders</a></li>
                        <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Report R7 - Fast Movers</a></li>
                        <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Report R8a - Order
                            Stats by Province</a></li>
                        <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Report R8b - Fast
                            Movers by Province, by Product</a></li>
                        <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Report R9a - Fast
                            Movers by Customer Type</a></li>
                        <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Report R9b - Fast
                            Movers by Customer Type, by Product</a></li>
                        <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Report R10a - Promotions</a></li>
                        <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Report R10b - Subjects</a></li>
                        <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Report R11 - Order/Enquiry
                            Data</a></li>
                        <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Report R16b - Any
                            Product by Qty Groups</a></li>
                        <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Report R19a - Edit
                            Agent Response Summary</a></li>
                        <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Report R19b - Edit
                            Agent Response Detail</a></li>
                        <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Report R19c - Edit
                            Agent Response Totals</a></li>
                        <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Report IR1 - Inventory
                            Stock</a></li>
                        <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Report IR5 - Critical
                            Level Check</a></li>
                        <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Report IR6 - Reorder
                            Level Check</a></li>
                        <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Report IR10 - Broken
                            Web Links</a></li>
                        <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Report S1 - All in
                            One Summary</a></li>
                        <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Report S2 - Product
                            Analysis</a></li>
                        <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Advanced Search</a></li>
                    </ul>
                </div>
                <div class="dropdown">
                    <button class="btn dropdown-toggle menuBtn" type="button" id="maintenanceLink" data-toggle="dropdown">
                        Maintenance
                    </button>
                    <ul class="dropdown-menu" role="menu" aria-labelledby="maintenanceLink">
                        <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Content Management</a></li>
                        <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Change Password</a></li>
                        <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Data Dictionaries</a></li>
                        <li role="presentation"><a role="menuitem" tabindex="-1" href="#">User Setup</a></li>
                        <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Administration</a></li>
                        <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Archive</a></li>
                    </ul>
                </div>
            </div>
        </div>
        <div class="indexmiddle">
        </div>
        <div class="indexbottom">
            <span class="indexblurb">Welcome to CallBase. Use the above drop down menus for system
                navigation, to access the Inbound Tracking, KnowledgeBase, Inventory, CRM and Order
                Processing Modules.</span></div>
    </div>
</asp:Content>
