//	File:	USPSGateway.cs
//	Author:	J. Heary
//	Date:	11/13/08
//	Desc:	Web service 1.0 proxy for communicating with USPS Track and Confirm services using 
//          HTTP-GET/HTTP-PUT protocol bindings.
//	Rev:	12/01/09 (jph)- added USPSGateway(string,string).
//          10/01/14 (jph)- changed return type from WebResponse to DataSet in TrackRequest() and TrackFieldRequest().
//	----------------------------------------------------------------------------------------------------
using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Services.Protocols;

namespace Argix {
    //
    public class USPSGateway:HttpSimpleClientProtocol {
        //Members
        private string mUserID = "";

        //Interface
        public USPSGateway() {
            //Default constructor
            base.Url = ConfigurationManager.AppSettings["USPS_Url"];
            this.mUserID = ConfigurationManager.AppSettings["USPS_UserID"];
        }
        public USPSGateway(string webSvcUrl,string userID) {
            //Constructor
            base.Url = webSvcUrl;
            this.mUserID = userID;
        }
        public DataSet TrackRequest(string itemNumber) {
            //Track the specified carton and return a simple tracking response
            DataSet ds = new DataSet();
            try {
                //Validate
                if (base.Url.Length == 0) throw new ApplicationException("Must specify a web service URL.");
                if (this.mUserID.Length == 0) throw new ApplicationException("Must specify a valid USPS UserID.");

                //Create URL request per USPS Track and Confirm specifications
                Uri uri = new Uri(base.Url + "?API=TrackV2&XML=<TrackRequest USERID='" + this.mUserID + "'><TrackID ID='" + itemNumber + "'></TrackID></TrackRequest>");
                WebResponse response = base.GetWebResponse(base.GetWebRequest(uri));
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8"));
                ds.ReadXml(reader, XmlReadMode.Auto);
            }
            catch (Exception ex) { throw new ApplicationException(ex.Message,ex); }
            return ds;
        }
        public DataSet TrackFieldRequest(string itemNumber) {
            //Track the specified carton and return a detailed tracking response
            DataSet ds = new DataSet();
            try {
                //Validate
                if (base.Url.Length == 0) throw new ApplicationException("Must specify a web service URL.");
                if (this.mUserID.Length == 0) throw new ApplicationException("Must specify a valid USPS UserID.");

                //Create URL request per USPS Track and Confirm specifications
                Uri uri = new Uri(base.Url + "?API=TrackV2&XML=<TrackFieldRequest USERID='" + this.mUserID + "'><TrackID ID='" + itemNumber + "'></TrackID></TrackFieldRequest>");
                WebResponse response = base.GetWebResponse(base.GetWebRequest(uri));
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8"));
                ds.ReadXml(reader, XmlReadMode.Auto);
            }
            catch(Exception ex) { throw new ApplicationException(ex.Message, ex); }
            return ds;
        }
        public DataSet TrackFieldRequests(string[] itemNumbers) {
            //Track the specified cartons
            DataSet ds = new DataSet();
            try {
                //Validate
                if (base.Url.Length == 0) throw new ApplicationException("Must specify a web service URL.");
                if (this.mUserID.Length == 0) throw new ApplicationException("Must specify a valid USPS UserID.");

                //Create URL request per USPS Track and Confirm specifications
                Uri uri = null;
                string trackIDs = "";
                for (int i = 0;i < itemNumbers.Length;i++) {
                    trackIDs += "<TrackID ID='" + itemNumbers[i] + "'></TrackID>";
                }
                uri = new Uri(base.Url + "?API=TrackV2&XML=<TrackFieldRequest USERID='" + this.mUserID + "'>" + trackIDs + "</TrackFieldRequest>");
                WebResponse response = base.GetWebResponse(base.GetWebRequest(uri));
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8"));
                ds.ReadXml(reader, XmlReadMode.Auto);
            }
            catch(Exception ex) { throw new ApplicationException(ex.Message, ex); }
            return ds;
        }

        public DataSet VerifyAddress(string firmName, string address1, string address2, string city, string state, string zip5, string zip4) {
            //
            DataSet ds = new DataSet();
            try {
                //Validate
                if(base.Url.Length == 0) throw new ApplicationException("Must specify a web service URL.");
                if(this.mUserID.Length == 0) throw new ApplicationException("Must specify a valid USPS UserID.");

                Uri uri = new Uri(base.Url + "?API=Verify&XML=<AddressValidateRequest USERID='" + this.mUserID + "'><Address ID='0'><FirmName>" + firmName.Trim() + "</FirmName><Address1>" + address1.Trim() + "</Address1><Address2>" + address2.Trim() + "</Address2><City>" + city.Trim() + "</City><State>" + state.Trim() + "</State><Zip5>" + zip5.Trim() + "</Zip5><Zip4>" + zip4.Trim() + "</Zip4></Address></AddressValidateRequest>");
                WebResponse response = base.GetWebResponse(base.GetWebRequest(uri));
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8"));
                ds.ReadXml(reader, XmlReadMode.Auto);
            }
            catch(Exception ex) { throw new ApplicationException(ex.Message, ex); }
            return ds;
        }
        public DataSet LookupZipCode(string firmName, string address1, string address2, string city, string state) {
            //
            DataSet ds = new DataSet();
            try {
                //Validate
                if(base.Url.Length == 0) throw new ApplicationException("Must specify a web service URL.");
                if(this.mUserID.Length == 0) throw new ApplicationException("Must specify a valid USPS UserID.");

                Uri uri = new Uri(base.Url + "?API=ZipCodeLookup&XML=<AddressValidateRequest USERID='" + this.mUserID + "'><Address ID='0'><FirmName>" + firmName.Trim() + "</FirmName><Address1>" + address1.Trim() + "</Address1><Address2>" + address2.Trim() + "</Address2><City>" + city.Trim() + "</City><State>" + state.Trim() + "</State></Address></AddressValidateRequest>");
                WebResponse response = base.GetWebResponse(base.GetWebRequest(uri));
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8"));
                ds.ReadXml(reader, XmlReadMode.Auto);
            }
            catch(Exception ex) { throw new ApplicationException(ex.Message, ex); }
            return ds;
        }
        public DataSet LookupCityState(string zip5) {
            //
            DataSet ds = new DataSet();
            try {
                //Validate
                if(base.Url.Length == 0) throw new ApplicationException("Must specify a web service URL.");
                if(this.mUserID.Length == 0) throw new ApplicationException("Must specify a valid USPS UserID.");

                Uri uri = new Uri(base.Url + "?API=CityStateLookup&XML=<CityStateLookupRequest USERID='" + this.mUserID + "'><ZipCode ID='0'><Zip5>" + zip5.Trim() + "</Zip5></ZipCode></CityStateLookupRequest>");
                WebResponse response = base.GetWebResponse(base.GetWebRequest(uri));
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8"));
                ds.ReadXml(reader, XmlReadMode.Auto);
            }
            catch(Exception ex) { throw new ApplicationException(ex.Message, ex); }
            return ds;
        }
    }
}

