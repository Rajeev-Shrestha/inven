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
    public class DescriptionTypesQueryProcessor : QueryBase<DescriptionTypes>, IDescriptionTypesQueryProcessor
    {
        public DescriptionTypesQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
        {
        }
        public DescriptionTypes Update(DescriptionTypes descriptionTypes)
        {
            var original = GetValidDescriptionTypes(descriptionTypes.Id);
            ValidateAuthorization(descriptionTypes);
            CheckVersionMismatch(descriptionTypes, original);

            original.Value = descriptionTypes.Value;
            original.Active = descriptionTypes.Active;
            original.CompanyId = descriptionTypes.CompanyId;

            _dbContext.Set<DescriptionTypes>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
        public virtual DescriptionTypes GetValidDescriptionTypes(int descriptionTypesId)
        {
            var descriptionTypes = _dbContext.Set<DescriptionTypes>().FirstOrDefault(sc => sc.Id == descriptionTypesId);
            if (descriptionTypes == null)
            {
                throw new RootObjectNotFoundException("Description Types not found");
            }
            return descriptionTypes;
        }
        public DescriptionTypes GetDescriptionTypes(int descriptionTypesId)
        {
            var descriptionTypes = _dbContext.Set<DescriptionTypes>().FirstOrDefault(d => d.Id == descriptionTypesId);
            return descriptionTypes;
        }
        public void SaveAllDescriptionTypes(List<DescriptionTypes> descriptionTypes)
        {
            _dbContext.Set<DescriptionTypes>().AddRange(descriptionTypes);
            _dbContext.SaveChanges();
        }
        public DescriptionTypes Save(DescriptionTypes descriptionTypes)
        {
            descriptionTypes.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<DescriptionTypes>().Add(descriptionTypes);
            _dbContext.SaveChanges();
            return descriptionTypes;
        }
        public int SaveAll(List<DescriptionTypes> descriptionTypes)
        {
            _dbContext.Set<DescriptionTypes>().AddRange(descriptionTypes);
            return _dbContext.SaveChanges();
        }
        public DescriptionTypes ActivateDescriptionTypes(int id)
        {
            var original = GetValidDescriptionTypes(id);
            ValidateAuthorization(original);

            original.Active = true;
            _dbContext.Set<DescriptionTypes>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
        public DescriptionTypeViewModel GetDescriptionTypesViewModel(int id)
        {
            var descriptionTypes = _dbContext.Set<DescriptionTypes>().Single(s => s.Id == id);
            var mapper = new DescriptionTypeToDescriptionTypeViewModelMapper();
            return mapper.Map(descriptionTypes);
        }
        public bool Delete(int descriptionTypesId)
        {
            var doc = GetDescriptionTypes(descriptionTypesId);
            ValidateAuthorization(doc);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<DescriptionTypes>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }
        public bool Exists(Expression<Func<DescriptionTypes, bool>> @where)
        {
            return _dbContext.Set<DescriptionTypes>().Any(@where);
        }
        public DescriptionTypes[] GetDescriptionTypes(Expression<Func<DescriptionTypes, bool>> @where = null)
        {

            var query = _dbContext.Set<DescriptionTypes>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            var enumerable = query.ToArray();
            return enumerable;
        }
        public IQueryable<DescriptionTypes> GetActiveDescriptionTypes()
        {
            return _dbContext.Set<DescriptionTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active);
        }
        public IQueryable<DescriptionTypes> GetDeletedDescriptionTypes()
        {
            return _dbContext.Set<DescriptionTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active == false);
        }
        public IQueryable<DescriptionTypes> GetAllDescriptionTypes()
        {
            var result = _dbContext.Set<DescriptionTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId);
            return result;
        }
    }
}
