using Hrevert.Common.Enums;

namespace HrevertCRM.Entities
{
    public class GeneralSetting : BaseEntity
    {

        //General Options
        public int Id { get; set; }
        public string SelectedTheme { get; set; }
        public string LogoUrl { get; set; } // Logo save garda, tesko Url rakhnu parne 
        public string StoreName { get; set; }
        public string FaviconLogoUrl { get; set; } // Save FaviconLogo Url
        //Index Options
        public bool EnableSlides { get; set; }
        public bool EnableTopCategories { get; set; }
        public bool EnableTopTrendingProducts { get; set; }
        public bool EnableHotThisWeek { get; set; }
        public bool EnableLatestProducts { get; set; }
        public bool EnableGetInspired { get; set; }
    }
}
