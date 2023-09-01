using System.Collections.Generic;
using Hrevert.Common.Enums;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.ViewModels.ThemeSettings
{
    public class FooterSettingViewModel
    {
        public int? Id { get; set; }

        #region Brand Region

        public bool EnableFooterMenu { get; set; }
        public bool EnableBrands { get; set; }
        public virtual ICollection<ThemeImage> ThemeBrandImages { get; set; }
        public virtual ICollection<string> ThemeBrandImageUrls { get; set; }

        #endregion

        public bool ShowFooterLogo { get; set; }
        public string FooterLogoUrl { get; set; }
        public ThemeImage FooterImage { get; set; }
        public string AboutStore { get; set; }

        public bool EnableAbout { get; set; }
        public bool EnableContact { get; set; }
        public bool EnableStoreLocator { get; set; }
        public bool EnablePolicies { get; set; }
        public bool EnableFaq { get; set; }

        public bool ShowUserLoginLink { get; set; }
        public bool ShowOrderHistoryLink { get; set; }
        public bool ShowWishlistLink { get; set; }

        public ShowTrendingOrLastest ShowTrendingOrLastest { get; set; }

        public string WhereToFindUs { get; set; }
        public bool EnableNewsLetter { get; set; }

        public bool EnableCopyright { get; set; }
        public string CopyrightText { get; set; }

        public int CompanyId { get; set; }
        public bool Active { get; set; }
        public byte[] Version { get; set; }       
    }
}
