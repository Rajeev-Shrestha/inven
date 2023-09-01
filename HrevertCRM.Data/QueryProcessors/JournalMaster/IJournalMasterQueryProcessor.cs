using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Hrevert.Common.Enums;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface IJournalMasterQueryProcessor
    {
        JournalMaster Update(JournalMaster journalMaster);
        void SaveAllJournalMaster(List<JournalMaster> journalMasters);
        bool Delete(int journalMasterId);
        bool Exists(Expression<Func<JournalMaster, bool>> @where);
        JournalMaster[] GetJournalMasters(Expression<Func<JournalMaster, bool>> @where = null);
        JournalMaster Save(JournalMaster journalMaster);
        int SaveAll(List<JournalMaster> journalMasters);
        JournalMaster ActivateJournalMaster(int id);
        JournalMasterViewModel GetJournalMasterViewModel(int id);
        PagedDataInquiryResponse<JournalMasterViewModel> SearchJournalMasters(PagedDataRequest requestInfo, Expression<Func<JournalMaster, bool>> @where = null);
        bool DeleteRange(List<int?> journalmastersId);
    }
}