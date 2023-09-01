using System;
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

    public class GradeController : Controller
    {
        private readonly IGradeQueryProcessor _gradeQueryProcessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISecurityQueryProcessor _securityQueryProcessor;

        public GradeController(IGradeQueryProcessor gradeQueryProcessor,
            UserManager<ApplicationUser> userManager,
            IPagedDataRequestFactory pagedDataRequestFactory,
            ISecurityQueryProcessor securityQueryProcessor
        )
        {
            _gradeQueryProcessor = gradeQueryProcessor;
            _userManager = userManager;
            _securityQueryProcessor = securityQueryProcessor;
        }

        [HttpGet]
        [Route("getAllGrades")]
        public ObjectResult GetAllGrades()
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long) SecurityId.ViewGrades))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var result = _gradeQueryProcessor.GetAll();
            var mapper = new GradeToGradeViewModelMapper();
            var resultData = mapper.Map(result.ToList());
            return Ok(resultData);
        }

        [HttpGet]
        [Route("getAllActiveGrade")]
        public ObjectResult GetAllActiveGrade()
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long) SecurityId.ViewGrades))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var result = _gradeQueryProcessor.GetAllActive();
            var mapper = new GradeToGradeViewModelMapper();
            var resultData = mapper.Map(result.ToList());
            return Ok(resultData);
        }

        [HttpPost]
        [Route("Create")]
        public ObjectResult CreateNewGrade([FromBody] GradeViewModel gradeViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long) SecurityId.AddGrade))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                });
            var currentUserCompanyId = _userManager.FindByEmailAsync(User.Identity.Name).Result.CompanyId;
            gradeViewModel.CompanyId = currentUserCompanyId;
            gradeViewModel.GradeName = gradeViewModel.GradeName.Trim();
            var mapper = new GradeToGradeViewModelMapper();
            var mappedData = mapper.Map(gradeViewModel);
            var result = _gradeQueryProcessor.Save(mappedData);
            gradeViewModel = mapper.Map(result);
            return Ok(gradeViewModel);
        }

        [HttpDelete("{id}")]
        public ObjectResult DeleteGrade(int id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long) SecurityId.DeleteGrade))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            try
            {
                isDeleted = _gradeQueryProcessor.Delete(id);
            }
            catch (Exception)
            {
                
                throw;
            }
            return Ok(isDeleted);
        }
        [HttpPut]
        [Route("Update")]
        public ObjectResult UpdateGrade([FromBody] GradeViewModel gradeViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.UpdateGrade))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });
            gradeViewModel.GradeName = gradeViewModel.GradeName.Trim();            
            var mapper = new GradeToGradeViewModelMapper();
            var mappedData = mapper.Map(gradeViewModel);
            var gradeById = _gradeQueryProcessor.GetGradeById(mappedData.Id);
            if (gradeById != null)
            {
                var result = _gradeQueryProcessor.Update(mappedData);
                var gradeResultViewModel = mapper.Map(result);
                return Ok(gradeResultViewModel);
            }
            else
            {
                return Ok(BadRequest("Cannot update Grade"));
            }

        }

    }
}
