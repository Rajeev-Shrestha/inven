using System.Threading.Tasks;

namespace HrevertCRM.Web.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
