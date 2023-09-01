using Hrevert.Common.Enums;
using System;

namespace HrevertCRM.Entities
{
    public class SalesOpportunity: BaseEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime ClosingDate { get; set; }
        public decimal BusinessValue { get; set; }
        public int Probability { get; set; }
        public SalesOpportunityPriority Priority { get; set; }
        public bool IsClosed { get; set; }
        public DateTime ClosedDate { get; set; }
        public bool IsSucceeded { get; set; }
        public int CustomerId { get; set; }
        public string SalesRepresentative { get; set; } // Application User ho
        public int StageId{ get; set; }
        public int SourceId { get; set; }
        public int GradeId { get; set; }
        public int ReasonClosedId { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Stage Stage { get; set; }
        public virtual Source Source { get; set; }
        public virtual Grade Grade { get; set; }
        public virtual ReasonClosed ReasonClosed { get; set; }
    }
}
