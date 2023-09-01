using System.Collections.Generic;
using Hrevert.Common.Enums;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.ViewModels.ThemeSettings
{
    public class LayoutSettingViewModel
    {
        public int? Id { get; set; }
        public bool ShowLayoutTitle { get; set; }

        public int? CategoryOne { get; set; }
        public int? CategoryTwo { get; set; }
        public int? CategoryThree { get; set; }
        public int? CategoryFour { get; set; }

        public bool ShowTrendingItemsLayoutTitle { get; set; }
        public TitleStyle TrendingItemsTitleStyle { get; set; }
        public string TrendingItemsImageUrl { get; set; }
        public ThemeImage TrendingItemsImage { get; set; }
        public string TrendingItemsColor { get; set; }

        public bool ShowHotThisWeekLayoutTitle { get; set; }
        public TitleStyle HotThisWeekTitleStyle { get; set; }
        public string HotThisWeekImageUrl { get; set; }
        public ThemeImage HotThisWeekImage { get; set; }
        public string HotThisWeekColor { get; set; }

        public bool ShowLatestProductsLayoutTitle { get; set; }
        public TitleStyle LatestProductsTitleStyle { get; set; }
        public string LatestProductsImageUrl { get; set; }
        public ThemeImage LatestProductsImage { get; set; }
        public string LatestProductsColor { get; set; }

        public BackgroundImageOrColor BackgroundImageOrColor { get; set; }
        public string BackgroundImageUrl { get; set; }
        public ThemeImage BackgroundImage { get; set; }
        public string ColorCode { get; set; }

        public virtual List<PersonnelSettingViewModel> PersonnelSettingViewModels { get; set; }

        public bool EnableSeparator { get; set; }
        public string HotThisWeekSeparatorUrl { get; set; }
        public ThemeImage HotThisWeekSeparatorImage { get; set; }

        public string TrendingItemsSeparatorUrl { get; set; }
        public ThemeImage TrendingItemsSeparatorImage { get; set; }

        public string LatestProductsSeparatorUrl { get; set; }
        public ThemeImage LatestProductsSeparatorImage { get; set; }

        public int CompanyId { get; set; }
        public bool Active { get; set; }
        public byte[] Version { get; set; }
    }
}
