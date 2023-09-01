using System.Linq;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper
{
    public class ProductCategoryToProductCategoryViewModelMapper : MapperBase<ProductCategory, ProductCategoryViewModel>
    {
        public override ProductCategory Map(ProductCategoryViewModel viewModel)
        {
            var mapper = new ProductInCategoryToProductInCategoryViewModelMapper();
            var pc =  new ProductCategory
            {
                Id = viewModel.Id ?? 0,
                Name = viewModel.Name,
                Version = viewModel.Version,
                WebActive = viewModel.WebActive,
                Description = viewModel.Description,
                CategoryRank = viewModel.CategoryRank,
                ParentId = viewModel.ParentId,
                Active = viewModel.Active,
                CompanyId =viewModel.CompanyId
            };

            if (viewModel.ProductInCategories == null) return pc;
            var productInCategoryList = viewModel.ProductInCategories.Select(x => mapper.Map(x)).ToList();
            pc.ProductInCategories = productInCategoryList;
            return pc;
        }

        public override ProductCategoryViewModel Map(ProductCategory entity)
        {
            var mapper = new ProductInCategoryToProductInCategoryViewModelMapper();
            var vm = new ProductCategoryViewModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Version = entity.Version,
                WebActive = entity.WebActive,
                Description = entity.Description,
                CategoryRank = entity.CategoryRank,
                ParentId = entity.ParentId,
                Active = entity.Active,
                CompanyId = entity.CompanyId
            };

            if (entity.ProductInCategories == null) return vm;
            var productInCategoryviewModelList = entity.ProductInCategories.Select(category => mapper.Map(category)).ToList();
            vm.ProductInCategories = productInCategoryviewModelList;
            return vm;
        }
    }
}
