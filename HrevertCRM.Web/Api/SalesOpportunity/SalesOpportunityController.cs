using Hrevert.Common.Constants.Others;
using HrevertCRM.Data;
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
    public class SalesOpportunityController : Controller
    {
        private readonly ISalesOpportunityQueryProcessor _salesOpportunityQueryProcessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISecurityQueryProcessor _securityQueryProcessor;
        public SalesOpportunityController(
            ISalesOpportunityQueryProcessor salesOpportunityQueryProcessor,
            UserManager<ApplicationUser> userManager,
            ISecurityQueryProcessor securityQueryProcessor
        )
        {
            _salesOpportunityQueryProcessor = salesOpportunityQueryProcessor;
            _userManager = userManager;
            _securityQueryProcessor = securityQueryProcessor;
        }

        [HttpGet]
        [Route("getAll")]
        public ObjectResult GetAllSalesOpportunities()
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewSalesOpportunities))
                throw new System.Security.SecurityException(OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var result = _salesOpportunityQueryProcessor.GetAll();
            var mapper = new SalesOpportunityToSalesOpportunityViewModelMapper();
            var resultData = mapper.Map(result.ToList());
            return Ok(resultData);
        }

        [HttpGet]
        [Route("getAllActive")]
        public ObjectResult GetAllActiveSalesOpportunities()
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewSalesOpportunities))
                throw new System.Security.SecurityException(OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var result = _salesOpportunityQueryProcessor.GetAllActive();
            var mapper = new SalesOpportunityToSalesOpportunityViewModelMapper();
            var resultData = mapper.Map(result.ToList());
            return Ok(resultData);
        }

        [HttpPost]
        [Route("Create")]
        public ObjectResult CreateNewSaleOppotunity([FromBody] SalesOpportunityViewModel salesOpportunityViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.AddSalesOpportunity))
                throw new System.Security.SecurityException(OtherConstants.SecurityException.UserDoesNotHaveRightException);
            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });
            var currentUserCompanyId = _userManager.FindByEmailAsync(User.Identity.Name).Result.CompanyId;
            salesOpportunityViewModel.Title = salesOpportunityViewModel.Title.Trim();
            salesOpportunityViewModel.CompanyId = currentUserCompanyId;
            var mapper = new SalesOpportunityToSalesOpportunityViewModelMapper();
            var mappedData = mapper.Map(salesOpportunityViewModel);
            var result = _salesOpportunityQueryProcessor.Save(mappedData);
            salesOpportunityViewModel = mapper.Map(result);
            return Ok(salesOpportunityViewModel);
        }

        [HttpDelete("{id}")]
        public void DeleteSaleOpportunity(int id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteSalesOpportunity))
                throw new System.Security.SecurityException(OtherConstants.SecurityException.UserDoesNotHaveRightException);
            _salesOpportunityQueryProcessor.Delete(id);
        }

        [HttpPut]
        [Route("Update")]
        public ObjectResult UpdateSalesOpportunity([FromBody] SalesOpportunityViewModel salesOpportunityViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.UpdateSalesOpportunity))
                throw new System.Security.SecurityException(OtherConstants.SecurityException.UserDoesNotHaveRightException);
            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });
            var mapper = new SalesOpportunityToSalesOpportunityViewModelMapper();
            salesOpportunityViewModel.Title = salesOpportunityViewModel.Title.Trim();
            var mappedData = mapper.Map(salesOpportunityViewModel);
            var saleOpportunityById = _salesOpportunityQueryProcessor.GetSalesOpportunityById(mappedData.Id);
            if (saleOpportunityById != null)
            {
                var result = _salesOpportunityQueryProcessor.Update(mappedData);
                var saleOpportunityResultViewModel = mapper.Map(result);
                return Ok(saleOpportunityResultViewModel);
            }
            else
            {
                return Ok(BadRequest("Cannot update Sales Opportunity"));
            }
        }

        [HttpGet]
        [Route("GetSalesOppotunityByStageId/{id}")]
        public ObjectResult GetSalesOpportunityByStageId(int id)
        {
            return id == 0 ? Ok("We can't find any Sales Oppotunities with the given Stage") 
                : Ok(_salesOpportunityQueryProcessor.GetSalesOpportunityByStageId(id));
        }
    }
}
