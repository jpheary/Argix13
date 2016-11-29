using System;
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
using System.Xml;

namespace Argix.Enterprise {
    //
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.NotAllowed)]
    public class USPSService : IUSPSService {
        //Members

        //Interface
        public USPSService() { }
        public DataSet TrackRequest(string itemNumber) {
            //
            DataSet response = null;
            try {
                response = new USPSGateway().TrackRequest(itemNumber);
            }
            catch (Exception ex) { throw new FaultException<EnterpriseFault>(new EnterpriseFault(ex.Message),"Service Error"); }
            return response;
        }
        public DataSet TrackFieldRequest(string itemNumber) {
            //
            DataSet response = null;
            try {
                response = new USPSGateway().TrackFieldRequest(itemNumber);
            }
            catch(Exception ex) { throw new FaultException<EnterpriseFault>(new EnterpriseFault(ex.Message), "Service Error"); }
            return response;
        }
        public DataSet TrackFieldRequests(string[] itemNumbers) {
            //
            DataSet response = null;
            try {
                response = new USPSGateway().TrackFieldRequests(itemNumbers);
            }
            catch(Exception ex) { throw new FaultException<EnterpriseFault>(new EnterpriseFault(ex.Message), "Service Error"); }
            return response;
        }

        public DataSet VerifyAddress(string firmName, string address1, string address2, string city, string state, string zip5, string zip4) {
            //
            DataSet response = null;
            try {
                response = new USPSGateway().VerifyAddress(firmName, address1, address2, city, state, zip5, zip4);
            }
            catch(Exception ex) { throw new FaultException<EnterpriseFault>(new EnterpriseFault(ex.Message), "Service Error"); }
            return response;
        }
        public DataSet LookupZipCode(string firmName, string address1, string address2, string city, string state) {
            //
            DataSet response = null;
            try {
                response = new USPSGateway().LookupZipCode(firmName, address1, address2, city, state);
            }
            catch(Exception ex) { throw new FaultException<EnterpriseFault>(new EnterpriseFault(ex.Message), "Service Error"); }
            return response;
        }
        public DataSet LookupCityState(string zip5) {
            //
            DataSet response = null;
            try {
                response = new USPSGateway().TrackRequest(zip5);
            }
            catch(Exception ex) { throw new FaultException<EnterpriseFault>(new EnterpriseFault(ex.Message), "Service Error"); }
            return response;
        }

    }
}
