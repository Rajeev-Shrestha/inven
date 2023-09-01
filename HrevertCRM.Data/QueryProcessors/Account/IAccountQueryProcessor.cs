using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface IAccountQueryProcessor
    {
        Account Update(Account account);
        void SaveAllAccount(List<Account> accounts);
        bool Delete(int accountId);
        bool Exists(Expression<Func<Account, bool>> @where);
        Account[] GetAccounts(Expression<Func<Account, bool>> @where = null);
        Account Save(Account account);
        int SaveAll(List<Account> accounts);
        Account ActivateAccount(int id);
        AccountViewModel GetAccountViewModel(int id);
        IEnumerable<AccountNode> GetAccountsInTree();
        IEnumerable<AccountNode> GetAllAccountsInTree();
        IEnumerable<Account> GetChildrenAccounts(int parentAccountId);
        //Account GetParentAccount(int id);
        IEnumerable<AccountNode> SearchActive(string searchText);
        IEnumerable<AccountNode> SearchAll(string searchText);
        IEnumerable<Account> GetAllActiveAccounts();
        Account CheckIfDeletedAccountWithSameDescriptionExists(string accountDescription);
        Account CheckIfDeletedAccountWithSameCodeExists(string accountDescription);
        PagedDataInquiryResponse<AccountViewModel> SearchAccounts(PagedDataRequest requestInfo, Expression<Func<Account, bool>> @where = null);
        bool DeleteOnlyAccount(int id);
    }
}