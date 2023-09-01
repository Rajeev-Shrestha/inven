using System;
using System.Linq;
using Hrevert.Common.Constants.Others;
using Hrevert.Common.Enums;
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
using Microsoft.AspNetCore.Identity;
using NUglify.Helpers;
using System.Collections.Generic;


namespace HrevertCRM.Web.Api
{
    [Route("api/[controller]")]
    public class PurchaseOrderController : Controller
    {
        private readonly IPurchaseOrderQueryProcessor _purchaseOrderQueryProcessor;
        private readonly IDbContext _context;
        private readonly IPagedDataRequestFactory _pagedDataRequestFactory;
        private readonly IPurchaseOrderLineQueryProcessor _purchaseOrderLineQueryProcessor;
        private readonly IFiscalPeriodQueryProcessor _fiscalPeriodQueryProcessor;
        private readonly IPurchaseOrderTypesQueryProcessor _purchaseOrderTypesQueryProcessor;
        private readonly IPurchaseOrdersStatusQueryProcessor _purchaseOrdersStatusQueryProcessor;
        private readonly ISecurityQueryProcessor _securityQueryProcessor;
        private readonly IUserSettingQueryProcessor _userSettingQueryProcessor;
        private readonly IDiscountTypesQueryProcessor _discountTypesQueryProcessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<PurchaseOrderController> _logger;

        public PurchaseOrderController(IPurchaseOrderQueryProcessor purchaseOrderQueryProcessor,
            IDbContext context, IPagedDataRequestFactory pagedDataRequestFactory,
            ILoggerFactory factory, IPurchaseOrderLineQueryProcessor purchaseOrderLineQueryProcessor,
            IFiscalPeriodQueryProcessor fiscalPeriodQueryProcessor,
            IPurchaseOrderTypesQueryProcessor purchaseOrderTypesQueryProcessor,
            IPurchaseOrdersStatusQueryProcessor purchaseOrdersStatusQueryProcessor,
            ISecurityQueryProcessor securityQueryProcessor,
            IUserSettingQueryProcessor userSettingQueryProcessor,
            IDiscountTypesQueryProcessor discountTypesQueryProcessor,
             UserManager<ApplicationUser> userManager)
        {
            _purchaseOrderQueryProcessor = purchaseOrderQueryProcessor;
            _context = context;
            _pagedDataRequestFactory = pagedDataRequestFactory;
            _purchaseOrderLineQueryProcessor = purchaseOrderLineQueryProcessor;
            _fiscalPeriodQueryProcessor = fiscalPeriodQueryProcessor;
            _purchaseOrderTypesQueryProcessor = purchaseOrderTypesQueryProcessor;
            _purchaseOrdersStatusQueryProcessor = purchaseOrdersStatusQueryProcessor;
            _securityQueryProcessor = securityQueryProcessor;
            _userSettingQueryProcessor = userSettingQueryProcessor;
            _discountTypesQueryProcessor = discountTypesQueryProcessor;
            _userManager = userManager;
            _logger = factory.CreateLogger<PurchaseOrderController>();
        }
        
        [HttpGet]
        [Route("getpurchaseorder/{id}")]
        public ObjectResult Get(int id) //Get Includes Full PurchaseOrder data
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewPurchaseOrders))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            var result = _purchaseOrderQueryProcessor.GetPurchaseOrderViewModel(id);
            return Ok(result);
        }

        [HttpGet]
        [Route("activatpurchaseorder/{id}")]
        public ObjectResult ActivatePurchaseOrder(int id)
        {
            var mapper = new PurchaseOrderToPurchaseOrderViewModelMapper();
            return Ok(mapper.Map(_purchaseOrderQueryProcessor.ActivatePurchaseOrder(id)));
        }

        [HttpGet]
        [Route("getdefaults/{id}")]
        public ObjectResult GetDefaultValues(int id)
        {
            var defaultValues = _purchaseOrderQueryProcessor.GetDefaultValues(id);
            return Ok(defaultValues);
        }

        [HttpPost]
        [Route("createneworder")]
        public ObjectResult CreateOrder([FromBody] PurchaseOrderViewModel purchaseOrderViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.AddPurchaseOrder))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });

            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    var orderLines = purchaseOrderViewModel.PurchaseOrderLines;
                    var currentUserCompanyId = _userManager.FindByEmailAsync(User.Identity.Name).Result.CompanyId;
                    purchaseOrderViewModel.CompanyId = currentUserCompanyId;
                    purchaseOrderViewModel.PurchaseOrderCode = _purchaseOrderQueryProcessor.GeneratePurchaseOrderCode();
                    purchaseOrderViewModel.SalesOrderNumber = purchaseOrderViewModel.SalesOrderNumber?.Trim();
                    purchaseOrderViewModel.PurchaseRepId = _userManager.FindByEmailAsync(User.Identity.Name).Result.Id;
                    purchaseOrderViewModel.OrderDate = DateTime.Now;
                    purchaseOrderViewModel.FiscalPeriodId = _fiscalPeriodQueryProcessor.GetFiscalPeriodIdByCurrentDate();
                    if (purchaseOrderViewModel.FullyPaid)
                    {
                        purchaseOrderViewModel.FullyPaid = true;
                        purchaseOrderViewModel.PaidAmount = purchaseOrderViewModel.TotalAmount;
                    }

                    var mapper = new PurchaseOrderToPurchaseOrderViewModelMapper();
                    var newPurchaseOrder = mapper.Map(purchaseOrderViewModel);

                    newPurchaseOrder.PurchaseOrderLines = null;
                    var savedPurchaseOrder = _purchaseOrderQueryProcessor.Save(newPurchaseOrder);
                    if (orderLines != null && orderLines.Count > 0 )
                    {
                        orderLines.ForEach(o => o.PurchaseOrderId = savedPurchaseOrder.Id);
                        orderLines.ForEach(o => o.CompanyId = savedPurchaseOrder.CompanyId);
                        foreach (var purchaseOrderLineViewModel in orderLines)
                        {
                            if (purchaseOrderLineViewModel == null) continue;
                            _purchaseOrderLineQueryProcessor.UpdateQuantityOnOrderInProduct(purchaseOrderLineViewModel, purchaseOrderViewModel.OrderType);
                            if (savedPurchaseOrder.OrderType != PurchaseOrderType.Invoice) continue;
                            purchaseOrderLineViewModel.Shipped = true;
                            purchaseOrderLineViewModel.ShippedQuantity = purchaseOrderLineViewModel.ItemQuantity;
                        }   
                        _purchaseOrderLineQueryProcessor.SaveAllViewModels(orderLines);
                    }
                    trans.Commit();
                    purchaseOrderViewModel = mapper.Map(savedPurchaseOrder);
                    return Ok(purchaseOrderViewModel);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    _logger.LogCritical((int) SecurityId.AddPurchaseOrder, ex, ex.Message);
                    ModelState.AddModelError("", ex.Message);
                    return BadRequest(new
                    {
                        errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                    });
                }
            }
        }

        [HttpDelete("{id}")]
        public ObjectResult Delete(int id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeletePurchaseOrder))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            try
            {
                isDeleted = _purchaseOrderQueryProcessor.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeletePurchaseOrder, ex, ex.Message);
            }
            return Ok(isDeleted);
        }

        [HttpPut]
        [Route("updatepurchaseorder")]
        public ObjectResult Put([FromBody] PurchaseOrderViewModel purchaseOrderViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.UpdatePurchaseOrder))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });

            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    var orderLines = purchaseOrderViewModel.PurchaseOrderLines;
                    purchaseOrderViewModel.SalesOrderNumber = purchaseOrderViewModel.SalesOrderNumber?.Trim();
                    purchaseOrderViewModel.PurchaseRepId = purchaseOrderViewModel.PurchaseRepId?.Trim();
                    if (purchaseOrderViewModel.FullyPaid)
                    {
                        purchaseOrderViewModel.FullyPaid = true;
                        purchaseOrderViewModel.PaidAmount = purchaseOrderViewModel.TotalAmount;
                    }
                    var mapper = new PurchaseOrderToPurchaseOrderViewModelMapper();
                    var orderLineMapper = new PurchaseOrderLineToPurchaseOrderLineViewModelMapper();
                    var newPurchaseOrder = mapper.Map(purchaseOrderViewModel);
                    newPurchaseOrder.PurchaseOrderLines = null;
                    var updatedPurchaseOrder = _purchaseOrderQueryProcessor.Update(newPurchaseOrder);
                    if (orderLines.Count > 0)
                    {
                        orderLines.ForEach(x => x.CompanyId = updatedPurchaseOrder.CompanyId);

                        foreach (var orderLine in orderLines)
                        {
                            if (orderLine.Id == 0 || orderLine.Id == null)
                            {
                                orderLine.PurchaseOrderId = updatedPurchaseOrder.Id;
                                _purchaseOrderLineQueryProcessor.UpdateQuantityOnOrderInProduct(orderLine, purchaseOrderViewModel.OrderType);
                                if (purchaseOrderViewModel.OrderType == PurchaseOrderType.Invoice)
                                {
                                    orderLine.Shipped = true;
                                    orderLine.ShippedQuantity = orderLine.ItemQuantity;
                                }
                                _purchaseOrderLineQueryProcessor.Save(orderLineMapper.Map(orderLine));
                            }
                            else
                            {
                                _purchaseOrderLineQueryProcessor.UpdateQuantityOnOrderInProduct(orderLine, purchaseOrderViewModel.OrderType);
                                if (purchaseOrderViewModel.OrderType == PurchaseOrderType.Invoice)
                                {
                                    orderLine.Shipped = true;
                                    orderLine.ShippedQuantity = orderLine.ItemQuantity;
                                }
                                _purchaseOrderLineQueryProcessor.Update(orderLineMapper.Map(orderLine));
                            }
                        }
                    }
                    trans.Commit();
                    purchaseOrderViewModel = mapper.Map(updatedPurchaseOrder);
                    return Ok(purchaseOrderViewModel);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    _logger.LogCritical((int)SecurityId.UpdatePurchaseOrder, ex, ex.Message);
                    ModelState.AddModelError("", ex.Message);
                    return BadRequest(new
                    {
                        errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                    });
                }
            }   
        }

        [HttpGet]
        [Route("getpurchaseordertypes")]
        public ObjectResult GetPurchaseOrderTypes()
        {
            return Ok(_purchaseOrderTypesQueryProcessor.GetActivePurchaseOrderTypes());
        }

        [HttpGet]
        [Route("getpurchaseorderstatuses")]
        public ObjectResult GetPurchaseOrderStatuses()
        {
            return Ok(_purchaseOrdersStatusQueryProcessor.GetActivePurchaseOrdersStatus());
        }

        [HttpGet]
        [Route("getdiscounttypes")]
        public ObjectResult GetDiscountTypes()
        {
            return Ok(_discountTypesQueryProcessor.GetActiveDiscountTypes());
        }

        [HttpGet]
        [Route("getPurchaseOrderNumbers")]
        public ObjectResult GetSalesOrderNumbers()
        {
            return Ok(_purchaseOrderQueryProcessor.GetPurchaseOrderNumbers());
        }

        [HttpPost]
        [Route("GetDueDate")]
        public ObjectResult GetDueDate([FromBody] DueDateViewModel dueDateViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });
            return Ok(_purchaseOrderQueryProcessor.GetDueDate(dueDateViewModel));
        }

        [HttpGet]
        [Route("GetPageSize")]
        public ObjectResult GetPageSize()
        {
            var pageSize = _userSettingQueryProcessor.GetPageSize(EntityId.PurchaseOrder);
            return Ok(pageSize);
        }

        [HttpGet]
        [Route("searchPurchaseOrders")]
        public ObjectResult SearchPurchaseOrders()
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewPurchaseOrders))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            try
            {
                var requestInfo = _pagedDataRequestFactory.CreateWithSearchStringCompanyActive(Request.GetUri());
                requestInfo.PageSize = _userSettingQueryProcessor.ForPaging(requestInfo, EntityId.PurchaseOrder);
                return Ok(_purchaseOrderQueryProcessor.SearchPurchaseOrders(requestInfo));
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewPurchaseOrders, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("bulkDelete")]
        public ObjectResult BulkDelete([FromBody] List<int?> purchaseOrderId)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeletePurchaseOrder))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            if (purchaseOrderId == null || purchaseOrderId.Count <= 0) return Ok(false);
            try
            {
                isDeleted = _purchaseOrderQueryProcessor.DeleteRange(purchaseOrderId);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeletePurchaseOrder, ex, ex.Message);
            }
            return Ok(isDeleted);
        }
    }
}