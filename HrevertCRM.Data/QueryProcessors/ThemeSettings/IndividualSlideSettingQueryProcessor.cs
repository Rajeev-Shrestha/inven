using System.Collections.Generic;
using Hrevert.Common.Security;
using HrevertCRM.Entities;
using Microsoft.AspNetCore.Hosting;
using System.Linq;

namespace HrevertCRM.Data.QueryProcessors
{
    public class IndividualSlideSettingQueryProcessor : QueryBase<IndividualSlideSetting>, IIndividualSlideSettingQueryProcessor
    {
        private readonly IHostingEnvironment _env;

        public IndividualSlideSettingQueryProcessor(IUserSession userSession, IDbContext dbContext, IHostingEnvironment env)
            : base(userSession, dbContext)
        {
            _env = env;
        }

        public void UpdateIndividualSlideSettings(List<IndividualSlideSetting> individualSlideSettings)
        {
            throw new System.NotImplementedException();
        }

        public IndividualSlideSetting GetIndividualSlideSetting(int individualSlideSettingId)
        {
            var individualSlideSetting = _dbContext.Set<IndividualSlideSetting>().FirstOrDefault(x => x.Id == individualSlideSettingId);
            return individualSlideSetting;
        }

        public IndividualSlideSetting Save(IndividualSlideSetting individualSlideSetting)
        {
            _dbContext.Set<IndividualSlideSetting>().Add(individualSlideSetting);
            _dbContext.SaveChanges();
            return individualSlideSetting;
        }

        public void SaveIndividualSlideSettings(List<IndividualSlideSetting> individualSlideSettings)
        {
            individualSlideSettings.ForEach(x=>x.CompanyId=LoggedInUser.CompanyId);
            individualSlideSettings.ForEach(x => x.Active = true);
            _dbContext.Set<IndividualSlideSetting>().AddRange(individualSlideSettings);
            _dbContext.SaveChanges();
        }

        public IndividualSlideSetting Update(IndividualSlideSetting individualSlideSetting)
        {
            var original = GetIndividualSlideSetting(individualSlideSetting.Id);
            ValidateAuthorization(original);
            CheckVersionMismatch(individualSlideSetting, original);
            //pass value to original
            original.SlideBackgroundImageUrl = individualSlideSetting.SlideBackgroundImageUrl;
            original.ColorCode = individualSlideSetting.ColorCode;
            original.SlideImageUrl = individualSlideSetting.SlideImageUrl;
            original.LimitedTimeOfferText = individualSlideSetting.LimitedTimeOfferText;
            original.IsExpires = individualSlideSetting.IsExpires;
            original.ExpireDate = individualSlideSetting.ExpireDate;
            original.SalesPrice = individualSlideSetting.SalesPrice;
            original.OriginalPrice = individualSlideSetting.OriginalPrice;
            original.DiscountPercentage = individualSlideSetting.DiscountPercentage;
            original.ExploreToLinkPage = individualSlideSetting.ExploreToLinkPage;
            original.ShowAddToCartOption = individualSlideSetting.ShowAddToCartOption;
            original.ShowAddToListOption = individualSlideSetting.ShowAddToListOption;
            original.EnableFreeShipping = individualSlideSetting.EnableFreeShipping;
            original.SlideSettingId = individualSlideSetting.SlideSettingId;
            original.SlideBackground = individualSlideSetting.SlideBackground;
            original.OriginalPriceCheck = individualSlideSetting.OriginalPriceCheck;
            original.DiscountPercentageCheck = individualSlideSetting.DiscountPercentageCheck;
            original.ProductType = individualSlideSetting.ProductType;
            _dbContext.Set<IndividualSlideSetting>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
    }
}

