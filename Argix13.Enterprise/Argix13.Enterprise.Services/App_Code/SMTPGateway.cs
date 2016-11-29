using System;
using System.Net.Mail;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.Security;
using System.IO;

public class SMTPGateway {
    //Members
    private string mSMTPServer = "";
    
    //Interface
    public SMTPGateway() {
        //Constructor
        try {
            //Read configuration values for this service
            this.mSMTPServer = WebConfigurationManager.AppSettings["SMTPServer"].ToString();
        }
        catch (Exception ex) { throw new ApplicationException(ex.Message,ex); }
    }
    #region Accessors: SMTPServer, FromEmailAddress, AdminEmailAddress, PODReqEmailAddress
    public string SMTPServer { get { return this.mSMTPServer; } }
    #endregion
    public void SendMailMessage(string fromEmailAddress,string toEmailAddress,string subject,bool isBodyHtml, string body) {
        //
        try {
            MailMessage email = new MailMessage(fromEmailAddress,toEmailAddress);
            email.Subject = subject;
            email.BodyEncoding = System.Text.Encoding.UTF8;
            email.IsBodyHtml = isBodyHtml;
            email.Body = body;

            SmtpClient smtpClient = new SmtpClient(this.mSMTPServer);
            smtpClient.Send(email);
        }
        catch (Exception ex) { throw new ApplicationException(ex.Message,ex); }
    }
    public void SendMailMessage(string fromEmailAddress,string toEmailAddress,string subject,bool isBodyHtml,string body,string bccEmailAddress) {
        //
        try {
            MailMessage email = new MailMessage(fromEmailAddress,toEmailAddress);
            email.Subject = subject;
            email.BodyEncoding = System.Text.Encoding.UTF8;
            email.IsBodyHtml = isBodyHtml;
            email.Body = body;

            MailAddress copy = new MailAddress(bccEmailAddress);
            email.Bcc.Add(copy);

            SmtpClient smtpClient = new SmtpClient(this.mSMTPServer);
            smtpClient.Send(email);
        }
        catch (Exception ex) { throw new ApplicationException(ex.Message,ex); }
    }


}
