namespace HrevertCRM.Entities
{
    public class ProductsRefByKitAndAssembledType : BaseEntity
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int ProductRefId { get; set; }

        public virtual Product Product { get; set; }
    }
}
