using System.Collections.Generic;

namespace HrevertCRM.Entities
{
    public class DeliveryZone : BaseEntity, IWebItem
    {
        public int Id { get; set; }
        public string ZoneName { get; set; }
        public string ZoneCode { get; set; }

        public bool WebActive { get; set; }
        public virtual ICollection<Address> Addresses{ get; set; }
        public virtual ICollection<DeliveryRate> DeliveryRates { get; set; }
    }
}
