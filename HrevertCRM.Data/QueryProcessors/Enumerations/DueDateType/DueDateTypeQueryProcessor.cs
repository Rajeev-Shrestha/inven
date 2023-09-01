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
    public class DueDateTypesQueryProcessor : QueryBase<DueDateTypes>, IDueDateTypesQueryProcessor
    {
        public DueDateTypesQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
        {
        }
        public DueDateTypes Update(DueDateTypes dueDateTypes)
        {
            var original = GetValidDueDateTypes(dueDateTypes.Id);
            ValidateAuthorization(dueDateTypes);
            CheckVersionMismatch(dueDateTypes, original);

            original.Value = dueDateTypes.Value;
            original.Active = dueDateTypes.Active;
            original.CompanyId = dueDateTypes.CompanyId;

            _dbContext.Set<DueDateTypes>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
        public virtual DueDateTypes GetValidDueDateTypes(int dueDateTypesId)
        {
            var dueDateTypes = _dbContext.Set<DueDateTypes>().FirstOrDefault(sc => sc.Id == dueDateTypesId);
            if (dueDateTypes == null)
            {
                throw new RootObjectNotFoundException("Due Date Types not found");
            }
            return dueDateTypes;
        }
        public DueDateTypes GetDueDateTypes(int dueDateTypesId)
        {
            var dueDateTypes = _dbContext.Set<DueDateTypes>().FirstOrDefault(d => d.Id == dueDateTypesId);
            return dueDateTypes;
        }
        public void SaveAllDueDateTypes(List<DueDateTypes> dueDateTypes)
        {
            _dbContext.Set<DueDateTypes>().AddRange(dueDateTypes);
            _dbContext.SaveChanges();
        }
        public DueDateTypes Save(DueDateTypes dueDateTypes)
        {
            dueDateTypes.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<DueDateTypes>().Add(dueDateTypes);
            _dbContext.SaveChanges();
            return dueDateTypes;
        }
        public int SaveAll(List<DueDateTypes> dueDateTypes)
        {
            _dbContext.Set<DueDateTypes>().AddRange(dueDateTypes);
            return _dbContext.SaveChanges();
        }
        public DueDateTypes ActivateDueDateTypes(int id)
        {
            var original = GetValidDueDateTypes(id);
            ValidateAuthorization(original);

            original.Active = true;
            _dbContext.Set<DueDateTypes>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
        public DueDateTypeViewModel GetDueDateTypesViewModel(int id)
        {
            var dueDateTypes = _dbContext.Set<DueDateTypes>().Single(s => s.Id == id);
            var mapper = new DueDateTypeToDueDateTypeViewModelMapper();
            return mapper.Map(dueDateTypes);
        }
        public bool Delete(int dueDateTypesId)
        {
            var doc = GetDueDateTypes(dueDateTypesId);
            ValidateAuthorization(doc);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<DueDateTypes>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }
        public bool Exists(Expression<Func<DueDateTypes, bool>> @where)
        {
            return _dbContext.Set<DueDateTypes>().Any(@where);
        }
        public DueDateTypes[] GetDueDateTypes(Expression<Func<DueDateTypes, bool>> @where = null)
        {

            var query = _dbContext.Set<DueDateTypes>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            var enumerable = query.ToArray();
            return enumerable;
        }
        public IQueryable<DueDateTypes> GetActiveDueDateTypes()
        {
            return _dbContext.Set<DueDateTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active);
        }
        public IQueryable<DueDateTypes> GetDeletedDueDateTypes()
        {
            return _dbContext.Set<DueDateTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active == false);
        }
        public IQueryable<DueDateTypes> GetAllDueDateTypes()
        {
            var result = _dbContext.Set<DueDateTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId);
            return result;
        }
    }
}
