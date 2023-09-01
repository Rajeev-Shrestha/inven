using System.Collections.Generic;
using System.Linq;
using Hrevert.Common.Enums;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper
{
    public class PurchaseOrderToPurchaseOrderViewModelMapper : MapperBase<PurchaseOrder, PurchaseOrderViewModel>
    {
        public override PurchaseOrder Map(PurchaseOrderViewModel viewModel)
        {
            var mapper = new PurchaseOrderLineToPurchaseOrderLineViewModelMapper();
            var purchaseOrder= new PurchaseOrder
            {
                Id = viewModel.Id ?? 0,
                SalesOrderNumber = viewModel.SalesOrderNumber,
                OrderDate = viewModel.OrderDate,
                DueDate = viewModel.DueDate,
                Status = viewModel.Status,
                OrderType = viewModel.OrderType,
                FiscalPeriodId = viewModel.FiscalPeriodId,
                PaymentTermId = viewModel.PaymentTermId,
                DeliveryMethodId = viewModel.DeliveryMethodId,
                BillingAddressId = viewModel.BillingAddressId,
                ShippingAddressId = viewModel.ShippingAddressId ?? 0,
                PurchaseRepId = viewModel.PurchaseRepId,
                VendorId = viewModel.VendorId,
                InvoicedDate = viewModel.InvoicedDate,
                PaymentDueOn = viewModel.PaymentDueOn,
                Active = viewModel.Active,
                WebActive = viewModel.WebActive,
                CompanyId = viewModel.CompanyId,
                PurchaseOrderCode =viewModel.PurchaseOrderCode,
                Version = viewModel.Version,
                FullyPaid = viewModel.FullyPaid
            };

            if (viewModel.PurchaseOrderLines == null || viewModel.PurchaseOrderLines.Count < 0) return purchaseOrder;
            var lines = viewModel.PurchaseOrderLines.Select(s => mapper.Map(s)).ToList();
            purchaseOrder.PurchaseOrderLines= new List<PurchaseOrderLine>();
            foreach (var purchaseOrderLineViewModel in lines)
            {
                purchaseOrder.PurchaseOrderLines.Add(purchaseOrderLineViewModel);
            }
            return purchaseOrder;
        }

        public override PurchaseOrderViewModel Map(PurchaseOrder entity)
        {
            var vm = new PurchaseOrderViewModel
            {
                Id = entity.Id,
                SalesOrderNumber = entity.SalesOrderNumber,
                OrderDate = entity.OrderDate,
                DueDate = entity.DueDate,
                Status = entity.Status,
                OrderType = entity.OrderType,
                FiscalPeriodId = entity.FiscalPeriodId,
                PaymentTermId = entity.PaymentTermId,
                DeliveryMethodId = entity.DeliveryMethodId,
                BillingAddressId = entity.BillingAddressId,
                ShippingAddressId = entity.ShippingAddressId,
                PurchaseRepId = entity.PurchaseRepId,
                VendorId = entity.VendorId,
                InvoicedDate = entity.InvoicedDate,
                PaymentDueOn = entity.PaymentDueOn,
                Active = entity.Active,
                WebActive = entity.WebActive,
                CompanyId = entity.CompanyId,
                Version = entity.Version,
                PurchaseOrderCode = entity.PurchaseOrderCode,
                FullyPaid = entity.FullyPaid
            };
            if (entity.Vendor != null)
            {
                var vendorAddress = entity.Vendor.Addresses.FirstOrDefault(x => x.AddressType == AddressType.Billing);
                vm.VendorName = vendorAddress.MiddleName == null
                    ? vendorAddress.FirstName + " " + vendorAddress.LastName
                    : vendorAddress.FirstName + " " + vendorAddress.MiddleName + " " + vendorAddress.LastName;
            }
            if (entity.PurchaseOrderLines == null || entity.PurchaseOrderLines.Count <= 0) return vm;
            var mapper = new PurchaseOrderLineToPurchaseOrderLineViewModelMapper();
            vm.PurchaseOrderLines = new List<PurchaseOrderLineViewModel>();
            var lines = entity.PurchaseOrderLines.Select(s => mapper.Map(s)).ToList();
            foreach (var purchaseOrderLineViewModel in lines)
            {
                vm.PurchaseOrderLines.Add(purchaseOrderLineViewModel);
            }
            return vm;
        }
    }
}
