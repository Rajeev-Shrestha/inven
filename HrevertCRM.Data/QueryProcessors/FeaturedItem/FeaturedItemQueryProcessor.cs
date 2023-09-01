using Hrevert.Common.Security;
using HrevertCRM.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.ViewModels;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using HrevertCRM.Data.Mapper;
using PagedTaskDataInquiryResponse = HrevertCRM.Data.Common.PagedDataInquiryResponse<HrevertCRM.Data.ViewModels.FeaturedItemViewModel>;

namespace HrevertCRM.Data.QueryProcessors
{
    public class FeaturedItemQueryProcessor : QueryBase<FeaturedItem>, IFeaturedItemQueryProcessor
    {
        public FeaturedItemQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
        {
        }
        public FeaturedItem Update(FeaturedItem featuredItem)
        {
            var original = GetValidFeaturedItem(featuredItem.Id);
            ValidateAuthorization(original);
            CheckVersionMismatch(featuredItem, original);

            //pass value to original
            original.ImageType = featuredItem.ImageType;
            original.SortOrder = featuredItem.SortOrder;
            original.CompanyId = LoggedInUser.CompanyId;
            original.Active = featuredItem.Active;
            original.WebActive = featuredItem.WebActive;


            //validProduct.DateModified = DateTime.Now;
            _dbContext.Set<FeaturedItem>().Update(original);

            _dbContext.SaveChanges();
            return featuredItem;
        }

        public virtual FeaturedItem GetValidFeaturedItem(int featuredItemId)
        {
            var featuredItem = _dbContext.Set<FeaturedItem>().FirstOrDefault(sc => sc.Id == featuredItemId);
            if (featuredItem == null)
            {
                //  throw new RootObjectNotFoundException(ProductConstants.ProductQueryProcessorConstants.ProductNotFound);
                throw new RootObjectNotFoundException("Not Found.");
            }
            return featuredItem;
        }

        public FeaturedItem GetFeaturedItem(int featureItemId)
        {
            var product = _dbContext.Set<FeaturedItem>().Include(p => p.FeaturedItemMetadatas).FirstOrDefault(d => d.Id == featureItemId);
            return product;
        }

        public FeaturedItemViewModel GetFeaturedItemViewModel(int featureItemId)
        {
            //This mapper is for sending productmetadatas without medium images
            var mapper = new FeaturedItemToFeaturedItemViewModelMapper();
            var product = _dbContext.Set<FeaturedItem>().Include(p => p.FeaturedItemMetadatas).FirstOrDefault(d => d.Id == featureItemId);

            var metadatas = new List<FeaturedItemMetadata>();
            foreach (var i in product.FeaturedItemMetadatas)
            {
                if (i.Active)
                {
                    metadatas.Add(i);
                }

            }
            product.FeaturedItemMetadatas = metadatas;

            return mapper.Map(product);
        }

        public void SaveAllFeaturedItems(List<FeaturedItem> featureItems)
        {
            //products.ForEach(product => product.CompanyId = LoggedInUser.CompanyId);
            _dbContext.Set<FeaturedItem>().AddRange(featureItems);
            _dbContext.SaveChanges();
        }


        public FeaturedItem Save(FeaturedItem featuredItem)
        {
            featuredItem.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<FeaturedItem>().Add(featuredItem);
            _dbContext.SaveChanges();
            return featuredItem;
        }


        public IQueryable<FeaturedItem> GetAllActiveFeaturedItems()
        {
            var featuredItems = _dbContext.Set<FeaturedItem>().Where(p => p.Active && p.CompanyId == LoggedInUser.CompanyId);
            return featuredItems;
        }


        public bool Delete(int productId)
        {
            var doc = GetFeaturedItem(productId);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            var featuredItemMetadatas = _dbContext.Set<FeaturedItemMetadata>().Where(p => p.CompanyId == LoggedInUser.CompanyId && p.FeaturedItemId == productId);
            foreach (var item in featuredItemMetadatas)
            {
                item.Active = false;
                _dbContext.Set<FeaturedItemMetadata>().Update(item);
            }
            _dbContext.Set<FeaturedItem>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }

        public bool Exists(Expression<Func<FeaturedItem, bool>> where)
        {
            return _dbContext.Set<FeaturedItem>().Any(where);
        }

        public PagedTaskDataInquiryResponse GetFeaturedItems(PagedDataRequest requestInfo, Expression<Func<FeaturedItem, bool>> where = null)
        {
            var query = _dbContext.Set<FeaturedItem>().Where(FilterByActiveTrueAndCompany);
            query = where == null ? query : query.Where(where);
            //var enumerable = query as Product[] ?? query.ToArray();

            var totalItemCount = query.Count();
            var startIndex = ResultsPagingUtility.CalculateStartIndex(requestInfo.PageNumber, requestInfo.PageSize);
            var mapper = new FeaturedItemToFeaturedItemViewModelMapper();
            var docs = query.OrderBy(x => x.DateCreated).Skip(startIndex).Take(requestInfo.PageSize).Select(x => mapper.Map(x)).ToList();
            var queryResult = new QueryResult<FeaturedItemViewModel>(docs, totalItemCount, requestInfo.PageSize);
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

        public FeaturedItem[] GetFeaturedItems(Expression<Func<FeaturedItem, bool>> where = null)
        {
            var query = _dbContext.Set<FeaturedItem>().Where(FilterByActiveTrueAndCompany);

            query = where == null ? query : query.Where(where);
            var enumerable = query.ToArray();
            return enumerable;
        }

        public PagedTaskDataInquiryResponse GetActiveFeaturedItems(PagedDataRequest requestInfo, Expression<Func<FeaturedItem, bool>> where = null)
        {
            var query = _dbContext.Set<FeaturedItem>().Where(FilterByActiveTrueAndCompany);
            query = where == null ? query : query.Where(where);
            return FormatResultForPaging(requestInfo, query);
        }

        public PagedTaskDataInquiryResponse GetDeletedFeaturedItems(PagedDataRequest requestInfo, Expression<Func<FeaturedItem, bool>> where = null)
        {
            var query = _dbContext.Set<FeaturedItem>().Where(FilterByActiveFalseAndCompany);
            query = where == null ? query : query.Where(where);
            return FormatResultForPaging(requestInfo, query);
        }

        public PagedTaskDataInquiryResponse GetAllFeaturedItems(PagedDataRequest requestInfo, Expression<Func<FeaturedItem, bool>> where = null)
        {
            var query = _dbContext.Set<FeaturedItem>().Where(c => c.CompanyId == LoggedInUser.CompanyId);
            return FormatResultForPaging(requestInfo, query);
        }

        public PagedTaskDataInquiryResponse SearchActive(PagedDataRequest requestInfo, Expression<Func<FeaturedItem, bool>> where = null)
        {
            var filteredProduct = _dbContext.Set<FeaturedItem>().Where(FilterByActiveTrueAndCompany);
            var query = string.IsNullOrEmpty(requestInfo.SearchText) ? filteredProduct : filteredProduct.Where(s
                                                                  => s.Id == int.Parse(requestInfo.SearchText));
            return FormatResultForPaging(requestInfo, query);
        }

        public PagedTaskDataInquiryResponse SearchAll(PagedDataRequest requestInfo, Expression<Func<FeaturedItem, bool>> @where = null)
        {
            var filteredProduct = _dbContext.Set<FeaturedItem>().Where(x => x.CompanyId == LoggedInUser.CompanyId);
            var query = string.IsNullOrEmpty(requestInfo.SearchText) ? filteredProduct : filteredProduct.Where(s
                                                                  => s.Id == int.Parse(requestInfo.SearchText));
            //TODO: Make this LINQ query precompiled, using the method 

            return FormatResultForPaging(requestInfo, query);
        }

        private static PagedTaskDataInquiryResponse FormatResultForPaging(PagedDataRequest requestInfo, IQueryable<FeaturedItem> query)
        {
            var totalItemCount = query.Count();

            var startIndex = ResultsPagingUtility.CalculateStartIndex(requestInfo.PageNumber, requestInfo.PageSize);
            var mapper = new FeaturedItemToFeaturedItemViewModelMapper();
            var docs = query.OrderBy(x => x.DateCreated).Skip(startIndex).Take(requestInfo.PageSize).Select(
                s => mapper.Map(s)).ToList();

            var queryResult = new QueryResult<FeaturedItemViewModel>(docs, totalItemCount, requestInfo.PageSize);
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

        public FeaturedItem ActivateFeaturedItem(int id)
        {
            var original = GetValidFeaturedItem(id);
            ValidateAuthorization(original);

            original.Active = true;
            var featuredItemMetadatas = _dbContext.Set<FeaturedItemMetadata>().Where(p => p.CompanyId == LoggedInUser.CompanyId && p.FeaturedItemId == id);
            foreach (var item in featuredItemMetadatas)
            {
                item.Active = false;
                _dbContext.Set<FeaturedItemMetadata>().Update(item);
            }
            _dbContext.Set<FeaturedItem>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }


        public List<FeaturedItemViewModel> GetFeaturedItemsByProductCategoryIdAndImagetype(int imageTypeId, int categoryId)
        {
            //  var featuredItemsMetadata = _dbContext.Set<FeaturedItemMetadata>().Where(p=>p.Active && p.CompanyId==LoggedInUser.CompanyId);
            var featuredItems = _dbContext.Set<FeaturedItem>().Include(p => p.FeaturedItemMetadatas).Where(p => p.Active && (int)p.ImageType == imageTypeId && p.CompanyId == LoggedInUser.CompanyId && p.ItemId == categoryId);

            var featuredItemsList = new List<FeaturedItemViewModel>();
            var metadatas = new List<FeaturedItemMetadata>();
            var mapper = new FeaturedItemToFeaturedItemViewModelMapper();
            foreach (var item in featuredItems)
            {
                foreach (var i in item.FeaturedItemMetadatas)
                {
                    if (i.Active)
                    {
                        metadatas.Add(i);
                    }
                    item.FeaturedItemMetadatas = metadatas;
                }
                featuredItemsList.Add(mapper.Map(item));
            }

            return featuredItemsList;
        }

        public FeaturedItemBannerViewModel GetAllFeaturedItemsBannerImagesByCategoryId(int categoryId)
        {
            var featuredItemBannerViewModel = new FeaturedItemBannerViewModel();
            var featuredItemsIds = _dbContext.Set<FeaturedItem>().Where(p => p.Active && p.CompanyId == LoggedInUser.CompanyId && p.ItemId == categoryId).Select(p => p.Id).ToList();

            var fullWidthUlrs = new List<string>();
            var halfWidthUlrs = new List<string>();
            var quaterWidthUlrs = new List<string>();

            foreach (var i in featuredItemsIds)
            {
                var fullWidthImageUlrs = _dbContext.Set<FeaturedItemMetadata>().Where(p => p.FeaturedItemId == i && p.Active && p.ImageType == Hrevert.Common.Enums.ImageType.FullWidthImage).Select(p => p.MediaUrl).Distinct().ToList();
                var halfWidthImageUlrls = _dbContext.Set<FeaturedItemMetadata>().Where(p => p.FeaturedItemId == i && p.Active && p.ImageType == Hrevert.Common.Enums.ImageType.HalfWidthImage).Select(p => p.MediaUrl).Distinct().ToList();
                var quaterWidthImageUlrs = _dbContext.Set<FeaturedItemMetadata>().Where(p => p.FeaturedItemId == i && p.Active && p.ImageType == Hrevert.Common.Enums.ImageType.QuaterWidthImage).Select(p => p.MediaUrl).Distinct().ToList();
                fullWidthUlrs.AddRange(fullWidthImageUlrs);
                halfWidthUlrs.AddRange(halfWidthImageUlrls);
                quaterWidthUlrs.AddRange(quaterWidthImageUlrs);

            }
            featuredItemBannerViewModel.Id = categoryId;
            // featuredItemBannerViewModel.FullWidthUrls = fullWidthUlrs;
            // featuredItemBannerViewModel.HalfWidthUrls = halfWidthUlrs;
            // featuredItemBannerViewModel.QuaterWidthUrls = quaterWidthUlrs;

            return featuredItemBannerViewModel;
        }


        public List<FeaturedItemBannerViewModel> GetAllFeaturedItemsBannerImages()
        {
            var featuredItemBannerViewModelList = new List<FeaturedItemBannerViewModel>();
            var categoryIds = _dbContext.Set<ProductCategory>().Where(p => p.Active && p.CompanyId == LoggedInUser.CompanyId && p.ParentId == null).Select(p => p.Id).ToList();

            foreach (var i in categoryIds)
            {
                var featuredItemBannerViewModel = new FeaturedItemBannerViewModel
                {
                    BannerImageUrls = new List<string>(),
                    FullWidthUrls = new List<string>(),
                    HalfWidthUrls = new List<string>(),
                    QuaterWidthUrls = new List<string>()
                };

                var fullWidthImageUrls = _dbContext.Set<FeaturedItemMetadata>().Where(p => p.ItemId == i && p.Active && p.ImageType == Hrevert.Common.Enums.ImageType.FullWidthImage).Select(p => p.MediaUrl).Distinct().ToList();
                var halfWidthImageUrls = _dbContext.Set<FeaturedItemMetadata>().Where(p => p.ItemId == i && p.Active && p.ImageType == Hrevert.Common.Enums.ImageType.HalfWidthImage).Select(p => p.MediaUrl).Distinct().ToList();
                var quaterWidthImageUrls = _dbContext.Set<FeaturedItemMetadata>().Where(p => p.ItemId == i && p.Active && p.ImageType == Hrevert.Common.Enums.ImageType.QuaterWidthImage).Select(p => p.MediaUrl).Distinct().ToList();

                featuredItemBannerViewModel.Id = i;
                featuredItemBannerViewModel.BannerImageUrls.AddRange(fullWidthImageUrls);
                featuredItemBannerViewModel.BannerImageUrls.AddRange(halfWidthImageUrls);
                featuredItemBannerViewModel.BannerImageUrls.AddRange(quaterWidthImageUrls);
                // for fullwidth,halfwidth and quaterwidth
                featuredItemBannerViewModel.FullWidthUrls.AddRange(fullWidthImageUrls);
                featuredItemBannerViewModel.HalfWidthUrls.AddRange(halfWidthImageUrls);
                featuredItemBannerViewModel.QuaterWidthUrls.AddRange(quaterWidthImageUrls);

                featuredItemBannerViewModelList.Add(featuredItemBannerViewModel);
            }
            return featuredItemBannerViewModelList;
        }

        public bool DeleteRange(List<int?> featuredItemsId)
        {
            var featuredItemList = featuredItemsId.Select(featuredItemId => _dbContext.Set<FeaturedItem>().FirstOrDefault(x => x.Id == featuredItemId && x.CompanyId == LoggedInUser.CompanyId)).ToList();
            featuredItemList.ForEach(x => x.Active = false);
            _dbContext.Set<FeaturedItem>().UpdateRange(featuredItemList);
            _dbContext.SaveChanges();
            return true;
        }
    }
}