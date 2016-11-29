using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default:Page {
    //Members

    //Interface
    protected void Page_Load(object sender,EventArgs e) {
        //Event handler for page load event
        if (!Page.IsPostBack) {
            //Initialize control values
            this.Title = Master.ReportTitle = "Argix Logistics Reports 2015";
        }
        Master.Validated = false;
    }
}