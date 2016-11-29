<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="MultipleCartonSearch.aspx.cs" Inherits="MultipleCartonSearch" %>
<%@ MasterType VirtualPath="~/Site.master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Setup" Runat="Server">
<div style="margin:25px 0px 0px 50px">
    Terminal&nbsp;
    <asp:DropDownList ID="cboTerminal" runat="server" Width="288px" DataSourceID="odsTerminals" DataTextField="Description" DataValueField="Number" AutoPostBack="True" OnSelectedIndexChanged="OnTerminalSelected" />
</div>
<div style="margin:10px 0px 0px 50px">
    Numbers&nbsp;
    <asp:TextBox ID="txtNumbers" runat="server" Width="288px" Height="192px" MaxLength="2048000" TextMode="MultiLine" AutoPostBack="True" OnTextChanged="OnNumbersChanged" />
</div>
<asp:ObjectDataSource ID="odsTerminals" runat="server" TypeName="Argix.EnterpriseRGateway" SelectMethod="GetArgixTerminals" EnableCaching="true" CacheExpirationPolicy="Sliding" CacheDuration="900" />
</asp:Content>

