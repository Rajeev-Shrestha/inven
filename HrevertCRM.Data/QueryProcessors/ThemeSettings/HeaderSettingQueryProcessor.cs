using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Hrevert.Common.Security;
using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore;

namespace HrevertCRM.Data.QueryProcessors
{
    public class HeaderSettingQueryProcessor : QueryBase<HeaderSetting>, IHeaderSettingQueryProcessor
    {
        private readonly IHostingEnvironment _env;

        public HeaderSettingQueryProcessor(IUserSession userSession, IDbContext dbContext, IHostingEnvironment env)
            : base(userSession, dbContext)
        {
            _env = env;
        }

        public HeaderSetting Get(int headerSettingId)
        {
            var headerSetting = _dbContext.Set<HeaderSetting>().FirstOrDefault(x => x.Id == headerSettingId && x.CompanyId==LoggedInUser.CompanyId && x.Active);
            return headerSetting;
        }

        public HeaderSetting Save(HeaderSetting headerSetting)
        {
            headerSetting.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<HeaderSetting>().Add(headerSetting);
            _dbContext.SaveChanges();
            return headerSetting;
        }

        public HeaderSetting GetHeaderSetting()
        {
            var headerSetting = _dbContext.Set<HeaderSetting>().Include(x=>x.StoreLocators).FirstOrDefault(x => x.CompanyId == LoggedInUser.CompanyId && x.Active);
            return headerSetting;
        }

        public bool DeleteStoreLocator(int id)
        {
            var doc = _dbContext.Set<StoreLocator>().FirstOrDefault(x => x.Id == id);
            ValidateAuthorization(doc);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<StoreLocator>().Remove(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }

        public HeaderSetting Update(HeaderSetting headerSetting)
        {
            var original = Get(headerSetting.Id);
            ValidateAuthorization(original);
            CheckVersionMismatch(headerSetting, original);
            //pass value to original
            original.EnableOfferOfTheDay        = headerSetting.EnableOfferOfTheDay;
            original.OfferOfTheDayUrl           = headerSetting.OfferOfTheDayUrl;
            original.EnableStoreLocator         = headerSetting.EnableStoreLocator;
            original.NumberOfStores             = headerSetting.NumberOfStores;
            original.EnableWishlist             = headerSetting.EnableWishlist;
            original.EnableSocialLinks          = headerSetting.EnableSocialLinks;
            original.FacebookUrl                = headerSetting.FacebookUrl;
            original.TwitterUrl                 = headerSetting.TwitterUrl;
            original.InstagramUrl               = headerSetting.InstagramUrl;
            original.LinkedInUrl                = headerSetting.LinkedInUrl;
            original.TumblrUrl                  = headerSetting.TumblrUrl;
            original.RssUrl                     = headerSetting.RssUrl;
            _dbContext.Set<HeaderSetting>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
    }
}
