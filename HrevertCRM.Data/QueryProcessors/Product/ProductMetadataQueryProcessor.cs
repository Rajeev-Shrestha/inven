using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Hrevert.Common.Constants.Product;
using Hrevert.Common.Enums;
using Hrevert.Common.Security;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.Mapper;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using Microsoft.AspNetCore.Hosting;
using Remotion.Linq.Clauses;
using PagedTaskDataInquiryResponse =
    HrevertCRM.Data.Common.PagedDataInquiryResponse<HrevertCRM.Data.ViewModels.ProductMetadataViewModel>;

namespace HrevertCRM.Data.QueryProcessors
{
    public class ProductMetadataQueryProcessor : QueryBase<ProductMetadata>, IProductMetadataQueryProcessor
    {
        private readonly IHostingEnvironment _env;

        public ProductMetadataQueryProcessor(IUserSession userSession, IDbContext dbContext,
            IHostingEnvironment env) : base(userSession, dbContext)
        {
            _env = env;
        }

        public ProductMetadata Update(ProductMetadata productMetadata)
        {
            var original = GetValidProductMetadata(productMetadata.Id);
            ValidateAuthorization(productMetadata);
            CheckVersionMismatch(productMetadata, original);
            original.ProductId = productMetadata.ProductId;
            original.MediaType = productMetadata.MediaType;
            original.MediaUrl = productMetadata.MediaUrl;
            original.Active = productMetadata.Active;
            original.WebActive = productMetadata.WebActive;
            original.CompanyId = productMetadata.CompanyId;

            _dbContext.Set<ProductMetadata>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        public virtual ProductMetadata GetValidProductMetadata(int productMetadataId)
        {
            var productMetadata = _dbContext.Set<ProductMetadata>().FirstOrDefault(sc => sc.Id == productMetadataId);
            if (productMetadata == null)
            {
                throw new RootObjectNotFoundException(ProductConstants.ProductMetadataQueryProcessorConstants.ProductMetadataNotFound);
            }
            return productMetadata;
        }

        public ProductMetadataViewModel GetProductMetadata(int productMetadataId)
        {
            var mapper = new ProductMetadataToProductMetadataViewModelMapper();
            var productMetadata = _dbContext.Set<ProductMetadata>().FirstOrDefault(d => d.Id == productMetadataId);
            return mapper.Map(productMetadata);
        }

        public ProductMetadata GetProductMetadataById(int productMetadataId)
        {
            var productMetadata = _dbContext.Set<ProductMetadata>().FirstOrDefault(d => d.Id == productMetadataId);
            return productMetadata;
        }
        public ProductMetadata GetProductMetadataByIdAndImageUri(int productId, string productImageUrl)
        {
            var productMetadata = _dbContext.Set<ProductMetadata>().FirstOrDefault(d => d.ProductId == productId && d.MediaUrl == productImageUrl && d.CompanyId == LoggedInUser.CompanyId);
            return productMetadata;
        }

        public void SaveAllProductMetadata(List<ProductMetadata> productMetadatas)
        {
            _dbContext.Set<ProductMetadata>().AddRange(productMetadatas);
            _dbContext.SaveChanges();
        }

        public ProductMetadata Save(ProductMetadata productMetadata)
        {
            productMetadata.WebActive = true;
            productMetadata.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<ProductMetadata>().Add(productMetadata);
            _dbContext.SaveChanges();
            return productMetadata;
        }

        public int SaveAll(List<ProductMetadata> productMetadatas)
        {
            _dbContext.Set<ProductMetadata>().AddRange(productMetadatas);
            return _dbContext.SaveChanges();

        }

        public List<ProductMetadata> SearchActive(string searchString)
        {
            var query = _dbContext.Set<ProductMetadata>().Where(FilterByActiveTrueAndCompany);
            var productMetadatas = new List<ProductMetadata>();
            if (!string.IsNullOrEmpty(searchString))
            {
                productMetadatas =
                    query.Where(s => s.MediaUrl.ToUpper().Contains(searchString.ToUpper())).ToList();
            }
            return productMetadatas;
        }

        public List<ProductMetadata> SearchAll(string searchString)
        {
            var query = _dbContext.Set<ProductMetadata>().Where(x => x.CompanyId == LoggedInUser.CompanyId);
            var productMetadatas = new List<ProductMetadata>();
            if (!string.IsNullOrEmpty(searchString))
            {
                productMetadatas =
                    query.Where(s => s.MediaUrl.ToUpper().Contains(searchString.ToUpper())).ToList();
            }
            return productMetadatas;
        }

        public ProductMetadata ActivateProductMetadata(int id)
        {
            var original = GetValidProductMetadata(id);
            ValidateAuthorization(original);

            original.Active = true;
            _dbContext.Set<ProductMetadata>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        public bool Delete(int productMetadataId)
        {
            var doc = GetProductMetadataById(productMetadataId);
            ValidateAuthorization(doc);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<ProductMetadata>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }
        public bool DeleteProductImage(int productId, string imageUrl)
        {
            var urlForSmallImage = "CompanyMM\\" + LoggedInUser.CompanyId + "\\Small\\" + imageUrl;
            var urlForMediumImage = "CompanyMM\\" + LoggedInUser.CompanyId + "\\Medium\\" + imageUrl;
            var urlForLargeImage = "CompanyMM\\" + LoggedInUser.CompanyId + "\\Large\\" + imageUrl;
            var mainUrlForSmallImage = Path.Combine(_env.WebRootPath, urlForSmallImage);
            var mainUrlForMediumImage = Path.Combine(_env.WebRootPath, urlForMediumImage);
            var mainUrlForLargeImage = Path.Combine(_env.WebRootPath, urlForLargeImage);
            File.Delete(mainUrlForSmallImage);
            File.Delete(mainUrlForMediumImage);
            File.Delete(mainUrlForLargeImage);
            var imageUrl1 = "companyMM/" + LoggedInUser.CompanyId + "/Small/" + imageUrl;
            var imageUrl2 = "companyMM/" + LoggedInUser.CompanyId + "/Medium/" + imageUrl;
            var imageUrl3 = "companyMM/" + LoggedInUser.CompanyId + "/Large/" + imageUrl;
            var doc1 = GetProductMetadataByIdAndImageUri(productId, imageUrl1);
            if (doc1 != null)
                _dbContext.Set<ProductMetadata>().Remove(doc1);
            _dbContext.SaveChanges();
            var doc2 = GetProductMetadataByIdAndImageUri(productId, imageUrl2);
            if (doc2 != null)
                _dbContext.Set<ProductMetadata>().Remove(doc2);
            _dbContext.SaveChanges();
            var doc3 = GetProductMetadataByIdAndImageUri(productId, imageUrl3);
            if (doc3 != null)
                _dbContext.Set<ProductMetadata>().Remove(doc3);
            _dbContext.SaveChanges();
            return true;
        }
        public bool Exists(Expression<Func<ProductMetadata, bool>> @where)
        {
            return _dbContext.Set<ProductMetadata>().Any(@where);
        }

        public PagedTaskDataInquiryResponse GetActiveProductMetadatas(PagedDataRequest requestInfo,
            Expression<Func<ProductMetadata, bool>> @where = null)
        {

            var query = _dbContext.Set<ProductMetadata>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);
            return FormatResultForPaging(requestInfo, query);
        }


        public PagedTaskDataInquiryResponse GetDeletedProductMetadatas(PagedDataRequest requestInfo,
            Expression<Func<ProductMetadata, bool>> @where = null)
        {
            var query = _dbContext.Set<ProductMetadata>().Where(FilterByActiveFalseAndCompany);
            query = @where == null ? query : query.Where(@where);
            return FormatResultForPaging(requestInfo, query);
        }

        public PagedTaskDataInquiryResponse GetAllProductMetadatas(PagedDataRequest requestInfo,
            Expression<Func<ProductMetadata, bool>> @where = null)
        {
            var query = _dbContext.Set<ProductMetadata>().Where(c => c.CompanyId == LoggedInUser.CompanyId);
            return FormatResultForPaging(requestInfo, query);
        }

        public ProductMetadata[] GetProductMetadatas(Expression<Func<ProductMetadata, bool>> @where = null)
        {
            var query = _dbContext.Set<ProductMetadata>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            var enumerable = query.ToArray();
            return enumerable;
        }

        private static PagedTaskDataInquiryResponse FormatResultForPaging(PagedDataRequest requestInfo, IQueryable<ProductMetadata> query)
        {
            var totalItemCount = query.Count();

            var startIndex = ResultsPagingUtility.CalculateStartIndex(requestInfo.PageNumber, requestInfo.PageSize);
            var mapper = new ProductMetadataToProductMetadataViewModelMapper();
            var docs = query.OrderBy(x => x.DateCreated).Skip(startIndex).Take(requestInfo.PageSize).Select(
                s => mapper.Map(s)).ToList();

            var queryResult = new QueryResult<ProductMetadataViewModel>(docs, totalItemCount, requestInfo.PageSize);
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

        public string SaveProductMetadata(int productId, List<Image> images)
        {
            if (images.Count == 0) return "Nothing to save";
            try
            {
                foreach (var image in images)
                {
                    if (image == null) continue;
                    if (!CheckFileExtesions(image)) return "Please choose only Image and Pdf files.";

                    //Check if the directory exists or create a new one
                    var bytes = Convert.FromBase64String(image.ImageBase64);
                    var filename = Regex.Replace(Path.GetFileNameWithoutExtension(image.FileName), @"\s+", "");
                    var fileExtension = Path.GetExtension(image.FileName);
                    var path = Path.Combine(_env.WebRootPath, @"companyMM\" + LoggedInUser.CompanyId);

                    CreateDirectory(path);
                    //Create path with filename
                    var fileWritten = false;
                    var i = 0;
                    while (!fileWritten)
                    {
                        if (i > 0)
                        {
                            if (!filename.Contains("_") && i == 1)
                            {
                                filename = filename + "_" + i;
                            }
                            else
                            {
                                filename = filename.Remove(filename.LastIndexOf("_", StringComparison.Ordinal)) + "_" + i;
                            }
                        }

                        var copyfileName = filename + fileExtension;
                        var filePath = Path.Combine(path, copyfileName);

                        if (File.Exists(filePath)) i++;
                        else
                        {
                            using (var imageFile = new FileStream(filePath, FileMode.Create))
                            {
                                imageFile.Write(bytes, 0, bytes.Length);
                                imageFile.Flush();
                                fileWritten = true;
                            }

                            var productMetadata = new ProductMetadata
                            {
                                CompanyId = LoggedInUser.CompanyId,
                                Active = true,
                                ProductId = productId,
                                MediaType = MediaType.Photo,
                                MediaUrl = "companyMM/" + LoggedInUser.CompanyId + "/" + copyfileName,
                                WebActive = true
                            };

                            Save(productMetadata);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "Saved";
        }

        public string SaveProductMetadatas(int productId, List<Image> images)
        {
            if (images.Count == 0) return "Nothing to save";
            try
            {
                var smallImagePath = Path.Combine(_env.WebRootPath, @"companyMM\" + LoggedInUser.CompanyId + @"\Small");
                var mediumImagePath = Path.Combine(_env.WebRootPath, @"companyMM\" + LoggedInUser.CompanyId + @"\Medium");
                var largeImagePath = Path.Combine(_env.WebRootPath, @"companyMM\" + LoggedInUser.CompanyId + @"\Large");

                CreateDirectory(smallImagePath);
                CreateDirectory(mediumImagePath);
                CreateDirectory(largeImagePath);

                foreach (var image in images)
                {
                    if (image == null) continue;
                    if (!CheckFileExtesions(image)) return "Please choose only Image and Pdf files.";

                    //Check if the directory exists or create a new one
                    var bytes = Convert.FromBase64String(image.ImageBase64);
                    var filename = Regex.Replace(Path.GetFileNameWithoutExtension(image.FileName), @"\s+", "");
                    var fileExtension = Path.GetExtension(image.FileName);


                    string copyFileName;
                    string filePath;
                    string mediaUrl;
                    //Create path with filename

                    switch (image.ImageSize)
                    {
                        case ImageSize.Small:
                            filePath = smallImagePath;
                            mediaUrl = "companyMM/" + LoggedInUser.CompanyId + "/" + "Small/";
                            break;
                        case ImageSize.Medium:
                            filePath = mediumImagePath;
                            mediaUrl = "companyMM/" + LoggedInUser.CompanyId + "/" + "Medium/";
                            break;
                        case ImageSize.Large:
                            filePath = largeImagePath;
                            mediaUrl = "companyMM/" + LoggedInUser.CompanyId + "/" + "Large/";
                            break;
                        // Do Nothing
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    var fileWritten = false;
                    var i = 0;
                    while (!fileWritten)
                    {
                        if (i > 0)
                        {
                            if (!filename.Contains("_") && i == 1)
                            {
                                filename = filename + "_" + i;
                            }
                            else
                            {
                                filename = filename.Remove(filename.LastIndexOf("_", StringComparison.Ordinal)) + "_" + i;
                            }
                        }

                        copyFileName = filename + fileExtension;
                        var pathWithImageName = Path.Combine(filePath, copyFileName);
                        if (File.Exists(pathWithImageName)) i++;
                        else
                        {
                            using (var imageFile = new FileStream(pathWithImageName, FileMode.Create))
                            {
                                imageFile.Write(bytes, 0, bytes.Length);
                                imageFile.Flush();
                                fileWritten = true;
                            }
                            var productMetadata = new ProductMetadata
                            {
                                CompanyId = LoggedInUser.CompanyId,
                                Active = true,
                                ProductId = productId,
                                MediaType = MediaType.Photo,
                                MediaUrl = mediaUrl + copyFileName,
                                ImageSize = image.ImageSize,
                                WebActive = true
                            };

                            Save(productMetadata);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "Saved";
        }

        public IQueryable<ProductMetadata> GetActiveProductMetadatas(int distributorId)
        {
            return _dbContext.Set<ProductMetadata>().Where(x => x.CompanyId == distributorId && x.Active);
        }

        public PagedTaskDataInquiryResponse SearchProductMetadatas(PagedDataRequest requestInfo, Expression<Func<ProductMetadata, bool>> @where = null)
        {
            var query = _dbContext.Set<ProductMetadata>().Where(c => c.CompanyId == LoggedInUser.CompanyId);
            if (requestInfo.Active)
                query = query.Where(x => x.Active);
            var result = string.IsNullOrEmpty(requestInfo.SearchText)
                ? query
                : query.Where(s => s.MediaUrl.ToUpper().Contains(requestInfo.SearchText.ToUpper()));
            return FormatResultForPaging(requestInfo, result);
        }

        public IEnumerable<string> GetProductMetadatasByProductId(int productId)
        {
            var productMetadataUrls = _dbContext
                .Set<ProductMetadata>()
                .Where(x => x.Active && x.CompanyId == LoggedInUser.CompanyId 
                && x.ProductId == productId).Select(x => x.MediaUrl);
            return productMetadataUrls;
        }

        private static bool CheckFileExtesions(Image image)
        {
            var allowedExtensions = new[] { ".JPG", ".PNG", ".JPEG", ".TXT", ".PDF" };
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
    }
}
