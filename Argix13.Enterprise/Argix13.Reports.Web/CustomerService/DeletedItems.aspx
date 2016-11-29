<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="DeletedItems.aspx.cs" Inherits="DeletedItems" %>
<%@ MasterType VirtualPath="~/Site.master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Setup" Runat="Server">
<script type="text/javascript">
    $(function () {
        $("#<%=txtSortDate.ClientID %>").datepicker({ minDate: -180, maxDate: +0 });
    });
</script>
<div style="margin:50px 0px 0px 25px">
    <label for="txtSortDate">Sort Date&nbsp;</label>
    <asp:TextBox ID="txtSortDate" runat="server" Width="100px" />
</div>
<div style="margin:10px 0px 0px 50px">
    Terminal&nbsp;
    <asp:DropDownList ID="cboTerminal" runat="server" Width="192px" DataSourceID="odsTerminals" DataTextField="Description" DataValueField="Number" AutoPostBack="True" OnSelectedIndexChanged="OnTerminalSelected" />
</div>
<asp:ObjectDataSource ID="odsTerminals" runat="server" TypeName="Argix.EnterpriseRGateway" SelectMethod="GetArgixTerminals" EnableCaching="true" CacheExpirationPolicy="Sliding" CacheDuration="900" />
</asp:Content>

