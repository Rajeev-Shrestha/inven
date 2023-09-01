using Hrevert.Common.Enums;
using HrevertCRM.Entities;
using System.ComponentModel.DataAnnotations;

namespace HrevertCRM.Data.ViewModels
{
    public class EcommerceSettingViewModel
    {
        public int? Id { get; set; }
        public DisplayQuantity DisplayQuantity { get; set; }
        public bool IncludeQuantityInSalesOrder { get; set; }
        public bool DisplayOutOfStockItems { get; set; }
        public int ProductPerCategory { get; set; }
        public bool DecreaseQuantityOnOrder { get; set; }
        [Required(ErrorMessage = "Please upload a logo")]
        public Image Image { get; set; }
        public string ImageUrl { get; set; }
        public int? DueDatePeriod { get; set; }

        public int CompanyId { get; set; }
        public bool Active { get; set; }
        public bool WebActive { get; set; }
        public byte[] Version { get; set; }
    }
}
