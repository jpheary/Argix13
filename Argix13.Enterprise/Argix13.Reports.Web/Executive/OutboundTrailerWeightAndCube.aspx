<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="OutboundTrailerWeightAndCube.aspx.cs" Inherits="OutboundTrailerWeightAndCube" %>
<%@ MasterType VirtualPath="~/Site.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Setup" Runat="Server">
<script type="text/javascript">
    var daysback = -90, daysforward = 0, daysspread = 7;
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
<div style="margin:50px 0px 0px 25px">
    <label for="txtFrom">BOL Dates&nbsp;</label>
    <asp:TextBox ID="txtFrom" runat="server" Width="100px" />
    <label for="txtTo">&nbsp;to&nbsp;</label>
    <asp:TextBox ID="txtTo" runat="server" Width="100px" />
</div>
<div style="margin:20px 0px 20px 25px">
    Terminal&nbsp;
    <asp:DropDownList id="cboTerminal" runat="server" Width="200px" AppendDataBoundItems="true" DataSourceID="odsTerminals" DataTextField="Description" DataValueField="TerminalCode">
        <asp:ListItem Text="All" Value="" Selected="True"></asp:ListItem>
    </asp:DropDownList>
</div>
<div style="margin:20px 0px 20px 25px">
    Agent&nbsp;
    <asp:DropDownList ID="cboAgent" runat="server" Width="200px" AppendDataBoundItems="true" DataSourceID="odsAgents" DataTextField="AgentName" DataValueField="AgentNumber">
        <asp:ListItem Text="All" Value="" Selected="True" />
    </asp:DropDownList>
</div>
<div style="margin:20px 0px 20px 25px">
    Report Type&nbsp;
    <asp:DropDownList ID="cboType" runat="server" Width="200px" >
        <asp:ListItem Text="BOL Weight Cube Report" Value="uspRptBOLWeightCubeReport" Selected="True" />
        <asp:ListItem Text="BOL Weight Cube Report By Client" Value="uspRptBOLWeightCubeReportByClient" />
    </asp:DropDownList>
</div>    
<asp:ObjectDataSource ID="odsTerminals" runat="server" TypeName="Argix.EnterpriseRGateway" SelectMethod="GetTerminals" EnableCaching="true" CacheExpirationPolicy="Sliding" CacheDuration="900" />
<asp:ObjectDataSource ID="odsAgents" runat="server" TypeName="Argix.EnterpriseRGateway" SelectMethod="GetAgents" EnableCaching="true" CacheExpirationPolicy="Sliding" CacheDuration="900" >
    <SelectParameters>
        <asp:Parameter Name="mainZoneOnly" DefaultValue="True" Type="Boolean" />
    </SelectParameters>
</asp:ObjectDataSource>
</asp:Content>

