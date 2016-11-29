<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="SortedBMC.aspx.cs" Inherits="SortedBMC" %>
<%@ MasterType VirtualPath="~/Site.master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Setup" Runat="Server">
<script type="text/javascript">
    $(function () {
        $("#<%=txtCloseDate.ClientID %>").datepicker({ minDate: -0, maxDate: +0 });
    });
</script>
<div style="margin:50px 0px 0px 25px">
    <label for="txtCloseDate">Zone Closed&nbsp;</label>
    <asp:TextBox ID="txtCloseDate" runat="server" Width="100px" />
</div>
<div style="margin:10px 0px 0px 50px">
    Terminal&nbsp;
    <asp:DropDownList ID="cboTerminal" runat="server" Width="300px" DataSourceID="odsTerminals" DataTextField="Description" DataValueField="Number" AutoPostBack="True" OnSelectedIndexChanged="OnTerminalSelected" />
</div>
<div style="margin:10px 0px 0px 50px">
    ReScan Status&nbsp;
    <asp:DropDownList ID="cboFilter" runat="server" Width="200px">
        <asp:ListItem Text="Show All" Value="1" Selected="True" />
        <asp:ListItem Text="Show Non-scanned Only" Value="0" />
    </asp:DropDownList>
</div>
<asp:ObjectDataSource ID="odsTerminals" runat="server" TypeName="Argix.EnterpriseRGateway" SelectMethod="GetArgixTerminals" EnableCaching="true" CacheExpirationPolicy="Sliding" CacheDuration="900" />
</asp:Content>

