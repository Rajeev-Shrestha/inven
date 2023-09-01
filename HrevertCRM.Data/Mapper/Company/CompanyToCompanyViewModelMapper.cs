using HrevertCRM.Data.ViewModels;

namespace HrevertCRM.Data.Mapper
{
    public class CompanyToCompanyViewModelMapper : MapperBase<Entities.Company, CompanyViewModel>
    {
        public override Entities.Company Map(CompanyViewModel viewModel)
        {
            return new Entities.Company
            {
                Id = viewModel.Id ?? 0,
                MasterId = viewModel.MasterId,
                Name = viewModel.Name,
                GpoBoxNumber = viewModel.GpoBoxNumber,
                Address = viewModel.Address,
                PhoneNumber = viewModel.PhoneNumber,
                FaxNo = viewModel.FaxNo,
                Email = viewModel.Email,
                WebsiteUrl = viewModel.WebsiteUrl,
                VatRegistrationNo = viewModel.VatRegistrationNo,
                PanRegistrationNo = viewModel.PanRegistrationNo,
                FiscalYearFormat = viewModel.FiscalYearFormat,
                IsCompanyInitialized = viewModel.IsCompanyInitialized,

                Active = viewModel.Active,
                Version = viewModel.Version,
                WebActive = viewModel.WebActive
            };
        }

        public override CompanyViewModel Map(Entities.Company entity)
        {
            return new CompanyViewModel
            {
                Id = entity.Id,
                MasterId = entity.MasterId,
                Name = entity.Name,
                GpoBoxNumber = entity.GpoBoxNumber,
                Address = entity.Address,
                PhoneNumber = entity.PhoneNumber,
                FaxNo = entity.FaxNo,
                Email = entity.Email,
                WebsiteUrl = entity.WebsiteUrl,
                VatRegistrationNo = entity.VatRegistrationNo,
                PanRegistrationNo = entity.PanRegistrationNo,
                FiscalYearFormat = entity.FiscalYearFormat,
                IsCompanyInitialized = entity.IsCompanyInitialized,

                Active = entity.Active,
                Version = entity.Version,
                WebActive = entity.WebActive
            };
        }
    }
}
