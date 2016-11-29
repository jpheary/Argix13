using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;

//
namespace Argix {
    //
    [ServiceBehavior(InstanceContextMode=InstanceContextMode.PerCall)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.NotAllowed)]
    public class SMTPService:ISMTPService {
        //Members

        //Interface
        public SMTPService() { }
        public void SendMailMessage(string fromEmailAddress,string toEmailAddress,string subject,bool isBodyHtml,string body) {
            //Send a mail message
            try {
                new SMTPGateway().SendMailMessage(fromEmailAddress,toEmailAddress,subject,isBodyHtml,body);
            }
            catch (Exception ex) { throw new FaultException<ConfigurationFault>(new ConfigurationFault(ex.Message),"Service Error"); }
        }
        public void SendMailMessage(string fromEmailAddress,string toEmailAddress,string subject,bool isBodyHtml,string body,string bccEmailAddress) {
            //Send a mail message
            try {
                new SMTPGateway().SendMailMessage(fromEmailAddress,toEmailAddress,subject,isBodyHtml,body,bccEmailAddress);
            }
            catch (Exception ex) { throw new FaultException<ConfigurationFault>(new ConfigurationFault(ex.Message),"Service Error"); }
        }
    }
}
