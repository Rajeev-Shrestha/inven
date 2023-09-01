 using System.Linq;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper
{
    public class StageToStageViewModelMapper : MapperBase<Stage, StageViewModel>
    {
        public override StageViewModel Map(Stage entity)
        {
            var mapper = new SalesOpportunityToSalesOpportunityViewModelMapper();
            var stageViewModel = new StageViewModel
            {
                Id= entity.Id,
                StageName= entity.StageName,
                Rank= entity.Rank,
                Probability= entity.Probability,
                CompanyId= entity.CompanyId,
                Active= entity.Active,
                Version=entity.Version
            };
            if (entity.SalesOpportunities != null && entity.SalesOpportunities.Count > 0)
            {
                stageViewModel.SalesOpportunities = mapper.Map(entity.SalesOpportunities.ToList());
            }
            return stageViewModel;
        }

        public override Stage Map(StageViewModel viewModel)
        {
            return new Stage
            {
               Id= viewModel.Id??0,
               StageName= viewModel.StageName,
               Rank= viewModel.Rank,
               Probability= viewModel.Probability,
               CompanyId= viewModel.CompanyId,
               Active=viewModel.Active,
               Version=viewModel.Version
            };
        }
    }
}
