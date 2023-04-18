using Vekalat.Application.Common.InfraServices;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Vekalat.InfraStructure.Data.Repositories.InfraRepository
{
    public class SenderService : ISenderService
    {
        public async Task SendEmail(string to, string subject, string body)
        {
          
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("mail.isbn.ir");
            mail.From = new MailAddress("isbn@isbn.ir", "سایت شابک");
            mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            //System.Net.Mail.Attachment attachment;
            // attachment = new System.Net.Mail.Attachment("c:/textfile.txt");
            // mail.Attachments.Add(attachment);

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("isbn@isbn.ir", "I@i123456");
            SmtpServer.EnableSsl = false;

            await SmtpServer.SendMailAsync(mail);
        }
        //public async Task SendEmail(string to, string subject, string body)
















        //public async Task<string> SendSms(string phoneNumber, string message)
        //{
        //    try
        //    {
        //        string username = "khaneketab/magfa";
        //        string password = "@4BzcRGtuWKs1E-!";
        //        string domain = "magfa";

        //        // Client
        //        var client = new RestClient("https://sms.magfa.com/api/http/sms/v2/send");

        //        // Auth
        //        client.Authenticator = new HttpBasicAuthenticator(username + "/" + domain, password);

        //        // Request
        //        var request = new RestRequest(Method.POST);
        //        request.AddHeader("cache-control", "no-cache");
        //        request.AddHeader("accept", "application/json");
        //        request.RequestFormat = DataFormat.Json;
        //        request.AddParameter("senders", "30004141");
        //        request.AddParameter("messages", message);
        //        request.AddParameter("recipients", phoneNumber);

        //        // Call
        //        IRestResponse response = await client.ExecuteAsync(request);
        //        if (response.StatusCode == HttpStatusCode.OK)
        //            return "Sent";
        //        else
        //            return "Failed";
        //    }
        //    catch
        //    {
        //        return "Error";
        //    }
        //}
    }
}