namespace HrevertCRM.Entities
{
    public class ProductRate : BaseEntity, IModificationHistory
    {
        public int Id { get; set; }
        public int DistributerId { get; set; }
        public int ProductId { get; set; }  
        public double CostPrice { get; set; }
        public double SalesPrice { get; set; }

        public Company Company { get; set; }
        public Distributor Distributor { get; set; }
        public Product Product { get; set; }
    }
}
