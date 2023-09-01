using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HrevertCRM.Data.ViewModels
{
    public class CustomerViewModel
    {
        private int? _customerLevelId; 
        private int? _paymentTermId;
        private int? _paymentMethodId ;
        private int? _billingAddressId;
        public int? Id { get; set; }

        [StringLength(20, ErrorMessage = "Customer Code can be at most 20 characters.")]
        public string CustomerCode { get; set; }

      //[Required(ErrorMessage = "Password is required")]
        [StringLength(30, ErrorMessage = "Password must be between {2} to {1} charcters long." , MinimumLength = 6)]
        public string Password { get; set; }
        
      //[Required]
        [DisplayName("Re-type Password")]
        [StringLength(30, ErrorMessage = "Password must be between {2} to {1} characters.", MinimumLength = 6)]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        public decimal OpeningBalance { get; set; }

        [StringLength(200, ErrorMessage = "Note can be at most 200 characters.")]
        public string Note { get; set; }

        [StringLength(25, ErrorMessage = "Tax Registration Number can be at most 25 characters.")]
        public string TaxRegNo { get; set; }

        [StringLength(50, ErrorMessage = "Display Name can be at most 50 characters.")]
        public string DisplayName { get; set; }

        public int? PaymentMethodId
        {
            get { return _paymentMethodId == 0 ? null : _paymentMethodId; }
            set { _paymentMethodId = value; }
        }

        public AddressViewModel BillingAddress { get; set; }

        public int? BillingAddressId
        {
            get { return _billingAddressId == 0 ? null : _billingAddressId; }
            set { _billingAddressId = value; }
        }
        public int? PaymentTermId
        {
            get { return _paymentTermId == 0 ? null : _paymentTermId; }
            set { _paymentTermId = value; }
        }

        public int? CustomerLevelId
        {
            get { return _customerLevelId == 0 ? null : _customerLevelId; }
            set { _customerLevelId = value; }
        }
        public bool? IsPrepayEnabled { get; set; }
        public bool? IsCodEnabled { get; set; }
        public int? OnAccountId { get; set; }
        public bool Active { get; set; }
        public bool WebActive { get; set; }
        public byte[] Version { get; set; }
        public int CompanyId { get; set; }
        public int? VatNo { get; set; }
        public int? PanNo { get; set; }
        public virtual ICollection<AddressViewModel> Addresses { get; set; }
        public virtual List<CustomerInContactGroupViewModel> CustomerInContactGroups { get; set; }
        public virtual List<SalesOrderViewModel> SalesOrders { get; set; }
    }
}
