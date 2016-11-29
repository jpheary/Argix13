using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Xml.Schema;
using Argix.Enterprise;

public partial class _TsortImages : System.Web.UI.Page {
    //Members

    //Interface
    protected void Page_Load(object sender, EventArgs e) {
        //Page load event handler
        try {
            if(!Page.IsPostBack) {
                ViewState.Add("SortDir","D");
                this.cboDocClass.DataBind();
                OnDocClassChanged(this.cboDocClass,EventArgs.Empty);
                this.txtSearch1.Focus();
            }
        }
        catch(Exception ex) { Master.ReportError(ex, 3); }
    }
    protected void OnDocClassChanged(object sender,EventArgs e) {
        //Event handler for change in selected document class; clear prior search and search results
        try {
            this.txtSearch1.Text = this.txtSearch2.Text = this.txtSearch3.Text = this.txtSearch4.Text = "";
            this.grdImages.DataSource = null;
            this.grdImages.DataBind();
        }
        catch(Exception ex) { Master.ReportError(ex, 3); }
    }
    protected void OnPropertySelectedIndexChanged(object sender,EventArgs e) {
        //Event handler for change in selected metadata property; clear prior search results
        try {
            this.grdImages.DataSource = null;
            this.grdImages.DataBind();
        }
        catch (Exception ex) { Master.ReportError(ex,3); }
    }
    protected void OnSearchTextChanged(object sender,EventArgs e) {
        //Event handler for change in search text; clear prior search results
        try {
            this.grdImages.DataSource = null;
            this.grdImages.DataBind();
        }
        catch (Exception ex) { Master.ReportError(ex,3); }
    }
    protected void OnOperandSelectedIndexChanged(object sender,EventArgs e) {
        //Event handler for change in selected operand; clear prior search results
        try {
            this.grdImages.DataSource = null;
            this.grdImages.DataBind();
        }
        catch (Exception ex) { Master.ReportError(ex,3); }
    }
    protected void OnCommand(object sender,CommandEventArgs e) {
        //Event handler for change in view
        try {
            switch (e.CommandName) {
                case "Search":
                    //Clear any prior results
                    for (int i = this.grdImages.Columns.Count - 1;i >= 7;i--) this.grdImages.Columns.RemoveAt(0);
                    this.grdImages.DataSource = null;
                    this.grdImages.DataBind();

                    //Search for documents
                    DataSet ds = getSearchData();
                    if (ds != null && ds.Tables["ClientResultTable"] != null && ds.Tables["ClientResultTable"].Rows.Count > 0) {
                        //Configure metadata grid columns from resultset
                        DataColumnCollection cols = ds.Tables["ClientResultTable"].Columns;
                        for (int j = cols.Count - 1;j >= 7;j--) {
                            BoundField bf = new BoundField();
                            string colName = cols[j].ColumnName;
                            bf.DataField = colName;
                            bf.HeaderText = XmlConvert.DecodeName(colName.Substring(colName.IndexOf("_") + 1));
                            bf.SortExpression = XmlConvert.EncodeName(colName);
                            bf.ItemStyle.Width = 150;
                            this.grdImages.Columns.Insert(0,bf);
                        }

                        //Display results
                        this.grdImages.DataSource = ds.Tables["ClientResultTable"];
                        this.grdImages.DataBind();
                    }
                    else
                        Master.ShowMessageBox("0 images were found.");
                    break;
            }
        }
        catch (Exception ex) { Master.ReportError(ex,4); }
    }
    protected void OnGridSorting(object sender,GridViewSortEventArgs e) {
        //Event handler for grid sorting event
        try {
            DataSet ds = new DataSet();
            DataSet _ds = getSearchData();
            if (ViewState["SortDir"].ToString() == "A")
                ds.Merge(_ds.Tables[0].Select("",XmlConvert.DecodeName(e.SortExpression) + " DESC"));
            else
                ds.Merge(_ds.Tables[0].Select("",XmlConvert.DecodeName(e.SortExpression) + " ASC"));
            this.grdImages.DataSource = ds;
            this.grdImages.DataBind();
        }
        catch (Exception ex) { Master.ReportError(ex,3); }
    }
    protected void OnGridSorted(object sender,EventArgs e) {
        //Event handler for grid completed sorting event
        try {
            ViewState["SortDir"] = ViewState["SortDir"].ToString() == "D" ? "A" : "D";
        }
        catch (Exception ex) { Master.ReportError(ex,3); }
    }

    protected DataSet getSearchData() {
        //Get image data
        //Form the request
        SearchRequest request = new SearchRequest();
        request.ScopeName = "All Sites";
        request.MaxResults = 250;
        request.DocumentClass = this.cboDocClass.SelectedValue;     //Tsort: TBill, ISA, TDS, BOL 
        request.PropertyName = this.cboProp1.SelectedValue;
        request.PropertyValue = this.txtSearch1.Text.Trim();
        if (this.txtSearch2.Text.Trim().Length > 0) {
            request.Operand1 = this.cboOperand1.SelectedValue;
            request.PropertyName1 = this.cboProp2.SelectedValue;
            request.PropertyValue1 = this.txtSearch2.Text.Trim();
        }
        if (this.txtSearch3.Text.Trim().Length > 0) {
            request.Operand2 = this.cboOperand2.SelectedValue;
            request.PropertyName2 = this.cboProp3.SelectedValue;
            request.PropertyValue2 = this.txtSearch3.Text.Trim();
        }
        if (this.txtSearch4.Text.Trim().Length > 0) {
            request.Operand3 = this.cboOperand3.SelectedValue;
            request.PropertyName3 = this.cboProp4.SelectedValue;
            request.PropertyValue3 = this.txtSearch4.Text.Trim();
        }

        //Search for documents
        return new ImagingGateway().SearchSharePointImageStore(request);
    }
}
