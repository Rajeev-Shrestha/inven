using System;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper
{
    public class SalesOpportunityToSalesOpportunityViewModelMapper : MapperBase<SalesOpportunity, SalesOpportunityViewModel>
    {
        public override SalesOpportunityViewModel Map(SalesOpportunity entity)
        {
            var viewModel= new SalesOpportunityViewModel
            {
                Id = entity.Id,
                Title= entity.Title,
                ClosingDate = entity.ClosingDate,
                BusinessValue = entity.BusinessValue,
                Probability = entity.Probability,
                Priority = entity.Priority,
                IsClosed = entity.IsClosed,
                ClosedDate = entity.ClosedDate,
                IsSucceeded = entity.IsSucceeded,
                CustomerId = entity.CustomerId,
                SalesRepresentative = entity.SalesRepresentative,
                StageId = entity.StageId,
                SourceId = entity.SourceId,
                GradeId = entity.GradeId,
                ReasonClosedId = entity.ReasonClosedId,
                CompanyId = entity.CompanyId,
                Active = entity.Active,
                Version = entity.Version
            };
            if (entity.ReasonClosed != null) 
                viewModel.ReasonClosedName = entity.ReasonClosed.Reason;
            if (entity.Grade != null)
                viewModel.GradeName = entity.Grade.GradeName;
            if (entity.Source != null)
                viewModel.SourceName = entity.Source.SourceName;
            if (entity.Stage != null)
                viewModel.StageName = entity.Stage.StageName;
            if (entity.Customer != null)
                viewModel.CustomerName = entity.Customer.DisplayName;
            if (entity.ApplicationUser != null)
                viewModel.SalesRepresentativeName = entity.ApplicationUser.FirstName + " " + entity.ApplicationUser.LastName;
            return viewModel;
        }

        public override SalesOpportunity Map(SalesOpportunityViewModel viewModel)
        {
            return new SalesOpportunity
            {
                Id = viewModel.Id ?? 0,
                Title = viewModel.Title,
                ClosingDate = viewModel.ClosingDate,
                BusinessValue = viewModel.BusinessValue,
                Probability = viewModel.Probability,
                Priority = viewModel.Priority,
                IsClosed = viewModel.IsClosed,
                ClosedDate = viewModel.ClosedDate,
                IsSucceeded = viewModel.IsSucceeded,
                CustomerId = viewModel.CustomerId,
                SalesRepresentative = viewModel.SalesRepresentative,
                StageId = viewModel.StageId,
                SourceId = viewModel.SourceId,
                GradeId = viewModel.GradeId,
                ReasonClosedId = viewModel.ReasonClosedId,
                CompanyId = viewModel.CompanyId,
                Active=viewModel.Active,
                Version=viewModel.Version
            };
        }
    }
}
