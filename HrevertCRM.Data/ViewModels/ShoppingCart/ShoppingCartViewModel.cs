using System.Collections.Generic;
using HrevertCRM.Entities;
using System.ComponentModel.DataAnnotations;

namespace HrevertCRM.Data.ViewModels
{
    public class ShoppingCartViewModel : IWebItem
    {

        public int? Id { get; set; }
        public int? CustomerId { get; set; }

        [Required(ErrorMessage = "Host IP Address is required")]
        [StringLength(255, ErrorMessage = "Host IP can be at most 255 characters")]
        public string HostIp { get; set; }
        public bool IsCheckedOut { get; set; }

        public decimal Amount { get; set; }
        public int? BillingAddressId { get; set; }
        public int? ShippingAddressId { get; set; }
        public int PaymentTermId { get; set; }
        public int DeliveryMethodId { get; set; }

        public bool WebActive { get; set; }
        public int CompanyId { get; set; }
        public bool Active { get; set; }
        public byte[] Version { get; set; }

        public List<ShoppingCartDetailViewModel> ShoppingCartDetails { get; set; }
    }
}