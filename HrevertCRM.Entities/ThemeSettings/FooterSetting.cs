using System.Collections.Generic;
using Hrevert.Common.Enums;

namespace HrevertCRM.Entities
{
    public class FooterSetting : BaseEntity
    {
        public int Id { get; set; }
        public bool EnableFooterMenu { get; set; }
        public bool EnableBrands { get; set; }
        //public virtual ICollection<string> ThemeBrandImageUrls { get; set; } // Send Brand Image 
        public virtual ICollection<BrandImage> BrandImages { get; set; } // retrieve from db 

        //Section 1
        public bool ShowFooterLogo { get; set; }
        public string FooterLogoUrl { get; set; }
        public string AboutStore { get; set; }

        //Section 2
        //Store Information
        //Use this section to insert links to pages relevant to your store
        public bool EnableAbout { get; set; }
        public bool EnableContact { get; set; }
        public bool EnableStoreLocator { get; set; }
        public bool EnablePolicies { get; set; }
        public bool EnableFaq { get; set; }

        //User Info
        //Use this section to insert links to pages relevant to users of the store
        public bool ShowUserLoginLink { get; set; }
        public bool ShowOrderHistoryLink { get; set; }
        public bool ShowWishlistLink { get; set; }

        //Section 3
        public ShowTrendingOrLastest ShowTrendingOrLastest { get; set; }

        //Section 4
        public string WhereToFindUs { get; set; }
        public bool EnableNewsLetter { get; set; }

        //Copyright Section
        public bool EnableCopyright { get; set; }
        public string CopyrightText { get; set; }
    }
}
