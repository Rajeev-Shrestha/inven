using System.Collections.Generic;

namespace HrevertCRM.Entities
{
    public class Source:BaseEntity
    {
        public int Id { get; set; }
        public string SourceName { get; set; }
        public virtual ICollection<SalesOpportunity> SalesOpportunities { get; set; }
    }
}
