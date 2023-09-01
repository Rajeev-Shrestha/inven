using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Hrevert.Common.Constants;
using Hrevert.Common.Security;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.Mapper;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using Microsoft.AspNetCore.Hosting;
using PagedTaskDataInquiryResponse =
    HrevertCRM.Data.Common.PagedDataInquiryResponse<HrevertCRM.Data.ViewModels.CarouselViewModel>;

namespace HrevertCRM.Data.QueryProcessors
{
    public class CarouselQueryProcessor : QueryBase<Carousel>, ICarouselQueryProcessor
    {
        private readonly IHostingEnvironment _env;

        public CarouselQueryProcessor(IUserSession userSession, IDbContext dbContext,
            IHostingEnvironment env)
            : base(userSession, dbContext)
        {
            _env = env;
        }

        public Carousel Update(Carousel carousel)
        {
            var original = GetValidCarousel(carousel.Id);
            ValidateAuthorization(original);
            CheckVersionMismatch(carousel, original);

            //pass value to original
            original.ItemId = carousel.ItemId;
            original.ProductOrCategory = carousel.ProductOrCategory;
            original.CompanyId = LoggedInUser.CompanyId;
            original.Active = carousel.Active;
            original.WebActive = carousel.WebActive;

            _dbContext.Set<Carousel>().Update(original);
            _dbContext.SaveChanges();
            return carousel;
        }

        public virtual Carousel GetValidCarousel(int carouselId)
        {
            var carousel = _dbContext.Set<Carousel>().FirstOrDefault(sc => sc.Id == carouselId);
            if (carousel == null)
            {
                throw new RootObjectNotFoundException(CarouselConstants.CarouselQueryProcessorConstants.CarouselNotFound);
            }
            return carousel;
        }

        public Carousel GetCarousel(int carouselId)
        {
            var carousel = _dbContext.Set<Carousel>().FirstOrDefault(d => d.Id == carouselId);
            return carousel;
        }

        public CarouselViewModel GetCarouselViewModel(int carouselId)
        {
            var mapper = new CarouselToCarouselViewModelMapper();
            var carousel = _dbContext.Set<Carousel>().FirstOrDefault(d => d.Id == carouselId);

            return mapper.Map(carousel);
        }


        public void SaveAllCarousel(List<Carousel> carousels)
        {
            _dbContext.Set<Carousel>().AddRange(carousels);
            _dbContext.SaveChanges();
        }

        public int GetCompanyIdByCarouselId(int carouselId)
        {
            return _dbContext.Set<Carousel>().Where(p => p.Id == carouselId).Select(p => p.CompanyId).Single();
        }

        public Carousel Save(Carousel carousel)
        {
            carousel.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<Carousel>().Add(carousel);
            _dbContext.SaveChanges();
            return carousel;
        }

        public bool Delete(int carouselId)
        {
            var doc = GetCarousel(carouselId);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<Carousel>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }

        public bool Exists(Expression<Func<Carousel, bool>> where)
        {
            if (_dbContext.Set<Carousel>().Any(where))
            {

            }
            return _dbContext.Set<Carousel>().Any(where);
        }

        public PagedTaskDataInquiryResponse GetCarousels(PagedDataRequest requestInfo,
            Expression<Func<Carousel, bool>> where = null)
        {
            var query = _dbContext.Set<Carousel>().Where(FilterByActiveTrueAndCompany);
            query = where == null ? query : query.Where(where);
            //var enumerable = query as Carousel[] ?? query.ToArray();

            var totalItemCount = query.Count();
            var startIndex = ResultsPagingUtility.CalculateStartIndex(requestInfo.PageNumber, requestInfo.PageSize);
            var mapper = new CarouselToCarouselViewModelMapper();
            var docs =
                query.OrderBy(x => x.DateCreated)
                    .Skip(startIndex)
                    .Take(requestInfo.PageSize)
                    .Select(s => mapper.Map(s))
                    .ToList();
            var queryResult = new QueryResult<CarouselViewModel>(docs, totalItemCount, requestInfo.PageSize);
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

        public Carousel[] GetCarousels(Expression<Func<Carousel, bool>> where = null)
        {
            var query = _dbContext.Set<Carousel>().Where(FilterByActiveTrueAndCompany);

            query = where == null ? query : query.Where(where);
            var enumerable = query.ToArray();
            return enumerable;
        }
       
        public string SaveImage(Carousel savedCarousel, Image image)
        {
            try
            {
                if (!CheckFileExtesions(image)) return "Please choose only Image and Pdf files.";

                //Check if the directory exists or create a new one
                var bytes = Convert.FromBase64String(image.ImageBase64);
                var filename = Regex.Replace(Path.GetFileNameWithoutExtension(image.FileName), @"\s+", "");
                var fileExtension = Path.GetExtension(image.FileName);
                var path = Path.Combine(_env.WebRootPath, @"companyMM\CarouselImages\" + LoggedInUser.CompanyId);

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

                        var carousel = new Carousel
                        {
                            Id = savedCarousel.Id,
                            ImageUrl = "companyMM/CarouselImages/" + LoggedInUser.CompanyId + "/" + copyfileName,
                            Version = savedCarousel.Version
                        };

                        if(UpdateImage(carousel) <= 0) return "Failed to Save Image";
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "Saved";
        }

        public IQueryable<Carousel> SearchCarousels(bool active, string searchText)
        {
            var carousels = _dbContext.Set<Carousel>().Where(p => p.CompanyId == LoggedInUser.CompanyId);
            if (active)
                carousels = carousels.Where(x => x.Active);
            var query = string.IsNullOrEmpty(searchText) || searchText.Contains("null")
               ? carousels
               : carousels.Where(s
                   => s.ProductOrCategory.ToString().Contains(searchText.ToUpper()));
            return query;
        }


        public Carousel ActivateCarousel(int id)
        {
            var original = GetValidCarousel(id);
            ValidateAuthorization(original);

            original.Active = true;
            _dbContext.Set<Carousel>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        private static bool CheckFileExtesions(Image image)
        {
            var allowedExtensions = new[] {".JPG", ".PNG", ".JPEG", ".TXT", ".PDF"};
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

        public int UpdateImage(Carousel carousel)
        {
            var original = GetValidCarousel(carousel.Id);
            ValidateAuthorization(original);
            CheckVersionMismatch(carousel, original);

            original.ImageUrl = carousel.ImageUrl;
            _dbContext.Set<Carousel>().Update(original);
            return _dbContext.SaveChanges();
        }
    }
}
