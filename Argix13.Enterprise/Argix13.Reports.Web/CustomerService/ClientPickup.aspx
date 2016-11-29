<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ClientPickup.aspx.cs" Inherits="ClientPickup" %>
<%@ Register Src="~/ClientVendorGrids.ascx" TagName="ClientVendorGrids" TagPrefix="uc2" %>
<%@ MasterType VirtualPath="~/Site.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Setup" Runat="Server">
<script type="text/javascript">
    var daysback = -180, daysforward = 0, daysspread = 48;
    $(function () {
        $("#<%=txtFrom.ClientID %>").datepicker({
            minDate: daysback, maxDate: 0, defaultDate: 0,
            onClose: function (selectedDate, instance) {
                $("#<%=txtTo.ClientID %>").datepicker("option", "minDate", selectedDate);

                var date = $.datepicker.parseDate("mm/dd/yy", selectedDate, instance.settings);
                date.setDate(date.getDate() + daysspread);
                var todate = $("#<%=txtTo.ClientID %>").datepicker("getDate");
                if (todate > date) $("#<%=txtTo.ClientID %>").datepicker("setDate", date);
            }
        });
        $("#<%=txtTo.ClientID %>").datepicker({
            minDate: 0, maxDate: daysforward, defaultDate: 0,
            onClose: function (selectedDate, instance) {
                $("#<%=txtFrom.ClientID %>").datepicker("option", "maxDate", selectedDate);

                var date = $.datepicker.parseDate("mm/dd/yy", selectedDate, instance.settings);
                date.setDate(date.getDate() - daysspread);
                var fromdate = $("#<%=txtFrom.ClientID %>").datepicker("getDate");
                if (fromdate < date) $("#<%=txtFrom.ClientID %>").datepicker("setDate", date);
            }
        });
    });
</script>
<div style="margin:50px 0px 0px 39px">
    <label for="txtFrom">Pickups&nbsp;</label>
    <asp:TextBox ID="txtFrom" runat="server" Width="100px" AutoPostBack="true" OnTextChanged="OnFromToDateChanged" />
    <label for="txtTo">&nbsp;-&nbsp;</label>
    <asp:TextBox ID="txtTo" runat="server" Width="100px" AutoPostBack="true" OnTextChanged="OnFromToDateChanged" />
</div>
<div style="margin:10px 0px 0px 84px">
    <asp:DropDownList ID="cboType" runat="server" Width="96px">
        <asp:ListItem Text="Summary" Value="Summary" Selected="True" />
        <asp:ListItem Text="Detail" Value="Detail" />
    </asp:DropDownList>
    &nbsp;report sorted by&nbsp;
    <asp:DropDownList ID="cboSortBy" runat="server" Width="60px">
        <asp:ListItem Text="Store" Value="Store" Selected="True" />
        <asp:ListItem Text="Zone" Value="Zone" />
    </asp:DropDownList>
</div>
<div style="margin:2% 0% 0% 60%; text-align:left">
    <asp:CheckBox ID="chkAllVendors" runat="server" Width="300px" Text="&nbsp;Pickups for all vendors" TextAlign="Right" Checked="true" AutoPostBack="True" OnCheckedChanged="OnAllVendorsChecked" />
</div>
<div style="margin:5px 0px 0px 10px">
    <uc2:ClientVendorGrids ID="dgdClientVendor" runat="server" Height="120px" ClientsEnabled="true" VendorsEnabled="false" OnAfterClientSelected="OnClientSelected" OnAfterVendorSelected="OnVendorSelected" />
</div>
<div class="clear"></div>
<div style="margin:10px 0px 0px 10px">
    <div class="gridbox gridpickup">
        <div class="pickuptitle"><div class="pickuptitlename">Pickups</div></div>
        <div class="clear"></div>
        <div id="divPickups" class="pickup">
            <asp:UpdatePanel ID="upnlPickups" runat="server" UpdateMode="Conditional" >
            <ContentTemplate>
                <asp:GridView ID="grdPickups" runat="server" Width="100%" AutoGenerateColumns="False" DataSourceID="odsPickups" DataKeyNames="PickupID,TerminalCode,VendorNumber,VendorName,PUDate,PUNumber,ManifestNumbers,TrailerNumbers" AllowSorting="True">
                    <Columns>
                        <asp:TemplateField HeaderText="" HeaderStyle-Width="24px" >
                            <HeaderTemplate><asp:CheckBox ID="chkAll" runat="server" Enabled="true" AutoPostBack="true" OnCheckedChanged="OnAllPickupsSelected"/></HeaderTemplate>
                            <ItemTemplate><asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="true" OnCheckedChanged="OnPickupSelected"/></ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="PickupID" HeaderText="ID" HeaderStyle-Width="24px" Visible="false" />
                        <asp:BoundField DataField="TerminalCode" HeaderText="Term" HeaderStyle-Width="24px" Visible="false" />
                        <asp:BoundField DataField="VendorNumber" HeaderText="Vendor#" HeaderStyle-Width="60px" SortExpression="VendorNumber" />
                        <asp:BoundField DataField="VendorName" HeaderText="Vendor" ItemStyle-Wrap="False" SortExpression="VendorName" HtmlEncode="False" />
                        <asp:BoundField DataField="PUDate" HeaderText="Pickup" HeaderStyle-Width="120px" SortExpression="PUDate" DataFormatString="{0:yyyy-MM-dd}" HtmlEncode="False" />
                        <asp:BoundField DataField="PUNumber" HeaderText="#" HeaderStyle-Width="48px" SortExpression="PUNumber" />
                        <asp:BoundField DataField="ManifestNumbers" HeaderText="Manifests" HeaderStyle-Width="144px" ItemStyle-Width="144px" ItemStyle-Wrap="true" NullDisplayText=" " />
                        <asp:BoundField DataField="TrailerNumbers" HeaderText="Trailers" HeaderStyle-Width="144px" NullDisplayText=" " />
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="txtFrom" EventName="TextChanged" />
                <asp:AsyncPostBackTrigger ControlID="txtTo" EventName="TextChanged" />
                <asp:AsyncPostBackTrigger ControlID="chkAllVendors" EventName="CheckedChanged" />
                <asp:AsyncPostBackTrigger ControlID="dgdClientVendor" EventName="AfterClientSelected" />
                <asp:AsyncPostBackTrigger ControlID="dgdClientVendor" EventName="AfterVendorSelected" />
            </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</div>
<asp:ObjectDataSource ID="odsPickups" runat="server" TypeName="Argix.EnterpriseRGateway" SelectMethod="GetPickups" EnableCaching="true" CacheExpirationPolicy="Sliding" CacheDuration="300" >
    <SelectParameters>
        <asp:ControlParameter Name="client" ControlID="dgdClientVendor" PropertyName="ClientNumber" Type="string" />
        <asp:ControlParameter Name="division" ControlID="dgdClientVendor" PropertyName="ClientDivsionNumber" Type="string" />
        <asp:ControlParameter Name="startDate" ControlID="txtFrom" PropertyName="Text" Type="string" />
        <asp:ControlParameter Name="endDate" ControlID="txtTo" PropertyName="Text" Type="string" />
        <asp:ControlParameter Name="vendor" ControlID="dgdClientVendor" PropertyName="VendorNumber" Type="string" />
    </SelectParameters>
</asp:ObjectDataSource>
</asp:Content>

