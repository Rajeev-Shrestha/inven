using HrevertCRM.Data.ViewModels.ThemeSettings;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper.ThemeSettings
{
    public class IndividualSlideSettingToIndividualSlideSettingViewModelMapper : MapperBase<IndividualSlideSetting, IndividualSlideSettingViewModel>
    {
        public override IndividualSlideSetting Map(IndividualSlideSettingViewModel viewModel)
        {
            return new IndividualSlideSetting
            {
                Id = viewModel.Id ?? 0,
                SlideBackgroundImageUrl = viewModel.SlideBackgroundImageUrl,
                ColorCode = viewModel.ColorCode,

                SlideImageUrl = viewModel.SlideImageUrl,
                LimitedTimeOfferText = viewModel.LimitedTimeOfferText,
                IsExpires = viewModel.IsExpires,
                ExpireDate = viewModel.ExpireDate,
                SalesPrice = viewModel.SalesPrice,
                OriginalPriceCheck = viewModel.OriginalPriceCheck,
                OriginalPrice = viewModel.OriginalPrice,
                DiscountPercentageCheck = viewModel.DiscountPercentageCheck,
                DiscountPercentage = viewModel.DiscountPercentage,
                ProductType = viewModel.ProductType,
                ExploreToLinkPage = viewModel.ExploreToLinkPage,
                ShowAddToCartOption = viewModel.ShowAddToCartOption,
                ShowAddToListOption = viewModel.ShowAddToListOption,
                EnableFreeShipping = viewModel.EnableFreeShipping,
                SlideBackground = viewModel.SlideBackground,
                SlideSettingId = viewModel.SlideSettingId,

                CompanyId = viewModel.CompanyId,
                Active = viewModel.Active,
                Version = viewModel.Version
            };
        }

        public override IndividualSlideSettingViewModel Map(IndividualSlideSetting entity)
        {
            return new IndividualSlideSettingViewModel
            {
                Id = entity.Id,
                SlideBackgroundImageUrl = entity.SlideBackgroundImageUrl,
                ColorCode = entity.ColorCode,

                SlideImageUrl = entity.SlideImageUrl,
                LimitedTimeOfferText = entity.LimitedTimeOfferText,
                IsExpires = entity.IsExpires,
                ExpireDate = entity.ExpireDate,
                SalesPrice = entity.SalesPrice,
                OriginalPriceCheck = entity.OriginalPriceCheck,
                OriginalPrice = entity.OriginalPrice,
                DiscountPercentageCheck = entity.DiscountPercentageCheck,
                DiscountPercentage = entity.DiscountPercentage,
                ProductType=entity.ProductType,
                ExploreToLinkPage = entity.ExploreToLinkPage,
                ShowAddToCartOption = entity.ShowAddToCartOption,
                ShowAddToListOption = entity.ShowAddToListOption,
                EnableFreeShipping = entity.EnableFreeShipping,
                
                SlideSettingId = entity.SlideSettingId,
                SlideBackground = entity.SlideBackground,
                CompanyId = entity.CompanyId,
                Active = entity.Active,
                Version = entity.Version
            };
        }
    }
}
