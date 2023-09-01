using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;

namespace HrevertCRM.Data.QueryProcessors.Enumerations
{
    public interface ILockTypesQueryProcessor
    {
        LockTypes Update(LockTypes lockTypes);
        LockTypes GetValidLockTypes(int lockTypesId);
        LockTypes GetLockTypes(int lockTypesId);
        void SaveAllLockTypes(List<LockTypes> lockTypes);
        LockTypes Save(LockTypes lockTypes);
        int SaveAll(List<LockTypes> lockTypes);
        LockTypes ActivateLockTypes(int id);
        LockTypeViewModel GetLockTypesViewModel(int id);
        bool Delete(int lockTypesId);
        bool Exists(Expression<Func<LockTypes, bool>> @where);
        LockTypes[] GetLockTypes(Expression<Func<LockTypes, bool>> @where = null);
        IQueryable<LockTypes> GetActiveLockTypes();
        IQueryable<LockTypes> GetDeletedLockTypes();
        IQueryable<LockTypes> GetAllLockTypes();
    }
}