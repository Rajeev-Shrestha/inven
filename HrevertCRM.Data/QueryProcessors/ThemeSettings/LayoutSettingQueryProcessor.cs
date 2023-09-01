using System;
using System.Linq;
using HrevertCRM.Entities;
using Microsoft.AspNetCore.Hosting;
using Hrevert.Common.Security;
using System.IO;
using Hrevert.Common.Enums;
using HrevertCRM.Data.Mapper.ThemeSettings;
using HrevertCRM.Data.ViewModels.ThemeSettings;
using Microsoft.EntityFrameworkCore;

namespace HrevertCRM.Data.QueryProcessors
{
    public class LayoutSettingQueryProcessor : QueryBase<LayoutSetting>, ILayoutSettingQueryProcessor
    {
        private readonly IHostingEnvironment _env;
        public LayoutSettingQueryProcessor(IUserSession userSession, IDbContext dbContext, IHostingEnvironment env)
            : base(userSession, dbContext)
        {
            _env = env;
        }
        public LayoutSetting Get(int getLayoutSettingId)
        {
            var layoutSetting = _dbContext.Set<LayoutSetting>()
                .Include(x => x.PersonnelSettings).FirstOrDefault(x => x.Id == getLayoutSettingId && x.CompanyId==LoggedInUser.CompanyId && x.Active);
            return layoutSetting;
        }
        public LayoutSetting Save(LayoutSetting layoutSetting)
        {
            layoutSetting.CompanyId = LoggedInUser.CompanyId;
            layoutSetting.Active = true;
            _dbContext.Set<LayoutSetting>().Add(layoutSetting);
            _dbContext.SaveChanges();
            return layoutSetting;
        }

        public LayoutSetting Update(LayoutSetting layoutSetting)
        {
            var original = Get(layoutSetting.Id);
            ValidateAuthorization(original);
            CheckVersionMismatch(layoutSetting, original);
            //pass value to original
            original.ShowLayoutTitle = layoutSetting.ShowLayoutTitle;
            original.CategoryOne = layoutSetting.CategoryOne;
            original.CategoryTwo = layoutSetting.CategoryTwo;
            original.CategoryThree = layoutSetting.CategoryThree;
            original.CategoryFour = layoutSetting.CategoryFour;
            original.ShowTrendingItemsLayoutTitle = layoutSetting.ShowTrendingItemsLayoutTitle;
            original.TrendingItemsTitleStyle = layoutSetting.TrendingItemsTitleStyle;
            original.TrendingItemsImageUrl = layoutSetting.TrendingItemsImageUrl;
            original.TrendingItemsColor = layoutSetting.TrendingItemsColor;
            original.ShowHotThisWeekLayoutTitle = layoutSetting.ShowHotThisWeekLayoutTitle;
            original.HotThisWeekTitleStyle = layoutSetting.HotThisWeekTitleStyle;
            original.HotThisWeekImageUrl = layoutSetting.HotThisWeekImageUrl;
            original.ShowLatestProductsLayoutTitle = layoutSetting.ShowLatestProductsLayoutTitle;
            original.LatestProductsTitleStyle = layoutSetting.LatestProductsTitleStyle;
            original.LatestProductsImageUrl = layoutSetting.LatestProductsImageUrl;
            original.BackgroundImageOrColor = layoutSetting.BackgroundImageOrColor;
            original.BackgroundImageUrl = layoutSetting.BackgroundImageUrl;
            original.ColorCode = layoutSetting.ColorCode;
            original.EnableSeparator = layoutSetting.EnableSeparator;
            original.HotThisWeekSeparatorUrl = layoutSetting.HotThisWeekSeparatorUrl;
            original.LatestProductsSeparatorUrl = layoutSetting.LatestProductsSeparatorUrl;
            original.TrendingItemsSeparatorUrl = layoutSetting.TrendingItemsSeparatorUrl;

            original.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<LayoutSetting>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        public string GetImageUrl(int layoutSettingId, ThemeSettingImageType img)
        {
            switch (img)
            {
                case ThemeSettingImageType.BackgroundImage:
                    return _dbContext.Set<LayoutSetting>().FirstOrDefault(x => x.Id == layoutSettingId && x.CompanyId==LoggedInUser.CompanyId && x.Active).BackgroundImageUrl;
                case ThemeSettingImageType.HotThisWeekImage:
                    return _dbContext.Set<LayoutSetting>().FirstOrDefault(x => x.Id == layoutSettingId && x.CompanyId == LoggedInUser.CompanyId && x.Active).HotThisWeekImageUrl;
                case ThemeSettingImageType.LatestProductsImage:
                    return _dbContext.Set<LayoutSetting>().FirstOrDefault(x => x.Id == layoutSettingId && x.CompanyId == LoggedInUser.CompanyId && x.Active).LatestProductsImageUrl;
                case ThemeSettingImageType.HotThisWeekSeparator:
                    return _dbContext.Set<LayoutSetting>().FirstOrDefault(x => x.Id == layoutSettingId && x.CompanyId == LoggedInUser.CompanyId && x.Active).HotThisWeekSeparatorUrl;
                case ThemeSettingImageType.LatestProductsSeparator:
                    return _dbContext.Set<LayoutSetting>().FirstOrDefault(x => x.Id == layoutSettingId && x.CompanyId == LoggedInUser.CompanyId && x.Active).LatestProductsSeparatorUrl;
                case ThemeSettingImageType.TrendingItemsSeparator:
                    return _dbContext.Set<LayoutSetting>().FirstOrDefault(x => x.Id == layoutSettingId && x.CompanyId == LoggedInUser.CompanyId && x.Active).TrendingItemsSeparatorUrl;
                case ThemeSettingImageType.TrendingItemsImage:
                    return _dbContext.Set<LayoutSetting>().FirstOrDefault(x => x.Id == layoutSettingId && x.CompanyId == LoggedInUser.CompanyId && x.Active).TrendingItemsImageUrl;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public bool DeleteImage(LayoutSettingViewModel layoutSettingViewModel, string imageUrl, ThemeSettingImageType img)
        {
            var layoutSettingMapper = new LayoutSettingToLayoutSettingViewModelMapper();
            var layoutSetting = layoutSettingMapper.Map(layoutSettingViewModel);
            string mainUrl;
            switch (img)
            {
                case ThemeSettingImageType.BackgroundImage:
                    mainUrl = Path.Combine(_env.WebRootPath, imageUrl);
                    File.Delete(mainUrl);
                    var doc1 = GetImageData(layoutSetting.Id, imageUrl, ThemeSettingImageType.BackgroundImage);
                    if (doc1 != null)
                    {
                        doc1.BackgroundImageUrl = null;
                        _dbContext.Set<LayoutSetting>().Update(doc1);
                    }
                    _dbContext.SaveChanges();
                    return true;
                case ThemeSettingImageType.TrendingItemsImage:
                    mainUrl = Path.Combine(_env.WebRootPath, imageUrl);
                    File.Delete(mainUrl);
                    var doc2 = GetImageData(layoutSetting.Id, imageUrl, ThemeSettingImageType.TrendingItemsImage);
                    if (doc2 != null)
                    {
                        doc2.TrendingItemsImageUrl = null;
                        _dbContext.Set<LayoutSetting>().Update(doc2);
                    }
                    _dbContext.SaveChanges();
                    return true;
                case ThemeSettingImageType.HotThisWeekImage:
                    mainUrl = Path.Combine(_env.WebRootPath, imageUrl);
                    File.Delete(mainUrl);
                    var doc3 = GetImageData(layoutSetting.Id, imageUrl, ThemeSettingImageType.HotThisWeekImage);
                    if (doc3 != null)
                    {
                        doc3.HotThisWeekImageUrl = null;
                        _dbContext.Set<LayoutSetting>().Update(doc3);
                    }
                    _dbContext.SaveChanges();
                    return true;
                case ThemeSettingImageType.LatestProductsImage:
                    mainUrl = Path.Combine(_env.WebRootPath, imageUrl);
                    File.Delete(mainUrl);
                    var doc4 = GetImageData(layoutSetting.Id, imageUrl, ThemeSettingImageType.LatestProductsImage);
                    if (doc4 != null)
                    {
                        doc4.LatestProductsImageUrl = null;
                        _dbContext.Set<LayoutSetting>().Update(doc4);
                    }
                    _dbContext.SaveChanges();
                    return true;
                case ThemeSettingImageType.HotThisWeekSeparator:
                    mainUrl = Path.Combine(_env.WebRootPath, imageUrl);
                    File.Delete(mainUrl);
                    var doc6 = GetImageData(layoutSetting.Id, imageUrl, ThemeSettingImageType.HotThisWeekSeparator);
                    if (doc6 != null)
                    {
                        doc6.HotThisWeekSeparatorUrl = null;
                        _dbContext.Set<LayoutSetting>().Update(doc6);
                    }
                    _dbContext.SaveChanges();
                    return true;
                case ThemeSettingImageType.LatestProductsSeparator:
                    mainUrl = Path.Combine(_env.WebRootPath, imageUrl);
                    File.Delete(mainUrl);
                    var doc7 = GetImageData(layoutSetting.Id, imageUrl, ThemeSettingImageType.LatestProductsSeparator);
                    if (doc7 != null)
                    {
                        doc7.LatestProductsSeparatorUrl = null;
                        _dbContext.Set<LayoutSetting>().Update(doc7);
                    }
                    _dbContext.SaveChanges();
                    return true;
                case ThemeSettingImageType.TrendingItemsSeparator:
                    mainUrl = Path.Combine(_env.WebRootPath, imageUrl);
                    File.Delete(mainUrl);
                    var doc8 = GetImageData(layoutSetting.Id, imageUrl, ThemeSettingImageType.TrendingItemsSeparator);
                    if (doc8 != null)
                    {
                        doc8.TrendingItemsSeparatorUrl = null;
                        _dbContext.Set<LayoutSetting>().Update(doc8);
                    }
                    _dbContext.SaveChanges();
                    return true;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public PersonnelSetting SavePersonnelSetting(PersonnelSetting personnelSetting)
        {
            personnelSetting.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<PersonnelSetting>().Add(personnelSetting);
            _dbContext.SaveChanges();
            return personnelSetting;
        }

        public LayoutSetting GetLayoutSetting()
        {
            var layoutSetting = _dbContext.Set<LayoutSetting>()
                .Include(x => x.PersonnelSettings).FirstOrDefault(x => x.CompanyId == LoggedInUser.CompanyId && x.Active);
            return layoutSetting;
        }

        public bool RemoveLayoutImage(string imageUrl, int layoutId)
        {
            var original = _dbContext.Set<LayoutSetting>()
                .FirstOrDefault(x => x.CompanyId == LoggedInUser.CompanyId && x.Id == layoutId && x.Active);
            const int result = 0;
            if (original == null) return result > 0;
            ValidateAuthorization(original);
            return DeleteLayoutImage(imageUrl, original);
        }

        private bool DeleteLayoutImage(string imageUrl, LayoutSetting original)
        {
            var result = 0;
            if (original.BackgroundImageUrl != null)
            {
                var imageName = original.BackgroundImageUrl.Substring(original.BackgroundImageUrl.LastIndexOf('/') + 1);
                if (imageUrl == imageName)
                {
                    var mainUrl = Path.Combine(_env.WebRootPath, original.BackgroundImageUrl);
                    File.Delete(mainUrl);
                    original.BackgroundImageUrl = null;
                    _dbContext.Set<LayoutSetting>().Update(original);
                    result = _dbContext.SaveChanges();
                }
            }
            if (original.HotThisWeekImageUrl != null)
            {
                var imageName = original.HotThisWeekImageUrl.Substring(original.HotThisWeekImageUrl.LastIndexOf('/') + 1);
                if (imageUrl == imageName)
                {
                    var mainUrl = Path.Combine(_env.WebRootPath, original.HotThisWeekImageUrl);
                    File.Delete(mainUrl);
                    original.HotThisWeekImageUrl = null;
                    _dbContext.Set<LayoutSetting>().Update(original);
                    result = _dbContext.SaveChanges();
                }
            }
            if (original.LatestProductsImageUrl != null)
            {
                var imageName = original.LatestProductsImageUrl.Substring(original.LatestProductsImageUrl.LastIndexOf('/') + 1);

                if (imageUrl == imageName)
                {
                    var mainUrl = Path.Combine(_env.WebRootPath, original.LatestProductsImageUrl);
                    File.Delete(mainUrl);
                    original.LatestProductsImageUrl = null;
                    _dbContext.Set<LayoutSetting>().Update(original);
                    result = _dbContext.SaveChanges();
                }
            }
            if (original.HotThisWeekSeparatorUrl != null)
            {
                var imageName =
                    original.HotThisWeekSeparatorUrl.Substring(original.HotThisWeekSeparatorUrl.LastIndexOf('/') + 1);
                if (imageUrl == imageName)
                {
                    var mainUrl = Path.Combine(_env.WebRootPath, original.HotThisWeekSeparatorUrl);
                    File.Delete(mainUrl);
                    original.HotThisWeekSeparatorUrl = null;
                    _dbContext.Set<LayoutSetting>().Update(original);
                    result = _dbContext.SaveChanges();
                }
            }
            if (original.LatestProductsSeparatorUrl != null)
            {
                var imageName =
                    original.LatestProductsSeparatorUrl.Substring(original.LatestProductsSeparatorUrl.LastIndexOf('/') + 1);
                if (imageUrl == imageName)
                {
                    var mainUrl = Path.Combine(_env.WebRootPath, original.LatestProductsSeparatorUrl);
                    File.Delete(mainUrl);
                    original.LatestProductsSeparatorUrl = null;
                    _dbContext.Set<LayoutSetting>().Update(original);
                    result = _dbContext.SaveChanges();
                }
            }
            if (original.TrendingItemsSeparatorUrl != null)
            {
                var imageName =
                    original.TrendingItemsSeparatorUrl.Substring(original.TrendingItemsSeparatorUrl.LastIndexOf('/') + 1);
                if (imageUrl == imageName)
                {
                    var mainUrl = Path.Combine(_env.WebRootPath, original.TrendingItemsSeparatorUrl);
                    File.Delete(mainUrl);
                    original.TrendingItemsSeparatorUrl = null;
                    _dbContext.Set<LayoutSetting>().Update(original);
                    result = _dbContext.SaveChanges();
                }
            }
            if (original.TrendingItemsImageUrl == null) return result > 0;
            {
                var imageName = original.TrendingItemsImageUrl.Substring(original.TrendingItemsImageUrl.LastIndexOf('/') + 1);
                if (imageUrl != imageName) return result > 0;
                var mainUrl = Path.Combine(_env.WebRootPath, original.TrendingItemsImageUrl);
                File.Delete(mainUrl);
                original.TrendingItemsImageUrl = null;
                _dbContext.Set<LayoutSetting>().Update(original);
                result = _dbContext.SaveChanges();
            }
            return result > 0;
        }

        private LayoutSetting GetImageData(int layoutSettingId, string imageUrl, ThemeSettingImageType img)
        {
            switch (img)
            {
                case ThemeSettingImageType.BackgroundImage:
                    return _dbContext.Set<LayoutSetting>().FirstOrDefault(x => x.Id == layoutSettingId && x.BackgroundImageUrl == imageUrl && x.CompanyId==LoggedInUser.CompanyId && x.Active);
                case ThemeSettingImageType.HotThisWeekImage:
                    return _dbContext.Set<LayoutSetting>().FirstOrDefault(x => x.Id == layoutSettingId && x.HotThisWeekImageUrl == imageUrl && x.CompanyId == LoggedInUser.CompanyId && x.Active);
                case ThemeSettingImageType.LatestProductsImage:
                    return _dbContext.Set<LayoutSetting>().FirstOrDefault(x => x.Id == layoutSettingId && x.LatestProductsImageUrl == imageUrl && x.CompanyId == LoggedInUser.CompanyId && x.Active);
                case ThemeSettingImageType.HotThisWeekSeparator:
                    return _dbContext.Set<LayoutSetting>().FirstOrDefault(x => x.Id == layoutSettingId && x.HotThisWeekSeparatorUrl == imageUrl && x.CompanyId == LoggedInUser.CompanyId && x.Active);
                case ThemeSettingImageType.LatestProductsSeparator:
                    return _dbContext.Set<LayoutSetting>().FirstOrDefault(x => x.Id == layoutSettingId && x.LatestProductsSeparatorUrl == imageUrl && x.CompanyId == LoggedInUser.CompanyId && x.Active);
                case ThemeSettingImageType.TrendingItemsSeparator:
                    return _dbContext.Set<LayoutSetting>().FirstOrDefault(x => x.Id == layoutSettingId && x.TrendingItemsSeparatorUrl == imageUrl && x.CompanyId == LoggedInUser.CompanyId && x.Active);
                case ThemeSettingImageType.TrendingItemsImage:
                    return _dbContext.Set<LayoutSetting>().FirstOrDefault(x => x.Id == layoutSettingId && x.TrendingItemsImageUrl == imageUrl && x.CompanyId == LoggedInUser.CompanyId && x.Active);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
