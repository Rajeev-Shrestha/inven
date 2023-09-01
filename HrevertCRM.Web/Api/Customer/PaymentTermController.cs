using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using Hrevert.Common.Constants.Customer;
using Hrevert.Common.Constants.Others;
using Hrevert.Common.Enums;
using HrevertCRM.Data;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.QueryProcessors;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Data.Mapper;
using HrevertCRM.Data.QueryProcessors.Enumerations;
using HrevertCRM.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
 
namespace HrevertCRM.Web.Api
{
    [Route("api/[controller]")]
    public class PaymentTermController : Controller
    {
        private readonly IPaymentTermQueryProcessor _paymentTermQueryProcessor;
        private readonly IDueDateTypesQueryProcessor _dueDateTypesQueryProcessor;
        private readonly IDueTypesQueryProcessor _dueTypesQueryProcessor;
        private readonly IPaymentDiscountTypesQueryProcessor _paymentDiscountTypesQueryProcessor;
        private readonly ITermTypesQueryProcessor _termTypesQueryProcessor;
        private readonly ISecurityQueryProcessor _securityQueryProcessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<PaymentTermController> _logger;

        public PaymentTermController(IPaymentTermQueryProcessor paymentTermQueryProcessor,
            IDbContext context, ILoggerFactory factory, 
            IPagedDataRequestFactory pagedDataRequestFactory, 
            IDueDateTypesQueryProcessor dueDateTypesQueryProcessor,
            IDueTypesQueryProcessor dueTypesQueryProcessor,
            IPaymentDiscountTypesQueryProcessor paymentDiscountTypesQueryProcessor,
            ITermTypesQueryProcessor termTypesQueryProcessor, ISecurityQueryProcessor securityQueryProcessor,
             UserManager<ApplicationUser> userManager)
        {
            _paymentTermQueryProcessor = paymentTermQueryProcessor;
            _dueDateTypesQueryProcessor = dueDateTypesQueryProcessor;
            _dueTypesQueryProcessor = dueTypesQueryProcessor;
            _paymentDiscountTypesQueryProcessor = paymentDiscountTypesQueryProcessor;
            _termTypesQueryProcessor = termTypesQueryProcessor;
            _securityQueryProcessor = securityQueryProcessor;
            _userManager = userManager;
            _logger = factory.CreateLogger<PaymentTermController>();
        }

        [HttpGet]
        [Route("activatepaymenterm/{id}")]
        public ObjectResult ActivatePaymentTerm(int id)
        {
            var mapper = new PaymentTermToPaymentTermViewModelMapper();
            return Ok(mapper.Map(_paymentTermQueryProcessor.ActivatePaymentTerm(id)));
        }

        [HttpGet]
        [Route("getpaymenttermbyid/{id}")]
        public ObjectResult Get(int id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewPaymentTerms))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            return Ok(_paymentTermQueryProcessor.GetPaymentTerm(id));
        }

        [HttpPost]
        public ObjectResult Create([FromBody] PaymentTermViewModel paymentTermViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.AddPaymentTerm))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });

            var currentUserCompanyId = _userManager.FindByEmailAsync(User.Identity.Name).Result.CompanyId;
            paymentTermViewModel.CompanyId = currentUserCompanyId;
            paymentTermViewModel.TermCode = paymentTermViewModel.TermCode.Trim();
            paymentTermViewModel.Description = paymentTermViewModel.Description?.Trim();

            var model = paymentTermViewModel;
            if (_paymentTermQueryProcessor.Exists(p => p.TermCode == model.TermCode &&p.CompanyId == model.CompanyId))
            {
                return BadRequest(CustomerConstants.PaymentTermControllerConstants.PaymentTermAlreadyExists);
            }

            var mapper = new PaymentTermToPaymentTermViewModelMapper();
            var newPaymentTerm = mapper.Map(paymentTermViewModel);
            try
            {
                var savedPaymentTerm = _paymentTermQueryProcessor.Save(newPaymentTerm);
                paymentTermViewModel = mapper.Map(savedPaymentTerm);
                return Ok(paymentTermViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.AddPaymentTerm, ex, ex.Message);
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
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeletePaymentTerm))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            try
            {
                isDeleted = _paymentTermQueryProcessor.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeletePaymentTerm, ex, ex.Message);
                //return BadRequest(ex.Message);
            }
            return Ok(isDeleted);
        }

        [HttpPut]
        public ObjectResult Put([FromBody] PaymentTermViewModel paymentTermViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.UpdatePaymentTerm))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });

            var model = paymentTermViewModel;
            if (_paymentTermQueryProcessor.Exists(p => p.TermCode == model.TermCode && p.Id != model.Id && p.CompanyId == model.CompanyId))
            {
                return BadRequest(CustomerConstants.PaymentTermControllerConstants.PaymentTermAlreadyExists);
            }

            var mapper = new PaymentTermToPaymentTermViewModelMapper();
            var newPaymentTerm = mapper.Map(paymentTermViewModel);
            try
            {
                var savedPaymentTerm = _paymentTermQueryProcessor.Update(newPaymentTerm);
                paymentTermViewModel = mapper.Map(savedPaymentTerm);
                return Ok(paymentTermViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.UpdatePaymentTerm, ex, ex.Message);
                ModelState.AddModelError("", ex.Message);
                return BadRequest(new
                {
                    errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                });
            }
        }
        [HttpGet]
        [Route("getduedate/{id}")]
        public ObjectResult GetDueDate(int id)
        {
            var dueDate = DateTime.Today;
            var payTerms = _paymentTermQueryProcessor.GetPaymentTerm(id);
            if (payTerms?.DueType == DueType.OnAccount)
            {
                if (payTerms.DueDateType == DueDateType.EndOfMonth)
                {
                    dueDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month,
                        DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));

                }
                else if (payTerms.DueDateType == DueDateType.EndofNextMonth)
                {
                    var todaysDate = DateTime.Today.AddMonths(1);
                    dueDate = new DateTime(todaysDate.Year, todaysDate.Month,
                        DateTime.DaysInMonth(todaysDate.Year, todaysDate.Month));

                }
                else if (payTerms.DueDateType == DueDateType.SpecifiedDays)
                {
                    dueDate = DateTime.Today.AddDays(payTerms.DueDateValue);
                }

            }
            return Ok(dueDate);
        }

        [HttpGet]
        [Route("getduedatetypes")]
        public ObjectResult GetDueDateTypes()
        {
            return Ok(_dueDateTypesQueryProcessor.GetActiveDueDateTypes());
        }

        [HttpGet]
        [Route("getduetypes")]
        public ObjectResult GetDueTypes()
        {
            return Ok(_dueTypesQueryProcessor.GetActiveDueTypes());
        }

        [HttpGet]
        [Route("getpaymentdiscounttypes")]
        public ObjectResult GetPaymentDiscountTypes()
        {
            return Ok(_paymentDiscountTypesQueryProcessor.GetActivePaymentDiscountTypes());
        }
        [HttpGet]
        [Route("gettermtypes")]
        public ObjectResult GetTermTypes()
        {
            return Ok(_termTypesQueryProcessor.GetActiveTermTypes());
        }

        [HttpGet]
        [Route("getOnAccountTerms")]
        public ObjectResult GetOnAccountTerms()
        {
            var result = _paymentTermQueryProcessor.GetOnAccountTerms();
            return Ok(result);
        }

        [HttpGet]
        [Route("getPayMethodsInPayTerm/{id}")]
        public ObjectResult GetPayMethodInPayTerms(int id)
        {
            var result = _paymentTermQueryProcessor.GetPayMethodsInPayTerm(id);
            return Ok(result);
        }

        [HttpPost]
        [Route("savePayMethodInPayTerms")]
        public void SavePayMethodInPayTerms([FromBody] PayMethodInPayTermViewModel payMethodsInPayTermViewModel)
        {
            DetermineMethodsToAdd(payMethodsInPayTermViewModel);
            SavePayMethodsInPayTerm(payMethodsInPayTermViewModel);
        }

        [HttpGet]
        [Route("getTermsWithoutAccountType")]
        public ObjectResult GetPaymentTermsWithoutOnAccountType()
        {
            var result = _paymentTermQueryProcessor.GetPaymentTermsWithoutOnAccountType();
            return Ok(result);
        }
        
        private void SavePayMethodsInPayTerm(PayMethodInPayTermViewModel payMethodsInPayTermViewModel)
        {

            var payMethodsInPayTermList = new List<PayMethodsInPayTerm>();
            foreach (var methodId in payMethodsInPayTermViewModel.PayMethodIds)
            {
                if (payMethodsInPayTermViewModel.PayTermId != 0)
                    payMethodsInPayTermList.Add(new PayMethodsInPayTerm
                    {
                        PayTermId = payMethodsInPayTermViewModel.PayTermId,
                        PayMethodId = methodId
                    });
            }
            _paymentTermQueryProcessor.SaveAllPayMethodsInPayTerm(payMethodsInPayTermList);
        }

        private void DetermineMethodsToAdd(PayMethodInPayTermViewModel payMethodsInPayTermViewModel)
        {
            //fetch existing paymentMethods
            if (payMethodsInPayTermViewModel.PayTermId == 0) return;
            var existingPayMethods =
                _paymentTermQueryProcessor.GetPayMethodIdsByTermId(payMethodsInPayTermViewModel.PayTermId);
            foreach (var methodId in existingPayMethods)
            {
                if (!payMethodsInPayTermViewModel.PayMethodIds.Contains(methodId))
                {
                    //remove from db
                    _paymentTermQueryProcessor.DeletePayMethodInPayTerm(payMethodsInPayTermViewModel.PayTermId, methodId);
                }
                else
                {
                    //leave as it is
                    payMethodsInPayTermViewModel.PayMethodIds.Remove(methodId);
                }
            }
        }

        [HttpGet]
        [Route("CheckIfDeletedPaymentTermWithSameCodeExists/{code}")]
        public ObjectResult CheckIfDeletedPaymentTermWithSameCodeExists(string code)
        {
            var paymentTerm = _paymentTermQueryProcessor.CheckIfDeletedPaymentTermWithSameCodeExists(code);
            var paymentTermMapper = new PaymentTermToPaymentTermViewModelMapper();
            if (paymentTerm != null)
            {
                paymentTermMapper.Map(paymentTerm);
            }
            return Ok(paymentTerm);

        }

        [HttpGet]
        [Route("CheckIfDeletedPaymentTermWithSameNameExists/{name}")]
        public ObjectResult CheckIfDeletedPaymentTermWithSameNameExists(string name)
        {
            var paymentTerm = _paymentTermQueryProcessor.CheckIfDeletedPaymentTermWithSameNameExists(name);
            var paymentTermMapper = new PaymentTermToPaymentTermViewModelMapper();
            if (paymentTerm != null)
            {
                paymentTermMapper.Map(paymentTerm);
            }
            return Ok(paymentTerm);
        }

        [HttpGet]
        [Route("searchPaymentTerms/{active}/{searchText}")]
        public ObjectResult SearchPaymentTerms(bool active, string searchText)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewPaymentTerms))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            try
            {
                var mapper = new PaymentTermToPaymentTermViewModelMapper();
                return Ok(_paymentTermQueryProcessor.SearchPaymentTerms(active, searchText).Select(f => mapper.Map(f)).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewPaymentTerms, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("bulkDelete")]
        public ObjectResult BulkDelete([FromBody] List<int?> paymentTermsId)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeletePaymentTerm))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            if (paymentTermsId == null || paymentTermsId.Count <= 0) return Ok(false);
            try
            {
                isDeleted = _paymentTermQueryProcessor.DeleteRange(paymentTermsId);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeletePaymentTerm, ex, ex.Message);
            }
            return Ok(isDeleted);
        }


    }
}
