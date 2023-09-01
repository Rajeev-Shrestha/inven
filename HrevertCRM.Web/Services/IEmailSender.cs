using System.Threading.Tasks;

namespace HrevertCRM.Web.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
