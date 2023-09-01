using System;
using Hrevert.Common.Enums;

namespace HrevertCRM.Entities
{
    public class IndividualSlideSetting : BaseEntity
    {
        public int Id { get; set; }
        
        //Slide Configuration
        //Background
        public string SlideBackgroundImageUrl { get; set; }
        public string ColorCode { get; set; }
        public SlideBackground SlideBackground { get; set; }

        //Slide Setting
        public string SlideImageUrl { get; set; }
        public string LimitedTimeOfferText { get; set; }
        public bool IsExpires { get; set; }
        public DateTime ExpireDate { get; set; }
        public double SalesPrice { get; set; }
        public bool OriginalPriceCheck { get; set; }
        public double OriginalPrice { get; set; }
        public bool DiscountPercentageCheck { get; set; }
        public double DiscountPercentage { get; set; }
        public int ProductType { get; set; }
        public string ExploreToLinkPage { get; set; }
        public bool ShowAddToCartOption { get; set; }
        public bool ShowAddToListOption { get; set; }
        public bool EnableFreeShipping { get; set; }
        public int SlideSettingId { get; set; }
        public SlideSetting SlideSetting { get; set; }
    }
}