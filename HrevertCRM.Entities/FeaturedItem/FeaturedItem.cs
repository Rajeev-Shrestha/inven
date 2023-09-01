
using Hrevert.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrevertCRM.Entities
{
    public class FeaturedItem : BaseEntity, IWebItem
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public ImageType ImageType { get; set; }
        public bool SortOrder { get; set;}
        public bool WebActive { get; set; }
      
        public ICollection<FeaturedItemMetadata> FeaturedItemMetadatas { get; set; }
          
    }
}
