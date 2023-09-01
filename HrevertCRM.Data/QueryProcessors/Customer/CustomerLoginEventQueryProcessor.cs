using System;
using System.Linq;
using Hrevert.Common.Constants.Customer;
using Hrevert.Common.Enums;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore;

namespace HrevertCRM.Data.QueryProcessors
{
    public class CustomerLoginEventQueryProcessor : ICustomerLoginEventQueryProcessor
    {
        private readonly IDbContext _dbContext;
        private const int LoginAttemptLimit = 5; //TODO: Get it from configuration file
        private const int LockDuration = 60; //TODO:Get it from configuration file

        public CustomerLoginEventQueryProcessor(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public CustomerLoginResultViewModel CheckLogin(string email, string password)
        {
            //Get the customer with his username/email
            var queriedCustomer = _dbContext.Set<Address>().Include(x => x.Customer).Where(x => x.Email == email).Select(x => x.Customer).FirstOrDefault();
            var address = _dbContext.Set<Address>().FirstOrDefault(x => x.Email == email);
            var customerLoginResult = new CustomerLoginResultViewModel();

            if (queriedCustomer == null)
            {
                customerLoginResult.LoggedIn = false;
                customerLoginResult.LoginMessage = CustomerConstants.CustomerLoginEventQueryProcessorConstants.InvalidLoginAttempt;
                return customerLoginResult;
            }

            SetValuesForCustomerLoginResult(email, queriedCustomer, address, customerLoginResult);

            //Get Existing CustomerLoginEvent or Create new
            var newCustomer = GetExistingCustomerLoginEventOrCreateNew(queriedCustomer);

            if (newCustomer.Locked)
            {
                //Check if the time has exceeded the lock duration of the user
                if (DateTime.Now >= newCustomer.LockedTime)
                {
                    //If the Locked Time exceeds lock duration, unlock the customer and reset the Locked, LockType and LockedTime Properties
                    UnlockUser(password, queriedCustomer, customerLoginResult, newCustomer);
                }
                else
                {
                    //Tell customer that he should wait to get access and inform him the time period after which he can try again to enter his credentials.
                    customerLoginResult.LoggedIn = false;
                    customerLoginResult.LoginMessage = CustomerConstants.CustomerLoginEventQueryProcessorConstants.AccountLocked + GetLockDuration(newCustomer) + " minutes";
                }
            }
            else
            {
                return CheckAuthorization(password, newCustomer, queriedCustomer, customerLoginResult);
            }

            return customerLoginResult;
        }

        private static void SetValuesForCustomerLoginResult(string email, Customer queriedCustomer, Address address, CustomerLoginResultViewModel customerLoginResult)
        {
            customerLoginResult.CustomerId = queriedCustomer.Id;
            customerLoginResult.Email = email;
            customerLoginResult.FirstName = address.FirstName;
            customerLoginResult.MiddleName = address.MiddleName;
            customerLoginResult.LastName = address.LastName;
        }

        private void UnlockUser(string password, Customer queriedCustomer, CustomerLoginResultViewModel customerLoginResult, CustomerLoginEvent newCustomer)
        {
            //If the Locked Time exceeds lock duration, unlock the customer and reset the Locked, LockType and LockedTime Properties
            newCustomer.Locked = false;
            newCustomer.LockType = LockType.None;
            newCustomer.LockedTime = DateTime.MinValue;
            newCustomer.FailedAttemptCount = 0;

            _dbContext.Set<CustomerLoginEvent>().Update(newCustomer);
            _dbContext.SaveChanges();

            CheckAuthorization(password, newCustomer, queriedCustomer, customerLoginResult);
        }

        private CustomerLoginResultViewModel CheckAuthorization(string password, CustomerLoginEvent customer, Customer queriedCustomer, CustomerLoginResultViewModel customerLoginResult)
        {
            if (!queriedCustomer.Password.Equals(password))
            {
                //Check if the invalid attempt is greater than limit or not
                if (++customer.FailedAttemptCount <= LoginAttemptLimit)
                {
                    customerLoginResult.LoggedIn = false;
                    customerLoginResult.LoginMessage = CustomerConstants.CustomerLoginEventQueryProcessorConstants.CredentialsDidntMatch;
                }
                else
                {
                    //If the invalid attempt is greater than specified, set the Locked, LockType and LockedTime Properties of the Customer
                    return LockUser(customer, customerLoginResult);
                }
            }
            else
            {
                //Login Successfull, Reset the FailedAttemptCount, LoginTime
                LoginSuccessfull(customer, customerLoginResult);
            }

            //save or update
            _dbContext.Set<CustomerLoginEvent>().Update(customer);
            _dbContext.SaveChanges();
            return customerLoginResult;
        }

        private CustomerLoginResultViewModel LockUser(CustomerLoginEvent customer, CustomerLoginResultViewModel customerLoginResult)
        {
            //If the invalid attempt is greater than specified, set the Locked, LockType and LockedTime Properties of the Customer
            customer.Locked = true;
            customer.LockType = LockType.MultipleInvalidAttempts;
            customer.LockedTime = DateTime.Now.AddMinutes(LockDuration);

            _dbContext.Set<CustomerLoginEvent>().Update(customer);
            _dbContext.SaveChanges();

            customerLoginResult.LoggedIn = false;
            customerLoginResult.LoginMessage = CustomerConstants.CustomerLoginEventQueryProcessorConstants.AccountLocked + GetLockDuration(customer) + " minutes";
            return customerLoginResult;
        }

        private static void LoginSuccessfull(CustomerLoginEvent customer, CustomerLoginResultViewModel customerLoginResult)
        {
            //Login Successfull, Reset the FailedAttemptCount, LoginTime
            customer.FailedAttemptCount = 0;
            customer.LoginTime = DateTime.Now;
            customerLoginResult.LoggedIn = true;
            customerLoginResult.LoginMessage = CustomerConstants.CustomerLoginEventQueryProcessorConstants.LoginSuccessful;
        }

        private int GetLockDuration(CustomerLoginEvent customerLoginEvent)
        {
            var lockedTime =
                _dbContext.Set<CustomerLoginEvent>()
                    .Where(x => x.Id == customerLoginEvent.Id)
                    .Select(x => x.LockedTime)
                    .FirstOrDefault();
            var lockDuration = lockedTime - DateTime.Now;

            return lockDuration.Minutes == 0 ? 1 : lockDuration.Minutes;
        }

        private CustomerLoginEvent GetExistingCustomerLoginEventOrCreateNew(Customer getCustomer)
        {
            CustomerLoginEvent customerLoginEvent;
            //Get the customer login event of customer, if it's not in the database then create one with the customerId
            if (!_dbContext.Set<CustomerLoginEvent>().Any(cli => cli.CustomerId == getCustomer.Id))
            {
                customerLoginEvent = new CustomerLoginEvent
                {
                    CustomerId = getCustomer.Id,
                    FailedAttemptCount = 0,
                    LockedTime = DateTime.MinValue,
                    LockType = LockType.None,
                    Locked = false,
                    LoginTime = DateTime.Now
                };
                _dbContext.Set<CustomerLoginEvent>().Add(customerLoginEvent);
                _dbContext.SaveChanges();
            }
            else
            {
                customerLoginEvent = _dbContext.Set<CustomerLoginEvent>().Single(cli => cli.CustomerId == getCustomer.Id);
            }

            return customerLoginEvent;
        }
    }
}
