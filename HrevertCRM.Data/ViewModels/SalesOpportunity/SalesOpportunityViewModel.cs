using Hrevert.Common.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace HrevertCRM.Data.ViewModels
{
    public class SalesOpportunityViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Sales Opportunity title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Closing Date is required")]
        public DateTime ClosingDate { get; set; }

        [Required(ErrorMessage = "Business Value is required")]
        public decimal BusinessValue { get; set; }

        [Required(ErrorMessage = "Probability is required")]
        public int Probability { get; set; }

        [EnumDataType(typeof(SalesOpportunityPriority))]
        public SalesOpportunityPriority Priority { get; set; }
        public bool IsClosed { get; set; }
        public DateTime ClosedDate { get; set; }
        public bool IsSucceeded { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string SalesRepresentative { get; set; } // Application User ho
        public string SalesRepresentativeName { get; set; }
        public int StageId { get; set; }
        public string StageName { get; set; }
        public int SourceId { get; set; }
        public string SourceName { get; set; }
        public int GradeId { get; set; }
        public string GradeName { get; set; }
        public int ReasonClosedId { get; set; }
        public string ReasonClosedName { get; set; }
        public int CompanyId { get; set; }
        public byte[] Version { get; set; }
        public bool Active { get; set; }
    }
}
