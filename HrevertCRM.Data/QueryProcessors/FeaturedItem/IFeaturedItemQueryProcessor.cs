using HrevertCRM.Data.Common;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface IFeaturedItemQueryProcessor
    {
        FeaturedItem Update(FeaturedItem featuredItem);
        FeaturedItem GetFeaturedItem(int featuredItemId);
        FeaturedItemViewModel GetFeaturedItemViewModel(int featuredItemId);
        void SaveAllFeaturedItems(List<FeaturedItem> featuredItems);
        bool Delete(int featuredItemId);
        bool Exists(Expression<Func<FeaturedItem, bool>> where);
        PagedDataInquiryResponse<FeaturedItemViewModel> GetActiveFeaturedItems(PagedDataRequest requestInfo, Expression<Func<FeaturedItem, bool>> @where = null);
        PagedDataInquiryResponse<FeaturedItemViewModel> GetDeletedFeaturedItems(PagedDataRequest requestInfo, Expression<Func<FeaturedItem, bool>> @where = null);
        PagedDataInquiryResponse<FeaturedItemViewModel> GetAllFeaturedItems(PagedDataRequest requestInfo, Expression<Func<FeaturedItem, bool>> @where = null);
        PagedDataInquiryResponse<FeaturedItemViewModel> SearchActive(PagedDataRequest requestInfo, Expression<Func<FeaturedItem, bool>> @where = null);
        PagedDataInquiryResponse<FeaturedItemViewModel> SearchAll(PagedDataRequest requestInfo, Expression<Func<FeaturedItem, bool>> @where = null);
        FeaturedItem ActivateFeaturedItem(int id);
        FeaturedItem[] GetFeaturedItems(Expression<Func<FeaturedItem, bool>> where = null);
        FeaturedItem Save(FeaturedItem featuredItem);
        IQueryable<FeaturedItem> GetAllActiveFeaturedItems();
       
        List<FeaturedItemViewModel> GetFeaturedItemsByProductCategoryIdAndImagetype(int imageType, int categoryId);

        FeaturedItemBannerViewModel GetAllFeaturedItemsBannerImagesByCategoryId(int categoryId);
        List<FeaturedItemBannerViewModel> GetAllFeaturedItemsBannerImages();
        bool DeleteRange(List<int?> featuredItemsId);
    }
}
