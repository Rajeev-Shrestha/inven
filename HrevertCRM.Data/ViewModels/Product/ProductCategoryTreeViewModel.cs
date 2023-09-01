using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.ViewModels
{
    public sealed class ProductCategoryTreeViewModel
    {
        public ProductCategoryTreeViewModel()
        {
            Children= new List<ProductCategoryTreeViewModel>();
        }

        public int? Id { get; set; }

        [StringLength(50, ErrorMessage = "Name can be at most 50 characters.")]
        public string Name { get; set; }

        [StringLength(200, ErrorMessage = "Description can be at most 200 characters.")]
        public string Description { get; set; }

        public short CategoryRank { get; set; }

        public int? ParentId { get; set; }
       
        public bool Active { get; set; }

        public bool WebActive { get; set; }
        public byte[] Version { get; set; }

        public List<ProductCategoryTreeViewModel> Children { get; set; }

        public List<ProductViewModel> ProductViewModels { get; set; }
    }
}
