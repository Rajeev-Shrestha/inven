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
using PagedTaskDataInquiryResponse = HrevertCRM.Data.Common.PagedDataInquiryResponse<HrevertCRM.Data.ViewModels.MeasureUnitViewModel>;
using HrevertCRM.Entities.Enumerations;

namespace HrevertCRM.Data.QueryProcessors
{
    public class MeasureUnitQueryProcessor : QueryBase<MeasureUnit>, IMeasureUnitQueryProcessor
    {
        public MeasureUnitQueryProcessor(IUserSession userSession, IDbContext dbContext)
            : base(userSession, dbContext)
        {
        }
        public MeasureUnit Update(MeasureUnit measureUnit)
        {
            var original = GetValidMeasureUnit(measureUnit.Id);
            ValidateAuthorization(original);
            CheckVersionMismatch(measureUnit, original);

            //pass value to original
            original.Measure = measureUnit.Measure;
            original.MeasureCode = measureUnit.MeasureCode;
            original.EntryMethod = measureUnit.EntryMethod;
            original.Active = measureUnit.Active;
            original.WebActive = measureUnit.WebActive;

            _dbContext.Set<MeasureUnit>().Update(original);
            _dbContext.SaveChanges();
            return measureUnit;
        }
        public virtual MeasureUnit GetValidMeasureUnit(int measureUnitId)
        {
            var measureUnit = _dbContext.Set<MeasureUnit>().FirstOrDefault(sc => sc.Id == measureUnitId);
            if (measureUnit == null)
            {
                throw new RootObjectNotFoundException(MeasureUnitConstants.MeasureUnitQueryProcessorConstants.MeasureUnitNotFound);
            }
            return measureUnit;
        }

        public MeasureUnit GetMeasureUnit(int measureUnitId)
        {
            var measureUnit = _dbContext.Set<MeasureUnit>().FirstOrDefault(d => d.Id == measureUnitId);
            return measureUnit;
        }
        public MeasureUnitViewModel GetMeasureUnitViewModel(int measureUnitId)
        {
            var mapper = new MeasureUnitToMeasureUnitViewModelMapper();
            var measureUnit = _dbContext.Set<MeasureUnit>().FirstOrDefault(d => d.Id == measureUnitId);

            return mapper.Map(measureUnit);
        }

        public void SaveAll(List<MeasureUnit> measureUnits)
        {
            _dbContext.Set<MeasureUnit>().AddRange(measureUnits);
            _dbContext.SaveChanges();
        }

        public int GetCompanyIdByMeasureUnitId(int measureUnitId)
        {
            return _dbContext.Set<MeasureUnit>().Where(p => p.Id == measureUnitId).Select(p => p.CompanyId).Single();
        }

        public MeasureUnit Save(MeasureUnit measureUnit)
        {
            measureUnit.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<MeasureUnit>().Add(measureUnit);
            _dbContext.SaveChanges();
            return measureUnit;
        }

        public bool Delete(int measureUnitId)
        {
            var doc = GetMeasureUnit(measureUnitId);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<MeasureUnit>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }

        public bool Exists(Expression<Func<MeasureUnit, bool>> where)
        {
            return _dbContext.Set<MeasureUnit>().Any(where);
        }

        public PagedTaskDataInquiryResponse GetMeasureUnits(PagedDataRequest requestInfo, Expression<Func<MeasureUnit, bool>> where = null)
        {
            var query = _dbContext.Set<MeasureUnit>().Where(FilterByActiveTrueAndCompany);
            query = where == null ? query : query.Where(where);
            //var enumerable = query as MeasureUnit[] ?? query.ToArray();

            return FormatResultForPaging(requestInfo, query);
        }

        private static PagedTaskDataInquiryResponse FormatResultForPaging(PagedDataRequest requestInfo, IQueryable<MeasureUnit> query)
        {
            var totalItemCount = query.Count();
            var startIndex = ResultsPagingUtility.CalculateStartIndex(requestInfo.PageNumber, requestInfo.PageSize);
            var mapper = new MeasureUnitToMeasureUnitViewModelMapper();
            var docs = query.OrderBy(x => x.DateCreated).Skip(startIndex).Take(requestInfo.PageSize).Select(s => mapper.Map(s)).ToList();
            var queryResult = new QueryResult<MeasureUnitViewModel>(docs, totalItemCount, requestInfo.PageSize);
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

        public MeasureUnit[] GetMeasureUnits(Expression<Func<MeasureUnit, bool>> where = null)
        {
            var query = _dbContext.Set<MeasureUnit>().Where(FilterByActiveTrueAndCompany);

            query = where == null ? query : query.Where(where);
            var enumerable = query.ToArray();
            return enumerable;
        }

        public MeasureUnit ActivateMeasureUnit(int id)
        {
            var original = GetValidMeasureUnit(id);
            ValidateAuthorization(original);

            original.Active = true;
            _dbContext.Set<MeasureUnit>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        public IQueryable<EntryMethodTypes> GetAllEntryMethodTypes()
        {
            return _dbContext.Set<EntryMethodTypes>().Where(p => p.CompanyId == LoggedInUser.CompanyId && p.Active);
        }

        public MeasureUnit CheckIfDeletedMeasureUnitWithSameCodeExists(string code)
        {
            var mesasureUnit =
                _dbContext.Set<MeasureUnit>()
                    .FirstOrDefault(
                        x => x.CompanyId == LoggedInUser.CompanyId &&
                             x.MeasureCode == code && (x.Active ||
                             x.Active == false));
            return mesasureUnit;
        }

        public MeasureUnit CheckIfDeletedMeasureUnitWithSameNameExists(string name)
        {
            var mesasureUnit =
                _dbContext.Set<MeasureUnit>()
                    .FirstOrDefault(
                        x => x.CompanyId == LoggedInUser.CompanyId &&
                             x.Measure == name && (x.Active ||
                                                       x.Active == false));
            return mesasureUnit;
        }

        public IQueryable<MeasureUnit> SearchMeasureUnits(bool active, string searchText)
        {
            var filteredMeasureUnits = _dbContext.Set<MeasureUnit>().Where(x => x.CompanyId == LoggedInUser.CompanyId);
            if (active)
                filteredMeasureUnits = filteredMeasureUnits.Where(x => x.Active);
            var query = string.IsNullOrEmpty(searchText) || searchText.Contains("null") ? filteredMeasureUnits : filteredMeasureUnits.Where(s
                                                                  => s.MeasureCode.ToUpper().Contains(searchText.ToUpper())
                                                                  || s.Measure.ToUpper().Contains(searchText.ToUpper()));
            return query;
        }
        public bool DeleteRange(List<int?> measureUnitsId)
        {
            var measureUnitList = measureUnitsId.Select(measureUnitId => _dbContext.Set<MeasureUnit>().FirstOrDefault(x => x.Id == measureUnitId)).ToList();
            measureUnitList.ForEach(x => x.Active = false);
            _dbContext.Set<MeasureUnit>().UpdateRange(measureUnitList);
            _dbContext.SaveChanges();
            return true;
        }
    }
}
