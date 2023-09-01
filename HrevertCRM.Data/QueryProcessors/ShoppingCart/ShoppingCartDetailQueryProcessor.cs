using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Hrevert.Common.Constants.ShoppingCart;
using Hrevert.Common.Security;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.Mapper;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore;
using PagedTaskDataInquiryResponse = HrevertCRM.Data.Common.PagedDataInquiryResponse<HrevertCRM.Data.ViewModels.ShoppingCartDetailViewModel>;

namespace HrevertCRM.Data.QueryProcessors
{
    public class ShoppingCartDetailQueryProcessor : QueryBase<ShoppingCartDetail>, IShoppingCartDetailQueryProcessor
    {
        public ShoppingCartDetailQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
        {
        }
        public ShoppingCartDetail Update(ShoppingCartDetail shoppingCartDetail)
        {
            var original = GetValidShoppingCartDetail(shoppingCartDetail.Id);
            ValidateAuthorization(shoppingCartDetail);
            CheckVersionMismatch(shoppingCartDetail, original);

            original.ProductId = shoppingCartDetail.ProductId;
            original.ProductCost = shoppingCartDetail.ProductCost;
            original.Quantity = shoppingCartDetail.Quantity;
            original.Discount = shoppingCartDetail.Discount;
            original.TaxAmount = shoppingCartDetail.TaxAmount;
            original.ShoppingDateTime = shoppingCartDetail.ShoppingDateTime;
            original.ShoppingCartId = shoppingCartDetail.ShoppingCartId;
            original.Active = shoppingCartDetail.Active;
            original.WebActive = shoppingCartDetail.WebActive;
            original.CompanyId = shoppingCartDetail.CompanyId;

            _dbContext.Set<ShoppingCartDetail>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        public virtual ShoppingCartDetail GetValidShoppingCartDetail(int shoppingCartDetailId)
        {
            var shoppingCartDetail = _dbContext.Set<ShoppingCartDetail>().FirstOrDefault(sc => sc.Id == shoppingCartDetailId);
            if (shoppingCartDetail == null)
            {
                throw new RootObjectNotFoundException(ShoppingCartConstants.ShoppingCartDetailQueryProcessorConstants.ShoppingCartDetailNotFound);
            }
            return shoppingCartDetail;
        }

        //public EditShoppingCartDetailViewModel GetShoppingCartDetailViewModel(int ShoppingCartDetailId)
        //{
        //    var ShoppingCartDetail = _dbContext.Set<ShoppingCartDetail>().Where(d => d.Id == ShoppingCartDetailId).Include(s => s.Addresses).Single();
        //    var mapper = new ShoppingCartDetailToEditShoppingCartDetailViewModelMapper();
        //    return mapper.Map(ShoppingCartDetail);
        //}
        public ShoppingCartDetail GetShoppingCartDetail(int shoppingCartDetailId)
        {
            var shoppingCartDetail = _dbContext.Set<ShoppingCartDetail>().FirstOrDefault(d => d.Id == shoppingCartDetailId);
            return shoppingCartDetail;
        }
        public void SaveAllShoppingCartDetail(List<ShoppingCartDetail> shoppingCartDetails)
        {
            _dbContext.Set<ShoppingCartDetail>().AddRange(shoppingCartDetails);
            _dbContext.SaveChanges();
        }

        public ShoppingCartDetail Save(ShoppingCartDetail shoppingCartDetail)
        {
            _dbContext.Set<ShoppingCartDetail>().Add(shoppingCartDetail);
            _dbContext.SaveChanges();
            return shoppingCartDetail;
        }

        public int SaveAll(List<ShoppingCartDetail> shoppingCartDetails)
        {
            _dbContext.Set<ShoppingCartDetail>().AddRange(shoppingCartDetails);
            return _dbContext.SaveChanges();

        }

        //public List<ShoppingCartDetail> SearchForShoppingCartDetails(string searchString)
        //{
        //    var ShoppingCartDetails = new List<ShoppingCartDetail>();
        //    if (!String.IsNullOrEmpty(searchString))
        //    {
        //        ShoppingCartDetails =
        //            _dbContext.Set<ShoppingCartDetail>().Where(s => s.Name.ToUpper().Contains(searchString.ToUpper())
        //                                                  || s.Code.ToUpper().Contains(searchString.ToUpper()) ||
        //                                                  s.Email.ToUpper().Contains(searchString.ToUpper())
        //                                                  || s.ContactName.ToUpper().Contains(searchString.ToUpper())).ToList();
        //    }
        //    ShoppingCartDetails = ShoppingCartDetails.Where(c => c.CompanyId == LoggedInUser.CompanyId).ToList();
        //    return ShoppingCartDetails;
        //}

        public ShoppingCartDetail ActivateShoppingCartDetail(int id)
        {
            var original = GetValidShoppingCartDetail(id);
            ValidateAuthorization(original);

            original.Active = true;
            _dbContext.Set<ShoppingCartDetail>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        public List<ProductViewModel> GetTrendingProducts()
        {
            var trendingProducts = _dbContext.Set<ShoppingCartDetail>().Include(x => x.Product)
                .Where(x => x.CompanyId == LoggedInUser.CompanyId && x.Active).GroupBy(x => x.ProductId)
                .Select(x => new {Id = x.Key, Count = x.Count()}).OrderByDescending(x => x.Count).Take(10);
            var productsVmList = new List<ProductViewModel>();
            foreach (var trendingProduct in trendingProducts)
            {
                productsVmList.Add(GetProductViewModel(trendingProduct.Id));
            }
            return productsVmList;
        }

        public List<ProductCategoryViewModel> GetTopCategories()
        {
            var listOfProductId = _dbContext.Set<ShoppingCartDetail>()
                .Where(x => x.CompanyId == LoggedInUser.CompanyId && x.Active).Select(x => x.ProductId).ToList();
            var listOfCategories = new List<int>();
            foreach (var productId in listOfProductId)
            {
                var categories = _dbContext.Set<ProductInCategory>()
                    .Where(x => x.CompanyId == LoggedInUser.CompanyId && x.Active && x.ProductId == productId)
                    .Select(x => x.CategoryId).ToList();
                if (categories.Count > 0)
                {
                    listOfCategories.AddRange(categories);
                }
            }
            var trendingCategories = listOfCategories.GroupBy(x => x).Select(x => new {Id = x.Key, Count = x.Count()})
                .OrderByDescending(x => x.Count).Take(4);
            var categoriesList = new List<ProductCategory>();
            foreach (var trendingCategory in trendingCategories )
            {
                var productCategory = _dbContext.Set<ProductCategory>().Include(x => x.ProductInCategories).FirstOrDefault(d => d.Id == trendingCategory.Id);
                if(productCategory != null)
                    categoriesList.Add(productCategory);
            }
            var productCategoryMapper = new ProductCategoryToProductCategoryViewModelMapper();
            return (categoriesList.Count > 0) ? productCategoryMapper.Map(categoriesList) : null;
        }

        public List<ProductViewModel> GetHotThisWeek()
        {
            var trendingProducts = _dbContext.Set<ShoppingCartDetail>().Include(x => x.Product)
                .Where(x => x.CompanyId == LoggedInUser.CompanyId && x.Active).GroupBy(x => x.ProductId)
                .Select(x => new { Id = x.Key, Count = x.Count() }).OrderByDescending(x => x.Count).Take(10);
            var productsVmList = new List<ProductViewModel>();
            foreach (var trendingProduct in trendingProducts)
            {
                productsVmList.Add(GetProductViewModel(trendingProduct.Id));
            }
            return productsVmList;
        }

        private ProductViewModel GetProductViewModel(int productId)
        {
            var mapper = new ProductToProductViewModelMapper();
            var product = _dbContext.Set<Product>()
                .Include(pc => pc.ProductInCategories).
                Include(x => x.ProductMetadatas).
                Include(x => x.ProductsReferencedByKitAndAssembledTypes).
                Include(x => x.TaxesInProducts).FirstOrDefault(d => d.Id == productId && d.CompanyId == LoggedInUser.CompanyId);
            var metadatas = product.ProductMetadatas;
            product.ProductMetadatas = null;
            product.ProductMetadatas = metadatas.Where(x => x.CompanyId == LoggedInUser.CompanyId && x.Active).ToList();
            return mapper.Map(product);
        }

        public bool Delete(int shoppingCartDetailId)
        {
            var doc = GetShoppingCartDetail(shoppingCartDetailId);
            ValidateAuthorization(doc);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<ShoppingCartDetail>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }

        public bool Exists(Expression<Func<ShoppingCartDetail, bool>> @where)
        {
            return _dbContext.Set<ShoppingCartDetail>().Any(@where);
        }

        public PagedTaskDataInquiryResponse GetActiveShoppingCartDetails(PagedDataRequest requestInfo, Expression<Func<ShoppingCartDetail, bool>> @where = null)
        {
            var query = _dbContext.Set<ShoppingCartDetail>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);
            return FormatResultForPaging(requestInfo, query);
        }

        private static PagedTaskDataInquiryResponse FormatResultForPaging(PagedDataRequest requestInfo, IQueryable<ShoppingCartDetail> query)
        {
            var totalItemCount = query.Count();

            var startIndex = ResultsPagingUtility.CalculateStartIndex(requestInfo.PageNumber, requestInfo.PageSize);
            var mapper = new ShoppingCartDetailToShoppingCartDetailViewModelMapper();
            var docs = query.OrderBy(x => x.DateCreated).Skip(startIndex).Take(requestInfo.PageSize).Select(
                s => mapper.Map(s)).ToList();

            var queryResult = new QueryResult<ShoppingCartDetailViewModel>(docs, totalItemCount, requestInfo.PageSize);
            var inquiryResponse = new PagedTaskDataInquiryResponse()
            {
                Items = docs,
                TotalRecords = totalItemCount,
                PageCount = queryResult.TotalPageCount,
                PageNumber = requestInfo.PageNumber,
                PageSize = requestInfo.PageSize
            };

            return inquiryResponse;
        }

        public PagedTaskDataInquiryResponse GetDeletedShoppingCartDetails(PagedDataRequest requestInfo, Expression<Func<ShoppingCartDetail, bool>> @where = null)
        {
            var query = _dbContext.Set<ShoppingCartDetail>().Where(FilterByActiveFalseAndCompany);
            query = @where == null ? query : query.Where(@where);
            return FormatResultForPaging(requestInfo, query);
        }

        public PagedTaskDataInquiryResponse GetAllShoppingCartDetails(PagedDataRequest requestInfo, Expression<Func<ShoppingCartDetail, bool>> @where = null)
        {
            var query = _dbContext.Set<ShoppingCartDetail>().Where(c => c.CompanyId == LoggedInUser.CompanyId);
            return FormatResultForPaging(requestInfo, query);
        }

        public ShoppingCartDetail[] GetShoppingCartDetails(Expression<Func<ShoppingCartDetail, bool>> @where = null)
        {

            var query = _dbContext.Set<ShoppingCartDetail>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            var enumerable = query.ToArray();
            return enumerable;
        }

        //public IQueryable<ShoppingCartDetail> GetActiveShoppingCartDetails()
        //{
        //    return _dbContext.Set<ShoppingCartDetail>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active);
        //}

        //public IQueryable<ShoppingCartDetail> GetDeletedShoppingCartDetails()
        //{
        //    return _dbContext.Set<ShoppingCartDetail>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active == false);
        //}
        //public IQueryable<ShoppingCartDetail> GetAllShoppingCartDetails()
        //{
        //    return _dbContext.Set<ShoppingCartDetail>().Where(f => f.CompanyId == LoggedInUser.CompanyId);
        //}
    }
}
