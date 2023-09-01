using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HrevertCRM.Data.ViewModels
{
    public class DeliveryZoneViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Zone Name is required")]
        [StringLength(30, ErrorMessage = "Zone Name can be at most 30 characters")]
        public string ZoneName { get; set; }

        [Required(ErrorMessage = "Zone Code is required")]
        [StringLength(5, ErrorMessage = "Zone Code can be at most 5 characters")]
        public string ZoneCode { get; set; }

        public bool WebActive { get; set; }
        public int CompanyId { get; set; }
        public bool Active { get; set; }
        public byte[] Version { get; set; }

        public virtual ICollection<AddressViewModel> Addresses { get; set; }
        public virtual ICollection<DeliveryRateViewModel> DeliveryRates { get; set; }
    }
}
