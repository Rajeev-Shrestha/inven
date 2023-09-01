using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper
{
    public class ProductRefByKitAndAssembledTypeMapperToGetNewEntity : MapperBase<ProductsRefByKitAndAssembledType, ProductsRefByKitAndAssembledTypeViewModel>
    {
        public override ProductsRefByKitAndAssembledType Map(ProductsRefByKitAndAssembledTypeViewModel viewModel)
        {
            return new ProductsRefByKitAndAssembledType
            {
                Id = viewModel.Id ?? 0,
                ProductId = viewModel.ProductId,
                ProductRefId = viewModel.ProductRefId,

                CompanyId = viewModel.CompanyId,
                Active = viewModel.Active,
                Version = viewModel.Version
            };
        }

        public override ProductsRefByKitAndAssembledTypeViewModel Map(ProductsRefByKitAndAssembledType entity)
        {
            return new ProductsRefByKitAndAssembledTypeViewModel
            {
                //Id = entity.Id,
                ProductId = entity.ProductId,
                ProductRefId = entity.ProductRefId,

                CompanyId = entity.CompanyId,
                Active = entity.Active,
                Version = entity.Version
            };
        }
    }
}
