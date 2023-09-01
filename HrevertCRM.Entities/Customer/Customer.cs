using System.Collections.Generic;

namespace HrevertCRM.Entities
{
    public class Customer :BaseEntity , IWebItem
    {
        public int Id { get; set; }
        public string CustomerCode { get; set; }
        public int? BillingAddressId { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public int? CustomerLevelId { get; set; }
        public bool WebActive { get; set; }
        public int? PaymentTermId { get; set; }
        public int? PaymentMethodId { get; set; }
        public decimal OpeningBalance { get; set; }
        public string Note { get; set; }
        public string TaxRegNo { get; set; }
        public string DisplayName { get; set; }
        public bool? IsPrepayEnabled { get; set; }
        public bool? IsCodEnabled { get; set; }
        public int? OnAccountId { get; set; }

        public int VatNo { get; set; }
        public int PanNo { get; set; }
        public virtual List<Address> Addresses { get; set; }
        public virtual Address BillingAddress { get; set; }
        public virtual CustomerLevel CustomerLevel { get; set; } 
        public virtual PaymentTerm PaymentTerm { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }

        public virtual IEnumerable<CustomerInContactGroup> CustomerInContactGroups { get; set; }
        public virtual List<SalesOrder> SalesOrders { get; set; }
        public virtual ICollection<SalesOpportunity> SalesOpportunity { get; set; }
        public virtual ICollection<Discount> Discounts { get; set; }
    }
}
