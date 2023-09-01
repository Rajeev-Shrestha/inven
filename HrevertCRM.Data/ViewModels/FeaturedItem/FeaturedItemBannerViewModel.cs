using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrevertCRM.Data.ViewModels
{
    public class FeaturedItemBannerViewModel
    {
        public int Id { get; set; }

        public List<string> FullWidthUrls {get;set;}

        public List<string> HalfWidthUrls { get; set; }

        public List<string> QuaterWidthUrls { get; set; }
        public List<string> BannerImageUrls { get; set; }
    }
}
