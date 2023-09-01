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
using PagedTaskDataInquiryResponse = HrevertCRM.Data.Common.PagedDataInquiryResponse<HrevertCRM.Data.ViewModels.ItemMeasureViewModel>;

namespace HrevertCRM.Data.QueryProcessors
{
    public class ItemMeasureQueryProcessor : QueryBase<ItemMeasure>, IItemMeasureQueryProcessor
    {

        public ItemMeasureQueryProcessor(IUserSession userSession, IDbContext dbContext)
            : base(userSession, dbContext)
        {
        }
        public ItemMeasure Update(ItemMeasure itemMeasure)
        {
            var original = GetValidItemMeasure(itemMeasure.Id);
            ValidateAuthorization(original);
            CheckVersionMismatch(itemMeasure, original);

            //pass value to original
            original.ProductId = itemMeasure.ProductId;
            original.MeasureUnitId = itemMeasure.MeasureUnitId;
            original.Price = itemMeasure.Price;
            original.MeasureUnitId = itemMeasure.MeasureUnitId;
            original.Active = itemMeasure.Active;
            original.WebActive = itemMeasure.WebActive;

            _dbContext.Set<ItemMeasure>().Update(original);
            _dbContext.SaveChanges();
            return itemMeasure;
        }
        public virtual ItemMeasure GetValidItemMeasure(int itemMeasureId)
        {
            var itemMeasure = _dbContext.Set<ItemMeasure>().FirstOrDefault(sc => sc.Id == itemMeasureId);
            if (itemMeasure == null)
            {
                throw new RootObjectNotFoundException(ItemMeasureConstants.ItemMeasureQueryProcessorConstants.ItemMeasureNotFound);
            }
            return itemMeasure;
        }

        public ItemMeasure GetItemMeasure(int itemMeasureId)
        {
            var itemMeasure = _dbContext.Set<ItemMeasure>().FirstOrDefault(d => d.Id == itemMeasureId);
            return itemMeasure;
        }
        public ItemMeasureViewModel GetItemMeasureViewModel(int itemMeasureId)
        {
            var mapper = new ItemMeasureToItemMeasureViewModelMapper();
            var itemMeasure = _dbContext.Set<ItemMeasure>().FirstOrDefault(d => d.Id == itemMeasureId);

            return mapper.Map(itemMeasure);
        }

        public void SaveAll(List<ItemMeasure> itemMeasures)
        {
            _dbContext.Set<ItemMeasure>().AddRange(itemMeasures);
            _dbContext.SaveChanges();
        }

        public int GetCompanyIdByItemMeasureId(int itemMeasureId)
        {
            return _dbContext.Set<ItemMeasure>().Where(p => p.Id == itemMeasureId).Select(p => p.CompanyId).Single();
        }

        public ItemMeasure Save(ItemMeasure itemMeasure)
        {
            itemMeasure.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<ItemMeasure>().Add(itemMeasure);
            _dbContext.SaveChanges();
            return itemMeasure;
        }

        public IQueryable<ItemMeasure> GetActiveItemMeasures(int distributorId)
        {
            return _dbContext.Set<ItemMeasure>().Where(x => x.CompanyId == distributorId);
        }

        public bool Delete(int itemMeasureId)
        {
            var doc = GetItemMeasure(itemMeasureId);
            if (doc == null) return false;
            _dbContext.Set<ItemMeasure>().Remove(doc);
            _dbContext.SaveChanges();
            return true;
        }

        public bool Exists(Expression<Func<ItemMeasure, bool>> where)
        {
            return _dbContext.Set<ItemMeasure>().Any(where);
        }

        public PagedTaskDataInquiryResponse GetItemMeasures(PagedDataRequest requestInfo, Expression<Func<ItemMeasure, bool>> where = null)
        {
            var query = _dbContext.Set<ItemMeasure>().Where(FilterByActiveTrueAndCompany);
            query = where == null ? query : query.Where(where);
            //var enumerable = query as ItemMeasure[] ?? query.ToArray();

            return FormatResultForPaging(requestInfo, query);
        }

        private static PagedTaskDataInquiryResponse FormatResultForPaging(PagedDataRequest requestInfo, IQueryable<ItemMeasure> query)
        {
            var totalItemCount = query.Count();
            var startIndex = ResultsPagingUtility.CalculateStartIndex(requestInfo.PageNumber, requestInfo.PageSize);
            var mapper = new ItemMeasureToItemMeasureViewModelMapper();
            var docs = query.OrderBy(x => x.DateCreated).Skip(startIndex).Take(requestInfo.PageSize).Select(s => mapper.Map(s)).ToList();
            var queryResult = new QueryResult<ItemMeasureViewModel>(docs, totalItemCount, requestInfo.PageSize);
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

        public ItemMeasure[] GetItemMeasures(Expression<Func<ItemMeasure, bool>> where = null)
        {
            var query = _dbContext.Set<ItemMeasure>().Where(FilterByActiveTrueAndCompany);

            query = where == null ? query : query.Where(where);
            var enumerable = query.ToArray();
            return enumerable;
        }

        public IQueryable<ItemMeasure> GetActiveItemMeasures()
        {
            return _dbContext.Set<ItemMeasure>().Include(x => x.Product).Where(p => p.Active && p.CompanyId == LoggedInUser.CompanyId && p.Product.Active);
        }
        public IQueryable<ItemMeasure> GetDeletedItemMeasures()
        {
            return _dbContext.Set<ItemMeasure>().Include(x => x.Product).Where(p => p.Active == false && p.CompanyId == LoggedInUser.CompanyId && p.Product.Active);
        }
        public IQueryable<ItemMeasure> GetAllItemMeasures()
        {
            return _dbContext.Set<ItemMeasure>().Include(x => x.Product).Where(p => p.CompanyId == LoggedInUser.CompanyId && p.Product.Active);
        }
        public PagedTaskDataInquiryResponse GetActiveItemMeasures(PagedDataRequest requestInfo, Expression<Func<ItemMeasure, bool>> where = null)
        {
            var query = _dbContext.Set<ItemMeasure>().Where(FilterByActiveTrueAndCompany);
            query = where == null ? query : query.Where(where);
            return FormatResultForPaging(requestInfo, query);
        }

        public PagedTaskDataInquiryResponse GetDeletedItemMeasures(PagedDataRequest requestInfo, Expression<Func<ItemMeasure, bool>> where = null)
        {
            var query = _dbContext.Set<ItemMeasure>().Where(FilterByActiveFalseAndCompany);
            query = where == null ? query : query.Where(where);
            return FormatResultForPaging(requestInfo, query);
        }

        public PagedTaskDataInquiryResponse GetAllItemMeasures(PagedDataRequest requestInfo, Expression<Func<ItemMeasure, bool>> where = null)
        {
            var query = _dbContext.Set<ItemMeasure>().Where(c => c.CompanyId == LoggedInUser.CompanyId);
            return FormatResultForPaging(requestInfo, query);
        }

        public PagedTaskDataInquiryResponse SearchActive(PagedDataRequest requestInfo, Expression<Func<ItemMeasure, bool>> where = null)
        {
            var filteredItemMeasure = _dbContext.Set<ItemMeasure>().Where(FilterByActiveTrueAndCompany);
            var query = string.IsNullOrEmpty(requestInfo.SearchText) ? filteredItemMeasure : filteredItemMeasure.Where(s
                                                                  => s.MeasureUnitId.ToString().Contains(requestInfo.SearchText.ToUpper())
                                                                  );
            //TODO: Make this LINQ query precompiled, using the method 
            return FormatResultForPaging(requestInfo, query);
        }

        public PagedTaskDataInquiryResponse SearchAll(PagedDataRequest requestInfo, Expression<Func<ItemMeasure, bool>> @where = null)
        {
            var filteredItemMeasure = _dbContext.Set<ItemMeasure>().Where(x => x.CompanyId == LoggedInUser.CompanyId);
            var query = string.IsNullOrEmpty(requestInfo.SearchText) ? filteredItemMeasure : filteredItemMeasure.Where(s
                                                                  => s.MeasureUnitId.ToString().Contains(requestInfo.SearchText.ToUpper()));
            //TODO: Make this LINQ query precompiled, using the method 
            return FormatResultForPaging(requestInfo, query);
        }

        public ItemMeasure ActivateItemMeasure(int id)
        {
            var original = GetValidItemMeasure(id);
            ValidateAuthorization(original);

            original.Active = true;
            _dbContext.Set<ItemMeasure>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

       public ItemMeasure CheckIfDeletedItemMeasureWithSameProductIdExists(int productId)
        {
            var deliveryZone =
                _dbContext.Set<ItemMeasure>()
                    .FirstOrDefault(
                        x => x.CompanyId == LoggedInUser.CompanyId &&
                             x.ProductId == productId && (x.Active ||
                             x.Active == false));
            return deliveryZone;
        }

        public IQueryable<ItemMeasure> SearchItemMeasures(bool active, string searchText)
        {
            var filteredItemMeasures = _dbContext.Set<ItemMeasure>().Include(x => x.Product).Where(x => x.CompanyId == LoggedInUser.CompanyId);
            if (active)
                filteredItemMeasures = filteredItemMeasures.Where(x => x.Active);
            var query = string.IsNullOrEmpty(searchText) || searchText.Contains("null")
                ? filteredItemMeasures
                : filteredItemMeasures.Where(s
                    => s.MeasureUnitId.ToString().Contains(searchText.ToUpper()) || s.Product.Name.ToUpper().Contains(searchText.ToUpper()));
            return query;
        }
        public bool DeleteRange(List<int?> itemMeasuresId)
        {
            var itemMeasureList = itemMeasuresId.Select(itemMeasureId => _dbContext.Set<ItemMeasure>().FirstOrDefault(x => x.Id == itemMeasureId && x.CompanyId == LoggedInUser.CompanyId)).ToList();
            itemMeasureList.ForEach(x => x.Active = false);
            _dbContext.Set<ItemMeasure>().UpdateRange(itemMeasureList);
            _dbContext.SaveChanges();
            return true;
        }
    }
}
