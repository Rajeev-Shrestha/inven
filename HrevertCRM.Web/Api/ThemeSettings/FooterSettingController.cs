using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HrevertCRM.Data.QueryProcessors;
using HrevertCRM.Data.ViewModels.ThemeSettings;
using HrevertCRM.Data;
using Hrevert.Common.Constants.Others;
using HrevertCRM.Data.Mapper.ThemeSettings;
using HrevertCRM.Entities;
using Hrevert.Common.Enums;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HrevertCRM.Web.Api
{
    [Route("api/[controller]")]
    public class FooterSettingController : Controller
    {
        private readonly IFooterSettingQueryProcessor _footerSettingQueryProcessor;
        private readonly IGeneralSettingQueryProcessor _generalSettingQueryProcecssor;
        private readonly IDbContext _context;
        private readonly IBrandImageQueryProcessor _brandImageQueryProcessor;
        private readonly ILogger<FooterSettingController> _logger;
        private readonly ISecurityQueryProcessor _securityQueryProcessor;
        public FooterSettingController(IFooterSettingQueryProcessor footerSettingQueryProcessor, 
            ILoggerFactory factory, ISecurityQueryProcessor securityQueryProcessor, 
            IGeneralSettingQueryProcessor generalSettingQueryProcecssor,
            IDbContext context,
            IBrandImageQueryProcessor brandImageQueryProcessor)
        {
            _footerSettingQueryProcessor = footerSettingQueryProcessor;
            _logger = factory.CreateLogger<FooterSettingController>();
            _securityQueryProcessor = securityQueryProcessor;
            _generalSettingQueryProcecssor = generalSettingQueryProcecssor;
            _context = context;
            _brandImageQueryProcessor = brandImageQueryProcessor;
        }
        // GET: api/values
        [HttpPost]
       [Route("Create")]
        public ObjectResult Create([FromBody] FooterSettingViewModel footerSettingViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.AddFooterSetting))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });
            var footerImage = footerSettingViewModel.FooterImage;
            var brandImages = footerSettingViewModel.ThemeBrandImages;
            var mapper = new FooterSettingToFooterSettingViewModelMapper();
            var newFooterSetting = mapper.Map(footerSettingViewModel);
            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    var footerImageUrl = "";
                    if (footerImage != null) footerImageUrl = SaveFooterSettingImage(footerImage);
                    if (footerImageUrl != null) newFooterSetting.FooterLogoUrl = footerImageUrl;
                    var savedFooterSetting = _footerSettingQueryProcessor.Save(newFooterSetting);

                    //Save Images
                    if (brandImages != null && brandImages.Count > 0)
                    {
                        foreach (var image in brandImages)
                        {
                            if (image?.ImageBase64 == null) continue;
                            var brandImageUrl = _generalSettingQueryProcecssor.SaveThemeSettingImage(image);
                            var brandImage = new BrandImage
                            {
                                ImageUrl = brandImageUrl,
                                FooterSettingId = savedFooterSetting.Id,
                                WebActive = true
                            };
                            _brandImageQueryProcessor.Save(brandImage);
                        }
                    }

                    trans.Commit();
                    footerSettingViewModel = mapper.Map(savedFooterSetting);
                    return Ok(footerSettingViewModel);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    _logger.LogCritical((int)SecurityId.AddFooterSetting, ex, ex.Message);
                    ModelState.AddModelError("", ex.Message);
                    return BadRequest(new
                    {
                        errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                    });
                }
            }
        }

        [HttpPut]
        [Route("update")]

        public ObjectResult Put([FromBody] FooterSettingViewModel footerSettingViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.UpdateFooterSetting))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });
            var footerImage = footerSettingViewModel.FooterImage;
            var brandImages = footerSettingViewModel.ThemeBrandImages;
            var mapper = new FooterSettingToFooterSettingViewModelMapper();
            var newFooterSetting = mapper.Map(footerSettingViewModel);
            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    if (footerImage.ImageBase64 != null)
                    {
                        var footerImageUrl = SaveFooterSettingImage(footerImage);
                        newFooterSetting.FooterLogoUrl = footerImageUrl;
                    }
                    var updatedFooterSetting = _footerSettingQueryProcessor.Update(newFooterSetting);

                    //Save Images
                    if (brandImages != null && brandImages.Count > 0)
                    {
                        foreach (var image in brandImages)
                        {
                            if(image.ImageBase64 == null) continue; 
                            var brandImageUrl = _generalSettingQueryProcecssor.SaveThemeSettingImage(image);
                            var brandImage = new BrandImage
                            {
                                ImageUrl = brandImageUrl,
                                FooterSettingId = updatedFooterSetting.Id,
                                WebActive = true
                            };
                            _brandImageQueryProcessor.Save(brandImage);
                        }
                    }

                    trans.Commit();
                    footerSettingViewModel = mapper.Map(updatedFooterSetting);
                    return Ok(footerSettingViewModel);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    _logger.LogCritical((int)SecurityId.UpdateFooterSetting, ex, ex.Message);
                    ModelState.AddModelError("", ex.Message);
                    return BadRequest(new
                    {
                        errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                    });
                }
            }
        }
        private void DetermineImagesToAdd(FooterSettingViewModel footerSettingViewModel, ThemeSettingImageType imageType)
        {
            //fetch existing catgories
            if (footerSettingViewModel.Id == null) return;
            if (imageType == ThemeSettingImageType.BrandImage)
            {
                var existingImagePaths = _footerSettingQueryProcessor.GetBrandImageUrls((int)footerSettingViewModel.Id);
                if(existingImagePaths == null || existingImagePaths.Count <= 0) return;
                var imageUrlList = new List<string>();
                foreach (var existingImageUrl in existingImagePaths)
                {
                    var value = existingImageUrl.Substring(existingImageUrl.LastIndexOf('/') + 1);
                    imageUrlList.Add(value);
                }
                foreach (var imgUrl in imageUrlList)
                {
                    if (footerSettingViewModel.ThemeBrandImages.Select(x => x.FileName).Any(x=>x.Equals(imgUrl)))
                        footerSettingViewModel.ThemeBrandImages.Remove(footerSettingViewModel.ThemeBrandImages.FirstOrDefault(x=>x.FileName==imgUrl));
                    else
                        //remove from db
                    _brandImageQueryProcessor.DeleteBrandImage((int)footerSettingViewModel.Id, imgUrl);
                }
            }
            else
            {
                var existingImagePath = _footerSettingQueryProcessor.GetFooterImagePath((int)footerSettingViewModel.Id);
                if (existingImagePath == null) return;
                var existingImageName = existingImagePath.Substring(existingImagePath.LastIndexOf('/') + 1);
                if (footerSettingViewModel.FooterImage.FileName.Equals(existingImageName))
                    footerSettingViewModel.FooterImage = null;
                else
                    _footerSettingQueryProcessor.DeleteImage(footerSettingViewModel, existingImagePath);
            }
        }
        private string SaveFooterSettingImage(ThemeImage footerImage)
        {
            var imageUrl = _generalSettingQueryProcecssor.SaveThemeSettingImage(footerImage);
            return imageUrl;
        }

        [HttpGet]
        [Route("getAll")]
        public ObjectResult Get()
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewFooterSettings))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var mapper = new FooterSettingToFooterSettingViewModelMapper();
            var result = _footerSettingQueryProcessor.Get();
            return Ok(mapper.Map(result));
        }

        [HttpDelete]
        [Route("deleteImage/{id}/{imageUrl}")]
        public ObjectResult DeleteBrandImage(int id, string imageUrl)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteFooterSetting))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            try
            {
                isDeleted = _footerSettingQueryProcessor.DeleteBrandImage(id, imageUrl);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeleteFooterSetting, ex, ex.Message);
            }
            return Ok(isDeleted);
        }
        [HttpPut]
        [Route("removeFooterLogo/{id}")]
        public ObjectResult RemoveFooterLogo(int id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.UpdateFooterSetting))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            try
            {
                isDeleted = _footerSettingQueryProcessor.RemoveFooterLogo(id);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.UpdateFooterSetting, ex, ex.Message);
            }
            return Ok(isDeleted);
        }
    }
}
