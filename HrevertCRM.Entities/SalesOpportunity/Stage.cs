using System.Collections.Generic;

namespace HrevertCRM.Entities
{
    public class Stage:BaseEntity
    {
        public int Id { get; set; }
        public string StageName { get; set; }
        public int Rank { get; set; }
        public int Probability { get; set; }
        public virtual ICollection<SalesOpportunity> SalesOpportunities { get; set; }
    }
}
