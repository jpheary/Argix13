<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Daily Route Detail.aspx.cs" Inherits="DailyRouteDetail" %>
<%@ MasterType VirtualPath="~/Site.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Setup" Runat="Server">
<script type="text/javascript">
    var daysback = -31, daysforward = 0, daysspread = 7;
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
    <label for="txtFrom">Routes&nbsp;</label>
    <asp:TextBox ID="txtFrom" runat="server" Width="100px" />
    <label for="txtTo">&nbsp;-&nbsp;</label>
    <asp:TextBox ID="txtTo" runat="server" Width="100px" />
</div>
<div style="margin:10px 0px 0px 50px">
    Depot&nbsp;
    <asp:DropDownList ID="cboDepot" runat="server" Width="192px" AppendDataBoundItems="true" DataSourceID="odsDepots" DataTextField="Depotname" DataValueField="RS_OrderClass">
        <asp:ListItem Text="All" Value="" Selected="True"></asp:ListItem>
    </asp:DropDownList>
</div>
    <asp:ObjectDataSource ID="odsDepots" runat="server" TypeName="Argix.Terminals.RoadshowGateway" SelectMethod="GetDepots" EnableCaching="true" CacheExpirationPolicy="Sliding" CacheDuration="600" />
</asp:Content>

