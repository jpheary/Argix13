using System;
using System.Data;
using System.Diagnostics;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Microsoft.Reporting.WebForms;
using Argix;

public partial class _PacSunSummary:System.Web.UI.Page {
    //Members
    private const string TITLE = "PacSun Summary";
    private const string SOURCE = "/Customer Service/PacSun Summary";
    private string mDSName = "SummaryDS";
    private string mUSPName = "uspRptPacSunShipmentsByShipmentDate",mTBLName = "NewTable";
   
    //Interface
    protected void Page_Init(object sender,EventArgs e) {
        //Event handler for page init event
        if(!Page.IsPostBack) {
            //Data cache for drillthrough reports
            Session.Add("PacSunDS",null);
        }
    }
    protected void Page_Load(object sender,EventArgs e) {
        //Event handler for page load event
        if(!Page.IsPostBack) {
            //Initialize control values
            Master.ReportTitle = TITLE + " Report";
            this.txtFrom.Text = this.txtTo.Text = DateTime.Today.ToString("MM/dd/yyyy");
            OnValidateForm(null,EventArgs.Empty);
        }
        Master.ButtonCommand += new CommandEventHandler(OnButtonCommand);
        Master.Viewer.Drillthrough += new DrillthroughEventHandler(OnViewerDrillthrough);
        Master.Viewer.ShowBackButton = true;
    }
    protected void OnValidateForm(object sender,EventArgs e) {
        //Event handler for changes in parameter data
        //Validate business rules
        bool valid = true;

        //Set report creation service state
        Master.Validated = valid;
    }
    protected void OnButtonCommand(object sender,CommandEventArgs e) {
        //Event handler for command button clicked
        try {
            //Change view to Viewer and reset to clear existing data
            Master.Viewer.Reset();
            Session["PacSunDS"] = null;

            //Get parameters for the query
            string _start = DateTime.Parse(this.txtFrom.Text).ToString("yyyy-MM-dd");
            string _end = DateTime.Parse(this.txtTo.Text).ToString("yyyy-MM-dd");

            //Initialize control values
            LocalReport report = Master.Viewer.LocalReport;
            report.DisplayName = TITLE;
            report.EnableExternalImages = true;
            EnterpriseRGateway enterprise = new EnterpriseRGateway();
            DataSet ds = enterprise.FillDataset(this.mUSPName,mTBLName,new object[] { _start,_end });
            if (ds.Tables[mTBLName] == null) ds.Tables.Add(mTBLName);
            switch(e.CommandName) {
                case "Run":
                    //Set local report and data source
                    System.IO.Stream stream = Master.GetReportDefinition(SOURCE);
                    report.LoadReportDefinition(stream);
                    report.DataSources.Clear();
                    report.DataSources.Add(new ReportDataSource(this.mDSName,ds.Tables[mTBLName]));

                    //Set the report parameters for the report
                    ReportParameter start = new ReportParameter("FromDate",_start);
                    ReportParameter end = new ReportParameter("ToDate",_end);
                    report.SetParameters(new ReportParameter[] { start,end });

                    //Update report rendering with new data
                    report.Refresh();
                    
                    if(!Master.Viewer.Enabled) Master.Viewer.CurrentPage = 1;
                    break;
                case "Data":
                    //Set local export report and data source
                    report.LoadReportDefinition(Master.CreateExportRdl(ds,this.mDSName));
                    report.DataSources.Clear();
                    report.DataSources.Add(new ReportDataSource(this.mDSName,ds.Tables[mTBLName]));
                    report.Refresh(); break;
                case "Excel":
                    //Create Excel mime-type page
                    Response.ClearHeaders();
                    Response.Clear();
                    Response.Charset = "";
                    Response.AddHeader("Content-Disposition","inline; filename=PacSunSummary.xls");
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
    #region Data Services: OnViewerDrillthrough()
    protected void OnViewerDrillthrough(object sender,DrillthroughEventArgs e) {
        //Event handler for drill through
        if(!e.Report.IsDrillthroughReport) return;

        //Determine which drillthrough report is requested
        LocalReport report = (LocalReport)e.Report;
        switch(e.ReportPath) {
            case "PacSunSummaryCartonDetail": {
                    //Get drillthrough parameters
                    string _start="",_end="",_terminal="",_store="",_manifest="";
                    for(int i=0;i<report.OriginalParametersToDrillthrough.Count;i++) {
                        switch(report.OriginalParametersToDrillthrough[i].Name) {
                            case "FromDate": _start = report.OriginalParametersToDrillthrough[i].Values[0]; break;
                            case "ToDate": _end = report.OriginalParametersToDrillthrough[i].Values[0]; break;
                            case "TerminalName": _terminal = report.OriginalParametersToDrillthrough[i].Values[0]; break;
                            case "StoreNumber": _store = report.OriginalParametersToDrillthrough[i].Values[0]; break;
                            case "ManifestNumber": _manifest = report.OriginalParametersToDrillthrough[i].Values[0]; break;
                        }
                    }

                    //Set data source and parameters
                    DataSet ds = (DataSet)Session["PacSunDS"];
                    report.DataSources.Clear();
                    report.DataSources.Add(new ReportDataSource("DetailDS",ds.Tables[mTBLName]));
                    ReportParameter start = new ReportParameter("FromDate",_start);
                    ReportParameter end = new ReportParameter("ToDate",_end);
                    ReportParameter terminal = new ReportParameter("TerminalName",_terminal);
                    ReportParameter store = new ReportParameter("StoreNumber",_store);
                    ReportParameter manifest = new ReportParameter("ManifestNumber",_manifest);
                    e.Report.SetParameters(new ReportParameter[] { start,end,terminal,store,manifest });
                    e.Report.Refresh();
                }
                break;
            case "PacSunSummaryOSDDetail": {
                    //Get drillthrough parameters
                    string _start="",_end="",_terminal="",_store="",_manifest="";
                    for(int i=0;i<report.OriginalParametersToDrillthrough.Count;i++) {
                        switch(report.OriginalParametersToDrillthrough[i].Name) {
                            case "FromDate": _start = report.OriginalParametersToDrillthrough[i].Values[0]; break;
                            case "ToDate": _end = report.OriginalParametersToDrillthrough[i].Values[0]; break;
                            case "TerminalName": _terminal = report.OriginalParametersToDrillthrough[i].Values[0]; break;
                            case "StoreNumber": _store = report.OriginalParametersToDrillthrough[i].Values[0]; break;
                            case "ManifestNumber": _manifest = report.OriginalParametersToDrillthrough[i].Values[0]; break;
                        }
                    }

                    //Set data source and parameters
                    DataSet ds = (DataSet)Session["PacSunDS"];
                    report.DataSources.Clear();
                    report.DataSources.Add(new ReportDataSource("DetailDS",ds.Tables[mTBLName]));
                    ReportParameter start = new ReportParameter("FromDate",_start);
                    ReportParameter end = new ReportParameter("ToDate",_end);
                    ReportParameter terminal = new ReportParameter("TerminalName",_terminal);
                    ReportParameter store = new ReportParameter("StoreNumber",_store);
                    ReportParameter manifest = new ReportParameter("ManifestNumber",_manifest);
                    e.Report.SetParameters(new ReportParameter[] { start,end,terminal,store,manifest });
                    e.Report.Refresh();
                }
                break;
        }
    }
    #endregion
}
