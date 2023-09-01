namespace HrevertCRM.Entities
{
    public class DeliveryRate : BaseEntity, IWebItem
    {
        public int Id { get; set; }
        public int? DeliveryMethodId { get; set; }
        public int? DeliveryZoneId { get; set; }
        public decimal? WeightFrom { get; set; }
        public decimal? WeightTo { get; set; }
        public int? ProductCategoryId { get; set; }
        public int? ProductId { get; set; }
        public decimal? MinimumRate { get; set; }
        public decimal? Rate { get; set; }
        public decimal? DocTotalFrom { get; set; }
        public decimal? DocTotalTo { get; set; }
        public int? UnitFrom { get; set; }
        public int? UnitTo { get; set; }
        public int? MeasureUnitId { get; set; }

        public DeliveryZone DeliveryZone { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public MeasureUnit MeasureUnit { get; set; }
        public Product Product { get; set; }
        public ProductCategory ProductCategory { get; set; }

        public bool WebActive { get; set; }
    }
}
