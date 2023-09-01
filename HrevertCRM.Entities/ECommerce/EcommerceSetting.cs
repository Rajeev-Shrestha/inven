using Hrevert.Common.Enums;

namespace HrevertCRM.Entities
{
    public class EcommerceSetting : BaseEntity, IWebItem
    {
        public int Id { get; set; }
        public DisplayQuantity DisplayQuantity { get; set; }
        public bool IncludeQuantityInSalesOrder { get; set; }
        public bool DisplayOutOfStockItems { get; set; }
        public int ProductPerCategory { get; set; }
        public bool DecreaseQuantityOnOrder { get; set; }
        public bool WebActive { get; set; }
        public string ImageUrl { get; set; }
        public int DueDatePeriod { get; set; }
    }
}
