<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="RequestedDeliveryDate.aspx.cs" Inherits="RequestedDeliveryDate" %>
<%@ MasterType VirtualPath="~/Site.master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Setup" Runat="Server">
<script type="text/javascript">
    $(function () {
        $("#<%=txtRequestDate.ClientID %>").datepicker({ minDate: -7, maxDate: +7 });
    });
</script>
<div style="margin:50px 0px 0px 25px">
    <label for="txtRequestDate">Requested Date&nbsp;</label>
    <asp:TextBox ID="txtRequestDate" runat="server" Width="100px" />
</div>
</asp:Content>