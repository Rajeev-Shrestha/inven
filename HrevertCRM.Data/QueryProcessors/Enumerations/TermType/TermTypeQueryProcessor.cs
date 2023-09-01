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
    public class TermTypesQueryProcessor : QueryBase<TermTypes>, ITermTypesQueryProcessor
    {
        public TermTypesQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
        {
        }
        public TermTypes Update(TermTypes termTypes)
        {
            var original = GetValidTermTypes(termTypes.Id);
            ValidateAuthorization(termTypes);
            CheckVersionMismatch(termTypes, original);

            original.Value = termTypes.Value;
            original.Active = termTypes.Active;
            original.CompanyId = termTypes.CompanyId;

            _dbContext.Set<TermTypes>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
        public virtual TermTypes GetValidTermTypes(int termTypesId)
        {
            var termTypes = _dbContext.Set<TermTypes>().FirstOrDefault(sc => sc.Id == termTypesId);
            if (termTypes == null)
            {
                throw new RootObjectNotFoundException("Term Types not found");
            }
            return termTypes;
        }
        public TermTypes GetTermTypes(int termTypesId)
        {
            var termTypes = _dbContext.Set<TermTypes>().FirstOrDefault(d => d.Id == termTypesId);
            return termTypes;
        }
        public void SaveAllTermTypes(List<TermTypes> termTypes)
        {
            _dbContext.Set<TermTypes>().AddRange(termTypes);
            _dbContext.SaveChanges();
        }
        public TermTypes Save(TermTypes termTypes)
        {
            termTypes.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<TermTypes>().Add(termTypes);
            _dbContext.SaveChanges();
            return termTypes;
        }
        public int SaveAll(List<TermTypes> termTypes)
        {
            _dbContext.Set<TermTypes>().AddRange(termTypes);
            return _dbContext.SaveChanges();
        }
        public TermTypes ActivateTermTypes(int id)
        {
            var original = GetValidTermTypes(id);
            ValidateAuthorization(original);

            original.Active = true;
            _dbContext.Set<TermTypes>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
        public TermTypeViewModel GetTermTypesViewModel(int id)
        {
            var termTypes = _dbContext.Set<TermTypes>().Single(s => s.Id == id);
            var mapper = new TermTypeToTermTypeViewModelMapper();
            return mapper.Map(termTypes);
        }
        public bool Delete(int termTypesId)
        {
            var doc = GetTermTypes(termTypesId);
            ValidateAuthorization(doc);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<TermTypes>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }
        public bool Exists(Expression<Func<TermTypes, bool>> @where)
        {
            return _dbContext.Set<TermTypes>().Any(@where);
        }
        public TermTypes[] GetTermTypes(Expression<Func<TermTypes, bool>> @where = null)
        {

            var query = _dbContext.Set<TermTypes>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            var enumerable = query.ToArray();
            return enumerable;
        }
        public IQueryable<TermTypes> GetActiveTermTypes()
        {
            return _dbContext.Set<TermTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active);
        }
        public IQueryable<TermTypes> GetDeletedTermTypes()
        {
            return _dbContext.Set<TermTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active == false);
        }
        public IQueryable<TermTypes> GetAllTermTypes()
        {
            var result = _dbContext.Set<TermTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId);
            return result;
        }
    }
}
