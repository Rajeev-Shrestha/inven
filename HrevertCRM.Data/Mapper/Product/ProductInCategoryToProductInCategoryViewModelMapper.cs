using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper
{
    public class ProductInCategoryToProductInCategoryViewModelMapper : MapperBase<ProductInCategory, ProductInCategoryViewModel>
    {
        public override ProductInCategory Map(ProductInCategoryViewModel viewModel)
        {
            return new ProductInCategory
            {
                ProductId = viewModel.ProductId,
                CategoryId = viewModel.CategoryId
            };
        }

        public override ProductInCategoryViewModel Map(ProductInCategory entity)
        {
            return new ProductInCategoryViewModel
            {
                ProductId = entity.ProductId,
                CategoryId = entity.CategoryId
            };
        }
    }
}
