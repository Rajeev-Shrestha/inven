using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;

namespace HrevertCRM.Data.QueryProcessors.Enumerations
{
    public interface IAccountTypesQueryProcessor
    {
        AccountTypes Update(AccountTypes accountTypes);
        AccountTypes GetValidAccountTypes(int accountTypesId);
        AccountTypes GetAccountTypes(int accountTypesId);
        void SaveAllAccountTypes(List<AccountTypes> accountTypes);
        AccountTypes Save(AccountTypes accountTypes);
        int SaveAll(List<AccountTypes> accountTypes);
        AccountTypes ActivateAccountTypes(int id);
        AccountTypeViewModel GetAccountTypesViewModel(int id);
        bool Delete(int accountTypesId);
        bool Exists(Expression<Func<AccountTypes, bool>> @where);
        AccountTypes[] GetAccountTypes(Expression<Func<AccountTypes, bool>> @where = null);
        IQueryable<AccountTypes> GetActiveAccountTypes();
        IQueryable<AccountTypes> GetDeletedAccountTypes();
        IQueryable<AccountTypes> GetAllAccountTypes();
    }
}