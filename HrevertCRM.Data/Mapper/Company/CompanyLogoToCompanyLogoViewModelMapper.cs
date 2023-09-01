using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper
{
    public class CompanyLogoToCompanyLogoViewModelMapper : MapperBase<CompanyLogo, CompanyLogoViewModel>
    {
        public override CompanyLogoViewModel Map(CompanyLogo entity)
        {
            var companyLogoViewModel = new CompanyLogoViewModel
            {
                Id = entity.Id,
                Active = entity.Active,
                WebActive = entity.WebActive,
                Version = entity.Version,
                CompanyId = entity.CompanyId,
                CompanyName = entity.CompanyName,
                MediaUrl = entity.MediaUrl      
            };
            if (entity.Image == null) return companyLogoViewModel;
            companyLogoViewModel.LogoImage = entity.Image;
            return companyLogoViewModel;
        }

        public override CompanyLogo Map(CompanyLogoViewModel viewModel)
        {
            var companyLogo = new CompanyLogo
            {
                Id = viewModel.Id ?? 0,
                CompanyId = viewModel.CompanyId,
                Active = viewModel.Active,
                WebActive = viewModel.WebActive,
                Version = viewModel.Version,
                MediaUrl =viewModel.MediaUrl,
                CompanyName = viewModel.CompanyName
            };

            if (viewModel.LogoImage == null) return companyLogo;
            companyLogo.Image = viewModel.LogoImage;
            return companyLogo;
        }
    }
}
