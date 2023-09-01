using System;

namespace HrevertCRM.Entities
{
    public class ProductPriceRule : BaseEntity, IWebItem
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public int? ProductId { get; set; }
        public int? CategoryId { get; set; }
        public int? Quantity { get; set; }
        public int? FreeQuantity { get; set; }
        public double? FixedPrice { get; set; }
        public double? DiscountPercent { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Company Company { get; set; }
        public Product Product { get; set; }
        public ProductCategory Category { get; set; }
        public bool WebActive { get; set; }
    }
}
