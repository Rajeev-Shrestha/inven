using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper
{
    public class CompanyWebSettingToCompanyWebSettingViewModelMapper : MapperBase<CompanyWebSetting, CompanyWebSettingViewModel>
    {
        public override CompanyWebSetting Map(CompanyWebSettingViewModel viewModel)
        {
            return new CompanyWebSetting
            {
                Id = viewModel.Id ?? 0,
                AllowGuest = viewModel.AllowGuest,
                ShippingCalculationType = viewModel.ShippingCalculationType,
                DiscountCalculationType = viewModel.DiscountCalculationType,
                SalesOrderCode = viewModel.SalesOrderCode,
                PurchaseOrderCode = viewModel.PurchaseOrderCode,
                SalesRepId = viewModel.SalesRepId,
                CustomerSerialNo = viewModel.CustomerSerialNo,
                VendorSerialNo = viewModel.VendorSerialNo,
                CompanyId = viewModel.CompanyId,
                Active = viewModel.Active,
                WebActive = viewModel.WebActive,
                Version = viewModel.Version,
                IsEstoreInitialized = viewModel.IsEstoreInitialized,
                PaymentMethodId = viewModel.PaymentMethodId,
                DeliveryMethodId = viewModel.DeliveryMethodId
            };
        }

        public override CompanyWebSettingViewModel Map(CompanyWebSetting entity)
        {
            return new CompanyWebSettingViewModel
            {
                Id = entity.Id,
                AllowGuest = entity.AllowGuest,
                ShippingCalculationType = entity.ShippingCalculationType,
                DiscountCalculationType = entity.DiscountCalculationType,
                SalesOrderCode = entity.SalesOrderCode,
                PurchaseOrderCode = entity.PurchaseOrderCode,
                SalesRepId = entity.SalesRepId,
                CustomerSerialNo = entity.CustomerSerialNo,
                VendorSerialNo = entity.VendorSerialNo,
                CompanyId = entity.CompanyId,
                Active = entity.Active,
                WebActive = entity.WebActive,
                Version = entity.Version,
                IsEstoreInitialized = entity.IsEstoreInitialized,
                PaymentMethodId = entity.PaymentMethodId,
                DeliveryMethodId = entity.DeliveryMethodId
            };
        }
    }
}
