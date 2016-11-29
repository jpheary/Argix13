<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="TimePeriodCartonCompare.aspx.cs" Inherits="TimePeriodCartonCompare" %>
<%@ Register Src="~/ClientVendorGrids.ascx" TagName="ClientVendorGrids" TagPrefix="uc2" %>
<%@ MasterType VirtualPath="~/Site.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Setup" Runat="Server">
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
    <label for="txtFrom">Time Period&nbsp;</label>
    <asp:TextBox ID="txtFrom" runat="server" Width="100px" AutoPostBack="true" OnTextChanged="OnFromToDateChanged" />
    <label for="txtTo">&nbsp;-&nbsp;</label>
    <asp:TextBox ID="txtTo" runat="server" Width="100px" AutoPostBack="true" OnTextChanged="OnFromToDateChanged" />
</div>
<div style="margin:20px 0px 0px 50px">
    Type&nbsp;
    <asp:DropDownList ID="cboType" runat="server" Width="144px">
        <asp:ListItem Text="Over" Value="Over" Selected="True" />
        <asp:ListItem Text="Short" Value="Short" />
    </asp:DropDownList>            
</div>
<div style="margin:20px 0px 0px 50px">
    <asp:CheckBox ID="chkAllDivs" runat="server" Width="288px" Text="All Divisions (shown as '01')" Checked="true" TextAlign="Right" AutoPostBack="true" OnCheckedChanged="OnAllDivsCheckedChanged" />
</div>
<div style="margin:20px 0px 0px 50px">
    <uc2:ClientVendorGrids ID="dgdClientVendor" runat="server" Height="270px" ClientDivision="01" OnAfterClientSelected="OnClientSelected" OnAfterVendorSelected="OnVendorSelected" />
</div>
</asp:Content>

