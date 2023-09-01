using System;
using System.Linq;
using Hrevert.Common.Constants.Others;
using HrevertCRM.Data;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.Mapper;
using HrevertCRM.Data.QueryProcessors;
using HrevertCRM.Data.QueryProcessors.Enumerations;
using HrevertCRM.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace HrevertCRM.Web.Api
{
    [Route("api/[controller]")]
    public class DeliveryRateController : Controller
    {
        private readonly IDeliveryRateQueryProcessor _deliveryRateQueryProcessor;
        private readonly IPagedDataRequestFactory _pagedDataRequestFactory;
        private readonly ISecurityQueryProcessor _securityQueryProcessor;
        private readonly ILogger<DeliveryRateController> _logger;

        public DeliveryRateController(IDeliveryRateQueryProcessor deliveryRateQueryProcessor,
            IDbContext context, IPagedDataRequestFactory pagedDataRequestFactory,
            ILoggerFactory factory,
            IEncryptionTypesQueryProcessor encryptionTypesQueryProcessor,
            ISecurityQueryProcessor securityQueryProcessor,
            IProductQueryProcessor productQueryProcessor)
        {
            _deliveryRateQueryProcessor = deliveryRateQueryProcessor;
            _pagedDataRequestFactory = pagedDataRequestFactory;
            _securityQueryProcessor = securityQueryProcessor;
            _logger = factory.CreateLogger<DeliveryRateController>();
        }

        [HttpGet]
        [Route("getDeliveryRatebyid/{id}")]
        public ObjectResult Get(int id) //Get Includes Full DeliveryRate data
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewDeliveryRates))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            return Ok(_deliveryRateQueryProcessor.GetDeliveryRateViewModel(id));
        }


        [HttpGet]
        [Route("activateDeliveryRate/{id}")]
        public ObjectResult ActivateDeliveryRate(int id)
        {
            var mapper = new DeliveryRateToDeliveryRateViewModelMapper();
            return Ok(mapper.Map(_deliveryRateQueryProcessor.ActivateDeliveryRate(id)));
        }

        [HttpPost]
        [Route("createdeliveryrate")]
        public ObjectResult Create([FromBody] DeliveryRateViewModel deliveryRateViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.AddDeliveryRate))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });
            try
            {
                var mapper = new DeliveryRateToDeliveryRateViewModelMapper(); 
                var mappedDeliveryRate = mapper.Map(deliveryRateViewModel);
                var savedDeliveryRate = _deliveryRateQueryProcessor.Save(mappedDeliveryRate);
                return Ok(savedDeliveryRate); 
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.AddDeliveryRate, ex, ex.Message);
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
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteDeliveryRate))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            try
            {
                isDeleted = _deliveryRateQueryProcessor.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeleteDeliveryRate, ex, ex.Message);
            }
            return Ok(isDeleted);
        }

        [HttpPut]
        [Route("updatedeliveryrate")]
        public ObjectResult Put([FromBody] DeliveryRateViewModel deliveryRateViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.UpdateDeliveryRate))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });

            var mapper = new DeliveryRateToDeliveryRateViewModelMapper();
            var newDeliveryRate = mapper.Map(deliveryRateViewModel);
            try
            {
                var updatedDeliveryRate = _deliveryRateQueryProcessor.Update(newDeliveryRate);
                deliveryRateViewModel = mapper.Map(updatedDeliveryRate);
                return Ok(deliveryRateViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.UpdateDeliveryRate, ex, ex.Message);
                ModelState.AddModelError("", ex.Message);
                return BadRequest(new
                {
                    errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                });
            }
        }

        [HttpGet]
        [Route("searchDeliveryRates/{active}/{searchText}")]
        public ObjectResult SearchDeliveryRates(bool active, string searchText)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewDeliveryRates))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            try
            {
                var mapper = new DeliveryRateToDeliveryRateViewModelMapper();
                return Ok(_deliveryRateQueryProcessor.SearchDeliveryRates(active, searchText).Select(x => mapper.Map(x)));
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewDeliveryRates, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("bulkDelete")]
        public ObjectResult BulkDelete([FromBody] List<int?> deliveryRateId)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteDeliveryRate))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            if (deliveryRateId == null || deliveryRateId.Count <= 0) return Ok(false);
            try
            {
                isDeleted = _deliveryRateQueryProcessor.DeleteRange(deliveryRateId);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeleteDeliveryRate, ex, ex.Message);
            }
            return Ok(isDeleted);
        }
    }
}
