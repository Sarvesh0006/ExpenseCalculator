using Dapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ExpenseCalculator.DataLayer
{
    public static class Utility
    {
        public static object SendEmail(string toEmail, string mailSubject, string mailBody)
        {
            MailMessage m = new MailMessage();
            SmtpClient sc = new SmtpClient();
            try
            {
                m.From = new MailAddress(EmailMaster.Email.ToString());
                m.To.Add(toEmail);
                m.Subject = mailSubject;
                m.Body = mailBody;
                m.IsBodyHtml = true;
                sc.Host = EmailMaster.HostServer.ToString();
                sc.Port = EmailMaster.Port;
                sc.Credentials = new System.Net.NetworkCredential(EmailMaster.Email.ToString(), EmailMaster.Password.ToString());
                sc.EnableSsl = false;
                sc.Send(m);
                return new { statusCode = 200, message = "Successfully sent." };
            }
            catch (Exception ex)
            {
                return new { statusCode = 500, message = ex.Message.ToString() };
            }
        }
    }
}
