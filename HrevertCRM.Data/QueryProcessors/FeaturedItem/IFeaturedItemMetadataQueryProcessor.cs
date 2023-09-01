using HrevertCRM.Data.Common;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface IFeaturedItemMetadataQueryProcessor
    {
        FeaturedItemMetadata Update(FeaturedItemMetadata featureItemMetadata);
        FeaturedItemMetadata GetValidFeaturedItemMetadata(int featureItemMetadataId);
        FeaturedItemMetadataViewModel GetFeaturedItemMetadata(int featureItemMetadataId);
        FeaturedItemMetadata GetFeaturedItemMetadataById(int featureItemMetadataId);
        void SaveAllFeaturedItemMetadata(List<FeaturedItemMetadata> featureItemMetadatas);
        FeaturedItemMetadata Save(FeaturedItemMetadata featureItemMetadata);
        int SaveAll(List<FeaturedItemMetadata> featureditemMetadatas);
        FeaturedItemMetadata ActivateFeaturedItemMetadata(int id);
        bool Delete(int featuredItemMetadataId);
        bool Exists(Expression<Func<FeaturedItemMetadata, bool>> @where);
        FeaturedItemMetadata[] GetFeaturedItemMetadatas(Expression<Func<FeaturedItemMetadata, bool>> @where = null);
        //string SaveImage(int productId, IFormFileCollection files);
        string SaveFeaturedItemMetadatas(int featuredItemId, int itemId, List<Image> images);

        bool RemoveBannerImageByImageUrlAndImageTypeId(int imageTypeId, string imageUrl);
        PagedDataInquiryResponse<FeaturedItemMetadataViewModel> SearchFeatureItemMetadatas(PagedDataRequest requestInfo, Expression<Func<FeaturedItemMetadata, bool>> @where = null);
    }
}
