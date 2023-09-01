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
    public class SuffixTypesQueryProcessor : QueryBase<SuffixTypes>, ISuffixTypesQueryProcessor
    {
        public SuffixTypesQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
        {
        }
        public SuffixTypes Update(SuffixTypes suffixTypes)
        {
            var original = GetValidSuffixTypes(suffixTypes.Id);
            ValidateAuthorization(suffixTypes);
            CheckVersionMismatch(suffixTypes, original);

            original.Value = suffixTypes.Value;
            original.Active = suffixTypes.Active;
            original.CompanyId = suffixTypes.CompanyId;

            _dbContext.Set<SuffixTypes>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
        public virtual SuffixTypes GetValidSuffixTypes(int suffixTypesId)
        {
            var suffixTypes = _dbContext.Set<SuffixTypes>().FirstOrDefault(sc => sc.Id == suffixTypesId);
            if (suffixTypes == null)
            {
                throw new RootObjectNotFoundException("SuffixTypes not found");
            }
            return suffixTypes;
        }
        public SuffixTypes GetSuffixTypes(int suffixTypesId)
        {
            var suffixTypes = _dbContext.Set<SuffixTypes>().FirstOrDefault(d => d.Id == suffixTypesId);
            return suffixTypes;
        }
        public void SaveAllSuffixTypes(List<SuffixTypes> suffixTypes)
        {
            _dbContext.Set<SuffixTypes>().AddRange(suffixTypes);
            _dbContext.SaveChanges();
        }
        public SuffixTypes Save(SuffixTypes suffixTypes)
        {
            suffixTypes.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<SuffixTypes>().Add(suffixTypes);
            _dbContext.SaveChanges();
            return suffixTypes;
        }
        public int SaveAll(List<SuffixTypes> suffixTypes)
        {
            _dbContext.Set<SuffixTypes>().AddRange(suffixTypes);
            return _dbContext.SaveChanges();
        }
        public SuffixTypes ActivateSuffixTypes(int id)
        {
            var original = GetValidSuffixTypes(id);
            ValidateAuthorization(original);

            original.Active = true;
            _dbContext.Set<SuffixTypes>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
        public SuffixTypeViewModel GetSuffixTypesViewModel(int id)
        {
            var suffixTypes = _dbContext.Set<SuffixTypes>().Single(s => s.Id == id);
            var mapper = new SuffixTypeToSuffixTypeViewModelMapper();
            return mapper.Map(suffixTypes);
        }
        public bool Delete(int suffixTypesId)
        {
            var doc = GetSuffixTypes(suffixTypesId);
            ValidateAuthorization(doc);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<SuffixTypes>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }
        public bool Exists(Expression<Func<SuffixTypes, bool>> @where)
        {
            return _dbContext.Set<SuffixTypes>().Any(@where);
        }
        public SuffixTypes[] GetSuffixTypes(Expression<Func<SuffixTypes, bool>> @where = null)
        {

            var query = _dbContext.Set<SuffixTypes>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            var enumerable = query.ToArray();
            return enumerable;
        }
        public IQueryable<SuffixTypes> GetActiveSuffixTypes()
        {
            return _dbContext.Set<SuffixTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active);
        }
        public IQueryable<SuffixTypes> GetDeletedSuffixTypes()
        {
            return _dbContext.Set<SuffixTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active == false);
        }
        public IQueryable<SuffixTypes> GetAllSuffixTypes()
        {
            var result = _dbContext.Set<SuffixTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId);
            return result;
        }
    }
}
