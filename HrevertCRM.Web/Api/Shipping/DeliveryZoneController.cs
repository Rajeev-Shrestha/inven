using System;
using System.Linq;
using System.Security;
using Hrevert.Common.Constants.Others;
using HrevertCRM.Data;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.Mapper;
using HrevertCRM.Data.QueryProcessors;
using HrevertCRM.Data.QueryProcessors.Enumerations;
using HrevertCRM.Data.ViewModels;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Hrevert.Common.Constants;
using Microsoft.AspNetCore.Identity;
using HrevertCRM.Entities;
using System.Collections.Generic;

namespace HrevertCRM.Web.Api
{
    [Route("api/[controller]")]
    public class DeliveryZoneController : Controller
    {
        private readonly IDeliveryZoneQueryProcessor _deliveryZoneQueryProcessor;
        private readonly IPagedDataRequestFactory _pagedDataRequestFactory;
        private readonly ISecurityQueryProcessor _securityQueryProcessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<DeliveryZoneController> _logger;

        public DeliveryZoneController(IDeliveryZoneQueryProcessor deliveryZoneQueryProcessor,
            IDbContext context, IPagedDataRequestFactory pagedDataRequestFactory,
            ILoggerFactory factory,
            IEncryptionTypesQueryProcessor encryptionTypesQueryProcessor,
            ISecurityQueryProcessor securityQueryProcessor,
            IProductQueryProcessor productQueryProcessor,
            UserManager<ApplicationUser> userManager)
        {
            _deliveryZoneQueryProcessor = deliveryZoneQueryProcessor;
            _pagedDataRequestFactory = pagedDataRequestFactory;
            _securityQueryProcessor = securityQueryProcessor;
            _userManager = userManager;
            _logger = factory.CreateLogger<DeliveryZoneController>();
        }

        [HttpGet]
        [Route("getDeliveryZonebyid/{id}")]
        public ObjectResult Get(int id) //Get Includes Full DeliveryZone data
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewDeliveryZones))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            return Ok(_deliveryZoneQueryProcessor.GetDeliveryZoneViewModel(id));
        }


        [HttpGet]
        [Route("activateDeliveryZone/{id}")]
        public ObjectResult ActivateDeliveryZone(int id)
        {
            var mapper = new DeliveryZoneToDeliveryZoneViewModelMapper();
            return Ok(mapper.Map(_deliveryZoneQueryProcessor.ActivateDeliveryZone(id)));
        }

        [HttpPost]
        [Route("createzone")]
        public ObjectResult Create([FromBody] DeliveryZoneViewModel deliveryZoneViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.AddDeliveryZone))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });
            try
            {
                var currentUserCompanyId = _userManager.FindByEmailAsync(User.Identity.Name).Result.CompanyId;
                deliveryZoneViewModel.CompanyId = currentUserCompanyId;
                if (_deliveryZoneQueryProcessor.Exists(p=>p.ZoneCode == deliveryZoneViewModel.ZoneCode && p.CompanyId == deliveryZoneViewModel.CompanyId))
                {
                    return BadRequest(DeliveryZoneConstants.DeliveryZoneControllerConstants.DeliveryZoneCodeAlreadyExists);
                }


                var mapper = new DeliveryZoneToDeliveryZoneViewModelMapper();
                var mappedDeliveryZone = mapper.Map(deliveryZoneViewModel);

                var savedDeliveryZone = _deliveryZoneQueryProcessor.Save(mappedDeliveryZone);
                return Ok(savedDeliveryZone);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.AddDeliveryZone, ex, ex.Message);
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
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteDeliveryZone))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            try
            {
               isDeleted = _deliveryZoneQueryProcessor.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeleteDeliveryZone, ex, ex.Message);
            }
            return Ok(isDeleted);
        }

        [HttpPut]
        [Route("updatezone")]
        public ObjectResult Put([FromBody] DeliveryZoneViewModel deliveryZoneViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.UpdateDeliveryZone))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });

            var mapper = new DeliveryZoneToDeliveryZoneViewModelMapper();
            var newDeliveryZone = mapper.Map(deliveryZoneViewModel);
            try
            {
                if (_deliveryZoneQueryProcessor.Exists(p => p.ZoneCode == deliveryZoneViewModel.ZoneCode && p.Id!=deliveryZoneViewModel.Id && p.CompanyId == deliveryZoneViewModel.CompanyId))
                {
                    return BadRequest(DeliveryZoneConstants.DeliveryZoneControllerConstants.DeliveryZoneCodeAlreadyExists);
                }

                if (_deliveryZoneQueryProcessor.Exists(p => p.ZoneName == deliveryZoneViewModel.ZoneName && p.Id!=deliveryZoneViewModel.Id && p.CompanyId==deliveryZoneViewModel.CompanyId))
                {
                    return BadRequest(DeliveryZoneConstants.DeliveryZoneControllerConstants.DeliveryZoneNameAlreadyExists);
                }

                var updatedDeliveryZone = _deliveryZoneQueryProcessor.Update(newDeliveryZone);
                deliveryZoneViewModel = mapper.Map(updatedDeliveryZone);
                return Ok(deliveryZoneViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.UpdateDeliveryZone, ex, ex.Message);
                ModelState.AddModelError("", ex.Message);
                return BadRequest(new
                {
                    errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                });
            }
        }

        [HttpGet]
        [Route("CheckIfDeletedDeliveryZoneWithSameCodeExists/{code}")]
        public ObjectResult CheckIfDeletedDeliveryZoneWithSameCodeExists(string code)
        {
            var deliveryZone = _deliveryZoneQueryProcessor.CheckIfDeletedDeliveryZoneWithSameCodeExists(code);
            var deliveryZoneMapper = new DeliveryZoneToDeliveryZoneViewModelMapper();
            if (deliveryZone != null)
            {
                deliveryZoneMapper.Map(deliveryZone);
            }
            return Ok(deliveryZone);

        }

        [HttpGet]
        [Route("searchDeliveryZones/{active}/{searchText}")]
        public ObjectResult SearchDeliveryZones(bool active, string searchText)
            {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewDeliveryZones))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            try
            {
                var mapper = new DeliveryZoneToDeliveryZoneViewModelMapper();
                return Ok(_deliveryZoneQueryProcessor.SearchDeliveryZones(active, searchText).Select(x => mapper.Map(x)));
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewDeliveryZones, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("bulkDelete")]
        public ObjectResult BulkDelete([FromBody] List<int?> zoneSettingId)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteDeliveryZone))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            if (zoneSettingId == null || zoneSettingId.Count <= 0) return Ok(false);
            try
            {
                isDeleted = _deliveryZoneQueryProcessor.DeleteRange(zoneSettingId);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeleteDeliveryZone, ex, ex.Message);
            }
            return Ok(isDeleted);
        }
    }
}
