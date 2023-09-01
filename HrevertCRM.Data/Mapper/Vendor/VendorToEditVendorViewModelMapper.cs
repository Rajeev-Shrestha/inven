using System.Linq;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper
{
    public class VendorToEditVendorViewModelMapper : MapperBase<Vendor, EditVendorViewModel>
    {
        public override Vendor Map(EditVendorViewModel viewModel)
        {
            var addressMapper = new AddressToAddressViewModelMapper();
            var vendor = new Vendor
            {
                Id = viewModel.Id ?? 0,
                Code = viewModel.Code,
                CreditLimit = viewModel.CreditLimit,
                Debit = viewModel.Debit,
                Credit = viewModel.Credit,
                ContactName = viewModel.ContactName,
                PaymentTermId = viewModel.PaymentTermId,
                PaymentMethodId = viewModel.PaymentMethodId,

                Active = viewModel.Active,
                WebActive = viewModel.WebActive,
                Version = viewModel.Version,
                CompanyId = viewModel.CompanyId,
            };
            if (viewModel.BillingAddress != null)
            {
                vendor.BillingAddress = addressMapper.Map(viewModel.BillingAddress);
            }
            if (viewModel.Addresses == null || viewModel.Addresses.Count < 0) return vendor;
            vendor.Addresses = viewModel.Addresses.Select(x => addressMapper.Map(x)).ToList();
            return vendor;
        }

        public override EditVendorViewModel Map(Vendor entity)
        {
            var addressMapper = new AddressToAddressViewModelMapper();
            var vendorViewModel = new EditVendorViewModel()
            {
                Id = entity.Id,
                Code = entity.Code,
                CreditLimit = entity.CreditLimit,
                Debit = entity.Debit,
                Credit = entity.Credit,
                ContactName = entity.ContactName,
                PaymentTermId = entity.PaymentTermId,
                PaymentMethodId = entity.PaymentMethodId,

                Active = entity.Active,
                WebActive = entity.WebActive,
                Version = entity.Version,
                CompanyId = entity.CompanyId,
            };
            if (entity.BillingAddress != null)
            {
                vendorViewModel.BillingAddress = addressMapper.Map(entity.BillingAddress);
            }
            if (entity.Addresses == null || entity.Addresses.Count < 0) return vendorViewModel;
            vendorViewModel.Addresses = entity.Addresses.Select(x => addressMapper.Map(x)).ToList();
            return vendorViewModel;
        }
    }
}
