using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Net.Configuration;
using System.Configuration;
//using FinanceWeb.Models.Email;

namespace FinanceWeb.Models.Email
{
    internal class ErrorNotify:IDisposable
    {
        private EmailNotification emailObject;
        private Configuration configFile;
        private MailSettingsSectionGroup mailSettings;
        

        internal ErrorNotify()
        {
            configFile = WebConfigurationManager.OpenWebConfiguration(@"~\Web.config");

            if (configFile == null)
                throw new Exception("config file is null");

            mailSettings = configFile.GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup;

            if (mailSettings.Smtp.Network.Host == null)
                throw new Exception("Mail setting host is null");

            emailObject = new EmailNotification(mailSettings.Smtp.Network.Host,
                                                                  mailSettings.Smtp.From,
                                                                  ConfigurationManager.AppSettings["Display"]);
        }

        internal void sendException(Exception exception)
        {

              
                emailObject.setRecipient(ConfigurationManager.AppSettings["Admin"]);
                emailObject.setSubject("Error occured on Finance Integration Portal portal");
                emailObject.setMessage("Exception message:\r\n " + exception.Message + "\r\n"
                                  + "Stack Trace:\r\n" + exception.StackTrace + "\r\n"
                                  + "Source:\r\n" + exception.Source + "\r\n"
                                  + "Additional Data:\r\n" + exception.Data + "\r\n"
                                  + "Time Generated:\r\n" + DateTime.Now + "\r\n");

        }


        
        void  IDisposable.Dispose()
        {
            this.emailObject.sendMail(); //send mail before dispose

            this.configFile = null;
            this.mailSettings = null;
            this.emailObject = null;
        }
    }
    
}