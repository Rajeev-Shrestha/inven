using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HrevertCRM.Data.QueryProcessors;
using HrevertCRM.Data;
using Hrevert.Common.Constants.Others;
using HrevertCRM.Data.Mapper.ThemeSettings;
using HrevertCRM.Data.ViewModels.ThemeSettings;
using System.Linq;
using HrevertCRM.Entities;
using Hrevert.Common.Enums;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HrevertCRM.Web.Api
{
    [Route("api/[controller]")]
    public class LayoutSettingController : Controller
    {
        private readonly ILayoutSettingQueryProcessor _layoutSettingQueryProcessor;
        private readonly ILogger<LayoutSettingController> _logger;
        private readonly ISecurityQueryProcessor _securityQueryProcessor;
        private readonly IGeneralSettingQueryProcessor _generalSettingQueryProcessor;

        public LayoutSettingController(
            ILayoutSettingQueryProcessor layoutSettingQueryProcessor,
            IGeneralSettingQueryProcessor generalSettingQueryProcessor,
            ILoggerFactory factory, ISecurityQueryProcessor securityQueryProcessor)
        {
            _layoutSettingQueryProcessor = layoutSettingQueryProcessor;
            _logger = factory.CreateLogger<LayoutSettingController>();
            _securityQueryProcessor = securityQueryProcessor;
            _generalSettingQueryProcessor = generalSettingQueryProcessor;

        }

        [HttpGet]
        [Route("getLayoutSetting")]
        public ObjectResult GetLayoutSetting()
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewLayoutSettings))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
             
            try
            {
                var mapper = new LayoutSettingToLayoutSettingViewModelMapper();
                var data = _layoutSettingQueryProcessor.GetLayoutSetting();
                return Ok(mapper.Map(data));
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewLayoutSettings, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("get/{id}")]
        public ObjectResult Get(int id) 
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewLayoutSettings))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            try
            {
                var mapper = new LayoutSettingToLayoutSettingViewModelMapper();
                var data = _layoutSettingQueryProcessor.Get(id);
                return Ok(mapper.Map(data));
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewLayoutSettings, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("create")]
        public ObjectResult Create([FromBody] LayoutSettingViewModel layoutSettingViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.AddLayoutSetting))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });
            var personnelSettings = layoutSettingViewModel.PersonnelSettingViewModels;
            var backGroundImage = layoutSettingViewModel.BackgroundImage;
            var hotThisWeekImage = layoutSettingViewModel.HotThisWeekImage;
            var latestProductsImage = layoutSettingViewModel.LatestProductsImage;
            var hotThisWeekSeparatorImage = layoutSettingViewModel.HotThisWeekSeparatorImage;
            var latestproductsSeparatorImage = layoutSettingViewModel.LatestProductsSeparatorImage;
            var trendingItemsSeparatorImage = layoutSettingViewModel.TrendingItemsSeparatorImage;
            var trendingItemsImage = layoutSettingViewModel.TrendingItemsImage;
            layoutSettingViewModel.PersonnelSettingViewModels = null;
            var mapper = new LayoutSettingToLayoutSettingViewModelMapper();
            var newLayoutSetting = mapper.Map(layoutSettingViewModel);
            try
            {
                var backgroundImgUrl = "";
                var hotThisWeekImgUrl = "";
                var latestProductsImgUrl = "";
                var hotThisWeekSeparatorImageUrl = "";
                var latestproductsSeparatorImageUrl = "";
                var trendingItemsSeparatorImageUrl = "";
                var trendingImgUrl = "";
                if (backGroundImage?.ImageBase64 != null) backgroundImgUrl = SaveLayoutSettingImage(backGroundImage);
                if (hotThisWeekImage?.ImageBase64 != null) hotThisWeekImgUrl = SaveLayoutSettingImage(hotThisWeekImage);
                if (latestProductsImage?.ImageBase64 != null) latestProductsImgUrl = SaveLayoutSettingImage(latestProductsImage);
                if (hotThisWeekSeparatorImage?.ImageBase64 != null) hotThisWeekSeparatorImageUrl = SaveLayoutSettingImage(hotThisWeekSeparatorImage);
                if (latestproductsSeparatorImage?.ImageBase64 != null) latestproductsSeparatorImageUrl = SaveLayoutSettingImage(latestproductsSeparatorImage);
                if (trendingItemsSeparatorImage?.ImageBase64 != null) trendingItemsSeparatorImageUrl = SaveLayoutSettingImage(trendingItemsSeparatorImage);
                if (trendingItemsImage?.ImageBase64 != null) trendingImgUrl = SaveLayoutSettingImage(trendingItemsImage);

                if (backgroundImgUrl != null) newLayoutSetting.BackgroundImageUrl = backgroundImgUrl;
                if (hotThisWeekImgUrl != null) newLayoutSetting.HotThisWeekImageUrl = hotThisWeekImgUrl;
                if (latestProductsImgUrl != null) newLayoutSetting.LatestProductsImageUrl = latestProductsImgUrl;
                if (hotThisWeekSeparatorImageUrl != null) newLayoutSetting.HotThisWeekSeparatorUrl = hotThisWeekSeparatorImageUrl;
                if (latestproductsSeparatorImageUrl != null) newLayoutSetting.LatestProductsSeparatorUrl = latestproductsSeparatorImageUrl;
                if (trendingItemsSeparatorImageUrl != null) newLayoutSetting.TrendingItemsSeparatorUrl = trendingItemsSeparatorImageUrl;
                if (trendingImgUrl != null) newLayoutSetting.TrendingItemsImageUrl = trendingImgUrl;

                var savedLayoutSetting = _layoutSettingQueryProcessor.Save(newLayoutSetting);

                //Save Personnel Settings
                if (personnelSettings != null && personnelSettings.Count > 0)
                {
                    foreach (var personnelSettingViewModel in personnelSettings)
                    {
                        
                        var personnelImageUrl = personnelSettingViewModel.PersonnelImage.ImageBase64 == null ? null :
                            SaveLayoutSettingImage(personnelSettingViewModel.PersonnelImage);
                        var personnelSetting = new PersonnelSetting
                        {
                            PersonnelImageUrl = personnelImageUrl,
                            RecommendationText = personnelSettingViewModel.RecommendationText,
                            RecommendingPersonName = personnelSettingViewModel.RecommendingPersonName,
                            RecommendingPersonAddress = personnelSettingViewModel.RecommendingPersonAddress,
                            LayoutSettingId = savedLayoutSetting.Id
                        };
                        _layoutSettingQueryProcessor.SavePersonnelSetting(personnelSetting);
                    }
                }

                layoutSettingViewModel = mapper.Map(savedLayoutSetting);
                return Ok(layoutSettingViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.AddLayoutSetting, ex, ex.Message);
                ModelState.AddModelError("", ex.Message);
                return BadRequest(new
                {
                    errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                });
            }
        }

        [HttpPut]
        [Route("update")]
        public ObjectResult Put([FromBody] LayoutSettingViewModel layoutSettingViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.UpdateLayoutSetting))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });
            var personnelSettings = layoutSettingViewModel.PersonnelSettingViewModels;
            var backGroundImage = layoutSettingViewModel.BackgroundImage;
            var hotThisWeekImage = layoutSettingViewModel.HotThisWeekImage;
            var latestProductsImage = layoutSettingViewModel.LatestProductsImage;
            var hotThisWeekSeparatorImage = layoutSettingViewModel.HotThisWeekSeparatorImage;
            var latestproductsSeparatorImage = layoutSettingViewModel.LatestProductsSeparatorImage;
            var trendingItemsSeparatorImage = layoutSettingViewModel.TrendingItemsSeparatorImage;
            var trendingItemsImage = layoutSettingViewModel.TrendingItemsImage;
            var mapper = new LayoutSettingToLayoutSettingViewModelMapper();
            var newLayoutSetting = mapper.Map(layoutSettingViewModel);
            try
            {
                if (backGroundImage?.ImageBase64 != null)
                {
                    //DetermineImagesToAdd(layoutSettingViewModel, layoutSettingViewModel.BackgroundImage);
                    var backgroundImageUrl = SaveLayoutSettingImage(backGroundImage);
                    newLayoutSetting.BackgroundImageUrl = backgroundImageUrl;
                }
                if (hotThisWeekImage?.ImageBase64 != null)
                {
                    //DetermineImagesToAdd(layoutSettingViewModel, layoutSettingViewModel.HotThisWeekImage);
                    var hotThisWeekImageUrl = SaveLayoutSettingImage(hotThisWeekImage);
                    newLayoutSetting.HotThisWeekImageUrl = hotThisWeekImageUrl;
                }
                if (latestProductsImage?.ImageBase64 != null)
                {
                    //DetermineImagesToAdd(layoutSettingViewModel, layoutSettingViewModel.LatestProductsImage);
                    var latestProductsImageUrl = SaveLayoutSettingImage(latestProductsImage);
                    newLayoutSetting.LatestProductsImageUrl = latestProductsImageUrl;
                }
                if (hotThisWeekSeparatorImage?.ImageBase64 != null)
                {
                    //DetermineImagesToAdd(layoutSettingViewModel, layoutSettingViewModel.ParallaxImage);
                    hotThisWeekSeparatorImage.ImageType = ThemeSettingImageType.HotThisWeekSeparator;
                    var hotThisWeekSeparatorImageUrl = SaveLayoutSettingImage(hotThisWeekSeparatorImage);
                    newLayoutSetting.HotThisWeekSeparatorUrl = hotThisWeekSeparatorImageUrl;
                }
                if (trendingItemsSeparatorImage?.ImageBase64 != null)
                {
                    //DetermineImagesToAdd(layoutSettingViewModel, layoutSettingViewModel.ParallaxImage);
                    trendingItemsSeparatorImage.ImageType = ThemeSettingImageType.TrendingItemsSeparator;
                    var trendingItemsSeparatorImageUrl = SaveLayoutSettingImage(trendingItemsSeparatorImage);
                    newLayoutSetting.TrendingItemsSeparatorUrl = trendingItemsSeparatorImageUrl;
                }
                if (latestproductsSeparatorImage?.ImageBase64 != null)
                {
                    //DetermineImagesToAdd(layoutSettingViewModel, layoutSettingViewModel.ParallaxImage);
                    latestproductsSeparatorImage.ImageType = ThemeSettingImageType.LatestProductsSeparator;
                    var latestproductsSeparatorImageUrl = SaveLayoutSettingImage(latestproductsSeparatorImage);
                    newLayoutSetting.LatestProductsSeparatorUrl = latestproductsSeparatorImageUrl;
                }
                if (trendingItemsImage?.ImageBase64 != null)
                {
                    //DetermineImagesToAdd(layoutSettingViewModel, layoutSettingViewModel.TrendingItemsImage);
                    var trendingItemsUrl = SaveLayoutSettingImage(trendingItemsImage);
                    newLayoutSetting.TrendingItemsImageUrl = trendingItemsUrl;
                }

                var updatedLayoutSetting = _layoutSettingQueryProcessor.Update(newLayoutSetting);
                //Save Personnel Settings
                if (personnelSettings != null && personnelSettings.Count > 0)
                {
                    foreach (var personnelSettingViewModel in personnelSettings)
                    {
                        if (personnelSettingViewModel.Id == null)
                        {
                            var personnelImageUrl = personnelSettingViewModel.PersonnelImage.ImageBase64 == null
                                ? personnelSettingViewModel.PersonnelImageUrl
                                : SaveLayoutSettingImage(personnelSettingViewModel.PersonnelImage);
                            var personnelSetting = new PersonnelSetting
                            {
                                PersonnelImageUrl = personnelImageUrl,
                                RecommendationText = personnelSettingViewModel.RecommendationText,
                                RecommendingPersonName = personnelSettingViewModel.RecommendingPersonName,
                                RecommendingPersonAddress = personnelSettingViewModel.RecommendingPersonAddress,
                                LayoutSettingId = updatedLayoutSetting.Id
                            };
                            _layoutSettingQueryProcessor.SavePersonnelSetting(personnelSetting);
                        }
                        else
                        {
                            var personnelImageUrl = personnelSettingViewModel.PersonnelImage.ImageBase64 == null
                                ? personnelSettingViewModel.PersonnelImageUrl
                                : SaveLayoutSettingImage(personnelSettingViewModel.PersonnelImage);
                            var personnelSetting = new PersonnelSetting
                            {
                                Id = (int)personnelSettingViewModel.Id,
                                PersonnelImageUrl = personnelImageUrl,
                                RecommendationText = personnelSettingViewModel.RecommendationText,
                                RecommendingPersonName = personnelSettingViewModel.RecommendingPersonName,
                                RecommendingPersonAddress = personnelSettingViewModel.RecommendingPersonAddress,
                                LayoutSettingId = updatedLayoutSetting.Id
                            };
                            _layoutSettingQueryProcessor.SavePersonnelSetting(personnelSetting);
                        }
                    }
                }
                layoutSettingViewModel = mapper.Map(updatedLayoutSetting);
                return Ok(layoutSettingViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.UpdateLayoutSetting, ex, ex.Message);
                ModelState.AddModelError("", ex.Message);
                return BadRequest(new
                {
                    errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                });
            }
        }
        //private void DetermineImagesToAdd(LayoutSettingViewModel layoutSettingViewModel, ThemeImage img)
        //{
        //    if (layoutSettingViewModel.Id == null) return;
        //    switch (img.ImageType)
        //    {
        //        case ThemeSettingImageType.BackgroundImage:
        //            var existingImageUrl = _layoutSettingQueryProcessor.GetImageUrl((int)layoutSettingViewModel.Id, ThemeSettingImageType.BackgroundImage);
        //            if (layoutSettingViewModel.BackgroundImage.FileName.Any(x => x.Equals(existingImageUrl)))
        //                //leave as it is
        //                layoutSettingViewModel.BackgroundImage = null;
        //            else
        //                //remove from db
        //                _layoutSettingQueryProcessor.DeleteImage(layoutSettingViewModel, existingImageUrl, ThemeSettingImageType.BackgroundImage);
        //            break;
        //        case ThemeSettingImageType.HotThisWeekImage:
        //            var existingImageUrl1 = _layoutSettingQueryProcessor.GetImageUrl((int)layoutSettingViewModel.Id, ThemeSettingImageType.HotThisWeekImage);
        //            if (layoutSettingViewModel.HotThisWeekImage.FileName.Any(x => x.Equals(existingImageUrl1)))
        //                //leave as it is
        //                layoutSettingViewModel.HotThisWeekImage = null;
        //            else
        //                //remove from db
        //                _layoutSettingQueryProcessor.DeleteImage(layoutSettingViewModel, existingImageUrl1, ThemeSettingImageType.HotThisWeekImage);
        //            break;
        //        case ThemeSettingImageType.LatestProductsImage:
        //            var existingImageUrl2 = _layoutSettingQueryProcessor.GetImageUrl((int)layoutSettingViewModel.Id, ThemeSettingImageType.LatestProductsImage);
        //            if (layoutSettingViewModel.LatestProductsImage.FileName.Any(x => x.Equals(existingImageUrl2)))
        //                //leave as it is
        //                layoutSettingViewModel.LatestProductsImage = null;
        //            else
        //                //remove from db
        //                _layoutSettingQueryProcessor.DeleteImage(layoutSettingViewModel, existingImageUrl2, ThemeSettingImageType.LatestProductsImage);
        //            break;
        //        case ThemeSettingImageType.ParallaxImage:
        //            var existingImageUrl3 = _layoutSettingQueryProcessor.GetImageUrl((int)layoutSettingViewModel.Id, ThemeSettingImageType.ParallaxImage);
        //            if (layoutSettingViewModel.ParallaxImage.FileName.Any(x => x.Equals(existingImageUrl3)))
        //                //leave as it is
        //                layoutSettingViewModel.ParallaxImage = null;
        //            else
        //                //remove from db
        //                _layoutSettingQueryProcessor.DeleteImage(layoutSettingViewModel, existingImageUrl3, ThemeSettingImageType.ParallaxImage);
        //            break;
        //        case ThemeSettingImageType.TrendingItemsImage:
        //            var existingImageUrl5 = _layoutSettingQueryProcessor.GetImageUrl((int)layoutSettingViewModel.Id, ThemeSettingImageType.TrendingItemsImage);
        //            if (layoutSettingViewModel.TrendingItemsImage.FileName.Any(x => x.Equals(existingImageUrl5)))
        //                //leave as it is
        //                layoutSettingViewModel.TrendingItemsImage = null;
        //            else
        //                //remove from db
        //                _layoutSettingQueryProcessor.DeleteImage(layoutSettingViewModel, existingImageUrl5, ThemeSettingImageType.TrendingItemsImage);
        //            break;
        //        default:
        //            throw new ArgumentOutOfRangeException();
        //    }
        //}

        private string SaveLayoutSettingImage(ThemeImage image)
        {
            var imageUrl = _generalSettingQueryProcessor.SaveThemeSettingImage(image);
            return imageUrl;
        }

        [HttpDelete]
        [Route("removeLayoutImage/{imageUri}/{id}")]
        public ObjectResult RemoveLayoutImage(string imageUri, int id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.UpdateLayoutSetting))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            try
            {
                 isDeleted = _layoutSettingQueryProcessor.RemoveLayoutImage(imageUri, id);
            } 
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.UpdateLayoutSetting, ex, ex.Message);
            }
            return Ok(isDeleted);
        }

    }
}
