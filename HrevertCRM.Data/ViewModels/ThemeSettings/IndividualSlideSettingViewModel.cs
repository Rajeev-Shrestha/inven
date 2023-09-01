using System;
using Hrevert.Common.Enums;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.ViewModels.ThemeSettings
{
    public class IndividualSlideSettingViewModel
    {
        public int? Id { get; set; }
        public SlideBackground SlideBackground { get; set; }
        public string SlideBackgroundImageUrl { get; set; }
        public ThemeImage SlideBackgroundImage { get; set; }
        public string ColorCode { get; set; }

        public string SlideImageUrl { get; set; }
        public ThemeImage SlideImage { get; set; }
        public string LimitedTimeOfferText { get; set; }
        public bool IsExpires { get; set; }
        public DateTime ExpireDate { get; set; }
        public double SalesPrice { get; set; }
        public bool OriginalPriceCheck { get; set; }
        public double OriginalPrice { get; set; }
        public bool DiscountPercentageCheck { get; set; }
        public double DiscountPercentage { get; set; }

        public string ExploreToLinkPage { get; set; }
        public bool ShowAddToCartOption { get; set; }
        public bool ShowAddToListOption { get; set; }
        public bool EnableFreeShipping { get; set; }
        public int ProductType { get; set; }

        public int SlideSettingId { get; set; }

        public int CompanyId { get; set; }
        public bool Active { get; set; }
        public byte[] Version { get; set; }
    }
}
