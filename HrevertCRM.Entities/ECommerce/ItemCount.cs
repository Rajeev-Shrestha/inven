namespace HrevertCRM.Entities
{
    public class ItemCount : BaseEntity, IWebItem
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int QuantityOnOrder { get; set; }
        public int QuantityOnInvoice { get; set; }
        public bool WebActive { get; set; }
    }
}
