using HrevertCRM.Data.ViewModels;

namespace HrevertCRM.Data.Mapper.Security
{
    public class SecurityToSecurityRightViewModelMapper : MapperBase<Entities.Security, SecurityRightViewModel>
    {
        public override Entities.Security Map(SecurityRightViewModel securityRightViewModel)
        {
            return new Entities.Security
            {
                Id = securityRightViewModel.Id ?? 0,
                CompanyId = securityRightViewModel.CompanyId,
                Active = securityRightViewModel.Active,
                Version = securityRightViewModel.Version,
                WebActive = securityRightViewModel.WebActive
            };
        }

        public override SecurityRightViewModel Map(Entities.Security entity)
        {
            return new SecurityRightViewModel
            {
                Id = entity.Id,
                CompanyId = entity.CompanyId,
                Active = entity.Active,
                Version = entity.Version,
                WebActive = entity.WebActive,
                SecurityName = entity.SecurityDescription,
                SecurityId = entity.Id
            };
        }
    }
}
