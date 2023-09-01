using System;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper
{
    public class GradeToGradeViewModelMapper : MapperBase<Grade, GradeViewModel>
    {
        public override GradeViewModel Map(Grade entity)
        {
            return new GradeViewModel
            {
                Id= entity.Id,
                GradeName= entity.GradeName,
                Rank= entity.Rank,
                CompanyId= entity.CompanyId,
                Active= entity.Active,
                Version= entity.Version
            };
        }

        public override Grade Map(GradeViewModel viewModel)
        {
            return new Grade
            {
                Id = viewModel.Id??0,
                GradeName = viewModel.GradeName,
                Rank = viewModel.Rank,
                CompanyId= viewModel.CompanyId,
                Active= viewModel.Active,
                Version= viewModel.Version
            };
        }
    }
}
