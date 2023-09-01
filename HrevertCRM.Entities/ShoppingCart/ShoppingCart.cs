using System.Collections.Generic;

namespace HrevertCRM.Entities
{
    public class ShoppingCart : BaseEntity, IWebItem
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public string HostIp { get; set; }
        public decimal Amount { get; set; }
        public bool WebActive { get; set; } 
        public int? BillingAddressId { get; set; }
        public int? ShippingAddressId { get; set; }
        public int? PaymentTermId { get; set; }
        public int? DeliveryMethodId { get; set; }
        public bool IsCheckedOut { get; set; }

        public virtual Address BillingAddress{ get; set; }
        public virtual PaymentTerm PaymentTerm { get; set; }
        public virtual DeliveryMethod DeliveryMethod { get; set; }
        public virtual Company Company { get; set; }

        public virtual ICollection<ShoppingCartDetail> ShoppingCartDetails { get; set; }
    }
}
