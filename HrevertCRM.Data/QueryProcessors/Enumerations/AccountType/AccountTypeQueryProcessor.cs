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
    public class AccountTypesQueryProcessor : QueryBase<AccountTypes>, IAccountTypesQueryProcessor
    {
        public AccountTypesQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
        {
        }
        public AccountTypes Update(AccountTypes accountTypes)
        {
            var original = GetValidAccountTypes(accountTypes.Id);
            ValidateAuthorization(accountTypes);
            CheckVersionMismatch(accountTypes, original);

            original.Value = accountTypes.Value;
            original.Active = accountTypes.Active;
            original.CompanyId = accountTypes.CompanyId;

            _dbContext.Set<AccountTypes>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
        public virtual AccountTypes GetValidAccountTypes(int accountTypesId)
        {
            var accountTypes = _dbContext.Set<AccountTypes>().FirstOrDefault(sc => sc.Id == accountTypesId);
            if (accountTypes == null)
            {
                throw new RootObjectNotFoundException("Account Types not found");
            }
            return accountTypes;
        }
        public AccountTypes GetAccountTypes(int accountTypesId)
        {
            var accountTypes = _dbContext.Set<AccountTypes>().FirstOrDefault(d => d.Id == accountTypesId);
            return accountTypes;
        }
        public void SaveAllAccountTypes(List<AccountTypes> accountTypes)
        {
            _dbContext.Set<AccountTypes>().AddRange(accountTypes);
            _dbContext.SaveChanges();
        }
        public AccountTypes Save(AccountTypes accountTypes)
        {
            accountTypes.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<AccountTypes>().Add(accountTypes);
            _dbContext.SaveChanges();
            return accountTypes;
        }
        public int SaveAll(List<AccountTypes> accountTypes)
        {
            _dbContext.Set<AccountTypes>().AddRange(accountTypes);
            return _dbContext.SaveChanges();
        }
        public AccountTypes ActivateAccountTypes(int id)
        {
            var original = GetValidAccountTypes(id);
            ValidateAuthorization(original);

            original.Active = true;
            _dbContext.Set<AccountTypes>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
        public AccountTypeViewModel GetAccountTypesViewModel(int id)
        {
            var accountTypes = _dbContext.Set<AccountTypes>().Single(s => s.Id == id);
            var mapper = new AccountTypeToAccountTypeViewModelMapper();
            return mapper.Map(accountTypes);
        }
        public bool Delete(int accountTypesId)
        {
            var doc = GetAccountTypes(accountTypesId);
            ValidateAuthorization(doc);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<AccountTypes>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }
        public bool Exists(Expression<Func<AccountTypes, bool>> @where)
        {
            return _dbContext.Set<AccountTypes>().Any(@where);
        }
        public AccountTypes[] GetAccountTypes(Expression<Func<AccountTypes, bool>> @where = null)
        {

            var query = _dbContext.Set<AccountTypes>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            var enumerable = query.ToArray();
            return enumerable;
        }
        public IQueryable<AccountTypes> GetActiveAccountTypes()
        {
            return _dbContext.Set<AccountTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active);
        }
        public IQueryable<AccountTypes> GetDeletedAccountTypes()
        {
            return _dbContext.Set<AccountTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active == false);
        }
        public IQueryable<AccountTypes> GetAllAccountTypes()
        {
            var result = _dbContext.Set<AccountTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId);
            return result;
        }
    }
}
