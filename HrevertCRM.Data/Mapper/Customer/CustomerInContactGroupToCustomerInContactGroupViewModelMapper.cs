using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper
{
    public class CustomerInContactGroupToCustomerInContactGroupViewModelMapper : MapperBase<CustomerInContactGroup, CustomerInContactGroupViewModel>
    {
        public override CustomerInContactGroup Map(CustomerInContactGroupViewModel viewModel)
        {
            return new CustomerInContactGroup
            {
                ContactGroupId = viewModel.CustomerContactGroupId,
                CustomerId = viewModel.CustomerId
            };
        }

        public override CustomerInContactGroupViewModel Map(CustomerInContactGroup entity)
        {
            return new CustomerInContactGroupViewModel
            {
                CustomerId = entity.CustomerId,
                CustomerContactGroupId = entity.ContactGroupId
            };
        }
    }
}
