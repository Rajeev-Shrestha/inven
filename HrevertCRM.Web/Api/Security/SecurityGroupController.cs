using System;
using System.Linq;
using System.Security;
using Hrevert.Common.Constants.Others;
using Hrevert.Common.Constants.Security;
using HrevertCRM.Data;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.Mapper;
using HrevertCRM.Data.QueryProcessors;
using HrevertCRM.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using HrevertCRM.Entities;
using System.Collections.Generic;

namespace HrevertCRM.Web.Api
{
    [Route("api/[controller]")]
    public class SecurityGroupController : Controller
    {
        private readonly ISecurityGroupQueryProcessor _securityGroupQueryProcessor;
        private readonly ISecurityRightQueryProcessor _securityRightQueryProcessor;
        private readonly ISecurityQueryProcessor _securityQueryProcessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<SecurityGroupController> _logger;

        public SecurityGroupController(ISecurityGroupQueryProcessor securityGroupQueryProcessor,
            ISecurityRightQueryProcessor securityRightQueryProcessor,
            IDbContext context, IPagedDataRequestFactory pagedDataRequestFactory,
            ILoggerFactory factory, ISecurityQueryProcessor securityQueryProcessor,
                UserManager<ApplicationUser> userManager)
        {
            _securityGroupQueryProcessor = securityGroupQueryProcessor;
            _securityRightQueryProcessor = securityRightQueryProcessor;
            _securityQueryProcessor = securityQueryProcessor;
            _userManager = userManager;
            _logger = factory.CreateLogger<SecurityGroupController>();
        }

       [HttpGet]
       [Route("getallsecuritygroup")]
        public ObjectResult GetAll()
       {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewSecurityGroups))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            try
            {
                var mapper = new SecurityGroupToSecurityGroupViewModelMapper();
                var securityGroups= _securityGroupQueryProcessor.GetSecurityGroup().Select(x => mapper.Map(x)).ToList();
               
                return Ok(securityGroups);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewSecurityGroups, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        [Route("GetSecurityGroup/{id}")]
        public ObjectResult Get(int id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewSecurityGroups))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            return Ok(_securityGroupQueryProcessor.GetSecurityGroup(id));
        }


        [HttpGet]
        [Route("GetSecurityGroups")]
        public ObjectResult GetSecurityGroupsByLoggedInUserId()
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewSecurityGroups))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            return Ok(_securityGroupQueryProcessor.GetSecurityGroupsByLoggedInUserId());
        }

        [HttpPost]
        [Route("createsecuritygroup")]
        public ObjectResult Create([FromBody] SecurityGroupViewModel securityGroupViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.AddSecurityGroup))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });
            var currentUserCompanyId = _userManager.FindByEmailAsync(User.Identity.Name).Result.CompanyId;
            securityGroupViewModel.CompanyId = currentUserCompanyId;
            securityGroupViewModel.GroupDescription = securityGroupViewModel.GroupDescription.Trim();
            securityGroupViewModel.GroupName = securityGroupViewModel.GroupName.Trim();

            if (_securityGroupQueryProcessor.Exists(p => p.GroupName == securityGroupViewModel.GroupName && p.CompanyId == securityGroupViewModel.CompanyId))
            {
                return BadRequest(SecurityConstants.SecurityGroupControllerConstants.SecurityGroupExists);
            }

            var mapper = new SecurityGroupToSecurityGroupViewModelMapper();
            var newSecurityGroup = mapper.Map(securityGroupViewModel);

            try
            {
                var savedSecurityGroup =_securityGroupQueryProcessor.Save(newSecurityGroup);
                return Ok(savedSecurityGroup);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int) SecurityId.AddSecurityGroup, ex, ex.Message);
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
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteSecurityGroup))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            try
            {
                isDeleted = _securityGroupQueryProcessor.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeleteSecurity, ex, ex.Message);
            }
            return Ok(isDeleted);
        }

        [HttpPost]
        [Route("assignsecuritytogroup")]
        public ObjectResult AssignSecurityToGroup([FromBody]AssignSecurityToGroupViewModel assignSecurityToGroup)
        {
           var result= _securityRightQueryProcessor.AssignRemoveSecurityRightToGroup(assignSecurityToGroup);
             return Ok(result);
        }

        [HttpPost]
        [Route("assignsecuritytouser")]
        public ObjectResult AssignSecurityToUser([FromBody]AssignSecurityToUserViewModel assignSecurityToUser)
        {
            var result = _securityRightQueryProcessor.AssignRemoveSecurityRightToUser(assignSecurityToUser);
            return Ok(result);
        }

        [HttpGet]
        [Route("getsecurityassignedtogroup/{id}")]
        public ObjectResult GetSecurityAssignedToGroup(int id)
        {
            var result = _securityRightQueryProcessor.GetAssignedGroupSecurity(id);
            return Ok(result);
        }

        [HttpPut]
        [Route("updatesecuritygroup")]
        public ObjectResult Put([FromBody] SecurityGroupViewModel securityGroupViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.UpdateSecurityGroup))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });

            securityGroupViewModel.GroupDescription = securityGroupViewModel.GroupDescription.Trim();
            securityGroupViewModel.GroupName = securityGroupViewModel.GroupName.Trim();

            if (_securityGroupQueryProcessor.Exists(p => p.GroupName == securityGroupViewModel.GroupName && p.Id != securityGroupViewModel.Id && p.CompanyId == securityGroupViewModel.CompanyId))
            {
                return BadRequest(SecurityConstants.SecurityGroupControllerConstants.SecurityGroupExists);
            }

            var mapper = new SecurityGroupToSecurityGroupViewModelMapper();
            var newSecurityGroup = mapper.Map(securityGroupViewModel);
            try
            {
                _securityGroupQueryProcessor.Update(newSecurityGroup);
                return Ok(securityGroupViewModel);
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
    }
}
