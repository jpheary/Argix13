using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class Error:System.Web.UI.Page {
    //Members
    private Exception mEx = null;

    //Interface
    protected void Page_Load(object sender,EventArgs e) {
        //Event handler for page load event
        if(!Page.IsPostBack) {
            //
            this.mEx = Server.GetLastError();
            ViewState["Exception"] = this.mEx;

            this.lblError.Text = this.mEx != null ? this.mEx.ToString() : "Unknown Error";
            Server.ClearError();
        }
        else {
            this.mEx = (Exception)ViewState["Exception"];
        }
    }
}
