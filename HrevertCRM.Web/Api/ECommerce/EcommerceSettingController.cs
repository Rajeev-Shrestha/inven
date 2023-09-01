using System;
using System.Linq;
using Hrevert.Common.Constants.Others;
using HrevertCRM.Data;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.Mapper;
using HrevertCRM.Data.QueryProcessors;
using HrevertCRM.Data.QueryProcessors.Enumerations;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HrevertCRM.Web.Api
{
    [Route("api/[controller]")]
    public class EcommerceSettingController : Controller
    {
        private readonly IEcommerceSettingQueryProcessor _ecommerceSettingQueryProcessor;
        private readonly ISecurityQueryProcessor _securityQueryProcessor;
        private readonly ILogger<EcommerceSettingController> _logger;

        public EcommerceSettingController(IEcommerceSettingQueryProcessor ecommerceSettingQueryProcessor,
            IDbContext context, IPagedDataRequestFactory pagedDataRequestFactory,
            ILoggerFactory factory,
            IEncryptionTypesQueryProcessor encryptionTypesQueryProcessor,
            ISecurityQueryProcessor securityQueryProcessor)
        {
            _ecommerceSettingQueryProcessor = ecommerceSettingQueryProcessor;
            _securityQueryProcessor = securityQueryProcessor;
            _logger = factory.CreateLogger<EcommerceSettingController>();
        }

        [HttpGet]
        [Route("getactiveEcommerceSettings")]
        public ObjectResult GetActiveEcommerceSettings()
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewECommerceSettings))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            try
            {
                var mapper = new EcommerceSettingToEcommerceSettingViewModelMapper();
                return Ok( mapper.Map(_ecommerceSettingQueryProcessor.GetActiveEcommerceSettings()));
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewECommerceSettings, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getEcommerceSettingbyid/{id}")]
        public ObjectResult Get(int id) //Get Includes Full EcommerceSetting data
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewECommerceSettings))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            return Ok(_ecommerceSettingQueryProcessor.GetEcommerceSettingViewModel(id));
        }


        [HttpGet]
        [Route("activateEcommerceSetting/{id}")]
        public ObjectResult ActivateEcommerceSetting(int id)
        {
            var mapper = new EcommerceSettingToEcommerceSettingViewModelMapper();
            return Ok(mapper.Map(_ecommerceSettingQueryProcessor.ActivateEcommerceSetting(id)));
        }

        [HttpPost]
        public ObjectResult Create([FromBody] EcommerceSettingViewModel ecommerceSettingViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.AddECommerceSetting))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });

            var image = ecommerceSettingViewModel.Image;
            var mapper = new EcommerceSettingToEcommerceSettingViewModelMapper();
            var newEcommerceSetting = mapper.Map(ecommerceSettingViewModel);
            try
            {
                var savedEcommerceSetting = _ecommerceSettingQueryProcessor.Save(newEcommerceSetting);
                ecommerceSettingViewModel = mapper.Map(savedEcommerceSetting);

                if (image != null)
                {
                    SaveEcommerceLogo(image);
                }
                return Ok(ecommerceSettingViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.AddECommerceSetting, ex, ex.Message);
                ModelState.AddModelError("", ex.Message);
                return BadRequest(new
                {
                    errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                });
            }
        }

        [HttpPost]
        [Route("SaveEcommerceLogo")]
        private void SaveEcommerceLogo([FromBody]Image image)
        {
            _ecommerceSettingQueryProcessor.SaveEcommerceLogo(image);
        }

        [HttpPost("DeleteLogo/{imageUri}")]
        public ObjectResult DeleteLogo(string imageUri)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteECommerceSetting))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            try
            {
                isDeleted = _ecommerceSettingQueryProcessor.DeleteLogo(imageUri);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeleteProduct, ex, ex.Message);
            }
            return Ok(isDeleted);
        }

        [HttpDelete("{id}")]
        public ObjectResult Delete(int id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteECommerceSetting))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            try
            {
                isDeleted = _ecommerceSettingQueryProcessor.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeleteECommerceSetting, ex, ex.Message);
            }
            return Ok(isDeleted);
        }

        [HttpPut]
        public ObjectResult Put([FromBody] EcommerceSettingViewModel ecommerceSettingViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.UpdateECommerceSetting))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });

            var image = ecommerceSettingViewModel.Image;
            var mapper = new EcommerceSettingToEcommerceSettingViewModelMapper();
            var newEcommerceSetting = mapper.Map(ecommerceSettingViewModel);

            try
            {
                if (_ecommerceSettingQueryProcessor.IsExist())
                {
                    var updatedEcommerceSetting = _ecommerceSettingQueryProcessor.Update(newEcommerceSetting);
                    ecommerceSettingViewModel = mapper.Map(updatedEcommerceSetting);
                    if (image != null)
                    {
                        SaveEcommerceLogo(image);
                    }
                }
                else
                {
                    var updatedEcommerceSetting = _ecommerceSettingQueryProcessor.Save(newEcommerceSetting);
                    ecommerceSettingViewModel = mapper.Map(updatedEcommerceSetting);
                    if (image != null)
                    {
                        SaveEcommerceLogo(image);
                    }
                }
                return Ok(ecommerceSettingViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.UpdateECommerceSetting, ex, ex.Message);
                ModelState.AddModelError("", ex.Message);
                return BadRequest(new
                {
                    errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                });
            }
        }
    }
}
