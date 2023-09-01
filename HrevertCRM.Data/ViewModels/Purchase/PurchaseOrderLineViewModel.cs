using System.ComponentModel.DataAnnotations;
using Hrevert.Common.Enums;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.ViewModels
{
    public class PurchaseOrderLineViewModel : IWebItem
    {
        public int? Id { get; set; }

        [StringLength(200, ErrorMessage = "Description can be at most 200 characters.")]
        public string Description { get; set; }

        [EnumDataType(typeof(DescriptionType))]
        public DescriptionType? DescriptionType { get; set; }

        public decimal ItemPrice { get; set; }

        public bool Shipped { get; set; }

        public decimal ItemQuantity { get; set; }

        public decimal ShippedQuantity { get; set; }

        public short LineOrder { get; set; } //Line Order records line numbers as entered

        public decimal Discount { get; set; }

        [EnumDataType(typeof(DiscountType))]
        public DiscountType DiscountType { get; set; }

        public decimal TaxAmount { get; set; }

        public int? TaxId { get; set; }

        public int PurchaseOrderId { get; set; }

        public int ProductId { get; set; }

        public int CompanyId { get; set; }
        public bool Active { get; set; }
        public byte[] Version { get; set; }
        public bool WebActive { get; set; }
    }
}
