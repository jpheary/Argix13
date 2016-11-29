using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Argix {
    //
    [ServiceContract(Namespace="http://Argix")]
    public interface ISMTPService {
        //

        [OperationContract]
        [FaultContractAttribute(typeof(ConfigurationFault),Action = "http://Argix.ConfigurationFault")]
        void SendMailMessage(string fromEmailAddress,string toEmailAddress,string subject,bool isBodyHtml,string body);

        [OperationContract(Name="SendMailMessageWithBlindCopy")]
        [FaultContractAttribute(typeof(ConfigurationFault),Action = "http://Argix.ConfigurationFault")]
        void SendMailMessage(string fromEmailAddress,string toEmailAddress,string subject,bool isBodyHtml,string body,string bccEmailAddress);
    }

    [DataContract]
    public class SMTPFault {
        private string mMessage="";
        public SMTPFault(string message) { this.mMessage = message; }
        [DataMember]
        public string Message { get { return this.mMessage; } set { this.mMessage = value; } }
    }
}
