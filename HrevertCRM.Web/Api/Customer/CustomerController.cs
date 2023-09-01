using System;
using System.Collections.Generic;
using System.Linq;
using Hrevert.Common.Constants.Customer;
using Hrevert.Common.Constants.Others;
using Hrevert.Common.Enums;
using HrevertCRM.Data;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.Mapper;
using HrevertCRM.Data.Mapper.Enumerations;
using HrevertCRM.Data.QueryProcessors;
using HrevertCRM.Data.QueryProcessors.Enumerations;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using NUglify.Helpers;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;

namespace HrevertCRM.Web.Api
{
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {
        private readonly ICustomerQueryProcessor _customerQueryProcessor;
        private readonly IPagedDataRequestFactory _pagedDataRequestFactory;
        private readonly IUserSettingQueryProcessor _userSettingQueryProcessor;
        private readonly IAddressQueryProcessor _addressQueryProcessor;
        private readonly IDbContext _dbContext;
        private readonly ICompanyWebSettingQueryProcessor _companyWebSettingQueryProcessor;
        private readonly IUserQueryProcessor _userQueryProcessor;
        private readonly ICustomerLoginEventQueryProcessor _customerLoginEventQueryProcessor;
        private readonly ITitleTypesQueryProcessor _titleTypesQueryProcessor;
        private readonly ISuffixTypesQueryProcessor _suffixTypesQueryProcessor;
        private readonly ISecurityQueryProcessor _securityQueryProcessor;
        private readonly ICountryQueryProcessor _countryQueryProcessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ICustomerQueryProcessor customerQueryProcessor,
            IDbContext context, IPagedDataRequestFactory pagedDataRequestFactory,
            ILoggerFactory factory, IUserSettingQueryProcessor userSettingQueryProcessor,
            IAddressQueryProcessor addressQueryProcessor, IDbContext dbContext,
            ICustomerInContactGroupQueryProcessor customerInContactGroupQueryProcessor,
            ICompanyWebSettingQueryProcessor companyWebSettingQueryProcessor,
            IUserQueryProcessor userQueryProcessor,
            ICustomerLoginEventQueryProcessor customerLoginEventQueryProcessor,
            ITitleTypesQueryProcessor titleTypesQueryProcessor,
            ISuffixTypesQueryProcessor suffixTypesQueryProcessor,
            ISecurityQueryProcessor securityQueryProcessor,
            ICountryQueryProcessor countryQueryProcessor,
             UserManager<ApplicationUser> userManager)
        {
            _customerQueryProcessor = customerQueryProcessor;
            _pagedDataRequestFactory = pagedDataRequestFactory;
            _userSettingQueryProcessor = userSettingQueryProcessor;
            _addressQueryProcessor = addressQueryProcessor;
            _dbContext = dbContext;
            _companyWebSettingQueryProcessor = companyWebSettingQueryProcessor;
            _userQueryProcessor = userQueryProcessor;
            _customerLoginEventQueryProcessor = customerLoginEventQueryProcessor;
            _titleTypesQueryProcessor = titleTypesQueryProcessor;
            _suffixTypesQueryProcessor = suffixTypesQueryProcessor;
            _securityQueryProcessor = securityQueryProcessor;
            _userManager = userManager;
            _countryQueryProcessor = countryQueryProcessor;
            _logger = factory.CreateLogger<CustomerController>();
        }

        [HttpGet]
        [Route("getcustomerbyid/{id}")]
        public ObjectResult Get(int id) //Get Includes Full customer data
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewCustomers))
               return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            var result = _customerQueryProcessor.GetCustomerViewModel(id);
            return Ok(result);
        }

        [HttpGet]
        [Route("activatecustomer/{id}")]
        public ObjectResult ActivateCustomer(int id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewCustomers))
               return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var mapper = new CustomerToCustomerViewModelMapper();
            return Ok(mapper.Map(_customerQueryProcessor.ActivateCustomer(id)));
        }

        [HttpPost]
        [Route("createcustomer")]
        public ObjectResult Create([FromBody] CustomerViewModel customerViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.AddCustomer))
               return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });
            using (var trans = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    if (customerViewModel.CustomerCode == null)
                        customerViewModel.CustomerCode = _customerQueryProcessor.GenerateCustomerCode();
                    var currentUserCompanyId = _userManager.FindByEmailAsync(User.Identity.Name).Result.CompanyId;
                    customerViewModel.CompanyId = currentUserCompanyId;
                    customerViewModel.CustomerCode = customerViewModel.CustomerCode?.Trim();
                    customerViewModel.DisplayName = customerViewModel.DisplayName?.Trim();
                    customerViewModel.TaxRegNo = customerViewModel.TaxRegNo?.Trim();
                    var addresses = customerViewModel.Addresses;
                    var billingAddress = customerViewModel.BillingAddress;

                    var model = customerViewModel;
                    if (_customerQueryProcessor.Exists(p => p.CustomerCode == model.CustomerCode && p.CompanyId == model.CompanyId))
                    {
                        return BadRequest(CustomerConstants.CustomerControllerConstants.CustomerAlreadyExists);
                    }

                    if (_addressQueryProcessor.Exists(p => p.Email == model.BillingAddress.Email && p.CompanyId == model.CompanyId))
                    {
                        return BadRequest(CustomerConstants.CustomerControllerConstants.CustomerAlreadyExistsWithThisEmail);
                    }
                    if (_addressQueryProcessor.Exists(p => p.Email == model.Addresses.FirstOrDefault(x => x.AddressType == AddressType.Shipping).Email && p.CompanyId == model.CompanyId))
                    {
                        return BadRequest(CustomerConstants.CustomerControllerConstants.CustomerAlreadyExistsWithThisEmail);
                    }
                    if (_addressQueryProcessor.Exists(p => p.Email == model.Addresses.FirstOrDefault(x => x.AddressType == AddressType.Contact).Email && p.CompanyId == model.CompanyId))
                    {
                        return BadRequest(CustomerConstants.CustomerControllerConstants.CustomerAlreadyExistsWithThisEmail);
                    }

                    var mapper = new CustomerToCustomerViewModelMapper();
                    var addressMapper = new AddressToAddressViewModelMapper();
                    var newCustomer = mapper.Map(customerViewModel);
                    newCustomer.Addresses = null;
                    newCustomer.BillingAddress = null;
                    var savedCustomer = _customerQueryProcessor.Save(newCustomer);
                    billingAddress.CustomerId = savedCustomer.Id;
                    billingAddress.AddressType = AddressType.Billing;
                    billingAddress.IsDefault = true;
                    foreach (var address in addresses)
                    {
                        address.CustomerId = savedCustomer.Id;
                    }
                    _addressQueryProcessor.Save(addressMapper.Map(billingAddress));
                    _addressQueryProcessor.SaveAll(addresses.ToList());
                    trans.Commit();
                    customerViewModel = mapper.Map(savedCustomer);
                    return Ok(customerViewModel);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    _logger.LogCritical((int) SecurityId.AddCustomer, ex, ex.Message);
                    ModelState.AddModelError("", ex.Message);
                    return BadRequest(new
                    {
                        errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                    });
                }
            }
        }

        [HttpPost]
        [Route("createcustomerforstorefront")]
        public ObjectResult CreateCustomer([FromBody] CustomerViewModel customerViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.AddCustomer))
               return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                });
            using (var trans = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    customerViewModel.CustomerCode = _customerQueryProcessor.GenerateCustomerCode();
                    var currentUserCompanyId = _userManager.FindByEmailAsync(User.Identity.Name).Result.CompanyId;
                    customerViewModel.CompanyId = currentUserCompanyId;
                    customerViewModel.CustomerCode = customerViewModel.CustomerCode?.Trim();
                    customerViewModel.DisplayName = customerViewModel.DisplayName?.Trim();
                    customerViewModel.TaxRegNo = customerViewModel.TaxRegNo?.Trim();

                    var model = customerViewModel;
                    if (_addressQueryProcessor.Exists(p => p.Email == model.BillingAddress.Email))
                    {
                        return BadRequest(CustomerConstants.CustomerControllerConstants.CustomerAlreadyExistsWithThisEmail);
                    }

                    var mapper = new CustomerToCustomerViewModelMapper();
                    var addressMapper = new AddressToAddressViewModelMapper();
                    var mappedCustomer = mapper.Map(customerViewModel);
                    mappedCustomer.BillingAddress = null;
                    mappedCustomer.Addresses = null;
                    var savedCustomer = _customerQueryProcessor.Save(mappedCustomer);
                    var billingAddressViewModel = customerViewModel.BillingAddress;
                    var address = customerViewModel.Addresses;
                    address.ForEach(x => x.CompanyId = savedCustomer.CompanyId);
                    address.ForEach(x => x.CustomerId = savedCustomer.Id);
                    address.ForEach(x => x.AddressType = AddressType.Shipping);
                    address.ForEach(x => x.IsDefault = true);
                    billingAddressViewModel.CustomerId = savedCustomer.Id;
                    billingAddressViewModel.CompanyId = savedCustomer.CompanyId;
                    billingAddressViewModel.IsDefault = true;
                    _addressQueryProcessor.SaveAll(address.ToList());
                    _addressQueryProcessor.Save(addressMapper.Map(billingAddressViewModel));
                    trans.Commit();
                    customerViewModel = mapper.Map(savedCustomer);

                    return Ok(customerViewModel);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    _logger.LogCritical((int)SecurityId.AddCustomer, ex, ex.Message);
                    ModelState.AddModelError("", ex.Message);
                    return BadRequest(new
                    {
                        errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(x => x.ErrorMessage)
                    });
                }
            }

        }


        [HttpDelete("{id}")]
        public ObjectResult Delete(int id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteCustomer))
               return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            try
            {
                isDeleted = _customerQueryProcessor.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int) SecurityId.DeleteCustomer, ex, ex.Message);
            }
            return Ok(isDeleted);
        }

        [HttpPut]
        [Route("updatecustomerdetails")]
        public ObjectResult Put([FromBody] EditCustomerViewModel customerViewModel)
        {   
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.UpdateCustomer))
               return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                });

            using (var trans = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    customerViewModel.CustomerCode = customerViewModel.CustomerCode?.Trim();
                    customerViewModel.DisplayName = customerViewModel.DisplayName?.Trim();
                    customerViewModel.TaxRegNo = customerViewModel.TaxRegNo?.Trim();

                    var model = customerViewModel;
                    if (_customerQueryProcessor.Exists(p => (p.CustomerCode == model.CustomerCode) && (p.Id != model.Id) && (p.CompanyId == model.CompanyId)))
                        return BadRequest(CustomerConstants.CustomerControllerConstants.CustomerAlreadyExists);
                    if (_addressQueryProcessor.Exists(p => (p.Email == model.BillingAddress.Email) && (p.CustomerId != model.Id) && (p.CompanyId == model.CompanyId)))
                        return BadRequest(CustomerConstants.CustomerControllerConstants.CustomerAlreadyExistsWithThisEmail);
                    if (_addressQueryProcessor.Exists(p => (p.Email == model.Addresses.FirstOrDefault(x => x.AddressType == AddressType.Shipping).Email)
                        && (p.CustomerId != model.Id) && (p.CompanyId == model.CompanyId)))
                        return BadRequest(CustomerConstants.CustomerControllerConstants.CustomerAlreadyExistsWithThisEmail);
                    if (_addressQueryProcessor.Exists(p => (p.Email == model.Addresses.FirstOrDefault(x => x.AddressType == AddressType.Contact).Email)
                        && (p.CustomerId != model.Id) && (p.CompanyId == model.CompanyId)))
                        return BadRequest(CustomerConstants.CustomerControllerConstants.CustomerAlreadyExistsWithThisEmail);

                    var editMapper = new CustomerToEditCustomerViewModelMapper();
                    var newCustomer = editMapper.Map(customerViewModel);
                    var updatedCustomer = _customerQueryProcessor.Update(newCustomer);
                    trans.Commit();
                    customerViewModel = editMapper.Map(updatedCustomer);
                    return Ok(customerViewModel);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    _logger.LogCritical((int)SecurityId.UpdateCustomer, ex, ex.Message);
                    ModelState.AddModelError("", ex.Message);
                    return BadRequest(new
                    {
                        errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(x => x.ErrorMessage)
                    });
                }
            }
        }

        [HttpPost]
        [Route("savecustomeraddress")]
        public AddressViewModel SaveBillingAddress([FromBody] AddressViewModel addressViewModel)
        {
            var mapper = new AddressToAddressViewModelMapper();
            var address = _customerQueryProcessor.SaveBillingAddress(mapper.Map(addressViewModel));
            return mapper.Map(address);
        }

        [HttpPost]
        [Route("checkcode/{customerCode}")]
        public bool CheckIfCustomerCodeExistsOrNot(string customerCode)
        {
            var result = _customerQueryProcessor.CheckIfCustomerCodeExistsOrNot(customerCode);
            return result;
        }

        [HttpPost]
        [Route("login/{email}/{password}")]
        public CustomerLoginResultViewModel CustomerLogin(string email, string password)
        {
            return _customerLoginEventQueryProcessor.CheckLogin(email, password);
        }

        [HttpPost]
        [Route("customerRegister")]
        public ObjectResult CustomerRegister([FromBody] CustomerViewModel customerViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                });

            var email = User.Identity.Name;
            var companyId = _userQueryProcessor.Get(email).CompanyId;
            var isAllowGuest = _companyWebSettingQueryProcessor.Get(companyId).AllowGuest;

            return isAllowGuest ? Create(customerViewModel) : Ok(CustomerConstants.CustomerControllerConstants.AllowGuestDisabled);
        }

        [HttpGet]
        [Route("gettitles")]
        public ObjectResult GetTitles()
        {
            var mapper = new TitleTypeToTitleTypeViewModelMapper();
            return Ok(_titleTypesQueryProcessor.GetActiveTitleTypes().Select(x => mapper.Map(x)));
        }

        [HttpGet]
        [Route("getsuffixes")]
        public ObjectResult GetSuffixes()
        {
            var mapper = new SuffixTypeToSuffixTypeViewModelMapper();
            return Ok(_suffixTypesQueryProcessor.GetActiveSuffixTypes().Select(x => mapper.Map(x)));
        }

        [HttpGet]
        [Route("getcountries")]
        public ObjectResult GetCountries()
        {
            var mapper = new CountryToCountryViewModelMapper();
            return Ok(_countryQueryProcessor.GetActiveCountries().Select(x => mapper.Map(x)));
        }

        [HttpGet]
        [Route("getorderssummary/{customerId}")]
        public ObjectResult GetOrdersSummary(int customerId)
        {
            var result = _customerQueryProcessor.GetOrdersSummary(customerId);
            return Ok(result);
        }

        [HttpGet]
        [Route("activecustomerwithoutpagaing")]
        public ObjectResult GetActiveCustomersWithoutPaging()
        {
            var mapper = new CustomerToCustomerViewModelMapper();
            return Ok(_customerQueryProcessor.GetActiveCustomersWithoutPaging().Select(x => mapper.Map(x)));
        }

        [HttpGet]
        [Route("deletedcustomerswithoutpagaing")]
        public ObjectResult GetDeletedCustomersWithoutPaging()
        {
            var mapper = new CustomerToCustomerViewModelMapper();
            return Ok(_customerQueryProcessor.GetDeletedCustomersWithoutPaging().Select(x => mapper.Map(x)));
        }

        [HttpPost]
        [Route("InsertCustomersViaCSV/{customers}/{updateExisting}")]
        public void ImportCustomersViaCsv(List<Customer> customers, bool updateExisting)
        {
            if(customers != null && customers.Count > 0)
                _customerQueryProcessor.ImportCustomers(customers, updateExisting);
        }

        [HttpGet]
        [Route("getCustomerAllAddresses/{customerId}")]
        public ObjectResult GetCoustomerAllAddresses(int customerId)
        {
            var result = _customerQueryProcessor.GetCustomerAllAddresses(customerId);

            var mapper = new AddressToAddressViewModelMapper();

            return Ok(result.Select(x=>mapper.Map(x)).ToList());
        }


        //[HttpGet]
        //[Route("CheckIfDeletedCustomerWithSameNameExists/{name}")]
        //public ObjectResult CheckIfDeletedProductWithSameNameExists(string name)
        //{
        //    var customer = _customerQueryProcessor.CheckIfDeletedCustomerWithSameNameExists(name);
        //    var customerMapper = new CustomerToCustomerViewModelMapper();
        //    return Ok(customerMapper.Map(customer));
        //}

        [HttpGet]
        [Route("CheckIfDeletedCustomerWithSameCodeExists/{code}")]
        public ObjectResult CheckIfDeletedCustomerWithSameCodeExists(string code)
        {
            var customer = _customerQueryProcessor.CheckIfDeletedCustomerWithSameCodeExists(code);
            var customerMapper = new CustomerToCustomerViewModelMapper();
            if (customer != null)
            {
                customerMapper.Map(customer);
            }
            return Ok(customer);
        }

        [HttpPost]
        [Route("CheckEmail/{email}")]
        public ObjectResult CheckEmail(string email)
        {
            var result = _addressQueryProcessor.Exists(p => p.Email == email);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetPageSize")]
        public ObjectResult GetPageSize()
        {
            var pageSize = _userSettingQueryProcessor.GetPageSize(EntityId.Customer);
            return Ok(pageSize);
        }

        [HttpGet]
        [Route("searchCustomers")]
        public ObjectResult SearchCustomers()
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewCustomers))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            try
            {
                var requestInfo = _pagedDataRequestFactory.CreateWithSearchStringCompanyActive(Request.GetUri());
                requestInfo.PageSize = _userSettingQueryProcessor.ForPaging(requestInfo, EntityId.Customer);
                var result = _customerQueryProcessor.SearchCustomers(requestInfo);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewCustomers, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("bulkDelete")]
        public ObjectResult BulkDelete([FromBody] List<int?> customersId)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteCustomer))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            if (customersId == null || customersId.Count <= 0) return Ok(false);
            try
            {
                isDeleted = _customerQueryProcessor.DeleteRange(customersId);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeleteCustomer, ex, ex.Message);
            }
            return Ok(isDeleted);
        }
    }
} 