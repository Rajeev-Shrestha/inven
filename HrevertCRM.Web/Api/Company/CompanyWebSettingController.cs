using System;
using System.Linq;
using Hrevert.Common.Constants.Company;
using HrevertCRM.Data;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.Mapper;
using Microsoft.AspNetCore.Mvc;
using HrevertCRM.Data.QueryProcessors;
using HrevertCRM.Data.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using HrevertCRM.Entities;

namespace HrevertCRM.Web.Api
{
    [Route("api/[controller]")]
    public class CompanyWebSettingController : Controller
    {
        private readonly ICompanyWebSettingQueryProcessor _companywebSettingQueryProcessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<CompanyController> _logger;

        public CompanyWebSettingController(ICompanyWebSettingQueryProcessor companywebSettingQueryProcessor,
            IDbContext context, IPagedDataRequestFactory pagedDataRequestFactory,
            ILoggerFactory factory,
             UserManager<ApplicationUser> userManager)
        {
            _companywebSettingQueryProcessor = companywebSettingQueryProcessor;
            _userManager = userManager;
            _logger = factory.CreateLogger<CompanyController>();
        }

        [HttpGet]
        [Route("getcompanywebsetting/{id}")]
        public ObjectResult Get(int id)
        {
            try
            {
                var companyWebSetting = _companywebSettingQueryProcessor.GetCompanyWebSetting();
                var companyWebSettingMapper = new CompanyWebSettingToCompanyWebSettingViewModelMapper();
                var mappedWebSetting = companyWebSettingMapper.Map(companyWebSetting);
                return Ok(mappedWebSetting);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.AddCompanyWebSetting, ex, ex.Message);
                ModelState.AddModelError("", ex.Message);
                return BadRequest(new
                {
                    errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                });
            }
        }

        [HttpGet]
        [Route("getcompanywebsetting")]
        public ObjectResult GetCompanyWebSetting()
        {
            try
            {
                var companyWebSetting = _companywebSettingQueryProcessor.GetCompanyWebSetting();
                var companyWebSettingMapper = new CompanyWebSettingToCompanyWebSettingViewModelMapper();
                if (companyWebSetting != null)
                {
                    companyWebSettingMapper.Map(companyWebSetting);
                }
                return Ok(companyWebSetting);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.AddCompanyWebSetting, ex, ex.Message);
                ModelState.AddModelError("", ex.Message);
                return BadRequest(new
                {
                    errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                });
            }
        }

        [HttpGet]
        [Route("activatecompanywebsetting/{id}")]
        public ObjectResult ActivateCompanyWebSetting(int id)
        {
            try
            {
                var companyWebSetting = _companywebSettingQueryProcessor.ActivateCompanyWebSetting(id);
                var companyWebSettingMapper = new CompanyWebSettingToCompanyWebSettingViewModelMapper();
                if (companyWebSetting != null)
                {
                    companyWebSettingMapper.Map(companyWebSetting);
                }
                return Ok(companyWebSetting);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.AddCompanyWebSetting, ex, ex.Message);
                ModelState.AddModelError("", ex.Message);
                return BadRequest(new
                {
                    errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                });
            }
        }

        [HttpPost]
        [Route("createcompanywebsetting")]
        public ObjectResult Create([FromBody] CompanyWebSettingViewModel companyWebSettingViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });

            var currentUserCompanyId = _userManager.FindByEmailAsync(User.Identity.Name).Result.CompanyId;
            companyWebSettingViewModel.CompanyId = currentUserCompanyId;
            var model = companyWebSettingViewModel;
            if (_companywebSettingQueryProcessor.Exists(p => p.CompanyId == model.CompanyId))
            {
                return BadRequest(CompanyConstants.CompanyWebSettingControllerConstants.CompanyWebSettingAlreadyExists);
            }

            var mapper = new CompanyWebSettingToCompanyWebSettingViewModelMapper();
            var newCompanyWebSetting = mapper.Map(companyWebSettingViewModel);
            try
            {
                var savedCompanyWebSetting = _companywebSettingQueryProcessor.Save(newCompanyWebSetting);
                companyWebSettingViewModel = mapper.Map(savedCompanyWebSetting);
                return Ok(companyWebSettingViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.AddCompanyWebSetting, ex, ex.Message);
                ModelState.AddModelError("", ex.Message);
                return BadRequest(new
                {
                    errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                });
            }
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            try
            {
                _companywebSettingQueryProcessor.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.AddCompanyWebSetting, ex, ex.Message);
                ModelState.AddModelError("", ex.Message);
            }
        }

        [HttpPut]
        [Route("updatecompanywebsetting")]
        public ObjectResult Put([FromBody] CompanyWebSettingViewModel companyWebSettingViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });

            var model = companyWebSettingViewModel;
            if (_companywebSettingQueryProcessor.Exists(p => p.CompanyId == model.CompanyId && p.Id != model.Id))
            {
                return BadRequest(CompanyConstants.CompanyWebSettingControllerConstants.CompanyWebSettingAlreadyExists);
            }

            var mapper = new CompanyWebSettingToCompanyWebSettingViewModelMapper();
            var newCompanyWbSetting = mapper.Map(companyWebSettingViewModel);
            try
            {
                newCompanyWbSetting.IsEstoreInitialized = true;
                newCompanyWbSetting.Active = true;
                var savedCompanyWebSetting = _companywebSettingQueryProcessor.Update(newCompanyWbSetting);
                companyWebSettingViewModel = mapper.Map(savedCompanyWebSetting);
                return Ok(companyWebSettingViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.UpdateCompany, ex, ex.Message);
                ModelState.AddModelError("", ex.Message);
                return BadRequest(new
                {
                    errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                });
            }
        }

        [HttpGet]
        [Route("getdiscountcalculationtypes")]
        public ObjectResult GetDiscountCalculationType()
        {
            var result = _companywebSettingQueryProcessor.GetDiscountCalculationTypes();
            return Ok(result);
        }

        [HttpGet]
        [Route("getshippingdiscountcalculationtypes")]
        public ObjectResult GetShippingDiscountCalculationType()
        {
            var result = _companywebSettingQueryProcessor.GetShippingDiscountCalculationTypes();

            return Ok(result);
        }

        [HttpGet]
        [Route("CheckIfDeletedCompanyWebSettingExists")]
        public ObjectResult CheckIfDeletedCompanyWebSettingExists()
        {
            var companySetting = _companywebSettingQueryProcessor.CheckIfDeletedCompanyWebSettingExists();
            var companySettingMapper = new CompanyWebSettingToCompanyWebSettingViewModelMapper();
            if (companySetting != null)
            {
                companySettingMapper.Map(companySetting);
            }
            return Ok(companySetting);

        }

    }
}
