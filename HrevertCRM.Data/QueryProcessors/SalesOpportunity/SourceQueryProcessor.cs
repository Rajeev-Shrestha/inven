using System;
using System.Collections.Generic;
using Hrevert.Common.Security;
using HrevertCRM.Entities;
using System.Linq;

namespace HrevertCRM.Data.QueryProcessors
{
    public class SourceQueryProcessor : QueryBase<Source>, ISourceQueryProcessor
    {
        public SourceQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
        {

        }
        public bool Delete(int sourceId)
        {

            var doc = GetSourceById(sourceId);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<Source>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }

        public IQueryable<Source> GetAll()
        {
            return _dbContext.Set<Source>().Where(x=>x.CompanyId==LoggedInUser.CompanyId);
        }

        public IQueryable<Source> GetAllActive()
        {
            return _dbContext.Set<Source>().Where(FilterByActiveTrueAndCompany);
        }

        public Source GetSourceById(int sourceId)
        {
            var source = _dbContext.Set<Source>().FirstOrDefault(x => x.Id == sourceId);
            return source;
        }

        public Source Save(Source source)
        {
            source.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<Source>().Add(source);
            _dbContext.SaveChanges();
            return source;
        }

        public int SaveAll(List<Source> sources)
        {
            _dbContext.Set<Source>().AddRange(sources);
            return _dbContext.SaveChanges();
        }

        public Source Update(Source source)
        {
            var original = GetSourceById(source.Id);
            ValidateAuthorization(source);
            CheckVersionMismatch(source, original);   //TODO: to test this method comment this out
            original.SourceName = source.SourceName;
            original.Active = source.Active;
            _dbContext.Set<Source>().Update(original);
            _dbContext.SaveChanges();
            return source;
        }
    }
}
