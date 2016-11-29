<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Error.aspx.cs" Inherits="Error" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Reports13 Error Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h2>Oops!</h2>
        <table id="tblPage" width="100%" align="center" border="0px" cellpadding="1px" cellspacing="1px">
			<tr><td width="48px">&nbsp</td><td>&nbsp;</td></tr>
			<tr>
				<td>&nbsp</td>
				<td>
					<p>
					    An unexpected error occurred. Please go back to the <a href="../Default.aspx">Reports Explorer</a> and try again.
					</p>
					<p>&nbsp;</p>
					<p><asp:Label ID="lblError" runat="server" Width="100%" Height="100%" Text=""></asp:Label></p>
				</td>
	        </tr>
	    </table>
    </div>
    </form>
</body>
</html>
