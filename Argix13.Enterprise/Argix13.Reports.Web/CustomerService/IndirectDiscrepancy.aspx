<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="IndirectDiscrepancy.aspx.cs" Inherits="IndirectDiscrepancy" %>
<%@ MasterType VirtualPath="~/Site.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Setup" Runat="Server">
<div style="margin:25px 0px 0px 50px">
    Terminal&nbsp;
    <asp:DropDownList ID="cboTerminal" runat="server" Width="288px" DataSourceID="odsTerminals" DataTextField="Description" DataValueField="Number" AutoPostBack="True" OnSelectedIndexChanged="OnTerminalSelected" />
</div>
<div style="margin:10px 0px 0px 50px">
    <asp:CheckBox ID="chkOverShort" runat="server" Text="Over/Short Only" width="120px" Checked="True" />
</div>
<div style="margin:10px 0px 0px 50px">
    Trips for&nbsp;
    <asp:TextBox ID="txtTripDaysBack" runat="server" width="24px" MaxLength="2" AutoPostBack="True" OnTextChanged="OnTripRangeChanged">1</asp:TextBox>&nbsp;days back.
</div>
<div class="clear"></div>
<div style="margin:10px 0px 0px 10px">
    <div class="gridbox gridpickup">
        <div class="pickuptitle"><div class="pickuptitlename">Indirect Trips</div></div>
        <div class="clear"></div>
        <div id="divPickups" class="pickup">
            <asp:UpdatePanel ID="upnlTrips" runat="server" ChildrenAsTriggers="true" RenderMode="Block" UpdateMode="Conditional" >
            <ContentTemplate>
                <asp:Panel ID="pnlTrips" runat="server" Width="100%" Height="288px" ScrollBars="Horizontal">
                    <asp:GridView ID="grdTrips" runat="server" Width="100%" DataSourceID="odsTrips" DataKeyNames="BolNumber" AutoGenerateColumns="False" AllowSorting="True" OnSelectedIndexChanged="OnTripSelected">
                        <Columns>
                            <asp:CommandField ButtonType="Image" SelectImageUrl="~/Content/Images/select.gif" HeaderStyle-Width="12px" ShowSelectButton="True" />
                            <asp:BoundField DataField="BolNumber" HeaderText="BOL#" HeaderStyle-Width="96px" SortExpression="BolNumber" HtmlEncode="False" />
                            <asp:BoundField DataField="CartonCount" HeaderText="Cartons" HeaderStyle-Width="72px" SortExpression="CartonCount" />
                            <asp:BoundField DataField="Carrier" HeaderText="Carrier" HeaderStyle-Width="72px" SortExpression="Carrier" />
                            <asp:BoundField DataField="TrailerNumber" HeaderText="Trailer#" HeaderStyle-Width="96px" SortExpression="TrailerNumber" />
                            <asp:BoundField DataField="Started" HeaderText="Started" HeaderStyle-Width="144px" SortExpression="Started" DataFormatString="{0:MM-dd-yyyy hh:mm tt}" HtmlEncode="False" />
                            <asp:BoundField DataField="Stopped" HeaderText="Stopped" HeaderStyle-Width="144px" SortExpression="Stopped" DataFormatString="{0:MM-dd-yyyy hh:mm tt}" HtmlEncode="False" />
                            <asp:BoundField DataField="Exported" HeaderText="Exported" HeaderStyle-Width="144px" SortExpression="Exported" DataFormatString="{0:MM-dd-yyyy hh:mm tt}" HtmlEncode="False" />
                            <asp:BoundField DataField="Imported" HeaderText="Imported" HeaderStyle-Width="144px" SortExpression="Imported" DataFormatString="{0:MM-dd-yyyy hh:mm tt}" HtmlEncode="False" />
                            <asp:BoundField DataField="Scanned" HeaderText="Scanned" HeaderStyle-Width="144px" SortExpression="Scanned" DataFormatString="{0:MM-dd-yyyy hh:mm tt}" HtmlEncode="False" />
                            <asp:BoundField DataField="OSDSend" HeaderText="OS&amp;D Sent" HeaderStyle-Width="144px" SortExpression="OSDSend" DataFormatString="{0:MM-dd-yyyy hh:mm tt}" HtmlEncode="False" />
                            <asp:BoundField DataField="Received" HeaderText="Received" HeaderStyle-Width="144px" SortExpression="Received" DataFormatString="{0:MM-dd-yyyy hh:mm tt}" HtmlEncode="False" />
                            <asp:BoundField DataField="CartonsExported" HeaderText="Ctns Exported" HeaderStyle-Width="72px" SortExpression="CartonsExported" />
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="cboTerminal" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="txtTripDaysBack" EventName="TextChanged" />
            </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</div>
<asp:ObjectDataSource ID="odsTerminals" runat="server" TypeName="Argix.EnterpriseRGateway" SelectMethod="GetArgixTerminals" EnableCaching="true" CacheExpirationPolicy="Sliding" CacheDuration="900" />
<asp:ObjectDataSource ID="odsTrips" runat="server" TypeName="Argix.EnterpriseRGateway" SelectMethod="GetIndirectTrips" EnableCaching="false" >
    <SelectParameters>
        <asp:ControlParameter Name="terminal" ControlID="cboTerminal" PropertyName="SelectedValue" Type="string" />
        <asp:ControlParameter Name="daysBack" ControlID="txtTripDaysBack" PropertyName="Text" Type="string" />
    </SelectParameters>
</asp:ObjectDataSource>
</asp:Content>

