using System.Linq;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper
{
    public class CustomerContactGroupToCustomerContactGroupViewModelMapper : MapperBase<CustomerContactGroup, CustomerContactGroupViewModel>
    {
        public override CustomerContactGroup Map(CustomerContactGroupViewModel viewModel)
        {
            var mapper = new CustomerInContactGroupToCustomerInContactGroupViewModelMapper();
            var vm =  new CustomerContactGroup
            {
                Id = viewModel.Id ?? 0,
                GroupName = viewModel.GroupName,
                Description = viewModel.Description,
                CompanyId = viewModel.CompanyId,
                Active = viewModel.Active,
                Version = viewModel.Version,
                WebActive = viewModel.WebActive,
                CustomerIdsInContactGroup = viewModel.CustomerIdsInContactGroup
            };

            if (viewModel.CustomerAndContactGroupList == null || viewModel.CustomerAndContactGroupList.Count < 0) return vm;
            if (viewModel.CustomerAndContactGroupList != null)
            {
                vm.CustomerAndContactGroupList = viewModel.CustomerAndContactGroupList.ToList().Select(x => mapper.Map(x)).ToList();
                vm.CustomerIdsInContactGroup = viewModel.CustomerAndContactGroupList.Select(x => x.CustomerId).ToList();
            }
            return vm;
        }

        public override CustomerContactGroupViewModel Map(CustomerContactGroup entity)
        {
            var mapper = new CustomerInContactGroupToCustomerInContactGroupViewModelMapper();
         var vm = new CustomerContactGroupViewModel
            {
                Id = entity.Id,
                GroupName = entity.GroupName,
                Description = entity.Description,
                CompanyId = entity.CompanyId,
                Active = entity.Active,
                Version = entity.Version,
                WebActive = entity.WebActive,
                CustomerIdsInContactGroup = entity.CustomerIdsInContactGroup
            };

            if (entity.CustomerAndContactGroupList == null || entity.CustomerAndContactGroupList.Count < 0) return vm;
            vm.CustomerAndContactGroupList = entity.CustomerAndContactGroupList.ToList().Select(x => mapper.Map(x)).ToList();
            vm.CustomerIdsInContactGroup = entity.CustomerAndContactGroupList.Select(x => x.CustomerId).ToList();
            return vm;
        }
    }
}
