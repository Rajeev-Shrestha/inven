using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Hrevert.Common.Enums;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.ViewModels
{
    public class ProductViewModelForStore
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Product Code is required")]
        [StringLength(50, ErrorMessage = "Code can be at most 50 characters.")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Product Name is required")]
        [StringLength(100, ErrorMessage = "Name can be at most 100 characters.")]
        public string Name { get; set; }

        [StringLength(200, ErrorMessage = "Description can be at most 200 characters.")]
        public string ShortDescription { get; set; }

        [StringLength(2000, ErrorMessage = "Description can be at most 2000 characters.")]
        public string LongDescription { get; set; }

        public double UnitPrice { get; set; }

        [Required(ErrorMessage = "Quantity on Hand is required")]
        public int QuantityOnHand { get; set; }
        public int QuantityOnOrder { get; set; }
        public bool? Commissionable { get; set; }
        public decimal? CommissionRate { get; set; }

        public byte[] Version { get; set; }
        public bool WebActive { get; set; }
        public bool Active { get; set; }
        public int CompanyId { get; set; }
        public bool AllowBackOrder { get; set; }

        public virtual List<int> Categories { get; set; }
        public virtual List<ImageUrlBySizeViewModel> ImageUrls { get; set; }
        public double DiscountPrice { get; set; }
        public int DiscountPercentage { get; set; }
        public DiscountType DiscountType { get; set; }

        public List<Discount> Discounts { get; set; }

        public virtual List<Image> Images { get; set; }
        public virtual List<int> ProductsReferencedByKitAndAssembledType { get; set; }
        public virtual List<string> ListOfRefProductNames { get; set; }
        public virtual List<int> Taxes { get; set; }
        public virtual List<TaxNameAndAmountList> TaxAndAmounts { get; set; }
        public List<TaxesInProduct> TaxesInProducts { get; set; }
        public List<TaxViewModel> TaxViewModelList { get; set; }
        public ProductType ProductType { get; set; }
        public List<ProductViewModel> ProductsRefByAssembledAndKit { get; set; }
        public virtual List<int> NewlyAssignedCats { get; set; }
    }

    public class ImageUrlBySizeViewModel
    {
        public string SmallImageUrl { get; set; }
        public string MediumImageUrl { get; set; }
        public string LargeImageUrl { get; set; }
    }
}
