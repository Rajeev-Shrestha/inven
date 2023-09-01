using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;

namespace HrevertCRM.Data.Mapper.Enumerations
{
    public class PaymentDiscountTypeToPaymentDiscountTypeViewModelMapper : MapperBase<PaymentDiscountTypes, PaymentDiscountTypeViewModel>
    {
        public override PaymentDiscountTypes Map(PaymentDiscountTypeViewModel viewModel)
        {
            return new PaymentDiscountTypes
            {
                Id = viewModel.Id ?? 0,
                Value = viewModel.Value,
                CompanyId = viewModel.CompanyId,
                Active = viewModel.Active,
                Version = viewModel.Version
            };
        }

        public override PaymentDiscountTypeViewModel Map(PaymentDiscountTypes entity)
        {
            return new PaymentDiscountTypeViewModel
            {
                Id = entity.Id,
                Value = entity.Value,
                CompanyId = entity.CompanyId,
                Active = entity.Active,
                Version = entity.Version
            };
        }
    }
}
