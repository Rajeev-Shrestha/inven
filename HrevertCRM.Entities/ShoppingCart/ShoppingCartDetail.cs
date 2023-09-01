using System;
using Hrevert.Common.Enums;

namespace HrevertCRM.Entities
{
    public class ShoppingCartDetail : BaseEntity, IWebItem
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public Guid? Guid { get; set; }
        public int ProductId { get; set; }
        public decimal ProductCost { get; set; }
        public ProductType ProductType { get; set; }
        public decimal Quantity { get; set; }
        public decimal Weight { get; set; }
        public decimal Discount { get; set; }
        public int ShoppingCartId { get; set; }
        public decimal TaxAmount { get; set; }
        public DateTime ShoppingDateTime { get; set; }
        public decimal? ShippingCost { get; set; }
        public bool WebActive { get; set; }

        public virtual ShoppingCart ShoppingCart { get; set; }
        public virtual Company Company { get; set; }
        public virtual Product Product { get; set; }
    }
}
