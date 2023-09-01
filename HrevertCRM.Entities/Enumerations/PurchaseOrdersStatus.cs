using System.ComponentModel.DataAnnotations;

namespace HrevertCRM.Entities.Enumerations
{
    public class PurchaseOrdersStatus : BaseEntity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Value is required.")]
        [StringLength(30, ErrorMessage = "Value can be at most 20 characters.")]
        public string Value { get; set; }
    }
}
