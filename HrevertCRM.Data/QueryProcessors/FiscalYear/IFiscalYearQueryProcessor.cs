using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface IFiscalYearQueryProcessor
    {
        FiscalYear GetFiscalYearByCurrentDate();
        FiscalYear Update(FiscalYear fiscalYear);
        FiscalYear GetFiscalYear(int fiscalYearId);
        FiscalYear Save(FiscalYear fiscalYear);
        void SaveAll(List<FiscalYear> fiscalYears);
        bool Delete(int fiscalYearId);
        bool Exists(Expression<Func<FiscalYear, bool>> @where);
        
        List<FiscalYear> GetFiscalYearDates();
        FiscalYear[] GetFiscalYears(Expression<Func<FiscalYear, bool>> @where = null);
        PagedDataInquiryResponse<FiscalYearViewModel> GetFiscalYears(PagedDataRequest requestInfo,
            Expression<Func<FiscalYear, bool>> @where = null);   
        FiscalYear ActivateFiscalYear(int id);
        List<int> GetExistingFiscalPeriodsOfFiscalYear(int? id);
        IQueryable<FiscalYear> SearchFiscalYears(bool active, string searchText);
        bool DeleteRange(List<int?> fiscalYearsId);
    }
}
