using System.Linq;
using HrevertCRM.Data.ViewModels.ThemeSettings;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper.ThemeSettings
{
    public class HeaderSettingToHeaderSettingViewModelMapper : MapperBase<HeaderSetting, HeaderSettingViewModel>
    {
        public override HeaderSetting Map(HeaderSettingViewModel viewModel)
        {
            var headerSetting = new HeaderSetting
            {
                Id = viewModel.Id ?? 0,
                EnableOfferOfTheDay = viewModel.EnableOfferOfTheDay,
                OfferOfTheDayUrl = viewModel.OfferOfTheDayUrl,
                
                EnableStoreLocator = viewModel.EnableStoreLocator,
                NumberOfStores = viewModel.NumberOfStores ?? 0,

                EnableWishlist = viewModel.EnableWishlist,
                EnableSocialLinks = viewModel.EnableSocialLinks,
                FacebookUrl = viewModel.FacebookUrl,
                TwitterUrl = viewModel.TwitterUrl,
                InstagramUrl = viewModel.InstagramUrl,
                LinkedInUrl = viewModel.LinkedInUrl,
                TumblrUrl = viewModel.TumblrUrl,
                RssUrl = viewModel.RssUrl,

                CompanyId = viewModel.CompanyId,
                Active = viewModel.Active,
                Version = viewModel.Version
            };
            if (viewModel.StoreLocatorViewModels == null || viewModel.StoreLocatorViewModels.Count <= 0) return headerSetting;
            var mapper = new StoreLocatorToStoreLocatorViewModelMapper();
            headerSetting.StoreLocators = viewModel.StoreLocatorViewModels.Select(o => mapper.Map(o)).ToList();
            return headerSetting;
        }

        public override HeaderSettingViewModel Map(HeaderSetting entity)
        {
            var headerSettingVm = new HeaderSettingViewModel
            {
                Id = entity.Id,
                EnableOfferOfTheDay = entity.EnableOfferOfTheDay,
                OfferOfTheDayUrl = entity.OfferOfTheDayUrl,

                EnableStoreLocator = entity.EnableStoreLocator,
                NumberOfStores = entity.NumberOfStores,

                EnableWishlist = entity.EnableWishlist,
                EnableSocialLinks = entity.EnableSocialLinks,
                FacebookUrl = entity.FacebookUrl,
                TwitterUrl = entity.TwitterUrl,
                InstagramUrl = entity.InstagramUrl,
                LinkedInUrl = entity.LinkedInUrl,
                TumblrUrl = entity.TumblrUrl,
                RssUrl = entity.RssUrl,

                CompanyId = entity.CompanyId,
                Active = entity.Active,
                Version = entity.Version
            };
            if (entity.StoreLocators == null || entity.StoreLocators.Count <= 0) return headerSettingVm;
            var mapper = new StoreLocatorToStoreLocatorViewModelMapper();
            headerSettingVm.StoreLocatorViewModels = entity.StoreLocators.Select(o => mapper.Map(o)).ToList();
            return headerSettingVm;
        }
    }
}

