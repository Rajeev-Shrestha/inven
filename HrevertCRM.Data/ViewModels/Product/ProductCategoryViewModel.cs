using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.ViewModels
{
    public class ProductCategoryViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Product Category name is required")]
        [StringLength(50, ErrorMessage = "Name can be at most 50 characters.")]
        public string Name { get; set; }
        public Image CategoryImage { get; set; }
        public string CategoryImageUrl { get; set; }

        [StringLength(200, ErrorMessage = "Description can ne at most 200 characters.")]
        public string Description { get; set; }

        public short CategoryRank { get; set; }

        public int? ParentId { get; set; }

        public bool WebActive { get; set; }
        public byte[] Version { get; set; }

        public bool Active { get; set; }

        public int CompanyId { get; set; }

        public virtual List<ProductViewModel> Products { get; set; }
        public virtual List<ProductInCategoryViewModel> ProductInCategories { get; set; }
    }
}
