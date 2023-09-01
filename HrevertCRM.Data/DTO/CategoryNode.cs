using System.Collections.Generic;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.DTO
{
    public class CategoryNode
    {
        public List<CategoryNode> Children = new List<CategoryNode>();
        public CategoryNode Parent { get; set; }
        public ProductCategory Source { get; set; }
    }
}
