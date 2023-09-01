using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrevertCRM.Data.Mapper
{
    public class ReasonClosedToReasonClosedViewModelMapper : MapperBase<ReasonClosed, ReasonClosedViewModel>
    {
        public override ReasonClosedViewModel Map(ReasonClosed entity)
        {
            return new ReasonClosedViewModel
            {
                Id = entity.Id,
                Reason = entity.Reason,
                CompanyId= entity.CompanyId,
                Active = entity.Active,
                Version = entity.Version
            };
        }

        public override ReasonClosed Map(ReasonClosedViewModel viewModel)
        {
            return new ReasonClosed
            {
                Id = viewModel.Id ?? 0,
                Reason = viewModel.Reason,
                CompanyId= viewModel.CompanyId,
                Active = viewModel.Active,
                Version = viewModel.Version
            };
        }
    }
}
