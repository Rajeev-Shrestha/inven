using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper
{
    public class PaymentTermToPaymentTermViewModelMapper : MapperBase<PaymentTerm, PaymentTermViewModel>
    {
        public override PaymentTerm Map(PaymentTermViewModel viewModel)
        {
            return new PaymentTerm
            {
                Id = viewModel.Id ?? 0,
                TermName = viewModel.TermName,
                TermCode = viewModel.TermCode,
                Description = viewModel.Description,
                TermType = viewModel.TermType,
                DueDateType = viewModel.DueDateType,
                DueType = viewModel.DueType,
                DueDateValue = viewModel.DueDateValue ?? 0,
                DiscountType = viewModel.DiscountType,
                DiscountValue = viewModel.DiscountValue ?? 0,
                DiscountDays = viewModel.DiscountDays ?? 0,
                Version = viewModel.Version,
                CompanyId = viewModel.CompanyId,
                Active = viewModel.Active,
                WebActive = viewModel.WebActive
            };
        }

        public override PaymentTermViewModel Map(PaymentTerm entity)
        {
            return new PaymentTermViewModel
            {
                Id = entity.Id,
                TermCode = entity.TermCode,
                TermName = entity.TermName,
                Description = entity.Description,
                TermType = entity.TermType,
                DueDateType = entity.DueDateType,
                DueType = entity.DueType,
                DueDateValue = entity.DueDateValue,
                DiscountType = entity.DiscountType,
                DiscountValue = entity.DiscountValue,
                DiscountDays = entity.DiscountDays,
                Version = entity.Version,
                CompanyId = entity.CompanyId,
                Active = entity.Active,
                WebActive = entity.WebActive
            };
        }
    }
}

