using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Microsoft.Reporting.WebForms;

public partial class SiteMaster:MasterPage {
    //Members
    private const string AntiXsrfTokenKey = "__AntiXsrfToken";
    private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
    private string _antiXsrfTokenValue;

    public event CommandEventHandler ButtonCommand = null;

    private const string ITEM_SUBPATH = "/Argix08.Reports";

    //Interface
    public string ReportTitle { get { return this.lblReportTitle.Text; } set { this.lblReportTitle.Text = value; } }
    public ReportViewer Viewer { get { return this.rsViewer; } }
    public bool Validated { set { this.btnRun.Enabled = this.btnData.Enabled = value; } }
    public string Status { set { ShowMsgBox(value); } }
    public void ReportError(Exception ex) { reportError(ex); }
    public Stream GetReportDefinition(string report) {
        //Return a report definition from the SQL reporting server
        Stream stream = null;
        try { 
            microsoft.ReportingService2010 rs = new microsoft.ReportingService2010();
            rs.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
            byte[] bytes = rs.GetItemDefinition(ITEM_SUBPATH + report);
            stream = new System.IO.MemoryStream(bytes);
        }
        catch (Exception ex) { reportError(ex); }
        return stream;
    }
    public Stream CreateExportRdl(DataSet ds,string dataSetName) {
        //Open a new stream for writing
        XmlTextWriter writer = null;
        try {
            writer = new XmlTextWriter(new MemoryStream(),Encoding.UTF8);
            writer.Formatting = Formatting.Indented;

            //Create list of dataset fields
            ArrayList fields = new ArrayList();
            for (int i = 0;i <= ds.Tables[0].Columns.Count - 1;i++)
                fields.Add(ds.Tables[0].Columns[i].ColumnName);

            //Report element
            writer.WriteProcessingInstruction("xml","version=\"1.0\" encoding=\"utf-8\"");
            writer.WriteStartElement("Report");
            writer.WriteAttributeString("xmlns",null,"http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition");
            #region <Body>...</Body>
            writer.WriteStartElement("Body");
            writer.WriteStartElement("ReportItems");
            writer.WriteStartElement("Tablix");
            writer.WriteAttributeString("Name","Tablix1");
            #region <TablixBody>...</TablixBody>
            writer.WriteStartElement("TablixBody");
            #region <TablixColumns>...</TablixColumns>
            writer.WriteStartElement("TablixColumns");
            foreach (string fieldName in fields) {
                writer.WriteStartElement("TablixColumn");
                writer.WriteElementString("Width","0.75in");
                writer.WriteEndElement(); //TablixColumn
            }
            writer.WriteEndElement(); //TablixColumns
            #endregion
            #region <TablixRows>...</TablixRows>
            writer.WriteStartElement("TablixRows");
            #region <TablixRow>...</TablixRow>
            writer.WriteStartElement("TablixRow");
            writer.WriteElementString("Height","0.25in");
            writer.WriteStartElement("TablixCells");
            foreach (string fieldName in fields) {
                writer.WriteStartElement("TablixCell");
                writer.WriteStartElement("CellContents");
                writer.WriteStartElement("Textbox");
                writer.WriteAttributeString("Name","hdr" + System.Xml.XmlConvert.EncodeName(fieldName));
                writer.WriteElementString("CanGrow","false");
                writer.WriteElementString("KeepTogether","true");
                writer.WriteStartElement("Paragraphs");
                writer.WriteStartElement("Paragraph");
                writer.WriteStartElement("TextRuns");
                writer.WriteStartElement("TextRun");
                writer.WriteElementString("Value",fieldName);
                writer.WriteStartElement("Style");
                writer.WriteElementString("FontFamily","Tahoma");
                writer.WriteElementString("FontSize","8pt");
                writer.WriteEndElement(); //Style
                writer.WriteEndElement(); //TextRun
                writer.WriteEndElement(); //TextRuns
                writer.WriteEndElement(); //Paragraph
                writer.WriteEndElement(); //Paragraphs
                writer.WriteStartElement("Style");
                writer.WriteStartElement("Border");
                writer.WriteElementString("Color","LightGrey");
                writer.WriteElementString("Style","Solid");
                writer.WriteEndElement(); //Border
                writer.WriteElementString("PaddingLeft","2pt");
                writer.WriteElementString("PaddingRight","2pt");
                writer.WriteElementString("PaddingTop","2pt");
                writer.WriteElementString("PaddingBottom","2pt");
                writer.WriteEndElement(); //Style
                writer.WriteEndElement(); //Textbox
                writer.WriteEndElement(); //CellContents
                writer.WriteEndElement(); //TablixCell
            }
            writer.WriteEndElement(); //TablixCells
            writer.WriteEndElement(); //TablixRow
            #endregion
            #region <TablixRow>...</TablixRow>
            writer.WriteStartElement("TablixRow");
            writer.WriteElementString("Height","0.20in");
            writer.WriteStartElement("TablixCells");
            foreach (string fieldName in fields) {
                writer.WriteStartElement("TablixCell");
                writer.WriteStartElement("CellContents");
                writer.WriteStartElement("Textbox");
                writer.WriteAttributeString("Name","txt" + System.Xml.XmlConvert.EncodeName(fieldName));
                writer.WriteElementString("CanGrow","false");
                writer.WriteElementString("KeepTogether","true");
                writer.WriteStartElement("Paragraphs");
                writer.WriteStartElement("Paragraph");
                writer.WriteStartElement("TextRuns");
                writer.WriteStartElement("TextRun");
                writer.WriteElementString("Value","=Fields!" + System.Xml.XmlConvert.EncodeName(fieldName) + ".Value");
                writer.WriteStartElement("Style");
                writer.WriteElementString("FontFamily","Tahoma");
                writer.WriteElementString("FontSize","8pt");
                writer.WriteEndElement(); //Style
                writer.WriteEndElement(); //TextRun
                writer.WriteEndElement(); //TextRuns
                writer.WriteEndElement(); //Paragraph
                writer.WriteEndElement(); //Paragraphs
                writer.WriteStartElement("Style");
                writer.WriteStartElement("Border");
                writer.WriteElementString("Color","LightGrey");
                writer.WriteElementString("Style","Solid");
                writer.WriteEndElement(); //Border
                writer.WriteElementString("PaddingLeft","2pt");
                writer.WriteElementString("PaddingRight","2pt");
                writer.WriteElementString("PaddingTop","2pt");
                writer.WriteElementString("PaddingBottom","2pt");
                writer.WriteEndElement(); //Style
                writer.WriteEndElement(); //Textbox
                writer.WriteEndElement(); //CellContents
                writer.WriteEndElement(); //TablixCell
            }
            writer.WriteEndElement(); //TablixCells
            writer.WriteEndElement(); //TablixRow
            #endregion
            writer.WriteEndElement(); //TablixRows
            #endregion
            writer.WriteEndElement(); //TablixBody
            #endregion
            #region <TablixColumnHierarchy>...</TablixColumnHierarchy>
            writer.WriteStartElement("TablixColumnHierarchy");
            writer.WriteStartElement("TablixMembers");
            foreach (string fieldName in fields) {
                writer.WriteStartElement("TablixMember");
                writer.WriteEndElement(); //TablixMember
            }
            writer.WriteEndElement(); //TablixMembers
            writer.WriteEndElement(); //TablixColumnHierarchy
            #endregion
            #region <TablixRowHierarchy>...</TablixRowHierarchy>
            writer.WriteStartElement("TablixRowHierarchy");
            writer.WriteStartElement("TablixMembers");
            writer.WriteStartElement("TablixMember");
            writer.WriteElementString("KeepWithGroup","After");
            writer.WriteEndElement(); //TablixMember
            writer.WriteStartElement("TablixMember");
            writer.WriteStartElement("Group");
            writer.WriteAttributeString("Name","Details");
            writer.WriteEndElement(); //Group
            writer.WriteEndElement(); //TablixMember
            writer.WriteEndElement(); //TablixMembers
            writer.WriteEndElement(); //TablixRowHierarchy
            #endregion
            writer.WriteElementString("DataSetName",dataSetName);
            writer.WriteElementString("Top","0in");
            writer.WriteElementString("Left","0in");
            writer.WriteElementString("Height","0.5in");
            writer.WriteElementString("Width","10.7in");
            writer.WriteStartElement("Style");
            writer.WriteStartElement("Border");
            writer.WriteElementString("Style","None");
            writer.WriteEndElement(); //Border
            writer.WriteEndElement(); //Style
            writer.WriteEndElement(); //Tablix
            writer.WriteEndElement(); //ReportItems
            writer.WriteElementString("Height","1.2in");
            writer.WriteStartElement("Style");
            writer.WriteEndElement(); //Style
            writer.WriteEndElement(); //Body
            #endregion
            #region <Page>...</Page>
            writer.WriteStartElement("Page");
            writer.WriteElementString("PageHeight","8.5in");
            writer.WriteElementString("PageWidth","11in");
            writer.WriteElementString("InteractiveHeight","11in");
            writer.WriteElementString("InteractiveWidth","8.5in");
            writer.WriteElementString("LeftMargin","0.4in");
            writer.WriteElementString("RightMargin","0.4in");
            writer.WriteElementString("TopMargin","0.4in");
            writer.WriteElementString("BottomMargin","0.4in");
            writer.WriteStartElement("Style");
            writer.WriteElementString("BackgroundColor","White");
            writer.WriteEndElement(); //Style
            writer.WriteEndElement(); //Page
            #endregion
            #region <DataSources>...</DataSources>
            writer.WriteStartElement("DataSources");
            writer.WriteStartElement("DataSource");
            writer.WriteAttributeString("Name","TSORT");
            writer.WriteElementString("DataSourceReference","RGXVMSQLR.TSORTR");
            writer.WriteEndElement(); //DataSource
            writer.WriteEndElement(); //DataSources
            #endregion
            #region <DataSets>...</DataSets>
            writer.WriteStartElement("DataSets");
            writer.WriteStartElement("DataSet");
            writer.WriteAttributeString("Name",dataSetName);
            writer.WriteStartElement("Query");
            writer.WriteElementString("DataSourceName","TSORT");
            writer.WriteElementString("CommandType","StoredProcedure");
            writer.WriteElementString("CommandText","usp");
            writer.WriteEndElement(); //Query
            writer.WriteStartElement("Fields");
            foreach (string fieldName in fields) {
                writer.WriteStartElement("Field");
                writer.WriteAttributeString("Name",System.Xml.XmlConvert.EncodeName(fieldName));
                writer.WriteElementString("DataField",fieldName);
                writer.WriteEndElement(); //Field
            }
            writer.WriteEndElement(); //Fields
            writer.WriteEndElement(); //DataSet
            writer.WriteEndElement(); //DataSets
            #endregion
            writer.WriteElementString("Width","11.0");
            writer.WriteElementString("AutoRefresh","0");
            writer.WriteElementString("Language","en-US");
            writer.WriteElementString("ConsumeContainerWhitespace","true");
            writer.WriteEndElement(); //Report
            writer.Flush();
            writer.BaseStream.Seek(0,0);
        }
        catch (Exception ex) { reportError(ex); }
        return writer.BaseStream;
    }
    protected void Page_Init(object sender,EventArgs e) {
        // The code below helps to protect against XSRF attacks
        try { 
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value,out requestCookieGuidValue)) {
                // Use the Anti-XSRF token from the cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else {
                // Generate a new Anti-XSRF token and save to the cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey) {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection) {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }
            Page.PreLoad += master_Page_PreLoad;
        }
        catch (Exception ex) { reportError(ex); }
    }
    protected void master_Page_PreLoad(object sender,EventArgs e) {
        try { 
            if (!IsPostBack) {
                // Set Anti-XSRF token
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            }
            else {
                // Validate the Anti-XSRF token
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty)) {
                    throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
                }
            }
        }
        catch (Exception ex) { reportError(ex); }
    }
    protected void Page_Load(object sender,EventArgs e) {
        //Event handler for page load event
        try { 
            if (!Page.IsPostBack) {
                //Initialize control values
            }
        }
        catch (Exception ex) { reportError(ex); }
    }
    protected void OnTreeNodeDataBound(object sender,TreeNodeEventArgs e) {
        //Event handler for treeview node data bounded
        try { 
            string url = e.Node.NavigateUrl;
            if (url.Trim().Length > 0) {
                if (e.Node.Text + " Report" == this.lblReportTitle.Text) {
                    e.Node.Selected = true;
                    e.Node.Parent.Expanded = true;
                }
            }
        }
        catch (Exception ex) { reportError(ex); }
    }
    public void OnButtonCommand(object sender,CommandEventArgs e) {
        //Event handler for export button clicked
        try { 
            switch (e.CommandName) {
                case "Setup":
                    this.btnSetup.BorderStyle = BorderStyle.Inset;
                    this.btnRun.BorderStyle = BorderStyle.Outset;
                    this.btnData.BorderStyle = BorderStyle.Outset;
                    //this.btnExcel.BorderStyle = BorderStyle.Outset;
                    this.mvMain.SetActiveView(this.vwParams);
                    this.rsViewer.Reset();
                    break;
                case "Run":
                    this.btnSetup.BorderStyle = BorderStyle.Outset;
                    this.btnRun.BorderStyle = BorderStyle.Inset;
                    this.btnData.BorderStyle = BorderStyle.Outset;
                    //this.btnExcel.BorderStyle = BorderStyle.Outset;
                    this.mvMain.SetActiveView(this.vwReport);
                    if (this.ButtonCommand != null) this.ButtonCommand(sender,e);
                    break;
                case "Data":
                    this.btnSetup.BorderStyle = BorderStyle.Outset;
                    this.btnRun.BorderStyle = BorderStyle.Outset;
                    this.btnData.BorderStyle = BorderStyle.Inset;
                    //this.btnExcel.BorderStyle = BorderStyle.Outset;
                    this.mvMain.SetActiveView(this.vwReport);
                    if (this.ButtonCommand != null) this.ButtonCommand(sender,e);
                    break;
            }
        }
        catch (Exception ex) { reportError(ex); }
    }
    protected void OnViewerError(object sender,ReportErrorEventArgs e) { reportError(e.Exception); }

    #region Local Services: reportError(), ShowMsgBox()
    private void reportError(Exception ex) {
        //Report an exception to the user
        try {
            string msg = ex.Message;
            if (ex.InnerException != null) msg = ex.Message + "\n\n NOTE: " + ex.InnerException.Message;
            ShowMsgBox(msg);
        }
        catch (Exception) { }
    }
    public void ShowMsgBox(string message) {
        message = message.Replace("'","").Replace("\n"," ").Replace("\r"," ");
        ScriptManager.RegisterStartupScript(this.lblReportTitle,typeof(Label),"Error","alert('" + message + "');",true);
    }
    #endregion
}