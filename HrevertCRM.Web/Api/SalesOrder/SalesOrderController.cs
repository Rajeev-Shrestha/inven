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
using Microsoft.AspNetCore.Mvc;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using NUglify.Helpers;
using System.Collections.Generic;


//using System.Web.Http;
//using Microsoft.AspNetCore.Http;

namespace HrevertCRM.Web.Api
{
    [Route("api/[controller]")]
    public class SalesOrderController : Controller
    {
        private readonly ISalesOrderQueryProcessor _salesOrderQueryProcessor;
        private readonly IDbContext _context;
        private readonly IPagedDataRequestFactory _pagedDataRequestFactory;
        private readonly ISalesOrderLineQueryProcessor _salesOrderLineQueryProcessor;
        private readonly IFiscalPeriodQueryProcessor _fiscalPeriodQueryProcessor;
        private readonly ISalesOrderTypesQueryProcessor _salesOrderTypesQueryProcessor;
        private readonly ISalesOrdersStatusQueryProcessor _salesOrdersStatusQueryProcessor;
        private readonly ISecurityQueryProcessor _securityQueryProcessor;
        private readonly IUserSettingQueryProcessor _userSettingQueryProcessor;
        private readonly IDiscountTypesQueryProcessor _discountTypesQueryProcessor;
        private readonly ILogger<SalesOrderController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        public SalesOrderController(ISalesOrderQueryProcessor salesOrderQueryProcessor,
            IDbContext context, IPagedDataRequestFactory pagedDataRequestFactory,
            ILoggerFactory factory, ISalesOrderLineQueryProcessor salesOrderLineQueryProcessor,
            IFiscalPeriodQueryProcessor fiscalPeriodQueryProcessor,
            ISalesOrderTypesQueryProcessor salesOrderTypesQueryProcessor,
            ISalesOrdersStatusQueryProcessor salesOrdersStatusQueryProcessor,
            ISecurityQueryProcessor securityQueryProcessor,
            IUserSettingQueryProcessor userSettingQueryProcessor,
            IDiscountTypesQueryProcessor discountTypesQueryProcessor,
             UserManager<ApplicationUser> userManager)
        {
            _salesOrderQueryProcessor = salesOrderQueryProcessor;
            _context = context;
            _pagedDataRequestFactory = pagedDataRequestFactory;
            _salesOrderLineQueryProcessor = salesOrderLineQueryProcessor;
            _fiscalPeriodQueryProcessor = fiscalPeriodQueryProcessor;
            _salesOrderTypesQueryProcessor = salesOrderTypesQueryProcessor;
            _salesOrdersStatusQueryProcessor = salesOrdersStatusQueryProcessor;
            _securityQueryProcessor = securityQueryProcessor;
            _userSettingQueryProcessor = userSettingQueryProcessor;
            _discountTypesQueryProcessor = discountTypesQueryProcessor;
            _userManager = userManager;
            _logger = factory.CreateLogger<SalesOrderController>();
        }
        

        [HttpGet]
        [Route("getsalesorder/{id}")]
        public ObjectResult Get(int id) //Get Includes Full SalesOrder data
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewSalesOrders))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            return Ok(_salesOrderQueryProcessor.GetSalesOrderViewModel(id));
        }

        [HttpGet]
        [Route("activatesalesorder/{id}")]
        public ObjectResult ActivateSalesOrder(int id)
        {
            var mapper = new SalesOrderToSalesOrderViewModelMapper();
            return Ok(mapper.Map(_salesOrderQueryProcessor.ActivateSalesOrder(id)));
        }

        [HttpGet]
        [Route("getdefaults/{id}")]
        public ObjectResult GetDefaultValues(int id)
        {
            var defaultValues = _salesOrderQueryProcessor.GetDefaultValues(id);
            return Ok(defaultValues);
        }

        [HttpPost]
        [Route("createneworder")]
        public ObjectResult CreateOrder([FromBody] SalesOrderViewModel salesOrderViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.AddSalesOrder))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });

            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    var orderLines = salesOrderViewModel.SalesOrderLines;
                    salesOrderViewModel.SalesOrderCode = _salesOrderQueryProcessor.GenerateSalesOrderCode();
                    salesOrderViewModel.PurchaseOrderNumber = salesOrderViewModel.PurchaseOrderNumber?.Trim();
                    salesOrderViewModel.SalesPolicy = salesOrderViewModel.SalesPolicy?.Trim();
                    salesOrderViewModel.FiscalPeriodId = _fiscalPeriodQueryProcessor.GetFiscalPeriodIdByCurrentDate();
                    if (salesOrderViewModel.FullyPaid)
                    {
                        salesOrderViewModel.FullyPaid = true;
                        salesOrderViewModel.PaidAmount = salesOrderViewModel.TotalAmount;
                    }

                    var mapper = new SalesOrderToSalesOrderViewModelMapper();
                    var newSalesOrder = mapper.Map(salesOrderViewModel);

                    newSalesOrder.SalesOrderLines = null;
                    var savedSalesOrder = _salesOrderQueryProcessor.Save(newSalesOrder);

                    if (orderLines != null && orderLines.Count > 0)
                    {
                        orderLines.ForEach(o => o.SalesOrderId = savedSalesOrder.Id);
                        orderLines.ForEach(o => o.CompanyId = savedSalesOrder.CompanyId);
                        foreach (var salesOrderLineViewModel in orderLines)
                        {
                            if (salesOrderLineViewModel == null) continue;
                            _salesOrderLineQueryProcessor.UpdateQuantityOnOrderInProduct(salesOrderLineViewModel, salesOrderViewModel.OrderType);
                            if (savedSalesOrder.OrderType != SalesOrderType.Invoice) continue;
                            salesOrderLineViewModel.Shipped = true;
                            salesOrderLineViewModel.ShippedQuantity = salesOrderLineViewModel.ItemQuantity;
                        }
                        _salesOrderLineQueryProcessor.SaveAll(orderLines);
                    }
                    trans.Commit();
                    salesOrderViewModel = mapper.Map(savedSalesOrder);
                    return Ok(salesOrderViewModel);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    _logger.LogCritical((int) SecurityId.AddSalesOrder, ex, ex.Message);
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
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteSalesOrder))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            try
            {
                isDeleted = _salesOrderQueryProcessor.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int) SecurityId.DeleteSalesOrder, ex, ex.Message);
            }
            return Ok(isDeleted);
        }

        [HttpPut]
        [Route("updateorder")]
        public ObjectResult Put([FromBody] SalesOrderViewModel salesOrderViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.UpdateSalesOrder))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });

            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    var orderLines = salesOrderViewModel.SalesOrderLines;
                    salesOrderViewModel.PurchaseOrderNumber = salesOrderViewModel.PurchaseOrderNumber?.Trim();
                    salesOrderViewModel.SalesRepId = salesOrderViewModel.SalesRepId?.Trim();
                    salesOrderViewModel.SalesPolicy = salesOrderViewModel.SalesPolicy?.Trim();
                    if (salesOrderViewModel.FullyPaid)
                    {
                        salesOrderViewModel.FullyPaid = true;
                        salesOrderViewModel.PaidAmount = salesOrderViewModel.TotalAmount;
                    }

                    var mapper = new SalesOrderToSalesOrderViewModelMapper();
                    var orderLineMapper = new SalesOrderLineToSalesOrderLineViewModelMapper();
                    var newSalesOrder = mapper.Map(salesOrderViewModel);
                    newSalesOrder.SalesOrderLines = null;
                    var updatedSalesOrder = _salesOrderQueryProcessor.Update(newSalesOrder);
                    if (orderLines.Count > 0)
                    {
                        orderLines.ForEach(x => x.DescriptionType = DescriptionType.Modified);
                        orderLines.ForEach(x => x.CompanyId = updatedSalesOrder.CompanyId);

                        foreach (var orderLine in orderLines)
                        {
                            if (orderLine.Id == 0 || orderLine.Id == null)
                            {
                                orderLine.SalesOrderId = updatedSalesOrder.Id;
                                _salesOrderLineQueryProcessor.UpdateQuantityOnOrderInProduct(orderLine, salesOrderViewModel.OrderType);
                                if (salesOrderViewModel.OrderType == SalesOrderType.Invoice)
                                {
                                    orderLine.Shipped = true;
                                    orderLine.ShippedQuantity = orderLine.ItemQuantity;
                                }
                                _salesOrderLineQueryProcessor.Save(orderLineMapper.Map(orderLine));
                            }
                            else
                            {
                                _salesOrderLineQueryProcessor.UpdateQuantityOnOrderInProduct(orderLine, salesOrderViewModel.OrderType);
                                if (salesOrderViewModel.OrderType == SalesOrderType.Invoice)
                                {
                                    orderLine.Shipped = true;
                                    orderLine.ShippedQuantity = orderLine.ItemQuantity;
                                }
                                _salesOrderLineQueryProcessor.Update(orderLineMapper.Map(orderLine));
                            }
                        }
                    }
                    trans.Commit();
                    salesOrderViewModel = mapper.Map(updatedSalesOrder);
                    return Ok(salesOrderViewModel);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    _logger.LogCritical((int)SecurityId.UpdateSalesOrder, ex, ex.Message);
                    ModelState.AddModelError("", ex.Message);
                    return BadRequest(new
                    {
                        errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                    });
                }
            }
        }

        [HttpGet]
        [Route("getsalesordertypes")]
        public ObjectResult GetSalesOrderTypes()
        {
            return Ok(_salesOrderTypesQueryProcessor.GetActiveSalesOrderTypes());
        }
        [HttpGet]
        [Route("getsalesorderstatuses")]
        public ObjectResult GetSalesOrderStatuses()
        {
            return Ok(_salesOrdersStatusQueryProcessor.GetActiveSalesOrdersStatus());
        }

        [HttpPost]
        [Route("getduedate/{termId}")]
        public ObjectResult GetDueDate(int termId, [FromBody] DateTime? date)
        {
            var result = _salesOrderQueryProcessor.GetDueDate(date, termId);
            return Ok(result);
        }

        [HttpGet]
        [Route("getdiscounttypes")]
        public ObjectResult GetDiscountTypes()
        {
            return Ok(_discountTypesQueryProcessor.GetActiveDiscountTypes());
        }

        [HttpGet]
        [Route("getSalesOrderNumbers")]
        public ObjectResult GetSalesOrderNumbers()
        {
            return Ok(_salesOrderQueryProcessor.GetSalesOrderNumbers());
        }

        [HttpGet]
        [Route("GetPageSize")]
        public ObjectResult GetPageSize()
        {
            var pageSize = _userSettingQueryProcessor.GetPageSize(EntityId.SalesOrder);
            return Ok(pageSize);
        }

        [HttpGet]
        [Route("searchSalesOrders")]
        public ObjectResult SearchSalesOrders()
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewSalesOrders))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            try
            {
                var requestInfo = _pagedDataRequestFactory.CreateWithSearchStringCompanyActive(Request.GetUri());
                requestInfo.PageSize = _userSettingQueryProcessor.ForPaging(requestInfo, EntityId.SalesOrder);
                var salesOrders = _salesOrderQueryProcessor.SearchSalesOrders(requestInfo);
                return Ok(salesOrders);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewSalesOrders, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("bulkDelete")]
        public ObjectResult BulkDelete([FromBody] List<int?> salesOrderId)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteSalesOrder))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            if (salesOrderId == null || salesOrderId.Count <= 0) return Ok(false);
            try
            {
                isDeleted = _salesOrderQueryProcessor.DeleteRange(salesOrderId);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeleteSalesOrder, ex, ex.Message);
            }
            return Ok(isDeleted);
        }
    }
}
