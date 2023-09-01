using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;

namespace HrevertCRM.Data.Mapper.Enumerations
{
    public class SalesOrderStatusToSalesOrderStatusViewModelMapper : MapperBase<SalesOrdersStatus, SalesOrderStatusViewModel>
    {
        public override SalesOrdersStatus Map(SalesOrderStatusViewModel viewModel)
        {
            return new SalesOrdersStatus
            {
                Id = viewModel.Id ?? 0,
                Value = viewModel.Value,
                CompanyId = viewModel.CompanyId,
                Active = viewModel.Active,
                Version = viewModel.Version
            };
        }

        public override SalesOrderStatusViewModel Map(SalesOrdersStatus entity)
        {
            return new SalesOrderStatusViewModel
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
