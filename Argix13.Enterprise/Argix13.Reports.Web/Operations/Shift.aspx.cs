using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Microsoft.Reporting.WebForms;
using Argix;

public partial class Shift :System.Web.UI.Page {
    //Members
    private const string TITLE = "Shift";
    private const string SOURCE = "/Operations/Shift";
    private string mDSName = "ShiftReportDS";
    private string mUSPName = "uspRptShiftDataGetList",mTBLName = "NewTable";

    //Interface
    protected void Page_Load(object sender,EventArgs e) {
        //Event handler for page load event
        if(!Page.IsPostBack && !ScriptManager.GetCurrent(Page).IsInAsyncPostBack) {
            //Initialize control values
            Master.ReportTitle = TITLE + " Report";
            this.txtShiftDate.Text = DateTime.Today.ToString("MM/dd/yyyy");
            this.cboTerminal.DataBind();
            if(this.cboTerminal.Items.Count > 0) this.cboTerminal.SelectedValue = "05";
            OnTerminalSelected(null,EventArgs.Empty);
            this.cboType.SelectedIndex = 0;
        }
        Master.ButtonCommand += new CommandEventHandler(OnButtonCommand);
        OnValidateForm(null,EventArgs.Empty);
    }
    protected void OnShiftDateChanged(object sender,EventArgs e) {
        //Cascade
        this.cboShift.DataBind();
        if(this.cboShift.Items.Count > 0) this.cboShift.SelectedIndex = 0;
        OnShiftChanged(null,EventArgs.Empty);
    }
    protected void OnTerminalSelected(object sender,EventArgs e) {
        //Event handler for change in selected client
        this.cboShift.DataBind();
        if(this.cboShift.Items.Count > 0) this.cboShift.SelectedIndex = 0;
        OnShiftChanged(null,EventArgs.Empty);
    }
    protected void OnShiftChanged(object sender,EventArgs e) {
        //Event handler for change in selected client
        OnValidateForm(null,EventArgs.Empty);
    }
    protected void OnValidateForm(object sender,EventArgs e) {
        //Event handler for changes in parameter data
        Master.Validated = (this.cboTerminal.SelectedValue != null && this.cboShift.SelectedValue != null);
    }
    protected void OnButtonCommand(object sender,CommandEventArgs e) {
        //Event handler for view button clicked
        try {
            //Change view to Viewer and reset to clear existing data
            Master.Viewer.Reset();

            //Get parameters for the query
            string _start = DateTime.Parse(this.txtShiftDate.Text).ToString("yyyy-MM-dd");
            string _terminal = this.cboTerminal.SelectedValue;
            int _shift = int.Parse(this.cboShift.SelectedValue);
            string _header = "Shift Report";

            //Initialize control values
            LocalReport report = Master.Viewer.LocalReport;
            report.DisplayName = TITLE;
            report.EnableExternalImages = true;
            EnterpriseRGateway enterprise = new EnterpriseRGateway();
            DataSet ds = new DataSet(this.mDSName);
            DataSet _ds = enterprise.FillDataset(this.mUSPName,mTBLName,new object[] { _terminal,_start,_shift });
            if (_ds.Tables[mTBLName] == null) _ds.Tables.Add(mTBLName);
            string filter="";
            switch(this.cboType.SelectedValue.ToLower()) {
                case "tsort": filter = "FreightType = 'Tsort'"; _header = "Tsort"; break;
                case "returns": filter = "FreightType = 'Returns'"; _header = "Return"; break;
                case "both": filter = null; _header = "Tsort & Return"; break;
            }
            DataRow[] rows = _ds.Tables[mTBLName].Select(filter);
            ds.Merge(rows);
            if (ds.Tables[mTBLName] == null) ds.Tables.Add(mTBLName);
            switch(e.CommandName) {
                case "Run":
                    //Set local report and data source
                    System.IO.Stream stream = Master.GetReportDefinition(SOURCE);
                    report.LoadReportDefinition(stream);
                    report.DataSources.Clear();
                    report.DataSources.Add(new ReportDataSource(this.mDSName,ds.Tables[mTBLName]));

                    //Set the report parameters for the report
                    ReportParameter terminalID = new ReportParameter("TerminalID",_terminal);
                    ReportParameter start = new ReportParameter("Date",_start);
                    ReportParameter shift = new ReportParameter("ShiftNumber",_shift.ToString());
                    ReportParameter terminal = new ReportParameter("SortedLocation",this.cboTerminal.SelectedItem.Text);
                    ReportParameter freigthType = new ReportParameter("FreightType",_header);

                    DataSet shifts = enterprise.GetShifts(_terminal,DateTime.Parse(this.txtShiftDate.Text));
                    DataRow row = shifts.Tables["ShiftTable"].Select("NUMBER='" + this.cboShift.SelectedValue.ToString() + "'")[0];
                    ReportParameter shiftTimeIn = new ReportParameter("ShiftTimeIn",DateTime.Parse(row["StartTime"].ToString()).ToString("yyyy-MM-dd"));
                    ReportParameter shiftTimeOut = new ReportParameter("ShiftTimeOut",DateTime.Parse(row["EndTime"].ToString()).ToString("yyyy-MM-dd"));
                    ReportParameter shiftBreak = new ReportParameter("ShiftBreak",DateTime.Parse(row["BreakTime"].ToString()).ToString("yyyy-MM-dd"));
                    ReportParameter shiftProdTime = new ReportParameter("ProductionTime",string.Concat(Decimal.Truncate((Convert.ToDecimal(row["ProductionTime"]) / 60)),":",Convert.ToString(Convert.ToDecimal(row["ProductionTime"]) % 60)));
                    report.SetParameters(new ReportParameter[] { terminalID,start,shift,terminal,shiftTimeIn,shiftTimeOut,shiftBreak,shiftProdTime,freigthType });

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
                    Response.AddHeader("Content-Disposition","inline; filename=Shift.xls");
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