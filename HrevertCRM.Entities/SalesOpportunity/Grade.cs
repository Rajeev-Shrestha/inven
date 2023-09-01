using System.Collections.Generic;

namespace HrevertCRM.Entities
{
    public class Grade: BaseEntity
    {
        public int Id { get; set; }
        public string GradeName { get; set; }
        public int Rank { get; set; }
        public virtual ICollection<SalesOpportunity> SalesOpportunities { get; set; }
    }
}
