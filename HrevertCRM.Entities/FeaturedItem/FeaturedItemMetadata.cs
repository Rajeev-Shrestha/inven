using Hrevert.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HrevertCRM.Entities
{
    public class FeaturedItemMetadata : BaseEntity, IWebItem
    {
        public int Id { get; set; }
        public int FeaturedItemId { get; set; }
        public int ItemId { get; set; }
        public MediaType MediaType { get; set; }
        public string MediaUrl { get; set; }
        public bool WebActive { get; set; }
        public FeaturedItem FeaturedItem { get; set; }
        public ImageType ImageType { get; set; }
    }
}
