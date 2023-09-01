using Hrevert.Common.Enums;

namespace HrevertCRM.Entities
{
    public class CompanyWebSetting : BaseEntity, IWebItem 
    {
        public int Id { get; set; }
        public ShippingCalculationType ShippingCalculationType { get; set; }
        public DiscountCalculationType DiscountCalculationType { get; set; }
        public string SalesRepId { get; set; }
        public bool AllowGuest { get; set; }
        public int CustomerSerialNo { get; set; } = 0;
        public int VendorSerialNo { get; set; } = 0;
        public int SalesOrderCode { get; set; } = 0;
        public int PurchaseOrderCode { get; set; } = 0;
        public bool IsEstoreInitialized { get; set; }
        public int PaymentMethodId { get; set; }
        public int DeliveryMethodId { get; set; }
        public bool WebActive { get; set; }
    }
}
