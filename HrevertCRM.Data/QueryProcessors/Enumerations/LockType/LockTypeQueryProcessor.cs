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
    public class LockTypesQueryProcessor : QueryBase<LockTypes>, ILockTypesQueryProcessor
    {
        public LockTypesQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
        {
        }
        public LockTypes Update(LockTypes lockTypes)
        {
            var original = GetValidLockTypes(lockTypes.Id);
            ValidateAuthorization(lockTypes);
            CheckVersionMismatch(lockTypes, original);

            original.Value = lockTypes.Value;
            original.Active = lockTypes.Active;
            original.CompanyId = lockTypes.CompanyId;

            _dbContext.Set<LockTypes>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
        public virtual LockTypes GetValidLockTypes(int lockTypesId)
        {
            var lockTypes = _dbContext.Set<LockTypes>().FirstOrDefault(sc => sc.Id == lockTypesId);
            if (lockTypes == null)
            {
                throw new RootObjectNotFoundException("LockTypes not found");
            }
            return lockTypes;
        }
        public LockTypes GetLockTypes(int lockTypesId)
        {
            var lockTypes = _dbContext.Set<LockTypes>().FirstOrDefault(d => d.Id == lockTypesId);
            return lockTypes;
        }
        public void SaveAllLockTypes(List<LockTypes> lockTypes)
        {
            _dbContext.Set<LockTypes>().AddRange(lockTypes);
            _dbContext.SaveChanges();
        }
        public LockTypes Save(LockTypes lockTypes)
        {
            lockTypes.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<LockTypes>().Add(lockTypes);
            _dbContext.SaveChanges();
            return lockTypes;
        }
        public int SaveAll(List<LockTypes> lockTypes)
        {
            _dbContext.Set<LockTypes>().AddRange(lockTypes);
            return _dbContext.SaveChanges();
        }
        public LockTypes ActivateLockTypes(int id)
        {
            var original = GetValidLockTypes(id);
            ValidateAuthorization(original);

            original.Active = true;
            _dbContext.Set<LockTypes>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
        public LockTypeViewModel GetLockTypesViewModel(int id)
        {
            var lockTypes = _dbContext.Set<LockTypes>().Single(s => s.Id == id);
            var mapper = new LockTypeToLockTypeViewModelMapper();
            return mapper.Map(lockTypes);
        }
        public bool Delete(int lockTypesId)
        {
            var doc = GetLockTypes(lockTypesId);
            ValidateAuthorization(doc);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<LockTypes>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }
        public bool Exists(Expression<Func<LockTypes, bool>> @where)
        {
            return _dbContext.Set<LockTypes>().Any(@where);
        }
        public LockTypes[] GetLockTypes(Expression<Func<LockTypes, bool>> @where = null)
        {

            var query = _dbContext.Set<LockTypes>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            var enumerable = query.ToArray();
            return enumerable;
        }
        public IQueryable<LockTypes> GetActiveLockTypes()
        {
            return _dbContext.Set<LockTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active);
        }
        public IQueryable<LockTypes> GetDeletedLockTypes()
        {
            return _dbContext.Set<LockTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active == false);
        }
        public IQueryable<LockTypes> GetAllLockTypes()
        {
            var result = _dbContext.Set<LockTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId);
            return result;
        }
    }
}
