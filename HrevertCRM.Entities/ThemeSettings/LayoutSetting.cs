using System.Collections.Generic;
using Hrevert.Common.Enums;

namespace HrevertCRM.Entities
{
    public class LayoutSetting : BaseEntity
    {
        public int Id { get; set; }

        public bool ShowLayoutTitle { get; set; }

        //Index Page Options
        //Configure Your Index Page Here

        //Top categories (Select Top 4 Categories)
        public int CategoryOne { get; set; }
        public int CategoryTwo { get; set; }
        public int CategoryThree { get; set; }
        public int CategoryFour { get; set; }

        //Trending Items
        public bool ShowTrendingItemsLayoutTitle { get; set; }
        public TitleStyle TrendingItemsTitleStyle { get; set; }
        public string TrendingItemsImageUrl { get; set; }
        public string TrendingItemsColor { get; set; }

        //Hot This Week
        public bool ShowHotThisWeekLayoutTitle { get; set; }
        public TitleStyle HotThisWeekTitleStyle { get; set; }
        public string HotThisWeekImageUrl { get; set; }
        public string HotThisWeekColor { get; set; }

        //Latest Products
        public bool ShowLatestProductsLayoutTitle { get; set; }
        public TitleStyle LatestProductsTitleStyle { get; set; }
        public string LatestProductsImageUrl { get; set; }
        public string LatestProductsColor { get; set; }

        //Recommendation(s)
        //Get Inspired

        //Background
        public BackgroundImageOrColor BackgroundImageOrColor { get; set; }
        public string BackgroundImageUrl { get; set; }
        public string ColorCode { get; set; }

        //Personnel Settings
        public virtual ICollection<PersonnelSetting> PersonnelSettings { get; set; }

        //Layout Separators
        public bool EnableSeparator { get; set; }
        public string HotThisWeekSeparatorUrl { get; set; }
        public string TrendingItemsSeparatorUrl { get; set; }
        public string LatestProductsSeparatorUrl { get; set; }
    }
}
