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

public partial class TimePeriodCartonCompare :System.Web.UI.Page {
    //Members
    private const string TITLE = "Time Period Carton Compare";
    private const string SOURCE = "";
    private string mDSName = "";
    private string mUSPName = "",mTBLName = "NewTable";
    private const string REPORT_SRC_OVER = "/Customer Service/Time Period Carton Compare Over";
    private const string REPORT_SRC_SHORT = "/Customer Service/Time Period Carton Compare Short";
    private const string REPORT_DS_OVER = "TimePeriodCartonOverDS";
    private const string REPORT_DS_SHORT = "TimePeriodCartonShortDS";
    private const string USP_REPORT_OVER = "uspRptDiscrepancyCartonTimePeriodOver";
    private const string USP_REPORT_SHORT = "uspRptDiscrepancyCartonTimePeriodShort";
   
    //Interface
    protected void Page_Load(object sender,EventArgs e) {
        //Event handler for page load event
        if(!Page.IsPostBack && !ScriptManager.GetCurrent(Page).IsInAsyncPostBack) {
            //Initialize control values
            Master.ReportTitle = TITLE + " Report";
            this.txtFrom.Text = DateTime.Today.AddDays(-DateTime.Today.Day + 1).AddMonths(-1).ToString("MM/dd/yyyy");
            this.txtTo.Text = DateTime.Today.AddDays(-DateTime.Today.Day).ToString("MM/dd/yyyy");
            OnAllDivsCheckedChanged(this.chkAllDivs,EventArgs.Empty);
        }
        Master.ButtonCommand += new CommandEventHandler(OnButtonCommand);
    }
    protected void OnFromToDateChanged(object sender,EventArgs e) {
        //Event handler for change in from/to dates
        OnValidateForm(null,EventArgs.Empty);
    }
    protected void OnAllDivsCheckedChanged(object sender, EventArgs e) {
        //Update client list
        this.dgdClientVendor.ClientDivision = this.chkAllDivs.Checked ? "00" : "";
        this.dgdClientVendor.ClientSelectedIndex = -1;
        OnClientSelected(this.dgdClientVendor,EventArgs.Empty);
    }
    protected void OnClientSelected(object sender,EventArgs e) {
        //Event handler for change in selected client
        this.dgdClientVendor.VendorSelectedIndex = -1;
        OnVendorSelected(this.dgdClientVendor,EventArgs.Empty);
    }
    protected void OnVendorSelected(object sender,EventArgs e) {
        //Event handler for change in selected vendor
        OnValidateForm(null,EventArgs.Empty);
    }
    protected void OnValidateForm(object sender,EventArgs e) {
        //Event handler for changes in parameter data
        Master.Validated = this.dgdClientVendor.VendorSelectedRow != null;
    }
    protected void OnButtonCommand(object sender,CommandEventArgs e) {
        //Event handler for command button clicked
        try {
            //Change view to Viewer and reset to clear existing data
            Master.Viewer.Reset();

            //Get parameters for the query
            string _fromDate = DateTime.Parse(this.txtFrom.Text).ToString("yyyy-MM-dd");
            string _toDate = DateTime.Parse(this.txtTo.Text).ToString("yyyy-MM-dd");
            string _client = this.dgdClientVendor.ClientNumber;
            string _div = (this.chkAllDivs.Checked) ? null : this.dgdClientVendor.ClientDivsionNumber;
            string _clientName = this.dgdClientVendor.ClientName;
            string _vendor = this.dgdClientVendor.VendorNumber;
            string _vendorName = this.dgdClientVendor.VendorName;

            //Initialize control values
            LocalReport report = Master.Viewer.LocalReport;
            report.DisplayName = TITLE;
            report.EnableExternalImages = true;
            EnterpriseRGateway enterprise = new EnterpriseRGateway();
            DataSet ds = new DataSet();
            string reportFile = "",reportDS = "",usp="";
            switch(this.cboType.SelectedItem.Text) {
                case "Over":
                    reportFile = REPORT_SRC_OVER;
                    reportDS = REPORT_DS_OVER;
                    usp = USP_REPORT_OVER;
                    break;
                case "Short":
                    reportFile = REPORT_SRC_SHORT;
                    reportDS = REPORT_DS_SHORT;
                    usp = USP_REPORT_SHORT;
                    break;
            }
            ds = enterprise.FillDataset(usp,mTBLName,new object[] { _fromDate,_toDate,_vendor,_client,_div });
            if (ds.Tables[mTBLName] == null) ds.Tables.Add(mTBLName);
            switch(e.CommandName) {
                case "Run":
                    //Set local report and data source
                    System.IO.Stream stream = Master.GetReportDefinition(reportFile);
                    report.LoadReportDefinition(stream);
                    report.DataSources.Clear();
                    report.DataSources.Add(new ReportDataSource(reportDS,ds.Tables[mTBLName]));

                    //Set the report parameters for the report
                    ReportParameter fromDate = new ReportParameter("Startdate",_fromDate);
                    ReportParameter toDate = new ReportParameter("EndDate",_toDate);
                    ReportParameter client = new ReportParameter("ClientNumber",_client + "-" + (_div==null?"All":_div) + " " + _clientName);
                    ReportParameter div = new ReportParameter("DivisionNumber",(_div==null?"All":_div));
                    ReportParameter clientName = new ReportParameter("ClientName",_clientName);
                    ReportParameter vendor = new ReportParameter("VendorNumber",_vendor + " " + _vendorName);
                    ReportParameter vendorName = new ReportParameter("VendorName",_vendorName);
                    report.SetParameters(new ReportParameter[] { fromDate,toDate,vendor,client,div });

                    //Update report rendering with new data
                    report.Refresh();
                    
                    if(!Master.Viewer.Enabled) Master.Viewer.CurrentPage = 1;
                    break;
                case "Data":
                    //Set local export report and data source
                    report.LoadReportDefinition(Master.CreateExportRdl(ds,reportDS));
                    report.DataSources.Clear();
                    report.DataSources.Add(new ReportDataSource(reportDS,ds.Tables[mTBLName]));
                    report.Refresh();
                    break;
                case "Excel":
                    //Create Excel mime-type page
                    Response.ClearHeaders();
                    Response.Clear();
                    Response.Charset = "";
                    Response.AddHeader("Content-Disposition","inline; filename=TimePeriodCartonCompare.xls");
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
