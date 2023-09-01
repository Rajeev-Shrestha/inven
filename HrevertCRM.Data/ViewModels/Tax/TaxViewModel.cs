using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Hrevert.Common.Enums;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.ViewModels
{
    public class TaxViewModel : IWebItem
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Tax Code is required")]
        [StringLength(30, ErrorMessage = "Tax Code can be at most 30 characters.")]
        public string TaxCode { get; set; }

        [StringLength(1000, ErrorMessage = "Description can be at most 30 characters.")]
        public string Description { get; set; }

        [DisplayName("Is Recoverable")]
      //  [Range(typeof(bool), "true", "false", ErrorMessage = "The field Is Recoverable must be checked.")]
        public bool IsRecoverable { get; set; }

        [EnumDataType(typeof(TaxCaculationType))]
        public TaxCaculationType TaxType { get; set; }

        public decimal TaxRate { get; set; }

        public int RecoverableCalculationType { get; set; } //May be straight forward things like VAT calulation type

        public bool Active { get; set; }
        public int CompanyId { get; set; }
        public byte[] Version { get; set; }
        public bool WebActive { get; set; }
    }
}
