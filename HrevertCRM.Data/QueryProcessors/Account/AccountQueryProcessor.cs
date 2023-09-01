using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Hrevert.Common.Constants.Account;
using Hrevert.Common.Security;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.Mapper;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using PagedTaskDataInquiryResponse = HrevertCRM.Data.Common.PagedDataInquiryResponse<HrevertCRM.Data.ViewModels.AccountViewModel>;

namespace HrevertCRM.Data.QueryProcessors
{
    public class AccountQueryProcessor : QueryBase<Account>, IAccountQueryProcessor
    {
        public AccountQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
        {
        }
        public Account Update(Account account)
        {
            var original = GetValidAccount(account.Id);
            ValidateAuthorization(account);
            CheckVersionMismatch(account, original);       

            original.AccountCode = account.AccountCode;
            original.AccountDescription = account.AccountDescription;
            original.AccountType = account.AccountType;
            original.ParentAccountId = account.ParentAccountId;
            original.CurrentBalance = account.CurrentBalance;
            original.BankAccount = account.BankAccount;
            original.MainAccount = account.MainAccount;
            original.Level = account.Level;
            original.Active = account.Active;
            original.WebActive = account.WebActive;
            original.CompanyId = account.CompanyId;
            original.AccountCashFlowType = account.AccountCashFlowType;

            _dbContext.Set<Account>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        public virtual Account GetValidAccount(int accountId)
        {
            var account = _dbContext.Set<Account>().FirstOrDefault(sc => sc.Id == accountId);
            if (account == null) throw new RootObjectNotFoundException(AccountConstants.AccountQueryProcessorConstants.AccountNotFound);
            return account;
        }

        public Account GetAccount(int accountId)
        {
            var account = _dbContext.Set<Account>().FirstOrDefault(d => d.Id == accountId);
            return account;
        }
        public void SaveAllAccount(List<Account> accounts)
        {
            _dbContext.Set<Account>().AddRange(accounts);
            _dbContext.SaveChanges();
        }

        public Account Save(Account account)
        {
             account.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<Account>().Add(account);
            _dbContext.SaveChanges();
            return account;
        }

        public int SaveAll(List<Account> accounts)
        {
            _dbContext.Set<Account>().AddRange(accounts);
            return _dbContext.SaveChanges();
        }

        public Account ActivateAccount(int id)
        {
            var original = GetValidAccount(id);
            ValidateAuthorization(original);

            original.Active = true;
            var childAccount = _dbContext.Set<Account>().Where(p => p.ParentAccountId == id).Where(p => p.CompanyId == LoggedInUser.CompanyId);
            if (childAccount.ToList().Count > 0)
            {
                foreach (var item in childAccount)
                {
                    item.Active = true;
                    var childParentId = item.Id;
                    var childOfChildAccount = _dbContext.Set<Account>().Where(p => p.ParentAccountId == childParentId).Where(p => p.CompanyId == LoggedInUser.CompanyId);
                    if (childOfChildAccount.ToList().Count > 0)
                    {
                        foreach (var c in childOfChildAccount)
                        {
                            c.Active = true;
                            _dbContext.Set<Account>().Update(c);
                        }
                    }
                    _dbContext.Set<Account>().Update(item);
                }
            }
            _dbContext.Set<Account>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        public AccountViewModel GetAccountViewModel(int id)
        {
            var account = _dbContext.Set<Account>().Single(s => s.Id == id);
            var mapper = new AccountToAccountViewModelMapper();
            return mapper.Map(account);
        }

        public IEnumerable<AccountNode> GetAccountsInTree()
        {
            var lookup = new Dictionary<int?, AccountNode>();
            var accounts = _dbContext.Set<Account>().Where(FilterByActiveTrueAndCompany).Where(p => p.Level == Hrevert.Common.Enums.AccountLevel.General && p.CompanyId==LoggedInUser.CompanyId).ToList();
            accounts.ForEach(x => lookup.Add(x.Id, new AccountNode {Source = x}));

            return GetTree(lookup);
        }

        public IEnumerable<AccountNode> GetAllAccountsInTree()
        {
            var lookup = new Dictionary<int?, AccountNode>();
            var accounts = _dbContext.Set<Account>().Where(p=>p.Level == Hrevert.Common.Enums.AccountLevel.General && p.CompanyId==LoggedInUser.CompanyId).ToList();
            accounts.ForEach(x => lookup.Add(x.Id, new AccountNode { Source = x }));

            return GetTree(lookup);
        }

        public IEnumerable<Account> GetChildrenAccounts(int parentAccountId)
        {
            var childrenAccounts = _dbContext.Set<Account>().Where(FilterByActiveTrueAndCompany).ToList();
            childrenAccounts = childrenAccounts.Where(x => x.ParentAccountId == parentAccountId).ToList();
            return childrenAccounts;
        }


        public IEnumerable<AccountNode> SearchActive(string searchText)
        {
            var lookup = new Dictionary<int?, AccountNode>();
            var filteredAccounts = _dbContext.Set<Account>().Where(FilterByActiveTrueAndCompany).ToList();
            var accounts = string.IsNullOrEmpty(searchText) ? filteredAccounts : filteredAccounts.Where(s
                                                                  => s.AccountDescription.ToUpper().Contains(searchText.ToUpper())).ToList();
            accounts.ForEach(x => lookup.Add(x.Id, new AccountNode { Source = x }));
            return GetTree(lookup);
        }

        private static IEnumerable<AccountNode> GetTree(Dictionary<int?, AccountNode> lookup)
        {
            foreach (var item in lookup.Values)
            {
                if (item.Source.ParentAccountId == null) continue;
                AccountNode proposedParent;
                if (!lookup.TryGetValue(item.Source.ParentAccountId, out proposedParent)) continue;
                item.Parent = proposedParent;
                proposedParent.Children.Add(item);
            }
            var values = lookup.Values.Where(x => x.Parent == null);
            return values;
        }

        public IEnumerable<AccountNode> SearchAll(string searchText)
        {
            var lookup = new Dictionary<int?, AccountNode>();
            var filteredAccounts = _dbContext.Set<Account>().Where(x => x.CompanyId == LoggedInUser.CompanyId).ToList();
            var accounts = string.IsNullOrEmpty(searchText) ? filteredAccounts : filteredAccounts.Where(s
                                                                  => s.AccountDescription.ToUpper().Contains(searchText.ToUpper())).ToList();
            accounts.ForEach(x => lookup.Add(x.Id, new AccountNode { Source = x }));

            return GetTree(lookup);
        }
        //public bool Delete(int id)
        //{
        //    var doc = GetAccount(id);
        //    var result = 0;
        //    if (doc == null) return result > 0;
        //    DeleteAccountTree(doc);
        //    doc.Active = false;
        //    _dbContext.Set<Account>().Update(doc);
        //    result = _dbContext.SaveChanges();
        //    return result > 0;
        //}
        //private void DeleteAccountTree(Account accountTreeViewModel)
        //{
        //    accountTreeViewModel.Active = false;
        //    //find children product
        //    var child = _dbContext.Set<Account>().Where(x => x.ParentAccountId == accountTreeViewModel.Id).ToList();
        //    foreach (var categoryNode in child)
        //    {
        //        DeleteAccountTree(categoryNode);
        //    }
        //    _dbContext.SaveChanges();
        //}

        public bool Delete(int accountId)
        {
            var doc = GetAccount(accountId);
            ValidateAuthorization(doc);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            var childAccount = _dbContext.Set<Account>().Where(p => p.ParentAccountId == accountId).Where(p => p.CompanyId == LoggedInUser.CompanyId);
            if (childAccount.ToList().Count > 0)
            {
                foreach (var item in childAccount)
                {
                    item.Active = false;
                    var childParentId = item.Id;
                    var childOfChildAccount = _dbContext.Set<Account>().Where(p => p.ParentAccountId == childParentId).Where(p => p.CompanyId == LoggedInUser.CompanyId);
                    if (childOfChildAccount.ToList().Count > 0)
                    {
                        foreach (var c in childOfChildAccount)
                        {
                            c.Active = false;
                            _dbContext.Set<Account>().Update(c);
                        }
                    }
                    _dbContext.Set<Account>().Update(item);
                }
            }

            _dbContext.Set<Account>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }

        public bool Exists(Expression<Func<Account, bool>> @where)
        {
            return _dbContext.Set<Account>().Any(@where);
        }

        
        public Account[] GetAccounts(Expression<Func<Account, bool>> @where = null)
        {
            var query = _dbContext.Set<Account>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            var enumerable = query.ToArray();
            return enumerable;
        }

        public IEnumerable<Account> GetAllActiveAccounts()
        {
            var accounts = _dbContext.Set<Account>().Where(FilterByActiveTrueAndCompany).Where(p=>p.CompanyId==LoggedInUser.CompanyId && p.Level==Hrevert.Common.Enums.AccountLevel.General);
            return accounts;
        }
        private static PagedTaskDataInquiryResponse FormatResultForPaging(PagedDataRequest requestInfo, IQueryable<Account> query)
        {
            var totalItemCount = query.Count();

            var startIndex = ResultsPagingUtility.CalculateStartIndex(requestInfo.PageNumber, requestInfo.PageSize);
            var mapper = new AccountToAccountViewModelMapper();
            var docs = query.OrderBy(x => x.DateCreated).Skip(startIndex).Take(requestInfo.PageSize).Select(s => mapper.Map(s)).ToList();

            var queryResult = new QueryResult<AccountViewModel>(docs, totalItemCount, requestInfo.PageSize);
            var inquiryResponse = new PagedTaskDataInquiryResponse()
            {
                Items = docs,
                TotalRecords = totalItemCount,
                PageCount = queryResult.TotalPageCount,
                PageNumber = requestInfo.PageNumber,
                PageSize = requestInfo.PageSize
            };

            return inquiryResponse;
        }

        public Account CheckIfDeletedAccountWithSameDescriptionExists(string accountDescription)
        {
            var account =
                _dbContext.Set<Account>()
                    .FirstOrDefault(
                        x =>
                            x.AccountDescription == accountDescription && x.CompanyId == LoggedInUser.CompanyId && (x.Active ||
                            x.Active == false));
            return account;
        }
        public Account CheckIfDeletedAccountWithSameCodeExists(string accountCode)
        {
            var account =
                _dbContext.Set<Account>()
                    .FirstOrDefault(
                        x =>
                            x.AccountCode == accountCode && x.CompanyId == LoggedInUser.CompanyId && (x.Active ||
                            x.Active == false));
            return account;
        }

        public PagedDataInquiryResponse<AccountViewModel> SearchAccounts(PagedDataRequest requestInfo, Expression<Func<Account, bool>> @where = null)
        {
            var filteredAccounts = _dbContext.Set<Account>().Where(x => x.CompanyId == LoggedInUser.CompanyId);
            if (requestInfo.Active)
                filteredAccounts = filteredAccounts.Where(req => req.Active);
            return FormatResultForPaging(requestInfo, filteredAccounts);
        }
        public bool DeleteOnlyAccount(int id)
        {
            var doc = GetAccount(id);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<Account>().Update(doc);
            result= _dbContext.SaveChanges();
            return result > 0;
        }
    }
}
