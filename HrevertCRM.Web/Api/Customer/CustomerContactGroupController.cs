using System;
using System.Linq;
using System.Security;
using Hrevert.Common.Constants.Customer;
using Hrevert.Common.Constants.Others;
using HrevertCRM.Data;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.Mapper;
using Microsoft.AspNetCore.Mvc;
using HrevertCRM.Data.QueryProcessors;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;

namespace HrevertCRM.Web.Api
{
    [Route("api/[controller]")]
    public class CustomerContactGroupController : Controller
    {
        private readonly ICustomerContactGroupQueryProcessor _customerContactGroupQueryProcessor;
        private readonly IDbContext _context;
        private readonly IPagedDataRequestFactory _pagedDataRequestFactory;
        private readonly ICustomerInContactGroupQueryProcessor _customerInContactGroupQueryProcessor;
        private readonly ISecurityQueryProcessor _securityQueryProcessor;
        private readonly IUserSettingQueryProcessor _userSettingQueryProcessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<CustomerContactGroupController> _logger;

        public CustomerContactGroupController(ICustomerContactGroupQueryProcessor customerContactGroupQueryProcessor,
            IDbContext context, IPagedDataRequestFactory pagedDataRequestFactory,
            ICustomerInContactGroupQueryProcessor customerInContactGroupQueryProcessor, 
            ILoggerFactory factory,
            ISecurityQueryProcessor securityQueryProcessor,
            IUserSettingQueryProcessor userSettingQueryProcessor,
            UserManager<ApplicationUser> userManager
        )
        {
            _customerContactGroupQueryProcessor = customerContactGroupQueryProcessor;
            _context = context;
            _pagedDataRequestFactory = pagedDataRequestFactory;
            _customerInContactGroupQueryProcessor = customerInContactGroupQueryProcessor;
            _securityQueryProcessor = securityQueryProcessor;
            _userSettingQueryProcessor = userSettingQueryProcessor;
            _userManager = userManager;
            _logger = factory.CreateLogger<CustomerContactGroupController>();
        }
        
        [HttpGet]
        [Route("getcustomercontactgroup/{id}")]
        public ObjectResult Get(int id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewCustomerContactGroups))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            var mapper = new CustomerContactGroupToCustomerContactGroupViewModelMapper();
            return Ok(mapper.Map(_customerContactGroupQueryProcessor.GetCustomerContactGroup(id)));
        }

        [HttpPost]
        [Route("creategroup")]
        public ObjectResult Create([FromBody] CustomerContactGroupViewModel customerContactGroupViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.AddCustomerContactGroup))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });

            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    var currentUserCompanyId = _userManager.FindByEmailAsync(User.Identity.Name).Result.CompanyId;
                    customerContactGroupViewModel.CompanyId = currentUserCompanyId;
                    customerContactGroupViewModel.GroupName = customerContactGroupViewModel.GroupName.Trim();

                    if (_customerContactGroupQueryProcessor.Exists(p => p.GroupName == customerContactGroupViewModel.GroupName && p.CompanyId == customerContactGroupViewModel.CompanyId))
                    {
                        return BadRequest(CustomerConstants.CustomerContactGroupControllerConstants.ContactGroupAlreadyExists);
                    }

                    var mapper = new CustomerContactGroupToCustomerContactGroupViewModelMapper();
                    var customerContactGroup = mapper.Map(customerContactGroupViewModel);
                    var savedContactGroup = _customerContactGroupQueryProcessor.Save(customerContactGroup);

                    customerContactGroupViewModel.Id = savedContactGroup.Id;
                    SaveCustomerInContactGroup(customerContactGroupViewModel);

                    trans.Commit();

                    return Ok(customerContactGroupViewModel);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    _logger.LogCritical((int)SecurityId.AddCustomerContactGroup, ex, ex.Message);
                    ModelState.AddModelError("", ex.Message);
                    return BadRequest(new
                    {
                        errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                    });
                }
            }
        }

        [HttpDelete("{id}")]
        public ObjectResult Delete(int id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteCustomerContactGroup))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            try
            {
                isDeleted = _customerContactGroupQueryProcessor.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int) SecurityId.DeleteCustomerContactGroup, ex, ex.Message);
            }
            return Ok(isDeleted);
        }

        [HttpPut]
        [Route("updategroup")]
        public ObjectResult Put([FromBody] CustomerContactGroupViewModel customerContactGroupViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.UpdateCustomerContactGroup))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });

            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    customerContactGroupViewModel.GroupName = customerContactGroupViewModel.GroupName.Trim();

                    if (_customerContactGroupQueryProcessor.Exists(p => p.GroupName == customerContactGroupViewModel.GroupName && p.Id != customerContactGroupViewModel.Id &&  p.CompanyId == customerContactGroupViewModel.CompanyId))
                    {
                        return BadRequest(CustomerConstants.CustomerContactGroupControllerConstants.ContactGroupAlreadyExists);
                    }

                    var mapper = new CustomerContactGroupToCustomerContactGroupViewModelMapper();
                    var customerContactGroup = mapper.Map(customerContactGroupViewModel);

                        DetermineCustomersToAdd(customerContactGroupViewModel);
                        SaveCustomerInContactGroup(customerContactGroupViewModel);

                    

                    _customerContactGroupQueryProcessor.Update(customerContactGroup);
                    trans.Commit();
                    return Ok(customerContactGroupViewModel);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    _logger.LogCritical((int)SecurityId.UpdateCustomerContactGroup, ex, ex.Message);
                    ModelState.AddModelError("", ex.Message);
                    return BadRequest(new
                    {
                        errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                    });
                }
            }
        }

        private void SaveCustomerInContactGroup(CustomerContactGroupViewModel customerContactGroupViewModel)
        {
            var customersInContactGroup = (from customerId in customerContactGroupViewModel.CustomerIdsInContactGroup
                where customerContactGroupViewModel.Id != null
                select new CustomerInContactGroup
                {
                    ContactGroupId = customerContactGroupViewModel.Id ?? 0, CustomerId = customerId
                }).ToList();

            _customerInContactGroupQueryProcessor.SaveAll(customersInContactGroup);
        }

        private void DetermineCustomersToAdd(CustomerContactGroupViewModel customerContactGroupViewModel)
        {
            if (customerContactGroupViewModel.Id == null) return;
            var existingCustomers =
                _customerInContactGroupQueryProcessor.GetExistingCustomersOfGroup((int)customerContactGroupViewModel.Id);
            foreach (var existingCustomerId in existingCustomers)
            {
                if (!customerContactGroupViewModel.CustomerIdsInContactGroup.Contains(existingCustomerId))
                {
                    _customerInContactGroupQueryProcessor.Delete(existingCustomerId, (int)customerContactGroupViewModel.Id);
                }
                else
                {
                    customerContactGroupViewModel.CustomerIdsInContactGroup.Remove(existingCustomerId);
                }
            }
        }

        [HttpGet]
        [Route("searchactivecustomercontactgroup/{searchText}")]
        public ObjectResult SearchActive(string searchText)
        {
            var emailSettings = _customerContactGroupQueryProcessor.SearchActive(searchText);

            var mapper = new CustomerContactGroupToCustomerContactGroupViewModelMapper();
            var res = emailSettings.Select(fy => mapper.Map(fy)).ToList();
            return Ok(res);
        }

        [HttpGet]
        [Route("searchallcustomercontactgroup/{searchText}")]
        public ObjectResult SearchAll(string searchText)
        {
            var emailSettings = _customerContactGroupQueryProcessor.SearchAll(searchText);

            var mapper = new CustomerContactGroupToCustomerContactGroupViewModelMapper();
            return Ok(emailSettings.Select(fy => mapper.Map(fy)).ToList());
        }

        [HttpGet]
        [Route("activatecustomercontactgroup/{id}")]
        public ObjectResult ActivateCustomer(int id)
        {
            var result = _customerContactGroupQueryProcessor.ActivateCustomerContactGroup(id);
            return Ok(result);
        }

        [HttpGet]
        [Route("CheckIfDeletedCustomerContactWithSameNameExists/{name}")]
        public ObjectResult CheckIfDeletedCustomerContactWithSameNameExists(string name)
        {
            var contactGroup = _customerContactGroupQueryProcessor.CheckIfDeletedCustomerContactWithSameNameExists(name);
            var contactGroupMapper = new CustomerContactGroupToCustomerContactGroupViewModelMapper();
            if (contactGroup != null)
            {
                contactGroupMapper.Map(contactGroup);
            }
            return Ok(contactGroup);
        }

        [HttpGet]
        [Route("GetPageSize")]
        public ObjectResult GetPageSize()
        {
            var pageSize = _userSettingQueryProcessor.GetPageSize(EntityId.CustomerContactGroup);
            return Ok(pageSize);
        }

        [HttpGet]
        [Route("getContactGroups")]
        public ObjectResult GetContactGroups()
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewCustomerContactGroups))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            try
            {
                var requestInfo = _pagedDataRequestFactory.CreateWithSearchStringCompanyActive(Request.GetUri());
                requestInfo.PageSize = _userSettingQueryProcessor.ForPaging(requestInfo, EntityId.CustomerContactGroup);
                var result = _customerContactGroupQueryProcessor.GetContactGroups(requestInfo);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewCustomerContactGroups, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
