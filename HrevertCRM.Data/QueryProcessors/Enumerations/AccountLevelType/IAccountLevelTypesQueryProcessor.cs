using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;

namespace HrevertCRM.Data.QueryProcessors.Enumerations
{
    public interface IAccountLevelTypesQueryProcessor
    {
        AccountLevelTypes Update(AccountLevelTypes accountLevelTypes);
        AccountLevelTypes GetValidAccountLevelTypes(int accountLevelTypesId);
        AccountLevelTypes GetAccountLevelTypes(int accountLevelTypesId);
        void SaveAllAccountLevelTypes(List<AccountLevelTypes> accountLevelTypes);
        AccountLevelTypes Save(AccountLevelTypes accountLevelTypes);
        int SaveAll(List<AccountLevelTypes> accountLevelTypes);
        AccountLevelTypes ActivateAccountLevelTypes(int id);
        AccountLevelTypeViewModel GetAccountLevelTypesViewModel(int id);
        bool Delete(int accountLevelTypesId);
        bool Exists(Expression<Func<AccountLevelTypes, bool>> @where);
        AccountLevelTypes[] GetAccountLevelTypes(Expression<Func<AccountLevelTypes, bool>> @where = null);
        IQueryable<AccountLevelTypes> GetActiveAccountLevelTypes();
        IQueryable<AccountLevelTypes> GetDeletedAccountLevelTypes();
        IQueryable<AccountLevelTypes> GetAllAccountLevelTypes();
    }
}