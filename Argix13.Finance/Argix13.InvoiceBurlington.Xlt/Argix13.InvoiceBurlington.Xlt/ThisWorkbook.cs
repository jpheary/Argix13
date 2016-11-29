using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using Microsoft.Office.Tools.Excel;
using Microsoft.VisualStudio.Tools.Applications.Runtime;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;

namespace Argix.Finance {
    //
    public partial class ThisWorkbook {
        //Members
        private const string USP_DETAIL = "dbo.uspInvBurlingtonInvoiceByReleaseDateGet",TBL_DETAIL = "ClientInvoiceTable";
        private const int ROW0_DETAIL = 12;

        [System.Runtime.InteropServices.DllImport("kernel32.dll",CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern System.IntPtr GetCommandLine();

        //Interface
        private void ThisWorkbook_Startup(object sender,System.EventArgs e) {
            //Event handler for workbook startup event
            try {
                System.IntPtr p = GetCommandLine();
                string cmd = System.Runtime.InteropServices.Marshal.PtrToStringAuto(p);
                string clid = "",invoice = "";
#if DEBUG
                clid = "132"; invoice = "335351900";    //Burlington: 132, 335281100
#endif
                if (cmd != null) {
                    string query = cmd.Substring(cmd.IndexOf('?') + 1);
                    string[] args = query.Split('&');
                    if (args.Length > 0) clid = args[0].Substring(args[0].IndexOf("=") + 1).Trim();
                    if (args.Length > 1) invoice = args[1].Substring(args[1].IndexOf("=") + 1).Trim();
                }
                if (invoice.Length > 0) {
                    //Create detail worksheet
                    try {
                        SqlDataAdapter adapter = new SqlDataAdapter(USP_DETAIL,global::Argix.Finance.Settings.Default.TSortR);
                        adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                        adapter.SelectCommand.Parameters.AddRange(new SqlParameter[] { new SqlParameter("@InvoiceNumber",invoice) });
                        adapter.TableMappings.Add("Table",TBL_DETAIL);
                        InvoiceDataset ds = new InvoiceDataset();
                        adapter.Fill(ds,TBL_DETAIL);
                        if (ds.Tables[TBL_DETAIL] != null && ds.Tables[TBL_DETAIL].Rows.Count > 0) {
                            createSummary(ds.ClientInvoiceTable);
                            createDetailHeader(ds.ClientInvoiceTable[0]);
                            createDetailBody(ds.ClientInvoiceTable);
                            createDetailFooter(ds.ClientInvoiceTable[0]);
                        }
                        else
                            MessageBox.Show("No data found for invoice #" + invoice + ".");
                    }
                    catch (Exception ex) { reportError(ex); }
                }
                else {
                    MessageBox.Show("Invoice unspecified.");
                }
            }
            catch (Exception ex) { reportError(ex); }
        }
        private void ThisWorkbook_BeforeSave(bool SaveAsUI,ref bool Cancel) {
            //Event handler for before save
            try {
                //Remove customization so dll isn't called from a saved file (i.e. only from the template)
                this.RemoveCustomization();
            }
            catch (Exception ex) { reportError(ex); }
        }
        private void ThisWorkbook_Shutdown(object sender,System.EventArgs e) {
            //Event handler for workbook shutdown event
        }

        #region VSTO Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup() {
            this.Startup += new System.EventHandler(ThisWorkbook_Startup);
            this.Shutdown += new System.EventHandler(ThisWorkbook_Shutdown);
            this.BeforeSave += new Microsoft.Office.Interop.Excel.WorkbookEvents_BeforeSaveEventHandler(ThisWorkbook_BeforeSave);
        }

        #endregion
        private void createSummary(InvoiceDataset.ClientInvoiceTableDataTable invoiceTable) {
            //Set named range summary values
            Summary summary = global::Argix.Finance.Globals.Summary;

            //Header
            summary.SumBillToName.Value = invoiceTable[0].BillToName.Trim();
            summary.SumBillToAddress.Value = invoiceTable[0].BillToAddressline1.Trim() + ((!invoiceTable[0].IsBillToAddressline2Null() && invoiceTable[0].BillToAddressline2.Trim().Length > 0) ? invoiceTable[0].BillToAddressline2.Trim() : "");
            summary.SumBillToCityStateZip.Value = invoiceTable[0].BillToCity.Trim() + ", " + invoiceTable[0].BillToState.Trim() + " " + invoiceTable[0].BillToZip.Trim();
            summary.SumInvoice.Value = "Invoice #:     " + invoiceTable[0].InvoiceNumber.Trim();
            summary.SumManifests.Value = "Manifest #:     " + invoiceTable[0].ManifestList.Trim();
            summary.SumInvoiceDate.Value = "Invoice Date:     " + invoiceTable[0].InvoiceDate.ToString("MMMM dd, yyyy");

            //Body
            int cartons = 0,weight = 0;
            decimal charges = 0;
            for (int i = 0;i < invoiceTable.Rows.Count;i++) {
                cartons += invoiceTable[i].CtnQty;
                weight += invoiceTable[i].Weight;
                charges += invoiceTable[i].DeliveryTotal;
            }
            summary.SumReleaseDate.Value = invoiceTable[0].ReleaseDate.ToShortDateString();
            summary.SumCartons.Value = cartons;
            summary.SumWeight.Value = weight;
            summary.SumTotalCharge.Value = charges;

            //Remit To
            summary.SumRemitToName.Value = invoiceTable[0].RemitToName.Trim();
            summary.SumRemitToAddress.Value = invoiceTable[0].RemitToAddressLine1.Trim();
            summary.SumRemitToCityStateZip.Value = invoiceTable[0].RemitToCity.Trim() + ", " + invoiceTable[0].RemitToState.Trim() + " " + invoiceTable[0].RemitToZip.Trim();
        }
        private void createDetailHeader(InvoiceDataset.ClientInvoiceTableRow invoice) {
            //Set named range summary values
            Detail detail = global::Argix.Finance.Globals.Detail;

            //Remit To
            detail.ClientNumberDiv.Value = (invoice.IsClientNumberNull() ? "" : invoice.ClientNumber.Trim()) + ' ' + (invoice.IsClientDivisionNull() ? "" : invoice.ClientDivision.Trim());
            detail.RemitToName.Value = invoice.RemitToName.Trim();
            detail.RemitToAddressLine1.Value = invoice.RemitToAddressLine1.Trim() + " " + invoice.RemitToCity.Trim() + ", " + invoice.RemitToState.Trim() + " " + invoice.RemitToZip.Trim();
            detail.Telephone.Value = (invoice.IsTelephoneNull() ? "" : invoice.Telephone.ToString());

            //Bill To
            detail.BillToName.Value = invoice.BillToName.Trim();
            detail.BillToAddressLine1.Value = invoice.BillToAddressline1.Trim();
            detail.BillToAddressLine2.Value = (!invoice.IsBillToAddressline2Null() && invoice.BillToAddressline2.Trim().Length > 0) ? invoice.BillToAddressline2.Trim() : invoice.BillToCity.Trim() + ", " + invoice.BillToState.Trim() + " " + invoice.BillToZip.Trim();
            detail.BillToCityStateZip.Value = (!invoice.IsBillToAddressline2Null() && invoice.BillToAddressline2.Trim().Length > 0) ? invoice.BillToCity.Trim() + ", " + invoice.BillToState.Trim() + " " + invoice.BillToZip.Trim() : "";

            //Account
            detail.InvoiceNumber.Value = invoice.InvoiceNumber.Trim();
            detail.InvoiceDate.Value = invoice.InvoiceDate;
        }
        private void createDetailBody(InvoiceDataset.ClientInvoiceTableDataTable invoiceTable) {
            //Create body of detail worksheet
            //Excel.Worksheet ws = (Excel.Worksheet)this.Worksheets["Detail"];
            Detail ws = global::Argix.Finance.Globals.Detail;
            Application.ScreenUpdating = false;

            //Insert a row at row0 + 1 (pushes down) for every row of data
            int rowCount = invoiceTable.Rows.Count;
            Excel.Range row0 = ws.Range[ws.Cells[ROW0_DETAIL + 1,1],ws.Cells[ROW0_DETAIL + 1,17]].EntireRow;
            for (int i = 0;i < rowCount - 1;i++)
                row0.Insert(Excel.XlInsertShiftDirection.xlShiftDown,false);

            //Populate entire data table into a range of worksheet cells
            object[,] values = new object[rowCount,17];
            for (int i = 0;i < rowCount;i++) {
                values[i,0] = "'" + invoiceTable[i].InvoiceNumber.Trim();
                values[i,1] = invoiceTable[i].InvoiceDate.ToString("MM/dd/yyyy");
                values[i,2] = invoiceTable[i].ReleaseDate.ToString("MM/dd/yyyy");
                values[i,3] = "'" + (!invoiceTable[i].IsLocationCodeNull() ? invoiceTable[i].LocationCode : "");
                //values[i,4] = "'" + invoiceTable[i].StoreName.Trim();
                values[i,4] = "'" + invoiceTable[i].StoreState.Trim();
                values[i,5] = "'" + invoiceTable[i].StoreZip.Trim();
                values[i,6] = invoiceTable[i].CtnQty;
                //values[i,8] = invoiceTable[i].CartonRate;
                values[i,7] = invoiceTable[i].PltQty;
                values[i,8] = invoiceTable[i].PalletRate;
                values[i,9] = invoiceTable[i].Weight;
                values[i,10] = invoiceTable[i].RatedWeight;
                values[i,11] = invoiceTable[i].WeightRate;
                //values[i,14] = invoiceTable[i].Surcharge;
                //values[i,15] = invoiceTable[i].IsConsolidationChargeNull() ? 0.0m : invoiceTable[i].ConsolidationCharge;
                values[i,12] = invoiceTable[i].FuelRate;
                values[i,13] = invoiceTable[i].FuelSurcharge;
                values[i,14] = invoiceTable[i].DeliveryTotal;
                //values[i,19] = "'" + invoiceTable[i].RateNote;
                values[i,15] = "'" + invoiceTable[i].ManifestList;
                values[i,16] = "'" + invoiceTable[i].TrailerList;
            }
            ws.Range[ws.Cells[ROW0_DETAIL,1],ws.Cells[ROW0_DETAIL + rowCount - 1,17]].Value2 = values;

            Application.ScreenUpdating = true;
        }
        private void createDetailFooter(InvoiceDataset.ClientInvoiceTableRow invoice) {
            //
            Detail detail = global::Argix.Finance.Globals.Detail;
            detail.Reference.Value2 = "PLEASE REFERENCE INVOICE# " + invoice.InvoiceNumber + " WHEN REMITING PAYMENT. I.C.C. REGULATIONS REQUIRE THAT THIS BILL BE PAID WITHIN " + invoice.PaymentDays + " DAYS";
        }
        private void reportError(Exception ex) { MessageBox.Show("UNEXPECTED ERROR: " + ex.Message); }
    }
}
