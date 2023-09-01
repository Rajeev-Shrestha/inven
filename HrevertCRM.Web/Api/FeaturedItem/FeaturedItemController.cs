using Hrevert.Common.Constants.Others;
using HrevertCRM.Data;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.Mapper;
using HrevertCRM.Data.QueryProcessors;
using HrevertCRM.Data.QueryProcessors.Enumerations;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using HrevertCRM.Data.Mapper.Enumerations;
using Microsoft.AspNetCore.Identity;
using Hrevert.Common.Constants.FeaturedItem;

namespace HrevertCRM.Web.Api
{
    [Route("api/[controller]")]
    public class FeaturedItemController : Controller
    {
        private readonly IProductCategoryQueryProcessor _productCategoryQueryProcessor;
        private readonly IFeaturedItemQueryProcessor _featuredItemQueryProcessor;
        private readonly IFeaturedItemMetadataQueryProcessor _featuredItemMetadataQueryProcessor;
        private readonly IPagedDataRequestFactory _pagedDataRequestFactory;
        private readonly IUserSettingQueryProcessor _userSettingQueryProcessor;
        private readonly ISecurityQueryProcessor _securityQueryProcessor;
        private readonly IProductQueryProcessor _productQueryProcessor;
        private readonly IDbContext _context;
        private readonly IImageTypeQueryProcessor _imageTypeQueryProcessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<FeaturedItemController> _logger;


        public FeaturedItemController(IFeaturedItemQueryProcessor featuredItemQueryProcessor,
           IFeaturedItemMetadataQueryProcessor featuredItemMetadataQueryProcessor,
           IPagedDataRequestFactory pagedDataRequestFactory,
           IUserSettingQueryProcessor userSettingQueryProcessor,
           IDbContext context, IProductQueryProcessor productQueryProcessor,
           IProductCategoryQueryProcessor productCategoryQueryProcessor,
           ILoggerFactory factory,
           IHostingEnvironment env,
           IImageTypeQueryProcessor imageTypeQueryProcessor,
           ISecurityQueryProcessor securityQueryProcessor,
           UserManager<ApplicationUser> userManager)
        {
            _featuredItemQueryProcessor = featuredItemQueryProcessor;
            _featuredItemMetadataQueryProcessor = featuredItemMetadataQueryProcessor;
            _context = context;
            _pagedDataRequestFactory = pagedDataRequestFactory;
            _userSettingQueryProcessor = userSettingQueryProcessor;
            _securityQueryProcessor = securityQueryProcessor;
            _productQueryProcessor = productQueryProcessor;
            _productCategoryQueryProcessor = productCategoryQueryProcessor;
            _imageTypeQueryProcessor = imageTypeQueryProcessor;
            _userManager = userManager;
            _logger = factory.CreateLogger<FeaturedItemController>();
        }


        [HttpGet]
        [Route("getallactivefeatureditems")]
        public ObjectResult GetAllActiveFeaturedItems()
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewFeaturedItems))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            try
            {
                var mapper = new FeaturedItemToFeaturedItemViewModelMapper();
                var featuredItemViewModelList = new List<FeaturedItemViewModel>();
                var featuredItems = _featuredItemQueryProcessor.GetAllActiveFeaturedItems();
                foreach (var item in featuredItems)
                {
                    var productCategory = _productCategoryQueryProcessor.GetProductCategory(item.ItemId);
                    var featuredItemViewModel = mapper.Map(item);
                    featuredItemViewModel.ItemName = productCategory.Name;
                    featuredItemViewModelList.Add(featuredItemViewModel);
                }
                return Ok(featuredItemViewModelList);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewFeaturedItems, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getfeatureditem/{id}")]
        public ObjectResult Get(int id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewFeaturedItems))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            var result = _featuredItemQueryProcessor.GetFeaturedItemViewModel(id);

            if (result.ItemId != 0)
            {
                var product = _productCategoryQueryProcessor.GetProductCategory(result.ItemId);
                result.ItemName = product.Name;
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("searchactivefeatureditems")]
        public ObjectResult SearchActive()
        {
            var requestInfo = _pagedDataRequestFactory.CreateWithSearchString(Request.GetUri());
            var result = _featuredItemQueryProcessor.SearchActive(requestInfo);
            return Ok(result);
        }

        [HttpGet]
        [Route("searchallfeatureditems")]
        public ObjectResult SearchAll()
        {
            var requestInfo = _pagedDataRequestFactory.CreateWithSearchString(Request.GetUri());

            var result = _featuredItemQueryProcessor.SearchAll(requestInfo);
            return Ok(result);
        }

        [HttpGet]
        [Route("activatefeatureditem/{id}")]
        public ObjectResult ActivateProduct(int id)
        {
            var mapper =new FeaturedItemToFeaturedItemViewModelMapper();
            return Ok(mapper.Map(_featuredItemQueryProcessor.ActivateFeaturedItem(id)));
        }

        [HttpPost]
        public ObjectResult Create([FromBody] FeaturedItemViewModel featuredItemViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.AddFeaturedItem))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });

            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    var currentUserCompanyId = _userManager.FindByEmailAsync(User.Identity.Name).Result.CompanyId;
                    featuredItemViewModel.CompanyId = currentUserCompanyId;
                    var model = featuredItemViewModel;

                    if (_featuredItemQueryProcessor.Exists(p => p.ImageType == model.ImageType && p.ItemId == model.ItemId && p.CompanyId == model.CompanyId))
                    {
                        return BadRequest(FeaturedItemConstants.FeaturedItemControllerConstants.FeaturedItemAlreadyExists);
                    }

                    var mapper = new FeaturedItemToFeaturedItemViewModelMapper();
                    var mappedProduct = mapper.Map(featuredItemViewModel);

                    var savedProduct = _featuredItemQueryProcessor.Save(mappedProduct);
                    SaveFeaturedItemMetadatas(savedProduct.Id, savedProduct.ItemId, featuredItemViewModel.BannerImage);
                    trans.Commit();

                    return Ok(featuredItemViewModel);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    _logger.LogCritical((int)SecurityId.AddFeaturedItem, ex, ex.Message);
                    ModelState.AddModelError("", ex.Message);
                    return BadRequest(new
                    {
                        errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                    });
                }
            }
        }

        private string SaveFeaturedItemMetadatas(int featureItemId, int itemId, List<Image> images)
        {
            return _featuredItemMetadataQueryProcessor.SaveFeaturedItemMetadatas(featureItemId, itemId, images);
        }


        [HttpDelete("{id}")]
        public ObjectResult Delete(int id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteFeaturedItem))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            try
            {
                isDeleted = _featuredItemQueryProcessor.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeleteProduct, ex, ex.Message);
            }
            return Ok(isDeleted);
        }

        [HttpPut]
        public ObjectResult Put([FromBody] FeaturedItemViewModel featuredItemViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.UpdateFeaturedItem))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var model = featuredItemViewModel;

                    if (_featuredItemQueryProcessor.Exists(p => p.ImageType == model.ImageType && p.Id != model.Id && p.CompanyId == model.CompanyId))
                    {
                        return BadRequest(FeaturedItemConstants.FeaturedItemControllerConstants.FeaturedItemAlreadyExists);
                    }

                    var mapper = new FeaturedItemToFeaturedItemViewModelMapper();
                    var newProduct = mapper.Map(featuredItemViewModel);
                    var updatedProduct = _featuredItemQueryProcessor.Update(newProduct);
                    SaveFeaturedItemMetadatas(updatedProduct.Id, updatedProduct.ItemId, featuredItemViewModel.BannerImage);
                    transaction.Commit();

                    featuredItemViewModel = mapper.Map(updatedProduct);
                    return Ok(featuredItemViewModel);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _logger.LogCritical((int)SecurityId.UpdateFeaturedItem, ex, ex.Message);
                    ModelState.AddModelError("", ex.Message);
                    return BadRequest(new
                    {
                        errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                    });
                }
            }
        }


        [HttpGet]
        [Route("getAllBannerImagesByCategoryId/{id}")]
        public ObjectResult GetAllFeaturedItemsBannerImagesForStoreFrontByCategoryId(int id)
        {
            var banners = _featuredItemQueryProcessor.GetAllFeaturedItemsBannerImagesByCategoryId(id);

            return Ok(banners);
        }

        [HttpGet]
        [Route("getAllBannerImages")]
        public ObjectResult GetAllFeaturedItemsBannerImagesForStoreFront()
        {
            var banners = _featuredItemQueryProcessor.GetAllFeaturedItemsBannerImages();

            return Ok(banners);
        }


        [HttpGet]
        [Route("getimagetypes")]
        public ObjectResult GetAllImageTypes()
        {
            var mapper = new ImageTypesToImageTypeViewModelMapper();

            return Ok(_imageTypeQueryProcessor.GetActiveImageTypes().Select(x => mapper.Map(x)));
        }

        [HttpGet]
        [Route("getBannerImagesByCategoryIdAndImageTypeId/{imageTypeId}/{categoryId}")]
        public ObjectResult GetAllBannerImagesByCategoryIdAndImageType(int imageTypeId, int categoryId)
        {
            var mapper = new FeaturedItemToFeaturedItemViewModelMapper();

            var featuredItems = _featuredItemQueryProcessor.GetFeaturedItemsByProductCategoryIdAndImagetype(imageTypeId, categoryId);

            return Ok(featuredItems);
        }
        [HttpPost]
        [Route("bulkDelete")]
        public ObjectResult BulkDelete([FromBody] List<int?> featuredItemId)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteFeaturedItem))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            if (featuredItemId == null || featuredItemId.Count <= 0) return Ok(false);
            try
            {
                var itemMeasureMapper = new FeaturedItemToFeaturedItemViewModelMapper();
                isDeleted = _featuredItemQueryProcessor.DeleteRange(featuredItemId);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeleteFeaturedItem, ex, ex.Message);
            }
            return Ok(isDeleted);
        }

    }
}
