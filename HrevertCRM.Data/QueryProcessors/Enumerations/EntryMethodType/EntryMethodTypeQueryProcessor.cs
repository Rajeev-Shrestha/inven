using Hrevert.Common.Security;
using HrevertCRM.Entities.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.Mapper.Enumerations;

namespace HrevertCRM.Data.QueryProcessors.Enumerations
{
    public class EntryMethodTypeQueryProcessor: QueryBase<EntryMethodTypes>, IEntryMethodTypeQueryProcessor
    {
       public EntryMethodTypeQueryProcessor(IUserSession userSession, IDbContext dbContext): base(userSession, dbContext)
        {

        }
       public EntryMethodTypes Update(EntryMethodTypes entryMethodTypes) {
            var original = GetValidEntryMethodTypes(entryMethodTypes.Id);
            ValidateAuthorization(entryMethodTypes);
            CheckVersionMismatch(entryMethodTypes, original);

            original.Value = entryMethodTypes.Value;
            original.Active = entryMethodTypes.Active;
            original.CompanyId = entryMethodTypes.CompanyId;

            _dbContext.Set<EntryMethodTypes>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
      
       public EntryMethodTypes GetEntryMethodTypes(int entryMethodTypesId) {
            var entryMethodType = _dbContext.Set<EntryMethodTypes>().FirstOrDefault(d => d.Id == entryMethodTypesId);
            return entryMethodType;
        }
       public void SaveAllEntryMethodTypes(List<EntryMethodTypes> entryMethodTypes) {

            _dbContext.Set<EntryMethodTypes>().AddRange(entryMethodTypes);
            _dbContext.SaveChanges();

        }
        public EntryMethodTypes Save(EntryMethodTypes entryMethodTypes)
        {
            entryMethodTypes.CompanyId = LoggedInUser.CompanyId;
           _dbContext.Set<EntryMethodTypes>().Add(entryMethodTypes);
            _dbContext.SaveChanges();

           return entryMethodTypes;

        }
        public int SaveAll(List<EntryMethodTypes> entryMethodTypes) {
            _dbContext.Set<EntryMethodTypes>().AddRange(entryMethodTypes);
          return  _dbContext.SaveChanges();
        }
        public EntryMethodTypes ActivateEntryMethodTypes(int id) {

            var original = GetValidEntryMethodTypes(id);
            ValidateAuthorization(original);

            original.Active = true;
            _dbContext.Set<EntryMethodTypes>().Update(original);
            _dbContext.SaveChanges();
            return original;

        }
        public EntryMethodTypeViewModel GetEntryMethodTypesViewModel(int id) {
            var entryMethods = _dbContext.Set<EntryMethodTypes>().Single(s => s.Id == id);
            var mapper = new EntryMethodTypesToEntryMethodTypesViewModelMapper();
            return mapper.Map(entryMethods);
        }
        public bool Delete(int entryMethodTypesId) {
            var doc = GetEntryMethodTypes(entryMethodTypesId);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<EntryMethodTypes>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }
        public bool Exists(Expression<Func<EntryMethodTypes, bool>> @where) {
            return _dbContext.Set<EntryMethodTypes>().Any(@where);
        }
        public EntryMethodTypes[] GetEntryMethodTypes(Expression<Func<EntryMethodTypes, bool>> @where = null) {

            var query = _dbContext.Set<EntryMethodTypes>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            var enumerable = query.ToArray();
            return enumerable;
        }
        public IQueryable<EntryMethodTypes> GetActiveEntryMethodTypes() {

            return _dbContext.Set<EntryMethodTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active);

        }
        public IQueryable<EntryMethodTypes> GetDeletedEntryMethodTypes() {

            return _dbContext.Set<EntryMethodTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active==false);
        }
        public IQueryable<EntryMethodTypes> GetAllEntryMethodTypes() {
            var result= _dbContext.Set<EntryMethodTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active);

            return result;
        }



        public virtual EntryMethodTypes GetValidEntryMethodTypes(int entryMethodTypesId)
        {
            var entryMethods = _dbContext.Set<EntryMethodTypes>().FirstOrDefault(sc => sc.Id == entryMethodTypesId);
            if (entryMethods == null)
            {
                throw new RootObjectNotFoundException("EntryMethods not found");
            }
            return entryMethods;
        }
    }
}
