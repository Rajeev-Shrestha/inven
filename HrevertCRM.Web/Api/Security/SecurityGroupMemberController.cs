using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using Hrevert.Common.Constants.Others;
using Hrevert.Common.Constants.Security;
using HrevertCRM.Data;
using HrevertCRM.Data.QueryProcessors;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using HrevertCRM.Data.Mapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;

namespace HrevertCRM.Web.Api
{
    [Route("api/[controller]")]
    public class SecurityGroupMemberController : Controller
    {
        private readonly ISecurityGroupMemberQueryProcessor _securityGroupMemberQueryProcessor;
        private readonly ISecurityQueryProcessor _securityQueryProcessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<SecurityGroupMemberController> _logger;

        public SecurityGroupMemberController(
            ISecurityGroupMemberQueryProcessor securityGroupMemberQueryProcessor,
            IDbContext context, ILoggerFactory factory,
            ISecurityQueryProcessor securityQueryProcessor,
                UserManager<ApplicationUser> userManager)
        {
            _securityGroupMemberQueryProcessor = securityGroupMemberQueryProcessor;
            _securityQueryProcessor = securityQueryProcessor;
            _userManager = userManager;
            _logger = factory.CreateLogger<SecurityGroupMemberController>();
        }

        [HttpGet]
        [Route("getallsecuritygroupmembers")]
        public ObjectResult GetAll()
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewSecurityGroupMembers))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            try
            {
                var mapper = new SecurityGroupMemberToSecurityGroupMemberViewModelMapper();
                return Ok(_securityGroupMemberQueryProcessor.GetActiveSecurityGroupMembers().Select(s => mapper.Map(s)));
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int) SecurityId.ViewSecurityGroupMembers, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetSecurityGroupMember/{securityGroupId}/{memberId}")]
        public ObjectResult Get(int securityGroupId, string memberId)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewSecurityGroupMembers))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            return Ok(_securityGroupMemberQueryProcessor.GetSecurityGroupMember(memberId, securityGroupId));
        }

        [HttpGet]
        [Route("getsecuritymember/{id}")]
        public ObjectResult GetMembers(int id)
        {
            return Ok(_securityGroupMemberQueryProcessor.GetMembers(id));
        }

        [HttpPost]
        [Route("createsecuritygroup")]
        public ObjectResult Create([FromBody] SecurityGroupMemberViewModel securityGroupMemberViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.AddSecurityGroupMember))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });
            var currentUserCompanyId = _userManager.FindByEmailAsync(User.Identity.Name).Result.CompanyId;
            securityGroupMemberViewModel.CompanyId = currentUserCompanyId;
            var model = securityGroupMemberViewModel;
            if (
                _securityGroupMemberQueryProcessor.Exists(
                    p => p.MemberId == model.MemberId && p.SecurityGroupId == model.SecurityGroupId && p.CompanyId == model.CompanyId))
            {
                return BadRequest(SecurityConstants.SecurityGroupMemberControllerConstants.MemberAlreadyExists);
            }

            var mapper = new SecurityGroupMemberToSecurityGroupMemberViewModelMapper();
            var newSecurityGroupMember = mapper.Map(securityGroupMemberViewModel);
            try
            {
                var savedSecurityGroupMember = _securityGroupMemberQueryProcessor.Save(newSecurityGroupMember);
                securityGroupMemberViewModel = mapper.Map(savedSecurityGroupMember);
                return Ok(securityGroupMemberViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int) SecurityId.AddSecurityGroupMember, ex, ex.Message);
                ModelState.AddModelError("", ex.Message);
                return BadRequest(new
                {
                    errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                });
            }
        }

        [HttpDelete("{id}")]
        public ObjectResult Delete(string memberId,int securityGroupId)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteSecurityGroupMember))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            try
            {
                isDeleted = _securityGroupMemberQueryProcessor.Delete(memberId, securityGroupId);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeleteSecurityGroupMember, ex, ex.Message);
            }
            return Ok(isDeleted);
        }

        [HttpPost]
        [Route("savemembersingroup/{id}")]
        public ObjectResult SaveMembersInGroup(int id,[FromBody] List<string> membersList)
        {   
            var securityGroupMemberViewModel = new SecurityGroupMemberViewModel
            {
                SecurityGroupId = id,
                MembersOfGroupList = membersList
            };
            DetermineMembersToAdd(securityGroupMemberViewModel);
            var securityGroupMembersList = new List<SecurityGroupMember>();
            var mapper = new SecurityGroupMemberToSecurityGroupMemberViewModelMapper();

            foreach (var memberId in securityGroupMemberViewModel.MembersOfGroupList)
            {
                securityGroupMemberViewModel.MemberId = memberId;
                var securityGroupMember = mapper.Map(securityGroupMemberViewModel);
                securityGroupMembersList.Add(securityGroupMember);
            }

            try
            {
                var numberOfSavedMembers = _securityGroupMemberQueryProcessor.SaveAll(securityGroupMembersList);
                return Ok(numberOfSavedMembers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private void DetermineMembersToAdd(SecurityGroupMemberViewModel securityGroupMemberViewModel)
        {
            //fetch existing catgories
            var existingMembers =
                _securityGroupMemberQueryProcessor.GetExistingMembersOfGroup(securityGroupMemberViewModel.SecurityGroupId);
            foreach (var memberId in existingMembers)
            {
                //var categoryName = _productCategoryQueryProcessor.GetCategoryNameByCategoryId(categoryId);
                if (!securityGroupMemberViewModel.MembersOfGroupList.Contains(memberId))
                {
                    //remove from db
                    _securityGroupMemberQueryProcessor.Delete(memberId, securityGroupMemberViewModel.SecurityGroupId);
                }
                else
                {
                    //leave as it is
                    securityGroupMemberViewModel.MembersOfGroupList.Remove(memberId);
                }
            }
        }
    }
}
