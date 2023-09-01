using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Hrevert.Common.Constants.JournalMaster;
using Hrevert.Common.Security;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.Mapper;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using PagedTaskDataInquiryResponse = HrevertCRM.Data.Common.PagedDataInquiryResponse<HrevertCRM.Data.ViewModels.JournalMasterViewModel>;

namespace HrevertCRM.Data.QueryProcessors
{
    public class JournalMasterQueryProcessor : QueryBase<JournalMaster>, IJournalMasterQueryProcessor
    {
        public JournalMasterQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
        {
        }
        public JournalMaster Update(JournalMaster journalMaster)
        {
            var original = GetValidJournalMaster(journalMaster.Id);
            ValidateAuthorization(journalMaster);
            CheckVersionMismatch(journalMaster, original);
        
            original.JournalType = journalMaster.JournalType;
            original.Closed = journalMaster.Closed;
            original.Description = journalMaster.Description;
            original.Credit = journalMaster.Credit;
            original.Debit = journalMaster.Debit;
            original.FiscalPeriodId = journalMaster.FiscalPeriodId;
            original.Posted = journalMaster.Posted;
            original.Note = journalMaster.Note;
            original.PostedDate = journalMaster.PostedDate;
            original.Printed = journalMaster.Printed;
            original.Cancelled = journalMaster.Cancelled;
            original.IsSystem = journalMaster.IsSystem;
            original.Active = journalMaster.Active;
            original.WebActive = journalMaster.WebActive;
            original.CompanyId = journalMaster.CompanyId;

            _dbContext.Set<JournalMaster>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
        public virtual JournalMaster GetValidJournalMaster(int journalMasterId)
        {
            var journalMaster = _dbContext.Set<JournalMaster>().FirstOrDefault(sc => sc.Id == journalMasterId);
            if (journalMaster == null)
            {
                throw new RootObjectNotFoundException(JournalMasterConstants.JournalMasterQueryProcessorConstants.JournalNotFound);
            }
            return journalMaster;
        }
        public JournalMaster GetJournalMaster(int journalMasterId)
        {
            var journalMaster = _dbContext.Set<JournalMaster>().FirstOrDefault(d => d.Id == journalMasterId);
            return journalMaster;
        }
        public void SaveAllJournalMaster(List<JournalMaster> journalMasters)
        {
            _dbContext.Set<JournalMaster>().AddRange(journalMasters);
            _dbContext.SaveChanges();
        }
        public JournalMaster Save(JournalMaster journalMaster)
        {
            journalMaster.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<JournalMaster>().Add(journalMaster);
            _dbContext.SaveChanges();
            return journalMaster;
        }
        public int SaveAll(List<JournalMaster> journalMasters)
        {
            _dbContext.Set<JournalMaster>().AddRange(journalMasters);
            return _dbContext.SaveChanges();
        }
        public JournalMaster ActivateJournalMaster(int id)
        {
            var original = GetValidJournalMaster(id);
            ValidateAuthorization(original);

            original.Active = true;
            _dbContext.Set<JournalMaster>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
        public JournalMasterViewModel GetJournalMasterViewModel(int id)
        {
            var journalMaster = _dbContext.Set<JournalMaster>().Single(s => s.Id == id);
            var mapper = new JournalMasterToJournalMasterViewModelMapper();
            return mapper.Map(journalMaster);
        }

        public PagedTaskDataInquiryResponse SearchJournalMasters(PagedDataRequest requestInfo, Expression<Func<JournalMaster, bool>> @where = null)
        {
            var query = _dbContext.Set<JournalMaster>().Where(c => c.CompanyId == LoggedInUser.CompanyId);
            if (requestInfo.Active)
                query = query.Where(x => x.Active);
            var result = string.IsNullOrEmpty(requestInfo.SearchText) ? query : query.Where(s
                    => s.Description.ToUpper().Contains(requestInfo.SearchText.ToUpper()) ||
                     s.Note.ToUpper().Contains(requestInfo.SearchText.ToUpper()));
            return FormatResultForPaging(requestInfo, result);
        }

        public bool Delete(int journalMasterId)
        {
            var doc = GetJournalMaster(journalMasterId);
            ValidateAuthorization(doc);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<JournalMaster>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }
        public bool Exists(Expression<Func<JournalMaster, bool>> @where)
        {
            return _dbContext.Set<JournalMaster>().Any(@where);
        }
        public JournalMaster[] GetJournalMasters(Expression<Func<JournalMaster, bool>> @where = null)
        {

            var query = _dbContext.Set<JournalMaster>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            var enumerable = query.ToArray();
            return enumerable;
        }

        private static PagedTaskDataInquiryResponse FormatResultForPaging(PagedDataRequest requestInfo, IQueryable<JournalMaster> query)
        {
            var totalItemCount = query.Count();

            var startIndex = ResultsPagingUtility.CalculateStartIndex(requestInfo.PageNumber, requestInfo.PageSize);
            var mapper = new JournalMasterToJournalMasterViewModelMapper();
            var docs = query.OrderBy(x => x.DateCreated).Skip(startIndex).Take(requestInfo.PageSize).Select(
                s => mapper.Map(s)).ToList();

            var queryResult = new QueryResult<JournalMasterViewModel>(docs, totalItemCount, requestInfo.PageSize);
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

        public bool DeleteRange(List<int?> journalmastersId)
        {
            var journalMasterList = journalmastersId.Select(journalmasterId => _dbContext.Set<JournalMaster>().FirstOrDefault(x => x.Id == journalmasterId && x.CompanyId == LoggedInUser.CompanyId)).ToList();
            journalMasterList.ForEach(x => x.Active = false);
            _dbContext.Set<JournalMaster>().UpdateRange(journalMasterList);
            _dbContext.SaveChanges();
            return true;
        }
    }
}
