using Hrevert.Common.Enums;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.ViewModels.ThemeSettings
{
    public class GeneralSettingViewModel
    {
        public int? Id { get; set; }
        public string SelectedTheme { get; set; }
        public virtual string LogoUrl { get; set; }
        public virtual ThemeImage LogoImage { get; set; }
        public string StoreName { get; set; }
        public virtual string FaviconLogoUrl { get; set; }
        public virtual ThemeImage FaviconLogoImage { get; set; }

        public bool EnableSlides { get; set; }
        public bool EnableTopCategories { get; set; }
        public bool EnableTopTrendingProducts { get; set; }
        public bool EnableHotThisWeek { get; set; }
        public bool EnableLatestProducts { get; set; }
        public bool EnableGetInspired { get; set; }

        public int CompanyId { get; set; }
        public bool Active { get; set; }
        public byte[] Version { get; set; }
    }
}
