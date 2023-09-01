using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface IFiscalPeriodQueryProcessor
    {
        FiscalPeriod Update(FiscalPeriod fiscalPeriod);
        FiscalPeriod GetFiscalPeriod(int fiscalPeriodId);
        FiscalPeriod Save(FiscalPeriod fiscalPeriod);
        PagedDataInquiryResponse<FiscalPeriodViewModel> GetFiscalPeriods(PagedDataRequest requestInfo,
            Expression<Func<FiscalPeriod, bool>> @where = null);
        void SaveAll(List<FiscalPeriod> fiscalPeriods);
        bool Delete(int fiscalPeriodId);
        bool Exists(Expression<Func<FiscalPeriod, bool>> @where);
        List<FiscalPeriod> GetFiscalPeriodDates(int fiscalPeriodId);
        FiscalYear GetFiscalYearDates(int fiscalYearId);
        List<FiscalPeriod> GetActiveFiscalPeriodsByFiscalYear(int fiscalYearId);
        List<FiscalPeriod> GetDeletedFiscalPeriodsByFiscalYear(int fiscalYearId);
        List<FiscalPeriod> GetAllFiscalPeriodsByFiscalYear(int fiscalYearId);
        int GetFiscalPeriodIdByCurrentDate();
        FiscalPeriod ActivateFiscalPeriod(int fiscalPeriodId);
        FiscalYear GetFiscalYearIdByFiscalPeriods(DateTime startDate, DateTime endDate);
        IQueryable<FiscalPeriod> GetFiscalPeriods(bool active);
    }
}
