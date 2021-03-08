using System.Net.Mail;
using System.Net;
using NotesMarketplace.Models.MailModels;
using NotesMarketplace.Data.SystemConfigDB;
using System;
using System.Diagnostics;

namespace NotesMarketplace.Web.Mailer
{
    public class SendMail
    {
        public static bool SendEmail(EmailModel email)
        {
            try
            {
                MailMessage mm = new MailMessage();
                mm.From = new MailAddress(SystemConfigData.GetSystemConfigData("MailAddress").DataValue, "Notes Marketplace");
                foreach(string address in email.EmailTo)
                    mm.To.Add(new MailAddress(address));
                mm.Subject = email.EmailSubject;
                mm.Body = email.EmailBody;
                mm.IsBodyHtml = true;
                using (var smtp = new SmtpClient()
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(mm.From.Address, SystemConfigData.GetSystemConfigData("MailPassword").DataValue)
                }
                )
                {
                    smtp.Send(mm);
                    return true;
                }
            }catch(Exception e)
            {
                Debug.WriteLine(e);
                return false;
            }
        }
    }
}