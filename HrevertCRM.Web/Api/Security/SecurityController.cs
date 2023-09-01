using System;
using System.Linq;
using System.Security;
using Hrevert.Common.Constants.Others;
using Hrevert.Common.Constants.Security;
using HrevertCRM.Data;
using HrevertCRM.Data.Common;
using Microsoft.AspNetCore.Mvc;
using HrevertCRM.Data.QueryProcessors;
using HrevertCRM.Data.Mapper;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;

namespace HrevertCRM.Web.Api
{
    [Route("api/[controller]")]
    public class SecurityController : Controller
    {
        private readonly ISecurityQueryProcessor _securityQueryProcessor;
        private readonly IPagedDataRequestFactory _pagedDataRequestFactory;
        private readonly IUserSettingQueryProcessor _userSettingQueryProcessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<SecurityController> _logger;

        public SecurityController(ISecurityQueryProcessor securityQueryProcessor,
            IDbContext context, IPagedDataRequestFactory pagedDataRequestFactory,
            ILoggerFactory factory, IUserSettingQueryProcessor userSettingQueryProcessor,
                UserManager<ApplicationUser> userManager)
        {
            _securityQueryProcessor = securityQueryProcessor;
            _pagedDataRequestFactory = pagedDataRequestFactory;
            _userSettingQueryProcessor = userSettingQueryProcessor;
            _userManager = userManager;
            _logger = factory.CreateLogger<SecurityController>();
        }

        [HttpGet]
        [Route("getsecurity/{id}")]
        public ObjectResult Get(int id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewSecurities))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            return Ok(_securityQueryProcessor.GetSecurity(id));
        }

       
        [HttpPost]
        [Route("createsecurity")]
        public ObjectResult Create([FromBody] SecurityViewModel securityViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.AddSecurity))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });

            var currentUserCompanyId = _userManager.FindByEmailAsync(User.Identity.Name).Result.CompanyId;
            securityViewModel.CompanyId = currentUserCompanyId;
            securityViewModel.SecurityDescription = securityViewModel.SecurityDescription.Trim();
            var model = securityViewModel;
            if (_securityQueryProcessor.Exists(p => p.SecurityCode == model.SecurityCode && p.CompanyId == model.CompanyId))
            {
                return BadRequest(SecurityConstants.SecurityControllerConstants.SecurityExists);
            }

            var mapper = new SecurityToSecurityViewModelMapper();
            var newSecurity = mapper.Map(securityViewModel);
            try
            {
                var savedSecurity = _securityQueryProcessor.Save(newSecurity);
                securityViewModel = mapper.Map(savedSecurity);
                return Ok(securityViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int) SecurityId.AddSecurity, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public ObjectResult Delete(int id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteSecurity))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            try
            {
                isDeleted  = _securityQueryProcessor.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeleteSecurity, ex, ex.Message);
            }
            return Ok(isDeleted);
        }

        [HttpPut]
        [Route("updatesecurity")]
        public ObjectResult Put([FromBody] SecurityViewModel securityViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.UpdateSecurity))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });

            var model = securityViewModel;
            if (_securityQueryProcessor.Exists(p => p.SecurityCode == model.SecurityCode && p.Id != model.Id && p.CompanyId == model.CompanyId))
            {
                return BadRequest(SecurityConstants.SecurityControllerConstants.SecurityExists);
            }

            var mapper = new SecurityToSecurityViewModelMapper();
            var newSecurity = mapper.Map(securityViewModel);
            try
            {
                var updatedSecurity =_securityQueryProcessor.Update(newSecurity);
                securityViewModel = mapper.Map(updatedSecurity);
                return Ok(securityViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.UpdateSecurity, ex, ex.Message);
                ModelState.AddModelError("", ex.Message);
                return BadRequest(new
                {
                    errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                });
            }
        }

        [HttpGet]
        [Route("GetPageSize")]
        public ObjectResult GetPageSize()
        {
            var pageSize = _userSettingQueryProcessor.GetPageSize(EntityId.Security);
            return Ok(pageSize);
        }

        [HttpGet]
        [Route("searchSecurities")]
        public ObjectResult SearchSecurities()
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewSecurities))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            try
            {
                var requestInfo = _pagedDataRequestFactory.CreateWithSearchStringCompanyActive(Request.GetUri());
                requestInfo.PageSize = _userSettingQueryProcessor.ForPaging(requestInfo, EntityId.Security);
                return Ok(_securityQueryProcessor.SearchSecurities(requestInfo));
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewSecurities, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

    }
}
