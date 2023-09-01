using System;
using Hrevert.Common.Enums;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.ViewModels
{
    public class DiscountViewModel : IWebItem
    {
        public int? Id { get; set; }
        public int? ItemId { get; set; }
        public int? CategoryId { get; set; }
        public DiscountType DiscountType { get; set; }
        public double? DiscountValue { get; set; }
        public DateTime DiscountStartDate { get; set; }
        public DateTime DiscountEndDate { get; set; }
        public int? MinimumQuantity { get; set; }
        public int? CustomerId { get; set; }
        public int? CustomerLevelId { get; set; }

        public int CompanyId { get; set; }
        public bool Active { get; set; }
        public byte[] Version { get; set; }
        public bool WebActive { get; set; }

        public ProductViewModel ProductViewModel { get; set; }
        public ProductCategoryViewModel ProductCategoryViewModel { get; set; }
        public CustomerViewModel CustomerViewModel { get; set; }
        public CustomerLevelViewModel CustomerLevelViewModel { get; set; }
        public string ItemName { get; set; }
    }
}
