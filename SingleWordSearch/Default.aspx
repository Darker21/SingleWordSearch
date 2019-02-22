<%@ Page Title="Play" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SingleWordSearch._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="ContentMain" runat="server">

    <h1>Play</h1>
    <br /><br />
    <asp:Panel ID="panelWordSearch" runat="server" >

    </asp:Panel>
    <br />
    <asp:Panel ID="PanelWordsToFind" runat="server">

    </asp:Panel>
    <script src="Scripts/WordSearch.js" type="text/javascript"></script>
</asp:Content>
