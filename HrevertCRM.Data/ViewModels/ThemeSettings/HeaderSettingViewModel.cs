using System.Collections.Generic;

namespace HrevertCRM.Data.ViewModels.ThemeSettings
{
    public class HeaderSettingViewModel
    {
        public int? Id { get; set; }
        public bool EnableOfferOfTheDay { get; set; }
        public string OfferOfTheDayUrl { get; set; }

        public bool EnableStoreLocator { get; set; }
        public int? NumberOfStores { get; set; }
        public List<StoreLocatorViewModel> StoreLocatorViewModels { get; set; }

        public bool EnableWishlist { get; set; }
        public bool EnableSocialLinks { get; set; }
        public string FacebookUrl { get; set; }
        public string TwitterUrl { get; set; }
        public string InstagramUrl { get; set; }
        public string LinkedInUrl { get; set; }
        public string TumblrUrl { get; set; }
        public string RssUrl { get; set; }

        public int CompanyId { get; set; }
        public bool Active { get; set; }
        public byte[] Version { get; set; }
    }
}
