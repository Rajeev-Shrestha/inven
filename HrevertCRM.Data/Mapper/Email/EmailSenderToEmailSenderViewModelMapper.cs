using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper
{
    public class EmailSenderToEmailSenderViewModelMapper : MapperBase<EmailSender, EmailSenderViewModel>
    {
        public override EmailSender Map(EmailSenderViewModel viewModel)
        {
            return new EmailSender
            {
                Id = viewModel.Id ?? 0,
                MailFrom = viewModel.MailFrom,
                MailTo = viewModel.MailTo,
                Cc = viewModel.Cc,
                Subject = viewModel.Subject,
                Message = viewModel.Message,
                IsEmailSent = viewModel.IsEmailSent,
                CompanyId = viewModel.CompanyId,
                Active = viewModel.Active,
                EmailNotSentCause = viewModel.EmailNotSentCause,
            };
        }

        public override EmailSenderViewModel Map(EmailSender entity)
        {
            return new EmailSenderViewModel
            {
                Id = entity.Id,
                MailFrom = entity.MailFrom,
                MailTo = entity.MailTo,
                Cc = entity.Cc,
                Subject = entity.Subject,
                Message = entity.Message,
                IsEmailSent = entity.IsEmailSent,
                CompanyId = entity.CompanyId,
                Active = entity.Active,
                EmailNotSentCause = entity.EmailNotSentCause,
            };
        }
    }
}
