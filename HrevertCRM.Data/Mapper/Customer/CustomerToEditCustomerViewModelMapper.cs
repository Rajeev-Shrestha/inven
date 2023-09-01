using System.Linq;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper
{
    public class CustomerToEditCustomerViewModelMapper : MapperBase<Customer, EditCustomerViewModel>
    {
        public override Customer Map(EditCustomerViewModel viewModel)
        {
            var addressMapper = new AddressToAddressViewModelMapper();
            var customerEntity = new Customer
            {
                Id = viewModel.Id ?? 0,
                BillingAddressId = viewModel.BillingAddressId,
                CustomerCode = viewModel.CustomerCode,
                Password = viewModel.Password,
                CustomerLevelId = viewModel.CustomerLevelId,
                PaymentTermId = viewModel.PaymentTermId,
                PaymentMethodId = viewModel.PaymentMethodId,
                OpeningBalance = viewModel.OpeningBalance,
                Note = viewModel.Note,
                TaxRegNo = viewModel.TaxRegNo,
                DisplayName = viewModel.DisplayName,
                IsPrepayEnabled = viewModel.IsPrepayEnabled,
                IsCodEnabled = viewModel.IsCodEnabled,
                OnAccountId = viewModel.OnAccountId,

                VatNo = viewModel.VatNo ?? 0,
                PanNo = viewModel.PanNo ?? 0,
                WebActive = viewModel.WebActive,
                Version = viewModel.Version,
                CompanyId = viewModel.CompanyId,
                Active = viewModel.Active,
            };
            if (viewModel.BillingAddress != null)
            {
                customerEntity.BillingAddress = addressMapper.Map(viewModel.BillingAddress);
            }
            if (viewModel.Addresses == null || viewModel.Addresses.Count < 0) return customerEntity;
            customerEntity.Addresses = viewModel.Addresses.Select(s => addressMapper.Map(s)).ToList();

            return customerEntity;
        }

        public override EditCustomerViewModel Map(Customer entity)
        {
            var editCustomerViewModel= new EditCustomerViewModel
            {
                Id = entity.Id,
                BillingAddressId = entity.BillingAddressId,
                CustomerCode = entity.CustomerCode,
                Password = entity.Password,
                CustomerLevelId = entity.CustomerLevelId,
                PaymentTermId = entity.PaymentTermId,
                PaymentMethodId = entity.PaymentMethodId,
                OpeningBalance = entity.OpeningBalance,
                Note = entity.Note,
                TaxRegNo = entity.TaxRegNo,
                DisplayName = entity.DisplayName,
                IsPrepayEnabled = entity.IsPrepayEnabled,
                IsCodEnabled = entity.IsCodEnabled,
                OnAccountId = entity.OnAccountId,

                VatNo = entity.VatNo,
                PanNo = entity.PanNo,
                WebActive = entity.WebActive,
                Version = entity.Version,
                CompanyId = entity.CompanyId,
                Active = entity.Active
            };
            if (entity.BillingAddress != null)
            {
                var addressMapper = new AddressToAddressViewModelMapper();
                editCustomerViewModel.BillingAddress = addressMapper.Map(entity.BillingAddress);
            }
            if (entity.Addresses == null || entity.Addresses.Count < 0) return editCustomerViewModel;
            var mapper = new AddressToAddressViewModelMapper();
            editCustomerViewModel.Addresses = entity.Addresses.Select(s => mapper.Map(s)).ToList();

            return editCustomerViewModel;
        }
    }
}
