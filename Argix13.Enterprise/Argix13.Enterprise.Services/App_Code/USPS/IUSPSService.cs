using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Argix.Enterprise {
    //Freight Interfaces
    [ServiceContract(Namespace = "http://Argix.Enterprise")]
    public interface IUSPSService {
        //General Interface        
        [OperationContract]
        [FaultContractAttribute(typeof(EnterpriseFault),Action = "http://Argix.EnterpriseFault")]
        DataSet TrackRequest(string itemNumber);
        [OperationContract]
        [FaultContractAttribute(typeof(EnterpriseFault), Action = "http://Argix.EnterpriseFault")]
        DataSet TrackFieldRequest(string itemNumber);
        [OperationContract]
        [FaultContractAttribute(typeof(EnterpriseFault), Action = "http://Argix.EnterpriseFault")]
        DataSet TrackFieldRequests(string[] itemNumbers);

        [OperationContract]
        [FaultContractAttribute(typeof(EnterpriseFault), Action = "http://Argix.EnterpriseFault")]
        DataSet VerifyAddress(string firmName, string address1, string address2, string city, string state, string zip5, string zip4);
        [OperationContract]
        [FaultContractAttribute(typeof(EnterpriseFault), Action = "http://Argix.EnterpriseFault")]
        DataSet LookupZipCode(string firmName, string address1, string address2, string city, string state);
        [OperationContract]
        [FaultContractAttribute(typeof(EnterpriseFault), Action = "http://Argix.EnterpriseFault")]
        DataSet LookupCityState(string zip5);
    }
}
