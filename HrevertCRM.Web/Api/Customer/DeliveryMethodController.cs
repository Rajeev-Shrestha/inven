using System;
using System.Linq;
using System.Security;
using Hrevert.Common.Constants.Customer;
using Hrevert.Common.Constants.Others;
using HrevertCRM.Data;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.QueryProcessors;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Data.Mapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using HrevertCRM.Entities;
using System.Collections.Generic;

namespace HrevertCRM.Web.Api
{
    [Route("api/[controller]")]
    public class DeliveryMethodController : Controller
    {
        private readonly IDeliveryMethodQueryProcessor _deliveryMethodQueryProcessor;
        private readonly ISecurityQueryProcessor _securityQueryProcessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<DeliveryMethodController> _logger;

        public DeliveryMethodController(IDeliveryMethodQueryProcessor deliveryMethodQueryProcessor,
            IDbContext context, ILoggerFactory factory, 
            IPagedDataRequestFactory pagedDataRequestFactory,
            ISecurityQueryProcessor securityQueryProcessor,
             UserManager<ApplicationUser> userManager)
        {
            _deliveryMethodQueryProcessor = deliveryMethodQueryProcessor;
            _securityQueryProcessor = securityQueryProcessor;
            _userManager = userManager;
            _logger = factory.CreateLogger<DeliveryMethodController>();
        }
        
        [HttpGet]
        [Route("activatedeliverymethod/{id}")]
        public ObjectResult ActivateProduct(int id)
        {
            var mapper = new DeliveryMethodToDeliveryMethodViewModelMapper();
            return Ok(mapper.Map(_deliveryMethodQueryProcessor.ActivateDeliveryMethod(id)));
        }

        [HttpGet]
        [Route("getdeliverymethod/{id}")]
        public ObjectResult Get(int id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewDeliveryMethods))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            return Ok(_deliveryMethodQueryProcessor.GetDeliveryMethod(id));
        }

        [HttpPost]
        public ObjectResult Create([FromBody] DeliveryMethodViewModel deliveryMethodViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.AddDeliveryMethod))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });

            var currentUserCompanyId = _userManager.FindByEmailAsync(User.Identity.Name).Result.CompanyId;
            deliveryMethodViewModel.CompanyId = currentUserCompanyId;
            deliveryMethodViewModel.DeliveryCode = deliveryMethodViewModel.DeliveryCode.Trim();
            deliveryMethodViewModel.Description = deliveryMethodViewModel.Description.Trim();

            var model = deliveryMethodViewModel;
            if (_deliveryMethodQueryProcessor.Exists(p => p.DeliveryCode == model.DeliveryCode && p.CompanyId == model.CompanyId))
            {
                return BadRequest(CustomerConstants.DeliveryMethodControllerConstants.DeliveryMethodAlreadyExists);
            }

            var mapper = new DeliveryMethodToDeliveryMethodViewModelMapper();
            var newDeliveryMethod = mapper.Map(deliveryMethodViewModel);
            try
            {
                var savedDeliveryMethod = _deliveryMethodQueryProcessor.Save(newDeliveryMethod);
                deliveryMethodViewModel = mapper.Map(savedDeliveryMethod);
                return Ok(deliveryMethodViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int) SecurityId.AddDeliveryMethod, ex, ex.Message);
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
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteDeliveryMethod))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            try
            {
                isDeleted = _deliveryMethodQueryProcessor.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeleteDeliveryMethod, ex, ex.Message);
            }
            return Ok(isDeleted);
        }

        [HttpPut]
        public ObjectResult Put([FromBody] DeliveryMethodViewModel deliveryMethodViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.UpdateDeliveryMethod))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });

            deliveryMethodViewModel.DeliveryCode = deliveryMethodViewModel.DeliveryCode.Trim();
            deliveryMethodViewModel.Description = deliveryMethodViewModel.Description.Trim();
            var model = deliveryMethodViewModel;
            if (_deliveryMethodQueryProcessor.Exists(p => p.DeliveryCode == model.DeliveryCode && p.Id != model.Id && p.CompanyId == model.CompanyId))
            {
                return BadRequest(CustomerConstants.DeliveryMethodControllerConstants.DeliveryMethodAlreadyExists);
            }

            var mapper = new DeliveryMethodToDeliveryMethodViewModelMapper();
            var newDeliveryMethod = mapper.Map(deliveryMethodViewModel);
            try
            {
                var savedDeliveryMethod = _deliveryMethodQueryProcessor.Update(newDeliveryMethod);
                deliveryMethodViewModel = mapper.Map(savedDeliveryMethod);
                return Ok(deliveryMethodViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.UpdateDeliveryMethod, ex, ex.Message);
                ModelState.AddModelError("", ex.Message);
                return BadRequest(new
                {
                    errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                });
            }
        }

        [HttpGet]
        [Route("CheckIfDeletedDeliveryMethodWithSameCodeExists/{code}")]
        public ObjectResult CheckIfDeletedDeliveryMethodWithSameCodeExists(string code)
        {

            var deliveryMethod = _deliveryMethodQueryProcessor.CheckIfDeletedDeliveryMethodWithSameCodeExists(code);
            var deliveryMethodMapper = new DeliveryMethodToDeliveryMethodViewModelMapper();
            if (deliveryMethod != null)
            {
                deliveryMethodMapper.Map(deliveryMethod);
            }
            return Ok(deliveryMethod);
        }

        [HttpGet]
        [Route("searchDeliveryMethods/{active}/{searchText}")]
        public ObjectResult SearchDeliveryMethods(bool active, string searchText)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewDeliveryMethods))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            try
            {
                var mapper = new DeliveryMethodToDeliveryMethodViewModelMapper();
                return Ok(_deliveryMethodQueryProcessor.SearchDeliveryMethods(active, searchText).Select(f => mapper.Map(f)).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewDeliveryMethods, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("bulkDelete")]
        public ObjectResult BulkDelete([FromBody] List<int?> deliverMethodsId)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteDeliveryMethod))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            if (deliverMethodsId == null || deliverMethodsId.Count <= 0) return Ok(false);
            try
            {
                isDeleted = _deliveryMethodQueryProcessor.DeleteRange(deliverMethodsId);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeleteDeliveryMethod, ex, ex.Message);
            }
            return Ok(isDeleted);
        }
    }
}
