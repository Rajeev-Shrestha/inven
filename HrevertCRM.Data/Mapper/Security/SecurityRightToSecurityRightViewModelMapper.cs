using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper
{
    public class SecurityRightToSecurityRightViewModelMapper : MapperBase<SecurityRight, SecurityRightViewModel>
    {
        public override SecurityRight Map(SecurityRightViewModel securityRightViewModel)
        {
            return new SecurityRight
            {
                Id = securityRightViewModel.Id ?? 0,
                SecurityGroupId = securityRightViewModel.SecurityGroupId,
                UserId = securityRightViewModel.UserId,
                Allowed = securityRightViewModel.Allowed,
                SecurityId = securityRightViewModel.SecurityId,
                CompanyId = securityRightViewModel.CompanyId,
                Active = securityRightViewModel.Active,
                Version = securityRightViewModel.Version,
                WebActive = securityRightViewModel.WebActive
            };
        }

        public override SecurityRightViewModel Map(SecurityRight entity)
        {
            var securityRightVm = new SecurityRightViewModel
            {
                Id = entity.Id,
                SecurityGroupId = entity.SecurityGroupId,
                UserId = entity.UserId,
                Allowed = entity.Allowed,
                SecurityId = entity.SecurityId,
                CompanyId = entity.CompanyId,
                Active = entity.Active,
                Version = entity.Version,
                WebActive = entity.WebActive
            };
            if (entity.Security == null) return securityRightVm;
            securityRightVm.SecurityName = entity.Security.SecurityDescription;
            return securityRightVm;
        }
    }
}
