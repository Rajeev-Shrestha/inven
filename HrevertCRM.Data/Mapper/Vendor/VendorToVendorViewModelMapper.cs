using System.Collections.Generic;
using System.Linq;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper
{
    public class VendorToVendorViewModelMapper : MapperBase<Vendor, VendorViewModel>
    {
        public override Vendor Map(VendorViewModel viewModel)
        {
            var purchaseOrderMapper = new PurchaseOrderToPurchaseOrderViewModelMapper();
            var addressMapper = new AddressToAddressViewModelMapper();
            var vendor = new Vendor
            {
                Id = viewModel.Id ?? 0,
                Code = viewModel.Code,
                ContactName = viewModel.ContactName,
                CreditLimit = viewModel.CreditLimit,
                Debit = viewModel.Debit,
                Credit = viewModel.Credit,
                PaymentTermId = viewModel.PaymentTermId,
                PaymentMethodId = viewModel.PaymentMethodId,
                Active = viewModel.Active,
                WebActive = viewModel.WebActive,
                Version = viewModel.Version,
                CompanyId = viewModel.CompanyId
            };

            if (viewModel.BillingAddress != null)
            {
                vendor.BillingAddress = addressMapper.Map(viewModel.BillingAddress);
            }
            if (viewModel.Addresses == null || viewModel.Addresses.Count < 0) return vendor;
            vendor.Addresses = viewModel.Addresses.Select(s => addressMapper.Map(s)).ToList();

            if (viewModel.PurchaseOrders == null) return vendor;
            var purchaseOrders = viewModel.PurchaseOrders.ToList().Select(x => purchaseOrderMapper.Map(x)).ToList();
            vendor.PurchaseOrders = purchaseOrders;
            return vendor;
        }

        public override VendorViewModel Map(Vendor entity)
        {
            var purchaseOrderMapper = new PurchaseOrderToPurchaseOrderViewModelMapper();
            var addressMapper = new AddressToAddressViewModelMapper();

            var vendorViewModel =  new VendorViewModel
            {
                Id = entity.Id,
                Code = entity.Code,
                ContactName = entity.ContactName,
                CreditLimit = entity.CreditLimit,
                Debit = entity.Debit,
                Credit = entity.Credit,
                PaymentTermId = entity.PaymentTermId,
                PaymentMethodId = entity.PaymentMethodId,

                Active = entity.Active,
                WebActive = entity.WebActive,
                Version = entity.Version,
                CompanyId = entity.CompanyId
            };

            if (entity.BillingAddress != null)
            {
                vendorViewModel.BillingAddress = addressMapper.Map(entity.BillingAddress);
            }

            if (vendorViewModel.Addresses == null || vendorViewModel.Addresses.Count < 0) return vendorViewModel;
            vendorViewModel.Addresses = entity.Addresses.Select(s => addressMapper.Map(s)).ToList();

            if (entity.PurchaseOrders == null) return vendorViewModel;
            var purchaseOrders = entity.PurchaseOrders.Select(x => purchaseOrderMapper.Map(x));
            entity.PurchaseOrders = purchaseOrders as ICollection<PurchaseOrder>;
            return vendorViewModel;
        }
    }
}
