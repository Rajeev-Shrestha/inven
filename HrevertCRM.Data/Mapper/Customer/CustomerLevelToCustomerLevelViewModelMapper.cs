using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper
{
    public class CustomerLevelToCustomerLevelViewModelMapper : MapperBase<CustomerLevel, CustomerLevelViewModel>
    {
        public override CustomerLevel Map(CustomerLevelViewModel viewModel)
        {
            if(viewModel != null)
            return new CustomerLevel
            {
                Id = viewModel.Id ?? 0,
                Name = viewModel.Name,
                CompanyId = viewModel.CompanyId,
                Version = viewModel.Version
            };
            return null;
        }

        public override CustomerLevelViewModel Map(CustomerLevel entity)
        {
            if (entity != null)
                return new CustomerLevelViewModel
            {
                Id = entity.Id,
                Name = entity.Name,
                CompanyId = entity.CompanyId,
                Version = entity.Version
            };
            return null;
        }
    }
}
