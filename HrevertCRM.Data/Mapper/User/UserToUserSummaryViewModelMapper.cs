using HrevertCRM.Data.ViewModels.User;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper
{
    public class UserToUserSummaryViewModelMapper : MapperBase<ApplicationUser, UserSummaryViewModel>
    {
        public override ApplicationUser Map(UserSummaryViewModel viewModel)
        {
            return new ApplicationUser
            {
                Id = viewModel.Id,
                FirstName = viewModel.FirstName,
                MiddleName = viewModel.MiddleName,
                LastName = viewModel.LastName,
                Active = viewModel.Active
            };
        }

        public override UserSummaryViewModel Map(ApplicationUser entity)
        {
            return new UserSummaryViewModel
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                MiddleName = entity.MiddleName,
                LastName = entity.LastName,
                Active = entity.Active
            };
        }
    }
}
