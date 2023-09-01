using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HrevertCRM.Data.QueryProcessors;
using HrevertCRM.Data.Common;
using Microsoft.Extensions.Logging;
using HrevertCRM.Data;
using HrevertCRM.Data.Mapper;
using HrevertCRM.Data.ViewModels;
using Hrevert.Common.Constants.Others;
using HrevertCRM.Entities;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Identity;

namespace HrevertCRM.Web.Api
{
    [Route("api/[controller]")]
    public class CompanyLogoController : Controller
    {
        private readonly ICompanyLogoQueryProcessor _companyLogoQueryProcessor;
        private readonly IPagedDataRequestFactory _pagedDataRequestFactory;
        private readonly ISecurityQueryProcessor _securityQueryProcessor;
        private readonly ILogger<CompanyLogoController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICompanyQueryProcessor _companyQueryProcessor;
        public CompanyLogoController(ICompanyLogoQueryProcessor companyLogoQueryProcessor, IPagedDataRequestFactory pagedDataRequestFactory, ILoggerFactory factory, ISecurityQueryProcessor securityQueryProcessor, UserManager<ApplicationUser> userManager,
            ICompanyQueryProcessor companyQueryProcessor)
        {
            _companyLogoQueryProcessor = companyLogoQueryProcessor;
            _pagedDataRequestFactory = pagedDataRequestFactory;
            _securityQueryProcessor = securityQueryProcessor;
            _userManager = userManager;
            _companyQueryProcessor = companyQueryProcessor;
            _logger = factory.CreateLogger<CompanyLogoController>();
        }

        [HttpGet]
        [Route("getallcompanylogos")]
        public ObjectResult GetAllCompanyLogos()
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewCompanyLogos))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            try
            {
                var mapper = new CompanyLogoToCompanyLogoViewModelMapper();
                var logos = mapper.Map(_companyLogoQueryProcessor.GetAllCompanyLogos());
                return Ok(logos);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewCompanyLogos, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getactivecompanylogos")]
        public ObjectResult GetActiveCompanyLogo()
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewCompanyLogos))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            try
            {
                var mapper = new CompanyLogoToCompanyLogoViewModelMapper();
                var logos = _companyLogoQueryProcessor.GetActiveCompanyLogos();
                var logo = mapper.Map(logos);

                return Ok(logo);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewCompanyLogos, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getactivecompanylogoinbase64")]
        public ObjectResult GetCompanyLogoInBase64()
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewCompanyLogos))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            try
            {
                var companyLogo = _companyLogoQueryProcessor.GetActiveCompanyLogo();
                if (companyLogo == null) return NotFound("Could not find logo. Please upload one first!");
                return Ok(companyLogo);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewCompanyLogos, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        [Route("getdeletedcompanylogos")]
        public ObjectResult GetDeletedCompanyLogos()
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewCompanyLogos))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            try
            {
                var mapper = new CompanyLogoToCompanyLogoViewModelMapper();
                var mappedLogos = mapper.Map(_companyLogoQueryProcessor.GetDeletedCompanyLogos());
                return Ok(mappedLogos);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewCompanyLogos, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        [Route("searchactivecompanylogos")]
        public ObjectResult SearchActive()
        {
            var requestInfo = _pagedDataRequestFactory.CreateWithSearchString(Request.GetUri());
            return Ok(_companyLogoQueryProcessor.SearchActive(requestInfo));
        }

        [HttpGet]
        [Route("searchallcompanylogos")]
        public ObjectResult SearchAll()
        {
            var requestInfo = _pagedDataRequestFactory.CreateWithSearchString(Request.GetUri());
            return Ok(_companyLogoQueryProcessor.SearchAll(requestInfo));
        }

        [HttpGet]
        [Route("getcompanylogobyid/{id}")]
        public ObjectResult Get(int id) //Get Includes Full CompanyLogo data
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewCompanyLogos))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            return Ok(_companyLogoQueryProcessor.GetCompanyLogoViewModel(id));
        }


        [HttpGet]
        [Route("activatecompanylogo/{id}")]
        public ObjectResult ActivateCompanyLogo(int id)
        {
            return Ok(_companyLogoQueryProcessor.ActivateCompanyLogo(id));
        }

        [HttpPost]
        [Route("createcompanylogo")]
        public ObjectResult Create([FromBody] CompanyLogoViewModel companyLogoViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.AddCompanyLogo))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });
            try
            {
                var mapper = new CompanyLogoToCompanyLogoViewModelMapper();
                var mappedLogo = mapper.Map(companyLogoViewModel);
                mappedLogo.Image = null;
                var currentUserCompanyId = _userManager.FindByEmailAsync(User.Identity.Name).Result.CompanyId;
                var company = _companyQueryProcessor.GetCompany(currentUserCompanyId);
                mappedLogo.CompanyId = currentUserCompanyId;
                mappedLogo.CompanyName = company.Name;
                var savedCarousel = _companyLogoQueryProcessor.Save(mappedLogo);
                SaveImage(savedCarousel.Id, companyLogoViewModel.LogoImage);
                return Ok(mapper.Map(savedCarousel));
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.AddCompanyLogo, ex, ex.Message);
                ModelState.AddModelError("", ex.Message);
                return BadRequest(new
                {
                    errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                });
            }
        }

        private void SaveImage(int savedCompanyLogoId, Image image)
        {
            _companyLogoQueryProcessor.SaveImage(savedCompanyLogoId, image);
        }

        [HttpDelete("{id}")]
        public ObjectResult Delete(int id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteCompanyLogo))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            try
            {
                isDeleted = _companyLogoQueryProcessor.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeleteCompanyLogo, ex, ex.Message);
            }
            return Ok(isDeleted);
        }

        [HttpPut]
        [Route("updatecompanylogo")]
        public ObjectResult Put([FromBody] CompanyLogoViewModel companyLogoViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.UpdateCompanyLogo))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });

            var mapper = new CompanyLogoToCompanyLogoViewModelMapper();
            var newLogo = mapper.Map(companyLogoViewModel);
            try
            {
                var updatedLogo = _companyLogoQueryProcessor.Update(newLogo);
                companyLogoViewModel = mapper.Map(updatedLogo);
                return Ok(companyLogoViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.UpdateCompanyLogo, ex, ex.Message);
                ModelState.AddModelError("", ex.Message);
                return BadRequest(new
                {
                    errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                });
            }
        }
    }
}
