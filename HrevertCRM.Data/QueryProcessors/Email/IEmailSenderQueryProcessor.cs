using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using Microsoft.AspNetCore.Http;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface IEmailSenderQueryProcessor
    {
        EmailSender Save(EmailSenderViewModel emailViewModel);
        void SendEmail(EmailSettingViewModel emailSetting, EmailSender email, IFormFileCollection files);
        void EmailCartDetail(EmailSettingViewModel emailSetting, EmailSenderViewModel emailSenderViewModel, string fileBase64);
        void ReportBug(EmailSenderViewModel emailSenderViewModel);
        void SendUsernamePassToCustomer(string email, string userName, string password);
    }
}
