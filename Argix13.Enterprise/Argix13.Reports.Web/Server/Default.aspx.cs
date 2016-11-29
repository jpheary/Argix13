using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

public partial class Default :System.Web.UI.Page {
    //Members
    private string mItemPath = "";

    //Interface
    protected void Page_Load(object sender,EventArgs e) {
        //Event handler for page load event
        if(!Page.IsPostBack) {
            //Initialize control values
            this.mItemPath = Request.QueryString["ItemPath"] == null ? "" : Request.QueryString["ItemPath"].ToString();
            ViewState["ItemPath"] = this.mItemPath;

            string title = this.mItemPath.Substring(this.mItemPath.LastIndexOfAny(new char[]{'/'}) + 1);
            this.Title = Master.ReportTitle = title + " Report";
        }
        else {
            this.mItemPath = ViewState["ItemPath"].ToString();
        }
        Master.Viewer.ServerReport.ReportPath = this.mItemPath;
    }
}