<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="VirtualvsOptimizedTrailerLoad.aspx.cs" Inherits="VirtualvsOptimizedTrailerLoad" %>
<%@ MasterType VirtualPath="~/Site.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Setup" Runat="Server">
<script type="text/javascript">
    var daysback = -365, daysforward = 0, daysspread = 90;
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
    <label for="txtFrom">Dates&nbsp;</label>
    <asp:TextBox ID="txtFrom" runat="server" Width="100px" />
    <label for="txtTo">&nbsp;to&nbsp;</label>
    <asp:TextBox ID="txtTo" runat="server" Width="100px" />
</div>
<div style="margin:20px 0px 20px 25px">
    Optimum Weight&nbsp;
    <asp:TextBox ID="txtWeight" runat="server" Width="96px" ToolTip="Enter an optimized weight and press 'Enter'">36000</asp:TextBox>&nbsp;
    <asp:RequiredFieldValidator ID="rfvWeight" runat="server" ControlToValidate="txtWeight" ErrorMessage="*" SetFocusOnError="True" />
    <asp:RangeValidator ID="rvWeight" runat="server" ControlToValidate="txtWeight" ErrorMessage="Please enter a valid weight (1 - 999999)" MaximumValue="999999" MinimumValue="1" Type="Integer" SetFocusOnError="True" />
</div>
<div style="margin:20px 0px 20px 25px">
    Optimum Cube&nbsp;
    <asp:TextBox ID="txtCube" runat="server" Width="96px" ToolTip="Enter an optimized cube and press 'Enter'">5293555</asp:TextBox>&nbsp;
    <asp:RequiredFieldValidator ID="rfvCube" runat="server" ControlToValidate="txtCube" ErrorMessage="*" SetFocusOnError="True" />
    <asp:RangeValidator ID="rvCube" runat="server" ControlToValidate="txtCube" ErrorMessage="Please enter a valid cube (1 - 9999999)" MaximumValue="9999999" MinimumValue="1" Type="Integer" SetFocusOnError="True" />
</div>
<div style="margin:20px 0px 20px 25px">
    Main Zone&nbsp;
    <asp:DropDownList ID="cboAgent" runat="server" Width="96px" AppendDataBoundItems="true" DataSourceID="odsAgents" DataTextField="MainZone" DataValueField="MainZone">
        <asp:ListItem Text="All" Value="" Selected="True" />
    </asp:DropDownList>
</div>
<asp:ObjectDataSource ID="odsAgents" runat="server" TypeName="Argix.EnterpriseRGateway" SelectMethod="GetAgents" EnableCaching="true" CacheExpirationPolicy="Sliding" CacheDuration="600">
    <SelectParameters>
        <asp:Parameter Name="mainZoneOnly" DefaultValue="true" Type="Boolean" />
    </SelectParameters>
</asp:ObjectDataSource>
</asp:Content>

