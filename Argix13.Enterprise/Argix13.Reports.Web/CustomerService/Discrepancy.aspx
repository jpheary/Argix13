<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Discrepancy.aspx.cs" Inherits="Discrepancy" %>
<%@ MasterType VirtualPath="~/Site.master" %>

<asp:Content ID="cSetup" ContentPlaceHolderID="Setup" Runat="Server">
<script type="text/javascript">
    $(function () {
        $("#<%=txtFrom.ClientID %>").datepicker({
            onClose: function (selectedDate) { $("#<%=txtTo.ClientID %>").datepicker("option", "minDate", selectedDate); }
        });
        $("#<%=txtTo.ClientID %>").datepicker({
            onClose: function (selectedDate) { $("#<%=txtFrom.ClientID %>").datepicker("option", "maxDate", selectedDate); }
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
    function scrollShippers(number) {
        var grd = document.getElementById('<%=grdShippers.ClientID %>');
        for (var i = 1; i < grd.rows.length; i++) {
            var cell = grd.rows[i].cells[1];
            if (cell.innerHTML.substr(0, number.length) == number) {
                var pnl = document.getElementById('divShippers');
                pnl.scrollTop = i * (grd.clientHeight / grd.rows.length);
                break;
            }
        }
    }
</script>
<div>
    <div style="margin:50px 0px 0px 25px">
        <label for="txtFrom">Pickups:&nbsp;</label>
        <asp:TextBox ID="txtFrom" runat="server" Width="100px" />
        <label for="txtTo">&nbsp;-&nbsp;</label>
        <asp:TextBox ID="txtTo" runat="server" Width="100px" />
    </div>
    <div style="margin:10px 0px 20px 27px">
        Freight:&nbsp;
        <asp:DropDownList ID="ddlFreightType" runat="server" Width="75px" AutoPostBack="True" OnSelectedIndexChanged="OnFreightTypeChanged">
            <asp:ListItem Text="Tsort" Value="Tsort" Selected="True" />
            <asp:ListItem Text="Returns" Value="Returns" />
        </asp:DropDownList>
        &nbsp;viewed by&nbsp;
        <asp:DropDownList ID="cboViewBy" runat="server" Width="75px">
            <asp:ListItem Text="Store" Selected="True" />
            <asp:ListItem Text="Zone" />
            <asp:ListItem Text="State" />
        </asp:DropDownList>    
    </div>
    <div class="gridcontrol">
        <div class="gridbox gridclient">
            <div class="clienttitle">
                <div class="clienttitlename">Clients</div>
                <div class="clienttitlesearch">
                    <asp:TextBox ID="txtFindClient" runat="server" ToolTip="Enter a client number... <press Enter>" AutoPostBack="True" OnTextChanged="OnClientSearch" />
                    <asp:ImageButton ID="imgFindClient" runat="server" ImageUrl="~/Content/Images/search.gif" ToolTip="Search for a client..." OnClick="OnFindClient" />
                </div>
            </div>
            <div class="clear"></div>
            <div id="divClients" class="client">
                <asp:GridView ID="grdClients" runat="server" Width="100%" AutoGenerateColumns="False" AllowSorting="True" DataSourceID="odsClients" DataKeyNames="ClientNumber,TerminalCode" OnSelectedIndexChanged="OnClientSelected">
                    <Columns>
                        <asp:CommandField HeaderStyle-Width="16px" ButtonType="Image" ShowSelectButton="True" SelectImageUrl="~/Content/Images/select.gif" />
                        <asp:BoundField DataField="ClientNumber" HeaderText="Num" HeaderStyle-Width="48px" SortExpression="ClientNumber" />
                        <asp:BoundField DataField="DivisionNumber" HeaderText="Div" HeaderStyle-Width="48px" SortExpression="DivisionNumber" />
                        <asp:BoundField DataField="ClientName" HeaderText="Name" SortExpression="ClientName" HtmlEncode="False" />
                        <asp:BoundField DataField="TerminalCode" HeaderText="Term" HeaderStyle-Width="48px" SortExpression="TerminalCode" />
                    </Columns>
                </asp:GridView>
                <asp:HiddenField ID="hfClientDivision" runat="server" Value="" />
                <asp:HiddenField ID="hfClientActiveOnly" runat="server" Value="false" />
            </div>
        </div>
        <div class="gridbox gridshipper">
            <div class="shippertitle">
                <div class="shippertitlename">
                    <asp:DropDownList ID="ddlShipper" runat="server" Width="75px" ToolTip="Shipper" Enabled="false">
                        <asp:ListItem Text="Vendors" Value="0" Selected="True" />
                        <asp:ListItem Text="Agents" Value="1" />
                    </asp:DropDownList>                    
                </div>
                <div class="shippertitlesearch">
                    <asp:TextBox ID="txtFindShipper" runat="server" ToolTip="Enter a shipper number... <press Enter>" AutoPostBack="True" OnTextChanged="OnShipperSearch" />
                    <asp:ImageButton ID="imgFindShipper" runat="server" ImageUrl="~/Content/Images/search.gif" ToolTip="Search for a shipper..." OnClick="OnFindShipper" />
                </div>
            </div>
            <div class="clear"></div>
            <div id="divShippers" class="shipper">
                <asp:GridView ID="grdShippers" runat="server" Width="100%" AutoGenerateColumns="False" AllowSorting="True" DataSourceID="odsShippers">
                    <Columns>
                        <asp:TemplateField HeaderText="" HeaderStyle-Width="24px" >
                            <ItemTemplate><asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="true" OnCheckedChanged="OnShipperCheckedChanged" /></ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ShipperNumber" HeaderText="Num" HeaderStyle-Width="60px" SortExpression="ShipperNumber" />
                        <asp:BoundField DataField="ShipperName" HeaderText="Name" SortExpression="ShipperName" HtmlEncode="False" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</div>
<asp:ObjectDataSource ID="odsClients" runat="server" TypeName="Argix.EnterpriseRGateway" SelectMethod="GetClients" EnableCaching="true" CacheExpirationPolicy="Sliding" CacheDuration="900">
    <SelectParameters>
        <asp:Parameter Name="number" DefaultValue="" ConvertEmptyStringToNull="true" Type="String" />
        <asp:ControlParameter Name="division" ControlID="hfClientDivision" PropertyName="Value" DefaultValue="" ConvertEmptyStringToNull="true" Type="String" />
        <asp:ControlParameter Name="activeOnly" ControlID="hfClientActiveOnly" PropertyName="Value" DefaultValue="False" Type="Boolean" />
    </SelectParameters>
</asp:ObjectDataSource>
<asp:ObjectDataSource ID="odsShippers" runat="server" TypeName="Argix.EnterpriseRGateway" SelectMethod="GetShippers" EnableCaching="true" CacheExpirationPolicy="Sliding" CacheDuration="900">
    <SelectParameters>
        <asp:ControlParameter Name="freightType" ControlID="ddlShipper" DefaultValue="0" PropertyName="SelectedValue" Type="Object" />
        <asp:ControlParameter Name="clientNumber" ControlID="grdClients" PropertyName="SelectedDataKey.Values[0]" DefaultValue="000" Type="String" />
        <asp:ControlParameter Name="clientTerminal" ControlID="grdClients" PropertyName="SelectedDataKey.Values[1]" Type="String" />
    </SelectParameters>
</asp:ObjectDataSource>
</asp:Content>

