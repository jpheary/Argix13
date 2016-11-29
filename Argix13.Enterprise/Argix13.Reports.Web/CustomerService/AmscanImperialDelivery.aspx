<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="AmscanImperialDelivery.aspx.cs" Inherits="AmscanImperialDelivery" %>
<%@ MasterType VirtualPath="~/Site.master" %>

<asp:Content ID="cSetup" ContentPlaceHolderID="Setup" Runat="Server">
<script type="text/javascript">
    $(function () {
        $("#<%=txtDeliveryDate.ClientID %>").datepicker({ minDate: -7, maxDate: +7 });
    });
</script>
<div style="margin:50px 0px 0px 25px">
    <label for="txtDeliveryDate">Delivery Date&nbsp;</label>
    <asp:TextBox ID="txtDeliveryDate" runat="server" Width="100px" />
</div>
</asp:Content>

