using System;
using System.Linq;
using System.Security;
using Hrevert.Common.Constants;
using Hrevert.Common.Constants.Others;
using HrevertCRM.Data;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.Mapper;
using HrevertCRM.Data.QueryProcessors;
using HrevertCRM.Data.QueryProcessors.Enumerations;
using HrevertCRM.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using HrevertCRM.Entities;
using System.Collections.Generic;

namespace HrevertCRM.Web.Api
{
    [Route("api/[controller]")]
    public class EmailSettingController : Controller
    {
        private readonly IEmailSettingQueryProcessor _emailSettingQueryProcessor;
        private readonly IEmailSenderQueryProcessor _emailSenderQueryProcessor;
        private readonly IEncryptionTypesQueryProcessor _encryptionTypesQueryProcessor;
        private readonly ISecurityQueryProcessor _securityQueryProcessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<EmailSettingController> _logger;

        public EmailSettingController(IEmailSettingQueryProcessor emailSettingQueryProcessor,
            IDbContext context, IPagedDataRequestFactory pagedDataRequestFactory,
            ILoggerFactory factory, IEmailSenderQueryProcessor emailSenderQueryProcessor,
            IEncryptionTypesQueryProcessor encryptionTypesQueryProcessor,
            ISecurityQueryProcessor securityQueryProcessor,
             UserManager<ApplicationUser> userManager)
        {
            _emailSettingQueryProcessor = emailSettingQueryProcessor;
            _emailSenderQueryProcessor = emailSenderQueryProcessor;
            _encryptionTypesQueryProcessor = encryptionTypesQueryProcessor;
            _securityQueryProcessor = securityQueryProcessor;
            _userManager = userManager;
            _logger = factory.CreateLogger<EmailSettingController>();
        }  

        [HttpGet]
        [Route("getemailbyid/{id}")]
        public ObjectResult Get(int id) //Get Includes Full EmailSetting data
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewEmailSettings))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            return Ok(_emailSettingQueryProcessor.GetEmailSettingViewModel(id));
        }

        [HttpPost]
        [Route("sendemail")]
        public ObjectResult SendEmail(int emailSettingId, string mailTo, string cc, string subject, string message)
        {
            if (emailSettingId == 0 || mailTo == null || subject == null || message == null)
                return BadRequest(EmailSettingConstants.EmailSettingControllerConstants.EnterRequiredFields);

            var emailSetting = _emailSettingQueryProcessor.GetEmailSettingViewModel(emailSettingId);
            var files = Request.Form.Files;
            var currentUserCompanyId = _userManager.FindByEmailAsync(User.Identity.Name).Result.CompanyId;
            var emailViewModel = new EmailSenderViewModel
            {
                MailFrom = emailSetting.Sender,
                MailTo = mailTo,
                Cc = cc,
                Subject = subject,
                Message = message,
                IsEmailSent = true,
                Active = true,
                CompanyId = currentUserCompanyId
            };
            var savedEmail = _emailSenderQueryProcessor.Save(emailViewModel);
            _emailSenderQueryProcessor.SendEmail(emailSetting, savedEmail, files);
            return Ok("");
        }

        [HttpGet]
        [Route("activateemailsetting/{id}")]
        public ObjectResult ActivateEmailSetting(int id)
        {
            var mapper = new EmailSettingToEmailSettingViewModelMapper();
            return Ok(mapper.Map(_emailSettingQueryProcessor.ActivateEmailSetting(id)));
        }

        [HttpPost]
        [Route("createemailsetting")]
        public ObjectResult Create([FromBody] EmailSettingViewModel emailSettingViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.AddEmailSetting))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });

            var currentUserCompanyId = _userManager.FindByEmailAsync(User.Identity.Name).Result.CompanyId;
            emailSettingViewModel.CompanyId = currentUserCompanyId;
            emailSettingViewModel.Host = emailSettingViewModel.Host?.Trim();
            emailSettingViewModel.Name = emailSettingViewModel.Name?.Trim();
            emailSettingViewModel.Sender = emailSettingViewModel.Sender?.Trim();
            emailSettingViewModel.UserName = emailSettingViewModel.UserName?.Trim();
            emailSettingViewModel.Password = emailSettingViewModel.Password?.Trim();

            var model = emailSettingViewModel;
            if (_emailSettingQueryProcessor.Exists(p => p.UserName == model.UserName && p.CompanyId == model.CompanyId))
            {
                return BadRequest(EmailSettingConstants.EmailSettingControllerConstants.EmailSettingAlreadyExists);
            }

            var mapper = new EmailSettingToEmailSettingViewModelMapper();
            var newEmailSetting = mapper.Map(emailSettingViewModel);
            try
            {
                var savedEmailSetting = _emailSettingQueryProcessor.Save(newEmailSetting);
                emailSettingViewModel = mapper.Map(savedEmailSetting);
                return Ok(emailSettingViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.AddEmailSetting, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public ObjectResult Delete(int id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteEmailSetting))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            try
            {
                isDeleted = _emailSettingQueryProcessor.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeleteEmailSetting, ex, ex.Message);
            }
            return Ok(isDeleted);
        }

        [HttpPut]
        [Route("updateemailsetting")]
        public ObjectResult Put([FromBody] EmailSettingViewModel emailSettingViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.UpdateEmailSetting))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });

            emailSettingViewModel.Host = emailSettingViewModel.Host?.Trim();
            emailSettingViewModel.Name = emailSettingViewModel.Name?.Trim();
            emailSettingViewModel.Sender = emailSettingViewModel.Sender?.Trim();
            emailSettingViewModel.UserName = emailSettingViewModel.UserName?.Trim();
            emailSettingViewModel.Password = emailSettingViewModel.Password?.Trim();

            var model = emailSettingViewModel;
            if (_emailSettingQueryProcessor.Exists(p => p.UserName == model.UserName && p.Id != model.Id && p.CompanyId == model.CompanyId))
            {
                return BadRequest(EmailSettingConstants.EmailSettingControllerConstants.EmailSettingAlreadyExists);
            }

            var mapper = new EmailSettingToEmailSettingViewModelMapper();
            var newEmailSetting = mapper.Map(emailSettingViewModel);
            try
            {
                var updatedEmailSetting = _emailSettingQueryProcessor.Update(newEmailSetting);
                emailSettingViewModel = mapper.Map(updatedEmailSetting);
                return Ok(emailSettingViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.UpdateEmailSetting, ex, ex.Message);
                ModelState.AddModelError("", ex.Message);
                return BadRequest(new
                {
                    errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                });
            }
        }

        [HttpGet]
        [Route("getencryptiontypes")]
        public ObjectResult GetEncryptionTypes()
        {
            return Ok(_encryptionTypesQueryProcessor.GetActiveEncryptionTypes());
        }

        [HttpGet]
        [Route("CheckIfDeletedEmailSettingWithSameNameExists/{name}")]
        public ObjectResult CheckIfDeletedEmailSettingWithSameNameExists(string name)
        {
            var emailSetting = _emailSettingQueryProcessor.CheckIfDeletedEmailSettingWithSameNameExists(name);
            var emailSettingMapper = new EmailSettingToEmailSettingViewModelMapper();
            if (emailSetting != null)
            {
                emailSettingMapper.Map(emailSetting);
            }
            return Ok(emailSetting);
        }

        [HttpGet]
        [Route("searchEmailSettings/{active}/{searchText}")]
        public ObjectResult SearchEmailSettings(bool active, string searchText)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewEmailSettings))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            try
            {
                var mapper = new EmailSettingToEmailSettingViewModelMapper();
                return Ok(_emailSettingQueryProcessor.SearchEmailSettings(active, searchText).Select(f => mapper.Map(f)).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewEmailSettings, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("bulkDelete")]
        public ObjectResult BulkDelete([FromBody] List<int?> emailSettingId)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteEmailSetting))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            if (emailSettingId == null || emailSettingId.Count <= 0) return Ok(false);
            try
            {
                isDeleted = _emailSettingQueryProcessor.DeleteRange(emailSettingId);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeleteEmailSetting, ex, ex.Message);
            }
            return Ok(isDeleted);
        }
    }
}
