using HrevertCRM.Data.ViewModels.ThemeSettings;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper.ThemeSettings
{
    public class StoreLocatorToStoreLocatorViewModelMapper : MapperBase<StoreLocator, StoreLocatorViewModel>
    {
        public override StoreLocator Map(StoreLocatorViewModel viewModel)
        {
            return new StoreLocator
            {
                Id = viewModel.Id ?? 0,
                Name = viewModel.Name,
                Phone = viewModel.Phone,
                Fax = viewModel.Fax,
                MobileNumber = viewModel.MobileNumber,
                Email = viewModel.Email,
                Longitude = viewModel.Longitude,
                Latitude = viewModel.Latitude,

                HeaderSettingId = viewModel.HeaderSettingId,

                CompanyId = viewModel.CompanyId,
                Active = viewModel.Active,
                Version = viewModel.Version
            };
        }

        public override StoreLocatorViewModel Map(StoreLocator entity)
        {
            return new StoreLocatorViewModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Phone = entity.Phone,
                Fax = entity.Fax,
                MobileNumber = entity.MobileNumber,
                Email = entity.Email,
                Longitude = entity.Longitude,
                Latitude = entity.Latitude,

                HeaderSettingId = entity.HeaderSettingId,

                CompanyId = entity.CompanyId,
                Active = entity.Active,
                Version = entity.Version
            };
        }
    }
}
