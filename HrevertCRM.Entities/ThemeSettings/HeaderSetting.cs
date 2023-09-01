using System.Collections.Generic;

namespace HrevertCRM.Entities
{
    public class HeaderSetting : BaseEntity
    {
        public int Id { get; set; }
        public bool EnableOfferOfTheDay { get; set; }
        public string OfferOfTheDayUrl { get; set; }

        public bool EnableStoreLocator { get; set; }
        public int NumberOfStores { get; set; }
        public virtual ICollection<StoreLocator> StoreLocators { get; set; }

        public bool EnableWishlist { get; set; }

        public bool EnableSocialLinks { get; set; }
        public string FacebookUrl { get; set; }
        public string TwitterUrl { get; set; }
        public string InstagramUrl { get; set; }
        public string LinkedInUrl { get; set; }
        public string TumblrUrl { get; set; }
        public string RssUrl { get; set; }

    }
}
