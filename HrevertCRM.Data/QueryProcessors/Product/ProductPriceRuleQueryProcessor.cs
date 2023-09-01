using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Hrevert.Common.Constants.Product;
using Hrevert.Common.Security;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.Mapper;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore;
using PagedTaskDataInquiryResponse = HrevertCRM.Data.Common.PagedDataInquiryResponse<HrevertCRM.Data.ViewModels.ProductPriceRuleViewModel>;

namespace HrevertCRM.Data.QueryProcessors
{
    public class ProductPriceRuleQueryProcessor : QueryBase<ProductPriceRule>, IProductPriceRuleQueryProcessor
    {

        public ProductPriceRuleQueryProcessor(IUserSession userSession, IDbContext dbContext)
            : base(userSession, dbContext)
        {
        }
        public ProductPriceRule Update(ProductPriceRule productPriceRule)
        {
            var original = GetValidProductPriceRule(productPriceRule.Id);
            ValidateAuthorization(original);
            CheckVersionMismatch(productPriceRule, original);

            //pass value to original
            original.CustomerId = productPriceRule.CustomerId;
            original.ProductId = productPriceRule.ProductId;
            original.CategoryId = productPriceRule.CategoryId;
            original.Quantity = productPriceRule.Quantity;
            original.FreeQuantity = productPriceRule.FreeQuantity;
            original.FixedPrice = productPriceRule.FixedPrice;
            original.DiscountPercent = productPriceRule.DiscountPercent;
            original.StartDate = productPriceRule.StartDate;
            original.EndDate = productPriceRule.EndDate;

            original.Active = productPriceRule.Active;
            original.CompanyId = LoggedInUser.CompanyId;
            original.WebActive = productPriceRule.WebActive;

            _dbContext.Set<ProductPriceRule>().Update(original);
            _dbContext.SaveChanges();
            return productPriceRule;
        }
        public virtual ProductPriceRule GetValidProductPriceRule(int productPriceRuleId)
        {
            var productPriceRule = _dbContext.Set<ProductPriceRule>().FirstOrDefault(sc => sc.Id == productPriceRuleId);
            if (productPriceRule == null)
            {
                throw new RootObjectNotFoundException(ProductConstants.ProductPriceRuleQueryProcessorConstants.ProductPriceRuleNotFound);
            }
            return productPriceRule;
        }

        public ProductPriceRule GetProductPriceRule(int productPriceRuleId)
        {
            var productPriceRule = _dbContext.Set<ProductPriceRule>().FirstOrDefault(d => d.Id == productPriceRuleId);
            return productPriceRule;
        }

        public ProductPriceRuleViewModel GetProductPriceRuleViewModel(int productPriceRuleId)
        {
            var mapper = new ProductPriceRuleToProductPriceRuleViewModelMapper();
            var productPriceRule = _dbContext.Set<ProductPriceRule>().FirstOrDefault(d => d.Id == productPriceRuleId);

            return mapper.Map(productPriceRule);
        }

        public void SaveAllProductPriceRule(List<ProductPriceRule> productPriceRules)
        {
            //ProductPriceRules.ForEach(ProductPriceRule => ProductPriceRule.CompanyId = LoggedInUser.CompanyId);
            _dbContext.Set<ProductPriceRule>().AddRange(productPriceRules);
            _dbContext.SaveChanges();
        }

        public int GetCompanyIdByProductPriceRuleId(int productPriceRuleId)
        {
            return _dbContext.Set<ProductPriceRule>().Where(p => p.Id == productPriceRuleId).Select(p => p.CompanyId).Single();

        }

        public ProductPriceRule Save(ProductPriceRule productPriceRule)
        {
            productPriceRule.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<ProductPriceRule>().Add(productPriceRule);
            _dbContext.SaveChanges();
            return productPriceRule;
        }

        public IQueryable<ProductPriceRule> GetAllActiveProductPriceRules()
        {
            return _dbContext.Set<ProductPriceRule>().Where(p => p.Active && p.CompanyId == LoggedInUser.CompanyId);
        }

        public bool Delete(int productPriceRuleId)
        {
            var doc = GetProductPriceRule(productPriceRuleId);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<ProductPriceRule>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }

        public bool Exists(Expression<Func<ProductPriceRule, bool>> where)
        {
            if (_dbContext.Set<ProductPriceRule>().Any(where))
            {

            }
            return _dbContext.Set<ProductPriceRule>().Any(where);
        }

        public PagedTaskDataInquiryResponse GetProductPriceRules(PagedDataRequest requestInfo, Expression<Func<ProductPriceRule, bool>> where = null)
        {
            var query = _dbContext.Set<ProductPriceRule>().Where(FilterByActiveTrueAndCompany);
            query = where == null ? query : query.Where(where);
            //var enumerable = query as ProductPriceRule[] ?? query.ToArray();

            var totalItemCount = query.Count();
            var startIndex = ResultsPagingUtility.CalculateStartIndex(requestInfo.PageNumber, requestInfo.PageSize);
            var mapper = new ProductPriceRuleToProductPriceRuleViewModelMapper();
            var docs = query.OrderBy(x => x.DateCreated).Skip(startIndex).Take(requestInfo.PageSize).Select(s => mapper.Map(s)).ToList();
            var queryResult = new QueryResult<ProductPriceRuleViewModel>(docs, totalItemCount, requestInfo.PageSize);
            var inquiryResponse = new PagedTaskDataInquiryResponse
            {
                Items = docs,
                TotalRecords = totalItemCount,
                PageCount = queryResult.TotalPageCount,
                PageNumber = requestInfo.PageNumber,
                PageSize = requestInfo.PageSize
            };
            return inquiryResponse;
        }

        public ProductPriceRule[] GetProductPriceRules(Expression<Func<ProductPriceRule, bool>> where = null)
        {
            var query = _dbContext.Set<ProductPriceRule>().Where(FilterByActiveTrueAndCompany);

            query = where == null ? query : query.Where(where);
            var enumerable = query.ToArray();
            return enumerable;
        }

        //public IQueryable<ProductPriceRule> GetActiveProductPriceRules()
        //{
        //    var ProductPriceRules = _dbContext.Set<ProductPriceRule>().Include(a => a.ProductPriceRuleInCategories).ThenInclude(t => t.ProductPriceRuleCategory).Where(p => p.Active && p.CompanyId == LoggedInUser.CompanyId);
        //    return ProductPriceRules;
        //}
        //public IQueryable<ProductPriceRule> GetDeletedProductPriceRules()
        //{
        //    return _dbContext.Set<ProductPriceRule>().Include(p=> p.ProductPriceRuleInCategories).Where(p => p.Active == false && p.CompanyId == LoggedInUser.CompanyId);
        //}
        public PagedTaskDataInquiryResponse GetActiveProductPriceRules(PagedDataRequest requestInfo, Expression<Func<ProductPriceRule, bool>> where = null)
        {

            var query = _dbContext.Set<ProductPriceRule>().Where(FilterByActiveTrueAndCompany);
            query = where == null ? query : query.Where(where);
            return FormatResultForPaging(requestInfo, query);
        }


        public PagedTaskDataInquiryResponse GetDeletedProductPriceRules(PagedDataRequest requestInfo, Expression<Func<ProductPriceRule, bool>> where = null)
        {

            var query = _dbContext.Set<ProductPriceRule>().Where(FilterByActiveFalseAndCompany);
            query = where == null ? query : query.Where(where);
            return FormatResultForPaging(requestInfo, query);
        }

        public PagedTaskDataInquiryResponse GetAllProductPriceRules(PagedDataRequest requestInfo, Expression<Func<ProductPriceRule, bool>> where = null)
        {
            var query = _dbContext.Set<ProductPriceRule>().Where(c => c.CompanyId == LoggedInUser.CompanyId);
            return FormatResultForPaging(requestInfo, query);
        }

        public PagedTaskDataInquiryResponse SearchActive(PagedDataRequest requestInfo, Expression<Func<ProductPriceRule, bool>> where = null)
        {
            var filteredProductPriceRule = _dbContext.Set<ProductPriceRule>().Include(x => x.Product).Include(x => x.Category).Where(FilterByActiveTrueAndCompany);
            var query = string.IsNullOrEmpty(requestInfo.SearchText) ? filteredProductPriceRule : filteredProductPriceRule.Where(s
                                                                  => s.Product.Name.ToUpper().Contains(requestInfo.SearchText.ToUpper())
                                                                  || s.Product.Code.ToUpper().Contains(requestInfo.SearchText.ToUpper())
                                                                  || s.Category.Name.ToUpper().Contains(requestInfo.SearchText.ToUpper())
                                                                  );
            //TODO: Make this LINQ query precompiled, using the method 

            return FormatResultForPaging(requestInfo, query);
        }

        public PagedTaskDataInquiryResponse SearchAll(PagedDataRequest requestInfo, Expression<Func<ProductPriceRule, bool>> @where = null)
        {
            var filteredProductPriceRule = _dbContext.Set<ProductPriceRule>().Include(x => x.Product).Include(x => x.Category).Where(x => x.CompanyId == LoggedInUser.CompanyId);
            var query = string.IsNullOrEmpty(requestInfo.SearchText) ? filteredProductPriceRule : filteredProductPriceRule.Where(s
                                                                  => s.Product.Name.ToUpper().Contains(requestInfo.SearchText.ToUpper())
                                                                  || s.Product.Code.ToUpper().Contains(requestInfo.SearchText.ToUpper())
                                                                  || s.Category.Name.ToUpper().Contains(requestInfo.SearchText.ToUpper())
                                                                  );
            //TODO: Make this LINQ query precompiled, using the method 

            return FormatResultForPaging(requestInfo, query);
        }
        private static PagedTaskDataInquiryResponse FormatResultForPaging(PagedDataRequest requestInfo, IQueryable<ProductPriceRule> query)
        {
            var totalItemCount = query.Count();

            var startIndex = ResultsPagingUtility.CalculateStartIndex(requestInfo.PageNumber, requestInfo.PageSize);
            var mapper = new ProductPriceRuleToProductPriceRuleViewModelMapper();
            var docs = query.OrderBy(x => x.DateCreated).Skip(startIndex).Take(requestInfo.PageSize).Select(
                s => mapper.Map(s)).ToList();

            var queryResult = new QueryResult<ProductPriceRuleViewModel>(docs, totalItemCount, requestInfo.PageSize);
            var inquiryResponse = new PagedTaskDataInquiryResponse
            {
                Items = docs,
                TotalRecords = totalItemCount,
                PageCount = queryResult.TotalPageCount,
                PageNumber = requestInfo.PageNumber,
                PageSize = requestInfo.PageSize
            };

            return inquiryResponse;
        }

        public ProductPriceRule ActivateProductPriceRule(int id)
        {
            var original = GetValidProductPriceRule(id);
            ValidateAuthorization(original);

            original.Active = true;
            _dbContext.Set<ProductPriceRule>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
    }
}
