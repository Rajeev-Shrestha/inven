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
using PagedTaskDataInquiryResponse = HrevertCRM.Data.Common.PagedDataInquiryResponse<HrevertCRM.Data.ViewModels.DeliveryZoneViewModel>;

namespace HrevertCRM.Data.QueryProcessors
{
    public class DeliveryZoneQueryProcessor : QueryBase<DeliveryZone>, IDeliveryZoneQueryProcessor
    {

        public DeliveryZoneQueryProcessor(IUserSession userSession, IDbContext dbContext)
            : base(userSession, dbContext)
        {
        }
        public DeliveryZone Update(DeliveryZone deliveryZone)
        {
            var original = GetValidDeliveryZone(deliveryZone.Id);
            ValidateAuthorization(original);
            CheckVersionMismatch(deliveryZone, original);

            //pass value to original
            original.ZoneName = deliveryZone.ZoneName;
            original.ZoneCode = deliveryZone.ZoneCode;
            original.Active = deliveryZone.Active;
            original.WebActive = deliveryZone.WebActive;

            _dbContext.Set<DeliveryZone>().Update(original);
            _dbContext.SaveChanges();
            return deliveryZone;
        }
        public virtual DeliveryZone GetValidDeliveryZone(int deliveryZoneId)
        {
            var deliveryZone = _dbContext.Set<DeliveryZone>().FirstOrDefault(sc => sc.Id == deliveryZoneId);
            if (deliveryZone == null)
            {
                throw new RootObjectNotFoundException(DeliveryZoneConstants.DeliveryZoneQueryProcessorConstants.DeliveryZoneNotFound);
            }
            return deliveryZone;
        }

        public DeliveryZone GetDeliveryZone(int deliveryZoneId)
        {
            var deliveryZone = _dbContext.Set<DeliveryZone>().FirstOrDefault(d => d.Id == deliveryZoneId);
            return deliveryZone;
        }

        public DeliveryZoneViewModel GetDeliveryZoneViewModel(int deliveryZoneId)
        {
            var mapper = new DeliveryZoneToDeliveryZoneViewModelMapper();
            var deliveryZone = _dbContext.Set<DeliveryZone>().FirstOrDefault(d => d.Id == deliveryZoneId);

            return mapper.Map(deliveryZone);
        }

        public void SaveAll(List<DeliveryZone> deliveryZones)
        {
            _dbContext.Set<DeliveryZone>().AddRange(deliveryZones);
            _dbContext.SaveChanges();
        }

        public int GetCompanyIdByDeliveryZoneId(int deliveryZoneId)
        {
            return _dbContext.Set<DeliveryZone>().Where(p => p.Id == deliveryZoneId).Select(p => p.CompanyId).Single();
        }

        public DeliveryZone Save(DeliveryZone deliveryZone)
        {
            deliveryZone.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<DeliveryZone>().Add(deliveryZone);
            _dbContext.SaveChanges();
            return deliveryZone;
        }

        public bool Delete(int deliveryZoneId)
        {
            var doc = GetDeliveryZone(deliveryZoneId);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<DeliveryZone>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }

        public bool Exists(Expression<Func<DeliveryZone, bool>> where)
        {
            return _dbContext.Set<DeliveryZone>().Any(where);
        }

        public PagedTaskDataInquiryResponse GetDeliveryZones(PagedDataRequest requestInfo, Expression<Func<DeliveryZone, bool>> where = null)
        {
            var query = _dbContext.Set<DeliveryZone>().Where(FilterByActiveTrueAndCompany);
            query = where == null ? query : query.Where(where);
            return FormatResultForPaging(requestInfo, query);
        }

        private static PagedTaskDataInquiryResponse FormatResultForPaging(PagedDataRequest requestInfo, IQueryable<DeliveryZone> query)
        {
            var totalItemCount = query.Count();
            var startIndex = ResultsPagingUtility.CalculateStartIndex(requestInfo.PageNumber, requestInfo.PageSize);
            var mapper = new DeliveryZoneToDeliveryZoneViewModelMapper();
            var docs = query.OrderBy(x => x.DateCreated).Skip(startIndex).Take(requestInfo.PageSize).Select(s => mapper.Map(s)).ToList();
            var queryResult = new QueryResult<DeliveryZoneViewModel>(docs, totalItemCount, requestInfo.PageSize);
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

        public DeliveryZone[] GetDeliveryZones(Expression<Func<DeliveryZone, bool>> where = null)
        {
            var query = _dbContext.Set<DeliveryZone>().Where(FilterByActiveTrueAndCompany);

            query = where == null ? query : query.Where(where);
            var enumerable = query.ToArray();
            return enumerable;
        }

        public DeliveryZone ActivateDeliveryZone(int id)
        {
            var original = GetValidDeliveryZone(id);
            ValidateAuthorization(original);

            original.Active = true;
            _dbContext.Set<DeliveryZone>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

       public DeliveryZone CheckIfDeletedDeliveryZoneWithSameCodeExists(string code)
        {
            var deliveryZone =
                _dbContext.Set<DeliveryZone>()
                    .FirstOrDefault(
                        x => x.CompanyId == LoggedInUser.CompanyId &&
                             x.ZoneCode == code && (x.Active ||
                             x.Active == false));
            return deliveryZone;
        }

        public IQueryable<DeliveryZone> SearchDeliveryZones(bool active, string searchText)
        {
            var filteredDeliveryZones = _dbContext.Set<DeliveryZone>().Where(x => x.CompanyId == LoggedInUser.CompanyId);
            if (active)
                filteredDeliveryZones = filteredDeliveryZones.Where(x => x.Active);
            var query = string.IsNullOrEmpty(searchText) || searchText.Contains("null") ? filteredDeliveryZones : filteredDeliveryZones.Where(s =>
                                                                 s.ZoneName.ToUpper().Contains(searchText.ToUpper()) ||
                                                                 s.ZoneCode.ToUpper().Contains(searchText.ToUpper()));
            return query;
        }

        public bool DeleteRange(List<int?> deliveryZonesId)
        {
            var deliveryZoneList = deliveryZonesId.Select(deliveryZoneId => _dbContext.Set<DeliveryZone>().FirstOrDefault(x => x.Id == deliveryZoneId)).ToList();
            deliveryZoneList.ForEach(x => x.Active = false);
            _dbContext.Set<DeliveryZone>().UpdateRange(deliveryZoneList);
            _dbContext.SaveChanges();
            return true;
        }
    }
}
