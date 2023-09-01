using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Hosting;
using Hrevert.Common.Security;
using System.IO;
using System.Text.RegularExpressions;
using Hrevert.Common.Enums;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.QueryProcessors
{
    public class GeneralSettingQueryProcessor : QueryBase<GeneralSetting>, IGeneralSettingQueryProcessor
    {
        private readonly IHostingEnvironment _env;
        public GeneralSettingQueryProcessor(IUserSession userSession, IDbContext dbContext, IHostingEnvironment env)
            : base(userSession, dbContext)
        {
            _env = env;
        }
        public GeneralSetting ActivateBrand(int generalSettingId)
        {
            var original = GetGeneralSetting(generalSettingId);
            ValidateAuthorization(original);
            original.Active = true;
            _dbContext.Set<GeneralSetting>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        public bool Delete(int generalSettingId)
        {
            var data = GetGeneralSetting(generalSettingId);
            var result = 0;
            if (data == null) return result > 0;
            data.Active = false;
            _dbContext.Set<GeneralSetting>().Update(data);
            result = _dbContext.SaveChanges();
            return result > 0;
        }

        public bool Exists(Expression<Func<GeneralSetting, bool>> where)
        {
            return _dbContext.Set<GeneralSetting>().Any(where);
        }

        public GeneralSetting GetGeneralSetting(int generalSettingId)
        {
            var generalSetting = _dbContext.Set<GeneralSetting>().FirstOrDefault(x => x.Id == generalSettingId && x.CompanyId==LoggedInUser.CompanyId && x.Active);
            return generalSetting;
        }

        public GeneralSetting Save(GeneralSetting generalSetting)
        {
            generalSetting.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<GeneralSetting>().Add(generalSetting);
            _dbContext.SaveChanges();
            return generalSetting;
        }

        public void SaveAll(List<GeneralSetting> generalSettings)
        {
            _dbContext.Set<GeneralSetting>().AddRange(generalSettings);
            _dbContext.SaveChanges();
        }

        public string SaveThemeSettingImage(ThemeImage image)
        {
            string mediaUrl;
            string copyFileName;
            try
            {
                var directoryPath = GetDirectoryPath(image);
                if (!CheckFileExtesions(image)) return "Please choose Image files only.";

                //Check if the directory exists or create a new one
                var bytes = Convert.FromBase64String(image.ImageBase64);
                var filename = Regex.Replace(Path.GetFileNameWithoutExtension(image.FileName), @"\s+", "");
                var fileExtension = Path.GetExtension(image.FileName);

                //Create path with filename
                switch (image.ImageType)
                {
                    case ThemeSettingImageType.Logo:
                        mediaUrl = "companyMM/ThemeSettingImages/Logo/" + LoggedInUser.CompanyId + "/";
                        copyFileName = filename + fileExtension;
                        break;
                    case ThemeSettingImageType.Favicon:
                        mediaUrl = "companyMM/ThemeSettingImages/Favicon/" + LoggedInUser.CompanyId + "/";
                        copyFileName = filename + fileExtension;
                        break;
                    case ThemeSettingImageType.FooterImage:
                        mediaUrl = "companyMM/ThemeSettingImages/Footer/" + LoggedInUser.CompanyId + "/";
                        copyFileName = filename + fileExtension;
                        break;
                    case ThemeSettingImageType.BrandImage:
                        mediaUrl = "companyMM/ThemeSettingImages/Brand/" + LoggedInUser.CompanyId + "/";
                        copyFileName = filename + fileExtension;
                        break;
                    case ThemeSettingImageType.BackgroundImage:
                        mediaUrl = "companyMM/ThemeSettingImages/Background/" + LoggedInUser.CompanyId + "/";
                        copyFileName = filename + fileExtension;
                        break;
                    case ThemeSettingImageType.HotThisWeekImage:
                        mediaUrl = "companyMM/ThemeSettingImages/HotThisWeek/" + LoggedInUser.CompanyId + "/";
                        copyFileName = filename + fileExtension;
                        break;
                    case ThemeSettingImageType.LatestProductsImage:
                        mediaUrl = "companyMM/ThemeSettingImages/LatestProducts/" + LoggedInUser.CompanyId + "/";
                        copyFileName = filename + fileExtension;
                        break;
                    case ThemeSettingImageType.TrendingItemsImage:
                        mediaUrl = "companyMM/ThemeSettingImages/TrendingItems/" + LoggedInUser.CompanyId + "/";
                        copyFileName = filename + fileExtension;
                        break;
                    case ThemeSettingImageType.PersonnelImage:
                        mediaUrl = "companyMM/ThemeSettingImages/Personnel/" + LoggedInUser.CompanyId + "/";
                        copyFileName = filename + fileExtension;
                        break;
                    case ThemeSettingImageType.SlideImage:
                        mediaUrl = "companyMM/ThemeSettingImages/SlideImage/" + LoggedInUser.CompanyId + "/";
                        copyFileName = filename + fileExtension;
                        break;
                    case ThemeSettingImageType.SlideBackgroundImage:
                        mediaUrl = "companyMM/ThemeSettingImages/SlideBackgroundImage/" + LoggedInUser.CompanyId + "/";
                        copyFileName = filename + fileExtension;
                        break;
                    case ThemeSettingImageType.HotThisWeekSeparator:
                        mediaUrl = "companyMM/ThemeSettingImages/HotThisWeekSeparator/" + LoggedInUser.CompanyId + "/";
                        copyFileName = filename + fileExtension;
                        break;
                    case ThemeSettingImageType.LatestProductsSeparator:
                        mediaUrl = "companyMM/ThemeSettingImages/LatestProductsSeparator/" + LoggedInUser.CompanyId + "/";
                        copyFileName = filename + fileExtension;
                        break;
                    case ThemeSettingImageType.TrendingItemsSeparator:
                        mediaUrl = "companyMM/ThemeSettingImages/TrendingItemsSeparator/" + LoggedInUser.CompanyId + "/";
                        copyFileName = filename + fileExtension;
                        break;
                    // Do Nothing
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                var pathWithImageName = Path.Combine(directoryPath, copyFileName);
                using (var imageFile = new FileStream(pathWithImageName, FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return mediaUrl + copyFileName;
        }

        private string GetDirectoryPath(ThemeImage image)
        {
            string directoryPath;
            switch (image.ImageType)
            {
                case ThemeSettingImageType.Logo:
                    directoryPath = Path.Combine(_env.WebRootPath, @"companyMM\ThemeSettingImages\Logo\" + LoggedInUser.CompanyId);
                    CreateDirectory(directoryPath);
                    break;
                case ThemeSettingImageType.Favicon:
                    directoryPath = Path.Combine(_env.WebRootPath, @"companyMM\ThemeSettingImages\Favicon\" + LoggedInUser.CompanyId);
                    CreateDirectory(directoryPath);
                    break;
                case ThemeSettingImageType.FooterImage:
                    directoryPath = Path.Combine(_env.WebRootPath, @"companyMM\ThemeSettingImages\Footer\" + LoggedInUser.CompanyId);
                    CreateDirectory(directoryPath);
                    break;
                case ThemeSettingImageType.BackgroundImage:
                    directoryPath = Path.Combine(_env.WebRootPath, @"companyMM\ThemeSettingImages\Background\" + LoggedInUser.CompanyId);
                    CreateDirectory(directoryPath);
                    break;
                case ThemeSettingImageType.HotThisWeekImage:
                    directoryPath = Path.Combine(_env.WebRootPath, @"companyMM\ThemeSettingImages\HotThisWeek\" + LoggedInUser.CompanyId);
                    CreateDirectory(directoryPath);
                    break;
                case ThemeSettingImageType.LatestProductsImage:
                    directoryPath = Path.Combine(_env.WebRootPath, @"companyMM\ThemeSettingImages\LatestProducts\" + LoggedInUser.CompanyId);
                    CreateDirectory(directoryPath);
                    break;
                case ThemeSettingImageType.PersonnelImage:
                    directoryPath = Path.Combine(_env.WebRootPath, @"companyMM\ThemeSettingImages\Personnel\" + LoggedInUser.CompanyId);
                    CreateDirectory(directoryPath);
                    break;
                case ThemeSettingImageType.TrendingItemsImage:
                    directoryPath = Path.Combine(_env.WebRootPath, @"companyMM\ThemeSettingImages\TrendingItems\" + LoggedInUser.CompanyId);
                    CreateDirectory(directoryPath);
                    break;
                case ThemeSettingImageType.HotThisWeekSeparator:
                    directoryPath = Path.Combine(_env.WebRootPath, @"companyMM\ThemeSettingImages\HotThisWeekSeparator\" + LoggedInUser.CompanyId);
                    CreateDirectory(directoryPath);
                    break;
                case ThemeSettingImageType.LatestProductsSeparator:
                    directoryPath = Path.Combine(_env.WebRootPath, @"companyMM\ThemeSettingImages\LatestProductsSeparator\" + LoggedInUser.CompanyId);
                    CreateDirectory(directoryPath);
                    break;
                case ThemeSettingImageType.TrendingItemsSeparator:
                    directoryPath = Path.Combine(_env.WebRootPath, @"companyMM\ThemeSettingImages\TrendingItemsSeparator\" + LoggedInUser.CompanyId);
                    CreateDirectory(directoryPath);
                    break;
                // Do Nothing
                case ThemeSettingImageType.BrandImage:
                    directoryPath = Path.Combine(_env.WebRootPath, @"companyMM\ThemeSettingImages\Brand\" + LoggedInUser.CompanyId);
                    CreateDirectory(directoryPath);
                    break;
                case ThemeSettingImageType.SlideImage:
                    directoryPath = Path.Combine(_env.WebRootPath, @"companyMM\ThemeSettingImages\Slide\" + LoggedInUser.CompanyId);
                    CreateDirectory(directoryPath);
                    break;
                case ThemeSettingImageType.SlideBackgroundImage:
                    directoryPath = Path.Combine(_env.WebRootPath, @"companyMM\ThemeSettingImages\SlideBackground\" + LoggedInUser.CompanyId);
                    CreateDirectory(directoryPath);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return directoryPath;
        }

        private static void CreateDirectory(string path)
        {
            var directory = new DirectoryInfo(path);
            if (directory.Exists == false)
                directory.Create();
        }

        private static bool CheckFileExtesions(ThemeImage image)
        {
            var allowedExtensions = new[] { ".JPG", ".PNG", ".JPEG" };
            var ext = Path.GetExtension(image.FileName);
            return allowedExtensions.Contains(ext.ToUpper());
        }
        public GeneralSetting Update(GeneralSetting generalSetting)
        {
            var original = GetGeneralSetting(generalSetting.Id);
            ValidateAuthorization(original);
            CheckVersionMismatch(generalSetting, original);
            //pass value to original
            original.SelectedTheme = generalSetting.SelectedTheme;
            original.LogoUrl = generalSetting.LogoUrl;
            original.StoreName = generalSetting.StoreName;
            original.FaviconLogoUrl = generalSetting.FaviconLogoUrl;
            original.EnableSlides = generalSetting.EnableSlides;
            original.EnableTopCategories = generalSetting.EnableTopCategories;
            original.EnableTopTrendingProducts = generalSetting.EnableTopTrendingProducts;
            original.EnableHotThisWeek = generalSetting.EnableHotThisWeek;
            original.EnableLatestProducts = generalSetting.EnableLatestProducts;
            original.EnableGetInspired = generalSetting.EnableGetInspired;
            original.Active = generalSetting.Active;
            original.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<GeneralSetting>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        public string GetLogoImageUrl(int id)
        {
            var logoImageUrl= _dbContext.Set<GeneralSetting>().FirstOrDefault(x => x.Id == id && x.CompanyId == LoggedInUser.CompanyId && x.Active).LogoUrl;
            return logoImageUrl;
        }

        //public bool DeleteImage(GeneralSettingViewModel generalSettingViewModel, string imageUrl, ThemeImage img)
        //{
        //    if(img.ImageType== ThemeSettingImageType.Logo)
        //    {
        //        var mainUrl = Path.Combine(_env.WebRootPath, imageUrl);
        //        File.Delete(mainUrl);
        //        var doc1 = GetLogo((int)generalSettingViewModel.Id, imageUrl);
        //        if (doc1 != null)
        //            doc1.LogoUrl = null;
        //        _dbContext.Set<GeneralSetting>().Update(doc1);
        //    }
        //    else
        //    {
        //        var mainUrl = Path.Combine(_env.WebRootPath, imageUrl);
        //        File.Delete(mainUrl);
        //        var doc1 = GetImageUrl((int)generalSettingViewModel.Id, imageUrl);
        //        if (doc1 != null)
        //            doc1.FaviconLogoUrl = null;
        //            _dbContext.Set<GeneralSetting>().Update(doc1);
        //    }
        //    _dbContext.SaveChanges();
        //    return true;
        //}
        public GeneralSetting GetImageUrl(int id, string imageUrl)
        {
            return _dbContext.Set<GeneralSetting>().FirstOrDefault(x => x.Id == id && x.FaviconLogoUrl == imageUrl && x.CompanyId==LoggedInUser.CompanyId && x.Active);
        }
        public GeneralSetting GetLogo(int id, string imageUrl)
        {
            return _dbContext.Set<GeneralSetting>().FirstOrDefault(x => x.Id == id && x.LogoUrl == imageUrl && x.CompanyId==LoggedInUser.CompanyId && x.Active);
        }
        public string GetGetFaviconImageUrl(int id)
        {
            var imageUrl= _dbContext.Set<GeneralSetting>().FirstOrDefault(x => x.Id == id && x.CompanyId == LoggedInUser.CompanyId && x.Active).FaviconLogoUrl;
            return imageUrl;
        }

        public GeneralSetting Get()
        {
            return _dbContext.Set<GeneralSetting>().FirstOrDefault(x=>x.CompanyId==LoggedInUser.CompanyId && x.Active);
        }

        public bool RemoveLogo(int id)
        {
            var original = GetGeneralSetting(id);
            ValidateAuthorization(original);
            var result = 0;
            if (original == null) return result > 0;
            var mainUrl = Path.Combine(_env.WebRootPath, original.LogoUrl);
            File.Delete(mainUrl);
            original.LogoUrl = null;
           _dbContext.Set<GeneralSetting>().Update(original);
            result = _dbContext.SaveChanges();
            return result>0;
        }

        public bool RemoveFavicon(int id)
        {
            var original = GetGeneralSetting(id);
            ValidateAuthorization(original);
            var result = 0;
            if (original == null) return result > 0;
            var mainUrl = Path.Combine(_env.WebRootPath, original.FaviconLogoUrl);
            File.Delete(mainUrl);
            original.FaviconLogoUrl = null;
            _dbContext.Set<GeneralSetting>().Update(original);
            result = _dbContext.SaveChanges();
            return result > 0;
        }

        public List<object> GetThemeColor()
        {
            var folderPath = Path.Combine(_env.WebRootPath, @"companyMM\ThemeColor");
            var ListCss = Directory
                .GetFiles(folderPath, "*").ToArray();
            List<object> ThemeColorList = new List<object>();
            foreach (var item in ListCss)
            {
                var fileExtension = Path.GetExtension(item).Replace(".", "");
                byte[] cssArray = File.ReadAllBytes(item);
                int pos = item.LastIndexOf("\\") + 1;
                object Css = new
                {
                    Name = item.Substring(pos, item.Length - pos)
                };
                ThemeColorList.Add(Css);
            }
            return ThemeColorList;
        }
    }
}
