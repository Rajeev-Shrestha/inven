using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hrevert.Common.Constants.Others;
using Hrevert.Common.Constants.User;
using HrevertCRM.Data;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.QueryProcessors;
using HrevertCRM.Entities;
using HrevertCRM.Data.Mapper;
using HrevertCRM.Data.QueryProcessors.Enumerations;
using HrevertCRM.Data.ViewModels.User;
using HrevertCRM.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.Extensions.Logging;
using MailKit.Net.Smtp;
using MimeKit;

namespace HrevertCRM.Web.Api
{
   
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserQueryProcessor _userQueryProcessor;
        private readonly IPagedDataRequestFactory _pagedDataRequestFactory;
        private readonly IUserTypesQueryProcessor _userTypesQueryProcessor;
        private readonly ISecurityQueryProcessor _securityQueryProcessor;
        private readonly ICompanyQueryProcessor _companyQueryProcessor;
        private readonly ICompanyWebSettingQueryProcessor _companyWebSettingQueryProcessor;
        private readonly ISecurityGroupMemberQueryProcessor _securityGroupMemberQueryProcessor;
        private readonly IDbContext _context;
        private readonly ILogger<UserController> _logger;

        public UserController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUserQueryProcessor userQueryProcessor,
            IPagedDataRequestFactory pagedDataRequestFactory,
            IUserTypesQueryProcessor userTypesQueryProcessor,
            ISecurityQueryProcessor securityQueryProcessor,
            ILoggerFactory factory,
            IUserSettingQueryProcessor userSettingQueryProcessor,
            ICompanyQueryProcessor companyQueryProcessor,
            ICompanyWebSettingQueryProcessor companyWebSettingQueryProcessor,
            ISecurityGroupMemberQueryProcessor securityGroupMemberQueryProcessor,
            IDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userQueryProcessor = userQueryProcessor;
            _pagedDataRequestFactory = pagedDataRequestFactory;
            _userTypesQueryProcessor = userTypesQueryProcessor;
            _securityQueryProcessor = securityQueryProcessor;
            _logger = factory.CreateLogger<UserController>();
            _companyQueryProcessor = companyQueryProcessor;
            _companyWebSettingQueryProcessor = companyWebSettingQueryProcessor;
            _securityGroupMemberQueryProcessor = securityGroupMemberQueryProcessor;
            _context = context;
        }

        [HttpGet]
        [Route("getuserbyid/{id}")]
        public async Task<ObjectResult> GetById(string id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long) SecurityId.ViewUsers))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var mapper = new UserToUserViewModelMapper();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound(UserConstants.UserControllerConstants.UserNotFound);
            var result = mapper.Map(user);
            return Ok(result);
        }

        [HttpGet]
        [Route("getuserbyidwithoutidentity/{id}")]
        public ObjectResult GetUserById(string id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewUsers))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            var companyId = _userManager.FindByEmailAsync(User.Identity.Name).Result.CompanyId;
            var user = _userQueryProcessor.GetUser(id, companyId);

            if (user.Equals(null)) return NotFound(UserConstants.UserControllerConstants.UserNotFound);
            var mapper = new UserToUserViewModelMapper();
            var userVm = mapper.Map(user);

            var companyVm = _companyQueryProcessor.GetCompanyByUserId(userVm.Id);
            var isEstoreInitialized = _companyWebSettingQueryProcessor.CheckIfEstoreIsInitialized(userVm.Id);
            userVm.IsCompanyInitialized = companyVm.IsCompanyInitialized;
            userVm.IsEstoreInitialized = isEstoreInitialized;
            userVm.CompanyName = companyVm.Name;
            userVm.CompanyVersion = companyVm.Version;
            return Ok(userVm);
        }

        [Route("login"), HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginViewModel loginInfo)
        {
            if (!ModelState.IsValid) return BadRequest();
            var user = await _userManager.FindByEmailAsync(loginInfo.UserName);
            if (!user.Active) return BadRequest("Invalid Login Attempt");
            var result =
                await
                    _signInManager.PasswordSignInAsync(loginInfo.UserName, loginInfo.Password, isPersistent: false,
                        lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return Json("OK");
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("UserSummary")]
        public ObjectResult GetUserSummary()
        {
            var currentUser = _userManager.FindByEmailAsync(User.Identity.Name).Result;

            var users = _userManager.Users.Where(user => user.CompanyId == currentUser.CompanyId && user.Active)
                .Include(s => s.SecurityGroupMemberUsers);

            var list = new List<UserSummaryViewModel>();
            var userMapper = new UserToUserSummaryViewModelMapper();

            foreach (var user in users)
            {
                var securityGroupIds = user.SecurityGroupMemberUsers.Where(x => x.MemberId == user.Id)
                    .Select(x => x.SecurityGroupId)
                    .ToList();

                var securityGroupNames = _userQueryProcessor.GetGroupNamesFromGroupIds(securityGroupIds);

                var groupNames = string.Join(", ", securityGroupNames);
                var userSummaryViewModel = userMapper.Map(user);
                userSummaryViewModel.SecurityGroupNames = groupNames;

                if (userSummaryViewModel.Active)
                {
                    list.Add(userSummaryViewModel);
                }
            }
            return Ok(list);
        }

        [HttpGet]
        [Route("GetLoggedInUserDetail")]
        public async Task<ObjectResult> GetLoggedInUserDetail()
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            if (user.Equals(null)) return NotFound(UserConstants.UserControllerConstants.UserNotFound);
            var mapper = new UserToUserViewModelMapper();
            var userVm = mapper.Map(user);

            #region Check if this User has Authority to Assign Rights or Not

            var securityGroupsWithAuthorityToAssignRight =
                _securityQueryProcessor.GetGroupsWithAuthorityToAssignRight();

            var securityGroups = _securityGroupMemberQueryProcessor.GetSecurityGroupsOfMemberId(user.Id);
            if (securityGroupsWithAuthorityToAssignRight == null) return Ok(userVm);
            foreach (var securityGroupId in securityGroupsWithAuthorityToAssignRight)
            {
                if (securityGroups != null && securityGroupId != null && securityGroups.Contains((int)securityGroupId))
                    userVm.HasAuthorityToAssignRight = true;
            }

            #endregion

            return Ok(userVm);
        }

        [Authorize]
        [HttpGet]
        [Route("CheckFirstLogin")]
        public async Task<ObjectResult> GetLoggedInUserDetailToCheckFirstLogin()
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewUsers))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            var user = await _userManager.FindByEmailAsync(User.Identity.Name);            
            if (user.Equals(null)) return NotFound(UserConstants.UserControllerConstants.UserNotFound);
            var mapper = new UserToUserViewModelMapper();
            var userVm = mapper.Map(user);
            var companyVm = _companyQueryProcessor.GetCompanyByUserId(userVm.Id);
            var isEstoreInitialized = _companyWebSettingQueryProcessor.CheckIfEstoreIsInitialized(userVm.Id);
            userVm.IsCompanyInitialized = companyVm.IsCompanyInitialized;
            userVm.IsEstoreInitialized = isEstoreInitialized;
            userVm.CompanyName = companyVm.Name;
            userVm.CompanyVersion = companyVm.Version;
            userVm.CompanyViewModel = companyVm;
            return Ok(userVm);
        }

        [HttpGet]
        [Route("getbyid/{id}")]
        public ObjectResult Get(string id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long) SecurityId.ViewUsers))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var mapper = new UserToUserViewModelMapper();
            //Retrieving Current User's companyId by its email
            var companyId = _userManager.FindByEmailAsync(User.Identity.Name).Result.CompanyId;

            return Ok(mapper.Map(_userQueryProcessor.GetUser(id, companyId)));
        }

        [HttpGet]
        [Route("searchactiveusers")]
        public ObjectResult SearchActive()
        {
            var requestInfo = _pagedDataRequestFactory.CreateWithSearchString(Request.GetUri());
            return Ok(_userQueryProcessor.SearchActive(requestInfo));
        }

        [HttpGet]
        [Route("searchallusers")]
        public ObjectResult SearchAll()
        {
            var requestInfo = _pagedDataRequestFactory.CreateWithSearchString(Request.GetUri());
            return Ok(_userQueryProcessor.SearchAll(requestInfo));
        }

        [HttpPost]
        [Route("createuser")]
        public async Task<ObjectResult> Create([FromBody] UserViewModel userViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long) SecurityId.AddUser))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                });
            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    //Retrieving the current application user in order to assign the companyId to the new User
                    //userViewModel.UserType = UserType.CompanyUsers;
                    var loggedInUser = _userManager.FindByEmailAsync(User.Identity.Name).Result;

                    //Checking if a user already exists with the same email
                    var oldUser = await _userManager.FindByEmailAsync(userViewModel.Email);
                    if (oldUser != null)
                    {
                        return Ok(BadRequest(UserConstants.UserControllerConstants.UserAlreadyExists));
                    }
                    var mapper = new UserToUserViewModelMapper();
                    var newUser = mapper.Map(userViewModel);
                    newUser.CompanyId = loggedInUser.CompanyId; //Assigning the companyid to the new user

                    var res = await _userManager.CreateAsync(newUser, userViewModel.Password);
                    if (!res.Succeeded) return BadRequest(res.Errors);

                    //Save Roles of Users
                    userViewModel.Id = newUser.Id;
                    if (userViewModel.SecurityGroupIdList != null && userViewModel.SecurityGroupIdList.Count > 0)
                        SaveRolesOfUsers(userViewModel);

                    //Send Email for Confirmation
                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                    //var callbackUrl = Url.Action("ConfirmEmail", "Account", new {userId = newUser.Id, code},
                    //    HttpContext.Request.Scheme);
                    //using (var client = new SmtpClient())
                    //{
                    //    var emailMessage = new MimeMessage();
                    //    emailMessage.To.Add(new MailboxAddress(userViewModel.Email));
                    //    emailMessage.From.Add(new MailboxAddress("hrevertcrm@ntbazar.com"));
                    //    emailMessage.Subject = "Confirm Your Account";
                    //    var builder = new BodyBuilder
                    //    {
                    //        TextBody = $"Please confirm your account by clicking this link: " +
                    //                   $"<a href='{callbackUrl}'>link</a>"
                    //    };
                    //    emailMessage.Body = builder.ToMessageBody();
                    //    await client.ConnectAsync("mail.ntbazar.com", 8889);
                    //    await client.AuthenticateAsync("hrevertcrm@ntbazar.com", "p@77w0rd");
                    //    await client.SendAsync(emailMessage);
                    //}
                    _logger.LogInformation(3, "User created a new account with password.");
                    trans.Commit();
                    return Ok(res);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    _logger.LogCritical((int)SecurityId.AddUser, ex, ex.Message);
                    ModelState.AddModelError("", ex.Message);
                    return BadRequest(new
                    {
                        errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                    });
                }
            }
        }

        private void SaveRolesOfUsers(UserViewModel userViewModel)
        {
            var rolesOfUsersList = new List<SecurityGroupMember>();
            if (userViewModel.SecurityGroupIdList != null && userViewModel.SecurityGroupIdList.Count > 0)
            {
                foreach (var securityGroupId in userViewModel.SecurityGroupIdList)
                {
                    if(userViewModel.Id != null)
                        rolesOfUsersList.Add(new SecurityGroupMember
                        {
                            SecurityGroupId = securityGroupId,
                            MemberId = userViewModel.Id,
                        });
                }
            }
            _securityGroupMemberQueryProcessor.SaveAll(rolesOfUsersList);
        }

        [HttpDelete]
        [Route("deleteuser/{id}")]
        public ObjectResult Delete(string id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long) SecurityId.DeleteUser))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (id == null) return Ok("User can not be null");
            var isDeleted = false;
            try
            {
                //Retrieving Current User's companyId by its email
                var companyId = _userManager.FindByEmailAsync(User.Identity.Name).Result.CompanyId;

                isDeleted = _userQueryProcessor.DeleteUser(id, companyId);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeleteUser, ex, ex.Message);
            }
            return Ok(isDeleted);
        }

        private void DetermineRolesToAdd(UserViewModel userViewModel)
        {
            //Fetch existing roles
            if (userViewModel.Id == null) return;
            var existingRoles = _securityGroupMemberQueryProcessor.GetExistingRolesOfUser(userViewModel.Id);
            foreach (var roleId in existingRoles)
            {
                if (!userViewModel.SecurityGroupIdList.Contains(roleId))
                {
                    //remove from db
                    _securityGroupMemberQueryProcessor.Delete(userViewModel.Id, roleId);
                }
                else
                {
                    //leave as it is
                    userViewModel.SecurityGroupIdList.Remove(roleId);
                }
            }
        }

        [Authorize]
        [HttpPut]
        [Route("updateuser")]
        public async Task<ObjectResult> Put([FromBody] UserViewModel userModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.UpdateUser))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });

            var oldUser = await _userManager.FindByIdAsync(userModel.Id);
            //var oldUser = _userQueryProcessor.GetUser(userModel.Id, userModel.CompanyId);
            if (oldUser == null)
            {
                return Ok(BadRequest(UserConstants.UserControllerConstants.UserNotFound));
            }

            oldUser.FirstName = userModel.FirstName;
            oldUser.MiddleName = userModel.MiddleName;
            oldUser.LastName = userModel.LastName;
            oldUser.Email = userModel.Email;
            oldUser.Phone = userModel.Phone;
            oldUser.UserName = userModel.Email;
            oldUser.Gender = userModel.Gender;
            oldUser.UserType = userModel.UserType;
            oldUser.Address = userModel.Address;
            oldUser.CompanyId = userModel.CompanyId;
            oldUser.Active = userModel.Active;
            oldUser.WebActive = userModel.WebActive;

            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    if (!string.IsNullOrEmpty(userModel.Password))
                    {
                        await _userManager.RemovePasswordAsync(oldUser);
                        await _userManager.AddPasswordAsync(oldUser, userModel.Password.Trim());
                    }
                    _userQueryProcessor.UpdateUser(oldUser, userModel.CompanyId);
                    //This method deals with update of Roles in Users In SecurityGroupIdList
                    DetermineRolesToAdd(userModel);

                    //This method saves the Roles in Users In SecurityGroupIdList
                    SaveRolesOfUsers(userModel);

                    trans.Commit();
                    return Ok(oldUser);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    _logger.LogCritical((int)SecurityId.UpdateUser, ex, ex.Message);
                    ModelState.AddModelError("", ex.Message);
                    return BadRequest(new
                    {
                        errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                    });
                }
            }
        }

        public async Task<ObjectResult> UpdateUser(UserViewModel userViewModel, ApplicationUser oldUser)
        {
            if (oldUser == null)
            {
                return Ok(BadRequest(UserConstants.UserControllerConstants.UserNotFound));
            }

            oldUser.FirstName = userViewModel.FirstName;
            oldUser.MiddleName = userViewModel.MiddleName;
            oldUser.LastName = userViewModel.LastName;
            oldUser.Email = userViewModel.Email;
            oldUser.Phone = userViewModel.Phone;
            oldUser.UserName = userViewModel.Email;
            oldUser.Gender = userViewModel.Gender;
            oldUser.UserType = userViewModel.UserType;
            oldUser.Address = userViewModel.Address;
            oldUser.CompanyId = userViewModel.CompanyId;
            oldUser.Active = userViewModel.Active;
            oldUser.WebActive = userViewModel.WebActive;

            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    //oldUser.CompanyId = _userManager.FindByEmailAsync(User.Identity.Name).Result.CompanyId;
                    //await _userManager.UpdateAsync(oldUser);
                    if (userViewModel.Password != null)
                    {
                        await _userManager.RemovePasswordAsync(oldUser);
                        await _userManager.AddPasswordAsync(oldUser, userViewModel.Password.Trim());
                    }
                    _userQueryProcessor.UpdateUser(oldUser, userViewModel.CompanyId);
                    //This method deals with update of Roles in Users In SecurityGroupIdList
                    DetermineRolesToAdd(userViewModel);

                    //This method saves the Roles in Users In SecurityGroupIdList
                    SaveRolesOfUsers(userViewModel);

                    trans.Commit();
                    return Ok(oldUser);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    _logger.LogCritical((int)SecurityId.UpdateUser, ex, ex.Message);
                    ModelState.AddModelError("", ex.Message);
                    return BadRequest(new
                    {
                        errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                    });
                }
            }
        }


        [Authorize]
        [HttpPost]
        [Route("updateprofile")]
        public async Task<ObjectResult> UpdateProfile([FromBody] UserViewModel userModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.UpdateUser))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });

            var oldUser = await _userManager.FindByIdAsync(userModel.Id);
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (oldUser.Id == currentUser.Id)
            {
                return await UpdateUser(userModel, oldUser);
            }

            return Ok(BadRequest(UserConstants.UserControllerConstants.ProfileDoesNotBelongToLoggedInUser));
        }

        [Authorize]
        [HttpGet]
        [Route("activateuser/{id}")]
        public ObjectResult ActivateUser(string id)
        {
            //Retrieving Current User's companyId by its email
            var companyId = _userManager.FindByEmailAsync(User.Identity.Name).Result.CompanyId;
            var mapper = new UserToUserViewModelMapper();
            return Ok(mapper.Map(_userQueryProcessor.ActivateUser(id, companyId)));
        }

        [HttpGet]
        [Route("getusertypes")]
        public ObjectResult GetUserTypes()
        {
            return Ok(_userTypesQueryProcessor.GetActiveUserTypes());
        }

        [HttpGet]
        [Route("activeuserswithoutpaging")]
        public ObjectResult GetActiveUsersWithoutPaging()
        {
            var currentUser = _userManager.FindByEmailAsync(User.Identity.Name).Result;
            var mapper = new UserToUserViewModelMapper();
            return Ok(_userQueryProcessor.GetActiveUsersWithoutPaging(currentUser.CompanyId).Select(x => mapper.Map(x)));
        }

        [HttpGet]
        [Route("getUserNames")]
        public ObjectResult GetUserNames()
        {
            var companyId = _userManager.FindByEmailAsync(User.Identity.Name).Result.CompanyId;
            return Ok(_userQueryProcessor.GetUserNames(companyId));
        }

        [HttpGet]
        [Route("CheckIfDeletedUserWithSameEmailExists/{email}")]
        public ObjectResult CheckIfDeletedUserWithSameEmailExists(string email)
        {
            var companyId = _userManager.FindByEmailAsync(User.Identity.Name).Result.CompanyId;
            var user = _userQueryProcessor.CheckIfDeletedUserWithSameEmailExists(email, companyId);
            var userMapper = new UserToUserViewModelMapper();
            if (user != null)
            {
                userMapper.Map(user);
            }
            return Ok(user);    
        }

        [HttpGet]
        [Route("getUsers/{active}/{searchText}")]
        public ObjectResult GetUsers(bool active, string searchText)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewUsers))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            try
            {
                //Retrieving Current User by its email
                var currentUser = _userManager.FindByEmailAsync(User.Identity.Name).Result;
                var usersByCompanyId =
                    _userManager.Users.Where(user => user.CompanyId == currentUser.CompanyId)
                        .Include(s => s.SecurityGroupMemberUsers).ToList();
                if (active)
                    usersByCompanyId = usersByCompanyId.Where(x => x.Active).ToList();
                var users = string.IsNullOrEmpty(searchText) || searchText.Contains("null")
                    ? usersByCompanyId
                    : usersByCompanyId.Where(
                        x =>
                            x.FirstName.ToUpper().Contains(searchText.ToUpper()) ||
                            x.LastName.ToUpper().Contains(searchText.ToUpper()) ||
                            x.Email.ToUpper().Contains(searchText.ToUpper())).ToList();
                var list = new List<UserViewModel>();
                foreach (var user in users)
                {
                    var securityGroups =
                        user.SecurityGroupMemberUsers.Where(x => x.MemberId == user.Id)
                            .Select(x => x.SecurityGroupId)
                            .ToList();
                    var mapper = new UserToUserViewModelMapper();
                    var userViewModel = mapper.Map(user);
                    userViewModel.SecurityGroupIdList = securityGroups;
                    if (userViewModel.Active || userViewModel.Active == false)
                        list.Add(userViewModel);
                }
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("bulkDelete")]
        public ObjectResult BulkDelete([FromBody] List<string> userId)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteUser))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            if (userId == null || userId.Count <= 0) return Ok(false);
            var isDeleted = false;
            try
            {
                int companyId = _userManager.FindByEmailAsync(User.Identity.Name).Result.CompanyId;
                isDeleted = _userQueryProcessor.DeleteRange(userId, companyId);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeleteUser, ex, ex.Message);
            }
            return Ok(isDeleted);
        }
    }
}
