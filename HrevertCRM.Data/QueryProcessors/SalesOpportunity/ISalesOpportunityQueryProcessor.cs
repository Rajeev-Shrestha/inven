using HrevertCRM.Entities;
using System.Collections.Generic;
using System.Linq;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface ISalesOpportunityQueryProcessor
    {
        SalesOpportunity Save(SalesOpportunity salesOpportunity);
        SalesOpportunity Update(SalesOpportunity salesOpportunity);
        int SaveAll(List<SalesOpportunity> salesOpportunities);
        bool Delete(int salesOpportunityId);
        SalesOpportunity GetSalesOpportunityById(int salesOpportunityId);
        IQueryable<SalesOpportunity> GetAll();
        IQueryable<SalesOpportunity> GetAllActive();
        IQueryable<SalesOpportunity> GetSalesOpportunityByStageId(int id);
    }
}
