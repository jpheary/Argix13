<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="DeliveryPaperwork.aspx.cs" Inherits="DeliveryPaperwork" %>
<%@ MasterType VirtualPath="~/Site.master" %>

<asp:Content ID="cSetup" ContentPlaceHolderID="Setup" Runat="Server">
<script type="text/javascript">
    var daysback = -31, daysforward = 0, daysspread = 5;
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
<div style="margin:25px 0px 0px 50px">
    Terminal&nbsp;
    <asp:DropDownList ID="cboTerminal" runat="server" Width="192px" DataSourceID="odsTerminals" DataTextField="Description" DataValueField="Number" OnSelectedIndexChanged="OnTerminalSelected" AutoPostBack="True"></asp:DropDownList>
</div>
<div style="margin:10px 0px 0px 50px">
    Report Type&nbsp;
    <asp:DropDownList ID="cboType" runat="server" Width="192px">
        <asp:ListItem Text="Manifest" Value="Manifest" Selected="True" />
        <asp:ListItem Text="Delivery Bill" Value="Delivery Bill" />
        <asp:ListItem Text="Delivery Bill w/PO Number" Value="Delivery Bill w/PO Number" />
    </asp:DropDownList>
</div>
<div style="margin:10px 0px 0px 50px">
    <label for="txtFrom">Closed Date&nbsp;</label>
    <asp:TextBox ID="txtFrom" runat="server" Width="100px" AutoPostBack="true" OnTextChanged="OnFromToDateChanged" />
    <label for="txtTo">&nbsp;-&nbsp;</label>
    <asp:TextBox ID="txtTo" runat="server" Width="100px" AutoPostBack="true" OnTextChanged="OnFromToDateChanged" />
</div>
<div style="margin:10px 0px 0px 50px">
    Search by TL#&nbsp;
    <asp:TextBox ID="txtSearch" runat="server" Width="120px" ToolTip="Search for a TL... <press Enter>" AutoPostBack="True" OnTextChanged="OnSearch" />
    <asp:ImageButton ID="imgFind" runat="server" Width="20px" ImageAlign="Middle" ImageUrl="~/Content/Images/search.gif" ToolTip="Search for a TL..." OnClick="OnFind" />
</div>
<div class="clear"></div>
<div style="margin:10px 0px 0px 10px">
    <div class="gridbox gridpickup">
        <div class="pickuptitle"><div class="pickuptitlename">Closed TL's</div></div>
        <div class="clear"></div>
        <div id="divPickups" class="pickup">
            <asp:UpdatePanel ID="upnlTLs" runat="server" UpdateMode="Conditional" >
            <ContentTemplate>
                <asp:GridView ID="grdTLs" runat="server" Width="100%" DataSourceID="odsTLs" AutoGenerateColumns="False" AllowSorting="True" OnSelectedIndexChanged="OnTLSelected">
                    <Columns>
                        <asp:CommandField ButtonType="Image" SelectImageUrl="~/Content/Images/select.gif" HeaderStyle-Width="12px" ShowSelectButton="True" />
                        <asp:BoundField DataField="NUMBER" HeaderText="Number" HeaderStyle-Width="120px" SortExpression="NUMBER" />
                        <asp:BoundField DataField="ZONE_CODE" HeaderText="Zone" HeaderStyle-Width="72px" SortExpression="ZONE_CODE" />
                        <asp:BoundField DataField="ZONE_TYPE" HeaderText="Type" HeaderStyle-Width="96px" SortExpression="ZONE_TYPE" />
                        <asp:BoundField DataField="AGENT_NUMBER" HeaderText="Agent#" HeaderStyle-Width="96px" SortExpression="AGENT_NUMBER" />
                        <asp:BoundField DataField="OPEN_DATE" HeaderText="Open Date" HeaderStyle-Width="96px" ItemStyle-Wrap="false" DataFormatString="{0:MM-dd-yyyy}" HtmlEncode="False" />
                        <asp:BoundField DataField="OPEN_TIME" HeaderText="Open Time" HeaderStyle-Width="72px" ItemStyle-Wrap="false" DataFormatString="{0:hh:mm tt}" HtmlEncode="False" />
                        <asp:BoundField DataField="CLOSE_DATE" HeaderText="Close Date" HeaderStyle-Width="96px" ItemStyle-Wrap="false" DataFormatString="{0:MM-dd-yyyy}" HtmlEncode="False" />
                        <asp:BoundField DataField="CLOSE_TIME" HeaderText="Close Time" HeaderStyle-Width="72px" ItemStyle-Wrap="false" DataFormatString="{0:hh:mm tt}" HtmlEncode="False" />
                        <asp:BoundField DataField="PRINTED_DATE" HeaderText="Print Date" HeaderStyle-Width="96px" ItemStyle-Wrap="false" DataFormatString="{0:MM-dd-yyyy}" HtmlEncode="False" Visible="true" />
                        <asp:BoundField DataField="PRINTED_TIME" HeaderText="Print Time" HeaderStyle-Width="72px" ItemStyle-Wrap="false" DataFormatString="{0:hh:mm tt}" HtmlEncode="False" Visible="true" />
                        <asp:BoundField DataField="CTM_DATE" HeaderText="CTM Date" HeaderStyle-Width="96px" ItemStyle-Wrap="false" DataFormatString="{0:MM-dd-yyyy}" HtmlEncode="False" Visible="true" />
                        <asp:BoundField DataField="CTM_TIME" HeaderText="CTM Time" HeaderStyle-Width="72px" ItemStyle-Wrap="false" DataFormatString="{0:hh:mm tt}" HtmlEncode="False" Visible="true" />
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="cboTerminal" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="txtFrom" EventName="TextChanged" />
                <asp:AsyncPostBackTrigger ControlID="txtTo" EventName="TextChanged" />
            </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</div>
<asp:ObjectDataSource ID="odsTerminals" runat="server" TypeName="Argix.EnterpriseRGateway" SelectMethod="GetArgixTerminals" EnableCaching="true" CacheExpirationPolicy="Sliding" CacheDuration="900" />
<asp:ObjectDataSource ID="odsTLs" runat="server" TypeName="Argix.EnterpriseRGateway" SelectMethod="GetTLs" EnableCaching="true" CacheExpirationPolicy="Sliding" CacheDuration="180" >
    <SelectParameters>
        <asp:ControlParameter Name="terminal" ControlID="cboTerminal" PropertyName="SelectedValue" Type="string" />
        <asp:ControlParameter Name="startDate" ControlID="txtFrom" PropertyName="Text" Type="string" />
        <asp:ControlParameter Name="endDate" ControlID="txtTo" PropertyName="Text" Type="string" />
    </SelectParameters>
</asp:ObjectDataSource>
<asp:ObjectDataSource ID="odsTL" runat="server" TypeName="Argix.EnterpriseRGateway" SelectMethod="FindTL" EnableCaching="false" >
    <SelectParameters>
        <asp:ControlParameter Name="terminal" ControlID="cboTerminal" PropertyName="SelectedValue" Type="string" />
        <asp:ControlParameter Name="TLNumber" ControlID="txtSearch" PropertyName="Text" Type="string" />
    </SelectParameters>
</asp:ObjectDataSource>
</asp:Content>

