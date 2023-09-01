namespace HrevertCRM.Data.ViewModels
{
    public class ConvertCartToOrderViewModel
    {
        public int CartId { get; set; }
        public int CustomerId { get; set; }
        public AddressViewModel ShippingAddressViewModel { get; set; }
        //public Address BillingAddress { get; set; }
        public int BillingAddressId { get; set; }
        public int PaymentMethodId { get; set; }
        public int DeliveryMethodId { get; set; }
        public int PaymentTermId { get; set; }
        //public string FileBase64 { get; set; }  // To test this viewmodel comment this out
    }
}
