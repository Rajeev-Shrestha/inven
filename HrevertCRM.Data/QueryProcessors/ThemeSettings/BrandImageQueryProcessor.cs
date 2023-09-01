using HrevertCRM.Entities;
using Hrevert.Common.Security;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Linq;

namespace HrevertCRM.Data.QueryProcessors
{
    public class BrandImageQueryProcessor : QueryBase<BrandImage>, IBrandImageQueryProcessor
    {
        private readonly IHostingEnvironment _env;
        public BrandImageQueryProcessor(IUserSession userSession, IDbContext dbContext, IHostingEnvironment env)
            : base(userSession, dbContext)
        {
            _env = env;
        }

        public void DeleteBrandImage(int footerSettingId, string imageUrl)
        {
           var imgUrl = Path.Combine(_env.WebRootPath, @"companyMM\ThemeSettingImages\Brand\" + LoggedInUser.CompanyId, imageUrl);
            File.Delete(imgUrl);
            var imageUrl1 = @"companyMM\ThemeSettingImages\Brand\" + LoggedInUser.CompanyId + imageUrl;
            var doc1 = GetBrandImageIdAndImageUri(footerSettingId, imageUrl1);
            if (doc1 != null)
                _dbContext.Set<BrandImage>().Remove(doc1);
            _dbContext.SaveChanges();
        }
        private BrandImage GetBrandImageIdAndImageUri(int footerSettingId, string imgUrl)
        {
            var data= _dbContext.Set<BrandImage>().FirstOrDefault(x => x.FooterSettingId == footerSettingId && x.CompanyId == LoggedInUser.CompanyId && x.ImageUrl == imgUrl && x.Active);
            return data;
        }
        public BrandImage GetBrandImage(int id) 
        {
            var brandImage = _dbContext.Set<BrandImage>().FirstOrDefault(x => x.Id == id && x.CompanyId==LoggedInUser.CompanyId && x.Active);
            return brandImage;
        }

        public BrandImage Save(BrandImage brandImage)
        {
            brandImage.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<BrandImage>().Add(brandImage);
            _dbContext.SaveChanges();
            return brandImage;
        }

        public BrandImage Update(BrandImage brandImage)
        {
            var original = GetBrandImage(brandImage.Id);
            ValidateAuthorization(original);
            CheckVersionMismatch(brandImage, original);
            //pass value to original
            original.CompanyId = LoggedInUser.CompanyId;
            original.Active = true;
            original.FooterSettingId = brandImage.FooterSettingId;
            original.ImageUrl = brandImage.ImageUrl;
            _dbContext.Set<BrandImage>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
    }
}
