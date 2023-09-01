using System.Collections.Generic;
using Hrevert.Common.Enums;

namespace HrevertCRM.Entities
{
    public class Product : BaseEntity, IWebItem
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public double UnitPrice { get; set; }
        public ProductType ProductType { get; set; }
        public int QuantityOnHand { get; set; }
        public int QuantityOnOrder { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public bool WebActive { get; set; }
        public bool? Commissionable { get; set; }
        public decimal? CommissionRate { get; set; }
        public bool AllowBackOrder { get; set; }

        public virtual ICollection<TaxesInProduct> TaxesInProducts { get; set; }
        public virtual ICollection<ProductInCategory> ProductInCategories { get; set; }
        public virtual ICollection<ProductMetadata> ProductMetadatas { get; set; }
        public virtual ICollection<ProductPriceRule> ProductPriceRules { get; set; }
        public virtual ICollection<ProductRate> ProductRates { get; set; }
        public virtual ICollection<SalesOrderLine> SalesOrderLines { get; set; }
        public virtual ICollection<PurchaseOrderLine> PurchaseOrderLines { get; set; }
        public virtual ICollection<ShoppingCartDetail> ShoppingCartDetails { get; set; }
        public virtual ICollection<DeliveryRate> DeliveryRates { get; set; }
        public virtual ICollection<ItemMeasure> ItemMeasures { get; set; }
        public virtual ICollection<ProductsRefByKitAndAssembledType> ProductsReferencedByKitAndAssembledTypes { get; set; }
        public virtual ICollection<Discount> Discounts { get; set; }
        public virtual Company Company { get; set; }

        //public ICollection<IFormFile> Files { get; set; }
    }
}
