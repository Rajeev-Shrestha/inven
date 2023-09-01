using System;
using System.Linq;
using System.Net.Http;
using System.Security;
using Hrevert.Common.Constants.Customer;
using Hrevert.Common.Constants.Others;
using HrevertCRM.Data;
using HrevertCRM.Data.Mapper;
using HrevertCRM.Data.QueryProcessors;
using HrevertCRM.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using HrevertCRM.Entities;

namespace HrevertCRM.Web.Api
{
    [Route("api/[controller]")]
    public class CustomerLevelController : Controller
    {
        private readonly ICustomerLevelQueryProcessor _customerLevelQueryProcessor;
        private readonly ISecurityQueryProcessor _securityQueryProcessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<CustomerLevelController> _logger;

        public CustomerLevelController(ICustomerLevelQueryProcessor customerLevelQueryProcessor,
            IDbContext context,
            ILoggerFactory factory,ISecurityQueryProcessor securityQueryProcessor,
             UserManager<ApplicationUser> userManager)
        {
            _customerLevelQueryProcessor = customerLevelQueryProcessor;
            _securityQueryProcessor = securityQueryProcessor;
            _userManager = userManager;
            _logger = factory.CreateLogger<CustomerLevelController>();
        }

        [HttpGet]
        [Route("getallcustomerlevels")]
        public ObjectResult GetAll(HttpRequestMessage requestMessage)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewCustomerLevels))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            try
            {
                var mapper = new CustomerLevelToCustomerLevelViewModelMapper();
                return
                    Ok(
                        _customerLevelQueryProcessor.GetActiveCustomerLevels()
                            .Select(customerLevel => mapper.Map(customerLevel)));
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int) SecurityId.ViewCustomerLevels, ex, ex.Message);
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        [Route("getcustomerlevelbyid/{id}")]
        public ObjectResult Get(int id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewCustomerLevels))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            return Ok(_customerLevelQueryProcessor.GetCustomerLevel(id));
        }

        [HttpPost]
        [Route("createcustomerlevel")]
        public ObjectResult Create([FromBody] CustomerLevelViewModel customerLevelViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.AddCustomerLevel))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });
            var currentUserCompanyId = _userManager.FindByEmailAsync(User.Identity.Name).Result.CompanyId;
            customerLevelViewModel.CompanyId = currentUserCompanyId;
            customerLevelViewModel.Name = customerLevelViewModel.Name.Trim();
            var model = customerLevelViewModel;
            if (_customerLevelQueryProcessor.Exists(p => p.Name == model.Name && p.CompanyId == model.CompanyId))
            {
                return BadRequest(CustomerConstants.CustomerLevelControllerConstants.CustomerLevelAlreadyExists);
            }

            var mapper = new CustomerLevelToCustomerLevelViewModelMapper();
            var newCustomerLevel = mapper.Map(customerLevelViewModel);
            try
            {
                var savedCustomerLevel = _customerLevelQueryProcessor.Save(newCustomerLevel);
                customerLevelViewModel = mapper.Map(savedCustomerLevel);
                return Ok(customerLevelViewModel);                
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int) SecurityId.AddCustomerLevel, ex, ex.Message);
                ModelState.AddModelError("", ex.Message);
                return BadRequest(new
                {
                    errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                });
            }
        }

        [HttpDelete("{id}")]
        public ObjectResult Delete(int id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteCustomerLevel))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            try
            {
                isDeleted = _customerLevelQueryProcessor.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeleteCustomerLevel, ex, ex.Message);                
            }
            return Ok(isDeleted);
        }

        [HttpPut]
        [Route("updatecustomerlevel")]
        public ObjectResult Put([FromBody] CustomerLevelViewModel customerLevelViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.UpdateCustomerLevel))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });

            var model = customerLevelViewModel;
            if (_customerLevelQueryProcessor.Exists(p => p.Name == model.Name && p.Id != model.Id && p.CompanyId == model.CompanyId))
            {
                return BadRequest(CustomerConstants.CustomerLevelControllerConstants.CustomerLevelAlreadyExists);
            }

            var mapper = new CustomerLevelToCustomerLevelViewModelMapper();
            var newCustomerLevel = mapper.Map(customerLevelViewModel);
            try
            {
                var savedCustomerLevel = _customerLevelQueryProcessor.Update(newCustomerLevel);
                customerLevelViewModel = mapper.Map(savedCustomerLevel); 
                return Ok(customerLevelViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.UpdateCustomerLevel, ex, ex.Message);
                ModelState.AddModelError("", ex.Message);
                return BadRequest(new
                {
                    errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                });
            }
        }
    }
}
