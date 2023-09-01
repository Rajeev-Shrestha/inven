using System;
using System.Collections.Generic;
using Hrevert.Common.Security;
using HrevertCRM.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace HrevertCRM.Data.QueryProcessors
{
    public class SalesOpportunityQueryProcessor: QueryBase<SalesOpportunity>, ISalesOpportunityQueryProcessor
    {
        public SalesOpportunityQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
        {

        }
        public bool Delete(int salesOpportunityId)
        {
            var doc = GetSalesOpportunityById(salesOpportunityId);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<SalesOpportunity>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }

        public IQueryable<SalesOpportunity> GetAll()
        {
            return _dbContext.Set<SalesOpportunity>().Include(a => a.Customer).Include(a => a.Stage)
                .Include(a => a.Grade).Include(a => a.Source).Include(a => a.ReasonClosed).Include(a => a.ApplicationUser)
                .Where(x => x.CompanyId == LoggedInUser.CompanyId);
        }
        public IQueryable<SalesOpportunity> GetAllActive()
        {
            return _dbContext.Set<SalesOpportunity>().Include(a=>a.Customer).Include(a=>a.Stage)
                .Include(a=>a.Grade).Include(a=>a.Source).Include(a=>a.ReasonClosed).Include(a=>a.ApplicationUser)
                .Where(FilterByActiveTrueAndCompany);
        }

        public IQueryable<SalesOpportunity> GetSalesOpportunityByStageId(int id)
        {
            return _dbContext.Set<SalesOpportunity>().Where(x => x.CompanyId == LoggedInUser.CompanyId && x.Active && x.StageId == id);
        }

        public SalesOpportunity GetSalesOpportunityById(int salesOpportunityId)
        {
            var salesOpportunity = _dbContext.Set<SalesOpportunity>().FirstOrDefault(x => x.Id == salesOpportunityId);
            return salesOpportunity;
        }

        public SalesOpportunity Save(SalesOpportunity salesOpportunity)
        {
            salesOpportunity.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<SalesOpportunity>().Add(salesOpportunity);
            _dbContext.SaveChanges();
            return salesOpportunity;
        }

        public int SaveAll(List<SalesOpportunity> salesOpportunities)
        {
            _dbContext.Set<SalesOpportunity>().AddRange(salesOpportunities);
            return _dbContext.SaveChanges();
        }

        public SalesOpportunity Update(SalesOpportunity salesOpportunity)
        {
            var original = GetSalesOpportunityById(salesOpportunity.Id);
            ValidateAuthorization(salesOpportunity);
            CheckVersionMismatch(salesOpportunity, original);   //TODO: to test this method comment this out
            original.Title = salesOpportunity.Title;
            original.ClosingDate = salesOpportunity.ClosingDate;
            original.BusinessValue = salesOpportunity.BusinessValue;
            original.Probability = salesOpportunity.Probability;
            original.Priority = salesOpportunity.Priority;
            original.IsClosed = salesOpportunity.IsClosed;
            original.ClosedDate = salesOpportunity.ClosedDate;
            original.IsSucceeded = salesOpportunity.IsSucceeded;
            original.CustomerId = salesOpportunity.CustomerId;
            original.SalesRepresentative = salesOpportunity.SalesRepresentative;
            original.StageId = salesOpportunity.StageId;
            original.SourceId = salesOpportunity.SourceId;
            original.GradeId = salesOpportunity.GradeId;
            original.ReasonClosedId = salesOpportunity.ReasonClosedId;
            original.Active = salesOpportunity.Active;
            _dbContext.Set<SalesOpportunity>().Update(original);
            _dbContext.SaveChanges();
            return salesOpportunity;
        }
    }
}
