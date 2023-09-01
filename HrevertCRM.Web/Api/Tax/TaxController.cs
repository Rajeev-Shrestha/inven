using System;
using System.Linq;
using Hrevert.Common.Constants.Others;
using Hrevert.Common.Constants.SalesOrder;
using HrevertCRM.Data;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.Mapper;
using HrevertCRM.Data.QueryProcessors;
using HrevertCRM.Data.QueryProcessors.Enumerations;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace HrevertCRM.Web.Api
{
    [Route("api/[controller]")]
    public class TaxController : Controller
    {
        private readonly ITaxQueryProcessor _taxQueryProcessor;
        private readonly ITaxCalculationTypesQueryProcessor _taxCalculationTypesQueryProcessor;
        private readonly ISecurityQueryProcessor _securityQueryProcessor;
        private readonly ILogger<TaxController> _logger;

        public TaxController(ITaxQueryProcessor taxQueryProcessor,
            IDbContext context, IPagedDataRequestFactory pagedDataRequestFactory,
            ILoggerFactory factory,
            ITaxCalculationTypesQueryProcessor taxCalculationTypesQueryProcessor,
            ISecurityQueryProcessor securityQueryProcessor,
            IUserSettingQueryProcessor userSettingQueryProcessor,
            UserManager<ApplicationUser> userManager)
        {
            _taxQueryProcessor = taxQueryProcessor;
            _taxCalculationTypesQueryProcessor = taxCalculationTypesQueryProcessor;
            _securityQueryProcessor = securityQueryProcessor;
            _logger = factory.CreateLogger<TaxController>();
        }

        [HttpGet]
        [Route("getallactivetaxes")]
        public ObjectResult GetAllActiveTaxes()
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long) SecurityId.ViewTaxes))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            try
            {
                var mapper = new TaxToTaxViewModelMapper();
                return Ok(_taxQueryProcessor.GetAllActiveTaxes().Select(f => mapper.Map(f)).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int) SecurityId.ViewTaxes, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getTax/{id}")]
        public ObjectResult Get(int id) //Get Includes Full Tax data
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long) SecurityId.ViewTaxes))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            try
            {
                return Ok(_taxQueryProcessor.GetTaxViewModel(id));
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewTaxes, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        [Route("activateTax/{id}")]
        public ObjectResult ActivateTax(int id)
        {
            var mapper = new TaxToTaxViewModelMapper();
            return Ok(mapper.Map(_taxQueryProcessor.ActivateTax(id)));
        }

        [HttpPost]
        [Route("createTax")]
        public ObjectResult Create([FromBody] TaxViewModel taxViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.AddTax))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });
            taxViewModel.Description = taxViewModel.Description?.Trim();
            var model = taxViewModel;
            if (_taxQueryProcessor.Exists(p => p.TaxCode == model.TaxCode && p.CompanyId == model.CompanyId))
            {
                return BadRequest(SalesOrderConstants.TaxControllerConstants.TaxAlreadyExists);
            }

            var mapper = new TaxToTaxViewModelMapper();
            var newTax = mapper.Map(taxViewModel);
            try
            {
                var savedTax = _taxQueryProcessor.Save(newTax);
                taxViewModel = mapper.Map(savedTax);
                return Ok(taxViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.AddTax, ex, ex.Message);
                ModelState.AddModelError("", ex.Message);
                return BadRequest(new
                {
                    errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                });
            }
        }

        [HttpDelete("{id}")]
        public ObjectResult Delete(int id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteTax))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            try
            {
                isDeleted = _taxQueryProcessor.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeleteTax, ex, ex.Message);
            }
            return Ok(isDeleted);
        }

        [HttpPut]
        [Route("updateTax")]
        public ObjectResult Put([FromBody] TaxViewModel taxViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.UpdateTax))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });

            taxViewModel.Description = taxViewModel.Description?.Trim();
            var model = taxViewModel;
            if (_taxQueryProcessor.Exists(p => p.TaxCode == model.TaxCode && p.Id!= model.Id && p.CompanyId == model.CompanyId))
            {
                return BadRequest(SalesOrderConstants.TaxControllerConstants.TaxAlreadyExists);
            }

            var mapper = new TaxToTaxViewModelMapper();
            var newTax = mapper.Map(taxViewModel);
            try
            {
                var updatedTax = _taxQueryProcessor.Update(newTax);
                taxViewModel = mapper.Map(updatedTax);
                return Ok(taxViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.UpdateTax, ex, ex.Message);
                ModelState.AddModelError("", ex.Message);
                return BadRequest(new
                {
                    errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                });
            }
        }

        [HttpGet]
        [Route("gettaxcalculationtypes")]
        public ObjectResult GetTaxCalculationTypes()
        {
            return Ok(_taxCalculationTypesQueryProcessor.GetActiveTaxCalculationTypes());
        }

        [HttpGet]
        [Route("activetaxeswithoutpaging")]
        public ObjectResult GetActiveTaxesWithoutPaging()
        {
            var mapper = new TaxToTaxViewModelMapper();
            return Ok(_taxQueryProcessor.GetActiveTaxesWithoutPaging().Select(x => mapper.Map(x)));
        }

        [HttpGet]
        [Route("CheckIfDeletedTaxWithSameTaxCodeExists/{code}")]
        public ObjectResult CheckIfDeletedTaxWithSameTaxCodeExists(string code)
        {
            var tax = _taxQueryProcessor.CheckIfDeletedTaxWithSameTaxCodeExists(code);
            var taxMapper = new TaxToTaxViewModelMapper();
            if (tax != null)
            {
                taxMapper.Map(tax);
            }
            return Ok(tax);
        }

        [HttpGet]
        [Route("searchTaxes/{active}/{searchText}")]
        public ObjectResult SearchTaxes(bool active, string searchText)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewTaxes))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            try
            {
                var mapper = new TaxToTaxViewModelMapper();
                return Ok(_taxQueryProcessor.SearchTaxes(active, searchText).Select(f => mapper.Map(f)).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewTaxes, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("bulkDelete")]
        public ObjectResult BulkDelete([FromBody] List<int?> taxSettingId)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteTax))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            if (taxSettingId == null || taxSettingId.Count <= 0) return Ok(false);
            var isDeleted = false;
            try
            {
              
                isDeleted = _taxQueryProcessor.DeleteRange(taxSettingId);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeleteTax, ex, ex.Message);
            }
            return Ok(isDeleted);
        }
    }
}
