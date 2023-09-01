using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrevertCRM.Data.Mapper.Enumerations
{
    public class EntryMethodTypesToEntryMethodTypesViewModelMapper : MapperBase<EntryMethodTypes, EntryMethodTypeViewModel>
    {
        public override EntryMethodTypes Map(EntryMethodTypeViewModel viewModel)
        {
            return new EntryMethodTypes
            {
                Id = viewModel.Id ?? 0,
                Value = viewModel.Value,
                CompanyId = viewModel.CompanyId,
                Active = viewModel.Active,
                Version = viewModel.Version
            };
        }

        public override EntryMethodTypeViewModel Map(EntryMethodTypes entity)
        {
            return new EntryMethodTypeViewModel
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
