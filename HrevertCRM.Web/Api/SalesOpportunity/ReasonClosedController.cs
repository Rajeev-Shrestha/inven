using Hrevert.Common.Constants.Others;
using HrevertCRM.Data;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.Mapper;
using HrevertCRM.Data.QueryProcessors;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace HrevertCRM.Web.Api
{
    [Route("api/[controller]")]
    public class ReasonClosedController:Controller
    {
        private readonly IReasonClosedQueryProcessor _reasonClosedQueryProcessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISecurityQueryProcessor _securityQueryProcessor;
        private readonly ILogger<ReasonClosedController> _logger;
        public ReasonClosedController(
            IReasonClosedQueryProcessor reasonClosedQueryProcessor,
            UserManager<ApplicationUser> userManager,
            IPagedDataRequestFactory pagedDataRequestFactory,
            ISecurityQueryProcessor securityQueryProcessor,
            ILoggerFactory factory
        )
        {
            _reasonClosedQueryProcessor = reasonClosedQueryProcessor;
            _userManager = userManager;
            _securityQueryProcessor = securityQueryProcessor;
            _logger = factory.CreateLogger<ReasonClosedController>();
        }

        [HttpGet]
        [Route("getAllReasonClosed")]
        public ObjectResult GetAll()
        {
            var result = _reasonClosedQueryProcessor.GetAll();
            var mapper = new ReasonClosedToReasonClosedViewModelMapper();
            var resultData = mapper.Map(result.ToList());
            return Ok(resultData);
        }

        [HttpGet]
        [Route("getAllActiveReasonClosed")]
        public ObjectResult GetAllActive()
        {
            var result = _reasonClosedQueryProcessor.GetAllActive();
            var mapper = new ReasonClosedToReasonClosedViewModelMapper();
            var resultData = mapper.Map(result.ToList());
            return Ok(resultData);
        }

        [HttpPost]
        [Route("Create")]
        public ObjectResult CreateNewReasonClosed([FromBody] ReasonClosedViewModel reasonClosedViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.AddReasonClosed))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });
            var currentUserCompanyId = _userManager.FindByEmailAsync(User.Identity.Name).Result.CompanyId;
            reasonClosedViewModel.CompanyId = currentUserCompanyId;
            reasonClosedViewModel.Reason = reasonClosedViewModel.Reason.Trim();
            var mapper = new ReasonClosedToReasonClosedViewModelMapper();
            var mappedData = mapper.Map(reasonClosedViewModel);
            var result = _reasonClosedQueryProcessor.Save(mappedData);
            reasonClosedViewModel = mapper.Map(result);
            return Ok(reasonClosedViewModel);
        }

        [HttpDelete("{id}")]
        public ObjectResult DeleteReasonClosed(int id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteReasonClosed))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            try
            {
                isDeleted = _reasonClosedQueryProcessor.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeleteReasonClosed, ex, ex.Message);
            }
            return Ok(isDeleted);
           
        }

        [HttpPut]
        [Route("Update")]
        public ObjectResult UpdateReasonClosed([FromBody] ReasonClosedViewModel reasonClosedViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.UpdateReasonClosed))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            reasonClosedViewModel.Reason = reasonClosedViewModel.Reason.Trim();
            var mapper = new ReasonClosedToReasonClosedViewModelMapper();
            var mappedData = mapper.Map(reasonClosedViewModel);
            var reasonClosedById = _reasonClosedQueryProcessor.GetReasonClosedById(mappedData.Id);
            if (reasonClosedById != null)
            {
                var result = _reasonClosedQueryProcessor.Update(mappedData);
                var reasonColsedResultViewModel = mapper.Map(result);
                return Ok(reasonColsedResultViewModel);
            }
            else
            {
                return Ok(BadRequest("Cannot update Reason Closed"));
            }

        }
        
    }
}
