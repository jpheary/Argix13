<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="CartonsCap.aspx.cs" Inherits="CartonsCap" %>
<%@ MasterType VirtualPath="~/Site.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Setup" Runat="Server">
<script type="text/javascript">
    $(function () {
        $("#<%=txtDate.ClientID %>").datepicker({ minDate: -31, maxDate: +14 });
    });
</script>
<div>
    <div style="margin:50px 0px 0px 50px">
        <label for="txtDate">Date&nbsp;</label>
        <asp:TextBox ID="txtDate" runat="server" Width="100px" />
    </div>
</div>
</asp:Content>

