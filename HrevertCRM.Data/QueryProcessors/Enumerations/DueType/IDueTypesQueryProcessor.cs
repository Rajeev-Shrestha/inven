using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;

namespace HrevertCRM.Data.QueryProcessors.Enumerations
{
    public interface IDueTypesQueryProcessor
    {
        DueTypes Update(DueTypes dueTypes);
        DueTypes GetValidDueTypes(int dueTypesId);
        DueTypes GetDueTypes(int dueTypesId);
        void SaveAllDueTypes(List<DueTypes> dueTypes);
        DueTypes Save(DueTypes dueTypes);
        int SaveAll(List<DueTypes> dueTypes);
        DueTypes ActivateDueTypes(int id);
        DueTypeViewModel GetDueTypesViewModel(int id);
        bool Delete(int dueTypesId);
        bool Exists(Expression<Func<DueTypes, bool>> @where);
        DueTypes[] GetDueTypes(Expression<Func<DueTypes, bool>> @where = null);
        IQueryable<DueTypes> GetActiveDueTypes();
        IQueryable<DueTypes> GetDeletedDueTypes();
        IQueryable<DueTypes> GetAllDueTypes();
    }
}