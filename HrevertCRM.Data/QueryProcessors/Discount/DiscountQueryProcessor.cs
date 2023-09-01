using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Hrevert.Common.Constants;
using Hrevert.Common.Security;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.Mapper;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore;
using PagedTaskDataInquiryResponse = HrevertCRM.Data.Common.PagedDataInquiryResponse<HrevertCRM.Data.ViewModels.DiscountViewModel>;

namespace HrevertCRM.Data.QueryProcessors
{
    public class DiscountQueryProcessor : QueryBase<Discount>, IDiscountQueryProcessor
    {
        public DiscountQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
        {
        }
        public Discount Update(Discount discount)
        {
            var original = GetValidDiscount(discount.Id);
            ValidateAuthorization(discount);
            CheckVersionMismatch(discount, original);

            original.ItemId = discount.ItemId;
            original.CategoryId = discount.CategoryId;
            original.DiscountType = discount.DiscountType;
            original.DiscountValue = discount.DiscountValue;
            original.DiscountStartDate = discount.DiscountStartDate;
            original.DiscountEndDate = discount.DiscountEndDate;
            original.MinimumQuantity = discount.MinimumQuantity;
            original.CustomerId = discount.CustomerId;
            original.CustomerLevelId = discount.CustomerLevelId;
            original.WebActive = discount.WebActive;
            original.Active = discount.Active;

            _dbContext.Set<Discount>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        public virtual Discount GetValidDiscount(int discountId)
        {
            var discount = _dbContext.Set<Discount>().FirstOrDefault(sc => sc.Id == discountId);
            if (discount == null)
            {
                throw new RootObjectNotFoundException(DiscountConstants.DiscountQueryProcessorConstants.DiscountNotFound);
            }
            return discount;
        }

        public DiscountViewModel GetDiscountViewModel(int discountId)
        {
            var discount = _dbContext.Set<Discount>().FirstOrDefault(d => d.CompanyId == LoggedInUser.CompanyId && d.Id == discountId);
            var mapper = new DiscountToDiscountViewModelMapper();
            return mapper.Map(discount);
        }
        public Discount GetDiscount(int discountId)
        {
            var discount = _dbContext.Set<Discount>().FirstOrDefault(d => d.Id == discountId);
            return discount;
        }
        public void SaveAllDiscount(List<Discount> discounts)
        {
            _dbContext.Set<Discount>().AddRange(discounts);
            _dbContext.SaveChanges();
        }

        public Discount Save(Discount discount)
        {
            discount.CompanyId = LoggedInUser.CompanyId;
            discount.Active = true;
            _dbContext.Set<Discount>().Add(discount);
            _dbContext.SaveChanges();
            return discount;
        }

        public int SaveAll(List<Discount> discounts)
        {
            _dbContext.Set<Discount>().AddRange(discounts);
            return _dbContext.SaveChanges();
        }

        public Discount ActivateDiscount(int id)
        {
            var original = GetValidDiscount(id);
            ValidateAuthorization(original);

            original.Active = true;
            _dbContext.Set<Discount>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        public bool Delete(int discountId)
        {
            var doc = GetDiscount(discountId);
            ValidateAuthorization(doc);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<Discount>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }

        public bool Exists(Expression<Func<Discount, bool>> where)
        {
            return _dbContext.Set<Discount>().Any(where);
        }

        public PagedTaskDataInquiryResponse GetDiscounts(PagedDataRequest requestInfo, Expression<Func<Discount, bool>> where = null)
        {
            var discounts = _dbContext.Set<Discount>()
                .Include(x => x.Product)
                .Include(x => x.ProductCategory)
                .Include(x => x.Customer)
                .Include(x => x.CustomerLevel)
                .Where(x => x.CompanyId == LoggedInUser.CompanyId);
            
            if (requestInfo.Active)
                discounts = discounts.Where(x => x.Active);
            var query = string.IsNullOrEmpty(requestInfo.SearchText)
                ? discounts.ToList()
                : discounts.Where(x => x.Product.Name.ToUpper().Contains(requestInfo.SearchText)
                                       || x.Customer.DisplayName.ToUpper().Contains(requestInfo.SearchText) ||
                                       x.ProductCategory.Name.ToUpper().Contains(requestInfo.SearchText) ||
                                       x.Product.Code.ToUpper().Contains(requestInfo.SearchText)).ToList();
            if (query == null || query.Count <= 0) return FormatResultForPaging(requestInfo, query);
            foreach (var discount in query)
            {
                if (discount.Product != null) discount.Product.Discounts = null;
                if (discount.ProductCategory != null) discount.ProductCategory.Discounts = null;
                if (discount.Customer != null) discount.Customer.Discounts = null;
                if (discount.CustomerLevel != null) discount.CustomerLevel.Discounts = null;
            }
            return FormatResultForPaging(requestInfo, query);
        }

        private static PagedTaskDataInquiryResponse FormatResultForPaging(PagedDataRequest requestInfo, List<Discount> query)
        {
            var totalItemCount = query.Count();
            var startIndex = ResultsPagingUtility.CalculateStartIndex(requestInfo.PageNumber, requestInfo.PageSize);
            var mapper = new DiscountToDiscountViewModelMapper();
            var docs = query.OrderBy(x => x.DateCreated).Skip(startIndex).Take(requestInfo.PageSize).Select(
                s => mapper.Map(s)).ToList();

            var queryResult = new QueryResult<DiscountViewModel>(docs, totalItemCount, requestInfo.PageSize);
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

        public Discount[] GetDiscounts(Expression<Func<Discount, bool>> where = null)
        {

            var query = _dbContext.Set<Discount>().Where(FilterByActiveTrueAndCompany);
            query = where == null ? query : query.Where(where);

            var enumerable = query.ToArray();
            return enumerable;
        }

        public IQueryable<Discount> GetActiveDiscountsWithoutPaging()
        {
            return _dbContext.Set<Discount>().Where(FilterByActiveTrueAndCompany);
        }
        public IQueryable<Discount> GetDeletedDiscountsWithoutPaging()
        {
            return _dbContext.Set<Discount>().Where(FilterByActiveFalseAndCompany);
        }

        public bool CheckDiscountPriceWithProductUnitPrice(int productId, double discountValue)
        {
            var product = _dbContext.Set<Product>().Where(p => p.Id == productId && p.Active);
            var result = product.SingleOrDefault().UnitPrice < discountValue;
            return result;
        }

        public bool DeleteRange(List<int?> discountsId)
        {
            var discountList = discountsId.Select(discountId => _dbContext.Set<Discount>().FirstOrDefault(x => x.Id == discountId && x.CompanyId == LoggedInUser.CompanyId)).ToList();
            discountList.ForEach(x => x.Active = false);
            _dbContext.Set<Discount>().UpdateRange(discountList);
            _dbContext.SaveChanges();
            return true;
        }

        public List<Discount> GetDiscountsProductWise()
        {
            var productsWithDiscounts = _dbContext.Set<Discount>().Include(x => x.Product).Where(x => x.ItemId != null && x.Active && x.DiscountEndDate >= DateTime.Now).ToList();
            return productsWithDiscounts;
        }

        public List<Discount> GetDiscountsCategoryWise()
        {
            var categoriesWithDiscounts = _dbContext.Set<Discount>().Include(x => x.ProductCategory).Where(x => x.CategoryId != null && x.Active && x.DiscountEndDate >= DateTime.Now).ToList();
            return categoriesWithDiscounts;
        }

        public Discount GetDiscountOfProductForSlide(int id)
        {
            var productsWithDiscounts = _dbContext.Set<Discount>().Include(x => x.Product)
                .FirstOrDefault(x => x.ItemId == id && x.Active);
            return productsWithDiscounts;
        }

        public Discount GetDiscountOfCategoryForSlide(int categoryId)
        {
            var productsWithDiscounts = _dbContext.Set<Discount>().Include(x => x.ProductCategory)
                .FirstOrDefault(x => x.CategoryId == categoryId && x.Active);
            return productsWithDiscounts;
        }
    }
}
