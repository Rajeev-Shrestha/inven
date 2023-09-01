using System;
using System.Linq;
using Hrevert.Common.Constants.Customer;
using Hrevert.Common.Constants.Others;
using HrevertCRM.Data;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.Mapper;
using HrevertCRM.Data.QueryProcessors;
using HrevertCRM.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using HrevertCRM.Entities;
using System.Collections.Generic;

namespace HrevertCRM.Web.Api
{
    [Route("api/[controller]")]
    public class PaymentMethodController : Controller
    {
        private readonly IPaymentMethodQueryProcessor _paymentMethodQueryProcessor;
        private readonly ISecurityQueryProcessor _securityQueryProcessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<PaymentMethodController> _logger;

        public PaymentMethodController(IPaymentMethodQueryProcessor paymentMethodQueryProcessor,
            IDbContext context, ILoggerFactory factory, 
            IPagedDataRequestFactory pagedDataRequestFactory,
            ISecurityQueryProcessor securityQueryProcessor,
            UserManager<ApplicationUser> userManager)
        {
            _paymentMethodQueryProcessor = paymentMethodQueryProcessor;
            _securityQueryProcessor = securityQueryProcessor;
            _userManager = userManager;
            _logger = factory.CreateLogger<PaymentMethodController>();
        }
        
        [HttpGet]
        [Route("activatepaymentmethod/{id}")]
        public ObjectResult ActivatePaymentMethod(int id)
        {
            var mapper = new PaymentMethodToPaymentMethodViewModelMapper();
            return Ok(mapper.Map(_paymentMethodQueryProcessor.ActivatePaymentMethod(id)));
        }

        [HttpGet]
        [Route("getpaymentmethodbyid/{id}")]
        public ObjectResult Get(int id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewPaymentMethods))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            return Ok(_paymentMethodQueryProcessor.GetPaymentMethod(id));
        }

        [HttpPost]
        public ObjectResult Create([FromBody] PaymentMethodViewModel paymentMethodViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.AddPaymentMethod))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });
            var currentUserCompanyId = _userManager.FindByEmailAsync(User.Identity.Name).Result.CompanyId;
            paymentMethodViewModel.CompanyId = currentUserCompanyId;
            paymentMethodViewModel.MethodCode = paymentMethodViewModel.MethodCode.Trim();
            paymentMethodViewModel.MethodName = paymentMethodViewModel.MethodName.Trim();
            if(!string.IsNullOrEmpty(paymentMethodViewModel.ReceipentMemo))
                paymentMethodViewModel.ReceipentMemo = paymentMethodViewModel.ReceipentMemo.Trim();
                        var model = paymentMethodViewModel;

            if (_paymentMethodQueryProcessor.Exists(p => p.MethodCode == model.MethodCode &&p.CompanyId == model.CompanyId))
            {
                return BadRequest(CustomerConstants.PaymentMethodControllerConstants.PaymentMethodCodeAlreadyExists);
            }
            if (_paymentMethodQueryProcessor.Exists(p => p.MethodName == model.MethodName && p.CompanyId == model.CompanyId))
            {
                return BadRequest(CustomerConstants.PaymentMethodControllerConstants.PaymentMethodNameAlreadyExists);
            }

            var mapper = new PaymentMethodToPaymentMethodViewModelMapper();
            var newPaymentMethod = mapper.Map(paymentMethodViewModel);
            try
            {
                var savedPaymentMethod = _paymentMethodQueryProcessor.Save(newPaymentMethod);
                paymentMethodViewModel = mapper.Map(savedPaymentMethod);
                return Ok(paymentMethodViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.AddPaymentMethod, ex, ex.Message);
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
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeletePaymentMethod))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            try
            {
                isDeleted =_paymentMethodQueryProcessor.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeletePaymentMethod, ex, ex.Message);
            }
            return Ok(isDeleted);
        }

        [HttpPut]
        public ObjectResult Put([FromBody] PaymentMethodViewModel paymentMethodViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.UpdatePaymentMethod))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });

            var model = paymentMethodViewModel;
            if (_paymentMethodQueryProcessor.Exists(p => p.MethodCode == model.MethodCode && p.Id != model.Id && p.CompanyId == model.CompanyId))
            {
                return BadRequest(CustomerConstants.PaymentMethodControllerConstants.PaymentMethodCodeAlreadyExists);
            }
            if (_paymentMethodQueryProcessor.Exists(p => p.MethodName == model.MethodName && p.Id != model.Id && p.CompanyId == model.CompanyId))
            {
                return BadRequest(CustomerConstants.PaymentMethodControllerConstants.PaymentMethodNameAlreadyExists);
            }

            var mapper = new PaymentMethodToPaymentMethodViewModelMapper();
            var newPaymentMethod = mapper.Map(paymentMethodViewModel);
            try
            {
                var savedPaymentMethod = _paymentMethodQueryProcessor.Update(newPaymentMethod);
                paymentMethodViewModel = mapper.Map(savedPaymentMethod);
                return Ok(paymentMethodViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.UpdatePaymentMethod, ex, ex.Message);
                ModelState.AddModelError("", ex.Message);
                return BadRequest(new
                {
                    errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                });
            }
        }


        [HttpGet]
        [Route("CheckIfDeletedPaymentMethodWithSameCodeExists/{code}")]
        public ObjectResult CheckIfDeletedPaymentMethodWithSameCodeExists(string code)
        {
            var paymentMethod = _paymentMethodQueryProcessor.CheckIfDeletedPaymentMethodWithSameCodeExists(code);
            var paymentMethodMapper = new PaymentMethodToPaymentMethodViewModelMapper();
            if (paymentMethod != null)
            {
                paymentMethodMapper.Map(paymentMethod);
            }
            return Ok(paymentMethod);

        }

        [HttpGet]
        [Route("CheckIfDeletedPaymentMethodWithSameNameExists/{name}")]
        public ObjectResult CheckIfDeletedPaymentMethodWithSameNameExists(string name)
        {
            var paymentMethod = _paymentMethodQueryProcessor.CheckIfDeletedPaymentMethodWithSameNameExists(name);
            var paymentMethodMapper = new PaymentMethodToPaymentMethodViewModelMapper();
            if (paymentMethod != null)
            {
                paymentMethodMapper.Map(paymentMethod);
            }
            return Ok(paymentMethod);
        }

        [HttpGet]
        [Route("searchPaymentMethods/{active}/{searchText}")]
        public ObjectResult SearchPaymentMethods(bool active, string searchText)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewPaymentMethods))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            try
            {
                var mapper = new PaymentMethodToPaymentMethodViewModelMapper();
                return Ok(_paymentMethodQueryProcessor.SearchPaymentMethods(active, searchText).Select(f => mapper.Map(f)).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewPaymentMethods, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("bulkDelete")]
        public ObjectResult BulkDelete([FromBody] List<int?> paymentMethodsId)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeletePaymentMethod))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            if (paymentMethodsId == null || paymentMethodsId.Count <= 0) return Ok(false);
            try
            {
                isDeleted = _paymentMethodQueryProcessor.DeleteRange(paymentMethodsId);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeletePaymentMethod, ex, ex.Message);
            }
            return Ok(isDeleted);
        }
    }
}
