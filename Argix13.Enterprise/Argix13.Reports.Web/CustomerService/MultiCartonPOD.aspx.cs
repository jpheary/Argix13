using System;
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

public partial class MultiCartonPOD :System.Web.UI.Page {
    //Members
    private const string TITLE = "MultiCarton POD";
    private const string CONFIG_FILESHARE = "MultiCartonPOD_ExcelFileShare";
    private const string SOURCE = "/Customer Service/MultiCarton POD";
    private string mDSName = "DataSet1";
    private string mUSPName = "uspTrackingCartonsFromText",mTBLName = "NewTable";
    private const string USP_REPORT_BYTRACKINGNUMBER = "uspTrackingCartonsFromTextByTrackingNumbers";
    private const string USP_REPORT_BYLABELSEQNUMBER = "uspTrackingCartonsFromTextByArgixTrackingNumbers";

    //Interface
    protected void Page_Load(object sender,EventArgs e) {
        //Event handler for page load event
        if(!Page.IsPostBack) {
            //Initialize control values
            Master.ReportTitle = TITLE + " Report";
            this.cboClient.DataBind();
            if(this.cboClient.Items.Count > 0) this.cboClient.SelectedIndex = 0;
            OnClientChanged(null,EventArgs.Empty);
        }
        Master.ButtonCommand += new CommandEventHandler(OnButtonCommand);
    }
    protected void OnClientChanged(object sender,EventArgs e) {
        //Event handler for change in selected client
        //Load vendors for this client
        this.cboVendor.Items.Clear();
        if(this.cboClient.SelectedValue != "") this.cboVendor.Items.Add(new ListItem("All",""));
        this.cboVendor.DataBind();
        if(this.cboVendor.Items.Count > 0) this.cboVendor.SelectedIndex = 0;
        OnVendorChanged(null,EventArgs.Empty);
    }
    protected void OnVendorChanged(object sender,EventArgs e) {
        //Event handler for change in selected vendor
        OnValidateForm(null,EventArgs.Empty);
    }
    protected void OnNumbersChanged(object sender,EventArgs e) {
        //Event handler for change in numbers
        OnValidateForm(null,EventArgs.Empty);
    }
    protected void OnValidateForm(object sender,EventArgs e) {
        //Event handler for changes in parameter data
        Master.Validated = (this.cboVendor.SelectedItem.Text == "All" || this.cboVendor.SelectedValue.Length > 0) && this.txtNumbers.Text.Length > 0;
    }
    protected void OnButtonCommand(object sender,CommandEventArgs e) {
        //Event handler for command button clicked
        try {
            //Change view to Viewer and reset to clear existing data
            Master.Viewer.Reset();

            //Get parameters for the query
            string _client = this.cboClient.SelectedValue != "" ? this.cboClient.SelectedValue : null;
            string _vendor = this.cboVendor.SelectedValue != "" ? this.cboVendor.SelectedValue : null;
            string _numbers = this.txtNumbers.Text.Replace("\r\n",",");

            //Initialize control values
            LocalReport report = Master.Viewer.LocalReport;
            report.DisplayName = TITLE;
            report.EnableExternalImages = true;
            EnterpriseRGateway enterprise = new EnterpriseRGateway();
            string sp = this.mUSPName;
            switch (this.cboBy.SelectedValue.ToLower()) {
                case "carton": sp = this.mUSPName; break;
                case "tracking": sp = USP_REPORT_BYTRACKINGNUMBER; break;
                case "labelsequence": sp = USP_REPORT_BYLABELSEQNUMBER; break;
            }
            DataSet ds = enterprise.FillDataset(sp,this.mTBLName,new object[] { _numbers,_client,_vendor });
            if (ds.Tables[mTBLName] == null) ds.Tables.Add(mTBLName);
            switch(e.CommandName) {
                case "Run":
                    //Set local report and data source
                    System.IO.Stream stream = Master.GetReportDefinition(SOURCE);
                    report.LoadReportDefinition(stream);
                    report.DataSources.Clear();
                    report.DataSources.Add(new ReportDataSource(this.mDSName, ds.Tables[mTBLName]));

                    //Set the report parameters for the report
                    ReportParameter numbers = new ReportParameter("Text", _numbers);
                    ReportParameter client = new ReportParameter("Client", _client);
                    ReportParameter vendor = new ReportParameter("Vendor", _vendor);
                    report.SetParameters(new ReportParameter[] { numbers, client, vendor });

                    //Update report rendering with new data
                    report.Refresh();

                    if (!Master.Viewer.Enabled) Master.Viewer.CurrentPage = 1;
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
                    Response.AddHeader("Content-Disposition","inline; filename=MultiCartonPOD.xls");
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
