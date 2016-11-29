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

public partial class SiteComplianceMaster:System.Web.UI.MasterPage {
    //Members

    //Interface
    public string ReportTitle { get { return Master.ReportTitle; } set { Master.ReportTitle = value; } }
    public ReportViewer Viewer { get { return Master.Viewer; } }
    public bool Validated { set { if(value) OnValidateForm(null,EventArgs.Empty); else Master.Validated = false; } }
    public string Status { set { Master.Status = value; } }
    public void ReportError(Exception ex) { Master.ReportError(ex); }
    public Stream GetReportDefinition(string report) { return Master.GetReportDefinition(report);}
    public Stream CreateExportRdl(DataSet ds,string dataSetName) { return Master.CreateExportRdl(ds, dataSetName);}

    public DateTime FromDate { get { return DateTime.Parse(this.txtFrom.Text); } }
    public DateTime ToDate { get { return DateTime.Parse(this.txtTo.Text); } }
    public string ClientNumber { get { return this.cboClient.SelectedValue!="" ? this.cboClient.SelectedValue : null; } }
    public string ClientName { get { return this.cboClient.SelectedItem.Text; } }
    public string VendorNumber { get { if(this.cboVendor.SelectedValue != "" && this.cboVendorLoc.SelectedValue == "") return this.cboVendor.SelectedValue; else return null; } }
    public string VendorLocationNumber { get { if(this.cboVendor.SelectedValue != "" && this.cboVendorLoc.SelectedValue != "") return this.cboVendorLoc.SelectedValue; else return null; } }
    public string VendorName { get { if(this.cboVendorLoc.SelectedValue != "") return this.cboVendorLoc.SelectedItem.Text; else return this.cboVendor.SelectedItem.Text; } }
    public string AgentNumber { get { if(this.cboAgent.SelectedValue != "" && this.cboAgentLoc.SelectedValue == "") return this.cboAgent.SelectedValue; else return null; } }
    public string AgentLocationNumber { get { if(this.cboAgent.SelectedValue != "" && this.cboAgentLoc.SelectedValue != "") return this.cboAgentLoc.SelectedValue; else return null; } }
    public string AgentName { get { if(this.cboAgentLoc.SelectedValue != "") return this.cboAgentLoc.SelectedItem.Text; else return this.cboAgent.SelectedItem.Text; } }
    public bool UseCutoffTime { get { return this.chkUseCutoff.Checked; } }
    public bool UseCutoffTimeVisible { get { return this.chkUseCutoff.Visible; } set { this.chkUseCutoff.Visible = value; } }

    protected void Page_Load(object sender,EventArgs e) {
        //Event handler for page load event
        if(!Page.IsPostBack) {
            //Initialize control values
            this.txtFrom.Text = this.txtTo.Text = DateTime.Today.ToString("MM/dd/yyyy");
            this.cboClient.DataBind();
            if(this.cboClient.Items.Count > 0) this.cboClient.SelectedIndex = 0;
            OnClientChanged(this.cboClient,EventArgs.Empty);
            this.cboAgent.DataBind();
            if(this.cboAgent.Items.Count > 0) this.cboAgent.SelectedIndex = 0;
            OnAgentChanged(this.cboAgent,EventArgs.Empty);
            OnValidateForm(null,EventArgs.Empty);
        }
    }
    protected void OnFromToDateChanged(object sender,EventArgs e) {
        //Event handler for change in from/to date
        OnValidateForm(null,EventArgs.Empty);
    }
    protected void OnClientChanged(object sender,EventArgs e) {
        //Event handler for change in agent value
        this.cboVendor.Items.Clear();
        this.cboVendor.Items.Add(new ListItem("All",""));
        this.cboVendor.DataBind();
        if(this.cboVendor.Items.Count > 0) this.cboVendor.SelectedIndex = 0;
        OnVendorChanged(this.cboVendor,EventArgs.Empty);
    }
    protected void OnActiveOnlyChecked(object sender,EventArgs e) {
        //Event handler for check state changed
        this.cboClient.DataBind();
        if(this.cboClient.Items.Count > 0) this.cboClient.SelectedIndex = 0;
        OnClientChanged(null,EventArgs.Empty);
    }
    protected void OnVendorChanged(object sender,EventArgs e) {
        //Event handler for change in agent value
        this.cboVendorLoc.Items.Clear();
        this.cboVendorLoc.Items.Add(new ListItem("All",""));
        this.cboVendorLoc.DataBind();
        if(this.cboVendorLoc.Items.Count > 0) this.cboVendorLoc.SelectedIndex = 0;
        OnVendorLocChanged(null,EventArgs.Empty);
    }
    protected void OnVendorLocChanged(object sender,EventArgs e) {
        //Event handler for change in agent value
        OnValidateForm(null,EventArgs.Empty);
    }
    protected void OnAgentChanged(object sender,EventArgs e) {
        //Event handler for change in agent value
        this.cboAgentLoc.Items.Clear();
        this.cboAgentLoc.Items.Add(new ListItem("All",""));
        this.cboAgentLoc.DataBind();
        if(this.cboAgentLoc.Items.Count > 0) this.cboAgentLoc.SelectedIndex = 0;
        OnAgentLocChanged(null,EventArgs.Empty);
    }
    protected void OnAgentLocChanged(object sender,EventArgs e) {
        //Event handler for change in agent value
        OnValidateForm(null,EventArgs.Empty);
    }
    protected void OnValidateForm(object sender,EventArgs e) {
        //Event handler for changes in parameter data
        Master.Validated =  this.cboClient.SelectedValue != null && 
                            this.cboVendor.SelectedValue != null &&
                            this.cboVendorLoc.SelectedValue != null && 
                            this.cboAgent.SelectedValue != null && 
                            this.cboAgentLoc.SelectedValue != null;
    }
}