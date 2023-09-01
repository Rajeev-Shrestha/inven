using System.Collections.Generic;

namespace HrevertCRM.Data.ViewModels
{
    public class SalesOrderDefaultValuesViewModel
    {
        public string LoggedInUserId { get; set; }
        public int? BillingAddressId { get; set; }
        public int? ShippingAddressId { get; set; }
        public int? PaymentTermsId { get; set; }

        public List<AddressViewModel> ShippingAddresses { get; set; }
        public List<AddressViewModel> BillingAddresses { get; set; }

        public CustomerViewModel Customer { get; set; }
    }
}