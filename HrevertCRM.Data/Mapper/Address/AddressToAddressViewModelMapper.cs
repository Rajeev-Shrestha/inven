using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper
{
    public class AddressToAddressViewModelMapper : MapperBase<Address, AddressViewModel>
    {
        public override Address Map(AddressViewModel viewModel)
        {
            return new Address
            {
                Id = viewModel.Id ?? 0,
                AddressType = viewModel.AddressType,
                UserId = viewModel.UserId,
                Fax = viewModel.Fax,
                Title = viewModel.Title,
                Suffix = viewModel.Suffix,
                FirstName = viewModel.FirstName,
                MiddleName = viewModel.MiddleName,
                LastName = viewModel.LastName,
                AddressLine1 = viewModel.AddressLine1,
                AddressLine2 = viewModel.AddressLine2,
                City = viewModel.City,
                State = viewModel.State,
                CountryId = viewModel.CountryId,
                Telephone = viewModel.Telephone,
                MobilePhone = viewModel.MobilePhone,
                Email = viewModel.Email,
                Website = viewModel.Website,
                Version = viewModel.Version,
                Active = viewModel.Active,
                WebActive = viewModel.WebActive,
                CompanyId = viewModel.CompanyId,
                IsDefault = viewModel.IsDefault,
                CustomerId = viewModel.CustomerId,
                VendorId = viewModel.VendorId,
                ZipCode = viewModel.ZipCode,
                DeliveryZoneId = viewModel.DeliveryZoneId
            };
        }

        public override AddressViewModel Map(Address entity)
        {
            return new AddressViewModel
            {
                Id = entity.Id,
                AddressType = entity.AddressType,
                UserId = entity.UserId,
                Fax = entity.Fax,
                Title = entity.Title,
                Suffix = entity.Suffix,
                FirstName = entity.FirstName,
                MiddleName = entity.MiddleName,
                LastName = entity.LastName,
                AddressLine1 = entity.AddressLine1,
                AddressLine2 = entity.AddressLine2,
                City = entity.City,
                State = entity.State,
                CountryId = entity.CountryId,
                Telephone = entity.Telephone,
                MobilePhone = entity.MobilePhone,
                Email = entity.Email,
                Website = entity.Website,
                Version = entity.Version,
                Active = entity.Active,
                WebActive = entity.WebActive,
                CompanyId = entity.CompanyId,
                IsDefault = entity.IsDefault,
                CustomerId = entity.CustomerId,
                VendorId = entity.VendorId,
                ZipCode = entity.ZipCode,
                DeliveryZoneId = entity.DeliveryZoneId
            };
        }
    }
}
