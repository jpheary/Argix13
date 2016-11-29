<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Shift.aspx.cs" Inherits="Shift" %>
<%@ MasterType VirtualPath="~/Site.master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Setup" Runat="Server">
<script type="text/javascript">
    $(function () {
        $("#<%=txtShiftDate.ClientID %>").datepicker({ minDate: -90, maxDate: +0 });
    });
</script>
<div style="margin:50px 0px 0px 25px">
    <label for="txtShiftDate">Shift Date&nbsp;</label>
    <asp:TextBox ID="txtShiftDate" runat="server" Width="100px" AutoPostBack="true" OnTextChanged="OnShiftDateChanged" />
</div>
<div style="margin:10px 0px 0px 50px">
    Terminal&nbsp;
    <asp:DropDownList ID="cboTerminal" runat="server" Width="288px" DataSourceID="odsTerminals" DataTextField="Description" DataValueField="Number" AutoPostBack="True" OnSelectedIndexChanged="OnTerminalSelected" />
</div>
<div style="margin:10px 0px 0px 50px">
    Shift&nbsp;
    <asp:DropDownList id="cboShift" runat="server" Width="96px" DataSourceID="odsShifts" DataTextField="NUMBER" DataValueField="NUMBER" AutoPostBack="True" OnSelectedIndexChanged="OnShiftChanged" />
</div>
<div style="margin:10px 0px 0px 50px">
    Freight Type&nbsp;
    <asp:DropDownList ID="cboType" runat="server" Width="134px">
        <asp:ListItem Text="Tsort" Value="Tsort" Selected="True" />
        <asp:ListItem Text="Returns" Value="Returns" />
        <asp:ListItem Text="Both" Value="Both" />
    </asp:DropDownList>
</div>
<asp:ObjectDataSource ID="odsTerminals" runat="server" TypeName="Argix.EnterpriseRGateway" SelectMethod="GetArgixTerminals" EnableCaching="true" CacheExpirationPolicy="Sliding" CacheDuration="900" />
<asp:ObjectDataSource ID="odsShifts" runat="server" TypeName="Argix.EnterpriseRGateway" SelectMethod="GetShifts" EnableCaching="true" CacheExpirationPolicy="Sliding" CacheDuration="900" >
    <SelectParameters>
        <asp:ControlParameter Name="terminal" ControlID="cboTerminal" PropertyName="SelectedValue" Type="String" />
        <asp:ControlParameter Name="date" ControlID="txtShiftDate" PropertyName="Text" Type="String" />
    </SelectParameters>
</asp:ObjectDataSource>
</asp:Content>

