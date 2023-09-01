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
using HrevertCRM.Entities;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HrevertCRM.Web.Api
{
    [Route("api/[controller]")]
    public class PurchaseOrderLineController : Controller
    {
        private readonly IPurchaseOrderLineQueryProcessor _purchaseOrderLineQueryProcessor;
        private readonly IPagedDataRequestFactory _pagedDataRequestFactory;
        private readonly IDescriptionTypesQueryProcessor _descriptionTypesQueryProcessor;
        private readonly IDiscountTypesQueryProcessor _discountTypesQueryProcessor;
        private readonly ISecurityQueryProcessor _securityQueryProcessor;
        private readonly IUserSettingQueryProcessor _userSettingQueryProcessor;
        private readonly ILogger<PurchaseOrderLineController> _logger;
        private readonly IDbContext _context;

        public PurchaseOrderLineController(IPurchaseOrderLineQueryProcessor purchaseOrderLineQueryProcessor,
            IDbContext context, IPagedDataRequestFactory pagedDataRequestFactory,
            ILoggerFactory factory, IDescriptionTypesQueryProcessor descriptionTypesQueryProcessor,
            IDiscountTypesQueryProcessor discountTypesQueryProcessor,
            ISecurityQueryProcessor securityQueryProcessor,
            IUserSettingQueryProcessor userSettingQueryProcessor)
        {
            _purchaseOrderLineQueryProcessor = purchaseOrderLineQueryProcessor;
            _context = context;
            _pagedDataRequestFactory = pagedDataRequestFactory;
            _descriptionTypesQueryProcessor = descriptionTypesQueryProcessor;
            _discountTypesQueryProcessor = discountTypesQueryProcessor;
            _securityQueryProcessor = securityQueryProcessor;
            _userSettingQueryProcessor = userSettingQueryProcessor;
            _logger = factory.CreateLogger<PurchaseOrderLineController>();
            
        }
        
        
        [HttpGet]
        [Route("GetPurchaseOrderLine/{id}")]
        public ObjectResult Get(int id) //Get Includes Full PurchaseOrderLine data
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewPurchaseOrderLines))
                throw new SecurityException(OtherConstants.SecurityException.UserDoesNotHaveRightException);

            return Ok(_purchaseOrderLineQueryProcessor.GetPurchaseOrderLineViewModel(id));
        }


        [HttpGet]
        [Route("activatePurchaseOrderLine/{id}")]
        public ObjectResult ActivatePurchaseOrderLine(int id)
        {
            return Ok(_purchaseOrderLineQueryProcessor.ActivatePurchaseOrderLine(id));
        }

        [HttpPost]
        [Route("createpurchaseorderline")]
        public ObjectResult Create([FromBody] PurchaseOrderLineViewModel purchaseOrderLineViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.AddPurchaseOrderLine))
                throw new SecurityException(OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });

            purchaseOrderLineViewModel.Description = purchaseOrderLineViewModel.Description?.Trim();

            //TODO: Ask arjun dai if we need to check if a sales order already exists
            //if (_PurchaseOrderLineQueryProcessor.Exists(p => p.PurchaseOrderLineCode == model.PurchaseOrderLineCode))
            //{
            //    return BadRequest("PurchaseOrderLine with the same email already exists!");
            //}

            var mapper = new PurchaseOrderLineToPurchaseOrderLineViewModelMapper();
            var newPurchaseOrderLine = mapper.Map(purchaseOrderLineViewModel);
            try
            {
                var savedPurchaseOrderLine = _purchaseOrderLineQueryProcessor.Save(newPurchaseOrderLine);
                purchaseOrderLineViewModel = mapper.Map(savedPurchaseOrderLine);
                return Ok(purchaseOrderLineViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.AddPurchaseOrderLine, ex, ex.Message);
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
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeletePurchaseOrderLine))
                throw new SecurityException(OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            try
            {
                isDeleted = _purchaseOrderLineQueryProcessor.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeletePurchaseOrderLine, ex, ex.Message);
            }
            return Ok(isDeleted);
        }

        [HttpPut]
        [Route("updatepurchaseorderline")]
        public ObjectResult Put([FromBody] PurchaseOrderLineViewModel purchaseOrderLineViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.UpdatePurchaseOrderLine))
                throw new SecurityException(OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if(!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });

            //if (_PurchaseOrderLineQueryProcessor.Exists(p => p.PurchaseOrderLineCode == model.PurchaseOrderLineCode && p.Id != model.Id && p.CompanyId == model.CompanyId))
            //{
            //    return BadRequest("Another PurchaseOrderLine with the same code already exists!");
            //}

            var mapper = new PurchaseOrderLineToPurchaseOrderLineViewModelMapper();
            var newPurchaseOrderLine = mapper.Map(purchaseOrderLineViewModel);
            try
            {
                var updatedPurchaseOrderLine = _purchaseOrderLineQueryProcessor.Update(newPurchaseOrderLine);
                purchaseOrderLineViewModel = mapper.Map(updatedPurchaseOrderLine);
                return Ok(purchaseOrderLineViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.UpdatePurchaseOrderLine, ex, ex.Message);
                ModelState.AddModelError("", ex.Message);
                return BadRequest(new
                {
                    errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                });
            }
        }

        [HttpGet]
        [Route("getdescriptiontypes")]
        public ObjectResult GetDescriptionTypes()
        {
            return Ok(_descriptionTypesQueryProcessor.GetActiveDescriptionTypes());
        }

        [HttpGet]
        [Route("getdiscounttypes")]
        public ObjectResult GetDiscountTypes()
        {
            return Ok(_discountTypesQueryProcessor.GetActiveDiscountTypes());
        }

        [HttpGet]
        [Route("GetPageSize")]
        public ObjectResult GetPageSize()
        {
            var pageSize = _userSettingQueryProcessor.GetPageSize(EntityId.PurchaseOrderLine);
            return Ok(pageSize);
        }

        [HttpGet]
        [Route("searchPurchaseOrderLines")]
        public ObjectResult SearchPurchaseOrderLines()
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewPurchaseOrderLines))
                throw new SecurityException(OtherConstants.SecurityException.UserDoesNotHaveRightException);
            try
            {
                var requestInfo = _pagedDataRequestFactory.Create(Request.GetUri());
                requestInfo.PageSize = _userSettingQueryProcessor.ForPaging(requestInfo, EntityId.PurchaseOrderLine);
                return Ok(_purchaseOrderLineQueryProcessor.SearchPurchaseOrderLines(requestInfo));
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewPurchaseOrderLines, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
