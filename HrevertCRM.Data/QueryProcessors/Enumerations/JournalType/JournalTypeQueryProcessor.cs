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
    public class JournalTypesQueryProcessor : QueryBase<JournalTypes>, IJournalTypesQueryProcessor
    {
        public JournalTypesQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
        {
        }
        public JournalTypes Update(JournalTypes journalTypes)
        {
            var original = GetValidJournalTypes(journalTypes.Id);
            ValidateAuthorization(journalTypes);
            CheckVersionMismatch(journalTypes, original);

            original.Value = journalTypes.Value;
            original.Active = journalTypes.Active;
            original.CompanyId = journalTypes.CompanyId;

            _dbContext.Set<JournalTypes>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
        public virtual JournalTypes GetValidJournalTypes(int journalTypesId)
        {
            var journalTypes = _dbContext.Set<JournalTypes>().FirstOrDefault(sc => sc.Id == journalTypesId);
            if (journalTypes == null)
            {
                throw new RootObjectNotFoundException("JournalTypes not found");
            }
            return journalTypes;
        }
        public JournalTypes GetJournalTypes(int journalTypesId)
        {
            var journalTypes = _dbContext.Set<JournalTypes>().FirstOrDefault(d => d.Id == journalTypesId);
            return journalTypes;
        }
        public void SaveAllJournalTypes(List<JournalTypes> journalTypes)
        {
            _dbContext.Set<JournalTypes>().AddRange(journalTypes);
            _dbContext.SaveChanges();
        }
        public JournalTypes Save(JournalTypes journalTypes)
        {
            journalTypes.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<JournalTypes>().Add(journalTypes);
            _dbContext.SaveChanges();
            return journalTypes;
        }
        public int SaveAll(List<JournalTypes> journalTypes)
        {
            _dbContext.Set<JournalTypes>().AddRange(journalTypes);
            return _dbContext.SaveChanges();
        }
        public JournalTypes ActivateJournalTypes(int id)
        {
            var original = GetValidJournalTypes(id);
            ValidateAuthorization(original);

            original.Active = true;
            _dbContext.Set<JournalTypes>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
        public JournalTypeViewModel GetJournalTypesViewModel(int id)
        {
            var journalTypes = _dbContext.Set<JournalTypes>().Single(s => s.Id == id);
            var mapper = new JournalTypeToJournalTypeViewModelMapper();
            return mapper.Map(journalTypes);
        }
        public bool Delete(int journalTypesId)
        {
            var doc = GetJournalTypes(journalTypesId);
            ValidateAuthorization(doc);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<JournalTypes>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }
        public bool Exists(Expression<Func<JournalTypes, bool>> @where)
        {
            return _dbContext.Set<JournalTypes>().Any(@where);
        }
        public JournalTypes[] GetJournalTypes(Expression<Func<JournalTypes, bool>> @where = null)
        {

            var query = _dbContext.Set<JournalTypes>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            var enumerable = query.ToArray();
            return enumerable;
        }
        public IQueryable<JournalTypes> GetActiveJournalTypes()
        {
            return _dbContext.Set<JournalTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active);
        }
        public IQueryable<JournalTypes> GetDeletedJournalTypes()
        {
            return _dbContext.Set<JournalTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active == false);
        }
        public IQueryable<JournalTypes> GetAllJournalTypes()
        {
            var result = _dbContext.Set<JournalTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId);
            return result;
        }
    }
}
