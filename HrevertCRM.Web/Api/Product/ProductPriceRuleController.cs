using System;
using System.Linq;
using System.Security;
using Hrevert.Common.Constants.Others;
using HrevertCRM.Data;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.Mapper;
using HrevertCRM.Data.QueryProcessors;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HrevertCRM.Web.Api
{
    [Route("api/[controller]")]
    [Authorize]
    public class ProductPriceRuleController : Controller
    {
        private readonly IProductPriceRuleQueryProcessor _productPriceRuleQueryProcessor;
        private readonly IPagedDataRequestFactory _pagedDataRequestFactory;
        private readonly IUserSettingQueryProcessor _userSettingQueryProcessor;
        private readonly ISecurityQueryProcessor _securityQueryProcessor;
        private readonly IDbContext _context;
        private readonly ILogger<ProductPriceRuleController> _logger;


        public ProductPriceRuleController(IProductPriceRuleQueryProcessor productPriceRuleQueryProcessor,
            IDbContext context, ILoggerFactory factory,
            IPagedDataRequestFactory pagedDataRequestFactory,
            IUserSettingQueryProcessor userSettingQueryProcessor,
            IAddressQueryProcessor addressQueryProcessor,
            IHostingEnvironment env,
            ISecurityQueryProcessor securityQueryProcessor
        )
        {
            _productPriceRuleQueryProcessor = productPriceRuleQueryProcessor;
            _context = context;
            _pagedDataRequestFactory = pagedDataRequestFactory;
            _userSettingQueryProcessor = userSettingQueryProcessor;
            _securityQueryProcessor = securityQueryProcessor;
            _logger = factory.CreateLogger<ProductPriceRuleController>();
        }

        //[Authorize(Policy = "CanViewProductPriceRules")] 
        [HttpGet]
        [Route("getallProductPriceRules")]
        public ObjectResult GetAllProductPriceRules()
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewProductPriceRules))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            try
            {
                var requestInfo = _pagedDataRequestFactory.Create(Request.GetUri());
                ForPaging(requestInfo);
                return Ok(_productPriceRuleQueryProcessor.GetAllProductPriceRules(requestInfo));
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewProductPriceRules, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getactiveProductPriceRules")]
        public ObjectResult GetActiveProductPriceRules()
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewProductPriceRules))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            try
            {
                var requestInfo = _pagedDataRequestFactory.Create(Request.GetUri());
                ForPaging(requestInfo);
                return Ok(_productPriceRuleQueryProcessor.GetActiveProductPriceRules(requestInfo));
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewProductPriceRules, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        [Route("getdeletedProductPriceRules")]
        public ObjectResult GetDeletedProductPriceRules()
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewProductPriceRules))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            try
            {
                var requestInfo = _pagedDataRequestFactory.Create(Request.GetUri());
                ForPaging(requestInfo);
                return Ok(_productPriceRuleQueryProcessor.GetDeletedProductPriceRules(requestInfo));
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewProductPriceRules, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        private void ForPaging(PagedDataRequest requestInfo)
        {
            if (requestInfo.PageSize == 0)
            {
                requestInfo.PageSize = _userSettingQueryProcessor.GetPageSize(EntityId.Customer);
            }
            else
            {
                _userSettingQueryProcessor.Save(new UserSetting()
                {
                    EntityId = EntityId.Customer,
                    PageSize = requestInfo.PageSize
                });
            }
        }

        [HttpGet]
        [Route("GetProductPriceRule/{id}")]
        public ObjectResult Get(int id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewProductPriceRules))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            return Ok(_productPriceRuleQueryProcessor.GetProductPriceRuleViewModel(id));
        }

        [HttpGet]
        [Route("searchactiveProductPriceRules")]
        public ObjectResult SearchActive()
        {
            var requestInfo = _pagedDataRequestFactory.CreateWithSearchString(Request.GetUri());
            return Ok(_productPriceRuleQueryProcessor.SearchActive(requestInfo));
        }

        [HttpGet]
        [Route("searchallProductPriceRules")]
        public ObjectResult SearchAll()
        {
            var requestInfo = _pagedDataRequestFactory.CreateWithSearchString(Request.GetUri());
            return Ok(_productPriceRuleQueryProcessor.SearchAll(requestInfo));
        }

        [HttpGet]
        [Route("activateProductPriceRule/{id}")]
        public ObjectResult ActivateProductPriceRule(int id)
        {
            return Ok(_productPriceRuleQueryProcessor.ActivateProductPriceRule(id));
        }



        [HttpPost]
        [Route("createProductPriceRule")]
        public ObjectResult Create([FromBody] ProductPriceRuleViewModel productPriceRuleViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.AddProductPriceRule))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });

            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    //var model = productPriceRuleViewModel;
                    //if (_productPriceRuleQueryProcessor.Exists(p => p.CategoryId == model.CategoryId))
                    //{
                    //    return BadRequest(ProductConstants.ProductPriceRuleControllerConstants.ProductPriceRuleExists);
                    //}

                    //if (_productPriceRuleQueryProcessor.Exists(p => p.ProductId == model.ProductId))
                    //{
                    //    return BadRequest(ProductConstants.ProductPriceRuleControllerConstants.ProductPriceRuleExists);
                    //}

                    var mapper = new ProductPriceRuleToProductPriceRuleViewModelMapper();
                    var mappedProductPriceRule = mapper.Map(productPriceRuleViewModel);

                    var savedProductPriceRule = _productPriceRuleQueryProcessor.Save(mappedProductPriceRule);
                    trans.Commit();

                    productPriceRuleViewModel = mapper.Map(savedProductPriceRule);
                    return Ok(productPriceRuleViewModel);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    _logger.LogCritical((int)SecurityId.AddProductPriceRule, ex, ex.Message);
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
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteProductPriceRule))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            var isDeleted = false;
            try
            {
                isDeleted = _productPriceRuleQueryProcessor.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeleteProductPriceRule, ex, ex.Message);
            }
            return Ok(isDeleted);
        }

        [HttpPut]
        public ObjectResult Put([FromBody] ProductPriceRuleViewModel productPriceRuleViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.UpdateProductPriceRule))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var mapper = new ProductPriceRuleToProductPriceRuleViewModelMapper();
                    var newProductPriceRule = mapper.Map(productPriceRuleViewModel);
                    var updatedProductPriceRule = _productPriceRuleQueryProcessor.Update(newProductPriceRule);
                    transaction.Commit();

                    productPriceRuleViewModel = mapper.Map(updatedProductPriceRule);
                    return Ok(productPriceRuleViewModel);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _logger.LogCritical((int)SecurityId.UpdateProductPriceRule, ex, ex.Message);
                    ModelState.AddModelError("", ex.Message);
                    return BadRequest(new
                    {
                        errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                    });
                }
            }
        }
    }
}
