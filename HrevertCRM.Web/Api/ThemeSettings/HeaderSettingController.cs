using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HrevertCRM.Data.QueryProcessors;
using HrevertCRM.Data;
using Hrevert.Common.Constants.Others;
using HrevertCRM.Data.Mapper.ThemeSettings;
using HrevertCRM.Data.ViewModels.ThemeSettings;
using HrevertCRM.Entities;

namespace HrevertCRM.Web.Api
{
    [Route("api/[controller]")]

    public class HeaderSettingController : Controller
    {
        private readonly IHeaderSettingQueryProcessor _headerSettingQueryProcessor;
        private readonly IStoreLocatorQueryProcessor _storeLocatorQueryProcessor;
        private readonly ILogger<HeaderSetting> _logger;
        private readonly ISecurityQueryProcessor _securityQueryProcessor;
        private readonly IDbContext _context;
        public HeaderSettingController(
            IHeaderSettingQueryProcessor headerSettingQueryProcessor, IStoreLocatorQueryProcessor storeLocatorQueryProcessor,
            ILoggerFactory factory, 
            ISecurityQueryProcessor securityQueryProcessor, 
            IDbContext context)
        {
            _headerSettingQueryProcessor = headerSettingQueryProcessor;
            _storeLocatorQueryProcessor = storeLocatorQueryProcessor;
            _logger = factory.CreateLogger<HeaderSetting>();
            _securityQueryProcessor = securityQueryProcessor;
            _context = context;
        }

        [HttpGet]
        [Route("getHeaderSetting")]
        public ObjectResult GetHeaderSetting()
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewGeneralSettings))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            try
            {
                var mapper = new HeaderSettingToHeaderSettingViewModelMapper();
                var data = _headerSettingQueryProcessor.GetHeaderSetting();
                return Ok(mapper.Map(data));
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewHeaderSettings, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("deleteStoreLocator/{id}")]
        public ObjectResult DeleteStoreLocator(int id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.UpdateGeneralSetting))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            try
            {
                isDeleted = _headerSettingQueryProcessor.DeleteStoreLocator(id);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeleteStoreLocator, ex, ex.Message);
            }
            return Ok(isDeleted);
        }

        [HttpGet]
        [Route("get/{id}")]
        public ObjectResult Get(int id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewGeneralSettings))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            try
            {
                var mapper = new HeaderSettingToHeaderSettingViewModelMapper();
                var data = _headerSettingQueryProcessor.Get(id);
                return Ok(mapper.Map(data));
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewHeaderSettings, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPost]
        [Route("create")]
        public ObjectResult Create([FromBody] HeaderSettingViewModel headerSettingViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.AddGeneralSetting))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });
            var storeLocatorViewModels = headerSettingViewModel.StoreLocatorViewModels;
            headerSettingViewModel.StoreLocatorViewModels = null;
            var mapper = new HeaderSettingToHeaderSettingViewModelMapper();
            var newHeaderSetting = mapper.Map(headerSettingViewModel);
            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    var savedHeaderSetting = _headerSettingQueryProcessor.Save(newHeaderSetting);
                    if (storeLocatorViewModels != null && storeLocatorViewModels.Count > 0)
                    {
                        storeLocatorViewModels.ForEach(x => x.HeaderSettingId = savedHeaderSetting.Id);
                        var storeLocationMapper = new StoreLocatorToStoreLocatorViewModelMapper();
                        _storeLocatorQueryProcessor.SaveListOfStoreLocators(storeLocationMapper.Map(storeLocatorViewModels));
                    }
                    trans.Commit();
                    headerSettingViewModel = mapper.Map(savedHeaderSetting);
                    return Ok(headerSettingViewModel);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    _logger.LogCritical((int)SecurityId.AddHeaderSetting, ex, ex.Message);
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
        public ObjectResult Put([FromBody] HeaderSettingViewModel headerSettingViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.UpdateHeaderSetting))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });
            var storeLocatorViewModels = headerSettingViewModel.StoreLocatorViewModels;
            headerSettingViewModel.StoreLocatorViewModels = null;
            var mapper = new HeaderSettingToHeaderSettingViewModelMapper();
            var newHeaderSetting = mapper.Map(headerSettingViewModel);
            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    var updatedHeaderSetting = _headerSettingQueryProcessor.Update(newHeaderSetting);
                    if (storeLocatorViewModels != null && storeLocatorViewModels.Count > 0)
                    {
                        storeLocatorViewModels.ForEach(x => x.HeaderSettingId = updatedHeaderSetting.Id);
                        var storeLocatorMapper = new StoreLocatorToStoreLocatorViewModelMapper();
                        _storeLocatorQueryProcessor.UpdateListOfStoreLocators(storeLocatorMapper.Map(storeLocatorViewModels));
                    }
                    trans.Commit();
                    headerSettingViewModel = mapper.Map(updatedHeaderSetting);
                    return Ok(headerSettingViewModel);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    _logger.LogCritical((int)SecurityId.UpdateHeaderSetting, ex, ex.Message);
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
