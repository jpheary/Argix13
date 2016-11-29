<%@ Control Language="C#" AutoEventWireup="true" ClassName="ClientDivisionGrids" CodeFile="ClientDivisionGrids.ascx.cs" Inherits="ClientDivisionGrids" EnableTheming="true" %>

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
                        <asp:BoundField DataField="ClientNumber" HeaderText="Num" HeaderStyle-Width="48px" SortExpression="ClientNumber" />
                        <asp:BoundField DataField="ClientName" HeaderText="Name" SortExpression="ClientName" HtmlEncode="False" />
                    </Columns>
                </asp:GridView>
                <asp:HiddenField ID="hfClientDivision" runat="server" Value="01" />
            </ContentTemplate>
                <Triggers>
                <asp:AsyncPostBackTrigger ControlID="txtFindClient" EventName="TextChanged" />
                <asp:AsyncPostBackTrigger ControlID="imgFindClient" EventName="Click" />
            </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <div class="gridbox griddivision">
        <div class="divisiontitle">
            <div class="divisiontitlename">Divisions</div>
            <div class="divisiontitlesearch">
                <asp:UpdatePanel ID="upnlDivisionsHeader" runat="server" UpdateMode="Always" >
                <ContentTemplate>
                    <asp:TextBox ID="txtFindDivision" runat="server" ToolTip="Enter a division number... <press Enter>" AutoPostBack="True" OnTextChanged="OnDivisionSearch"></asp:TextBox>
                    <asp:ImageButton ID="imgFindDivision" runat="server" ImageUrl="~/Content/Images/search.gif" ToolTip="Search for a division..." OnClick="OnFindDivision" />&nbsp;
                </ContentTemplate>
                </asp:UpdatePanel>                    
            </div>
        </div>
        <div class="clear"></div>
        <div id="divDivisions" class="division">
            <asp:UpdatePanel ID="upnlDivisions" runat="server" ChildrenAsTriggers="true" RenderMode="Block" UpdateMode="Conditional" >
            <ContentTemplate>
                <asp:GridView ID="grdDivisions" runat="server" Width="100%" AutoGenerateColumns="False" AllowSorting="True" DataSourceID="odsDivisions" OnSelectedIndexChanged="OnDivisionSelected">
                    <Columns>
                        <asp:CommandField ButtonType="Image" HeaderStyle-Width="16px" ShowSelectButton="True" SelectImageUrl="~/Content/Images/select.gif" />
                        <asp:BoundField DataField="DivisionNumber" HeaderText="Num" HeaderStyle-Width="60px" SortExpression="DivisionNumber" />
                        <asp:BoundField DataField="ClientName" HeaderText="Name" SortExpression="ClientName" />
                    </Columns>
                </asp:GridView>
                <asp:HiddenField ID="hfClientActiveOnly" runat="server" Value="false" />
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="grdClients" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="txtFindDivision" EventName="TextChanged" />
                <asp:AsyncPostBackTrigger ControlID="imgFindDivision" EventName="Click" />
            </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</div>
<asp:ObjectDataSource ID="odsClients" runat="server" TypeName="Argix.EnterpriseRGateway" SelectMethod="GetClients" EnableCaching="true" CacheExpirationPolicy="Sliding" CacheDuration="900">
    <SelectParameters>
        <asp:Parameter Name="number" DefaultValue="" ConvertEmptyStringToNull="true" Type="String" />
        <asp:ControlParameter Name="division" ControlID="hfClientDivision" PropertyName="Value" DefaultValue="01" ConvertEmptyStringToNull="true" Type="String" />
        <asp:Parameter Name="activeOnly" DefaultValue="False" Type="Boolean" />
    </SelectParameters>
</asp:ObjectDataSource>
<asp:ObjectDataSource ID="odsDivisions" runat="server" TypeName="Argix.EnterpriseRGateway" SelectMethod="GetClients" EnableCaching="true" CacheExpirationPolicy="Sliding" CacheDuration="900">
    <SelectParameters>
        <asp:ControlParameter Name="number" ControlID="grdClients" PropertyName="SelectedDataKey.Values[0]" DefaultValue="000" Type="String" />
        <asp:Parameter Name="division" DefaultValue="" ConvertEmptyStringToNull="true" Type="String" />
        <asp:ControlParameter Name="activeOnly" ControlID="hfClientActiveOnly" PropertyName="Value" DefaultValue="False" Type="Boolean" />
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
    function scrollDivisions(number) {
        var grd = document.getElementById('<%=grdDivisions.ClientID %>');
        for (var i = 1; i < grd.rows.length; i++) {
            var cell = grd.rows[i].cells[1];
            if (cell.innerHTML.substr(0, number.length) == number) {
                var pnl = document.getElementById('divDivisions');
                pnl.scrollTop = i * (grd.clientHeight / grd.rows.length);
                break;
            }
        }
    }
</script>
