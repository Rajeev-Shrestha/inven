using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper
{
    public class SecurityGroupToSecurityGroupViewModelMapper : MapperBase<SecurityGroup, SecurityGroupViewModel>
    {
        public override SecurityGroup Map(SecurityGroupViewModel securityGroupViewModel)
        {
            return new SecurityGroup
            {
                Id = securityGroupViewModel.Id ?? 0,
                GroupName = securityGroupViewModel.GroupName,
                GroupDescription = securityGroupViewModel.GroupDescription,
                CompanyId = securityGroupViewModel.CompanyId,
                Active = securityGroupViewModel.Active,
                Version = securityGroupViewModel.Version,
                WebActive = securityGroupViewModel.WebActive
            };
        }

        public override SecurityGroupViewModel Map(SecurityGroup entity)
        {
            return new SecurityGroupViewModel
            {
                Id = entity.Id,
                GroupName = entity.GroupName,
                GroupDescription = entity.GroupDescription,
                CompanyId = entity.CompanyId,
                Active = entity.Active,
                Version = entity.Version,
                WebActive = entity.WebActive
            };
        }
    }
}
