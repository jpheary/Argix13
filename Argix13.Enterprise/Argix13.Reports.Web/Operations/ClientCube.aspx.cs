using System;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Microsoft.Reporting.WebForms;
using Argix;

public partial class ClientCube :System.Web.UI.Page {
    //Members
    private const string TITLE = "Client Cube";
    private const string SOURCE = "/Operations/Client Cube";
    private string mDSName = "ClientCubeDS";
    private string mUSPName = "uspRptClientCube",mTBLName = "NewTable";

    //Interface
    protected void Page_Load(object sender,EventArgs e) {
        //Event handler for page load event
        if(!Page.IsPostBack) {
            //Initialize control values
            Master.ReportTitle = TITLE + " Report";
            this.txtFrom.Text = this.txtTo.Text = DateTime.Today.ToString("MM/dd/yyyy");
            this.cboClient.DataBind();
            if(this.cboClient.Items.Count > 0) this.cboClient.SelectedIndex = 0;
            OnClientChanged(null,EventArgs.Empty);
            this.cboZone.DataBind();
            if(this.cboZone.Items.Count > 0) this.cboZone.SelectedIndex = 0;
            OnValidateForm(null,EventArgs.Empty);
        }
        Master.ButtonCommand += new CommandEventHandler(OnButtonCommand);
        OnValidateForm(null,EventArgs.Empty);
    }
    protected void OnClientChanged(object sender,EventArgs e) {
        //Event handler for change in selected client
        this.cboTerminal.Items.Clear();
        this.cboTerminal.Items.Add(new ListItem("All",""));
        this.cboTerminal.DataBind();
        if(this.cboTerminal.Items.Count > 0) this.cboTerminal.SelectedIndex = 0;
        OnTerminalChanged(null,EventArgs.Empty);
    }
    protected void OnTerminalChanged(object sender,EventArgs e) {
        //Event handler for change in selected terminal
        this.cboVendor.Items.Clear();
        this.cboVendor.Items.Add(new ListItem("All",""));
        this.cboVendor.DataBind();
        if(this.cboVendor.Items.Count > 0) this.cboVendor.SelectedIndex = 0;
    }
    protected void OnValidateForm(object sender,EventArgs e) {
        //Event handler for changes in parameter data
        Master.Validated = (this.cboClient.SelectedValue.Length > 0);
    }
    protected void OnButtonCommand(object sender,CommandEventArgs e) {
        //Event handler for view button clicked
        try {
            //Change view to Viewer and reset to clear existing data
            Master.Viewer.Reset();

            //Get parameters for the query
            string _from = DateTime.Parse(this.txtFrom.Text).ToString("yyyy-MM-dd");
            string _to = DateTime.Parse(this.txtTo.Text).ToString("yyyy-MM-dd");
            string _client = this.cboClient.SelectedValue;
            string _terminal = this.cboTerminal.SelectedValue == "" ? null : this.cboTerminal.SelectedValue;
            string _vendor = this.cboVendor.SelectedValue == "" ? null : this.cboVendor.SelectedValue;
            string _zone = this.cboZone.SelectedValue == "" ? null : this.cboZone.SelectedValue;

            //Initialize control values
            LocalReport report = Master.Viewer.LocalReport;
            report.DisplayName = TITLE;
            report.EnableExternalImages = true;
            EnterpriseRGateway enterprise = new EnterpriseRGateway();
            DataSet ds = enterprise.FillDataset(this.mUSPName,mTBLName,new object[] { _client,_terminal,_vendor,_zone,_from,_to });
            if (ds.Tables[mTBLName] == null) ds.Tables.Add(mTBLName);
            switch(e.CommandName) {
                case "Run":
                    //Set local report and data source
                    System.IO.Stream stream = Master.GetReportDefinition(SOURCE);
                    report.LoadReportDefinition(stream);
                    report.DataSources.Clear();
                    report.DataSources.Add(new ReportDataSource(this.mDSName,ds.Tables[mTBLName]));

                    //Set the report parameters for the report
                    ReportParameter client = new ReportParameter("ClientNumber",_client);
                    ReportParameter terminal = new ReportParameter("TerminalCode",_terminal);
                    ReportParameter vendor = new ReportParameter("VendorNumber",_vendor);
                    ReportParameter zone = new ReportParameter("ZoneCode",_zone);
                    ReportParameter from = new ReportParameter("StartSortDate",_from.ToString());
                    ReportParameter to = new ReportParameter("EndSortDate",_to.ToString());
                    report.SetParameters(new ReportParameter[] { client,terminal,vendor,zone,from,to });

                    //Update report rendering with new data
                    report.Refresh();
                        
                    if(!Master.Viewer.Enabled) Master.Viewer.CurrentPage = 1;
                    break;
                case "Data":
                    //Set local export report and data source
                    report.LoadReportDefinition(Master.CreateExportRdl(ds,this.mDSName));
                    report.DataSources.Clear();
                    report.DataSources.Add(new ReportDataSource(this.mDSName,ds.Tables[mTBLName]));
                    report.Refresh();
                    break;
                case "Excel":
                    //Create Excel mime-type page
                    Response.ClearHeaders();
                    Response.Clear();
                    Response.Charset = "";
                    Response.AddHeader("Content-Disposition","inline; filename=ClientCube.xls");
                    Response.ContentType = "application/vnd.ms-excel";  //application/octet-stream";
                    System.IO.StringWriter sw = new System.IO.StringWriter();
                    System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(sw);
                    DataGrid dg = new DataGrid();
                    dg.DataSource = ds.Tables[mTBLName];
                    dg.DataBind();
                    dg.RenderControl(hw);
                    Response.Write(sw.ToString());
                    Response.End();
                    break;
            }
        }
        catch(Exception ex) { Master.ReportError(ex); }
    }
}
