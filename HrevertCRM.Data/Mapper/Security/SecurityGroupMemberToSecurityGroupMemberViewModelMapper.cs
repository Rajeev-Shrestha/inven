using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper
{
    public class SecurityGroupMemberToSecurityGroupMemberViewModelMapper : MapperBase<SecurityGroupMember, SecurityGroupMemberViewModel>
    {
        public override SecurityGroupMember Map(SecurityGroupMemberViewModel securityGroupMemberViewModel)
        {
            return new SecurityGroupMember
            {
                Id = securityGroupMemberViewModel.Id ?? 0,
                MemberId = securityGroupMemberViewModel.MemberId,
                SecurityGroupId = securityGroupMemberViewModel.SecurityGroupId,
                CompanyId = securityGroupMemberViewModel.CompanyId,
                Active = securityGroupMemberViewModel.Active,
                Version = securityGroupMemberViewModel.Version,
                WebActive = securityGroupMemberViewModel.WebActive
            };
        }

        public override SecurityGroupMemberViewModel Map(SecurityGroupMember entity)
        {
            return new SecurityGroupMemberViewModel
            {
                Id = entity.Id,
                MemberId = entity.MemberId,
                SecurityGroupId = entity.SecurityGroupId,
                CompanyId = entity.CompanyId,
                Active = entity.Active,
                Version = entity.Version,
                WebActive = entity.WebActive
            };
        }
    }
}
