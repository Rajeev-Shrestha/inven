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
using PagedTaskDataInquiryResponse = HrevertCRM.Data.Common.PagedDataInquiryResponse<HrevertCRM.Data.ViewModels.DeliveryRateViewModel>;

namespace HrevertCRM.Data.QueryProcessors
{
    public class DeliveryRateQueryProcessor : QueryBase<DeliveryRate>, IDeliveryRateQueryProcessor
    {
        public DeliveryRateQueryProcessor(IUserSession userSession, IDbContext dbContext)
            : base(userSession, dbContext)
        {
        }
        public DeliveryRate Update(DeliveryRate deliveryRate)
        {
            var original = GetValidDeliveryRate(deliveryRate.Id);
            ValidateAuthorization(original);
            CheckVersionMismatch(deliveryRate, original);

            //pass value to original
            original.DeliveryMethodId = deliveryRate.DeliveryMethodId;
            original.DeliveryZoneId = deliveryRate.DeliveryZoneId;
            original.WeightFrom = deliveryRate.WeightFrom;
            original.WeightTo = deliveryRate.WeightTo;
            original.ProductCategoryId = deliveryRate.ProductCategoryId;
            original.ProductId = deliveryRate.ProductId;
            original.MinimumRate = deliveryRate.MinimumRate;
            original.Rate = deliveryRate.Rate;
            original.DocTotalFrom = deliveryRate.DocTotalFrom;
            original.DocTotalTo = deliveryRate.DocTotalTo;
            original.UnitFrom = deliveryRate.UnitFrom;
            original.UnitTo = deliveryRate.UnitTo;
            original.MeasureUnitId = deliveryRate.MeasureUnitId;
            original.Active = deliveryRate.Active;
            original.WebActive = deliveryRate.WebActive;

            _dbContext.Set<DeliveryRate>().Update(original);
            _dbContext.SaveChanges();
            return deliveryRate;
        }
        public virtual DeliveryRate GetValidDeliveryRate(int deliveryRateId)
        {
            var deliveryRate = _dbContext.Set<DeliveryRate>().FirstOrDefault(sc => sc.Id == deliveryRateId);
            if (deliveryRate == null)
            {
                throw new RootObjectNotFoundException(DeliveryRateConstants.DeliveryRateQueryProcessorConstants.DeliveryRateNotFound);
            }
            return deliveryRate;
        }
        public DeliveryRate GetDeliveryRate(int deliveryRateId)
        {
            var deliveryRate = _dbContext.Set<DeliveryRate>().FirstOrDefault(d => d.Id == deliveryRateId);
            return deliveryRate;
        }
        public DeliveryRateViewModel GetDeliveryRateViewModel(int deliveryRateId)
        {
            var mapper = new DeliveryRateToDeliveryRateViewModelMapper();
            var deliveryRate = _dbContext.Set<DeliveryRate>().FirstOrDefault(d => d.Id == deliveryRateId);

            return mapper.Map(deliveryRate);
        }


        public void SaveAllDeliveryRate(List<DeliveryRate> deliveryRates)
        {
            _dbContext.Set<DeliveryRate>().AddRange(deliveryRates);
            _dbContext.SaveChanges();
        }

        public int GetCompanyIdByDeliveryRateId(int deliveryRateId)
        {
            return _dbContext.Set<DeliveryRate>().Where(p => p.Id == deliveryRateId).Select(p => p.CompanyId).Single();

        }

        public DeliveryRate Save(DeliveryRate deliveryRate)
        {
            deliveryRate.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<DeliveryRate>().Add(deliveryRate);
            _dbContext.SaveChanges();
            return deliveryRate;
        }

        public IQueryable<DeliveryRate> GetActiveDeliveryRates(int distributorId)
        {
            return _dbContext.Set<DeliveryRate>().Where(x => x.CompanyId == distributorId && x.Active);
        }

        public IQueryable<DeliveryRate> SearchDeliveryRates(bool active, string searchText)
        {
            var query = _dbContext.Set<DeliveryRate>().Include(x => x.Product).Where(x => x.CompanyId == LoggedInUser.CompanyId);
            if (active)
                query = query.Where(x => x.Active);
            var result = string.IsNullOrEmpty(searchText) || searchText.Contains("null")
                ? query
                : query.Where(
                    x =>
                        x.Product.Name.ToUpper().Contains(searchText.ToUpper()) ||
                        x.Product.Code.ToUpper().Contains(searchText.ToUpper()));
            return result;
        }

        public bool Delete(int deliveryRateId)
        {
            var doc = GetDeliveryRate(deliveryRateId);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<DeliveryRate>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }

        public bool Exists(Expression<Func<DeliveryRate, bool>> where)
        {
            return _dbContext.Set<DeliveryRate>().Any(where);
        }

        public PagedTaskDataInquiryResponse GetDeliveryRates(PagedDataRequest requestInfo, Expression<Func<DeliveryRate, bool>> where = null)
        {
            var query = _dbContext.Set<DeliveryRate>().Where(FilterByActiveTrueAndCompany);
            query = where == null ? query : query.Where(where);
            return FormatResultForPaging(requestInfo, query);
        }

        private static PagedTaskDataInquiryResponse FormatResultForPaging(PagedDataRequest requestInfo, IQueryable<DeliveryRate> query)
        {
            var totalItemCount = query.Count();
            var startIndex = ResultsPagingUtility.CalculateStartIndex(requestInfo.PageNumber, requestInfo.PageSize);
            var mapper = new DeliveryRateToDeliveryRateViewModelMapper();
            var docs = query.OrderBy(x => x.DateCreated).Skip(startIndex).Take(requestInfo.PageSize).Select(s => mapper.Map(s)).ToList();
            var queryResult = new QueryResult<DeliveryRateViewModel>(docs, totalItemCount, requestInfo.PageSize);
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

        public DeliveryRate[] GetDeliveryRates(Expression<Func<DeliveryRate, bool>> where = null)
        {
            var query = _dbContext.Set<DeliveryRate>().Where(FilterByActiveTrueAndCompany);

            query = where == null ? query : query.Where(where);
            var enumerable = query.ToArray();
            return enumerable;
        }
        

        public DeliveryRate ActivateDeliveryRate(int id)
        {
            var original = GetValidDeliveryRate(id);
            ValidateAuthorization(original);

            original.Active = true;
            _dbContext.Set<DeliveryRate>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        public bool DeleteRange(List<int?> deliveryRatesId)
        {
            var deliveryRatesList = deliveryRatesId.Select(deliveryRateId => _dbContext.Set<DeliveryRate>().FirstOrDefault(x => x.Id == deliveryRateId)).ToList();
            deliveryRatesList.ForEach(x => x.Active = false);
            _dbContext.Set<DeliveryRate>().UpdateRange(deliveryRatesList);
            _dbContext.SaveChanges();
            return true;
        }
    }
}
