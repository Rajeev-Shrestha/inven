namespace HrevertCRM.Entities
{
    public class TaxesInProduct : BaseEntity
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int TaxId { get; set; }
        public Tax Tax { get; set; }
        public Product Product { get; set; }
    }
}
