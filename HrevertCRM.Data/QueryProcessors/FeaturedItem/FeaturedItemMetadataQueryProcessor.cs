using Hrevert.Common.Enums;
using Hrevert.Common.Security;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.Mapper;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using PagedTaskDataInquiryResponse = HrevertCRM.Data.Common.PagedDataInquiryResponse<HrevertCRM.Data.ViewModels.FeaturedItemMetadataViewModel>;

namespace HrevertCRM.Data.QueryProcessors
{

    public class FeaturedItemMetadataQueryProcessor : QueryBase<FeaturedItemMetadata>, IFeaturedItemMetadataQueryProcessor
    {
        private readonly IHostingEnvironment _env;

        public FeaturedItemMetadataQueryProcessor(IUserSession userSession, IDbContext dbContext, IHostingEnvironment env) : base(userSession, dbContext)
        {
            _env = env;
        }

        public FeaturedItemMetadata Update(FeaturedItemMetadata featuredItemMetadata)
        {
            var original = GetValidFeaturedItemMetadata(featuredItemMetadata.Id);
            ValidateAuthorization(featuredItemMetadata);
            CheckVersionMismatch(featuredItemMetadata, original);
            original.FeaturedItemId = featuredItemMetadata.FeaturedItemId;
            original.MediaType = featuredItemMetadata.MediaType;
            original.MediaUrl = featuredItemMetadata.MediaUrl;
            original.Active = featuredItemMetadata.Active;
            original.ItemId = featuredItemMetadata.ItemId;
            original.WebActive = featuredItemMetadata.WebActive;
            original.CompanyId = featuredItemMetadata.CompanyId;

            _dbContext.Set<FeaturedItemMetadata>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        public virtual FeaturedItemMetadata GetValidFeaturedItemMetadata(int featuredItemMetadataId)
        {
            var featureditemMetadata = _dbContext.Set<FeaturedItemMetadata>().FirstOrDefault(sc => sc.Id == featuredItemMetadataId);
            if (featureditemMetadata == null)
            {
                // throw new RootObjectNotFoundException(ProductConstants.ProductMetadataQueryProcessorConstants.ProductMetadataNotFound);
                throw new RootObjectNotFoundException("Not Found");
            }
            return featureditemMetadata;
        }

        public FeaturedItemMetadataViewModel GetFeaturedItemMetadata(int productMetadataId)
        {
            var mapper = new FeaturedItemMetadataToFeaturedItemMetadataViewModelMapper();
            var productMetadata = _dbContext.Set<FeaturedItemMetadata>().FirstOrDefault(d => d.Id == productMetadataId);
            return mapper.Map(productMetadata);
        }

        public FeaturedItemMetadata GetFeaturedItemMetadataById(int productMetadataId)
        {
            var productMetadata = _dbContext.Set<FeaturedItemMetadata>().FirstOrDefault(d => d.Id == productMetadataId);
            return productMetadata;
        }

        public void SaveAllFeaturedItemMetadata(List<FeaturedItemMetadata> featuredItemMetadatas)
        {
            _dbContext.Set<FeaturedItemMetadata>().AddRange(featuredItemMetadatas);
            _dbContext.SaveChanges();
        }

        public FeaturedItemMetadata Save(FeaturedItemMetadata featuredItemMetadata)
        {
            featuredItemMetadata.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<FeaturedItemMetadata>().Add(featuredItemMetadata);
            _dbContext.SaveChanges();
            return featuredItemMetadata;
        }

        public int SaveAll(List<FeaturedItemMetadata> featuredItemMetadatas)
        {
            _dbContext.Set<FeaturedItemMetadata>().AddRange(featuredItemMetadatas);
            return _dbContext.SaveChanges();

        }

        public FeaturedItemMetadata ActivateFeaturedItemMetadata(int id)
        {
            var original = GetValidFeaturedItemMetadata(id);
            ValidateAuthorization(original);

            original.Active = true;
            _dbContext.Set<FeaturedItemMetadata>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        public bool Delete(int featureItemMetadataId)
        {
            var doc = GetFeaturedItemMetadataById(featureItemMetadataId);
            ValidateAuthorization(doc);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<FeaturedItemMetadata>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }

        public bool Exists(Expression<Func<FeaturedItemMetadata, bool>> @where)
        {
            return _dbContext.Set<FeaturedItemMetadata>().Any(@where);
        }

        public FeaturedItemMetadata[] GetFeaturedItemMetadatas(Expression<Func<FeaturedItemMetadata, bool>> @where = null)
        {
            var query = _dbContext.Set<FeaturedItemMetadata>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            var enumerable = query.ToArray();
            return enumerable;
        }

        private static PagedTaskDataInquiryResponse FormatResultForPaging(PagedDataRequest requestInfo, IQueryable<FeaturedItemMetadata> query)
        {
            var totalItemCount = query.Count();

            var startIndex = ResultsPagingUtility.CalculateStartIndex(requestInfo.PageNumber, requestInfo.PageSize);
            var mapper = new FeaturedItemMetadataToFeaturedItemMetadataViewModelMapper();
            var docs = query.OrderBy(x => x.DateCreated).Skip(startIndex).Take(requestInfo.PageSize).Select(
                s => mapper.Map(s)).ToList();

            var queryResult = new QueryResult<FeaturedItemMetadataViewModel>(docs, totalItemCount, requestInfo.PageSize);
            var inquiryResponse = new PagedTaskDataInquiryResponse()
            {
                Items = docs,
                TotalRecords = totalItemCount,
                PageCount = queryResult.TotalPageCount,
                PageNumber = requestInfo.PageNumber,
                PageSize = requestInfo.PageSize
            };

            return inquiryResponse;
        }

        public string SaveFeaturedItemMetadatas(int featureItemId, int itemId, List<Image> images)
        {
            if (images.Count == 0) return "Nothing to save";
            try
            {

                var fullWidthImagePath = Path.Combine(_env.WebRootPath, @"companyMM\BannerImages\" + LoggedInUser.CompanyId + @"\FullWidth");
                var halfWidthImagePath = Path.Combine(_env.WebRootPath, @"companyMM\BannerImages\" + LoggedInUser.CompanyId + @"\HalfWidth");
                var quaterWidthImagePath = Path.Combine(_env.WebRootPath, @"companyMM\BannerImages\" + LoggedInUser.CompanyId + @"\QuaterWidth");

                CreateDirectory(fullWidthImagePath);
                CreateDirectory(halfWidthImagePath);
                CreateDirectory(quaterWidthImagePath);

                foreach (var image in images)
                {
                    if (image == null) continue;
                    if (!CheckFileExtesions(image)) return "Please choose only Image.";

                    //Check if the directory exists or create a new one
                    var bytes = Convert.FromBase64String(image.ImageBase64);
                    var filename = Regex.Replace(Path.GetFileNameWithoutExtension(image.FileName), @"\s+", "");
                    var fileExtension = Path.GetExtension(image.FileName);


                    string copyFileName;
                    string filePath;
                    string mediaUrl;
                    //Create path with filename

                    switch (image.ImageType)
                    {
                        case ImageType.FullWidthImage:
                            copyFileName = filename + fileExtension;
                            filePath = Path.Combine(fullWidthImagePath, copyFileName);
                            mediaUrl = "companyMM/BannerImages/" + LoggedInUser.CompanyId + "/" + "FullWidth/" + copyFileName;
                            break;
                        case ImageType.HalfWidthImage:
                            copyFileName = filename + fileExtension;
                            filePath = Path.Combine(halfWidthImagePath, copyFileName);
                            mediaUrl = "companyMM/BannerImages/" + LoggedInUser.CompanyId + "/" + "HalfWidth/" + copyFileName;
                            break;
                        case ImageType.QuaterWidthImage:
                            copyFileName = filename + fileExtension;
                            filePath = Path.Combine(quaterWidthImagePath, copyFileName);
                            mediaUrl = "companyMM/BannerImages/" + LoggedInUser.CompanyId + "/" + "QuaterWidth/" + copyFileName;
                            break;
                        // Do Nothing
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    using (var imageFile = new FileStream(filePath, FileMode.Create))
                    {
                        imageFile.Write(bytes, 0, bytes.Length);
                        imageFile.Flush();
                    }

                    var featureItemMetadata = new FeaturedItemMetadata
                    {
                        CompanyId = LoggedInUser.CompanyId,
                        Active = true,
                        FeaturedItemId = featureItemId,
                        MediaType = MediaType.Photo,
                        MediaUrl = mediaUrl,
                        ImageType = image.ImageType,
                        ItemId = itemId
                    };

                    Save(featureItemMetadata);
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "Saved";
        }

        private static bool CheckFileExtesions(Image image)
        {
            var allowedExtensions = new[] { ".JPG", ".PNG", ".JPEG" };
            var ext = Path.GetExtension(image.FileName);
            return allowedExtensions.Contains(ext.ToUpper());
        }

        private static void CreateDirectory(string path)
        {
            var directory = new DirectoryInfo(path);

            if (directory.Exists == false)
            {
                directory.Create();
            }
        }

        public bool RemoveBannerImageByImageUrlAndImageTypeId(int imageTypeId, string imageUrl)
        {
            var featuredItemMetadata = _dbContext.Set<FeaturedItemMetadata>().FirstOrDefault(p => p.Active && p.CompanyId == LoggedInUser.CompanyId && (int)p.ImageType == imageTypeId && p.MediaUrl == imageUrl);
            var result = 0;
            if (featuredItemMetadata == null) return result > 0;
            _dbContext.Set<FeaturedItemMetadata>().Remove(featuredItemMetadata);
            result = _dbContext.SaveChanges();
            return result > 0;
        }

        public PagedTaskDataInquiryResponse SearchFeatureItemMetadatas(PagedDataRequest requestInfo, Expression<Func<FeaturedItemMetadata, bool>> @where = null)
        {
            var query = _dbContext.Set<FeaturedItemMetadata>().Where(c => c.CompanyId == LoggedInUser.CompanyId);
            if (requestInfo.Active)
                query = query.Where(x => x.Active);

            var result = string.IsNullOrEmpty(requestInfo.SearchText)
                ? query
                : query.Where(s => s.MediaUrl.ToUpper().Contains(requestInfo.SearchText.ToUpper()));
            return FormatResultForPaging(requestInfo, result);
        }
    }
}

