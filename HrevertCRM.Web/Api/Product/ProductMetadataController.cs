using System;
using System.Linq;
using Hrevert.Common.Constants.Others;
using Hrevert.Common.Constants.Product;
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

namespace HrevertCRM.Web.Api
{
    [Route("api/[controller]")]
    public class ProductMetadataController : Controller
    {
        private readonly IProductMetadataQueryProcessor _productMetadataQueryProcessor;
        private readonly IPagedDataRequestFactory _pagedDataRequestFactory;
        private readonly IMediaTypesQueryProcessor _mediaTypesQueryProcessor;
        private readonly ISecurityQueryProcessor _securityQueryProcessor;
        private readonly IUserSettingQueryProcessor _userSettingQueryProcessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ProductMetadataController> _logger;

        public ProductMetadataController(IProductMetadataQueryProcessor productMetadataQueryProcessor,
            IDbContext context, IPagedDataRequestFactory pagedDataRequestFactory,
            ILoggerFactory factory, IMediaTypesQueryProcessor mediaTypesQueryProcessor,
            ISecurityQueryProcessor securityQueryProcessor,
            IUserSettingQueryProcessor userSettingQueryProcessor,
            UserManager<ApplicationUser> userManager)
        {
            _productMetadataQueryProcessor = productMetadataQueryProcessor;
            _pagedDataRequestFactory = pagedDataRequestFactory;
            _mediaTypesQueryProcessor = mediaTypesQueryProcessor;
            _securityQueryProcessor = securityQueryProcessor;
            _userSettingQueryProcessor = userSettingQueryProcessor;
            _userManager = userManager;
            _logger = factory.CreateLogger<ProductMetadataController>();
        }
        
        
        [HttpGet]
        [Route("GetProductMetadata/{id}")]
        public ObjectResult Get(int id) //Get Includes Full ProductMetadata data
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewProductMetadatas))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            var metadataMapper = new ProductMetadataToProductMetadataViewModelMapper();
            var productMetadataViewModel = metadataMapper.Map(_productMetadataQueryProcessor.GetProductMetadata(id));
            return Ok(productMetadataViewModel);
        }

        [HttpGet]
        [Route("activateProductMetadata/{id}")]
        public ObjectResult ActivateProductMetadata(int id)
        {
            return Ok(_productMetadataQueryProcessor.ActivateProductMetadata(id));
        }

        [HttpPost]
        [Route("createproductmetadata")]
        public ObjectResult Create([FromBody] ProductMetadataViewModel productMetadataViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.AddProductMetadata))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });
            var currentUserCompanyId = _userManager.FindByEmailAsync(User.Identity.Name).Result.CompanyId;
            productMetadataViewModel.CompanyId = currentUserCompanyId;
            productMetadataViewModel.MediaUrl = productMetadataViewModel.MediaUrl?.Trim();
            var model = productMetadataViewModel;
            if (_productMetadataQueryProcessor.Exists(p => p.MediaUrl == model.MediaUrl && p.CompanyId == model.CompanyId))
            {
                return BadRequest(ProductConstants.ProductMetadataControllerConstants.ProductMetadataExists);
            }

            var mapper = new ProductMetadataToProductMetadataViewModelMapper();
            var newProductMetadata = mapper.Map(productMetadataViewModel);
            try
            {
                var savedProductMetadata = _productMetadataQueryProcessor.Save(newProductMetadata);
                productMetadataViewModel = mapper.Map(savedProductMetadata);
                return Ok(productMetadataViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.AddProductMetadata, ex, ex.Message);
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
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteProductMetadata))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            try
            {
                isDeleted = _productMetadataQueryProcessor.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeleteProductMetadata, ex, ex.Message);
            }
            return Ok(isDeleted);
        }

        [HttpPut]
        [Route("updateproductmetadata")]
        public ObjectResult Put([FromBody] ProductMetadataViewModel productMetadataViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.UpdateProductMetadata))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });

            productMetadataViewModel.MediaUrl = productMetadataViewModel.MediaUrl?.Trim();

            var model = productMetadataViewModel;
            if (_productMetadataQueryProcessor.Exists(p => p.MediaUrl == model.MediaUrl && p.Id != model.Id && p.CompanyId == model.CompanyId))
            {
                return BadRequest(ProductConstants.ProductMetadataControllerConstants.ProductMetadataExists);
            }

            var mapper = new ProductMetadataToProductMetadataViewModelMapper();
            var newProductMetadata = mapper.Map(productMetadataViewModel);
            try
            {
                var updatedProductMetadata = _productMetadataQueryProcessor.Update(newProductMetadata);
                productMetadataViewModel = mapper.Map(updatedProductMetadata);
                return Ok(productMetadataViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.UpdateProductMetadata, ex, ex.Message);
                ModelState.AddModelError("", ex.Message);
                return BadRequest(new
                {
                    errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                });
            }
        }

        [HttpGet]
        [Route("getmediatypes")]
        public ObjectResult GetMediaTypes()
        {
            return Ok(_mediaTypesQueryProcessor.GetActiveMediaTypes());
        }

        [HttpGet]
        [Route("GetPageSize")]
        public ObjectResult GetPageSize()
        {
            var pageSize = _userSettingQueryProcessor.GetPageSize(EntityId.ProductMetadata);
            return Ok(pageSize);
        }

        [HttpGet]
        [Route("searchProductMetadatas")]
        public ObjectResult SearchProductMetadatas()
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewProductMetadatas))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            try
            {
                var requestInfo = _pagedDataRequestFactory.Create(Request.GetUri());
                requestInfo.PageSize = _userSettingQueryProcessor.ForPaging(requestInfo, EntityId.ProductMetadata);
                return Ok(_productMetadataQueryProcessor.SearchProductMetadatas(requestInfo));
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewProductMetadatas, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
