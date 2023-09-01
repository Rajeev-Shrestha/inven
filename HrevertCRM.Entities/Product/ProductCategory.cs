using System.Collections.Generic;

namespace HrevertCRM.Entities
{
    public class ProductCategory : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CategoryImageUrl { get; set; }
        public string Description { get; set; }
        public short CategoryRank { get; set; }
        public int? ParentId { get; set; }
        public bool WebActive { get; set; }

        public ProductCategory ParentCategory { get; set; }

       // public virtual List<Product> Products { get; set; }
        public virtual List<ProductInCategory> ProductInCategories { get; set; }
        public virtual List<DeliveryRate> DeliveryRates { get; set; }
        public virtual ICollection<ProductPriceRule> ProductPriceRules { get; set; }
        public virtual ICollection<Discount> Discounts { get; set; }
    }
}
