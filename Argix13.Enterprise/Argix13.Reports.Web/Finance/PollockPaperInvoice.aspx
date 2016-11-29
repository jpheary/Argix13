<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="PollockPaperInvoice.aspx.cs" Inherits="PollockPaperInvoice" %>
<%@ MasterType VirtualPath="~/Site.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Setup" Runat="Server">
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
<div>
    <div style="margin:50px 0px 0px 50px">
        <label for="txtFrom">Dates&nbsp;</label>
        <asp:TextBox ID="txtFrom" runat="server" Width="100px" />
        <label for="txtTo">&nbsp;-&nbsp;</label>
        <asp:TextBox ID="txtTo" runat="server" Width="100px" />    
    </div>
</div>
</asp:Content>

