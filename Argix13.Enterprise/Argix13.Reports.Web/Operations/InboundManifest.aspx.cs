using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Microsoft.Reporting.WebForms;
using Argix;

public partial class InboundManifest :System.Web.UI.Page {
    //Members
    private const string TITLE = "Inbound Manifest";
    private const string SOURCE = "/Operations/Inbound Manifest";
    private string mDSName = "InboundManifestDS";
    private string mUSPName = "uspRptManifestDetailGetList",mTBLName = "NewTable";
   
    //Interface
    protected void Page_Load(object sender,EventArgs e) {
        //Event handler for page load event
        try {
            if(!Page.IsPostBack && !ScriptManager.GetCurrent(Page).IsInAsyncPostBack) {
                //Initialize control values
                Master.ReportTitle = TITLE + " Report";
                this.txtFrom.Text = this.txtTo.Text = DateTime.Today.ToString("MM/dd/yyyyy");
                this.odsClients.DataBind();
                this.grdClients.SelectedIndex = -1;
                this.grdClients.Enabled = this.txtFindClient.Enabled = this.imgFindClient.Enabled = this.grdClients.Rows.Count > 0;
                this.grdVendorLog.Enabled = this.txtFindLogEntry.Enabled = this.imgFindLogEntry.Enabled = false;
                OnDateChanged(null, EventArgs.Empty);
            }
            Master.ButtonCommand += new CommandEventHandler(OnButtonCommand);
        }
        catch(Exception ex) { Master.ReportError(ex); }
        finally { OnValidateForm(null, EventArgs.Empty); }
    }
    protected void OnDateChanged(object sender,EventArgs e) {
        //Update vendor list
        try {
            if(this.txtFrom.Text.Trim().Length > 0) this.hfStartDate.Value = DateTime.Parse(this.txtFrom.Text).ToShortDateString();
            if(this.txtTo.Text.Trim().Length > 0) this.hfEndDate.Value = DateTime.Parse(this.txtTo.Text).AddDays(1).AddMilliseconds(-1).ToShortDateString();
        }
        catch(Exception ex) { Master.ReportError(ex); }
        finally { OnValidateForm(null, EventArgs.Empty); }
    }
    protected void OnClientSelected(object sender,EventArgs e) {
        //Event handler for change in selected client
        try {
            //Clear vendor selection; forward event to client
            this.grdVendorLog.DataBind();
            this.grdVendorLog.SelectedIndex = -1;
            this.grdVendorLog.Enabled = this.txtFindLogEntry.Enabled = this.imgFindLogEntry.Enabled = this.grdVendorLog.Rows.Count > 0;
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
    protected void OnVendorLogCheckedChanged(object sender, EventArgs e) {
        //Event handler for change in selected vendor log
        OnValidateForm(null,EventArgs.Empty);
    }
    protected void OnFindVendorLogEntry(object sender, ImageClickEventArgs e) {
        //Event handler for vendor search
        OnVendorLogSearch(sender, EventArgs.Empty);
    }
    protected void OnVendorLogSearch(object sender, EventArgs e) {
        //Event handler for vendor search
        findRow(this.grdVendorLog, 1, this.txtFindLogEntry.Text);
        OnVendorLogCheckedChanged(this.grdVendorLog, EventArgs.Empty);
        ScriptManager.RegisterStartupScript(this.txtFindLogEntry, typeof(TextBox), "ScrollVendorLog", "scrollVendors('" + this.txtFindLogEntry.Text + "');", true);
    }
    protected void OnValidateForm(object sender, EventArgs e) {
        //Event handler for changes in parameter data
        try {
            Master.Validated = selectedRows.Length > 0;
        }
        catch(Exception ex) { Master.ReportError(ex); }
    }
    protected void OnButtonCommand(object sender, CommandEventArgs e) {
        //Event handler for view button clicked
        try {
            //Change view to Viewer and reset to clear existing data
            Master.Viewer.Reset();

            //Get parameters for the query
            string _client = this.grdClients.SelectedRow.Cells[1].Text;
            string _div = this.grdClients.SelectedRow.Cells[2].Text;
            string _name = this.grdClients.SelectedRow.Cells[3].Text;
            string _manifestID = "",_pickupDate = "",_pickupNum = "";

            LocalReport report = Master.Viewer.LocalReport;
            report.DisplayName = TITLE;
            report.EnableExternalImages = true;
            EnterpriseRGateway enterprise = new EnterpriseRGateway();
            DataSet ds = new DataSet(this.mDSName);
            foreach(GridViewRow row in selectedRows) {
                DataKey dataKey = (DataKey)this.grdVendorLog.DataKeys[row.RowIndex];
                _manifestID = dataKey["ID"].ToString();
                _pickupDate = DateTime.Parse(dataKey["PickupDate"].ToString()).ToString("MM-dd-yyyy");
                _pickupNum = dataKey["PickupNumber"].ToString();
                DataSet _ds = enterprise.FillDataset(this.mUSPName,mTBLName,new object[] { _manifestID });
                _ds.Tables[mTBLName].Columns.Add("ManifestID");
                _ds.Tables[mTBLName].Columns.Add("PickupDate");
                _ds.Tables[mTBLName].Columns.Add("PickupNumber");
                for(int j = 0;j < _ds.Tables[mTBLName].Rows.Count;j++) {
                    _ds.Tables[mTBLName].Rows[j]["ManifestID"] = _manifestID;
                    _ds.Tables[mTBLName].Rows[j]["PickupDate"] = _pickupDate;
                    _ds.Tables[mTBLName].Rows[j]["PickupNumber"] = _pickupNum;
                }
                ds.Merge(_ds);
            }
            if (ds.Tables[mTBLName] == null) ds.Tables.Add(mTBLName);
            switch(e.CommandName) {
                case "Run":
                    //Set local report and data source
                    System.IO.Stream stream = Master.GetReportDefinition(SOURCE);
                    report.LoadReportDefinition(stream);
                    report.DataSources.Clear();
                    report.DataSources.Add(new ReportDataSource(this.mDSName,ds.Tables[mTBLName]));

                    //Set the report parameters for the report
                    ReportParameter manifestID = new ReportParameter("ManifestID", "");
                    ReportParameter client = new ReportParameter("ClientNumber",_client);
                    ReportParameter div = new ReportParameter("ClientDivision",_div);
                    ReportParameter name = new ReportParameter("ClientName",_name);
                    ReportParameter punumber = new ReportParameter("PUNumber","");
                    ReportParameter pudate = new ReportParameter("PUDate", "");
                    report.SetParameters(new ReportParameter[] { manifestID,client,div,name,punumber,pudate });

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

    #region Local Services: selectedRows(), findRow()
    private GridViewRow[] selectedRows {
        get {
            GridViewRow[] rows = new GridViewRow[] { };
            int i = 0;
            foreach(GridViewRow row in this.grdVendorLog.Rows) {
                bool isChecked = ((CheckBox)row.FindControl("chkSelect")).Checked;
                if(isChecked) i++;
            }
            if(i > 0) {
                rows = new GridViewRow[i];
                int j = 0;
                foreach(GridViewRow row in this.grdVendorLog.Rows) {
                    bool isChecked = ((CheckBox)row.FindControl("chkSelect")).Checked;
                    if(isChecked) rows[j++] = row;
                }
            }
            return rows;
        }
    }
    private void findRow(GridView grid, int colIndex, string searchWord) {
        //Event handler for change in search text value
        GridViewRow oRow = null, oRowSimiliar = null, oRowMatch = null;
        string sCellText = "";
        long lCellValue = 0, lSearchValue = 0;
        int iLword = 0, iL = 0, iRows = 0, i = 0, j = 0;
        bool bASC = true, bIsNumeric = false, bHigher = false;

        //Validate
        if(grid.Rows.Count == 0) return;

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
                                //Look for exact match or closest match
                                if(j == iLword) {
                                    oRowMatch = oRow; break;
                                }
                                else {
                                    if(j == iL) { oRowSimiliar = oRow; break; }
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
    #endregion
}
