using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Microsoft.Reporting.WebForms;
using Argix;

public partial class InductedCartons :System.Web.UI.Page {
    //Members
    private const string TITLE = "Inducted Cartons";
    private const string SOURCE = "/Customer Service/Inducted Cartons";
    private string mDSName = "DataSet";
    private string mUSPName = "uspRptInductedCartonsGetList",mTBLName = "NewTable";

    //Interface
    protected void Page_Load(object sender,EventArgs e) {
        //Event handler for page load event
        if(!Page.IsPostBack) {
            //Initialize control values
            Master.ReportTitle = TITLE + " Report";
            this.txtFrom.Text = this.txtTo.Text = DateTime.Today.ToString("MM/dd/yyyy");
            OnTerminalSelected(null,EventArgs.Empty);
        }
        Master.ButtonCommand += new CommandEventHandler(OnButtonCommand);
    }
    protected void OnFromToDateChanged(object sender,EventArgs e) { OnValidateForm(null,EventArgs.Empty); }
    protected void OnTerminalSelected(object sender,EventArgs e) { OnValidateForm(null,EventArgs.Empty); }
    protected void OnFreightSelected(object sender,EventArgs e) { OnValidateForm(null,EventArgs.Empty); }
    protected void OnValidateForm(object sender,EventArgs e) {
        //Event handler for changes in parameter data
        Master.Validated = (this.cboTerminal.SelectedValue.Length > 0 && this.grdFreight.Rows.Count > 0 && this.grdFreight.SelectedRow != null);
    }
    protected void OnButtonCommand(object sender,CommandEventArgs e) {
        //Event handler for command button clicked
        try {
            //Change view to Viewer and reset to clear existing data
            Master.Viewer.Reset();

            //Get parameters for the query
            string _blNumber = "";
            string _terminalCode = this.cboTerminal.SelectedValue;

            //Initialize control values
            LocalReport report = Master.Viewer.LocalReport;
            report.DisplayName = TITLE;
            report.EnableExternalImages = true;
            EnterpriseRGateway enterprise = new EnterpriseRGateway();
            DataKey dataKey = (DataKey)this.grdFreight.DataKeys[this.grdFreight.SelectedRow.RowIndex];
            _blNumber = dataKey["BLNumber"].ToString();
            DataSet ds = enterprise.FillDataset(this.mUSPName,mTBLName,new object[] { _blNumber,_terminalCode });
            if (ds.Tables[mTBLName] == null) ds.Tables.Add(mTBLName);
            switch(e.CommandName) {
                case "Run":
                    //Set local report and data source
                    System.IO.Stream stream = Master.GetReportDefinition(SOURCE);
                    report.LoadReportDefinition(stream);
                    report.DataSources.Clear();
                    report.DataSources.Add(new ReportDataSource(this.mDSName,ds.Tables[mTBLName]));

                    //Set the report parameters for the report
                    ReportParameter blNumber = new ReportParameter("BLNumber",_blNumber);
                    ReportParameter terminalCode = new ReportParameter("TerminalCode",_terminalCode);
                    report.SetParameters(new ReportParameter[] { blNumber,terminalCode });

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
                    Response.AddHeader("Content-Disposition","inline; filename=InductedCartons.xls");
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