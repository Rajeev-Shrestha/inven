using System.Linq;
using HrevertCRM.Entities;
using HrevertCRM.Web.ViewModels;

namespace HrevertCRM.Data.Mapper
{
    public class UserToUserViewModelMapper : MapperBase<ApplicationUser, UserViewModel>
    {
        public override ApplicationUser Map(UserViewModel viewModel)
        {
            return new ApplicationUser
            {
                Id = viewModel.Id,
                Email = viewModel.Email,
                UserName = viewModel.Email,
                FirstName = viewModel.FirstName,
                MiddleName = viewModel.MiddleName,
                LastName = viewModel.LastName,
                Gender = viewModel.Gender,
                Phone = viewModel.Phone,
                Address = viewModel.Address,
                UserType = viewModel.UserType,
                CompanyId = viewModel.CompanyId,
                Version = viewModel.Version,
                WebActive = viewModel.WebActive,
                Active = viewModel.Active
            };
        }

        public override UserViewModel Map(ApplicationUser entity)
        {
            var userVm = new UserViewModel
            {
                Id = entity.Id,
                Email = entity.Email,
                UserName = entity.Email,
                FirstName = entity.FirstName,
                MiddleName = entity.MiddleName,
                LastName = entity.LastName,
                Gender = entity.Gender,
                Phone = entity.Phone,
                Address = entity.Address,
                UserType = entity.UserType,
                CompanyId = entity.CompanyId,
                Version = entity.Version,
                WebActive = entity.WebActive,
                Active = entity.Active
            };
            if (entity.SecurityGroupMemberUsers != null && entity.SecurityGroupMemberUsers.Count > 0)
            {
                userVm.SecurityGroupIdList = entity.SecurityGroupMemberUsers.Select(x => x.SecurityGroupId).ToList();
            }
            return userVm;
        }
    }
}
