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
using HrevertCRM.Entities;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace HrevertCRM.Web.Api
{
    [Route("api/[controller]")]
    public class MeasureUnitController : Controller
    {
        private readonly IMeasureUnitQueryProcessor _measureUnitQueryProcessor;
        private readonly IPagedDataRequestFactory _pagedDataRequestFactory;
        private readonly ISecurityQueryProcessor _securityQueryProcessor;
        private readonly ILogger<MeasureUnitController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public MeasureUnitController(IMeasureUnitQueryProcessor measureUnitQueryProcessor,
            IDbContext context, IPagedDataRequestFactory pagedDataRequestFactory,
            ILoggerFactory factory,
            IEncryptionTypesQueryProcessor encryptionTypesQueryProcessor,
            ISecurityQueryProcessor securityQueryProcessor,
            IProductQueryProcessor productQueryProcessor,
            UserManager<ApplicationUser> userManager)
        {
            _measureUnitQueryProcessor = measureUnitQueryProcessor;
            _pagedDataRequestFactory = pagedDataRequestFactory;
            _securityQueryProcessor = securityQueryProcessor;
            _userManager = userManager;
            _logger = factory.CreateLogger<MeasureUnitController>();
        }

        [HttpGet]
        [Route("getMeasureUnitbyid/{id}")]
        public ObjectResult Get(int id) //Get Includes Full MeasureUnit data
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewMeasureUnits))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            return Ok(_measureUnitQueryProcessor.GetMeasureUnitViewModel(id));
        }


        [HttpGet]
        [Route("activateMeasureUnit/{id}")]
        public ObjectResult ActivateMeasureUnit(int id)
        {
            var mapper = new MeasureUnitToMeasureUnitViewModelMapper();
            return Ok(mapper.Map(_measureUnitQueryProcessor.ActivateMeasureUnit(id)));
        }

        [HttpPost]
        [Route("createmeasureunit")]
        public ObjectResult Create([FromBody] MeasureUnitViewModel measureUnitViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.AddMeasureUnit))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });
            try
            {
                var model = measureUnitViewModel;
                var currentUserCompanyId = _userManager.FindByEmailAsync(User.Identity.Name).Result.CompanyId;
                model.CompanyId = currentUserCompanyId;
                if (_measureUnitQueryProcessor.Exists(p => (p.MeasureCode == model.MeasureCode) && (p.Id != model.Id) && (p.CompanyId == model.CompanyId)))
                    return BadRequest(MeasureUnitConstants.MeasureUnitControllerConstants.MeasureUnitAlreadyExists);

                var mapper = new MeasureUnitToMeasureUnitViewModelMapper();
                var mappedMeasureUnit = mapper.Map(measureUnitViewModel);
                var savedMeasureUnit = _measureUnitQueryProcessor.Save(mappedMeasureUnit);
                return Ok(savedMeasureUnit);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.AddMeasureUnit, ex, ex.Message);
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
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteMeasureUnit))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            try
            {
                isDeleted = _measureUnitQueryProcessor.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeleteMeasureUnit, ex, ex.Message);
            }
            return Ok(isDeleted);
        }

        [HttpPut]
        [Route("updatemeasureunit")]
        public ObjectResult Put([FromBody] MeasureUnitViewModel measureUnitViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.UpdateMeasureUnit))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });

            var mapper = new MeasureUnitToMeasureUnitViewModelMapper();
            var newMeasureUnit = mapper.Map(measureUnitViewModel);
            try
            {
                var model = measureUnitViewModel;
                if (_measureUnitQueryProcessor.Exists(p => (p.MeasureCode == model.MeasureCode) && (p.Id != model.Id) && (p.CompanyId == model.CompanyId)))
                    return BadRequest(MeasureUnitConstants.MeasureUnitControllerConstants.MeasureUnitAlreadyExists);

                var updatedMeasureUnit = _measureUnitQueryProcessor.Update(newMeasureUnit);
                measureUnitViewModel = mapper.Map(updatedMeasureUnit);
                return Ok(measureUnitViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.UpdateMeasureUnit, ex, ex.Message);
                ModelState.AddModelError("", ex.Message);
                return BadRequest(new
                {
                    errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                });
            }
        }

        [HttpGet]
        [Route("getallentrymethods")]

       public ObjectResult GetAllEntryMethods()
        {
            var results = _measureUnitQueryProcessor.GetAllEntryMethodTypes();

            return Ok(results);
        }


        [HttpGet]
        [Route("CheckIfDeletedMeasureUnitWithSameCodeExists/{code}")]
        public ObjectResult CheckIfDeletedMeasureUnitWithSameCodeExists(string code)
        {
            var measureUnit = _measureUnitQueryProcessor.CheckIfDeletedMeasureUnitWithSameCodeExists(code);
            var measureUnitMapper = new MeasureUnitToMeasureUnitViewModelMapper();
            if (measureUnit != null)
            {
                measureUnitMapper.Map(measureUnit);
            }
            return Ok(measureUnit);
        }

        [HttpGet]
        [Route("CheckIfDeletedMeasureUnitWithSameNameExists/{name}")]
        public ObjectResult CheckIfDeletedMeasureUnitWithSameNameExists(string name)
        {
            var measureUnit = _measureUnitQueryProcessor.CheckIfDeletedMeasureUnitWithSameNameExists(name);
            var measureUnitMapper = new MeasureUnitToMeasureUnitViewModelMapper();
            if (measureUnit != null)
            {
                measureUnitMapper.Map(measureUnit);
            }
            return Ok(measureUnit);
        }

        [HttpGet]
        [Route("searchMeasureUnits/{active}/{searchText}")]
        public ObjectResult SearchMeasureUnits(bool active, string searchText)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewMeasureUnits))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            try
            {
                var mapper = new MeasureUnitToMeasureUnitViewModelMapper();
                return Ok(_measureUnitQueryProcessor.SearchMeasureUnits(active, searchText).Select(x => mapper.Map(x)));
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewMeasureUnits, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("bulkDelete")]
        public ObjectResult BulkDelete([FromBody] List<int?> measureUnitsId)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteMeasureUnit))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            if (measureUnitsId == null || measureUnitsId.Count <= 0) return Ok(false);
            try
            {
                isDeleted = _measureUnitQueryProcessor.DeleteRange(measureUnitsId);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeleteMeasureUnit, ex, ex.Message);
            }
            return Ok(isDeleted);
        }
    }
}
