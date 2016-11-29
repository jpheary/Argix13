<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ProjectedDeliverySchedule.aspx.cs" Inherits="ProjectedDeliverySchedule" %>
<%@ MasterType VirtualPath="~/Site.master" %>

<asp:Content ID="idDateRange" ContentPlaceHolderID="Setup" Runat="Server">
<script type="text/javascript">
    var daysback = -90, daysforward = 90, daysspread = 21;
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
    <label for="txtFrom">Pickups&nbsp;</label>
    <asp:TextBox ID="txtFrom" runat="server" Width="100px" AutoPostBack="true" OnTextChanged="OnFromToDateChanged" />
    <label for="txtTo">&nbsp;-&nbsp;</label>
    <asp:TextBox ID="txtTo" runat="server" Width="100px" AutoPostBack="true" OnTextChanged="OnFromToDateChanged" />
</div>
<div style="margin: 10px 0px 0px 50px">
    Client&nbsp;
    <asp:DropDownList id="cboClient" runat="server" Width="288px" DataSourceID="odsClients" DataTextField="ClientName" DataValueField="ClientNumber" AutoPostBack="True" OnSelectedIndexChanged="OnClientChanged"></asp:DropDownList>
    &nbsp;&nbsp;
    <asp:CheckBox ID="chkActiveOnly" runat="server" Width="96px" Text="Active Only" Checked="true" AutoPostBack="true" OnCheckedChanged="OnActiveOnlyChecked" />
</div>
<div style="margin: 10px 0px 0px 50px">
    Agent&nbsp;
    <asp:DropDownList ID="cboAgent" runat="server" Width="288px" AppendDataBoundItems="true" DataSourceID="odsAgents" DataTextField="AgentSummary" DataValueField="AgentNumber" OnSelectedIndexChanged="OnAgentChanged">
        <asp:ListItem Text="All" Value="" Selected="True"></asp:ListItem>
    </asp:DropDownList>
</div>
<asp:ObjectDataSource ID="odsClients" runat="server" TypeName="Argix.EnterpriseRGateway" SelectMethod="GetSecureClients" EnableCaching="true" CacheExpirationPolicy="Sliding" CacheDuration="600">
    <SelectParameters>
        <asp:ControlParameter Name="activeOnly" ControlID="chkActiveOnly" PropertyName="Checked" Type="Boolean" />
    </SelectParameters>
</asp:ObjectDataSource>
<asp:ObjectDataSource ID="odsAgents" runat="server" TypeName="Argix.EnterpriseRGateway" SelectMethod="GetAgents" EnableCaching="true" CacheExpirationPolicy="Sliding" CacheDuration="600">
    <SelectParameters>
        <asp:Parameter Name="mainZoneOnly" DefaultValue="false" Type="Boolean" />
    </SelectParameters>
</asp:ObjectDataSource>
</asp:Content>
