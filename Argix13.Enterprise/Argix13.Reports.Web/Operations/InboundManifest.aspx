<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="InboundManifest.aspx.cs" Inherits="InboundManifest" %>
<%@ MasterType VirtualPath="~/Site.master" %>

<asp:Content ID="cSetup" ContentPlaceHolderID="Setup" Runat="Server">
<script type="text/javascript">
    var daysback = -61, daysforward = 0, daysspread = 7;
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
    function scrollClients(number) {
        var grd = document.getElementById('<%=grdClients.ClientID %>');
        for (var i = 1; i < grd.rows.length; i++) {
            var cell = grd.rows[i].cells[1];
            if (cell.innerHTML.substr(0, number.length) == number) {
                var pnl = document.getElementById('divClients');
                pnl.scrollTop = i * (grd.clientHeight / grd.rows.length);
                break;
            }
        }
    }
    function scrollVendors(number) {
        var grd = document.getElementById('<%=grdVendorLog.ClientID %>');
        for (var i = 1; i < grd.rows.length; i++) {
            var cell = grd.rows[i].cells[1];
            if (cell.innerHTML.substr(0, number.length) == number) {
                var pnl = document.getElementById('divVendors');
                pnl.scrollTop = i * (grd.clientHeight / grd.rows.length);
                break;
            }
        }
    }
</script>
<div style="margin:50px 0px 0px 25px">
    <label for="txtFrom">Pickups&nbsp;</label>
    <asp:TextBox ID="txtFrom" runat="server" Width="100px" AutoPostBack="true" OnTextChanged="OnDateChanged" />
    <label for="txtTo">&nbsp;to&nbsp;</label>
    <asp:TextBox ID="txtTo" runat="server" Width="100px" AutoPostBack="true" OnTextChanged="OnDateChanged" />
</div>
<div style="margin:20px 0px 20px 25px">
    <div class="gridcontrol">
        <div class="gridbox gridclient">
            <div class="clienttitle">
                <div class="clienttitlename">Clients</div>
                <div class="clienttitlesearch">
                    <asp:TextBox ID="txtFindClient" runat="server" Width="75px" BorderStyle="Inset" BorderWidth="1px" ToolTip="Enter a client number... <press Enter>" AutoPostBack="True" OnTextChanged="OnClientSearch" />
                    <asp:ImageButton ID="imgFindClient" runat="server" Height="16px" ImageAlign="Middle" ImageUrl="~/Content/Images/search.gif" ToolTip="Search for a client..." OnClick="OnFindClient" />
                </div>
            </div>
            <div class="clear"></div>
            <div id="divClients" class="client">
                <asp:GridView ID="grdClients" runat="server" Width="100%" AutoGenerateColumns="False" AllowSorting="True" DataSourceID="odsClients" DataKeyNames="ClientNumber,TerminalCode" OnSelectedIndexChanged="OnClientSelected">
                    <Columns>
                        <asp:CommandField HeaderStyle-Width="16px" ButtonType="Image" ShowSelectButton="True" SelectImageUrl="~/Content/Images/select.gif" />
                        <asp:BoundField DataField="ClientNumber" HeaderText="Num" HeaderStyle-Width="50px" SortExpression="ClientNumber" />
                        <asp:BoundField DataField="DivisionNumber" HeaderText="Div" HeaderStyle-Width="50px" SortExpression="DivisionNumber" />
                        <asp:BoundField DataField="ClientName" HeaderText="Name" SortExpression="ClientName" HtmlEncode="False" />
                        <asp:BoundField DataField="TerminalCode" HeaderText="Term" HeaderStyle-Width="50px" SortExpression="TerminalCode" />
                    </Columns>
                </asp:GridView>
                <asp:HiddenField ID="hfClientDivision" runat="server" Value="" />
                <asp:HiddenField ID="hfClientActiveOnly" runat="server" Value="false" />
            </div>
        </div>
        <div class="gridbox gridvendor">
            <div class="vendortitle">
                <div class="vendortitlename">Vendor Log</div>
                <div class="vendortitlesearch">
                    <asp:TextBox ID="txtFindLogEntry" runat="server" Width="75px" BorderStyle="Inset" BorderWidth="1px" ToolTip="Enter a vendor log number... <press Enter>" AutoPostBack="True" OnTextChanged="OnVendorLogSearch"></asp:TextBox>
                    <asp:ImageButton ID="imgFindLogEntry" runat="server" Height="16px" ImageAlign="Middle" ImageUrl="~/Content/Images/search.gif" ToolTip="Search for a vendor log entry..." OnClick="OnFindVendorLogEntry" />&nbsp;
                </div>
            </div>
            <div class="clear"></div>
            <div id="divVendors" class="vendor">
                <asp:GridView ID="grdVendorLog" runat="server" Width="100%" AutoGenerateColumns="False" DataSourceID="odsVendorLog" DataKeyNames="ID,PickupDate,PickupNumber" AllowSorting="True">
                    <Columns>
                        <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" >
                            <ItemTemplate><asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="true" OnCheckedChanged="OnVendorLogCheckedChanged"/></ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ID" HeaderText="ID" HeaderStyle-Width="85px" SortExpression="ID" />
                        <asp:BoundField DataField="ShipFrom" HeaderText="Shipper#" HeaderStyle-Width="75px" SortExpression="ShipFrom" />
                        <asp:BoundField DataField="ShipFromName" HeaderText="Shipper Name" SortExpression="ShipFromName" />
                        <asp:BoundField DataField="PickupDate" HeaderText="Pickup" HeaderStyle-Width="150px" SortExpression="PickupDate" DataFormatString="{0:MM/dd/yy HH:mm tt}" HtmlEncode="False" />
                        <asp:BoundField DataField="PickupNumber" HeaderText="#" ItemStyle-Width="50px" SortExpression="PickupNumber" />
                    </Columns>
                </asp:GridView>
                <asp:HiddenField ID="hfStartDate" runat="server" Value="" />
                <asp:HiddenField ID="hfEndDate" runat="server" Value="" />
            </div>
        </div>
    </div>
</div>
<asp:ObjectDataSource ID="odsClients" runat="server" TypeName="Argix.EnterpriseRGateway" SelectMethod="GetClients" EnableCaching="true" CacheExpirationPolicy="Sliding" CacheDuration="900">
    <SelectParameters>
        <asp:Parameter Name="number" DefaultValue="" ConvertEmptyStringToNull="true" Type="String" />
        <asp:ControlParameter Name="division" ControlID="hfClientDivision" PropertyName="Value" DefaultValue="" ConvertEmptyStringToNull="true" Type="String" />
        <asp:ControlParameter Name="activeOnly" ControlID="hfClientActiveOnly" PropertyName="Value" DefaultValue="True" Type="Boolean" />
    </SelectParameters>
</asp:ObjectDataSource>
<asp:ObjectDataSource ID="odsVendorLog" runat="server" TypeName="Argix.EnterpriseRGateway" SelectMethod="GetVendorLog" EnableCaching="true" CacheExpirationPolicy="Sliding" CacheDuration="180" >
    <SelectParameters>
        <asp:ControlParameter ControlID="grdClients" Name="client" PropertyName="SelectedDataKey.Values[0]" Type="String" />
        <asp:ControlParameter ControlID="grdClients" Name="clientDivision" PropertyName="SelectedDataKey.Values[1]" Type="String" />
        <asp:ControlParameter ControlID="hfStartDate" Name="startDate" PropertyName="Value" Type="DateTime" />
        <asp:ControlParameter ControlID="hfEndDate" Name="endDate" PropertyName="Value" Type="DateTime" />
    </SelectParameters>
</asp:ObjectDataSource>
</asp:Content>

