using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Hrevert.Common.Security;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.Mapper.Enumerations;
using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;

namespace HrevertCRM.Data.QueryProcessors.Enumerations
{
    public class AccountCashFlowTypesQueryProcessor : QueryBase<AccountCashFlowTypes>, IAccountCashFlowTypesQueryProcessor
    {
        public AccountCashFlowTypesQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
        {
        }
        public AccountCashFlowTypes Update(AccountCashFlowTypes accountCashFlowTypes)
        {
            var original = GetValidAccountCashFlowTypes(accountCashFlowTypes.Id);
            ValidateAuthorization(accountCashFlowTypes);
            CheckVersionMismatch(accountCashFlowTypes, original);

            original.Value = accountCashFlowTypes.Value;
            original.Active = accountCashFlowTypes.Active;
            original.CompanyId = accountCashFlowTypes.CompanyId;

            _dbContext.Set<AccountCashFlowTypes>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
        public virtual AccountCashFlowTypes GetValidAccountCashFlowTypes(int accountCashFlowTypesId)
        {
            var accountCashFlowTypes = _dbContext.Set<AccountCashFlowTypes>().FirstOrDefault(sc => sc.Id == accountCashFlowTypesId);
            if (accountCashFlowTypes == null)
            {
                throw new RootObjectNotFoundException("Account Cash Flow Types not found");
            }
            return accountCashFlowTypes;
        }
        public AccountCashFlowTypes GetAccountCashFlowTypes(int accountCashFlowTypesId)
        {
            var accountCashFlowTypes = _dbContext.Set<AccountCashFlowTypes>().FirstOrDefault(d => d.Id == accountCashFlowTypesId);
            return accountCashFlowTypes;
        }
        public void SaveAllAccountCashFlowTypes(List<AccountCashFlowTypes> accountCashFlowTypes)
        {
            _dbContext.Set<AccountCashFlowTypes>().AddRange(accountCashFlowTypes);
            _dbContext.SaveChanges();
        }
        public AccountCashFlowTypes Save(AccountCashFlowTypes accountCashFlowTypes)
        {
            accountCashFlowTypes.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<AccountCashFlowTypes>().Add(accountCashFlowTypes);
            _dbContext.SaveChanges();
            return accountCashFlowTypes;
        }
        public int SaveAll(List<AccountCashFlowTypes> accountCashFlowTypes)
        {
            _dbContext.Set<AccountCashFlowTypes>().AddRange(accountCashFlowTypes);
            return _dbContext.SaveChanges();
        }
        public AccountCashFlowTypes ActivateAccountCashFlowTypes(int id)
        {
            var original = GetValidAccountCashFlowTypes(id);
            ValidateAuthorization(original);

            original.Active = true;
            _dbContext.Set<AccountCashFlowTypes>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
        public AccountCashFlowTypeViewModel GetAccountCashFlowTypesViewModel(int id)
        {
            var accountCashFlowTypes = _dbContext.Set<AccountCashFlowTypes>().Single(s => s.Id == id);
            var mapper = new AccountCashFlowTypeToAccountCashFlowTypeViewModelMapper();
            return mapper.Map(accountCashFlowTypes);
        }
        public bool Delete(int accountCashFlowTypesId)
        {
            var doc = GetAccountCashFlowTypes(accountCashFlowTypesId);
            ValidateAuthorization(doc);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<AccountCashFlowTypes>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }
        public bool Exists(Expression<Func<AccountCashFlowTypes, bool>> @where)
        {
            return _dbContext.Set<AccountCashFlowTypes>().Any(@where);
        }
        public AccountCashFlowTypes[] GetAccountCashFlowTypes(Expression<Func<AccountCashFlowTypes, bool>> @where = null)
        {

            var query = _dbContext.Set<AccountCashFlowTypes>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            var enumerable = query.ToArray();
            return enumerable;
        }
        public IQueryable<AccountCashFlowTypes> GetActiveAccountCashFlowTypes()
        {
            return _dbContext.Set<AccountCashFlowTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active);
        }
        public IQueryable<AccountCashFlowTypes> GetDeletedAccountCashFlowTypes()
        {
            return _dbContext.Set<AccountCashFlowTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active == false);
        }
        public IQueryable<AccountCashFlowTypes> GetAllAccountCashFlowTypes()
        {
            var result = _dbContext.Set<AccountCashFlowTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId);
            return result;
        }
    }
}
