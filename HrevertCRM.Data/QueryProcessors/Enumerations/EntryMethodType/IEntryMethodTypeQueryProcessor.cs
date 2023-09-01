using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace HrevertCRM.Data.QueryProcessors.Enumerations
{
    public interface IEntryMethodTypeQueryProcessor
    {
        EntryMethodTypes Update(EntryMethodTypes entryMethodTypes);
        EntryMethodTypes GetValidEntryMethodTypes(int entryMethodTypesId);
        EntryMethodTypes GetEntryMethodTypes(int entryMethodTypesId);
        void SaveAllEntryMethodTypes(List<EntryMethodTypes> entryMethodTypes);
        EntryMethodTypes Save(EntryMethodTypes entryMethodTypes);
        int SaveAll(List<EntryMethodTypes> entryMethodTypes);
        EntryMethodTypes ActivateEntryMethodTypes(int id);
        EntryMethodTypeViewModel GetEntryMethodTypesViewModel(int id);
        bool Delete(int entryMethodTypesId);
        bool Exists(Expression<Func<EntryMethodTypes, bool>> @where);
        EntryMethodTypes[] GetEntryMethodTypes(Expression<Func<EntryMethodTypes, bool>> @where = null);
        IQueryable<EntryMethodTypes> GetActiveEntryMethodTypes();
        IQueryable<EntryMethodTypes> GetDeletedEntryMethodTypes();
        IQueryable<EntryMethodTypes> GetAllEntryMethodTypes();
    }
}
