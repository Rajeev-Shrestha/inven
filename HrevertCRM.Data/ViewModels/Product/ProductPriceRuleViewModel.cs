using System;
using System.ComponentModel.DataAnnotations;

namespace HrevertCRM.Data.ViewModels
{
    public class ProductPriceRuleViewModel
    {
        public int? Id { get; set; }
        public int? CustomerId { get; set; }
        public int? ProductId { get; set; }
        public int? CategoryId { get; set; }
        public int? Quantity { get; set; }
        public int? FreeQuantity { get; set; }
        public double? FixedPrice { get; set; }
        public double? DiscountPercent { get; set; }

        [Required(ErrorMessage = "Start Date is required")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End Date is required")]
        public DateTime EndDate { get; set; }

        public int CompanyId { get; set; }
        public bool Active { get; set; }
        public bool WebActive { get; set; }
        public byte[] Version { get; set; }
    }
}
