using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class ImagingMaster:System.Web.UI.MasterPage {
    //
    protected void Page_Load(object sender, EventArgs e) {
        //Event handler for page load event
    }
    public void ReportError(Exception ex, int logLevel) {
        //Report an exception to the user
        try {
            string msg = ex.Message;
            if(ex.InnerException != null) msg = ex.Message + "\n\n NOTE: " + ex.InnerException.Message;

            //string username = new MembershipServices().Username;
            //new EnterpriseGateway().WriteLogEntry(3, username, ex);
            ShowMessageBox(msg);
        }
        catch(Exception) { }
    }
    public void ShowMessageBox(string message) {
        //
        message = message.Replace("'", "").Replace("\n", " ").Replace("\r", " ");
        ScriptManager.RegisterStartupScript(this.lblMsg, typeof(Label), "Error", "alert('" + message + "');", true);
    }
}
