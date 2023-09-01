using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper
{
    public class EmailSettingToEmailSettingViewModelMapper : MapperBase<EmailSetting, EmailSettingViewModel>
    {
        public override EmailSetting Map(EmailSettingViewModel viewModel)
        {
            return new EmailSetting
            {
                Id = viewModel.Id ?? 0,
                Host = viewModel.Host,
                Port = viewModel.Port,
                Sender = viewModel.Sender,
                UserName = viewModel.UserName,
                Password = viewModel.Password,
                Name = viewModel.Name,
                EncryptionType = viewModel.EncryptionType,
                RequireAuthentication = viewModel.RequireAuthentication,
                WebActive = viewModel.WebActive,
                Active = viewModel.Active,
                Version = viewModel.Version,
                CompanyId = viewModel.CompanyId
            };
        }

        public override EmailSettingViewModel Map(EmailSetting entity)
        {
            return new EmailSettingViewModel
            {
                Id = entity.Id,
                Host = entity.Host,
                Port = entity.Port,
                Sender = entity.Sender,
                UserName = entity.UserName,
                Password = entity.Password,
                Name = entity.Name,
                EncryptionType = entity.EncryptionType,
                RequireAuthentication = entity.RequireAuthentication,
                WebActive = entity.WebActive,
                Active = entity.Active,
                CompanyId = entity.CompanyId,
                Version = entity.Version
            };
        }
    }
}
