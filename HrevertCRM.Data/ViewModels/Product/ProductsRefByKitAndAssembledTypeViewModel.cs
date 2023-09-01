namespace HrevertCRM.Data.ViewModels
{
    public class ProductsRefByKitAndAssembledTypeViewModel
    {
        public int? Id { get; set; }
        public int ProductId { get; set; }
        public int ProductRefId { get; set; }

        public int CompanyId { get; set; }
        public bool Active { get; set; }
        public byte[] Version { get; set; }
    }
}
