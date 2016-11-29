<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Ship Schedule OFDs.aspx.cs" Inherits="ShipScheduleOFDs" %>
<%@ MasterType VirtualPath="~/Site.master" %>

<asp:Content ID="cSetup" ContentPlaceHolderID="Setup" Runat="Server">
<script type="text/javascript">
    $(function () {
        $("#<%=txtOFDDate.ClientID %>").datepicker({ minDate: -30, maxDate: +30 });
    });
</script>
<div>
    <div style="margin:50px 0px 0px 25px">
        <label for="txtOFDDate">OFD Date&nbsp;</label>
        <asp:TextBox ID="txtOFDDate" runat="server" Width="100px" />
    </div>
</div>
</asp:Content>
