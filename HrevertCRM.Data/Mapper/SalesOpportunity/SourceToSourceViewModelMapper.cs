using System;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper
{
    public class SourceToSourceViewModelMapper : MapperBase<Source, SourceViewModel>
    {
        public override SourceViewModel Map(Source entity)
        {
            return new SourceViewModel
            {
                Id= entity.Id,
                SourceName= entity.SourceName,
                CompanyId= entity.CompanyId,
                Active= entity.Active,
                Version=entity.Version
            };
        }

        public override Source Map(SourceViewModel viewModel)
        {
            return new Source
            {
                Id=viewModel.Id??0,
                SourceName= viewModel.SourceName,
                CompanyId= viewModel.CompanyId,
                Active=viewModel.Active,
                Version=viewModel.Version
            };
        }
    }
}
