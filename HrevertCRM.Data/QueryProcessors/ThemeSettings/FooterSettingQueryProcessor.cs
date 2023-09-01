using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Hrevert.Common.Security;
using HrevertCRM.Data.ViewModels.ThemeSettings;
using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace HrevertCRM.Data.QueryProcessors
{
    public class FooterSettingQueryProcessor : QueryBase<FooterSetting>, IFooterSettingQueryProcessor
    {
        private readonly IHostingEnvironment _env;
        public FooterSettingQueryProcessor(IUserSession userSession, IDbContext dbContext,IHostingEnvironment env)
            : base(userSession, dbContext)
        {
            _env = env;
        }
        public FooterSetting ActivateFooterSetting(int footerSettingId)
        {
            var original = GetFooterSetting(footerSettingId);
            ValidateAuthorization(original);
            original.Active = true;
            _dbContext.Set<FooterSetting>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        public bool Delete(int footerSettingId)
        {
            var data = GetFooterSetting(footerSettingId);
            var result = 0;
            if (data == null) return result > 0;
            data.Active = false;
            _dbContext.Set<FooterSetting>().Update(data);
            result = _dbContext.SaveChanges();
            return result > 0;
        }

        public bool Exists(Expression<Func<FooterSetting, bool>> where)
        {
            return _dbContext.Set<FooterSetting>().Any(where);
        }

        public FooterSetting GetFooterSetting(int footerSettingId)
        {
            var footerSetting = _dbContext.Set<FooterSetting>()
                .Include(x => x.BrandImages).FirstOrDefault(x => x.Id == footerSettingId && x.CompanyId==LoggedInUser.CompanyId && x.Active);
            return footerSetting;
        }

        public FooterSetting Save(FooterSetting footerSetting)
        {
            footerSetting.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<FooterSetting>().Add(footerSetting);
            _dbContext.SaveChanges();
            return footerSetting;
        }

        public void SaveAll(List<FooterSetting> footerSettings)
        {
            _dbContext.Set<FooterSetting>().AddRange(footerSettings);
            _dbContext.SaveChanges();
        }

        public FooterSetting Update(FooterSetting footerSetting)
        {
            var original = GetFooterSetting(footerSetting.Id);
            ValidateAuthorization(original);
            CheckVersionMismatch(footerSetting, original);

            original.EnableFooterMenu = footerSetting.EnableFooterMenu;
            original.EnableBrands = footerSetting.EnableBrands;
            original.ShowFooterLogo = footerSetting.ShowFooterLogo;
            original.FooterLogoUrl = footerSetting.FooterLogoUrl;
            original.AboutStore = footerSetting.AboutStore;
            original.EnableAbout = footerSetting.EnableAbout;
            original.EnableContact = footerSetting.EnableContact;
            original.EnableStoreLocator = footerSetting.EnableStoreLocator;
            original.EnablePolicies = footerSetting.EnablePolicies;
            original.EnableFaq = footerSetting.EnableFaq;
            original.ShowUserLoginLink = footerSetting.ShowUserLoginLink;
            original.ShowOrderHistoryLink = footerSetting.ShowOrderHistoryLink;
            original.ShowWishlistLink = footerSetting.ShowWishlistLink;
            original.ShowTrendingOrLastest = footerSetting.ShowTrendingOrLastest;
            original.WhereToFindUs = footerSetting.WhereToFindUs;
            original.EnableNewsLetter = footerSetting.EnableNewsLetter;
            original.EnableCopyright = footerSetting.EnableCopyright;
            original.CopyrightText = footerSetting.CopyrightText;
            original.Active = footerSetting.Active;
            original.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<FooterSetting>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        public FooterSetting Get()
        {
            var result= _dbContext.Set<FooterSetting>().Include(x=>x.BrandImages).FirstOrDefault(x => x.CompanyId == LoggedInUser.CompanyId && x.Active);
            return result;
        }

        public List<string> GetBrandImageUrls(int footerSettingId)
        {
            return _dbContext.Set<BrandImage>().Where(x => x.FooterSettingId == footerSettingId && x.CompanyId == LoggedInUser.CompanyId && x.Active).Select(x => x.ImageUrl).ToList();
        }

        public string GetFooterImagePath(int footerSettingId)
        {
            return _dbContext.Set<FooterSetting>().FirstOrDefault(x => x.Id == footerSettingId && x.CompanyId==LoggedInUser.CompanyId && x.Active).FooterLogoUrl;
        }

        public bool DeleteImage(FooterSettingViewModel footerSettingViewModel, string imageUrl)
        {
                var mainUrl = Path.Combine(_env.WebRootPath, imageUrl);
                File.Delete(mainUrl);
                var doc = GetFooterSetting((int)footerSettingViewModel.Id, imageUrl);
                if (doc != null)
                    doc.FooterLogoUrl = null;
                _dbContext.Set<FooterSetting>().Update(doc);
                    _dbContext.SaveChanges();
            return true;
        }
        private FooterSetting GetFooterSetting(int footerSettingId, string imageUrl)
        {
           return _dbContext.Set<FooterSetting>().FirstOrDefault(x => x.Id == footerSettingId && x.FooterLogoUrl == imageUrl && x.CompanyId == LoggedInUser.CompanyId && x.Active);
        }

        public bool DeleteBrandImage(int footerSettingId, string imageUrl)
        {
            var imgUrl = @"companyMM/ThemeSettingImages/Brand/" + LoggedInUser.CompanyId + "/"+ imageUrl;
            var data = _dbContext.Set<BrandImage>().FirstOrDefault(x => x.FooterSettingId == footerSettingId && x.ImageUrl == imgUrl && x.CompanyId == LoggedInUser.CompanyId);
            var result = 0;
            if (data == null) return result > 0;
            var mainUrl = Path.Combine(_env.WebRootPath, data.ImageUrl);
            File.Delete(mainUrl);
            _dbContext.Set<BrandImage>().Remove(data);
            result = _dbContext.SaveChanges();
            return result > 0;
        }

        public bool RemoveFooterLogo(int footerSettingId)
        {
            var original = GetFooterSetting(footerSettingId);
            ValidateAuthorization(original);
            var result = 0;
            if (original == null) return result > 0;
            var mainUrl = Path.Combine(_env.WebRootPath, original.FooterLogoUrl);
            File.Delete(mainUrl);
            original.FooterLogoUrl = null;
            _dbContext.Set<FooterSetting>().Update(original);
            result = _dbContext.SaveChanges();
            return result > 0;
        }
    }
}
