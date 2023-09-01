namespace HrevertCRM.Entities
{
    public class ProductInCategory: BaseEntity
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int CategoryId { get; set; }

        public Product Product { get; set; }
        public ProductCategory ProductCategory { get; set; }
    }
}
