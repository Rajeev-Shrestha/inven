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
using System.Collections.Generic;

namespace HrevertCRM.Web.Api
{
    [Route("api/[controller]")]
    public class DiscountController : Controller
    {
        private readonly IDiscountQueryProcessor _discountQueryProcessor;
        private readonly IPagedDataRequestFactory _pagedDataRequestFactory;
        private readonly IUserSettingQueryProcessor _userSettingQueryProcessor;
        private readonly IDbContext _dbContext;
        private readonly ISecurityQueryProcessor _securityQueryProcessor;
        private readonly IDiscountTypesQueryProcessor _discountTypesQueryProcessor;
        private readonly ILogger<DiscountController> _logger;

        public DiscountController(IDiscountQueryProcessor discountQueryProcessor,
            IDbContext context, IPagedDataRequestFactory pagedDataRequestFactory,
            ILoggerFactory factory, IUserSettingQueryProcessor userSettingQueryProcessor,
            IAddressQueryProcessor addressQueryProcessor, IDbContext dbContext,
            ICompanyWebSettingQueryProcessor companyWebSettingQueryProcessor,
            IUserQueryProcessor userQueryProcessor,
            ITitleTypesQueryProcessor titleTypesQueryProcessor,
            ISuffixTypesQueryProcessor suffixTypesQueryProcessor,
            ISecurityQueryProcessor securityQueryProcessor,
            IDiscountTypesQueryProcessor discountTypesQueryProcessor)
        {
            _discountQueryProcessor = discountQueryProcessor;
            _pagedDataRequestFactory = pagedDataRequestFactory;
            _userSettingQueryProcessor = userSettingQueryProcessor;
            _dbContext = dbContext;
            _securityQueryProcessor = securityQueryProcessor;
            _discountTypesQueryProcessor = discountTypesQueryProcessor;
            _logger = factory.CreateLogger<DiscountController>();
        }

        [HttpGet]
        [Route("getDiscountbyid/{id}")]
        public ObjectResult Get(int id) //Get Includes Full Discount data
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewDiscounts))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            var result = _discountQueryProcessor.GetDiscountViewModel(id);
            return Ok(result);
        }

        [HttpGet]
        [Route("activateDiscount/{id}")]
        public ObjectResult ActivateDiscount(int id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewDiscounts))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var mapper = new DiscountToDiscountViewModelMapper();
            return Ok(mapper.Map(_discountQueryProcessor.ActivateDiscount(id)));
        }

        [HttpPost]
        [Route("creatediscount")]
        public ObjectResult Create([FromBody] DiscountViewModel discountViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.AddDiscount))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });
            using (var trans = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var model = discountViewModel;

                    var productId = model.ItemId>0? model.ItemId : 0;

                    if (productId > 0)
                    {
                        var res = _discountQueryProcessor.CheckDiscountPriceWithProductUnitPrice((int)productId, (double)model.DiscountValue);

                        if (res==true)
                        {
                            return BadRequest("Unit price is less than the given discount value");
                        }


                    }
                    var mapper = new DiscountToDiscountViewModelMapper();
                    var newDiscount = mapper.Map(discountViewModel);
                    var savedDiscount = _discountQueryProcessor.Save(newDiscount);
                    trans.Commit();
                    discountViewModel = mapper.Map(savedDiscount);
                    return Ok(discountViewModel);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    _logger.LogCritical((int)SecurityId.AddDiscount, ex, ex.Message);
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
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteDiscount))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            try
            {
                isDeleted = _discountQueryProcessor.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeleteDiscount, ex, ex.Message);
            }
            return Ok(isDeleted);
        }

        [HttpPut]
        [Route("updateDiscount")]
        public ObjectResult Put([FromBody] DiscountViewModel discountViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.UpdateDiscount))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            using (var trans = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var model = discountViewModel;

                    var productId = model.ItemId > 0 ? model.ItemId : 0;

                    if (productId > 0)
                    {
                        var res = _discountQueryProcessor.CheckDiscountPriceWithProductUnitPrice((int)productId, (double)model.DiscountValue);

                        if (res == true)
                        {
                            return BadRequest("Unit price is less than the given discount value");
                        }


                    }

                    var mapper = new DiscountToDiscountViewModelMapper();
                    var newDiscount = mapper.Map(discountViewModel);
                    var updatedDiscount = _discountQueryProcessor.Update(newDiscount);
                    trans.Commit();
                    discountViewModel = mapper.Map(updatedDiscount);
                    return Ok(discountViewModel);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    _logger.LogCritical((int)SecurityId.UpdateDiscount, ex, ex.Message);
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpGet]
        [Route("activediscountwithoutpagaing")]
        public ObjectResult GetActiveDiscountsWithoutPaging()
        {
            var mapper = new DiscountToDiscountViewModelMapper();
            return Ok(_discountQueryProcessor.GetActiveDiscountsWithoutPaging().Select(x => mapper.Map(x)));
        }

        [HttpGet]
        [Route("deleteddiscountswithoutpagaing")]
        public ObjectResult GetDeletedDiscountsWithoutPaging()
        {
            var mapper = new DiscountToDiscountViewModelMapper();
            return Ok(_discountQueryProcessor.GetDeletedDiscountsWithoutPaging().Select(x => mapper.Map(x)));
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
            var pageSize = _userSettingQueryProcessor.GetPageSize(EntityId.Discount);
            return Ok(pageSize);
        }

        [HttpGet]
        [Route("getDiscounts")]
        public ObjectResult GetDiscounts()
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewDiscounts))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            try
            {
                var requestInfo = _pagedDataRequestFactory.CreateWithSearchStringCompanyActive(Request.GetUri());
                requestInfo.PageSize = _userSettingQueryProcessor.ForPaging(requestInfo, EntityId.Discount);
                var result = _discountQueryProcessor.GetDiscounts(requestInfo);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewDiscounts, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("bulkDelete")]
        public ObjectResult BulkDelete([FromBody] List<int?> discountsId)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteDiscount))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            if (discountsId == null || discountsId.Count <= 0) return Ok(false);
            try
            {
                isDeleted = _discountQueryProcessor.DeleteRange(discountsId);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeleteDiscount, ex, ex.Message);
            }
            return Ok(isDeleted);
        }

        [HttpGet]
        [Route("getDiscountsProductWise")]
        public ObjectResult GetDiscountsProductWise()
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewProducts))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            try
            {
                var result = _discountQueryProcessor.GetDiscountsProductWise();
                var mapper = new DiscountToDiscountViewModelMapper();
                return Ok(mapper.Map(result));
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewDiscounts, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getDiscountsCategoryWise")]
        public ObjectResult GetDiscountsCategoryWise()
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewProducts))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            try
            {
                var result = _discountQueryProcessor.GetDiscountsCategoryWise();
                var mapper = new DiscountToDiscountViewModelMapper();
                return Ok(mapper.Map(result));
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewDiscounts, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getDiscountOfProductForSlide/{productId}")]
        public ObjectResult GetDiscountOfProductForSlide(int productId)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewProducts))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            try
            {
                var result = _discountQueryProcessor.GetDiscountOfProductForSlide(productId);
                var mapper = new DiscountToDiscountViewModelForSlideMapper();
                return Ok(mapper.Map(result));
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewDiscounts, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("getDiscountOfCategoryForSlide/{categoryId}")]
        public ObjectResult GetDiscountOfCategoryForSlide(int categoryId)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewProducts))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            try
            {
                var result = _discountQueryProcessor.GetDiscountOfCategoryForSlide(categoryId);
                var mapper = new DiscountToDiscountViewModelForSlideMapper();
                return Ok(mapper.Map(result));
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewDiscounts, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
