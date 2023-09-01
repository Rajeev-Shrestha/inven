using System.ComponentModel.DataAnnotations;

namespace HrevertCRM.Data.ViewModels
{
    public class PaymentMethodViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Payment Method Code is required")]
        [StringLength(20, ErrorMessage = "Method Code can be at most 20 characters.")]
        public string MethodCode { get; set; }

        [Required(ErrorMessage = "Payment Method Name is required")]
        [StringLength(50, ErrorMessage = "Method Name can be at most 50 characters.")]
        public string MethodName { get; set; }

        public int? AccountId { get; set; } //For Future

        [StringLength(500, ErrorMessage = "Receipent Memo can be at most 500 characters.")]
        public string ReceipentMemo { get; set; }

        public bool WebActive { get; set; }
        public int CompanyId { get; set; }
        public byte[] Version { get; set; }
        public bool Active { get; set; }
    }
}
