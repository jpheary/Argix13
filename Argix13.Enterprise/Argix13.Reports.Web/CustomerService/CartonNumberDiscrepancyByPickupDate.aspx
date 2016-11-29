<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="CartonNumberDiscrepancyByPickupDate.aspx.cs" Inherits="CartonNumberDiscrepancyByPickupDate" %>
<%@ Register Src="~/ClientVendorGrids.ascx" TagName="ClientVendorGrids" TagPrefix="uc2" %>
<%@ MasterType VirtualPath="~/Site.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Setup" Runat="Server">
<script type="text/javascript">
    $(function () {
        $("#<%=txtPickupDate.ClientID %>").datepicker({ minDate: -180, maxDate: +0 });
    });
</script>
<div>
    <div style="margin:50px 0px 0px 25px">
        <label for="txtPickupDate">Pickup Date&nbsp;</label>
        <asp:TextBox ID="txtPickupDate" runat="server" Width="100px" AutoPostBack="true" OnTextChanged="OnPickupDateChanged" />
    </div>
</div>
<div style="margin:10px 0px 0px 10px">
    <uc2:ClientVendorGrids ID="dgdClientVendor" runat="server" Height="350px" OnAfterClientSelected="OnClientSelected" OnAfterVendorSelected="OnVendorSelected" />
</div>
</asp:Content>

