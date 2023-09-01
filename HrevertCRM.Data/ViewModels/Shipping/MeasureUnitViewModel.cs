using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Hrevert.Common.Enums;

namespace HrevertCRM.Data.ViewModels
{
    public class MeasureUnitViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Measure is required")]
        [StringLength(30, ErrorMessage = "Measure can be at most 30 characters")]
        public string Measure { get; set; }

        [Required(ErrorMessage = "Measure Code is required")]
        [StringLength(10, ErrorMessage = "Measure Code can be at most 10 characters")]
        public string MeasureCode { get; set; }
        [EnumDataType(typeof(EntryMethod))]
        public EntryMethod EntryMethod { get; set; }

        public bool WebActive { get; set; }
        public int CompanyId { get; set; }
        public bool Active { get; set; }
        public byte[] Version { get; set; }

        public virtual ICollection<DeliveryRateViewModel> DeliveryRates { get; set; }
        public virtual ICollection<ItemMeasureViewModel> ItemMeasures { get; set; }
    }
}
