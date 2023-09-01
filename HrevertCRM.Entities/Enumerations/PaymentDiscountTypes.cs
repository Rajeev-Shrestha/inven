using System.ComponentModel.DataAnnotations;

namespace HrevertCRM.Entities.Enumerations
{
    public class PaymentDiscountTypes : BaseEntity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Value is required.")]
        [StringLength(50, ErrorMessage = "Value can be at most 50 characters.")]
        public string Value { get; set; }
    }
}
