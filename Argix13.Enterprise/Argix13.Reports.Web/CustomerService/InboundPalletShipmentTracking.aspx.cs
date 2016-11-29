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

public partial class InboundPalletShipmentTracking:System.Web.UI.Page {
    //Members
    private const string TITLE = "Inbound Pallet Shipment Tracking";
    private const string SOURCE = "/Customer Service/Pallet Shipment Inbound Tracking";
    private string mDSName = "DataSet1";
    private string mUSPName = "uspRptInboundPalletShipmentTracking",mTBLName = "NewTable";
   
    //Interface
    protected void Page_Load(object sender,EventArgs e) {
        //Event handler for page load event
        if(!Page.IsPostBack) {
            //Initialize control values
            Master.ReportTitle = TITLE + " Report";

            DataSet ds = new EnterpriseRGateway().GetClients("", "01", false);
            for(int i = 0; i < ds.Tables["ClientTable"].Rows.Count; i++) {
                if (ds.Tables["ClientTable"].Rows[i]["ClientNumber"].ToString().Trim().Substring(0,1) != "L") ds.Tables["ClientTable"].Rows[i].Delete();
            }
            ds.Tables["ClientTable"].Rows.Add(new object[] { "","","","","" });
            ds.AcceptChanges();
            DataView dv = new DataView(ds.Tables["ClientTable"],"","ClientName",DataViewRowState.CurrentRows);
            this.cboClient.DataSource = dv;
            this.cboClient.DataBind();
            if(this.cboClient.Items.Count > 0) this.cboClient.SelectedIndex = 0;
            this.txtFrom.Text = this.txtTo.Text = DateTime.Today.ToString("MM/dd/yyyy");
            OnValidateForm(null,EventArgs.Empty);
        }
        Master.ButtonCommand += new CommandEventHandler(OnButtonCommand);
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
            string _client = this.cboClient.SelectedValue == "" ? null : this.cboClient.SelectedValue;
            DateTime _pustartdate = DateTime.Parse(this.txtFrom.Text);
            DateTime _puenddate = DateTime.Parse(this.txtTo.Text);

            //Initialize control values
            LocalReport report = Master.Viewer.LocalReport;
            report.DisplayName = TITLE;
            report.EnableExternalImages = true;
            EnterpriseRGateway enterprise = new EnterpriseRGateway();
            DataSet ds = enterprise.FillDataset(this.mUSPName, mTBLName, new object[] { _client, _pustartdate, _puenddate });
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
                    ReportParameter pustartdate = new ReportParameter("PickupStartDate", _pustartdate.ToString("yyyy-MM-dd"));
                    ReportParameter puenddate = new ReportParameter("PickupEndDate", _puenddate.ToString("yyyy-MM-dd"));
                    report.SetParameters(new ReportParameter[] { client, pustartdate, puenddate });

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
                    Response.AddHeader("Content-Disposition","inline; filename=InboundTracking.xls");
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
