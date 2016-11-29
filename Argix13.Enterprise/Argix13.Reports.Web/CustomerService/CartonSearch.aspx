<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="CartonSearch.aspx.cs" Inherits="CartonSearch" %>
<%@ MasterType VirtualPath="~/Site.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Setup" Runat="Server">
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
</script>
<div style="margin:50px 0px 0px 10px">
    <asp:UpdatePanel ID="upnlHeader" runat="server" UpdateMode="Always" >
    <ContentTemplate>
        <asp:DropDownList ID="cboSearchBy" runat="server" width="144px" AutoPostBack="True" OnSelectedIndexChanged="OnSearchByChanged">
            <asp:ListItem Text="Carton #"  Value="ByCarton" Selected="True" />
            <asp:ListItem Text="Label Sequence #" Value="ByLabel" />
        </asp:DropDownList>
        &nbsp;
        <asp:TextBox ID="txtSearchNo" runat="server" Width="350px" AutoPostBack="True" OnTextChanged="OnValidateForm" />
    </ContentTemplate>
    </asp:UpdatePanel>
</div>
<div style="margin:10px 0px 0px 10px">
    <div class="gridbox gridclient" style="width:500px">
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
            <asp:GridView ID="grdClients" runat="server" Width="100%" DataSourceID="odsClients" AutoGenerateColumns="False" AllowSorting="True" AutoGenerateCheckBoxColumn="True" CheckAllCheckBoxVisible="False" OnSelectedIndexChanged="OnClientSelected">
                <Columns>
                    <asp:CommandField ButtonType="Image" HeaderStyle-Width="16px" SelectImageUrl="~/Content/Images/select.gif" SelectText="" ShowSelectButton="True" ShowCancelButton="False" />
                    <asp:BoundField DataField="ClientNumber" HeaderText="Num" HeaderStyle-Width="24px" SortExpression="ClientNumber" />
                    <asp:BoundField DataField="DivisionNumber" HeaderText="Div" HeaderStyle-Width="0px" SortExpression="DivisionNumber" Visible="False" />
                    <asp:BoundField DataField="ClientName" HeaderText="Name" SortExpression="ClientName" HtmlEncode="False" />
                    <asp:BoundField DataField="TerminalCode" HeaderText="Terminal" HeaderStyle-Width="48px" SortExpression="TerminalCode" />
                </Columns>
            </asp:GridView>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="txtFindClient" EventName="TextChanged" />
                <asp:AsyncPostBackTrigger ControlID="imgFindClient" EventName="Click" />
            </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</div>
<asp:ObjectDataSource ID="odsClients" runat="server" TypeName="Argix.EnterpriseRGateway" SelectMethod="GetClients" EnableCaching="true" CacheExpirationPolicy="Sliding" CacheDuration="600">
    <SelectParameters>
        <asp:Parameter Name="number" DefaultValue="" ConvertEmptyStringToNull="true" Type="String" />
        <asp:Parameter Name="division" DefaultValue="" ConvertEmptyStringToNull="true" Type="String" />
        <asp:Parameter Name="activeOnly" DefaultValue="True" Type="Boolean" />
    </SelectParameters>
</asp:ObjectDataSource>
</asp:Content>

