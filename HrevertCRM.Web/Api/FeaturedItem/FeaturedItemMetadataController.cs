using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HrevertCRM.Data.QueryProcessors;
using Microsoft.Extensions.Logging;
using HrevertCRM.Data.QueryProcessors.Enumerations;
using HrevertCRM.Data.Common;
using HrevertCRM.Data;
using HrevertCRM.Entities;
using Hrevert.Common.Constants.Others;
using HrevertCRM.Data.Mapper;
using HrevertCRM.Data.ViewModels;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Identity;

namespace HrevertCRM.Web.Api
{
    [Route("api/[controller]")]
    public class FeaturedItemMetadataController : Controller
    {
        private readonly IFeaturedItemMetadataQueryProcessor _featuredItemMetadataQueryProcessor;
        private readonly IPagedDataRequestFactory _pagedDataRequestFactory;
        private readonly IMediaTypesQueryProcessor _mediaTypesQueryProcessor;
        private readonly ISecurityQueryProcessor _securityQueryProcessor;
        private readonly IUserSettingQueryProcessor _userSettingQueryProcessor;
        private readonly ILogger<FeaturedItemMetadataController> _logger;

        public FeaturedItemMetadataController(IFeaturedItemMetadataQueryProcessor featuredItemMetadataQueryProcessor,
            IDbContext context, IPagedDataRequestFactory pagedDataRequestFactory,
            ILoggerFactory factory, IMediaTypesQueryProcessor mediaTypesQueryProcessor,
            ISecurityQueryProcessor securityQueryProcessor,
            IUserSettingQueryProcessor userSettingQueryProcessor, UserManager<ApplicationUser> userManager)
        {
            _featuredItemMetadataQueryProcessor = featuredItemMetadataQueryProcessor;
            _pagedDataRequestFactory = pagedDataRequestFactory;
            _mediaTypesQueryProcessor = mediaTypesQueryProcessor;
            _securityQueryProcessor = securityQueryProcessor;
            _userSettingQueryProcessor = userSettingQueryProcessor;
            _logger = factory.CreateLogger<FeaturedItemMetadataController>();
        }
        
        [HttpGet]
        [Route("getfeatureditemmetadata/{id}")]
        public ObjectResult Get(int id) //Get Includes Full ProductMetadata data
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewFeaturedItemMetadatas))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            var metadataMapper = new FeaturedItemMetadataToFeaturedItemMetadataViewModelMapper();
            var productMetadataViewModel = metadataMapper.Map(_featuredItemMetadataQueryProcessor.GetFeaturedItemMetadata(id));
            return Ok(productMetadataViewModel);
        }

        [HttpGet]
        [Route("activatefeatureditemmetadata/{id}")]
        public ObjectResult ActivateFeaturedItemMetadata(int id)
        {
            return Ok(_featuredItemMetadataQueryProcessor.ActivateFeaturedItemMetadata(id));
        }

        [HttpPost]
        [Route("createfeatureditemmetadata")]
        public ObjectResult Create([FromBody] FeaturedItemMetadataViewModel featuredItemMetadataViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.AddFeaturedItemMetadata))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });

            featuredItemMetadataViewModel.MediaUrl = featuredItemMetadataViewModel.MediaUrl?.Trim();

            var mapper = new FeaturedItemMetadataToFeaturedItemMetadataViewModelMapper();
            var newProductMetadata = mapper.Map(featuredItemMetadataViewModel);
            try
            {
                var savedProductMetadata = _featuredItemMetadataQueryProcessor.Save(newProductMetadata);
                featuredItemMetadataViewModel = mapper.Map(savedProductMetadata);
                return Ok(featuredItemMetadataViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.AddFeaturedItemMetadata, ex, ex.Message);
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
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteFeaturedItemMetadata))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            try
            {
                isDeleted = _featuredItemMetadataQueryProcessor.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeleteFeaturedItemMetadata, ex, ex.Message);
            }
            return Ok(isDeleted);
        }

        [HttpPut]
        [Route("updatefeatureditemmetadata")]
        public ObjectResult Put([FromBody] FeaturedItemMetadataViewModel featuredItemMetadataViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.UpdateProductMetadata))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });

            featuredItemMetadataViewModel.MediaUrl = featuredItemMetadataViewModel.MediaUrl?.Trim();

            var mapper = new FeaturedItemMetadataToFeaturedItemMetadataViewModelMapper();
            var newProductMetadata = mapper.Map(featuredItemMetadataViewModel);
            try
            {
                var updatedProductMetadata = _featuredItemMetadataQueryProcessor.Update(newProductMetadata);
                featuredItemMetadataViewModel = mapper.Map(updatedProductMetadata);
                return Ok(featuredItemMetadataViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.UpdateFeaturedItemMetadata, ex, ex.Message);
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

        [HttpPost]
        [Route("removeBannerFromFeaturedItemMetadata")]
        public void RemoveBannerImageFromFeaturedItemMetadta([FromBody] FeaturedItemRemoveMetadata featuredItemRemove)
        {
            _featuredItemMetadataQueryProcessor.RemoveBannerImageByImageUrlAndImageTypeId(featuredItemRemove.ImageTypeId, featuredItemRemove.ImageUrl);
           
        }

        [HttpGet]
        [Route("GetPageSize")]
        public ObjectResult GetPageSize()
        {
            var pageSize = _userSettingQueryProcessor.GetPageSize(EntityId.FeaturedItemMetadata);
            return Ok(pageSize);
        }

        [HttpGet]
        [Route("searchFeatureItemMetadatas")]
        public ObjectResult SearchFeatureItemMetadatas()
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewFeaturedItemMetadatas))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            try
            {
                var requestInfo = _pagedDataRequestFactory.Create(Request.GetUri());
                requestInfo.PageSize = _userSettingQueryProcessor.ForPaging(requestInfo, EntityId.FeaturedItemMetadata);
                return Ok(_featuredItemMetadataQueryProcessor.SearchFeatureItemMetadatas(requestInfo));
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewFeaturedItemMetadatas, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

    }
}