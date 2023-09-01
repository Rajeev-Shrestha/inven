using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface IProductInCategoryQueryProcessor
    {
        int SaveAll(List<ProductInCategory> productCategories);
        int SaveAllProductInCategories(List<ProductInCategory> productCategories);
        bool Delete(int productInCategoryId, int categoryId);
        ProductInCategory GetProductInCategory(int productId, int categoryId);
        List<int> GetExistingCategoriesOfProduct(int productId);
        bool Exists(Expression<Func<ProductInCategory, bool>> @where);
        ProductInCategory Save(ProductInCategory productIncategory);

        //This method is only written for the purpose of reducing the repeating codes while seeding in DbContextExtensions
        ProductInCategory CheckIfProductInCategoryExistsOrSave(ProductInCategory productInCategory);
    }
}
