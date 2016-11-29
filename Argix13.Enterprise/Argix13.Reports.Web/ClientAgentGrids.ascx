<%@ Control Language="C#" AutoEventWireup="true" ClassName="ClientAgentGrids" CodeFile="ClientAgentGrids.ascx.cs" Inherits="ClientAgentGrids" EnableTheming="true" %>

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
    <div class="gridbox gridagent">
        <div class="agenttitle">
            <div class="agenttitlename">Agents</div>
            <div class="agenttitlesearch">
                <asp:UpdatePanel ID="upnlAgentsHeader" runat="server" UpdateMode="Always" >
                <ContentTemplate>
                    <asp:TextBox ID="txtFindAgent" runat="server" ToolTip="Enter an agent number... <press Enter>" AutoPostBack="True" OnTextChanged="OnAgentSearch" />
                    <asp:ImageButton ID="imgFindAgent" runat="server" ImageUrl="~/Content/Images/search.gif" ToolTip="Search for a agent..." OnClick="OnFindAgent" />
                </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="clear"></div>
        <div id="divAgents" class="agent">
            <asp:UpdatePanel ID="upnlAgents" runat="server" UpdateMode="Conditional" >
            <ContentTemplate>
                <asp:GridView ID="grdAgents" runat="server" Width="100%" AutoGenerateColumns="False" AllowSorting="True" DataSourceID="odsAgents" OnSelectedIndexChanged="OnAgentSelected">
                    <Columns>
                        <asp:CommandField ButtonType="Image" HeaderStyle-Width="16px" ShowSelectButton="True" SelectImageUrl="~/Content/Images/select.gif" />
                        <asp:BoundField DataField="AgentNumber" HeaderText="Num" HeaderStyle-Width="60px" SortExpression="AgentNumber" />
                        <asp:BoundField DataField="AgentName" HeaderText="Name" SortExpression="AgentName" />
                        <asp:BoundField DataField="MainZone" HeaderText="Zone" HeaderStyle-Width="75px" SortExpression="MainZone" />
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="txtFindAgent" EventName="TextChanged" />
                <asp:AsyncPostBackTrigger ControlID="imgFindAgent" EventName="Click" />
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
<asp:ObjectDataSource ID="odsAgents" runat="server" TypeName="Argix.EnterpriseRGateway" SelectMethod="GetAgents" EnableCaching="true" CacheExpirationPolicy="Sliding" CacheDuration="900" >
    <SelectParameters>
        <asp:Parameter Name="mainZoneOnly" DefaultValue="false" Type="Boolean" />
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
    function scrollAgents(number) {
        var grd = document.getElementById('<%=grdAgents.ClientID %>');
        for (var i = 1; i < grd.rows.length; i++) {
            var cell = grd.rows[i].cells[1];
            if (cell.innerHTML.substr(0, number.length) == number) {
                var pnl = document.getElementById('divAgents');
                pnl.scrollTop = i * (grd.clientHeight / grd.rows.length);
                break;
            }
        }
    }
</script>
