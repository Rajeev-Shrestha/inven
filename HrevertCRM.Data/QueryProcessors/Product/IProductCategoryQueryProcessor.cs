using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.DTO;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using Microsoft.AspNetCore.Mvc;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface IProductCategoryQueryProcessor
    {
        ProductCategory Update(ProductCategory productCategory);
        ProductCategory GetProductCategory(int productCategoryId);
        int SaveAll(List<ProductCategory> productCategories);
        bool Delete(int productCategoryId);
        bool DeleteOnlyCategory(int productCategoryId);
        bool Exists(Expression<Func<ProductCategory, bool>> @where);
        PagedDataInquiryResponse<ProductCategoryViewModel> GetProductCategories(PagedDataRequest requestInfo,
            Expression<Func<ProductCategory, bool>> @where = null);
        ProductCategory[] GetProductCategories(Expression<Func<ProductCategory, bool>> @where = null);
        IQueryable<ProductCategory> GetAllProductCategories();
        ProductCategory Save(ProductCategory category);
        IEnumerable<CategoryNode> GetProductCategoriesInTree();
        IQueryable<ProductCategory> GetActiveProductCategories();
        IQueryable<ProductCategory> GetDeletedProductCategories();

        //This method is only written for the purpose of reducing the repeating codes while seeding in DbContextExtensions
        ProductCategory CheckIfCategoryExistsOrSave(ProductCategory productCategory);
        List<string> GetCategoryNamesByCategoryId(List<int> categoryIdList);
        int GetCategoryIdByCategoryName(string categoryName);
        string GetCategoryNameByCategoryId(int categoryId);

        string GetCategoryName(List<int> categoryLists);
        List<string> CategoryNamesBySplittingString(string value);
        List<int> GetCategoryIdByCategoryNames(List<string> categoryNames);
        List<ProductCategoryViewModel> GetFullProductCategories();
        ProductCategoryViewModel GetCategoryWithProducts(int productCategoryId);

        ProductCategoryWithChildren GetCategoryWithChildren(int categoryId);

        ProductCategoryWithChildren GetProductByCategoryIdLevelAndNoofProduct(int categoryId, int levelId, int noofProducts);

        // for searching products by its categoryId and text
        List<Product> GetProductsByCategoryIdAndSearchText(int categoryId, string text);

        // for pagination in ProductCategoryWithChildren

        //  PagedDataInquiryResponse<ProductCategoryWithChildren> GetProductByCategoryIdLevelIdNoofProductsWithPagination(PagedDataRequest requestInfo, Expression<Func<ProductCategoryWithChildren, bool>> @where = null);

        // ProductCategoryWithChildren[] GetProductByCategoryIdLevelIdNoofProductsWithPagination(Expression<Func<ProductCategoryWithChildren, bool>> @where = null);

        IQueryable<ProductCategory> GetAllParentProductCategories();

        IEnumerable<CategoryNode> GetAllProductCategoriesInTree();
        // for parent category
        IQueryable<ProductCategory> GetAllActiveProductCategories();

        ProductCategory ActivateProductCategory(int categoryId);
        IQueryable<ProductCategory> GetActiveProductCategories(int distributorId);
        ProductCategory CheckIfDeletedProductCategoryWithSameNameExists(string name);

        short GetLastCategoryRank();
        string SaveCategoryImage(Image categoryImage);
    }
}
