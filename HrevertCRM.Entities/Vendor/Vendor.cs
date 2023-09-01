using System.Collections.Generic;

namespace HrevertCRM.Entities
{
    public class Vendor : BaseEntity, IWebItem
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string ContactName { get; set; }

        public decimal CreditLimit { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public int? PaymentTermId { get; set; }
        public int? PaymentMethodId { get; set; }
        public int? BillingAddressId { get; set; }
        public bool WebActive { get; set; }

        public virtual Address BillingAddress { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }
        public virtual PaymentTerm PaymentTerm { get; set; }
        public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; }
        public virtual List<Address> Addresses { get; set; }
    }
}
