using Hrevert.Common.Enums;

namespace HrevertCRM.Entities
{
    public class PurchaseOrderLine : BaseEntity, IWebItem
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DescriptionType? DescriptionType { get; set; }
        public decimal ItemPrice { get; set; }
        public bool Shipped { get; set; }
        public decimal ItemQuantity { get; set; }
        public decimal ShippedQuantity { get; set; }
        public short LineOrder { get; set; } //Line Order records line numbers as entered
        public decimal Discount { get; set; }
        public DiscountType DiscountType { get; set; }
        public decimal TaxAmount { get; set; }
        public int? TaxId { get; set; }
        public int PurchaseOrderId { get; set; }
        public int ProductId { get; set; }
        public bool WebActive { get; set; }

        public Tax TaxType { get; set; }
        public virtual PurchaseOrder PurchaseOrder { get; set; }
        public Product Product { get; set; }
    }
}
