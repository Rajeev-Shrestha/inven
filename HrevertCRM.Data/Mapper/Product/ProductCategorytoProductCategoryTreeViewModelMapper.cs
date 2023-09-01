using System.Linq;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper
{
    public class ProductCategoryAndProductCategoryTreeViewModelMapper : MapperBase<ProductCategory, ProductCategoryTreeViewModel>
    {
        public override ProductCategory Map(ProductCategoryTreeViewModel viewModel)
        {
            return new ProductCategory
            {
                Id = viewModel.Id ?? 0,
                Name = viewModel.Name,
                CategoryRank = viewModel.CategoryRank,
                ParentId = viewModel.ParentId,
                Description = viewModel.Description,
                WebActive = viewModel.WebActive,
                Active = viewModel.Active,
                Version = viewModel.Version
            };
        }

        public override ProductCategoryTreeViewModel Map(ProductCategory categoryEntity)
        {
            var productMapper = new ProductToProductViewModelMapper();
            var productCat = new ProductCategoryTreeViewModel
            {
                Id = categoryEntity.Id,
                Name = categoryEntity.Name,
                CategoryRank = categoryEntity.CategoryRank,
                ParentId = categoryEntity.ParentId,
                Active = categoryEntity.Active,
                Description = categoryEntity.Description,
                WebActive = categoryEntity.WebActive,
                Version = categoryEntity.Version,
            };

            if (categoryEntity.ProductInCategories == null || categoryEntity.ProductInCategories.Count < 0) return productCat;
            productCat.ProductViewModels = categoryEntity.ProductInCategories.Select(pic => productMapper.Map(pic.Product)).ToList();
            return productCat;
            
        }
    }
}
