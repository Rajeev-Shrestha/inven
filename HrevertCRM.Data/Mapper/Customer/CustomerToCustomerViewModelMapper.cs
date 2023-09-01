using System.Linq;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper
{
    public class CustomerToCustomerViewModelMapper : MapperBase<Customer, CustomerViewModel>
    {
        public override Customer Map(CustomerViewModel viewModel)
        {
            var salesOrderMapper = new SalesOrderToSalesOrderViewModelMapper();
            var addressMapper = new AddressToAddressViewModelMapper();
            var customerContactgroupMapper = new CustomerInContactGroupToCustomerInContactGroupViewModelMapper();

            var customer = new Customer
            {
                Id = viewModel.Id ?? 0,
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
                customer.BillingAddress = addressMapper.Map(viewModel.BillingAddress);
            }

            if (viewModel.CustomerInContactGroups != null)
            {
                var customerInContactGroups = viewModel.CustomerInContactGroups.Select(x => customerContactgroupMapper.Map(x)).ToList();
                customer.CustomerInContactGroups = customerInContactGroups;
            }

            if (viewModel.SalesOrders == null) return customer;
            var salesOrders = viewModel.SalesOrders.Select(x => salesOrderMapper.Map(x)).ToList();
            customer.SalesOrders = salesOrders;
            return customer;
        }

        public override CustomerViewModel Map(Customer entity)
        {
            var salesOrderMapper = new SalesOrderToSalesOrderViewModelMapper();
            var addressMapper = new AddressToAddressViewModelMapper();
            var customerContactgroupMapper = new CustomerInContactGroupToCustomerInContactGroupViewModelMapper();

            var customerViewModel = new CustomerViewModel
            {
                Id = entity.Id,
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
                Active = entity.Active,
            };
            if (entity.BillingAddress != null)
            {
                customerViewModel.BillingAddress = addressMapper.Map(entity.BillingAddress);
            }

            if (entity.CustomerInContactGroups != null)
            {
               var customerInContactGroups = entity.CustomerInContactGroups.Select(x=>customerContactgroupMapper.Map(x)).ToList();
                customerViewModel.CustomerInContactGroups = customerInContactGroups;
            }
            if (entity.SalesOrders == null) return customerViewModel;
            var salesOrders = entity.SalesOrders.Select(x => salesOrderMapper.Map(x)).ToList();
            customerViewModel.SalesOrders = salesOrders;
            return customerViewModel;
        }
    }
}
