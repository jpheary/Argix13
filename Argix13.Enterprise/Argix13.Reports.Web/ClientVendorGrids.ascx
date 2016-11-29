<%@ Control Language="C#" AutoEventWireup="true" ClassName="ClientVendorGrids" CodeFile="ClientVendorGrids.ascx.cs" Inherits="ClientVendorGrids" EnableTheming="true" %>

<div class="gridcontrol">
    <div class="gridbox gridclient">
        <div class="clienttitle">
            <div class="clienttitlename">Clients</div>
            <div class="clienttitlesearch">
                <asp:UpdatePanel ID="upnlClientsHeader" runat="server" UpdateMode="Always" >
                <ContentTemplate>
                    <asp:TextBox ID="txtFindClient" runat="server" ToolTip="Enter a client number... <press Enter>" AutoPostBack="True" OnTextChanged="OnClientSearch" />
                    <asp:ImageButton ID="imgFindClient" runat="server" ImageUrl="~/Content/Images/search.gif" ToolTip="Search for a client..." OnClick="OnFindClient" />
                </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="clear"></div>
        <div id="divClients" class="client">
            <asp:UpdatePanel ID="upnlClients" runat="server" UpdateMode="Conditional" >
            <ContentTemplate>
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
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="txtFindClient" EventName="TextChanged" />
                <asp:AsyncPostBackTrigger ControlID="imgFindClient" EventName="Click" />
            </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <div class="gridbox gridvendor">
        <div class="vendortitle">
            <div class="vendortitlename">Vendors</div>
            <div class="vendortitlesearch">
                <asp:UpdatePanel ID="upnlVendorsHeader" runat="server" UpdateMode="Always" >
                <ContentTemplate>
                    <asp:TextBox ID="txtFindVendor" runat="server" ToolTip="Enter a vendor number... <press Enter>" AutoPostBack="True" OnTextChanged="OnVendorSearch"></asp:TextBox>
                    <asp:ImageButton ID="imgFindVendor" runat="server" ImageUrl="~/Content/Images/search.gif" ToolTip="Search for a vendor..." OnClick="OnFindVendor" />&nbsp;
                </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdateProgress ID="upgVendors" runat="server" AssociatedUpdatePanelID="upnlVendors"><ProgressTemplate>updating...</ProgressTemplate></asp:UpdateProgress>
            </div>
        </div>
        <div class="clear"></div>
        <div id="divVendors" class="vendor">
            <asp:UpdatePanel ID="upnlVendors" runat="server" UpdateMode="Conditional" >
            <ContentTemplate>
                <asp:GridView ID="grdVendors" runat="server" Width="100%" AutoGenerateColumns="False" AllowSorting="True" DataSourceID="odsVendors" OnSelectedIndexChanged="OnVendorSelected">
                    <Columns>
                        <asp:CommandField ButtonType="Image" HeaderStyle-Width="16px" ShowSelectButton="True" SelectImageUrl="~/Content/Images/select.gif" />
                        <asp:BoundField DataField="VendorNumber" HeaderText="#" HeaderStyle-Width="60px" SortExpression="VendorNumber" />
                        <asp:BoundField DataField="VendorName" HeaderText="Name" SortExpression="VendorName" HtmlEncode="False" />
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="grdClients" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="txtFindVendor" EventName="TextChanged" />
                <asp:AsyncPostBackTrigger ControlID="imgFindVendor" EventName="Click" />
            </Triggers>
            </asp:UpdatePanel>
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
<asp:ObjectDataSource ID="odsVendors" runat="server" TypeName="Argix.EnterpriseRGateway" SelectMethod="GetVendors" EnableCaching="true" CacheExpirationPolicy="Sliding" CacheDuration="900" >
    <SelectParameters>
        <asp:ControlParameter ControlID="grdClients" Name="clientNumber" PropertyName="SelectedDataKey.Values[0]" DefaultValue="000" Type="String" />
        <asp:ControlParameter ControlID="grdClients" Name="clientTerminal" PropertyName="SelectedDataKey.Values[1]" Type="String" />
    </SelectParameters>
</asp:ObjectDataSource>
<script type="text/javascript">
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
        var grd = document.getElementById('<%=grdVendors.ClientID %>');
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
