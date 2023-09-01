using System;
using Hrevert.Common.Enums;

namespace HrevertCRM.Entities
{
    public class Discount : BaseEntity, IWebItem
    {
        public int Id { get; set; }
        public int? ItemId { get; set; }
        public int? CategoryId { get; set; }
        public int? CustomerId { get; set; }
        public int? CustomerLevelId { get; set; }
        public double DiscountValue { get; set; }

        public DiscountType DiscountType { get; set; }
        public DateTime DiscountStartDate { get; set; }
        public DateTime DiscountEndDate { get; set; }
        public int? MinimumQuantity { get; set; }

        public virtual Product Product { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual CustomerLevel CustomerLevel { get; set; }
        public bool WebActive { get; set; }
    }
}
