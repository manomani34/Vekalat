using System.Threading.Tasks;

namespace Vekalat.Application.Common.InfraServices
{
    public interface ISenderService
    {
        Task SendEmail(string to, string subject, string body);
        //Task<string> SendSms(string phoneNumber, string message);
    }
}



