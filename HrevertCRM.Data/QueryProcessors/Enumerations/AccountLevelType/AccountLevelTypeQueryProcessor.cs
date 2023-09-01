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
    public class AccountLevelTypesQueryProcessor : QueryBase<AccountLevelTypes>, IAccountLevelTypesQueryProcessor
    {
        public AccountLevelTypesQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
        {
        }
        public AccountLevelTypes Update(AccountLevelTypes accountLevelTypes)
        {
            var original = GetValidAccountLevelTypes(accountLevelTypes.Id);
            ValidateAuthorization(accountLevelTypes);
            CheckVersionMismatch(accountLevelTypes, original);

            original.Value = accountLevelTypes.Value;
            original.Active = accountLevelTypes.Active;
            original.CompanyId = accountLevelTypes.CompanyId;

            _dbContext.Set<AccountLevelTypes>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
        public virtual AccountLevelTypes GetValidAccountLevelTypes(int accountLevelTypesId)
        {
            var accountLevelTypes = _dbContext.Set<AccountLevelTypes>().FirstOrDefault(sc => sc.Id == accountLevelTypesId);
            if (accountLevelTypes == null)
            {
                throw new RootObjectNotFoundException("Account Level Types not found");
            }
            return accountLevelTypes;
        }
        public AccountLevelTypes GetAccountLevelTypes(int accountLevelTypesId)
        {
            var accountLevelTypes = _dbContext.Set<AccountLevelTypes>().FirstOrDefault(d => d.Id == accountLevelTypesId);
            return accountLevelTypes;
        }
        public void SaveAllAccountLevelTypes(List<AccountLevelTypes> accountLevelTypes)
        {
            _dbContext.Set<AccountLevelTypes>().AddRange(accountLevelTypes);
            _dbContext.SaveChanges();
        }
        public AccountLevelTypes Save(AccountLevelTypes accountLevelTypes)
        {
            accountLevelTypes.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<AccountLevelTypes>().Add(accountLevelTypes);
            _dbContext.SaveChanges();
            return accountLevelTypes;
        }
        public int SaveAll(List<AccountLevelTypes> accountLevelTypes)
        {
            _dbContext.Set<AccountLevelTypes>().AddRange(accountLevelTypes);
            return _dbContext.SaveChanges();
        }
        public AccountLevelTypes ActivateAccountLevelTypes(int id)
        {
            var original = GetValidAccountLevelTypes(id);
            ValidateAuthorization(original);

            original.Active = true;
            _dbContext.Set<AccountLevelTypes>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
        public AccountLevelTypeViewModel GetAccountLevelTypesViewModel(int id)
        {
            var accountLevelTypes = _dbContext.Set<AccountLevelTypes>().Single(s => s.Id == id);
            var mapper = new AccountLevelTypeToAccountLevelTypeViewModelMapper();
            return mapper.Map(accountLevelTypes);
        }
        public bool Delete(int accountLevelTypesId)
        {
            var doc = GetAccountLevelTypes(accountLevelTypesId);
            ValidateAuthorization(doc);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<AccountLevelTypes>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }
        public bool Exists(Expression<Func<AccountLevelTypes, bool>> @where)
        {
            return _dbContext.Set<AccountLevelTypes>().Any(@where);
        }
        public AccountLevelTypes[] GetAccountLevelTypes(Expression<Func<AccountLevelTypes, bool>> @where = null)
        {

            var query = _dbContext.Set<AccountLevelTypes>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            var enumerable = query.ToArray();
            return enumerable;
        }
        public IQueryable<AccountLevelTypes> GetActiveAccountLevelTypes()
        {
            return _dbContext.Set<AccountLevelTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active);
        }
        public IQueryable<AccountLevelTypes> GetDeletedAccountLevelTypes()
        {
            return _dbContext.Set<AccountLevelTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active == false);
        }
        public IQueryable<AccountLevelTypes> GetAllAccountLevelTypes()
        {
            var result = _dbContext.Set<AccountLevelTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId);
            return result;
        }
    }
}
