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
    public class StageController : Controller
    {
        private readonly IStageQueryProcessor _stageQueryProcessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISecurityQueryProcessor _securityQueryProcessor;

        public StageController(IStageQueryProcessor stageQueryProcessor,
             UserManager<ApplicationUser> userManager,
            ISecurityQueryProcessor securityQueryProcessor,
            IPagedDataRequestFactory pagedDataRequestFactory
        )
        {
            _stageQueryProcessor = stageQueryProcessor;
            _userManager = userManager;
            _securityQueryProcessor = securityQueryProcessor;
        }
        [HttpGet]
        [Route("getAllStages")]
        public ObjectResult GetAll()
        {
            var result = _stageQueryProcessor.GetAll();
            var mapper = new StageToStageViewModelMapper();
            var resultData = mapper.Map(result.ToList());
            return Ok(resultData);
        }

        [HttpGet]
        [Route("getAllActiveStages")]
        public ObjectResult GetAllActive()
        {
            var result = _stageQueryProcessor.GetAllActive();
            var mapper = new StageToStageViewModelMapper();
            var resultData = mapper.Map(result.ToList());
            return Ok(resultData);
        }
        [HttpGet]
        [Route("getStageById/{id}")]
        public ObjectResult getStageById(int id)
        {
            var stage = _stageQueryProcessor.GetStageById(id);
            var mapper = new StageToStageViewModelMapper();
            var mappedData = mapper.Map(stage);
            return Ok(mappedData);
        }
        [HttpPost]
        [Route("Create")]
        public ObjectResult CreateNewStage([FromBody] StageViewModel stageViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.AddStage))
                throw new System.Security.SecurityException(OtherConstants.SecurityException.UserDoesNotHaveRightException);
            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });
            var currentUserCompanyId = _userManager.FindByEmailAsync(User.Identity.Name).Result.CompanyId;
            stageViewModel.CompanyId = currentUserCompanyId;
            stageViewModel.StageName = stageViewModel.StageName.Trim();
            var mapper = new StageToStageViewModelMapper();
            var mappedData = mapper.Map(stageViewModel);
            var result = _stageQueryProcessor.Save(mappedData);
            stageViewModel = mapper.Map(result);
            return Ok(stageViewModel);
        }

        [HttpDelete("{id}")]
        public void DeleteStage(int id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteStage))
                throw new System.Security.SecurityException(OtherConstants.SecurityException.UserDoesNotHaveRightException);
            _stageQueryProcessor.Delete(id);
        }
        [HttpPut]
        [Route("Update")]
        public ObjectResult UpdateStage([FromBody] StageViewModel stageViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.UpdateSource))
                throw new System.Security.SecurityException(OtherConstants.SecurityException.UserDoesNotHaveRightException);
            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });
            stageViewModel.StageName = stageViewModel.StageName.Trim();
            var mapper = new StageToStageViewModelMapper();
            var mappedData = mapper.Map(stageViewModel);
            var stageById = _stageQueryProcessor.GetStageById(mappedData.Id);
            if (stageById != null)
            {
                var result = _stageQueryProcessor.Update(mappedData);
                var stageResultViewModel = mapper.Map(result);
                return Ok(stageResultViewModel);
            }
            else
            {
                return Ok(BadRequest("Cannot update Stage"));
            }
        }

    }
}
