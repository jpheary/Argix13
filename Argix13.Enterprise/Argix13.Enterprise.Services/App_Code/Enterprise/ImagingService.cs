using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Search;
using Microsoft.SharePoint.Client.Search.Query;
using Microsoft.SharePoint.Utilities;

namespace Argix.Enterprise {
    //
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.NotAllowed)]
    public class ImagingService:IImagingService {
        //Members

        //Interface
        public ImagingService() { }
        public DocumentClasses GetDocumentClasses() {
            //Retrieve document classes
            DocumentClasses docs = null;
            try {
                docs = new DocumentClasses();
                System.Xml.XmlDataDocument xmlDoc = new System.Xml.XmlDataDocument();
                xmlDoc.DataSet.ReadXml(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "\\App_Data\\documentclass.xml");
                for (int i = 0;i < xmlDoc.DataSet.Tables["DocumentClassTable"].Rows.Count;i++) {
                    DocumentClass dc = new DocumentClass(xmlDoc.DataSet.Tables["DocumentClassTable"].Rows[i]["Department"].ToString(),xmlDoc.DataSet.Tables["DocumentClassTable"].Rows[i]["ClassName"].ToString());
                    docs.Add(dc);
                }
            }
            catch (Exception ex) { throw new FaultException<EnterpriseFault>(new EnterpriseFault(ex.Message),"Service Error"); }
            return docs;
        }
        public DocumentClasses GetDocumentClasses(string department) {
            //Retrieve document classes
            DocumentClasses docs = null;
            try {
                docs = new DocumentClasses();
                DocumentClasses _docs = GetDocumentClasses();
                foreach (DocumentClass dc in _docs) {
                    if (dc.Department == department) docs.Add(dc);
                }
            }
            catch (Exception ex) { throw new FaultException<EnterpriseFault>(new EnterpriseFault(ex.Message),"Service Error"); }
            return docs;
        }
        public MetaDatas GetMetaData() {
            //Retrieve document class metadata
            MetaDatas metas = null;
            try {
                metas = new MetaDatas();
                System.Xml.XmlDataDocument xmlMeta = new System.Xml.XmlDataDocument();
                xmlMeta.DataSet.ReadXml(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "\\App_Data\\metadata.xml");
                for (int i = 0;i < xmlMeta.DataSet.Tables["MetaDataTable"].Rows.Count;i++) {
                    MetaData md = new MetaData(xmlMeta.DataSet.Tables["MetaDataTable"].Rows[i]["ClassName"].ToString(),xmlMeta.DataSet.Tables["MetaDataTable"].Rows[i]["Property"].ToString(),xmlMeta.DataSet.Tables["MetaDataTable"].Rows[i]["Value"].ToString());
                    metas.Add(md);
                }
            }
            catch (Exception ex) { throw new FaultException<EnterpriseFault>(new EnterpriseFault(ex.Message),"Service Error"); }
            return metas;
        }
        public MetaDatas GetMetaData(string className) {
            //Retrieve document class metadata for the specified className
            MetaDatas metas = null;
            try {
                metas = new MetaDatas();
                MetaDatas _metas = GetMetaData();
                foreach (MetaData md in _metas) {
                    if (md.ClassName == className) metas.Add(md);
                }
            }
            catch (Exception ex) { throw new FaultException<EnterpriseFault>(new EnterpriseFault(ex.Message),"Service Error"); }
            return metas;
        }
        public DataSet SearchSharePointImageStore(SearchRequest request) {
            //Find all documents that match the request
            DataSet response = new DataSet();
            try {
                using (ClientContext context = new ClientContext(WebConfigurationManager.AppSettings["SP_Url"])) {
                    //Set credentials
                    context.Credentials = getCredentials();

                    //Create the query
                    KeywordQuery query = new KeywordQuery(context);
                    query.StartRow = 0;
                    query.RowLimit = request.MaxResults;
                    query.EnableStemming = true;
                    query.TrimDuplicates = false;
                    query.QueryText = "scope:\"" + request.ScopeName + "\" AND contentclass:\"STS_ListItem_DocumentLibrary\" AND IsDocument:\"True\" AND ";
                    if (request.PropertyName != null && request.PropertyName.Trim().Length > 0) query.QueryText += request.PropertyName + ":" + request.PropertyValue;
                    if (request.PropertyName1 != null && request.PropertyName1.Trim().Length > 0) query.QueryText += " " + request.Operand1 + " " + request.PropertyName1 + ":" + request.PropertyValue1;
                    if (request.PropertyName2 != null && request.PropertyName2.Trim().Length > 0) query.QueryText += " " + request.Operand2 + " " + request.PropertyName2 + ":" + request.PropertyValue2;
                    if (request.PropertyName3 != null && request.PropertyName3.Trim().Length > 0) query.QueryText += " " + request.Operand3 + " " + request.PropertyName3 + ":" + request.PropertyValue3;
                    query.SelectProperties.Add("scope");
                    query.SelectProperties.Add("contentclass");
                    query.SelectProperties.Add("IsDocument");
                    query.SelectProperties.Add("Title");
                    query.SelectProperties.Add("Path");
                    query.SelectProperties.Add("Description");
                    query.SelectProperties.Add("Size");
                    MetaDatas metas = GetMetaData(request.DocumentClass);
                    foreach (MetaData md in metas) { query.SelectProperties.Add(md.Value); }

                    //Create  the dataset schema
                    response.Tables.Add("ClientResultTable");
                    response.Tables["ClientResultTable"].Columns.Add("scope");
                    response.Tables["ClientResultTable"].Columns.Add("contentclass");
                    response.Tables["ClientResultTable"].Columns.Add("IsDocument");
                    response.Tables["ClientResultTable"].Columns.Add("Title");
                    response.Tables["ClientResultTable"].Columns.Add("Path");
                    response.Tables["ClientResultTable"].Columns.Add("Description");
                    response.Tables["ClientResultTable"].Columns.Add("Size");

                    //Create a field wth the metadata property (friendly) field
                    foreach (MetaData md in metas) { response.Tables["ClientResultTable"].Columns.Add(md.Property); }
                    response.AcceptChanges();

                    ClientResult<ResultTableCollection> results = new SearchExecutor(context).ExecuteQuery(query);
                    context.ExecuteQuery();
                    foreach (IDictionary result in results.Value[0].ResultRows) {
                        //Populate the dataset with a row for each result
                        DataRow row = response.Tables["ClientResultTable"].NewRow();
                        row["scope"] = result["scope"];
                        row["contentclass"] = result["contentclass"];
                        row["IsDocument"] = result["IsDocument"];
                        row["Title"] = result["Title"];

                        string path = result["Path"].ToString();
                        row["Path"] = path.Substring(path.Length - 1,1) == "/" ? path.Substring(0,path.Length - 1) : path;
                        row["Description"] = result["Description"];
                        row["Size"] = result["Size"];

                        foreach (MetaData md in metas) { row[md.Property] = result[md.Value]; }
                        response.Tables["ClientResultTable"].Rows.Add(row);
                    }
                }
            }
            catch (Exception ex) { throw new FaultException<EnterpriseFault>(new EnterpriseFault(ex.Message),"Service Error"); }
            return response;
        }
        public byte[] GetSharePointImageStream(SearchRequest request) {
            //Return a document from SharePoint by a specific request
            byte[] bytes=null;
            try {
                //Search images: return the first one
                DataSet ds = SearchSharePointImageStore(request);
                if (ds.Tables[0].Rows.Count > 0) {
                    //Get the url of the first document
                    string url = ds.Tables[0].Rows[0]["href"].ToString();

                    //Pull the image from a remote client and load into an image object
                    WebClient wc = new WebClient();
                    wc.Credentials = getCredentials();
                    bytes = wc.DownloadData(url);
                }
            }
            catch (Exception ex) { throw new FaultException<EnterpriseFault>(new EnterpriseFault(ex.Message),"Service Error"); }
            return bytes;
        }
        public byte[] GetSharePointImageStream(string uri) {
            //Return a document from SharePoint by Uri
            byte[] bytes = null;
            try {
                //Pull the image from a remote client and load into an image object
                WebClient wc = new WebClient();
                wc.Credentials = getCredentials();
                bytes = wc.DownloadData(uri);
            }
            catch (Exception ex) { throw new FaultException<EnterpriseFault>(new EnterpriseFault(ex.Message),"Service Error"); }
            return bytes;
        }

        private ICredentials getCredentials() {
            //Determine credentials for web service access
            ICredentials credentials = System.Net.CredentialCache.DefaultCredentials;
            if (WebConfigurationManager.AppSettings["SP_UseSpecificCredentials"] == "1") {
                string username = WebConfigurationManager.AppSettings["SP_UserID"];
                string password = WebConfigurationManager.AppSettings["SP_Password"];
                string domain = WebConfigurationManager.AppSettings["SP_Domain"];
                credentials = new NetworkCredential(username,password,domain);
            }
            return credentials;
        }
        private string getDateClause(string propertyName,string propertyValue) {
            //Build a WHERE clause for a datetime field
            string from = SPUtility.CreateISO8601DateTimeFromSystemDateTime(Convert.ToDateTime(propertyValue));
            string to = SPUtility.CreateISO8601DateTimeFromSystemDateTime(Convert.ToDateTime(propertyValue).AddDays(1).AddSeconds(-1));
            return " ((" + propertyName + " >= '" + from + "') AND (" + propertyName + " <= '" + to + "'))";
        }
    }
}
