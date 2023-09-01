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
    public class AccountDetailTypesQueryProcessor : QueryBase<AccountDetailTypes>, IAccountDetailTypesQueryProcessor
    {
        public AccountDetailTypesQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
        {
        }
        public AccountDetailTypes Update(AccountDetailTypes accountDetailTypes)
        {
            var original = GetValidAccountDetailTypes(accountDetailTypes.Id);
            ValidateAuthorization(accountDetailTypes);
            CheckVersionMismatch(accountDetailTypes, original);

            original.Value = accountDetailTypes.Value;
            original.Active = accountDetailTypes.Active;
            original.CompanyId = accountDetailTypes.CompanyId;

            _dbContext.Set<AccountDetailTypes>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
        public virtual AccountDetailTypes GetValidAccountDetailTypes(int accountDetailTypesId)
        {
            var accountDetailTypes = _dbContext.Set<AccountDetailTypes>().FirstOrDefault(sc => sc.Id == accountDetailTypesId);
            if (accountDetailTypes == null)
            {
                throw new RootObjectNotFoundException("Account Detail Types not found");
            }
            return accountDetailTypes;
        }
        public AccountDetailTypes GetAccountDetailTypes(int accountDetailTypesId)
        {
            var accountDetailTypes = _dbContext.Set<AccountDetailTypes>().FirstOrDefault(d => d.Id == accountDetailTypesId);
            return accountDetailTypes;
        }
        public void SaveAllAccountDetailTypes(List<AccountDetailTypes> accountDetailTypes)
        {
            _dbContext.Set<AccountDetailTypes>().AddRange(accountDetailTypes);
            _dbContext.SaveChanges();
        }
        public AccountDetailTypes Save(AccountDetailTypes accountDetailTypes)
        {
            accountDetailTypes.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<AccountDetailTypes>().Add(accountDetailTypes);
            _dbContext.SaveChanges();
            return accountDetailTypes;
        }
        public int SaveAll(List<AccountDetailTypes> accountDetailTypes)
        {
            _dbContext.Set<AccountDetailTypes>().AddRange(accountDetailTypes);
            return _dbContext.SaveChanges();
        }
        public AccountDetailTypes ActivateAccountDetailTypes(int id)
        {
            var original = GetValidAccountDetailTypes(id);
            ValidateAuthorization(original);

            original.Active = true;
            _dbContext.Set<AccountDetailTypes>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
        public AccountDetailTypeViewModel GetAccountDetailTypesViewModel(int id)
        {
            var accountDetailTypes = _dbContext.Set<AccountDetailTypes>().Single(s => s.Id == id);
            var mapper = new AccountDetailTypeToAccountDetailTypeViewModelMapper();
            return mapper.Map(accountDetailTypes);
        }
        public bool Delete(int accountDetailTypesId)
        {
            var doc = GetAccountDetailTypes(accountDetailTypesId);
            ValidateAuthorization(doc);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<AccountDetailTypes>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }
        public bool Exists(Expression<Func<AccountDetailTypes, bool>> @where)
        {
            return _dbContext.Set<AccountDetailTypes>().Any(@where);
        }
        public AccountDetailTypes[] GetAccountDetailTypes(Expression<Func<AccountDetailTypes, bool>> @where = null)
        {

            var query = _dbContext.Set<AccountDetailTypes>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            var enumerable = query.ToArray();
            return enumerable;
        }
        public IQueryable<AccountDetailTypes> GetActiveAccountDetailTypes()
        {
            return _dbContext.Set<AccountDetailTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active);
        }
        public IQueryable<AccountDetailTypes> GetDeletedAccountDetailTypes()
        {
            return _dbContext.Set<AccountDetailTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active == false);
        }
        public IQueryable<AccountDetailTypes> GetAllAccountDetailTypes()
        {
            var result = _dbContext.Set<AccountDetailTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId);
            return result;
        }
    }
}
