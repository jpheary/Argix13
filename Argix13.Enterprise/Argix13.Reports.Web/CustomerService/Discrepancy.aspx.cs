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

public partial class Discrepancy :System.Web.UI.Page {
    //Members
    private const string TITLE = "Discrepancy";
    private const string SOURCE = "";
    private string mDSName = "";
    private string mUSPName = "",mTBLName = "NewTable";
    private const string REPORT_SRC_TSORT_STATE = "/Customer Service/Discrepancy For Tsort By State", REPORT_SRC_TSORT_STORE = "/Customer Service/Discrepancy For Tsort By Store", REPORT_SRC_TSORT_ZONE = "/Customer Service/Discrepancy For Tsort By Zone";
    private const string REPORT_SRC_RETURNS_STORE = "/Customer Service/Discrepancy For Returns By Store", REPORT_SRC_RETURNS_VENDOR = "/Customer Service/Discrepancy For Returns By Vendor";
    private const string REPORT_DS_TSORT = "DiscrepancyTsortDS", REPORT_DS_RETURNS = "DiscrepancyReturnsDS";
    private const string USP_REPORT_VENDOR = "uspRptVendorDiscrepancyGetList", USP_REPORT_AGENT = "uspRptAgentDiscrepancyGetList";
   
    //Interface
    protected void Page_Load(object sender,EventArgs e) {
        //Event handler for page load event
        if(!Page.IsPostBack && !ScriptManager.GetCurrent(Page).IsInAsyncPostBack) {
            //Initialize control values
            Master.ReportTitle = TITLE + " Report";
            this.txtFrom.Text = this.txtTo.Text = DateTime.Today.ToString("MM/dd/yyyy");

            this.odsClients.DataBind();
            this.grdClients.SelectedIndex = -1;
            this.grdClients.Enabled = this.txtFindClient.Enabled = this.imgFindClient.Enabled = this.grdClients.Rows.Count > 0;
            this.grdShippers.Enabled = this.txtFindShipper.Enabled = this.imgFindShipper.Enabled = false;

            OnFreightTypeChanged(null, EventArgs.Empty);
        }
        Master.ButtonCommand += new CommandEventHandler(OnButtonCommand);
    }
    protected void OnFreightTypeChanged(object sender,EventArgs e) {
        //Event handler for change in selected freight type
        try {
            this.cboViewBy.Items.Clear();
            if(this.ddlFreightType.SelectedValue.ToLower() == "tsort") {
                //Configure for vendors as shippers
                this.cboViewBy.Items.Add("Store");
                this.cboViewBy.Items.Add("Zone");
                this.cboViewBy.Items.Add("State");
                this.ddlShipper.SelectedValue = "0";
            }
            else if(this.ddlFreightType.SelectedValue.ToLower() == "returns") {
                //Configure for agents as shippers
                this.cboViewBy.Items.Add("Store");
                this.cboViewBy.Items.Add("Vendor");
                this.ddlShipper.SelectedValue = "1";
            }
            this.cboViewBy.SelectedIndex = 0;
            OnClientSelected(null, null);
        }
        catch(Exception ex) { Master.ReportError(ex); }
        finally { OnValidateForm(null, EventArgs.Empty); }
    }
    protected void OnFindClient(object sender, ImageClickEventArgs e) {
        //Event handler for client search
        OnClientSearch(sender, EventArgs.Empty);
    }
    protected void OnClientSearch(object sender, EventArgs e) {
        //Event handler for client search
        findRow(this.grdClients, 1, this.txtFindClient.Text);
        OnClientSelected(this.grdClients, EventArgs.Empty);
        ScriptManager.RegisterStartupScript(this.txtFindClient, typeof(TextBox), "ScrollClients", "scrollClients('" + this.txtFindClient.Text + "');", true);
    }
    protected void OnClientSelected(object sender, EventArgs e) {
        //Event handler for change in selected client
        this.grdShippers.DataBind();
        this.grdShippers.SelectedIndex = -1;
        this.grdShippers.Enabled = this.txtFindShipper.Enabled = this.imgFindShipper.Enabled = this.grdShippers.Rows.Count > 0;
    }
    protected void OnFindShipper(object sender, ImageClickEventArgs e) {
        //Event handler for vendor search
        OnShipperSearch(sender, EventArgs.Empty);
    }
    protected void OnShipperSearch(object sender, EventArgs e) {
        //Event handler for vendor search
        findRow(this.grdShippers, 1, this.txtFindShipper.Text);
        ScriptManager.RegisterStartupScript(this.txtFindShipper, typeof(TextBox), "ScrollShippers", "scrollShippers('" + this.txtFindShipper.Text + "');", true);
    }
    protected void OnShipperCheckedChanged(object sender, EventArgs e) {
        //
        OnValidateForm(null, EventArgs.Empty);
    }
    protected void OnValidateForm(object sender,EventArgs e) {
        //Event handler for changes in parameter data
        Master.Validated = selectedShippers.Length > 0;
    }
    protected void OnButtonCommand(object sender, CommandEventArgs e) {
        //Event handler for command button clicked
        try {
            //Change view to Viewer and reset to clear existing data
            Master.Viewer.Reset();

            //Get parameters for the query
            string _fromDate = DateTime.Parse(this.txtFrom.Text).ToString("yyyy-MM-dd");
            string _toDate = DateTime.Parse(this.txtTo.Text).ToString("yyyy-MM-dd");
            string _client = this.grdClients.SelectedRow.Cells[1].Text;
            string _div = this.grdClients.SelectedRow.Cells[2].Text;
            string _clientName = this.grdClients.SelectedRow.Cells[3].Text;
            string _shipper = "", _shipperName = "";

            //Initialize control values
            LocalReport report = Master.Viewer.LocalReport;
            report.DisplayName = TITLE;
            report.EnableExternalImages = true;
            EnterpriseRGateway enterprise = new EnterpriseRGateway();
            DataSet ds = new DataSet();
            string usp = "", reportDS = "", reportFile = "";
            if(this.ddlFreightType.SelectedValue.ToLower() == "tsort") {
                usp = USP_REPORT_VENDOR;
                reportDS = REPORT_DS_TSORT;
                switch(this.cboViewBy.SelectedItem.Text.ToLower()) {
                    case "state": reportFile = REPORT_SRC_TSORT_STATE; break;
                    case "zone": reportFile = REPORT_SRC_TSORT_ZONE; break;
                    case "store": reportFile = REPORT_SRC_TSORT_STORE; break;
                }
            }
            else if(this.ddlFreightType.SelectedValue.ToLower() == "returns") {
                usp = USP_REPORT_AGENT;
                reportDS = REPORT_DS_RETURNS;
                switch(this.cboViewBy.SelectedItem.Text.ToLower()) {
                    case "vendor": reportFile = REPORT_SRC_RETURNS_VENDOR; break;
                    case "store": reportFile = REPORT_SRC_RETURNS_STORE; break;
                }
            }
            foreach(GridViewRow row in selectedShippers) {
                string __shipper = row.Cells[1].Text;
                _shipper = _shipper.Length == 0 ? __shipper : _shipper + ", " + __shipper;
                DataSet _ds = enterprise.FillDataset(usp, mTBLName, new object[] { _client, _div, __shipper, _fromDate, _toDate });
                ds.Merge(_ds);
            }
            if(ds.Tables[mTBLName] == null) ds.Tables.Add(mTBLName);
            switch(e.CommandName) {
                case "Run":
                    //Set local report and data source
                    System.IO.Stream stream = Master.GetReportDefinition(reportFile);
                    report.LoadReportDefinition(stream);
                    report.DataSources.Clear();
                    report.DataSources.Add(new ReportDataSource(reportDS,ds.Tables[mTBLName]));

                    //Set the report parameters for the report
                    ReportParameter fromDate = new ReportParameter("StartPUDate",_fromDate);
                    ReportParameter toDate = new ReportParameter("EndPUDate",_toDate);
                    ReportParameter client = new ReportParameter("ClientNumber",_client);
                    ReportParameter div = new ReportParameter("ClientDivision",_div);
                    ReportParameter clientName = new ReportParameter("ClientName",_clientName);
                    ReportParameter shipper = new ReportParameter(this.ddlShipper.SelectedValue == "0" ? "VendorNumber" : "AgentNumber", _shipper);
                    ReportParameter shipperName = new ReportParameter(this.ddlShipper.SelectedValue == "0" ? "VendorName" : "AgentName", _shipperName);
                    report.SetParameters(new ReportParameter[] { client,div,fromDate,toDate,clientName,shipper,shipperName });

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
                    Response.AddHeader("Content-Disposition","inline; filename=Discrepancy.xls");
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

    private void findRow(GridView grid, int colIndex, string searchWord) {
        //Event handler for change in search text value
        GridViewRow oRow = null, oRowSimiliar = null, oRowMatch = null;
        string sCellText = "";
        long lCellValue = 0, lSearchValue = 0;
        int iLword = 0, iL = 0, iRows = 0, i = 0, j = 0;
        bool bASC = true, bIsNumeric = false, bHigher = false;

        //Check for rows
        if(grid.Rows.Count == 0)
            return;

        //Get specifics for search word and grid
        iLword = searchWord.Length;
        iRows = grid.Rows.Count;
        if(colIndex < grid.Columns.Count && searchWord.Length > 0) {
            //Initial search conditions
            bASC = (grid.SortDirection == SortDirection.Ascending);
            bIsNumeric = (grid.Columns[colIndex].GetType() == Type.GetType("System.Int32"));
            i = 0;
            while(i < iRows) {
                //Get next row, cell value, and cell length
                oRow = grid.Rows[i];
                if(bIsNumeric) {
                    lCellValue = Convert.ToInt64(oRow.Cells[colIndex].Text);
                    try { lSearchValue = Convert.ToInt64(searchWord); }
                    catch(FormatException) { lSearchValue = 0; }
                    if(bASC) {
                        if(lSearchValue == lCellValue)
                            oRowMatch = oRow;
                        else if(lSearchValue > lCellValue)
                            oRowSimiliar = oRow;
                    }
                    else {
                        if(lSearchValue == lCellValue)
                            oRowMatch = oRow;
                        else if(lSearchValue < lCellValue)
                            oRowSimiliar = oRow;
                    }
                }
                else {
                    sCellText = oRow.Cells[colIndex].Text;
                    iL = sCellText.Length;
                    if(iL > 0) {
                        //Compare a substring of the cell text with the search word
                        for(j = 1; j <= iL; j++) {
                            if(sCellText.Substring(0, j).ToUpper() == searchWord.Substring(0, j).ToUpper()) {
                                if(j == iLword) {
                                    //Exact match
                                    oRowMatch = oRow;
                                    break;
                                }
                                else {
                                    if(j == iL) {
                                        //Close match
                                        oRowSimiliar = oRow;
                                        break;
                                    }
                                }
                            }
                            else {
                                //Is search word alphabetically higher than cell?
                                if(bASC)
                                    bHigher = (searchWord.ToUpper().ToCharArray()[j - 1] > sCellText.ToUpper().ToCharArray()[j - 1]);
                                else
                                    bHigher = (searchWord.ToUpper().ToCharArray()[j - 1] < sCellText.ToUpper().ToCharArray()[j - 1]);
                                if(bHigher)
                                    oRowSimiliar = oRow;
                                break;
                            }
                        }
                    }
                }
                if(oRowMatch != null) break;
                i++;
            }
            //Select match or closest row
            if(iRows > 0 && oRowMatch == null)
                oRowMatch = (oRowSimiliar != null) ? oRowSimiliar : grid.Rows[0];
            grid.SelectedIndex = oRowMatch.RowIndex;
        }
    }
    private GridViewRow[] selectedShippers {
        get {
            GridViewRow[] rows = new GridViewRow[] { };
            int i = 0;
            foreach(GridViewRow row in this.grdShippers.Rows) {
                bool isChecked = ((CheckBox)row.FindControl("chkSelect")).Checked;
                if(isChecked) i++;
            }
            if(i > 0) {
                rows = new GridViewRow[i];
                int j = 0;
                foreach(GridViewRow row in this.grdShippers.Rows) {
                    bool isChecked = ((CheckBox)row.FindControl("chkSelect")).Checked;
                    if(isChecked) rows[j++] = row;
                }
            }
            return rows;
        }
    }
}
