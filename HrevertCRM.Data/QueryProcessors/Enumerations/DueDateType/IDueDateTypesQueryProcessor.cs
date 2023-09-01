using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;

namespace HrevertCRM.Data.QueryProcessors.Enumerations
{
    public interface IDueDateTypesQueryProcessor
    {
        DueDateTypes Update(DueDateTypes dueDateTypes);
        DueDateTypes GetValidDueDateTypes(int dueDateTypesId);
        DueDateTypes GetDueDateTypes(int dueDateTypesId);
        void SaveAllDueDateTypes(List<DueDateTypes> dueDateTypes);
        DueDateTypes Save(DueDateTypes dueDateTypes);
        int SaveAll(List<DueDateTypes> dueDateTypes);
        DueDateTypes ActivateDueDateTypes(int id);
        DueDateTypeViewModel GetDueDateTypesViewModel(int id);
        bool Delete(int dueDateTypesId);
        bool Exists(Expression<Func<DueDateTypes, bool>> @where);
        DueDateTypes[] GetDueDateTypes(Expression<Func<DueDateTypes, bool>> @where = null);
        IQueryable<DueDateTypes> GetActiveDueDateTypes();
        IQueryable<DueDateTypes> GetDeletedDueDateTypes();
        IQueryable<DueDateTypes> GetAllDueDateTypes();
    }
}