using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Hrevert.Common.Security;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.QueryProcessors
{
    public class ProductInCategoryQueryProcessor : QueryBase<ProductInCategory>, IProductInCategoryQueryProcessor
    {

        public ProductInCategoryQueryProcessor(IUserSession userSession, IDbContext dbContext) 
            : base(userSession, dbContext)
        {
        }
        public ProductInCategory GetProductInCategory(int productId, int categoryId)
        {
            var productInCategory = _dbContext.Set<ProductInCategory>().FirstOrDefault(d => d.ProductId == productId && d.CategoryId == categoryId && d.CompanyId == LoggedInUser.CompanyId && d.Active);
            return productInCategory;
        }

        public List<int> GetExistingCategoriesOfProduct(int productId)
        {
            var existingCategories = _dbContext.Set<ProductInCategory>().Where(p => p.ProductId == productId && p.CompanyId == LoggedInUser.CompanyId && p.Active).Select(p => p.CategoryId).ToList(); 
            return existingCategories;
        }

        public int SaveAll(List<ProductInCategory> productCategories)
        {
            _dbContext.Set<ProductInCategory>().AddRange(productCategories);
          return  _dbContext.SaveChanges();
        }

        public int SaveAllProductInCategories(List<ProductInCategory> productCategories)
        {
            productCategories.ForEach(x => x.CompanyId = LoggedInUser.CompanyId);
            _dbContext.Set<ProductInCategory>().AddRange(productCategories);
            return _dbContext.SaveChanges();
        }

        public bool Delete(int productId, int categoryId)
        {
            var doc = GetProductInCategory(productId, categoryId);
            var result = 0;
            if (doc == null) return result > 0;
            _dbContext.Set<ProductInCategory>().Remove(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }

        public bool Exists(Expression<Func<ProductInCategory, bool>> @where)
        {
            var check = _dbContext.Set<ProductInCategory>().Any(@where);
            return check;
        }

        public ProductInCategory Save(ProductInCategory productInCategory)
        {
            _dbContext.Set<ProductInCategory>().Add(productInCategory);
            _dbContext.SaveChanges();
            return productInCategory;
        }

        public ProductInCategory CheckIfProductInCategoryExistsOrSave(ProductInCategory productInCategory)
        {
            bool check = _dbContext.Set<ProductInCategory>()
                .Any(x => x.ProductId == productInCategory.ProductId && x.CategoryId == productInCategory.CategoryId);
            if (!check)
            {
                Save(productInCategory);
            }
            return productInCategory;
        }
    }
}