using System.Collections.Generic;

namespace HrevertCRM.Entities
{
    public class ReasonClosed:BaseEntity
    {
        public int Id { get; set; }
        public string Reason { get; set; }
        public virtual ICollection<SalesOpportunity> SalesOpportunities { get; set; }
    }
}
