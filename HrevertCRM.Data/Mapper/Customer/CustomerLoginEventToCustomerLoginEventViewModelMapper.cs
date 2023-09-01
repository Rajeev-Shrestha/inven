using System;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper
{
    public class CustomerLoginEventToCustomerLoginEventViewModelMapper: MapperBase<CustomerLoginEvent, CustomerLoginEventViewModel>
    {
        public override CustomerLoginEvent Map(CustomerLoginEventViewModel viewModel)
        {
            return new CustomerLoginEvent()
            {
                Id = viewModel.Id ?? 0,
                CustomerId = viewModel.CustomerId,
                LockedTime = viewModel.LockedTime,
                FailedAttemptCount = viewModel.FailedAttemptCount,
                LockType = viewModel.LockType,
                Locked = viewModel.Locked
                
            };
        }

        public override CustomerLoginEventViewModel Map(CustomerLoginEvent entity)
        {
            return new CustomerLoginEventViewModel()
            {
                Id = entity.Id,
                CustomerId = entity.CustomerId,
                LockedTime = entity.LockedTime,
                FailedAttemptCount = entity.FailedAttemptCount,
                LockType = entity.LockType,
                Locked = entity.Locked,
                LoginTime = entity.LoginTime
            };
        }
    }
}
