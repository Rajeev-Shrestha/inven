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
    public class EncryptionTypesQueryProcessor : QueryBase<EncryptionTypes>, IEncryptionTypesQueryProcessor
    {
        public EncryptionTypesQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
        {
        }
        public EncryptionTypes Update(EncryptionTypes encryptionTypes)
        {
            var original = GetValidEncryptionTypes(encryptionTypes.Id);
            ValidateAuthorization(encryptionTypes);
            CheckVersionMismatch(encryptionTypes, original);

            original.Value = encryptionTypes.Value;
            original.Active = encryptionTypes.Active;
            original.CompanyId = encryptionTypes.CompanyId;

            _dbContext.Set<EncryptionTypes>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
        public virtual EncryptionTypes GetValidEncryptionTypes(int encryptionTypesId)
        {
            var encryptionTypes = _dbContext.Set<EncryptionTypes>().FirstOrDefault(sc => sc.Id == encryptionTypesId);
            if (encryptionTypes == null)
            {
                throw new RootObjectNotFoundException("Encryption Types not found");
            }
            return encryptionTypes;
        }
        public EncryptionTypes GetEncryptionTypes(int encryptionTypesId)
        {
            var encryptionTypes = _dbContext.Set<EncryptionTypes>().FirstOrDefault(d => d.Id == encryptionTypesId);
            return encryptionTypes;
        }
        public void SaveAllEncryptionTypes(List<EncryptionTypes> encryptionTypes)
        {
            _dbContext.Set<EncryptionTypes>().AddRange(encryptionTypes);
            _dbContext.SaveChanges();
        }
        public EncryptionTypes Save(EncryptionTypes encryptionTypes)
        {
            encryptionTypes.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<EncryptionTypes>().Add(encryptionTypes);
            _dbContext.SaveChanges();
            return encryptionTypes;
        }
        public int SaveAll(List<EncryptionTypes> encryptionTypes)
        {
            _dbContext.Set<EncryptionTypes>().AddRange(encryptionTypes);
            return _dbContext.SaveChanges();
        }
        public EncryptionTypes ActivateEncryptionTypes(int id)
        {
            var original = GetValidEncryptionTypes(id);
            ValidateAuthorization(original);

            original.Active = true;
            _dbContext.Set<EncryptionTypes>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
        public EncryptionTypeViewModel GetEncryptionTypesViewModel(int id)
        {
            var encryptionTypes = _dbContext.Set<EncryptionTypes>().Single(s => s.Id == id);
            var mapper = new EncryptionTypeToEncryptionTypeViewModelMapper();
            return mapper.Map(encryptionTypes);
        }
        public bool Delete(int encryptionTypesId)
        {
            var doc = GetEncryptionTypes(encryptionTypesId);
            ValidateAuthorization(doc);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<EncryptionTypes>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }
        public bool Exists(Expression<Func<EncryptionTypes, bool>> @where)
        {
            return _dbContext.Set<EncryptionTypes>().Any(@where);
        }
        public EncryptionTypes[] GetEncryptionTypes(Expression<Func<EncryptionTypes, bool>> @where = null)
        {

            var query = _dbContext.Set<EncryptionTypes>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            var enumerable = query.ToArray();
            return enumerable;
        }
        public IQueryable<EncryptionTypes> GetActiveEncryptionTypes()
        {
            return _dbContext.Set<EncryptionTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active);
        }
        public IQueryable<EncryptionTypes> GetDeletedEncryptionTypes()
        {
            return _dbContext.Set<EncryptionTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active == false);
        }
        public IQueryable<EncryptionTypes> GetAllEncryptionTypes()
        {
            var result = _dbContext.Set<EncryptionTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId);
            return result;
        }
    }
}
