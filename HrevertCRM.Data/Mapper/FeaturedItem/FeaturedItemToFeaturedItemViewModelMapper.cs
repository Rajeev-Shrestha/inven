using Hrevert.Common.Enums;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrevertCRM.Data.Mapper
{
    public class FeaturedItemToFeaturedItemViewModelMapper : MapperBase<FeaturedItem, FeaturedItemViewModel>
    {
        public override FeaturedItem Map(FeaturedItemViewModel viewModel)
        {
            var featuredItem =  new FeaturedItem
            {
                Id = viewModel.Id ?? 0,
                Active = viewModel.Active,
                WebActive = viewModel.WebActive,
                Version = viewModel.Version,
                ImageType = viewModel.ImageType,
                SortOrder = viewModel.SortOrder,
                CompanyId = viewModel.CompanyId,
                ItemId = viewModel.ItemId, 
            };

                return featuredItem;
        }

        public override FeaturedItemViewModel Map(FeaturedItem entity)
        {
            var featuredItemViewModel = new FeaturedItemViewModel
            {
                Id = entity.Id,
                Active = entity.Active,
                WebActive = entity.WebActive,
                ImageType = entity.ImageType, 
                SortOrder = entity.SortOrder,
                Version =  entity.Version,
                CompanyId = entity.CompanyId,
                ItemId = entity.ItemId
            };

            if (entity.FeaturedItemMetadatas == null || entity.FeaturedItemMetadatas.Count <= 0) return featuredItemViewModel;
            {
                featuredItemViewModel.FullWidthImageUrls = entity.FeaturedItemMetadatas.Where(x => x.ImageType == ImageType.FullWidthImage).Select(s => s.MediaUrl).Distinct().ToList();
                featuredItemViewModel.HalfWidthImageUrls = entity.FeaturedItemMetadatas.Where(x => x.ImageType == ImageType.HalfWidthImage).Select(s => s.MediaUrl).Distinct().ToList();
                featuredItemViewModel.QuaterWidthImageUrls = entity.FeaturedItemMetadatas.Where(x => x.ImageType == ImageType.QuaterWidthImage).Select(s => s.MediaUrl).Distinct().ToList();
            }


            return featuredItemViewModel;
        }
    }
}
