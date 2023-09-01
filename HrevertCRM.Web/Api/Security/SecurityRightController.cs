using System;
using System.Linq;
using System.Security;
using Hrevert.Common.Constants.Others;
using Hrevert.Common.Constants.Security;
using HrevertCRM.Data;
using HrevertCRM.Data.QueryProcessors;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Data.Mapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using HrevertCRM.Entities;

namespace HrevertCRM.Web.Api
{
    [Route("api/[controller]")]
    public class SecurityRightController : Controller
    {
        private readonly ISecurityRightQueryProcessor _securityRightQueryProcessor;
        private readonly ISecurityQueryProcessor _securityQueryProcessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<SecurityRightController> _logger;

        public SecurityRightController(ISecurityRightQueryProcessor securityRightQueryProcessor,
            IDbContext context, ILoggerFactory factory,
            ISecurityQueryProcessor securityQueryProcessor,
                UserManager<ApplicationUser> userManager)
        {
            _securityRightQueryProcessor = securityRightQueryProcessor;
            _securityQueryProcessor = securityQueryProcessor;
            _userManager = userManager;
            _logger = factory.CreateLogger<SecurityRightController>();
        }

        [HttpGet]
        [Route("getallsecurityright")]
        public ObjectResult GetAll()
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewSecurityRights))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            try
            {
                var mapper = new SecurityRightToSecurityRightViewModelMapper();
                return
                    Ok(
                        _securityRightQueryProcessor.GetActiveSecurityRights()
                            .Select(securityRight => mapper.Map(securityRight)));
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int) SecurityId.ViewSecurityRights, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetSecurityRight/{id}")]
        public ObjectResult Get(int id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewSecurityRights))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            return Ok(_securityRightQueryProcessor.GetSecurityRight(id));
        }

        [HttpPost]
        [Route("createsecurityright")]
        public ObjectResult Create([FromBody] SecurityRightViewModel securityRightViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.AddSecurityRight))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });

            var currentUserCompanyId = _userManager.FindByEmailAsync(User.Identity.Name).Result.CompanyId;
            securityRightViewModel.CompanyId = currentUserCompanyId;
            var model = securityRightViewModel;
            if (_securityRightQueryProcessor.Exists(p => p.UserId == model.UserId &&
                                                         p.SecurityGroupId == model.SecurityGroupId &&
                                                         p.SecurityId == model.SecurityId
                                                         && p.SecurityGroupId == model.SecurityGroupId
                                                         && p.CompanyId == model.CompanyId))
            {
                return BadRequest(SecurityConstants.SecurityRightControllerConstants.AlreadyHasRights);
            }

            var mapper = new SecurityRightToSecurityRightViewModelMapper();
            var newSecurityRight = mapper.Map(securityRightViewModel);
            try
            {
                var savedSecurityRight = _securityRightQueryProcessor.Save(newSecurityRight);
                securityRightViewModel = mapper.Map(savedSecurityRight);
                return Ok(securityRightViewModel);

            }
            catch (Exception ex)
            {
                _logger.LogCritical((int) SecurityId.AddSecurityRight, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public ObjectResult Delete(int id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteSecurityRight))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            try
            {
                isDeleted = _securityRightQueryProcessor.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeleteSecurityRight, ex, ex.Message);
            }
            return Ok(isDeleted);
        }

        [HttpPut]
        [Route("updatesecurityright")]
        public ObjectResult Put([FromBody] SecurityRightViewModel securityRightViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.UpdateSecurityRight))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });

            var model = securityRightViewModel;
            if (_securityRightQueryProcessor.Exists(p => p.UserId == model.UserId &&
            p.SecurityGroupId == model.SecurityGroupId && p.SecurityId == model.SecurityId
            && p.SecurityGroupId == model.SecurityGroupId && p.Id != model.Id && p.CompanyId==model.CompanyId))
            {
                return BadRequest(SecurityConstants.SecurityRightControllerConstants.AlreadyHasRights);
            }

            var mapper = new SecurityRightToSecurityRightViewModelMapper();
            var newSecurityRight = mapper.Map(securityRightViewModel);
            try
            {
                var updatedSecurityRight =_securityRightQueryProcessor.Update(newSecurityRight);
                securityRightViewModel = mapper.Map(updatedSecurityRight);
                return Ok(securityRightViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.UpdateSecurityRight, ex, ex.Message);
                ModelState.AddModelError("", ex.Message);
                return BadRequest(new
                {
                    errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                });
            }
        }

        [HttpGet]
        [Route("GetAssignedUserSecurity/{id}")]
        public ObjectResult GetAssignedUserSecurity(string id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewSecurityRights))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            return Ok(_securityRightQueryProcessor.GetAssignedUserSecurity(id));
        }
    }
}
