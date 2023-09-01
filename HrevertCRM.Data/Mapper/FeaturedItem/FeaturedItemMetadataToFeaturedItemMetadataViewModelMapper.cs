using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hrevert.Common.Enums;

namespace HrevertCRM.Data.Mapper
{
    public class FeaturedItemMetadataToFeaturedItemMetadataViewModelMapper: MapperBase<FeaturedItemMetadata, FeaturedItemMetadataViewModel>
    {
        public override FeaturedItemMetadata Map(FeaturedItemMetadataViewModel viewModel)
        {
            return new FeaturedItemMetadata
            {
                Id = viewModel.Id??0,
                FeaturedItemId = viewModel.FeaturedItemId,
                MediaType = viewModel.MediaType,
                ItemId = viewModel.ItemId,
                MediaUrl = viewModel.MediaUrl,
                WebActive = viewModel.WebActive,
                ImageType = viewModel.ImageType,
                Version = viewModel.Version,
                Active = viewModel.Active,
                CompanyId = viewModel.CompanyId
            };
        }

        public override FeaturedItemMetadataViewModel Map(FeaturedItemMetadata entity)
        {
            return new FeaturedItemMetadataViewModel
            {
                Id = entity.Id,
                FeaturedItemId = entity.FeaturedItemId,
                ItemId = entity.ItemId,
                MediaType = entity.MediaType,
                MediaUrl = entity.MediaUrl,
                WebActive = entity.WebActive,
                ImageType = entity.ImageType,
                Active = entity.Active,
                Version = entity.Version,
                CompanyId = entity.CompanyId

            };
        }

    }
}
