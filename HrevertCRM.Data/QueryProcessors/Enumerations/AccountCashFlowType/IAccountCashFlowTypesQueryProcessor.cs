using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;

namespace HrevertCRM.Data.QueryProcessors.Enumerations
{
    public interface IAccountCashFlowTypesQueryProcessor
    {
        AccountCashFlowTypes Update(AccountCashFlowTypes accountCashFlowTypes);
        AccountCashFlowTypes GetValidAccountCashFlowTypes(int accountCashFlowTypesId);
        AccountCashFlowTypes GetAccountCashFlowTypes(int accountCashFlowTypesId);
        void SaveAllAccountCashFlowTypes(List<AccountCashFlowTypes> accountCashFlowTypes);
        AccountCashFlowTypes Save(AccountCashFlowTypes accountCashFlowTypes);
        int SaveAll(List<AccountCashFlowTypes> accountCashFlowTypes);
        AccountCashFlowTypes ActivateAccountCashFlowTypes(int id);
        AccountCashFlowTypeViewModel GetAccountCashFlowTypesViewModel(int id);
        bool Delete(int accountCashFlowTypesId);
        bool Exists(Expression<Func<AccountCashFlowTypes, bool>> @where);
        AccountCashFlowTypes[] GetAccountCashFlowTypes(Expression<Func<AccountCashFlowTypes, bool>> @where = null);
        IQueryable<AccountCashFlowTypes> GetActiveAccountCashFlowTypes();
        IQueryable<AccountCashFlowTypes> GetDeletedAccountCashFlowTypes();
        IQueryable<AccountCashFlowTypes> GetAllAccountCashFlowTypes();
    }
}