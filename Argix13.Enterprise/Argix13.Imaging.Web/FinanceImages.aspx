<%@ page language="C#" masterpagefile="~/Imaging.master" autoeventwireup="true" enableeventvalidation="false" CodeFile="FinanceImages.aspx.cs" inherits="_FinanceImages" %>
<%@ MasterType VirtualPath="~/Imaging.master" %>

<asp:Content ID="cBody" runat="server" ContentPlaceHolderID="cpBody">
<div class="subtitle">Finance Images</div>
<asp:ValidationSummary ID="vsImages" runat="server" ValidationGroup="vgImages" />
<div class="form">
    <asp:UpdatePanel ID="upnlForm" runat="server" UpdateMode="Always">
    <ContentTemplate>
        <table style="width:100%; text-align:left">
            <tr>
                <td style="text-align:right">Document Class&nbsp;</td>
                <td><asp:DropDownList ID="cboDocClass" runat="server" DataSourceID="odsDocs" DataTextField="ClassName" DataValueField="ClassName" Width="150px" AutoPostBack="True" OnSelectedIndexChanged="OnDocClassChanged" />
                    &nbsp;&nbsp;&nbsp;Use * as a wildcard character; specify dates as yyyy-mm-dd.
                </td>
            </tr>
            <tr><td colspan="2" style="font-size:1px; height:5px">&nbsp;</td></tr>
            <tr>
                <td style="text-align:right">Search Criteria&nbsp;</td>
                <td>
                    <asp:DropDownList ID="cboProp1" runat="server" DataSourceID="odsMetaData" DataTextField="Property" DataValueField="Value" Width="150px" AutoPostBack="True" OnSelectedIndexChanged="OnPropertySelectedIndexChanged" />
                    <asp:TextBox ID="txtSearch1" runat="server" MaxLength="30" AutoPostBack="True" OnTextChanged="OnSearchTextChanged" />
                    <asp:DropDownList ID="cboOperand1" runat="server" Width="75px" AutoPostBack="True" OnSelectedIndexChanged="OnOperandSelectedIndexChanged" ><asp:ListItem Text="AND" Value="AND" /><asp:ListItem Text="OR" Value="OR" /></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>
                    <asp:DropDownList ID="cboProp2" runat="server" DataSourceID="odsMetaData" DataTextField="Property" DataValueField="Value" Width="150px" AutoPostBack="True" OnSelectedIndexChanged="OnPropertySelectedIndexChanged" />
                    <asp:TextBox ID="txtSearch2" runat="server" MaxLength="30" AutoPostBack="True" OnTextChanged="OnSearchTextChanged" />
                </td>
            </tr>
        </table>
        <asp:RequiredFieldValidator ID="rfvSearch1" runat="server" ControlToValidate="txtSearch1" ErrorMessage="Please enter search text in the first text box." ValidationGroup="vgImages" />
    </ContentTemplate>
    </asp:UpdatePanel>
<div class="services">
    <asp:UpdatePanel ID="upnlServices" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="submit" ValidationGroup="vgImages" CommandName="Search" OnCommand="OnCommand" />
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="cboDocClass" EventName="SelectedIndexChanged" />
        <asp:AsyncPostBackTrigger ControlID="cboProp1" EventName="SelectedIndexChanged" />
        <asp:AsyncPostBackTrigger ControlID="txtSearch1" EventName="TextChanged" />
        <asp:AsyncPostBackTrigger ControlID="cboOperand1" EventName="SelectedIndexChanged" />
        <asp:AsyncPostBackTrigger ControlID="cboProp2" EventName="SelectedIndexChanged" />
        <asp:AsyncPostBackTrigger ControlID="txtSearch2" EventName="TextChanged" />
    </Triggers>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="uprgServices" runat="server" AssociatedUpdatePanelID="upnlServices">
        <ProgressTemplate>Searching for images...</ProgressTemplate>
    </asp:UpdateProgress>
</div>
</div>
<div class="view">
    <asp:UpdatePanel ID="upnlImages" runat="server" UpdateMode="Conditional" >
    <ContentTemplate>
        <asp:GridView ID="grdImages" runat="server" Width="100%" AutoGenerateColumns="false" AllowSorting="true" OnSorting="OnGridSorting" OnSorted="OnGridSorted" >
            <Columns>
                <asp:BoundField DataField="scope" HeaderText="scope" ItemStyle-Width="75px" Visible="false" />
                <asp:BoundField DataField="contentclass" HeaderText="contentclass" ItemStyle-Width="75px" Visible="false" />
                <asp:BoundField DataField="IsDocument" HeaderText="IsDocument" ItemStyle-Width="75px" Visible="false" />
                <asp:BoundField DataField="Size" HeaderText="Size" ItemStyle-Width="50px" Visible="true" SortExpression="Size" />
                <asp:BoundField DataField="Description" HeaderText="Description" ItemStyle-Width="100px" Visible="true" />
                <asp:BoundField DataField="Title" HeaderText="Title" ItemStyle-Width="75px" Visible="false" />
                <asp:HyperLinkField DataTextField="Title" HeaderText="Path" ItemStyle-Width="250px" DataNavigateUrlFields="Path" DataNavigateUrlFormatString="{0}" Visible="true" />
            </Columns>
        </asp:GridView>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="cboDocClass" EventName="SelectedIndexChanged" />
        <asp:AsyncPostBackTrigger ControlID="cboProp1" EventName="SelectedIndexChanged" />
        <asp:AsyncPostBackTrigger ControlID="txtSearch1" EventName="TextChanged" />
        <asp:AsyncPostBackTrigger ControlID="cboOperand1" EventName="SelectedIndexChanged" />
        <asp:AsyncPostBackTrigger ControlID="cboProp2" EventName="SelectedIndexChanged" />
        <asp:AsyncPostBackTrigger ControlID="txtSearch2" EventName="TextChanged" />
        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Command" />
    </Triggers>
    </asp:UpdatePanel>
</div>
<asp:ObjectDataSource ID="odsDocs" runat="server" TypeName="Argix.Enterprise.ImagingGateway" SelectMethod="GetDocumentClasses" CacheExpirationPolicy="Sliding" CacheDuration="900" EnableCaching="true">
    <SelectParameters>
        <asp:Parameter Name="Department" DefaultValue="Finance" Type="String" />
    </SelectParameters>
</asp:ObjectDataSource>
<asp:ObjectDataSource ID="odsMetaData" runat="server" TypeName="Argix.Enterprise.ImagingGateway" SelectMethod="GetMetaData">
    <SelectParameters>
        <asp:ControlParameter Name="className" ControlID="cboDocClass" PropertyName="SelectedValue" Type="string" />
    </SelectParameters>
</asp:ObjectDataSource>
</asp:Content>
