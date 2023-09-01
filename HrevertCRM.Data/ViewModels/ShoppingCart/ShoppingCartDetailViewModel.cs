using HrevertCRM.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Hrevert.Common.Enums;

namespace HrevertCRM.Data.ViewModels
{
    public class ShoppingCartDetailViewModel : IWebItem
    {
        public int? Id { get; set; }
        public int ShoppingCartId { get; set; }
        [RegularExpression(@"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$")]
        [Required(ErrorMessage = "Please select a user.")]
        public Guid Guid { get; set; }
        public int? CustomerId { get; set; }
        public int ProductId { get; set; }
        public decimal ProductCost { get; set; }
        public string ProductDescription { get; set; }
        public decimal Quantity { get; set; }
        public decimal Weight { get; set; }
        public decimal Discount { get; set; }
        public decimal TaxAmount { get; set; }
        public ProductType ProductType { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime ShoppingDateTime { get; set; } = DateTime.Now;
        public decimal? ShippingCost { get; set; }


        public int CompanyId { get; set; }
        public bool WebActive { get; set; }
        public bool Active { get; set; }
        public byte[] Version { get; set; }

        // These property are just for the UI
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public List<string> ProductImageUrls { get; set; }
        public List<ShoppingCartDetailViewModel> ProductsRefByAssembledAndKit { get; set; }
        public List<TaxViewModel> Taxes { get; set; }
        public List<TaxNameAndAmountList> TaxAndAmounts { get; set; }


    }

    public class TaxNameAndAmountList
    {
        public string TaxName { get; set; }
        public decimal TaxAmount { get; set; }
    }
}
