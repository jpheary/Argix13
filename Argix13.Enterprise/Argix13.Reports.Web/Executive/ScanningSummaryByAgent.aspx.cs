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

public partial class ScanningSummaryByAgent :System.Web.UI.Page {
    //Members
    private const string TITLE = "Scanning Summary By Agent";
    private const string SOURCE = "/Executive/Scanning Summary By Agent";
    private string mDSName = "ScanningSummaryByAgentDS";
    private string mUSPName = "uspRptScanningSummaryByAgent",mTBLName = "NewTable";
    
    //Interface
    protected void Page_Load(object sender,EventArgs e) {
        //Event handler for page load event
        if(!Page.IsPostBack) {
            //Initialize control values
            Master.ReportTitle = TITLE + " Report";
            Master.ParamsSelectedValue = "Agents";
            Master.ParamsEnabled = false;
            Master.ShowSubAgents = false;
            OnValidateForm(null,EventArgs.Empty);
        }
        Master.Master.ButtonCommand += new CommandEventHandler(OnButtonCommand);
        Master.Viewer.Drillthrough += new DrillthroughEventHandler(OnViewerDrillthrough);
        Master.Viewer.ShowBackButton = true;
    }
    protected void OnValidateForm(object sender,EventArgs e) {
        //Event handler for changes in parameter data
        Master.Validated = true;
    }
    protected void OnButtonCommand(object sender,CommandEventArgs e) {
        //Event handler for command button clicked
        try {
            //Change view to Viewer and reset to clear existing data
            Master.Viewer.Reset();

            //Get parameters for the query
            string _client = Master.ClientNumber;
            string _division = Master.Division;
            string _agentnumber = Master.AgentNumber;
            string _subagentnumber = Master.SubAgentNumber;
            string _region = Master.Region;
            string _district = Master.District;
            string _store = Master.StoreNumber;
            string _start = Master.StartDate;
            string _end = Master.EndDate;

            //Initialize control values
            LocalReport report = Master.Viewer.LocalReport;
            report.DisplayName = TITLE;
            report.EnableExternalImages = true;
            EnterpriseRGateway enterprise = new EnterpriseRGateway();
            DataSet ds = enterprise.FillDataset(this.mUSPName,mTBLName,new object[] { _client,_division,_agentnumber,_subagentnumber,_region,_district,_store,_start,_end });
            if (ds.Tables[mTBLName] == null) ds.Tables.Add(mTBLName);
            switch(e.CommandName) {
                case "Run":
                    //Set local report and data source
                    System.IO.Stream stream = Master.GetReportDefinition(SOURCE);
                    report.LoadReportDefinition(stream);
                    report.DataSources.Clear();
                    report.DataSources.Add(new ReportDataSource(this.mDSName,ds.Tables[mTBLName]));
                    DataSet _ds = enterprise.FillDataset("uspRptClientRead",mTBLName,new object[] { _client });
                    report.DataSources.Add(new ReportDataSource("DataSet1",_ds.Tables[mTBLName]));

                    //Set the report parameters for the report
                    ReportParameter clientName = new ReportParameter("ClientName",Master.ClientName);
                    ReportParameter client = new ReportParameter("ClientNumber",_client);
                    ReportParameter division = new ReportParameter("Division",_division);
                    ReportParameter agentnumber = new ReportParameter("AgentParentNumber",_agentnumber);
                    ReportParameter subagentnumber = new ReportParameter("AgentNumber",_subagentnumber);
                    ReportParameter region = new ReportParameter("Region"); if(_region != null) region.Values.Add(_region);
                    ReportParameter district = new ReportParameter("District"); if(_district != null) district.Values.Add(_district);
                    ReportParameter store = new ReportParameter("StoreNumber"); if(_store != null) store.Values.Add(_store);
                    ReportParameter start = new ReportParameter("StartDate",_start);
                    ReportParameter end = new ReportParameter("EndDate",_end);
                    report.SetParameters(new ReportParameter[] { client,division,agentnumber,subagentnumber,region,district,store,start,end,clientName });

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
                    Response.AddHeader("Content-Disposition","inline; filename=ScanningSummaryByAgent.xls");
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
    protected void OnViewerDrillthrough(object sender,DrillthroughEventArgs e) {
        //Event handler for drill through
        if (!e.Report.IsDrillthroughReport) return;

        //Determine which drillthrough report is requested
        LocalReport report = (LocalReport)e.Report;
        report.DisplayName = e.ReportPath;
        report.EnableExternalImages = true;
        switch (e.ReportPath) {
            case "Scanning Summary By Client": {
                    //Get drillthrough parameters
                    string _agent = "",_start = "",_end = "";
                    for (int i = 0;i < report.OriginalParametersToDrillthrough.Count;i++) {
                        switch (report.OriginalParametersToDrillthrough[i].Name) {
                            case "StartDate": _start = report.OriginalParametersToDrillthrough[i].Values[0]; break;
                            case "EndDate": _end = report.OriginalParametersToDrillthrough[i].Values[0]; break;
                            case "AgentNumber": _agent = report.OriginalParametersToDrillthrough[i].Values[0]; break;
                        }
                    }

                    //Set local report and data source
                    System.IO.Stream stream = Master.GetReportDefinition("/Executive/" + e.ReportPath);
                    report.LoadReportDefinition(stream);
                    EnterpriseRGateway enterprise = new EnterpriseRGateway();
                    DataSet ds = enterprise.FillDataset("uspRptScanningSummaryByClient","NewTable",new object[] { null,_agent,_start,_end });
                    report.DataSources.Clear();
                    report.DataSources.Add(new ReportDataSource("ScanningSummaryByClientDS",ds.Tables["NewTable"]));
                    DataSet _ds = enterprise.FillDataset("uspRptAgentRead",mTBLName,new object[] { _agent });
                    report.DataSources.Add(new ReportDataSource("DataSet1",_ds.Tables["NewTable"]));

                    ReportParameter client = new ReportParameter("ClientNumber");
                    ReportParameter agent = new ReportParameter("AgentNumber",_agent);
                    ReportParameter start = new ReportParameter("StartDate",_start);
                    ReportParameter end = new ReportParameter("EndDate",_end);
                    report.SetParameters(new ReportParameter[] { client,agent,start,end });
                    report.Refresh();
                }
                break;
            case "Scanning Summary By Agent": {
                    //Get drillthrough parameters
                    string _client = "",_start = "",_end = "";
                    for (int i = 0;i < report.OriginalParametersToDrillthrough.Count;i++) {
                        switch (report.OriginalParametersToDrillthrough[i].Name) {
                            case "StartDate": _start = report.OriginalParametersToDrillthrough[i].Values[0]; break;
                            case "EndDate": _end = report.OriginalParametersToDrillthrough[i].Values[0]; break;
                            case "ClientNumber": _client = report.OriginalParametersToDrillthrough[i].Values[0]; break;
                        }
                    }

                    //Set local report and data source
                    System.IO.Stream stream = Master.GetReportDefinition("/Executive/" + e.ReportPath);
                    report.LoadReportDefinition(stream);
                    EnterpriseRGateway enterprise = new EnterpriseRGateway();
                    DataSet ds = enterprise.FillDataset(this.mUSPName,mTBLName,new object[] { _client,null,null,null,null,null,null,_start,_end });
                    report.DataSources.Clear();
                    report.DataSources.Add(new ReportDataSource(this.mDSName,ds.Tables[mTBLName]));
                    DataSet _ds = enterprise.FillDataset("uspRptClientRead",mTBLName,new object[] { _client });
                    report.DataSources.Add(new ReportDataSource("DataSet1",_ds.Tables[mTBLName]));

                    ReportParameter clientName = new ReportParameter("ClientName");
                    ReportParameter client = new ReportParameter("ClientNumber",_client);
                    ReportParameter division = new ReportParameter("Division");
                    ReportParameter agentnumber = new ReportParameter("AgentParentNumber");
                    ReportParameter subagentnumber = new ReportParameter("AgentNumber");
                    ReportParameter region = new ReportParameter("Region");
                    ReportParameter district = new ReportParameter("District");
                    ReportParameter store = new ReportParameter("StoreNumber");
                    ReportParameter start = new ReportParameter("StartDate",_start);
                    ReportParameter end = new ReportParameter("EndDate",_end);
                    report.SetParameters(new ReportParameter[] { client,division,agentnumber,subagentnumber,region,district,store,start,end,clientName });
                    report.Refresh();
                }
                break;
        }
    }
}
