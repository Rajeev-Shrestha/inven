namespace HrevertCRM.Data.ViewModels
{
    public class ItemMeasureViewModel
    {
        public int? Id { get; set; }
        public int ProductId { get; set; }
        public int MeasureUnitId { get; set; }
        public decimal Price { get; set; }

        public bool WebActive { get; set; }
        public int CompanyId { get; set; }
        public bool Active { get; set; }
        public byte[] Version { get; set; }
        public string ItemName { get; set; }
    }
}
