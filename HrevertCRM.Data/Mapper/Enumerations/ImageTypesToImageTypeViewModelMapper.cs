using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrevertCRM.Data.Mapper.Enumerations
{
    public class ImageTypesToImageTypeViewModelMapper : MapperBase<ImageTypes, ImageTypeViewModel>
    {
        public override ImageTypes Map(ImageTypeViewModel viewModel)
        {
            return new ImageTypes
            {
                Id = viewModel.Id ?? 0,
                Value = viewModel.Value,
                CompanyId = viewModel.CompanyId,
                Active = viewModel.Active,
                Version = viewModel.Version
            };
        }

        public override ImageTypeViewModel Map(ImageTypes entity)
        {
            return new ImageTypeViewModel
            {
                Id = entity.Id,
                Value = entity.Value,
                CompanyId = entity.CompanyId,
                Active = entity.Active,
                Version = entity.Version
            };
        }
    }
}
