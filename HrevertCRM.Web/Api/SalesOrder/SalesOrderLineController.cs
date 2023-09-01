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
using Microsoft.AspNetCore.Mvc;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.Extensions.Logging;


namespace HrevertCRM.Web.Api
{
    [Route("api/[controller]")]
    public class SalesOrderLineController : Controller
    {
        private readonly ISalesOrderLineQueryProcessor _salesOrderLineQueryProcessor;
        private readonly IPagedDataRequestFactory _pagedDataRequestFactory;
        private readonly IDescriptionTypesQueryProcessor _descriptionTypesQueryProcessor;
        private readonly IDiscountTypesQueryProcessor _discountTypesQueryProcessor;
        private readonly ISecurityQueryProcessor _securityQueryProcessor;
        private readonly IUserSettingQueryProcessor _userSettingQueryProcessor;
        private readonly ILogger<SalesOrderLineController> _logger;

        public SalesOrderLineController(ISalesOrderLineQueryProcessor salesOrderLineQueryProcessor,
            IDbContext context, IPagedDataRequestFactory pagedDataRequestFactory,
            ILoggerFactory factory, IDescriptionTypesQueryProcessor descriptionTypesQueryProcessor,
            IDiscountTypesQueryProcessor discountTypesQueryProcessor,
            ISecurityQueryProcessor securityQueryProcessor,
            IUserSettingQueryProcessor userSettingQueryProcessor)
        {
            _salesOrderLineQueryProcessor = salesOrderLineQueryProcessor;
            _pagedDataRequestFactory = pagedDataRequestFactory;
            _descriptionTypesQueryProcessor = descriptionTypesQueryProcessor;
            _discountTypesQueryProcessor = discountTypesQueryProcessor;
            _securityQueryProcessor = securityQueryProcessor;
            _userSettingQueryProcessor = userSettingQueryProcessor;
            _logger = factory.CreateLogger<SalesOrderLineController>();
        }
        
        
        [HttpGet]
        [Route("GetSalesOrderLine/{id}")]
        public ObjectResult Get(int id) //Get Includes Full SalesOrderLine data
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewSalesOrderLines))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            return Ok(_salesOrderLineQueryProcessor.GetSalesOrderLineViewModel(id));
        }

        [HttpGet]
        [Route("activateSalesOrderLine/{id}")]
        public ObjectResult ActivateSalesOrderLine(int id)
        {
            return Ok(_salesOrderLineQueryProcessor.ActivateSalesOrderLine(id));
        }

        [HttpPost]
        public ObjectResult Create([FromBody] SalesOrderLineViewModel salesOrderLineViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.AddSalesOrderLine))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });

            salesOrderLineViewModel.Description = salesOrderLineViewModel.Description?.Trim();

            var mapper = new SalesOrderLineToSalesOrderLineViewModelMapper();
            var newSalesOrderLine = mapper.Map(salesOrderLineViewModel);
            try
            {
                var savedSalesOrderLine = _salesOrderLineQueryProcessor.Save(newSalesOrderLine);
                salesOrderLineViewModel = mapper.Map(savedSalesOrderLine);
                return Ok(salesOrderLineViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.AddSalesOrderLine, ex, ex.Message);
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
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteSalesOrderLine))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            try
            {
                isDeleted = _salesOrderLineQueryProcessor.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeleteSalesOrderLine, ex, ex.Message);
            }
            return Ok(isDeleted);
        }

        [HttpPut]
        public ObjectResult Put([FromBody] SalesOrderLineViewModel salesOrderLineViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.UpdateSalesOrderLine))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });

            salesOrderLineViewModel.Description = salesOrderLineViewModel.Description?.Trim();

            var mapper = new SalesOrderLineToSalesOrderLineViewModelMapper();
            var newSalesOrderLine = mapper.Map(salesOrderLineViewModel);
            try
            {
                var updatedSalesOrderLine = _salesOrderLineQueryProcessor.Update(newSalesOrderLine);
                salesOrderLineViewModel = mapper.Map(updatedSalesOrderLine);
                return Ok(salesOrderLineViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.UpdateSalesOrderLine, ex, ex.Message);
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
            var pageSize = _userSettingQueryProcessor.GetPageSize(EntityId.SalesOrderLine);
            return Ok(pageSize);
        }

        [HttpGet]
        [Route("searchSalesOrderLines")]
        public ObjectResult SearchSalesOrderLines()
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewSalesOrderLines))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            try
            {
                var requestInfo = _pagedDataRequestFactory.Create(Request.GetUri());
                requestInfo.PageSize = _userSettingQueryProcessor.ForPaging(requestInfo, EntityId.SalesOrderLine);
                return Ok(_salesOrderLineQueryProcessor.SearchSalesOrderLines(requestInfo));
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewSalesOrderLines, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
