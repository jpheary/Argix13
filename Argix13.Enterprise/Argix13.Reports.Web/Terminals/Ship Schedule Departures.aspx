<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Ship Schedule Departures.aspx.cs" Inherits="ShipScheduleDepartures" %>
<%@ MasterType VirtualPath="~/Site.master" %>

<asp:Content ID="cSetup" ContentPlaceHolderID="Setup" Runat="Server">
<script type="text/javascript">
    $(function () {
        $("#<%=txtDepartureDate.ClientID %>").datepicker({ minDate: -30, maxDate: +30 });
    });
</script>
<div>
    <div style="margin:50px 0px 0px 25px">
        <label for="txtDepartureDate">Departure Date&nbsp;</label>
        <asp:TextBox ID="txtDepartureDate" runat="server" Width="100px" />
    </div>
</div>
</asp:Content>

