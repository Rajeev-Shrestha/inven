using HrevertCRM.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.ViewModels;
using System.Linq.Expressions;
using PagedTaskDataInquiryResponse =
    HrevertCRM.Data.Common.PagedDataInquiryResponse<HrevertCRM.Data.ViewModels.CompanyLogoViewModel>;
using Hrevert.Common.Security;
using Microsoft.AspNetCore.Hosting;
using Hrevert.Common.Constants;
using HrevertCRM.Data.Mapper;
using System.IO;
using System.Text.RegularExpressions;

namespace HrevertCRM.Data.QueryProcessors
{
    public class CompanyLogoQueryProcessor : QueryBase<CompanyLogo>, ICompanyLogoQueryProcessor
    {
        private readonly IHostingEnvironment _env;
        public CompanyLogoQueryProcessor(IUserSession userSession, IDbContext dbContext, IHostingEnvironment env) : base(userSession, dbContext)
        {
            _env = env;
        }


        public CompanyLogo Update(CompanyLogo companyLogo)
        {
            var original = GetValidCompanyLogo(companyLogo.Id);
            ValidateAuthorization(original);
            CheckVersionMismatch(companyLogo, original);

            // pass value to original
            original.CompanyName = companyLogo.CompanyName;
            original.CompanyId = companyLogo.CompanyId;
            original.Active = companyLogo.Active;
            original.WebActive = companyLogo.WebActive;
            _dbContext.Set<CompanyLogo>().Update(original);
            _dbContext.SaveChanges();
            return companyLogo;
        }

        public virtual CompanyLogo GetValidCompanyLogo(int companyLogoId)
        {
            var companyLogo = _dbContext.Set<CompanyLogo>().FirstOrDefault(sc => sc.Id == companyLogoId);
            if (companyLogo == null)
            {
                throw new RootObjectNotFoundException(CompanyLogoConstants.CompanyLogoQueryProcessorConstants.CompanyLogoNotFound);
            }
            return companyLogo;
        }

        public CompanyLogo ActivateCompanyLogo(int id)
        {
            var original = GetCompanyLogo(id);
            ValidateAuthorization(original);
            original.Active = true;
            _dbContext.Set<CompanyLogo>().Update(original);
            _dbContext.SaveChanges();
            return original;

        }

        public bool Delete(int companyLogoId)
        {
            var doc = GetCompanyLogo(companyLogoId);
            var result = 0;
            if (doc == null) return result > 0;
            _dbContext.Set<CompanyLogo>().Remove(doc);
            result = _dbContext.SaveChanges();
            return result > 0;

         

        }

        public CompanyLogo Save(CompanyLogo companyLogo)
        {
            companyLogo.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<CompanyLogo>().Add(companyLogo);
            _dbContext.SaveChanges();
            return companyLogo;
        }

        public bool Exists(Expression<Func<CompanyLogo, bool>> where)
        {
            if (_dbContext.Set<CompanyLogo>().Any(where))
            {

            }
            return _dbContext.Set<CompanyLogo>().Any(where);
        }

        public CompanyLogo GetActiveCompanyLogos()
        {
            return _dbContext.Set<CompanyLogo>().FirstOrDefault(p =>p.Active && p.CompanyId == LoggedInUser.CompanyId);
        }

        public CompanyLogo GetAllCompanyLogos()
        {
            return _dbContext.Set<CompanyLogo>().FirstOrDefault(p=>p.CompanyId==LoggedInUser.CompanyId);
        }

        public int GetCompanyIdByCompnayLogoId(int carouselId)
        {
            return _dbContext.Set<CompanyLogo>().Where(p => p.Id == carouselId).Select(p => p.CompanyId).Single();
        }

        public CompanyLogo GetCompanyLogo(int companyLogoId)
        {
            var carousel = _dbContext.Set<CompanyLogo>().FirstOrDefault(d => d.Id == companyLogoId);
            return carousel;
        }

        public CompanyLogo[] GetCompanyLogos(Expression<Func<CompanyLogo, bool>> where = null)
        {
            var query = _dbContext.Set<CompanyLogo>().Where(FilterByActiveTrueAndCompany);

            query = where == null ? query : query.Where(where);
            var enumerable = query.ToArray();
            return enumerable;
        }

        public CompanyLogoViewModel GetCompanyLogoViewModel(int companyLogoId)
        {
            var mapper = new CompanyLogoToCompanyLogoViewModelMapper();
            var carousel = _dbContext.Set<CompanyLogo>().FirstOrDefault(d => d.Id == companyLogoId);

            return mapper.Map(carousel);
        }

        public CompanyLogo GetDeletedCompanyLogos()
        {
            return _dbContext.Set<CompanyLogo>().FirstOrDefault(p =>p.Active == false && p.CompanyId == LoggedInUser.CompanyId);
        }

       
        public void SaveAllCompanyLogos(List<CompanyLogo> companyLogos)
        {
            _dbContext.Set<CompanyLogo>().AddRange(companyLogos);
            _dbContext.SaveChanges();
        }

        public string SaveImage(int savedCompanylogoId, Image image)
        {
            try
            {
                //Check if the directory exists or create a new one
                var bytes = Convert.FromBase64String(image.ImageBase64);
                var filename = Regex.Replace(Path.GetFileNameWithoutExtension(image.FileName), @"\s+", "");
                var fileExtension = Path.GetExtension(image.FileName);
                var path = Path.Combine(_env.WebRootPath, @"companyMM\LogoImages\" + LoggedInUser.CompanyId);

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

                        var companyLogo = new CompanyLogo
                        {
                            Id = savedCompanylogoId,
                            MediaUrl = "companyMM/LogoImages/" + LoggedInUser.CompanyId + "/" + copyfileName
                        };

                        if (UpdateImage(companyLogo) <= 0) return "Failed to Save Image";
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "Saved";
        }

        public string GetActiveCompanyLogo()
        {
            var companyLogo = _dbContext.Set<CompanyLogo>().FirstOrDefault(p => p.Active && p.CompanyId == LoggedInUser.CompanyId);
            if (companyLogo == null) return null;
            var logoPath = _env.ContentRootPath+ "/wwwroot/"+ companyLogo.MediaUrl;
            var imageArray = File.ReadAllBytes(logoPath);
            var base64ImageRepresentation = Convert.ToBase64String(imageArray);
            return base64ImageRepresentation;
        }

        public PagedDataInquiryResponse<CompanyLogoViewModel> SearchActive(PagedDataRequest requestInfo, Expression<Func<CompanyLogo, bool>> where = null)
        {
            var filteredCarousel = _dbContext.Set<CompanyLogo>().Where(FilterByActiveTrueAndCompany);
            var query = String.IsNullOrEmpty(requestInfo.SearchText)
                ? filteredCarousel
                : filteredCarousel.Where(s => s.CompanyName.ToString().Contains(requestInfo.SearchText.ToUpper())
                );
            return FormatResultForPaging(requestInfo, query);
        }

        private PagedTaskDataInquiryResponse FormatResultForPaging(PagedDataRequest requestInfo, IQueryable<CompanyLogo> query)
        {
            var totalItemCount = query.Count();
            var startIndex = ResultsPagingUtility.CalculateStartIndex(requestInfo.PageNumber, requestInfo.PageSize);
            var mapper = new CompanyLogoToCompanyLogoViewModelMapper();
            var docs =
                query.OrderBy(x => x.DateCreated)
                    .Skip(startIndex)
                    .Take(requestInfo.PageSize)
                    .Select(
                        s => mapper.Map(s)).ToList();

            var queryResult = new QueryResult<CompanyLogoViewModel>(docs, totalItemCount, requestInfo.PageSize);
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

        public PagedDataInquiryResponse<CompanyLogoViewModel> SearchAll(PagedDataRequest requestInfo, Expression<Func<CompanyLogo, bool>> where = null)
        {
            var filteredCarousel = _dbContext.Set<CompanyLogo>().Where(x => x.CompanyId == LoggedInUser.CompanyId);
            var query = string.IsNullOrEmpty(requestInfo.SearchText)
                ? filteredCarousel
                : filteredCarousel.Where(s
                    => s.CompanyName.ToString().Contains(requestInfo.SearchText.ToUpper()));
            //TODO: Make this LINQ query precompiled, using the method 

            return FormatResultForPaging(requestInfo, query);
        }

        private static void CreateDirectory(string path)
        {
            var directory = new DirectoryInfo(path);

            if (directory.Exists == false)
            {
                directory.Create();
            }
        }
        public int UpdateImage(CompanyLogo carousel)
        {
            var original = GetCompanyLogo(carousel.Id);
            ValidateAuthorization(original);
            //CheckVersionMismatch(carousel, original);

            original.MediaUrl = carousel.MediaUrl;
            _dbContext.Set<CompanyLogo>().Update(original);
            return _dbContext.SaveChanges();
        }
    }
}
