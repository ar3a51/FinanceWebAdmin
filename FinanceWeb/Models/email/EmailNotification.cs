using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.Net.Configuration;
using System.Web.Configuration;
using System.Configuration;



namespace FinanceWeb.Models.Email
{
    internal class EmailNotification : IDisposable
    {
       
        private String recipient;
        private String cc;
        private String message;
        private String subject;
        private SmtpClient mailClient;
        private MailMessage mail;
        private MailAddress addressFrom;
        private MailAddress addressTo;
        private NetworkCredential NetworkCred;


        internal EmailNotification(String smtpHost, String from, String displayName)
        {
            
            this.mailClient = new SmtpClient(smtpHost);
           // NetworkCred = CredentialCache.DefaultNetworkCredentials;
            //this.mailClient.Credentials = NetworkCred;
            

            this.addressFrom = new MailAddress(from,
                                               displayName);
            
        }

        internal void setSubject(String subject)
        {
            this.subject = subject;
        }
        internal void setRecipient(String recipient)
        {
            this.recipient = recipient;
        }

        internal void setCC(String CCrecipient)
        {
            this.cc = CCrecipient;
        }

        internal void setMessage(String message)
        {
            this.message = message;
        }

        internal Boolean sendMail()
        {
            

            
            //this.addressTo = new MailAddress(this.recipient);

            this.mail = new MailMessage();

            //this.mail.Sender = this.addressFrom;
            this.mail.From = this.addressFrom;
            //this.mail = new MailMessage(this.addressFrom,this.addressTo);
            this.mail.To.Add(this.recipient);
            //this.mail.From = new MailAddress("itsupport@charteredaccountants.com.au", "IT Support");
            this.mail.Priority = MailPriority.High;
            this.mail.Subject = this.subject;
            //this.mail.To = this.addressCollection;
            this.mail.Body = this.message;
            
            
            //this.mail = new MailMessage(
            this.mailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            this.mailClient.Send(this.mail);
            return true;

        }

     

        void IDisposable.Dispose()
        {
            this.addressFrom = null;
            this.addressTo = null;
            this.mail = null;
            this.mailClient = null;
        }
    }
}