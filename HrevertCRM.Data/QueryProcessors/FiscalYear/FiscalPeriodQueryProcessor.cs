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
using PagedTaskDataInquiryResponse = HrevertCRM.Data.Common.PagedDataInquiryResponse<HrevertCRM.Data.ViewModels.FiscalPeriodViewModel>;

namespace HrevertCRM.Data.QueryProcessors
{
    public class FiscalPeriodQueryProcessor : QueryBase<FiscalPeriod>, IFiscalPeriodQueryProcessor
    {
        public FiscalPeriodQueryProcessor(IUserSession userSession, IDbContext dbContext)
            : base(userSession, dbContext)
        {
        }

        public FiscalPeriod Update(FiscalPeriod fiscalPeriod)
        {
            var original = GetValidFiscalPeriod(fiscalPeriod.Id);
            ValidateAuthorization(fiscalPeriod);
            CheckVersionMismatch(fiscalPeriod, original);

            original.Name = fiscalPeriod.Name;
            original.StartDate = fiscalPeriod.StartDate;
            original.EndDate = fiscalPeriod.EndDate;
            original.Active = fiscalPeriod.Active;
            original.CompanyId = LoggedInUser.CompanyId;
           
            _dbContext.Set<FiscalPeriod>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        public virtual FiscalPeriod GetValidFiscalPeriod(int fiscalPeriodId)
        {
            var fiscalPeriod = _dbContext.Set<FiscalPeriod>().FirstOrDefault(sc => sc.Id == fiscalPeriodId);
            if (fiscalPeriod == null)
            {
                throw new RootObjectNotFoundException(FiscalYearConstants.FiscalPeriodQueryProcessorConstants.FiscalPeriodNotFound);
            }
            return fiscalPeriod;
        }

        public FiscalPeriod GetFiscalPeriod(int fiscalPeriodId)
        {
            var fiscalPeriod = _dbContext.Set<FiscalPeriod>().FirstOrDefault(d => d.Id == fiscalPeriodId);
            return fiscalPeriod;
        }

        public void SaveAll(List<FiscalPeriod> fiscalPeriod)
        {
            _dbContext.Set<FiscalPeriod>().AddRange(fiscalPeriod);
            _dbContext.SaveChanges();
        }
        public FiscalPeriod Save(FiscalPeriod fiscalPeriod)
        {
            fiscalPeriod.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<FiscalPeriod>().Add(fiscalPeriod);
            _dbContext.SaveChanges();
            return fiscalPeriod;
        }

        public bool Delete(int fiscalPeriodId)
        {
            var doc = GetFiscalPeriod(fiscalPeriodId);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<FiscalPeriod>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }

        public bool Exists(Expression<Func<FiscalPeriod, bool>> @where)
        {
            return _dbContext.Set<FiscalPeriod>().Any(@where);
        }

        public PagedTaskDataInquiryResponse GetFiscalPeriods(PagedDataRequest requestInfo, Expression<Func<FiscalPeriod, bool>> @where = null)
        {
            var query = _dbContext.Set<FiscalPeriod>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            //var enumerable = query as FiscalYear[] ?? query.ToArray();
            var totalItemCount = query.Count();
            var startIndex = ResultsPagingUtility.CalculateStartIndex(requestInfo.PageNumber, requestInfo.PageSize);
            var mapper = new FiscalPeriodToFiscalPeriodViewModelMapper();
            var docs = query.OrderBy(x => x.DateCreated).Skip(startIndex).Take(requestInfo.PageSize).Select(s => mapper.Map(s)).ToList();
            var queryResult = new QueryResult<FiscalPeriodViewModel>(docs, totalItemCount, requestInfo.PageSize);

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

        public FiscalPeriod[] GetFiscalPeriods(Expression<Func<FiscalPeriod, bool>> @where = null)
        {
            var query = _dbContext.Set<FiscalPeriod>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);
            var enumerable = query.ToArray();
            return enumerable;
        }

        public List<FiscalPeriod> GetFiscalPeriodDates(int fiscalYearId)
        {
            return _dbContext.Set<FiscalPeriod>().Where(p => p.FiscalYearId == fiscalYearId).Select(p => p).ToList();
        }

        public FiscalYear GetFiscalYearDates(int fiscalYearId)
        {
            return _dbContext.Set<FiscalYear>().Single(p => p.Id == fiscalYearId);
        }

        public List<FiscalPeriod> GetActiveFiscalPeriodsByFiscalYear(int fiscalYearId)
        {
            return _dbContext.Set<FiscalPeriod>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.FiscalYearId == fiscalYearId).ToList();   // to get all the fiscal period of that year active and inactive
        }
        public List<FiscalPeriod> GetDeletedFiscalPeriodsByFiscalYear(int fiscalYearId)
        {
            return _dbContext.Set<FiscalPeriod>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.FiscalYearId == fiscalYearId && f.Active == false).ToList();
        }
        public List<FiscalPeriod> GetAllFiscalPeriodsByFiscalYear(int fiscalYearId)
        {
            return _dbContext.Set<FiscalPeriod>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.FiscalYearId == fiscalYearId).ToList();
        }

        public int GetFiscalPeriodIdByCurrentDate()
        {
            var currentDate = DateTime.Now;
            var fiscalPeriodId = 0;
            var fiscalPeriods = GetFiscalPeriods(true);

            foreach (var fiscalPeriod in fiscalPeriods)
            {
                if (currentDate.Ticks > fiscalPeriod.StartDate.Ticks && currentDate.Ticks < fiscalPeriod.EndDate.Ticks)
                {
                    fiscalPeriodId = fiscalPeriod.Id;
                }
            }
            return fiscalPeriodId;
        }

        public FiscalPeriod ActivateFiscalPeriod(int fiscalPeriodId)
        {
            var original = GetValidFiscalPeriod(fiscalPeriodId);
            ValidateAuthorization(original);
            original.Active = true;
            _dbContext.Set<FiscalPeriod>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }


        private IQueryable<FiscalYear> GetActiveFiscalYears()
        {
            return _dbContext.Set<FiscalYear>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active);
        }
        public FiscalYear GetFiscalYearIdByFiscalPeriods(DateTime startDate, DateTime endDate)
        {
            var result = GetActiveFiscalYears();
            var fiscalyear = result.Where(p => p.StartDate.Year == startDate.Year && p.StartDate.Year == endDate.Year ); 
            return fiscalyear.SingleOrDefault();
        }

        public IQueryable<FiscalPeriod> GetFiscalPeriods(bool active)
        {
            var fiscalPeriods =  _dbContext.Set<FiscalPeriod>().Where(f => f.CompanyId == LoggedInUser.CompanyId);
            if (active)
                fiscalPeriods = fiscalPeriods.Where(x => x.Active);
            return fiscalPeriods;
        }
    }
}
