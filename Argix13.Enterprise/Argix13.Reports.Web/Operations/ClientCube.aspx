<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ClientCube.aspx.cs" Inherits="ClientCube" %>
<%@ MasterType VirtualPath="~/Site.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Setup" Runat="Server">
<script type="text/javascript">
    var daysback = -7, daysforward = 0, daysspread = 1;
    $(function () {
        $("#<%=txtFrom.ClientID %>").datepicker({
            minDate: daysback, maxDate: 0, defaultDate: 0, 
            onClose: function (selectedDate, instance) {
                $("#<%=txtTo.ClientID %>").datepicker("option", "minDate", selectedDate);

                var date = $.datepicker.parseDate("mm/dd/yy", selectedDate, instance.settings);
                date.setDate(date.getDate() + daysspread);
                var todate = $("#<%=txtTo.ClientID %>").datepicker("getDate");
                if (todate > date) $("#<%=txtTo.ClientID %>").datepicker("setDate", date);
            }
        });
        $("#<%=txtTo.ClientID %>").datepicker({
            minDate: 0, maxDate: daysforward, defaultDate: 0, 
            onClose: function (selectedDate, instance) {
                $("#<%=txtFrom.ClientID %>").datepicker("option", "maxDate", selectedDate);

                var date = $.datepicker.parseDate("mm/dd/yy", selectedDate, instance.settings);
                date.setDate(date.getDate() - daysspread);
                var fromdate = $("#<%=txtFrom.ClientID %>").datepicker("getDate");
                if (fromdate < date) $("#<%=txtFrom.ClientID %>").datepicker("setDate", date);
            }
        });
    });
</script>
<div style="margin:25px 0px 0px 39px">
    <label for="txtFrom">Date&nbsp;</label>
    <asp:TextBox ID="txtFrom" runat="server" Width="100px" />
    <label for="txtTo">&nbsp;-&nbsp;</label>
    <asp:TextBox ID="txtTo" runat="server" Width="100px" />
</div>
<div style="margin:10px 0px 0px 50px">
    Client&nbsp;
    <asp:DropDownList id="cboClient" runat="server" Width="288px" DataSourceID="odsClients" DataTextField="ClientName" DataValueField="ClientNumber" AutoPostBack="True" OnSelectedIndexChanged="OnClientChanged" />
</div>
<div style="margin:10px 0px 0px 50px">
    Terminal&nbsp;
    <asp:DropDownList id="cboTerminal" runat="server" Width="192px" DataSourceID="odsTerminals" AppendDataBoundItems="true" DataTextField="Description" DataValueField="TerminalCode" AutoPostBack="True" OnSelectedIndexChanged="OnTerminalChanged">
        <asp:ListItem Text="All" Value="" Selected="True" />
    </asp:DropDownList>
</div>
<div style="margin:10px 0px 0px 50px">
    Vendor&nbsp;
    <asp:DropDownList id="cboVendor" runat="server" Width="192px" DataSourceID="odsVendors" AppendDataBoundItems="true" DataTextField="VendorName" DataValueField="VendorNumber">
        <asp:ListItem Text="All" Value="" Selected="True" />
    </asp:DropDownList>
</div>
<div style="margin:10px 0px 0px 50px">
    Zone&nbsp;
    <asp:DropDownList id="cboZone" runat="server" Width="96px" DataSourceID="odsZones" AppendDataBoundItems="true" DataTextField="Code" DataValueField="Code">
        <asp:ListItem Text="All" Value="" Selected="True" />
    </asp:DropDownList>
</div>    
<asp:ObjectDataSource ID="odsClients" runat="server" TypeName="Argix.EnterpriseRGateway" SelectMethod="GetClients" EnableCaching="true" CacheExpirationPolicy="Sliding" CacheDuration="600">
    <SelectParameters>
        <asp:Parameter Name="number" DefaultValue="" ConvertEmptyStringToNull="true" Type="String" />
        <asp:Parameter Name="division" DefaultValue="01" Type="String" />
        <asp:Parameter Name="activeOnly" DefaultValue="False" Type="Boolean" />
    </SelectParameters>
</asp:ObjectDataSource>
<asp:ObjectDataSource ID="odsTerminals" runat="server" TypeName="Argix.EnterpriseRGateway" SelectMethod="GetClientTerminals" EnableCaching="true" CacheExpirationPolicy="Sliding" CacheDuration="600"  >
    <SelectParameters>
        <asp:ControlParameter Name="number" ControlID="cboClient" PropertyName="SelectedValue" Type="string" />
    </SelectParameters>
</asp:ObjectDataSource>
<asp:ObjectDataSource ID="odsVendors" runat="server" TypeName="Argix.EnterpriseRGateway" SelectMethod="GetVendorsList" EnableCaching="true" CacheExpirationPolicy="Sliding" CacheDuration="300"  >
    <SelectParameters>
        <asp:ControlParameter Name="clientNumber" ControlID="cboClient" PropertyName="SelectedValue" Type="string" />
        <asp:ControlParameter Name="clientTerminal" ControlID="cboTerminal" PropertyName="SelectedValue" Type="string" />
    </SelectParameters>
</asp:ObjectDataSource>
<asp:ObjectDataSource ID="odsZones" runat="server" TypeName="Argix.EnterpriseRGateway" SelectMethod="GetZones" EnableCaching="true" CacheExpirationPolicy="Sliding" CacheDuration="900" />
</asp:Content>

