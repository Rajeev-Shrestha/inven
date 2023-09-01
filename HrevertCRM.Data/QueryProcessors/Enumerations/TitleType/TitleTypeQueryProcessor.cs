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
    public class TitleTypesQueryProcessor : QueryBase<TitleTypes>, ITitleTypesQueryProcessor
    {
        public TitleTypesQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
        {
        }
        public TitleTypes Update(TitleTypes titleTypes)
        {
            var original = GetValidTitleTypes(titleTypes.Id);
            ValidateAuthorization(titleTypes);
            CheckVersionMismatch(titleTypes, original);

            original.Value = titleTypes.Value;
            original.Active = titleTypes.Active;
            original.CompanyId = titleTypes.CompanyId;

            _dbContext.Set<TitleTypes>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
        public virtual TitleTypes GetValidTitleTypes(int titleTypesId)
        {
            var titleTypes = _dbContext.Set<TitleTypes>().FirstOrDefault(sc => sc.Id == titleTypesId);
            if (titleTypes == null)
            {
                throw new RootObjectNotFoundException("TitleTypes not found");
            }
            return titleTypes;
        }
        public TitleTypes GetTitleTypes(int titleTypesId)
        {
            var titleTypes = _dbContext.Set<TitleTypes>().FirstOrDefault(d => d.Id == titleTypesId);
            return titleTypes;
        }
        public void SaveAllTitleTypes(List<TitleTypes> titleTypes)
        {
            _dbContext.Set<TitleTypes>().AddRange(titleTypes);
            _dbContext.SaveChanges();
        }
        public TitleTypes Save(TitleTypes titleTypes)
        {
            titleTypes.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<TitleTypes>().Add(titleTypes);
            _dbContext.SaveChanges();
            return titleTypes;
        }
        public int SaveAll(List<TitleTypes> titleTypes)
        {
            _dbContext.Set<TitleTypes>().AddRange(titleTypes);
            return _dbContext.SaveChanges();
        }
        public TitleTypes ActivateTitleTypes(int id)
        {
            var original = GetValidTitleTypes(id);
            ValidateAuthorization(original);

            original.Active = true;
            _dbContext.Set<TitleTypes>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
        public TitleTypeViewModel GetTitleTypesViewModel(int id)
        {
            var titleTypes = _dbContext.Set<TitleTypes>().Single(s => s.Id == id);
            var mapper = new TitleTypeToTitleTypeViewModelMapper();
            return mapper.Map(titleTypes);
        }
        public bool Delete(int titleTypesId)
        {
            var doc = GetTitleTypes(titleTypesId);
            ValidateAuthorization(doc);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<TitleTypes>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }
        public bool Exists(Expression<Func<TitleTypes, bool>> @where)
        {
            return _dbContext.Set<TitleTypes>().Any(@where);
        }
        public TitleTypes[] GetTitleTypes(Expression<Func<TitleTypes, bool>> @where = null)
        {

            var query = _dbContext.Set<TitleTypes>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            var enumerable = query.ToArray();
            return enumerable;
        }
        public IQueryable<TitleTypes> GetActiveTitleTypes()
        {
            return _dbContext.Set<TitleTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active);
        }
        public IQueryable<TitleTypes> GetDeletedTitleTypes()
        {
            return _dbContext.Set<TitleTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active == false);
        }
        public IQueryable<TitleTypes> GetAllTitleTypes()
        {
            var result = _dbContext.Set<TitleTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId);
            return result;
        }
    }
}
