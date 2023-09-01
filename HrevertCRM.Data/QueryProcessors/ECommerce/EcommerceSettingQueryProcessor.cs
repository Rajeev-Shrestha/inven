using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Hrevert.Common.Constants;
using Hrevert.Common.Enums;
using Hrevert.Common.Security;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.Mapper;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using Microsoft.AspNetCore.Hosting;
using PagedTaskDataInquiryResponse = HrevertCRM.Data.Common.PagedDataInquiryResponse<HrevertCRM.Data.ViewModels.EcommerceSettingViewModel>;

namespace HrevertCRM.Data.QueryProcessors
{
    public class EcommerceSettingQueryProcessor : QueryBase<EcommerceSetting>, IEcommerceSettingQueryProcessor
    {
        private readonly IHostingEnvironment _env;

        public EcommerceSettingQueryProcessor(IUserSession userSession, IDbContext dbContext, IHostingEnvironment env)
            : base(userSession, dbContext)
        {
            _env = env;
        }
        public EcommerceSetting Update(EcommerceSetting ecommerceSetting)
        {
            var original = GetValidEcommerceSetting(ecommerceSetting.Id);
            ValidateAuthorization(original);
            CheckVersionMismatch(ecommerceSetting, original);

            //pass value to original
            original.IncludeQuantityInSalesOrder = ecommerceSetting.IncludeQuantityInSalesOrder;
            original.DisplayOutOfStockItems = ecommerceSetting.DisplayOutOfStockItems;
            original.DisplayQuantity = ecommerceSetting.DisplayQuantity;
            original.ProductPerCategory = ecommerceSetting.ProductPerCategory;
            original.CompanyId = LoggedInUser.CompanyId;
            original.Active = ecommerceSetting.Active;
            original.WebActive = ecommerceSetting.WebActive;
            original.DecreaseQuantityOnOrder = ecommerceSetting.DecreaseQuantityOnOrder;
            original.ImageUrl = ecommerceSetting.ImageUrl;
            original.DueDatePeriod = ecommerceSetting.DueDatePeriod;

            _dbContext.Set<EcommerceSetting>().Update(original);
            _dbContext.SaveChanges();
            return ecommerceSetting;
        }
        public virtual EcommerceSetting GetValidEcommerceSetting(int ecommerceSettingId)
        {
            var ecommerceSetting = _dbContext.Set<EcommerceSetting>().FirstOrDefault(sc => sc.Id == ecommerceSettingId);
            if (ecommerceSetting == null)
            {
                throw new RootObjectNotFoundException(EcommerceSettingConstants.EcommerceSettingQueryProcessorConstants.EcommerceSettingNotFound);
            }
            return ecommerceSetting;
        }
        public EcommerceSetting GetEcommerceSetting(int ecommerceSettingId)
        {
            var ecommerceSetting = _dbContext.Set<EcommerceSetting>().FirstOrDefault(d => d.Id == ecommerceSettingId);
            return ecommerceSetting;
        }
        public EcommerceSettingViewModel GetEcommerceSettingViewModel(int ecommerceSettingId)
        {
            var mapper = new EcommerceSettingToEcommerceSettingViewModelMapper();
            var ecommerceSetting = _dbContext.Set<EcommerceSetting>().FirstOrDefault(d => d.Id == ecommerceSettingId);

            return mapper.Map(ecommerceSetting);
        }

        public void SaveAllEcommerceSetting(List<EcommerceSetting> ecommerceSettings)
        {
            _dbContext.Set<EcommerceSetting>().AddRange(ecommerceSettings);
            _dbContext.SaveChanges();
        }

        public int GetCompanyIdByEcommerceSettingId(int ecommerceSettingId)
        {
            return _dbContext.Set<EcommerceSetting>().Where(p => p.Id == ecommerceSettingId).Select(p => p.CompanyId).Single();
        }

        public EcommerceSetting Save(EcommerceSetting ecommerceSetting)
        {
            ecommerceSetting.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<EcommerceSetting>().Add(ecommerceSetting);
            _dbContext.SaveChanges();
            return ecommerceSetting;
        }

        public bool IsExist()
        {
            return _dbContext.Set<EcommerceSetting>().Any(x => x.CompanyId == LoggedInUser.CompanyId);
        }

        public string SaveEcommerceLogo(Image image)
        {
            try
            {
                var ecommerceSetting = _dbContext.Set<EcommerceSetting>()
                    .FirstOrDefault(x => x.CompanyId == LoggedInUser.CompanyId);
                var directoryPath = Path.Combine(_env.WebRootPath, @"companyMM\EcommerceLogo\" + LoggedInUser.CompanyId);
                CreateDirectory(directoryPath);

                var logoPath = Path.Combine(_env.WebRootPath, @"companyMM\EcommerceLogo\" + LoggedInUser.CompanyId);
                CreateDirectory(logoPath);
                if (!CheckFileExtesions(image)) return "Please choose only Image files.";

                //Check if the directory exists or create a new one
                var bytes = Convert.FromBase64String(image.ImageBase64);
                var filename = Regex.Replace(Path.GetFileNameWithoutExtension(image.FileName), @"\s+", "");
                var fileExtension = Path.GetExtension(image.FileName);

                //Create path with filename

                var filePath = logoPath;
                var mediaUrl = "companyMM/EcommerceLogo/" + LoggedInUser.CompanyId + "/";

                var copyFileName = filename + fileExtension;
                var pathWithImageName = Path.Combine(filePath, copyFileName);

                using (var imageFile = new FileStream(pathWithImageName, FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }
                ecommerceSetting.ImageUrl = mediaUrl + copyFileName;
                Update(ecommerceSetting);

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "Saved";
        }

        public bool DeleteLogo(string imageUrl)
        {
            var urlForImage = "CompanyMM\\EcommerceLogo\\" + LoggedInUser.CompanyId + "\\" + imageUrl;
            var mainUrlForImage = Path.Combine(_env.WebRootPath, urlForImage);
            File.Delete(mainUrlForImage);
            var imageUrl1 = "companyMM/EcommerceLogo/" + LoggedInUser.CompanyId + "/" + imageUrl;
            var doc1 = _dbContext.Set<EcommerceSetting>()
                .FirstOrDefault(x => x.CompanyId == LoggedInUser.CompanyId && x.ImageUrl == imageUrl1);
            if (doc1 != null)
            {
                doc1.ImageUrl = "";
                _dbContext.Set<EcommerceSetting>().Update(doc1);
            }
            _dbContext.SaveChanges();
            return true;
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

        public IQueryable<EcommerceSetting> SearchEcommerceSettings(bool active, string searchText)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int ecommerceSettingId)
        {
            var doc = GetEcommerceSetting(ecommerceSettingId);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<EcommerceSetting>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }

        public bool Exists(Expression<Func<EcommerceSetting, bool>> where)
        {
            return _dbContext.Set<EcommerceSetting>().Any(where);
        }

        public PagedTaskDataInquiryResponse GetEcommerceSettings(PagedDataRequest requestInfo, Expression<Func<EcommerceSetting, bool>> where = null)
        {
            var query = _dbContext.Set<EcommerceSetting>().Where(FilterByActiveTrueAndCompany);
            query = where == null ? query : query.Where(where);
            //var enumerable = query as EcommerceSetting[] ?? query.ToArray();

            var totalItemCount = query.Count();
            var startIndex = ResultsPagingUtility.CalculateStartIndex(requestInfo.PageNumber, requestInfo.PageSize);
            var mapper = new EcommerceSettingToEcommerceSettingViewModelMapper();
            var docs = query.OrderBy(x => x.DateCreated).Skip(startIndex).Take(requestInfo.PageSize).Select(s => mapper.Map(s)).ToList();
            var queryResult = new QueryResult<EcommerceSettingViewModel>(docs, totalItemCount, requestInfo.PageSize);
            var inquiryResponse = new PagedTaskDataInquiryResponse
            {
                Items = docs,
                TotalRecords = totalItemCount,
                PageCount = queryResult.TotalPageCount,
                PageNumber = requestInfo.PageNumber,
                PageSize = requestInfo.PageSize
            };
            return inquiryResponse;
        }

        public EcommerceSetting[] GetEcommerceSettings(Expression<Func<EcommerceSetting, bool>> where = null)
        {
            var query = _dbContext.Set<EcommerceSetting>().Where(FilterByActiveTrueAndCompany);

            query = where == null ? query : query.Where(where);
            var enumerable = query.ToArray();
            return enumerable;
        }

        public EcommerceSetting GetActiveEcommerceSettings()
        {
            var ecommerceSettings = _dbContext.Set<EcommerceSetting>().FirstOrDefault(p => p.Active && p.CompanyId == LoggedInUser.CompanyId);
            if (ecommerceSettings != null) return ecommerceSettings;

            var eSettingViewModel = new EcommerceSettingViewModel
            {
                CompanyId = LoggedInUser.CompanyId,
                Active = true,
                ProductPerCategory = 4,
                DisplayQuantity = DisplayQuantity.InStockOutStock,
                DisplayOutOfStockItems = false,
                IncludeQuantityInSalesOrder = false,
                WebActive = true
            };

            var mapper = new EcommerceSettingToEcommerceSettingViewModelMapper();
            ecommerceSettings = Save(mapper.Map(eSettingViewModel));

            return ecommerceSettings;
        }

        public EcommerceSetting ActivateEcommerceSetting(int id)
        {
            var original = GetValidEcommerceSetting(id);
            ValidateAuthorization(original);

            original.Active = true;
            _dbContext.Set<EcommerceSetting>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
    }
}
