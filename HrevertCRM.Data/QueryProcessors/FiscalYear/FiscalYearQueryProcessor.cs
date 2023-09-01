using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Hrevert.Common.Constants.FiscalYear;
using Hrevert.Common.Security;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using HrevertCRM.Data.Mapper;
using PagedTaskDataInquiryResponse = HrevertCRM.Data.Common.PagedDataInquiryResponse<HrevertCRM.Data.ViewModels.FiscalYearViewModel>;

namespace HrevertCRM.Data.QueryProcessors
{
    public class FiscalYearQueryProcessor : QueryBase<FiscalYear>, IFiscalYearQueryProcessor
    {
        public FiscalYearQueryProcessor(IUserSession userSession, IDbContext dbContext) 
            : base(userSession, dbContext)
        {
        }

        public FiscalYear Update(FiscalYear fiscalYear)
        {
            var original = GetValidFiscalYear(fiscalYear.Id);
            ValidateAuthorization(fiscalYear);
            CheckVersionMismatch(fiscalYear, original);

            original.Name = fiscalYear.Name;
            original.StartDate = fiscalYear.StartDate;
            original.EndDate = fiscalYear.EndDate;
            original.CompanyId = LoggedInUser.CompanyId;
            original.Active = fiscalYear.Active;
            original.WebActive = fiscalYear.WebActive;

            _dbContext.Set<FiscalYear>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        public virtual FiscalYear GetValidFiscalYear(int fiscalYearId)
        {
            var fiscalYear = _dbContext.Set<FiscalYear>().FirstOrDefault(sc => sc.Id == fiscalYearId);
            if (fiscalYear == null)
            {
                throw new RootObjectNotFoundException(FiscalYearConstants.FiscalYearQueryProcessorConstants.FiscalYearNotFound);
            }
            return fiscalYear;
        }

        public FiscalYear GetFiscalYear(int fiscalYearId)
        {
            var fiscalYear = _dbContext.Set<FiscalYear>().FirstOrDefault(d => d.Id == fiscalYearId);
           return fiscalYear;
        }

        public void SaveAll(List<FiscalYear> fiscalYear)
        {
            _dbContext.Set<FiscalYear>().AddRange(fiscalYear);
            _dbContext.SaveChanges();
        }
        public FiscalYear Save(FiscalYear fiscalYear)
        {
            fiscalYear.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<FiscalYear>().AddRange(fiscalYear);
            _dbContext.SaveChanges();
            return fiscalYear;
        }

        public bool Delete(int fiscalYearId)
        {
            int result;
            var doc = GetFiscalYear(fiscalYearId);

            var fiscalPeriods = _dbContext.Set<FiscalPeriod>().Where(p => p.FiscalYearId == fiscalYearId);
            if (fiscalPeriods.ToList().Count > 0)
            {
                foreach (var fiscalPeriod in fiscalPeriods)
                {
                    fiscalPeriod.Active = false;
                    _dbContext.Set<FiscalPeriod>().Update(fiscalPeriod);
                   // _dbContext.SaveChanges();
                }
            }
            if (doc != null)
            {
                doc.Active = false;
                _dbContext.Set<FiscalYear>().Update(doc);
            }
            result = _dbContext.SaveChanges();
            return result > 0;
        }

        public bool Exists(Expression<Func<FiscalYear, bool>> @where)
        {
            var isExists = _dbContext.Set<FiscalYear>().Where(f => f.CompanyId == LoggedInUser.CompanyId).Any(@where);
            return isExists;

        }

        public PagedTaskDataInquiryResponse GetFiscalYears(PagedDataRequest requestInfo, Expression<Func<FiscalYear, bool>> @where = null)
        {
            var query = _dbContext.Set<FiscalYear>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            //var enumerable = query as FiscalYear[] ?? query.ToArray();
            var totalItemCount = query.Count();
            var startIndex = ResultsPagingUtility.CalculateStartIndex(requestInfo.PageNumber, requestInfo.PageSize);
            var mapper = new FiscalYearToFiscalYearViewModelMapper();
            var docs = query.OrderBy(x => x.DateCreated).Skip(startIndex).Take(requestInfo.PageSize).Select(s => mapper.Map(s)).ToList();
            var queryResult = new QueryResult<FiscalYearViewModel>(docs, totalItemCount, requestInfo.PageSize);

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

        public FiscalYear[] GetFiscalYears(Expression<Func<FiscalYear, bool>> @where = null)
        {
            var query = _dbContext.Set<FiscalYear>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);
            var enumerable = query.ToArray();
            return enumerable;
        }

        public List<FiscalYear> GetFiscalYearDates()
        {
            return _dbContext.Set<FiscalYear>().Where(p => p.CompanyId == LoggedInUser.CompanyId).Select(p => p).ToList();
        }


        public FiscalYear ActivateFiscalYear(int id)
        {
            var original = GetValidFiscalYear(id);
            ValidateAuthorization(original);
            original.Active = true;
            _dbContext.Set<FiscalYear>().Update(original);

            var fiscalPeriods = _dbContext.Set<FiscalPeriod>().Where(p => p.FiscalYearId == id);
            if (fiscalPeriods.ToList().Count > 0)
            {
                foreach (var fiscalPeriod in fiscalPeriods)
                {
                    fiscalPeriod.Active = true;
                    _dbContext.Set<FiscalPeriod>().Update(fiscalPeriod);
                }
            }

            _dbContext.SaveChanges();
            return original;
        }

        public List<int> GetExistingFiscalPeriodsOfFiscalYear(int? id)
        {
            var existingFiscalPeriods = _dbContext.Set<FiscalPeriod>()
                .Where(x => x.FiscalYearId == id)
                .Select(x => x.Id).ToList();
            return existingFiscalPeriods;
        }

        public IQueryable<FiscalYear> SearchFiscalYears(bool active, string searchText)
        {
            var fiscalYears = _dbContext.Set<FiscalYear>().Where(f => f.CompanyId == LoggedInUser.CompanyId);
            if (active)
                fiscalYears = fiscalYears.Where(x => x.Active);
            var query = string.IsNullOrEmpty(searchText) || searchText.Contains("null")
                ? fiscalYears
                : fiscalYears.Where(s => s.Name.ToUpper().Contains(searchText.ToUpper()));
            return query;
        }

        public FiscalYear GetFiscalYearByCurrentDate()
        {
            var currentDate = DateTime.Now;
            var fiscalYears = SearchFiscalYears(true, null);

            foreach (var fiscalYear in fiscalYears)
            {
                if (currentDate.Ticks > fiscalYear.StartDate.Ticks && currentDate.Ticks < fiscalYear.EndDate.Ticks)
                {
                    return fiscalYear;
                }
            }
            return null;
        }

        public bool DeleteRange(List<int?> fiscalYearsId)
        {
            var fiscalYearList = fiscalYearsId.Select(fiscalYearId => _dbContext.Set<FiscalYear>().FirstOrDefault(x => x.Id == fiscalYearId && x.CompanyId == LoggedInUser.CompanyId)).ToList();
            fiscalYearList.ForEach(x => x.Active = false);
            _dbContext.Set<FiscalYear>().UpdateRange(fiscalYearList);
            _dbContext.SaveChanges();
            return true;
        }
    }
}
