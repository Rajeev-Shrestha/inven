using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.ViewModels
{
    public class VendorViewModel : IWebItem
    {
        private int? _paymentTermId;
        private int? _paymentMethodId;
        private int? _billingAddressId;

        public int? Id { get; set; }
        public string Code { get; set; }

        public decimal CreditLimit { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }

        [StringLength(25, ErrorMessage = "Contact Name can be at most 25 characters.")]
        public string ContactName { get; set; }

        public int? PaymentTermId
        {
            get { return _paymentTermId == 0 ? null : _paymentTermId; }
            set { _paymentTermId = value; }
        }
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

        public byte[] Version { get; set; }
        public int CompanyId { get; set; }
        public bool Active { get; set; }
        public bool WebActive { get; set; }

        public ICollection<AddressViewModel> Addresses { get; set; }
        public ICollection<PurchaseOrderViewModel> PurchaseOrders { get; set; }
    }
}
