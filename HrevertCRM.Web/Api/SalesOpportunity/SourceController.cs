using Hrevert.Common.Constants.Others;
using HrevertCRM.Data;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.Mapper;
using HrevertCRM.Data.QueryProcessors;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace HrevertCRM.Web.Api
{

    [Route("api/[controller]")]
    public class SourceController : Controller
    {
        private readonly ISourceQueryProcessor _sourceQueryProcessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISecurityQueryProcessor _securityQueryProcessor;
        public SourceController(ISourceQueryProcessor sourceQueryProcessor,
            UserManager<ApplicationUser> userManager,
            IPagedDataRequestFactory pagedDataRequestFactory,
            ISecurityQueryProcessor securityQueryProcessor
        )
        {
            _sourceQueryProcessor = sourceQueryProcessor;
            _userManager = userManager;
            _securityQueryProcessor = securityQueryProcessor;
        }

        [HttpGet]
        [Route("getAllSources")]
        public ObjectResult GetAll()
        {
            var result = _sourceQueryProcessor.GetAll();
            var mapper = new SourceToSourceViewModelMapper();
            var resultData = mapper.Map(result.ToList());
            return Ok(resultData);
        }

        [HttpGet]
        [Route("getAllActiveSources")]
        public ObjectResult GetAllActive()
        {
            var result = _sourceQueryProcessor.GetAllActive();
            var mapper = new SourceToSourceViewModelMapper();
            var resultData = mapper.Map(result.ToList());
            return Ok(resultData);
        }

        [HttpPost]
        [Route("Create")]
        public ObjectResult CreateNewSource([FromBody] SourceViewModel sourceViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.AddSource))
                throw new System.Security.SecurityException(OtherConstants.SecurityException.UserDoesNotHaveRightException);
            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });
            var currentUserCompanyId = _userManager.FindByEmailAsync(User.Identity.Name).Result.CompanyId;
            sourceViewModel.CompanyId = currentUserCompanyId;
            sourceViewModel.SourceName = sourceViewModel.SourceName.Trim();
            var mapper = new SourceToSourceViewModelMapper();
            var mappedData = mapper.Map(sourceViewModel);
            var result = _sourceQueryProcessor.Save(mappedData);
            sourceViewModel = mapper.Map(result);
            return Ok(sourceViewModel);
        }

        [HttpDelete("{id}")]
        public void DeleteSource(int id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteSource))
                throw new System.Security.SecurityException(OtherConstants.SecurityException.UserDoesNotHaveRightException);
            _sourceQueryProcessor.Delete(id);
        }
        [HttpPut]
        [Route("Update")]
        public ObjectResult UpdateSource([FromBody] SourceViewModel sourceViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.UpdateSource))
                throw new System.Security.SecurityException(OtherConstants.SecurityException.UserDoesNotHaveRightException);
            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });
            sourceViewModel.SourceName = sourceViewModel.SourceName.Trim();
            var mapper = new SourceToSourceViewModelMapper();
            var mappedData = mapper.Map(sourceViewModel);
            var sourceById = _sourceQueryProcessor.GetSourceById(mappedData.Id);
            if (sourceById != null)
            {
                var result = _sourceQueryProcessor.Update(mappedData);
                var sourceResultViewModel = mapper.Map(result);
                return Ok(sourceResultViewModel);
            }
            else
            {
                return Ok(BadRequest("Cannot update Source"));
            }
        }
    }
}
