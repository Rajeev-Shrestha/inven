using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HrevertCRM.Data.QueryProcessors;
using Microsoft.Extensions.Logging;
using HrevertCRM.Data;
using Hrevert.Common.Constants.Others;
using HrevertCRM.Data.Mapper.ThemeSettings;
using HrevertCRM.Data.ViewModels.ThemeSettings;
using HrevertCRM.Entities;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HrevertCRM.Web.Api
{
    [Route("api/[controller]")]
    public class GeneralSettingController : Controller
    {
        private readonly IGeneralSettingQueryProcessor _generalSettingQueryProcessor;
        private readonly ILogger<GeneralSettingController> _logger;
        private readonly ISecurityQueryProcessor _securityQueryProcessor;
        public GeneralSettingController(
            IGeneralSettingQueryProcessor generalSettingQueryProcessor, 
            ILoggerFactory factory, 
            ISecurityQueryProcessor securityQueryProcessor
        )
        {
            _generalSettingQueryProcessor = generalSettingQueryProcessor;
            _logger = factory.CreateLogger<GeneralSettingController>();
            _securityQueryProcessor = securityQueryProcessor;
        }
        [HttpGet]
        [Route("get/{id}")]
        public ObjectResult GetGeneralSetting(int id) //Get Includes Full Tax data
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewGeneralSettings))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            try
            {
                var mapper = new GeneralSettingToGeneralSettingViewModelMapper();
                var data = _generalSettingQueryProcessor.GetGeneralSetting(id);
                return Ok(mapper.Map(data));
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewGeneralSettings, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("create")]
        public ObjectResult Create([FromBody] GeneralSettingViewModel generalSettingViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.AddGeneralSetting))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });
            generalSettingViewModel.StoreName = generalSettingViewModel.StoreName?.Trim();
            var model = generalSettingViewModel;
            var logoImage = generalSettingViewModel.LogoImage;
            var faviconLogo = generalSettingViewModel.FaviconLogoImage;
            var mapper = new GeneralSettingToGeneralSettingViewModelMapper();
            var newGeneralSetting = mapper.Map(generalSettingViewModel);
            try
            {
                var logoImageUrl = "";
                var faviconImageUrl = "";
                if (logoImage != null) logoImageUrl = SaveGeneralSettingImage(logoImage);
                if (faviconLogo != null) faviconImageUrl = SaveGeneralSettingImage(faviconLogo);
                if (logoImageUrl != null) newGeneralSetting.LogoUrl = logoImageUrl;
                if (faviconImageUrl != null) newGeneralSetting.FaviconLogoUrl = faviconImageUrl;
                var savedGeneralSetting = _generalSettingQueryProcessor.Save(newGeneralSetting);
                generalSettingViewModel = mapper.Map(savedGeneralSetting);
                return Ok(generalSettingViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewGeneralSettings, ex, ex.Message);
                ModelState.AddModelError("", ex.Message);
                return BadRequest(new
                {
                    errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                });
            }
        }

        [HttpPut]
        [Route("update")]
        public ObjectResult Put([FromBody] GeneralSettingViewModel generalSettingViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.UpdateGeneralSetting))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });

            generalSettingViewModel.StoreName = generalSettingViewModel.StoreName?.Trim();
            var model = generalSettingViewModel;
            var mapper = new GeneralSettingToGeneralSettingViewModelMapper();
            var newGeneralSetting = mapper.Map(generalSettingViewModel);
            try
            {
                if (generalSettingViewModel.LogoImage?.ImageBase64 != null)
                {
                    if (generalSettingViewModel.Id != null)
                    {
                        var existingLogoImageUrl = _generalSettingQueryProcessor.GetLogoImageUrl((int)generalSettingViewModel.Id);
                        if (existingLogoImageUrl == null)
                        {
                            var logoImageUrl = SaveGeneralSettingImage(generalSettingViewModel.LogoImage);
                            newGeneralSetting.LogoUrl = logoImageUrl;
                        }
                    }
                }
                if (generalSettingViewModel.FaviconLogoImage?.ImageBase64 != null)
                {
                    if (generalSettingViewModel.Id != null)
                    {
                        var existingFaviconImageUrl = _generalSettingQueryProcessor.GetGetFaviconImageUrl((int)generalSettingViewModel.Id);
                        if (existingFaviconImageUrl == null)
                        {
                            var faviconImageUrl = SaveGeneralSettingImage(generalSettingViewModel.FaviconLogoImage);
                            newGeneralSetting.FaviconLogoUrl = faviconImageUrl;
                        }
                    }
                }
                var updatedGeneralSetting = _generalSettingQueryProcessor.Update(newGeneralSetting);
                generalSettingViewModel = mapper.Map(updatedGeneralSetting);
                return Ok(generalSettingViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.UpdateGeneralSetting, ex, ex.Message);
                ModelState.AddModelError("", ex.Message);
                return BadRequest(new
                {
                    errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                });
            }
        }
          private string SaveGeneralSettingImage(ThemeImage image)
        {
            var imageUrl = _generalSettingQueryProcessor.SaveThemeSettingImage(image);
            return imageUrl;
        }

        [HttpGet]
        [Route("getAll")]
        public ObjectResult GetAll()
        {
            var result = _generalSettingQueryProcessor.Get();
            var mapper = new GeneralSettingToGeneralSettingViewModelMapper();
            return Ok(mapper.Map(result));
        }

        [HttpGet]
        [Route("getThemeColor")]
        public ObjectResult GetThemeColor()
        {
            var result = _generalSettingQueryProcessor.GetThemeColor();
            //var mapper = new GeneralSettingToGeneralSettingViewModelMapper();
            return Ok(result);
        }

        [HttpPut]
        [Route("removeLogo/{id}")]
        public ObjectResult RemoveLogo(int id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.UpdateGeneralSetting))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            try
            {
                isDeleted = _generalSettingQueryProcessor.RemoveLogo(id);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.UpdateGeneralSetting, ex, ex.Message);
            }
            return Ok(isDeleted);
        }

        [HttpPut]
        [Route("removeFavicon/{id}")]
        public ObjectResult RemoveFavicon(int id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.UpdateGeneralSetting))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            try
            {
                isDeleted = _generalSettingQueryProcessor.RemoveFavicon(id);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.UpdateGeneralSetting, ex, ex.Message);
            }
            return Ok(isDeleted);
        }
    }
}
