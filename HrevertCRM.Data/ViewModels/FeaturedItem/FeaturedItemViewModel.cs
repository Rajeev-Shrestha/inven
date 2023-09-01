using Hrevert.Common.Enums;
using HrevertCRM.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HrevertCRM.Data.ViewModels
{
    public class FeaturedItemViewModel: IWebItem
    {
        public int? Id { get; set; }
        public ImageType ImageType { get; set; }
        public bool SortOrder { get; set; }
        public bool WebActive { get; set; }
        public byte [] Version { get; set; }
        public bool Active { get; set; }
        public int CompanyId { get; set; }
        public List<Image> BannerImage { get; set; }
        [Required]
        public int ItemId { get; set; }
        public string ItemName { get; set; }  
        public List<string> FullWidthImageUrls { get; set; }
        public List<string> HalfWidthImageUrls { get; set; }
        public List<string> QuaterWidthImageUrls { get; set; }
        
    }
}
