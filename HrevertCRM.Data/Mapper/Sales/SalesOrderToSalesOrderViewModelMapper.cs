using System.Collections.Generic;
using System.Linq;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper
{
    public class SalesOrderToSalesOrderViewModelMapper : MapperBase<SalesOrder, SalesOrderViewModel>
    {
        public override SalesOrder Map(SalesOrderViewModel viewModel)
        {
            var mapper = new SalesOrderLineToSalesOrderLineViewModelMapper();
            var order =  new SalesOrder
            {
                Id = viewModel.Id ?? 0,
                PurchaseOrderNumber = viewModel.PurchaseOrderNumber,
                DueDate = viewModel.DueDate,
                Status = viewModel.Status,
                SalesPolicy = viewModel.SalesPolicy,
                IsWebOrder = viewModel.IsWebOrder,
                FullyPaid = viewModel.FullyPaid,
                TotalAmount = viewModel.TotalAmount,
                PaidAmount = viewModel.PaidAmount,
                BillingAddressId = viewModel.BillingAddressId,
                ShippingAddressId = viewModel.ShippingAddressId ?? 0,
                PaymentTermId = viewModel.PaymentTermId,
                PaymentMethodId = viewModel.PaymentMethodId,
                FiscalPeriodId = viewModel.FiscalPeriodId,
                OrderType = viewModel.OrderType,
                DeliveryMethodId = viewModel.DeliveryMethodId,
                SalesRepId = viewModel.SalesRepId,
                CustomerId = viewModel.CustomerId,
                Version = viewModel.Version,
                CompanyId = viewModel.CompanyId,
                Active = viewModel.Active,
                WebActive = viewModel.WebActive,
                InvoicedDate = viewModel.InvoicedDate,
                PaymentDueOn = viewModel.PaymentDueOn,
                SalesOrderCode = viewModel.SalesOrderCode,
                ShippingCost = viewModel.ShippingCost
            };

            if (viewModel.SalesOrderLines == null) return order;
            var orderlineList = viewModel.SalesOrderLines.Select(x => mapper.Map(x));
            order.SalesOrderLines = orderlineList.ToList();
            return order;
        }

        public override SalesOrderViewModel Map(SalesOrder entity)
        {
            var vm = new SalesOrderViewModel
            {
                Id = entity.Id,
                PurchaseOrderNumber = entity.PurchaseOrderNumber,
                DueDate = entity.DueDate,
                Status = entity.Status,
                SalesPolicy = entity.SalesPolicy,
                IsWebOrder = entity.IsWebOrder,
                FullyPaid = entity.FullyPaid,
                TotalAmount = entity.TotalAmount,
                PaidAmount = entity.PaidAmount,
                BillingAddressId = entity.BillingAddressId,
                ShippingAddressId = entity.ShippingAddressId,
                PaymentTermId = entity.PaymentTermId,
                PaymentMethodId = entity.PaymentMethodId,
                FiscalPeriodId = entity.FiscalPeriodId,
                OrderType = entity.OrderType,
                DeliveryMethodId = entity.DeliveryMethodId,
                SalesRepId = entity.SalesRepId,
                CustomerId = entity.CustomerId,
                Version = entity.Version,
                CompanyId = entity.CompanyId,
                Active = entity.Active,
                WebActive = entity.WebActive,
                InvoicedDate = entity.InvoicedDate,
                PaymentDueOn = entity.PaymentDueOn,
                SalesOrderCode = entity.SalesOrderCode,
                ShippingCost = entity.ShippingCost,
                CustomerName = entity.Customer == null ? "" : entity.Customer.DisplayName
            };

            if (entity.SalesOrderLines== null || entity.SalesOrderLines.Count <= 0) return vm;
            var mapper = new SalesOrderLineToSalesOrderLineViewModelMapper();
            vm.SalesOrderLines = new List<SalesOrderLineViewModel>();
            var lines = entity.SalesOrderLines.Select(s => mapper.Map(s));
            foreach (var salesOrderLineViewModel in lines)
            {
                vm.SalesOrderLines.Add(salesOrderLineViewModel);
            }
            return vm;
        }
    }
}
