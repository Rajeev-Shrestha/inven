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
    public class DueTypesQueryProcessor : QueryBase<DueTypes>, IDueTypesQueryProcessor
    {
        public DueTypesQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
        {
        }
        public DueTypes Update(DueTypes dueTypes)
        {
            var original = GetValidDueTypes(dueTypes.Id);
            ValidateAuthorization(dueTypes);
            CheckVersionMismatch(dueTypes, original);

            original.Value = dueTypes.Value;
            original.Active = dueTypes.Active;
            original.CompanyId = dueTypes.CompanyId;

            _dbContext.Set<DueTypes>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
        public virtual DueTypes GetValidDueTypes(int dueTypesId)
        {
            var dueTypes = _dbContext.Set<DueTypes>().FirstOrDefault(sc => sc.Id == dueTypesId);
            if (dueTypes == null)
            {
                throw new RootObjectNotFoundException("Due Types not found");
            }
            return dueTypes;
        }
        public DueTypes GetDueTypes(int dueTypesId)
        {
            var dueTypes = _dbContext.Set<DueTypes>().FirstOrDefault(d => d.Id == dueTypesId);
            return dueTypes;
        }
        public void SaveAllDueTypes(List<DueTypes> dueTypes)
        {
            _dbContext.Set<DueTypes>().AddRange(dueTypes);
            _dbContext.SaveChanges();
        }
        public DueTypes Save(DueTypes dueTypes)
        {
            dueTypes.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<DueTypes>().Add(dueTypes);
            _dbContext.SaveChanges();
            return dueTypes;
        }
        public int SaveAll(List<DueTypes> dueTypes)
        {
            _dbContext.Set<DueTypes>().AddRange(dueTypes);
            return _dbContext.SaveChanges();
        }
        public DueTypes ActivateDueTypes(int id)
        {
            var original = GetValidDueTypes(id);
            ValidateAuthorization(original);

            original.Active = true;
            _dbContext.Set<DueTypes>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
        public DueTypeViewModel GetDueTypesViewModel(int id)
        {
            var dueTypes = _dbContext.Set<DueTypes>().Single(s => s.Id == id);
            var mapper = new DueTypeToDueTypeViewModelMapper();
            return mapper.Map(dueTypes);
        }
        public bool Delete(int dueTypesId)
        {
            var doc = GetDueTypes(dueTypesId);
            ValidateAuthorization(doc);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<DueTypes>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }
        public bool Exists(Expression<Func<DueTypes, bool>> @where)
        {
            return _dbContext.Set<DueTypes>().Any(@where);
        }
        public DueTypes[] GetDueTypes(Expression<Func<DueTypes, bool>> @where = null)
        {

            var query = _dbContext.Set<DueTypes>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            var enumerable = query.ToArray();
            return enumerable;
        }
        public IQueryable<DueTypes> GetActiveDueTypes()
        {
            return _dbContext.Set<DueTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active);
        }
        public IQueryable<DueTypes> GetDeletedDueTypes()
        {
            return _dbContext.Set<DueTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active == false);
        }
        public IQueryable<DueTypes> GetAllDueTypes()
        {
            var result = _dbContext.Set<DueTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId);
            return result;
        }
    }
}
