using HrevertCRM.Data.ViewModels;

namespace HrevertCRM.Data.Mapper
{
    public class SecurityToSecurityViewModelMapper : MapperBase<Entities.Security, SecurityViewModel>
    {
        public override Entities.Security Map(SecurityViewModel securityViewModel)
        {
            return new Entities.Security
            {
                Id = securityViewModel.Id ?? 0,
                SecurityCode = securityViewModel.SecurityCode,
                SecurityDescription = securityViewModel.SecurityDescription,
                CompanyId = securityViewModel.CompanyId,
                Active = securityViewModel.Active,
                Version = securityViewModel.Version,
                WebActive = securityViewModel.WebActive
            };
        }

        public override SecurityViewModel Map(Entities.Security entity)
        {
            return new SecurityViewModel
            {
                Id = entity.Id,
                SecurityCode = entity.SecurityCode,
                SecurityDescription = entity.SecurityDescription,
                CompanyId = entity.CompanyId,
                Active = entity.Active,
                Version = entity.Version,
                WebActive = entity.WebActive
            };
        }
    }
}
