using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;

namespace HrevertCRM.Data.Mapper.Enumerations
{
    public class CountryToCountryViewModelMapper : MapperBase<Country, CountryViewModel>
    {
        public override CountryViewModel Map(Country entity)
        {
            return new CountryViewModel
            {
                Id = entity.Id,
                Name = entity.Name,
                CompanyId = entity.CompanyId,
                Code = entity.Code,
                Active = entity.Active,
                Version = entity.Version
            };
        }

        public override Country Map(CountryViewModel viewModel)
        {
            return new Country
            {
                Id = viewModel.Id??0,
                Name = viewModel.Name,
                Code = viewModel.Code,
                CompanyId = viewModel.CompanyId,
                Active = viewModel.Active,
                Version = viewModel.Version
            };
        }
    }
}
