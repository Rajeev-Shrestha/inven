using HrevertCRM.Entities;
using System.Collections.Generic;
using System.Linq;

namespace HrevertCRM.Data.QueryProcessors
{
   public interface IReasonClosedQueryProcessor
    {
        ReasonClosed Save(ReasonClosed reasonClosed);
        ReasonClosed Update(ReasonClosed reasonClosed);
        int SaveAll(List<ReasonClosed> ClosedReasons);
        bool Delete(int reasonClosedId);
        ReasonClosed GetReasonClosedById(int reasonClosedId);
        IQueryable<ReasonClosed> GetAll();
        IQueryable<ReasonClosed> GetAllActive();
    }
}
