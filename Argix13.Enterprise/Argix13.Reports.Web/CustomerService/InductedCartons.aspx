<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="InductedCartons.aspx.cs" Inherits="InductedCartons" %>
<%@ MasterType VirtualPath="~/Site.master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Setup" Runat="Server">
<script type="text/javascript">
    var daysback = -365, daysforward = 0, daysspread = 30;
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
<div style="margin:25px 0px 0px 39px">
    <label for="txtFrom">Pickups&nbsp;</label>
    <asp:TextBox ID="txtFrom" runat="server" Width="100px" AutoPostBack="true" OnTextChanged="OnFromToDateChanged" />
    <label for="txtTo">&nbsp;-&nbsp;</label>
    <asp:TextBox ID="txtTo" runat="server" Width="100px" AutoPostBack="true" OnTextChanged="OnFromToDateChanged" />
</div>
<div style="margin: 10px 0px 0px 50px">
    Terminal&nbsp;
    <asp:DropDownList ID="cboTerminal" runat="server" Width="192px" AutoPostBack="True" OnSelectedIndexChanged="OnTerminalSelected">
        <asp:ListItem Text="Jamesburg" Value="05" Selected="True" />
    </asp:DropDownList>
</div>
<div class="clear"></div>
<div style="margin:10px 0px 0px 10px">
    <div class="gridbox gridpickup">
        <div class="pickuptitle"><div class="pickuptitlename">Freight</div></div>
        <div class="clear"></div>
        <div id="divPickups" class="pickup">
            <asp:UpdatePanel ID="upnlFreight" runat="server" UpdateMode="Conditional" >
            <ContentTemplate>
                <asp:GridView ID="grdFreight" runat="server" Width="100%" AutoGenerateColumns="false" DataSourceID="odsFreight" DataKeyNames="BLNumber" AllowSorting="True" OnSelectedIndexChanged="OnFreightSelected">
                    <Columns>
                        <asp:CommandField HeaderStyle-Width="16px" ButtonType="Image" ShowSelectButton="True" SelectImageUrl="~/Content/Images/select.gif" />
                        <asp:BoundField DataField="ShipperNumber" HeaderText="Shipper#" HeaderStyle-Width="72px" SortExpression="ShipperNumber" />
                        <asp:BoundField DataField="ShipperName" HeaderText="Shipper" HeaderStyle-Width="144px" SortExpression="ShipperName" HtmlEncode="False" />
                        <asp:BoundField DataField="BLNumber" HeaderText="BLNumber" HeaderStyle-Width="72px" SortExpression="BLNumber" />
                        <asp:BoundField DataField="Cartons" HeaderText="Cartons" HeaderStyle-Width="48px" SortExpression="Cartons" />
                        <asp:BoundField DataField="CarrierNumber" HeaderText="Carrier#" HeaderStyle-Width="72px" SortExpression="CarrierNumber" />
                        <asp:BoundField DataField="TrailerNumber" HeaderText="Trailer#" HeaderStyle-Width="72px" SortExpression="TrailerNumber" />
                        <asp:BoundField DataField="Started" HeaderText="Started" HeaderStyle-Width="120px" DataFormatString="{0:yyyy-MM-dd}" HtmlEncode="False" />
                        <asp:BoundField DataField="Stopped" HeaderText="Stopped" HeaderStyle-Width="120px" DataFormatString="{0:yyyy-MM-dd}" HtmlEncode="False" />
                        <asp:BoundField DataField="Imported" HeaderText="Imported" HeaderStyle-Width="120px" DataFormatString="{0:yyyy-MM-dd}" HtmlEncode="False" />
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsFreight" runat="server" TypeName="Argix.EnterpriseRGateway" SelectMethod="GetInductedFreight" EnableCaching="true" CacheExpirationPolicy="Sliding" CacheDuration="300" >
                    <SelectParameters>
                        <asp:ControlParameter Name="startImportedDate" ControlID="ddpFreight" PropertyName="FromDate" Type="DateTime" />
                        <asp:ControlParameter Name="endImportedDate" ControlID="ddpFreight" PropertyName="ToDate" Type="DateTime" />
                        <asp:ControlParameter Name="terminalCode" ControlID="cboTerminal" PropertyName="SelectedValue" Type="string" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddpFreight" EventName="DateTimeChanged" />
                <asp:AsyncPostBackTrigger ControlID="cboTerminal" EventName="SelectedIndexChanged" />
            </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</div>
</asp:Content>

