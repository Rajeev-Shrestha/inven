using System.Collections.Generic;
using Hrevert.Common.Security;
using HrevertCRM.Entities;
using System.Linq;
using System;

namespace HrevertCRM.Data.QueryProcessors
{
    public class ReasonClosedQueryProcessor: QueryBase<ReasonClosed>, IReasonClosedQueryProcessor
    {
        public ReasonClosedQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
        {

        }

        public bool Delete(int reasonClosedId)
        {
            var doc = GetReasonClosedById(reasonClosedId);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<ReasonClosed>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }

        public IQueryable<ReasonClosed> GetAll()
        {
            return _dbContext.Set<ReasonClosed>().Where(x => x.CompanyId == LoggedInUser.CompanyId);
        }

        public IQueryable<ReasonClosed> GetAllActive()
        {
            return _dbContext.Set<ReasonClosed>().Where(FilterByActiveTrueAndCompany);
        }

        public ReasonClosed GetReasonClosedById(int reasonClosedId)
        {
            var closedReasonData = _dbContext.Set<ReasonClosed>().FirstOrDefault(x => x.Id == reasonClosedId);
            return closedReasonData;
        }

        public ReasonClosed Save(ReasonClosed reasonClosed)
        {
            reasonClosed.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<ReasonClosed>().Add(reasonClosed);
            _dbContext.SaveChanges();
            return reasonClosed;
        }

        public int SaveAll(List<ReasonClosed> ClosedReasons)
        {
            _dbContext.Set<ReasonClosed>().AddRange(ClosedReasons);
            return _dbContext.SaveChanges();
        }

        public ReasonClosed Update(ReasonClosed reasonClosed)
        {
            var original = GetReasonClosedById(reasonClosed.Id);
            ValidateAuthorization(reasonClosed);
            CheckVersionMismatch(reasonClosed, original);   //TODO: to test this method comment this out
            original.Reason = reasonClosed.Reason;
            original.Active = reasonClosed.Active;
            _dbContext.Set<ReasonClosed>().Update(original);
            _dbContext.SaveChanges();
            return reasonClosed;
        }
    }
}
