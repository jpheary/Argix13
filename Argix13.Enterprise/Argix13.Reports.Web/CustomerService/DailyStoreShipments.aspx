<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="DailyStoreShipments.aspx.cs" Inherits="DailyStoreShipments" %>
<%@ Register Src="~/ClientDivisionGrids.ascx" TagName="ClientDivisionGrids" TagPrefix="uc6" %>
<%@ MasterType VirtualPath="~/Site.master" %>

<asp:Content ID="cSetup" ContentPlaceHolderID="Setup" Runat="Server">
<script type="text/javascript">
    $(function () {
        $("#<%=txtFrom.ClientID %>").datepicker({
            onClose: function (selectedDate) { $("#<%=txtTo.ClientID %>").datepicker("option", "minDate", selectedDate); }
        });
        $("#<%=txtTo.ClientID %>").datepicker({
            onClose: function (selectedDate) { $("#<%=txtFrom.ClientID %>").datepicker("option", "maxDate", selectedDate); }
        });
    });
</script>
<div style="margin:50px 0px 0px 37px">
    <label for="txtFrom">Dates:&nbsp;</label>
    <asp:TextBox ID="txtFrom" runat="server" Width="100px" />
    <label for="txtTo">&nbsp;-&nbsp;</label>
    <asp:TextBox ID="txtTo" runat="server" Width="100px" />
</div>
<div style="margin:10px 0px 20px 27px">
    <label for="txtDivision">Division:&nbsp;</label>
    <asp:TextBox ID="txtDivision" runat="server" Width="100px" />
</div>
<div style="margin:10px 0px 20px 25px">
    <uc6:ClientDivisionGrids ID="dgdClientDivision" runat="server" Height="300px" OnAfterClientSelected="OnClientSelected" OnAfterDivisionSelected="OnDivisionSelected" />
</div>
</asp:Content>

