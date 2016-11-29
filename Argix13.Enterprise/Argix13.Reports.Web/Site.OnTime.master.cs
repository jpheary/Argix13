using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Argix;
using Microsoft.Reporting.WebForms;

public partial class SiteOnTimeMaster:System.Web.UI.MasterPage {
    //Members

    //Interface
    public string ReportTitle { get { return Master.ReportTitle; } set { Master.ReportTitle = value; } }
    public ReportViewer Viewer { get { return Master.Viewer; } }
    public bool Validated { set { if(value) OnValidateForm(null,EventArgs.Empty); else Master.Validated = false; } }
    public string Status { set { Master.Status = value; } }
    public void ReportError(Exception ex) { Master.ReportError(ex); }
    public Stream GetReportDefinition(string report) { return Master.GetReportDefinition(report); }
    public Stream CreateExportRdl(DataSet ds,string dataSetName) { return Master.CreateExportRdl(ds,dataSetName); }

    public string ClientNumber { get { return this.dgdClientDivision.ClientNumber; } }
    public string ClientName { get { return this.dgdClientDivision.ClientName; } }
    public string DivisionNumber { get { return this.dgdClientDivision.DivisionNumber; } }
    public DateTime FromDate { get { return DateTime.Parse(this.txtFrom.Text); } }
    public DateTime ToDate { get { return DateTime.Parse(this.txtTo.Text); } }
    public string AgentNumber { get { return this.cboAgent.SelectedValue; } }
    public bool AgentsEnabled { get { return this.cboAgent.Enabled; } set { this.cboAgent.Enabled = value; } }

    protected void Page_Load(object sender,EventArgs e) {
        //Event handler for page load event
        if(!Page.IsPostBack) {
            //Initialize control values
            this.txtFrom.Text = DateTime.Today.ToString("MM/dd/yyyy");
            this.txtTo.Text = DateTime.Today.ToString("MM/dd/yyyy");
            this.cboAgent.DataBind();
            if(this.cboAgent.Items.Count > 0) this.cboAgent.SelectedIndex = 0;

            this.dgdClientDivision.ClientSelectedIndex = -1;
            OnClientSelected(null,EventArgs.Empty);
        }
    }
    protected void OnClientSelected(object sender,EventArgs e) {
        //Event handler for change in selected client
        this.dgdClientDivision.DivisionSelectedIndex = -1;
        OnDivisionSelected(null,EventArgs.Empty);
    }
    protected void OnDivisionSelected(object sender,EventArgs e) {
        //Event handler for change in selected vendor
        OnValidateForm(null,EventArgs.Empty);
    }
    protected void OnValidateForm(object sender,EventArgs e) {
        //Event handler for changes in parameter data
        Master.Validated = (this.dgdClientDivision.DivisionNumber.Length > 0);
    }
}