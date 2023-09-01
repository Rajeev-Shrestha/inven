using System.ComponentModel.DataAnnotations;
using Hrevert.Common.Enums;

namespace HrevertCRM.Data.ViewModels
{
    public class PaymentTermViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Term Code is required")]
        [StringLength(20, ErrorMessage = "Term Code can be at most 20 characters.")]
        public string TermCode { get; set; }
        [Required(ErrorMessage = "Term Code is required")]
        [StringLength(50, ErrorMessage = "Term Name can be at most 50 characters.")]
        public string TermName { get; set; }

     // [StringLength(100, ErrorMessage = "Description can be at most 100 characters.")]
        public string Description { get; set; }
        public TermType TermType { get; set; }
        public DueDateType DueDateType { get; set; }
        public DueType DueType { get; set; }
        public int? DueDateValue { get; set; }
        public PaymentDiscountType DiscountType { get; set; }
        public decimal? DiscountDays { get; set; }
        public decimal? DiscountValue { get; set; }

        public int CompanyId { get; set; }
        public byte[] Version { get; set; }
        public bool WebActive { get; set; }
        public bool Active { get; set; }
    }
}
