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

public partial class AuditTrailer :System.Web.UI.Page {
    //Members
    private static int SpinMax = 14, SpinMin = 1, SpinDefault = 7;
   
	private const string TITLE = "Audit Trailer";
    private const string SOURCE = "/Operations/Audit Trailer";
    private const string REPORT_SRC_OVERSHORT = "/Operations/Audit Trailer Over Short";
    private string mDSName = "AuditTrailerDS";
    private string mUSPName = "uspRptIndirectScanGetListForTripAudit",mTBLName = "NewTable";
    
    //Interface
    protected void Page_Load(object sender,EventArgs e) {
        //Event handler for page load event
        if(!Page.IsPostBack) {
            //Initialize control values
            Master.ReportTitle = TITLE + " Report";
            this.cboTerminal.DataBind();
            if(this.cboTerminal.Items.Count > 0) this.cboTerminal.SelectedIndex = 0;
            this.chkOverShort.Checked = true;
            this.txtTripDaysBack.Text = SpinDefault.ToString();
            OnTripRangeChanged(null,EventArgs.Empty);
        }
        Master.ButtonCommand += new CommandEventHandler(OnButtonCommand);
        OnValidateForm(null,EventArgs.Empty);
    }
    protected void OnTerminalSelected(object sender,System.EventArgs e) {
        //Event hanlder for change in selected terminal
        this.grdTrips.DataBind();
        if(this.grdTrips.Rows.Count > 0) this.grdTrips.SelectedIndex = 0;
        OnTripSelected(this,EventArgs.Empty);
    }
    protected void OnTripRangeChanged(object sender,EventArgs e) {
        //Event handler for change in trip range
        if(int.Parse(this.txtTripDaysBack.Text) < SpinMin)
            this.txtTripDaysBack.Text = SpinMin.ToString();
        else if(int.Parse(this.txtTripDaysBack.Text) > SpinMax)
            this.txtTripDaysBack.Text = SpinMax.ToString();
        OnTerminalSelected(this,EventArgs.Empty);
    }
    protected void OnTripSelected(object sender,EventArgs e) {
        //Event handler for change in selected trip
        OnValidateForm(null,EventArgs.Empty);
    }
    protected void OnValidateForm(object sender,EventArgs e) {
        //Event handler for changes in parameter data
        Master.Validated = (this.grdTrips.Rows.Count > 0 && this.grdTrips.SelectedRow != null);
    }
    protected void OnButtonCommand(object sender,CommandEventArgs e) {
        //Event handler for view button clicked
        try {
            //Change view to Viewer and reset to clear existing data
            Master.Viewer.Reset();
        
            //Get parameters for the query
            string _terminal = this.cboTerminal.SelectedValue;
            string _terminalName = this.cboTerminal.SelectedItem.Text;
            string _tripNumber = this.grdTrips.SelectedRow.Cells[1].Text.Trim();

            //Initialize control values
            LocalReport report = Master.Viewer.LocalReport;
            report.DisplayName = TITLE;
            report.EnableExternalImages = true;
            EnterpriseRGateway enterprise = new EnterpriseRGateway();
            DataSet ds = new DataSet(this.mDSName);
            DataSet _ds = enterprise.FillDataset(this.mUSPName,mTBLName,new object[] { _terminal,_tripNumber });
            if(_ds.Tables[mTBLName].Rows.Count >= 0) {
                //Filter for over/short if applicable
                if(this.chkOverShort.Checked) {
                    ds = _ds.Clone();
                    ds.Merge(_ds.Tables[mTBLName].Select("Match = 0"));
                }
                else
                    ds.Merge(_ds);
            }
            if (ds.Tables[mTBLName] == null) ds.Tables.Add(mTBLName);
            switch(e.CommandName) {
                case "Run":
                    //Set local report and data source
                    System.IO.Stream stream = Master.GetReportDefinition(this.chkOverShort.Checked ? REPORT_SRC_OVERSHORT : SOURCE);
                    report.LoadReportDefinition(stream);
                    report.DataSources.Clear();
                    report.DataSources.Add(new ReportDataSource(this.mDSName,ds.Tables[mTBLName]));

                    //Set the report parameters for the report
                    ReportParameter terminal = new ReportParameter("TerminalID",_terminal);
                    ReportParameter terminalName = new ReportParameter("TerminalName",_terminalName);
                    ReportParameter tripNumber = new ReportParameter("TripNumber",_tripNumber);
                    report.SetParameters(new ReportParameter[] { terminal,tripNumber,terminalName });

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
                    Response.AddHeader("Content-Disposition","inline; filename=AuditTrailer.xls");
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
        catch (Exception ex) { Master.ReportError(ex); }
    }
}
