<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="InboundPalletShipmentTracking.aspx.cs" Inherits="InboundPalletShipmentTracking" %>
<%@ MasterType VirtualPath="~/Site.master" %>

<asp:Content ID="cSetup" ContentPlaceHolderID="Setup" Runat="Server">
<script type="text/javascript">
    $(function () {
        $("#<%=txtFrom.ClientID %>").datepicker({
            defaultDate: "1", onClose: function (selectedDate) { $("#<%=txtTo.ClientID %>").datepicker("option", "minDate", selectedDate); }
        });
        $("#<%=txtTo.ClientID %>").datepicker({
            defaultDate: "1", onClose: function (selectedDate) { $("#<%=txtFrom.ClientID %>").datepicker("option", "maxDate", selectedDate); }
        });
    });
  </script>
<div style="margin:50px 0px 20px 25px">
    <label for="cboClient" style="margin:0px 35px 0px 0px">Client&nbsp;</label>
    <asp:DropDownList id="cboClient" runat="server" Width="300px" DataTextField="ClientName" DataValueField="ClientNumber" />
</div>
<div style="margin:20px 0px 0px 25px">
    <label for="txtFrom">Pickups from&nbsp;</label>
    <asp:TextBox ID="txtFrom" runat="server" Width="100px" />
    <label for="txtTo">&nbsp;to&nbsp;</label>
    <asp:TextBox ID="txtTo" runat="server" Width="100px" />    
</div>
</asp:Content>

