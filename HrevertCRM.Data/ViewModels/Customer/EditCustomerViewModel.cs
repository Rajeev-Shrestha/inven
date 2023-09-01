using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HrevertCRM.Data.ViewModels
{
    public class EditCustomerViewModel
    { 
        private int? _customerLevelId;
        public int? Id { get; set; }

        [Required(ErrorMessage = "Customer Code is required")]
        [StringLength(15, ErrorMessage = "Customer Code can be at most 15 characters.")]
        public string CustomerCode { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Password must be between {2} to {1} characters.", MinimumLength = 6)]
        public string Password { get; set; }

        [DisplayName("Re-type Password")]
        [StringLength(30, ErrorMessage = "Password must be between {2} to {1} characters.", MinimumLength = 6)]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        public decimal OpeningBalance { get; set; }

        public int? BillingAddressId { get; set; }

        public int? CustomerLevelId
        {
            get{ return _customerLevelId == 0 ? null : _customerLevelId; }
            set { _customerLevelId = value; }
        }

        public int? PaymentTermId { get; set; }


        [StringLength(200, ErrorMessage = "Note can be at most 200 characters.")] 
        public string Note { get; set; }

        [StringLength(25, ErrorMessage = "Tax Registration Number can be at most 25 characters.")]
        public string TaxRegNo { get; set; }

        [StringLength(50, ErrorMessage = "Display Name can be at most 50 characters.")]
        public string DisplayName { get; set; }

        public bool? IsPrepayEnabled { get; set; }
        public bool? IsCodEnabled { get; set; }
        public int? OnAccountId { get; set; }
        public int? PaymentMethodId { get; set; }
        
        public AddressViewModel BillingAddress { get; set; }
        public virtual List<AddressViewModel> Addresses { get; set; }

        public virtual List<int> AddressIdList { get; set; }

        public int? VatNo { get; set; }
        public int? PanNo { get; set; }
        public bool WebActive { get; set; }
        public byte[] Version { get; set; }
        public int CompanyId { get; set; }
        public bool Active { get; set; }

    }
}
