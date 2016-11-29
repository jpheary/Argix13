<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="PacSunDelivery.aspx.cs" Inherits="_PacSunDelivery" %>
<%@ MasterType VirtualPath="~/Site.master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Setup" Runat="Server">
<script type="text/javascript">
    var daysback = -365, daysforward = 0, daysspread = 31;
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
    <label for="txtFrom">Dates&nbsp;</label>
    <asp:TextBox ID="txtFrom" runat="server" Width="100px" AutoPostBack="true" OnTextChanged="OnFromToDateChanged" />
    <label for="txtTo">&nbsp;-&nbsp;</label>
    <asp:TextBox ID="txtTo" runat="server" Width="100px" AutoPostBack="true" OnTextChanged="OnFromToDateChanged" />
</div>
<div style="margin:10px 0px 0px 50px">
    Scope&nbsp;
    <asp:DropDownList ID="cboFilter" runat="server" Width="120px">
        <asp:ListItem Text="All" Selected="True" Value="0" />
        <asp:ListItem Text="OS&D Only" Value="1" />
    </asp:DropDownList>
</div>
</asp:Content>

