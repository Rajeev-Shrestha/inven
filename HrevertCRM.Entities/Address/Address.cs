using System.Collections.Generic;
using Hrevert.Common.Enums;

namespace HrevertCRM.Entities
{
    public class Address : BaseEntity, IWebItem
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public AddressType AddressType { get; set; }
        public string Fax { get; set; }
        public TitleType Title { get; set; }
        public SuffixType Suffix { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int CountryId { get; set; }
        public string Telephone { get; set; }
        public string ZipCode { get; set; }
        public string MobilePhone { get; set; }
        public string Email { get; set; }
        public int? CustomerId { get; set; }
        public int? VendorId { get; set; }
        public bool IsDefault { get; set; }
        public string Website { get; set; }
        public int? DeliveryZoneId { get; set; }

        public Company Company { get; set; }
        public ApplicationUser User { get; set; }
        public Customer Customer { get; set; }
        public Vendor Vendor { get; set; }
        public DeliveryZone DeliveryZone { get; set; }

        public virtual ICollection<SalesOrder> SalesOrders { get; set; }
        public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; }
        public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; }
        public bool WebActive { get; set; }
    }
}
