using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;

namespace HrevertCRM.Data.QueryProcessors.Enumerations
{
    public interface IAccountDetailTypesQueryProcessor
    {
        AccountDetailTypes Update(AccountDetailTypes accountDetailTypes);
        AccountDetailTypes GetValidAccountDetailTypes(int accountDetailTypesId);
        AccountDetailTypes GetAccountDetailTypes(int accountDetailTypesId);
        void SaveAllAccountDetailTypes(List<AccountDetailTypes> accountDetailTypes);
        AccountDetailTypes Save(AccountDetailTypes accountDetailTypes);
        int SaveAll(List<AccountDetailTypes> accountDetailTypes);
        AccountDetailTypes ActivateAccountDetailTypes(int id);
        AccountDetailTypeViewModel GetAccountDetailTypesViewModel(int id);
        bool Delete(int accountDetailTypesId);
        bool Exists(Expression<Func<AccountDetailTypes, bool>> @where);
        AccountDetailTypes[] GetAccountDetailTypes(Expression<Func<AccountDetailTypes, bool>> @where = null);
        IQueryable<AccountDetailTypes> GetActiveAccountDetailTypes();
        IQueryable<AccountDetailTypes> GetDeletedAccountDetailTypes();
        IQueryable<AccountDetailTypes> GetAllAccountDetailTypes();
    }
}