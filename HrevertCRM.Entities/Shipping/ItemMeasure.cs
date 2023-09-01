namespace HrevertCRM.Entities
{
    public class ItemMeasure : BaseEntity, IWebItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int MeasureUnitId { get; set; }
        public decimal Price { get; set; }

        public virtual Product Product { get; set; }
        public virtual MeasureUnit MeasureUnit { get; set; }
        public bool WebActive { get; set; }
    }
}
