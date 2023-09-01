using System.Collections.Generic;
using System.Linq;
using Hrevert.Common.Constants;
using Hrevert.Common.Security;
using HrevertCRM.Data.Common;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.QueryProcessors
{
    public class BugLoggerQueryProcessor : QueryBase<BugLogger>, IBugLoggerQueryProcessor
    {
        public BugLoggerQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
        {
        }
        public BugLogger Save(BugLogger bug)
        {
            _dbContext.Set<BugLogger>().Add(bug);
            _dbContext.SaveChanges();
            return bug;
        }

        public BugLogger Update(BugLogger bug)
        {
            var original = _dbContext.Set<BugLogger>().FirstOrDefault(bg => bg.Id == bug.Id);
            if(original == null) throw new RootObjectNotFoundException(BugLoggerConstants.BugLoggerQueryProcessorConstants.BugNotFound);
            ValidateAuthorization(bug);
            CheckVersionMismatch(bug, original);

            original.Message = bug.Message;

            _dbContext.Set<BugLogger>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        public BugLogger Get(int bugId)
        {
            var bug = _dbContext.Set<BugLogger>().FirstOrDefault(bg => bg.Id == bugId && bg.CompanyId == LoggedInUser.CompanyId && bg.Active);
            return bug;
        }

        public List<BugLogger> GetAllBugs()
        {
            var bugs = _dbContext.Set<BugLogger>().Where(FilterByActiveTrueAndCompany);
            return bugs.ToList();
        }
    }
}
