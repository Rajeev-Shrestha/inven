using System.Collections.Generic;
using Hrevert.Common.Enums;

namespace HrevertCRM.Entities
{
    public class MeasureUnit : BaseEntity, IWebItem
    {
        public int Id { get; set; }
        public string Measure { get; set; }
        public string MeasureCode { get; set; }
        public EntryMethod EntryMethod { get; set; }
        public bool WebActive { get; set; }

        public virtual ICollection<DeliveryRate> DeliveryRates { get; set; }
        public virtual ICollection<ItemMeasure> ItemMeasures { get; set; }
    }
}
        //public string ConversionMeasureId { get; set; }
        //public int? ConversionUnit { get; set; }
