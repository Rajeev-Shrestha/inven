using System;
using System.Linq;
using HrevertCRM.Data.DTO;
using HrevertCRM.Data.ViewModels;

namespace HrevertCRM.Data.Mapper
{
    public class CategoryNodeAndProductCategoryTreeViewModelMapper :MapperBase<CategoryNode, ProductCategoryTreeViewModel>
    {
        public override CategoryNode Map(ProductCategoryTreeViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        public override ProductCategoryTreeViewModel Map(CategoryNode categoryNode)
        {
            var productMapper = new ProductToProductViewModelMapper();
            var productCatVm = new ProductCategoryTreeViewModel
            {
                Name = categoryNode.Source.Name,
                Id = categoryNode.Source.Id,
                CategoryRank = categoryNode.Source.CategoryRank,
                ParentId = categoryNode.Source.ParentId,
                Description = categoryNode.Source.Description,
                WebActive = categoryNode.Source.WebActive,
                Active = categoryNode.Source.Active,
                Version = categoryNode.Source.Version
            };
           if(categoryNode.Source.ProductInCategories == null || categoryNode.Source.ProductInCategories.Count < 0) return productCatVm;
            productCatVm.ProductViewModels = categoryNode.Source.ProductInCategories.Select(pic => productMapper.Map(pic.Product)).ToList();
            return productCatVm;
        }
    }
}
