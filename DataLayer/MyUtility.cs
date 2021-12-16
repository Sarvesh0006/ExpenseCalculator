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
    public class MyUtility
    {
        private IHttpContextAccessor _httpContextAccessor;
        public MyUtility(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        //public object SendSMSFromCompany(string Message, string MobileNo)
        //{

        //    dynamic Result = null;
        //    using (IDbConnection db = ORMConnection.GetConnection())
        //    {
        //        try
        //        {
        //            var Identity = (ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity;
        //            string ComapnyId = Identity.FindFirst("CompanyId").Value.ToString();

        //            var parameters = new
        //            {
        //                CompanyId = ComapnyId,
        //                Action = 1
        //            };
        //            db.Open();
        //            Result = db.QueryFirstOrDefault("GetCompanySMSAndEmailSetting", parameters, commandType: CommandType.StoredProcedure);
        //            if (Convert.ToInt32(Result.responseCode) == 200)
        //            {
        //                string AUTH_KEY = Convert.ToString(Result.SMSAuthKey);
        //                string senderId = Convert.ToString(Result.SMSSenderId);
        //                var client = new RestClient("http://msg.icloudsms.com/rest/services/sendSMS/sendGroupSms?AUTH_KEY=" + AUTH_KEY + "&message=" + Message + "&senderId=" + senderId + "&routeId=1&mobileNos=" + MobileNo + "&smsContentType=english");
        //                var request = new RestRequest(Method.GET);
        //                request.AddHeader("Cache-Control", "no-cache");
        //                IRestResponse response = client.Execute(request);
        //                Result = new { statusCode = 200, message = "Successfully sent." };
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Result = ResponseResult.ExceptionResponse("Internal Server Error", ex.Message.ToString());
        //        }
        //        finally
        //        {
        //            db.Close();
        //        }
        //    }
        //    return Result;
        //}

        public object SendEmailFromCompany(string toEmail, string mailSubject, string mailBody)
        {
            dynamic Result = null;
            using (IDbConnection db = ORMConnection.GetConnection())
            {
                try
                {
                    var Identity = (ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity;
                    string ComapnyId = Identity.FindFirst("CompanyId").Value.ToString();

                    var parameters = new
                    {
                        CompanyId = ComapnyId,
                        Action = 2
                    };
                    db.Open();
                    Result = db.QueryFirstOrDefault("GetCompanySMSAndEmailSetting", parameters, commandType: CommandType.StoredProcedure);
                    if (Convert.ToInt32(Result.responseCode) == 200)
                    {
                        string HostServer = Convert.ToString(Result.HostMailServer); 
                        string Email = Convert.ToString(Result.CompanyEmail); 
                        string Password = Convert.ToString(Result.CompanymailPassword);
                        int Port = Convert.ToInt32(Result.MailPort);

                        MailMessage m = new MailMessage();
                        SmtpClient sc = new SmtpClient();
                        m.From = new MailAddress(Email.ToString());
                        m.To.Add(toEmail);
                        m.Subject = mailSubject;
                        m.Body = mailBody;
                        m.IsBodyHtml = true;
                        sc.Host = HostServer.ToString();
                        sc.Port = Port;
                        sc.Credentials = new System.Net.NetworkCredential(Email.ToString(), Password.ToString());
                        sc.EnableSsl = false;
                        sc.Send(m);
                        Result = new { statusCode = 200, message = "Successfully sent." };
                    }
                }
                catch (Exception ex)
                {
                    Result = ResponseResult.ExceptionResponse("Internal Server Error", ex.Message.ToString());
                }
                finally
                {
                    db.Close();
                }
            }
            return Result;
        }

        //public static object SendSms(string Message, string MobileNo)
        //{

        //    try
        //    {
        //        var client = new RestClient("http://msg.icloudsms.com/rest/services/sendSMS/sendGroupSms?AUTH_KEY=99791286d89586f31dd373ee15ae2265&message=" + Message + "&senderId=DEMOOS&routeId=1&mobileNos=" + MobileNo + "&smsContentType=english");
        //        var request = new RestRequest(Method.GET);
        //        request.AddHeader("Cache-Control", "no-cache");
        //        IRestResponse response = client.Execute(request);
        //        return new { statusCode = 200, message = "Successfully sent." };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new { statusCode = 500, message = ex.Message.ToString() };
        //    }
        //}

        //public static object SendEmail(string toEmail, string mailSubject, string mailBody)
        //{
        //    MailMessage m = new MailMessage();
        //    SmtpClient sc = new SmtpClient();
        //    try
        //    {
        //        m.From = new MailAddress(EmailMaster.Email.ToString());
        //        m.To.Add(toEmail);
        //        m.Subject = mailSubject;
        //        m.Body = mailBody;
        //        m.IsBodyHtml = true;
        //        sc.Host = EmailMaster.HostServer.ToString();
        //        sc.Port = EmailMaster.Port; 
        //        sc.Credentials = new System.Net.NetworkCredential(EmailMaster.Email.ToString(), EmailMaster.Password.ToString());
        //        sc.EnableSsl = false;
        //        sc.Send(m);
        //        return new { statusCode = 200, message = "Successfully sent." };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new { statusCode = 500, message = ex.Message.ToString() };
        //    }
        //}
    }
}
