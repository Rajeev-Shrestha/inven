using System.ComponentModel.DataAnnotations;
using Hrevert.Common.Enums;

namespace HrevertCRM.Data.ViewModels
{
    public class CompanyWebSettingViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Shipping Calculation Type is required")]
        public ShippingCalculationType ShippingCalculationType { get; set; }

        [Required(ErrorMessage = "Discount Calculation Type is required")]
        public DiscountCalculationType DiscountCalculationType { get; set; }

        //[Required(ErrorMessage = "Sales Rep is required")]
        public string SalesRepId { get; set; }

        [Required(ErrorMessage = "Please check if Allow Guest or not")]
        public bool AllowGuest { get; set; }

        [Required(ErrorMessage = "Please enter the starting value of Customer Serial")]
        public int CustomerSerialNo { get; set; } = 0;

        [Required(ErrorMessage = "Please enter the starting value of Vendor Serial")]
        public int VendorSerialNo { get; set; } = 0;

        [Required(ErrorMessage = "Please enter the starting value of Sales Order Code")]
        public int SalesOrderCode { get; set; } = 0;

        [Required(ErrorMessage = "Please enter the starting value of Purchase Order Code")]
        public int PurchaseOrderCode { get; set; } = 0;

        [Required(ErrorMessage = "Please choose a Payment Method to be used as a default")]
        public int PaymentMethodId { get; set; }

        [Required(ErrorMessage = "Please choose a Delivery Method to be used as a default")]
        public int DeliveryMethodId { get; set; }

        public bool IsEstoreInitialized { get; set; }

        public int CompanyId { get; set; }
        public bool Active { get; set; }
        public byte[] Version { get; set; }
        public bool WebActive { get; set; }
    }
}
