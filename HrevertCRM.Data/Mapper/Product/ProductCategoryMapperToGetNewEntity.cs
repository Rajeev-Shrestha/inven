using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper
{
    public class ProductCategoryMapperToGetNewEntity : MapperBase<ProductCategory, ProductCategoryViewModel>
    {
        public override ProductCategory Map(ProductCategoryViewModel viewModel)
        {
            return new ProductCategory
            {
                Id = viewModel.Id ?? 0,
                Name = viewModel.Name,
                CategoryImageUrl = viewModel.CategoryImageUrl,
                Version = viewModel.Version,
                WebActive = viewModel.WebActive,
                Description = viewModel.Description,
                CategoryRank = viewModel.CategoryRank,
                ParentId = viewModel.ParentId,
                Active = viewModel.Active,
                CompanyId = viewModel.CompanyId
            };
        }

        public override ProductCategoryViewModel Map(ProductCategory entity)
        {
            return new ProductCategoryViewModel
            {
                //Id = entity.Id,
                Name = entity.Name,
                Version = entity.Version,
                CategoryImageUrl = entity.CategoryImageUrl,
                WebActive = entity.WebActive,
                Description = entity.Description,
                CategoryRank = entity.CategoryRank,
                ParentId = entity.ParentId,
                Active = entity.Active,
                CompanyId = entity.CompanyId
            };
        }
    }
}
