using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;

namespace HrevertCRM.Data.QueryProcessors.Enumerations
{
    public interface IJournalTypesQueryProcessor
    {
        JournalTypes Update(JournalTypes journalTypes);
        JournalTypes GetValidJournalTypes(int journalTypesId);
        JournalTypes GetJournalTypes(int journalTypesId);
        void SaveAllJournalTypes(List<JournalTypes> journalTypes);
        JournalTypes Save(JournalTypes journalTypes);
        int SaveAll(List<JournalTypes> journalTypes);
        JournalTypes ActivateJournalTypes(int id);
        JournalTypeViewModel GetJournalTypesViewModel(int id);
        bool Delete(int journalTypesId);
        bool Exists(Expression<Func<JournalTypes, bool>> @where);
        JournalTypes[] GetJournalTypes(Expression<Func<JournalTypes, bool>> @where = null);
        IQueryable<JournalTypes> GetActiveJournalTypes();
        IQueryable<JournalTypes> GetDeletedJournalTypes();
        IQueryable<JournalTypes> GetAllJournalTypes();
    }
}