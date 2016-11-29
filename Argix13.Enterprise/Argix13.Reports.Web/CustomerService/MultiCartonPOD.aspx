<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="MultiCartonPOD.aspx.cs" Inherits="MultiCartonPOD" %>
<%@ MasterType VirtualPath="~/Site.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Setup" Runat="Server">
<table width="100%" border="0px" cellpadding="0" cellspacing="3px">
    <tr style="font-size:1px"><td width="120px">&nbsp;</td><td width="384px">&nbsp;</td><td>&nbsp;</td></tr>
        <tr>
        <td align="right">Client&nbsp;</td>
        <td>
            <asp:DropDownList id="cboClient" runat="server" Width="384px" AppendDataBoundItems="true" DataSourceID="odsClients" DataTextField="ClientName" DataValueField="ClientNumber" AutoPostBack="True" OnSelectedIndexChanged="OnClientChanged">
                <asp:ListItem Text="All" Value="" Selected="True"></asp:ListItem>
            </asp:DropDownList>
            <asp:ObjectDataSource ID="odsClients" runat="server" TypeName="Argix.EnterpriseRGateway" SelectMethod="GetClientsList" EnableCaching="true" CacheExpirationPolicy="Sliding" CacheDuration="600">
                <SelectParameters>
                    <asp:Parameter Name="activeOnly" DefaultValue="True" Type="Boolean" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </td>
        <td>&nbsp;</td>
    </tr>
    <tr style="font-size:1px; height:6px"><td colspan="3">&nbsp;</td></tr>
        <tr>
        <td align="right">Vendor&nbsp;</td>
        <td>
            <asp:DropDownList id="cboVendor" runat="server" Width="384px" AppendDataBoundItems="true" DataSourceID="odsVendors" DataTextField="VendorName" DataValueField="VendorNumber" AutoPostBack="True" OnSelectedIndexChanged="OnVendorChanged">
                <asp:ListItem Text="All" Value="" Selected="True"></asp:ListItem>
            </asp:DropDownList>
            <asp:ObjectDataSource ID="odsVendors" runat="server" TypeName="Argix.EnterpriseRGateway" SelectMethod="GetVendorsList" EnableCaching="true" CacheExpirationPolicy="Sliding" CacheDuration="180" >
                <SelectParameters>
                    <asp:ControlParameter Name="clientNumber" ControlID="cboClient" PropertyName="SelectedValue" ConvertEmptyStringToNull="true" Type="String" />
                    <asp:Parameter Name="clientTerminal" DefaultValue="" ConvertEmptyStringToNull="true" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </td>
        <td>&nbsp;</td>
    </tr>
    <tr style="font-size:1px; height:6px"><td colspan="3">&nbsp;</td></tr>
    <tr>
        <td align="right">By&nbsp;</td>
        <td>
            <asp:DropDownList id="cboBy" runat="server" Width="192px">
                <asp:ListItem Text="Carton Number" Value="Carton" Selected="True" />
                <asp:ListItem Text="Tracking Number" Value="Tracking" />
                <asp:ListItem Text="Label Sequence Number" Value="LabelSequence" />
            </asp:DropDownList>
        </td>
        <td>&nbsp;</td>
    </tr>
    <tr style="font-size:1px; height:6px"><td colspan="3">&nbsp;</td></tr>
    <tr>
        <td align="right" valign="top">Carton/Tracking #&nbsp;</td>
        <td><asp:TextBox ID="txtNumbers" runat="server" Height="192px" Width="384px" MaxLength="2048000" TextMode="MultiLine" AutoPostBack="True" OnTextChanged="OnNumbersChanged"></asp:TextBox></td>
        <td>&nbsp;</td>
    </tr>
    <tr style="font-size:1px; height:6px"><td colspan="3">&nbsp;</td></tr>
</table>
</asp:Content>

