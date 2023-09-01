using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper
{
    public class PaymentMethodToPaymentMethodViewModelMapper : MapperBase<PaymentMethod, PaymentMethodViewModel>
    {
        public override PaymentMethod Map(PaymentMethodViewModel viewModel)
        {
            return new PaymentMethod
            {
                Id = viewModel.Id ?? 0,
                MethodCode = viewModel.MethodCode,
                MethodName = viewModel.MethodName,
                AccountId = viewModel.AccountId ?? 0,
                Version = viewModel.Version,
                WebActive = viewModel.WebActive,
                CompanyId = viewModel.CompanyId,
                ReceipentMemo = viewModel.ReceipentMemo,
                Active = viewModel.Active
            };
        }

        public override PaymentMethodViewModel Map(PaymentMethod entity)
        {
            return new PaymentMethodViewModel
            {
                Id = entity.Id,
                MethodCode = entity.MethodCode,
                MethodName = entity.MethodName,
                AccountId = entity.AccountId,
                Version = entity.Version,
                WebActive = entity.WebActive,
                CompanyId = entity.CompanyId,
                ReceipentMemo = entity.ReceipentMemo,
                Active = entity.Active
            };
        }
    }
}
