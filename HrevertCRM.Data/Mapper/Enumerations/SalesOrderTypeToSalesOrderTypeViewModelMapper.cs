using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;

namespace HrevertCRM.Data.Mapper.Enumerations
{
    public class SalesOrderTypeToSalesOrderTypeViewModelMapper : MapperBase<SalesOrderTypes, SalesOrderTypeViewModel>
    {
        public override SalesOrderTypes Map(SalesOrderTypeViewModel viewModel)
        {
            return new SalesOrderTypes
            {
                Id = viewModel.Id ?? 0,
                Value = viewModel.Value,
                CompanyId = viewModel.CompanyId,
                Active = viewModel.Active,
                Version = viewModel.Version
            };
        }

        public override SalesOrderTypeViewModel Map(SalesOrderTypes entity)
        {
            return new SalesOrderTypeViewModel
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
