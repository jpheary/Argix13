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

public partial class DailyStoreShipments :System.Web.UI.Page {
    //Members
    private const string TITLE = "Daily Store Shipments";
    private const string SOURCE = "/Customer Service/Daily Store Shipping";
    private string mDSName = "DailyShipmentDS";
    private string mUSPName = "uspRptDailyShipmentsGetList",mTBLName = "NewTable";
   
    //Interface
    protected void Page_Load(object sender,EventArgs e) {
        //Event handler for page load event
        if(!Page.IsPostBack && !ScriptManager.GetCurrent(Page).IsInAsyncPostBack) {
            //Initialize control values
            Master.ReportTitle = TITLE + " Report";
            this.dgdClientDivision.ClientSelectedIndex = -1;
            this.txtFrom.Text = DateTime.Today.ToString("MM/dd/yyyy");
            this.txtTo.Text = DateTime.Today.ToString("MM/dd/yyyy");
            OnClientSelected(null, EventArgs.Empty);
            OnValidateForm(null,EventArgs.Empty);
        }
        Master.ButtonCommand += new CommandEventHandler(OnButtonCommand);
    }
    protected void OnAgentChanged(object sender,EventArgs e) {
        //Event handler for change in agent value
        OnValidateForm(null,EventArgs.Empty);
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
    protected void OnButtonCommand(object sender,CommandEventArgs e) {
        //Event handler for command button clicked
        try {
            //Change view to Viewer and reset to clear existing data
            Master.Viewer.Reset();

            //Get parameters for the query
            string _startDate = DateTime.Parse(this.txtFrom.Text).ToString("yyyy-MM-dd");
            string _endDate = DateTime.Parse(this.txtTo.Text).ToString("yyyy-MM-dd");
            string _startTime = "07:00:01",_endTime = "07:00:00";
            string _client = this.dgdClientDivision.ClientNumber;
            string _clientName = this.dgdClientDivision.ClientName;
            string _clientDiv = this.dgdClientDivision.DivisionNumber;
            string _div = this.txtDivision.Text.Trim().Length > 0 ? this.txtDivision.Text : null;

            //Initialize control values
            LocalReport report = Master.Viewer.LocalReport;
            report.DisplayName = TITLE;
            report.EnableExternalImages = true;
            EnterpriseRGateway enterprise = new EnterpriseRGateway();
            DataSet ds = enterprise.FillDataset(this.mUSPName,mTBLName,new object[] { _client,_clientDiv,_startDate,_endDate,_startTime,_endTime,_div });
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
                    ReportParameter clientName = new ReportParameter("ClientName",_clientName);
                    ReportParameter clientDiv = new ReportParameter("ClientDivision",_clientDiv);
                    ReportParameter startDate = new ReportParameter("StartShipDate",_startDate);
                    ReportParameter endDate = new ReportParameter("EndShipDate",_endDate);
                    ReportParameter startTime = new ReportParameter("StartShipTime",_startTime);
                    ReportParameter endTime = new ReportParameter("EndShipTime",_endTime);
                    ReportParameter div = new ReportParameter("Division",_div);
                    report.SetParameters(new ReportParameter[] { client,clientName,clientDiv,startDate,endDate,startTime,endTime,div });

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
                    Response.AddHeader("Content-Disposition","inline; filename=AmscanImperialDelivery.xls");
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
