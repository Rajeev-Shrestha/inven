using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;

namespace HrevertCRM.Data.Mapper.Enumerations
{
    public class SuffixTypeToSuffixTypeViewModelMapper : MapperBase<SuffixTypes, SuffixTypeViewModel>
    {
        public override SuffixTypes Map(SuffixTypeViewModel viewModel)
        {
            return new SuffixTypes
            {
                Id = viewModel.Id ?? 0,
                Value = viewModel.Value,
                CompanyId = viewModel.CompanyId,
                Active = viewModel.Active,
                Version = viewModel.Version
            };
        }

        public override SuffixTypeViewModel Map(SuffixTypes entity)
        {
            return new SuffixTypeViewModel
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
