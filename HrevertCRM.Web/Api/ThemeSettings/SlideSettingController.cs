using System;
using System.Linq;
using Hrevert.Common.Constants.Others;
using HrevertCRM.Data;
using HrevertCRM.Data.Mapper.ThemeSettings;
using HrevertCRM.Data.QueryProcessors;
using HrevertCRM.Data.ViewModels.ThemeSettings;
using HrevertCRM.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HrevertCRM.Web.Api.ThemeSettings
{
    [Route("api/[controller]")]
    public class SlideSettingController : Controller
    {
        private readonly ISlideSettingQueryProcessor _slideSettingQueryProcessor;
        private readonly IIndividualSlideSettingQueryProcessor _individualSlideSettingQueryProcessor;
        private readonly ISecurityQueryProcessor _securityQueryProcessor;
        private readonly IDbContext _context;
        private readonly IGeneralSettingQueryProcessor _generalSettingQueryProcessor;
        private readonly ILogger<SlideSetting> _logger;

        public SlideSettingController(
            ISlideSettingQueryProcessor slideSettingQueryProcessor, 
            IIndividualSlideSettingQueryProcessor individualSlideSettingQueryProcessor,
            ILoggerFactory factory,
            ISecurityQueryProcessor securityQueryProcessor,
            IDbContext context,
            IGeneralSettingQueryProcessor generalSettingQueryProcessor
            )
        {
            _slideSettingQueryProcessor = slideSettingQueryProcessor;
            _individualSlideSettingQueryProcessor = individualSlideSettingQueryProcessor;
            _securityQueryProcessor = securityQueryProcessor;
            _context = context;
            _generalSettingQueryProcessor = generalSettingQueryProcessor;
            _logger = factory.CreateLogger<SlideSetting>();
        }

        [HttpPost]
        [Route("create")]
        public ObjectResult Create([FromBody] SlideSettingViewModel slideSettingViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.AddSlideSetting))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });
            var indivisualSlideSettingViewModels = slideSettingViewModel.IndividualSlideSettingViewModels;
            slideSettingViewModel.IndividualSlideSettingViewModels = null;
            var mapper = new SlideSettingToSlideSettingViewModelMapper();
            var mappedSlideSettingViewModel = mapper.Map(slideSettingViewModel);
            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    var savedSlideSetting = _slideSettingQueryProcessor.Save(mappedSlideSettingViewModel);
                    if (indivisualSlideSettingViewModels != null && indivisualSlideSettingViewModels.Count > 0)
                    {
                        indivisualSlideSettingViewModels.ForEach(x => x.SlideSettingId = savedSlideSetting.Id);
                        foreach (var indivisualSlideSettingViewModel in indivisualSlideSettingViewModels)
                        {
                            var slideImage = indivisualSlideSettingViewModel.SlideImage;
                            var slideBackgroundImage = indivisualSlideSettingViewModel.SlideBackgroundImage;
                            indivisualSlideSettingViewModel.SlideBackgroundImage = null;
                            indivisualSlideSettingViewModel.SlideBackgroundImage = null;

                            if (slideImage != null)
                            {
                                var slideImageUrl = _generalSettingQueryProcessor.SaveThemeSettingImage(slideImage);
                                indivisualSlideSettingViewModel.SlideImageUrl = slideImageUrl;
                            }
                            if (slideBackgroundImage != null)
                            {
                                var slideBackgroundImageUrl = _generalSettingQueryProcessor.SaveThemeSettingImage(slideBackgroundImage);
                                indivisualSlideSettingViewModel.SlideBackgroundImageUrl = slideBackgroundImageUrl;
                            }
                            var individualSlideSettingMapper = new IndividualSlideSettingToIndividualSlideSettingViewModelMapper();
                            _individualSlideSettingQueryProcessor.Save(
                                individualSlideSettingMapper.Map(indivisualSlideSettingViewModel));
                        }
                    }
                    trans.Commit();
                    slideSettingViewModel = mapper.Map(savedSlideSetting);
                    return Ok(slideSettingViewModel);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    _logger.LogCritical((int)SecurityId.AddSlideSetting, ex, ex.Message);
                    ModelState.AddModelError("", ex.Message);
                    return BadRequest(new
                    {
                        errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                    });
                }
            }
        }

        [HttpGet]
        [Route("getAll")]
        public ObjectResult Get()
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewSlideSettings))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var mapper = new SlideSettingToSlideSettingViewModelMapper();
            var result = _slideSettingQueryProcessor.Get();
            return Ok(mapper.Map(result));
        }

        [HttpPut]
        [Route("update")]
        public ObjectResult Put([FromBody] SlideSettingViewModel slideSettingViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.UpdateSlideSetting))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });
            var individualSlideSettingViewModels = slideSettingViewModel.IndividualSlideSettingViewModels;
            slideSettingViewModel.IndividualSlideSettingViewModels = null;
            var mapper = new SlideSettingToSlideSettingViewModelMapper();
            var mappedSlideSettingViewModel = mapper.Map(slideSettingViewModel);
            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    var updatedSlideSetting = _slideSettingQueryProcessor.Update(mappedSlideSettingViewModel);
                    if (individualSlideSettingViewModels != null && individualSlideSettingViewModels.Count > 0)
                    {
                        individualSlideSettingViewModels.ForEach(x => x.SlideSettingId = updatedSlideSetting.Id);
                        foreach (var indivisualSlideSettingViewModel in individualSlideSettingViewModels)
                        {
                            var slideImage = indivisualSlideSettingViewModel.SlideImage;
                            var slideBackgroundImage = indivisualSlideSettingViewModel.SlideBackgroundImage;
                            indivisualSlideSettingViewModel.SlideBackgroundImage = null;
                            indivisualSlideSettingViewModel.SlideBackgroundImage = null;

                            if (slideImage?.ImageBase64 != null)
                            {
                                var slideImageUrl = _generalSettingQueryProcessor.SaveThemeSettingImage(slideImage);
                                indivisualSlideSettingViewModel.SlideImageUrl = slideImageUrl;
                            }
                            if (slideBackgroundImage?.ImageBase64 != null)
                            {
                                var slideBackgroundImageUrl = _generalSettingQueryProcessor.SaveThemeSettingImage(slideBackgroundImage);
                                indivisualSlideSettingViewModel.SlideBackgroundImageUrl = slideBackgroundImageUrl;
                            }
                            var individualSlideSettingMapper = new IndividualSlideSettingToIndividualSlideSettingViewModelMapper();
                            if (indivisualSlideSettingViewModel.Id == null)
                            {
                                _individualSlideSettingQueryProcessor.Save(
                                    individualSlideSettingMapper.Map(indivisualSlideSettingViewModel));
                            }
                            _individualSlideSettingQueryProcessor.Update(
                                individualSlideSettingMapper.Map(indivisualSlideSettingViewModel));
                        }
                    }

                    trans.Commit();
                    slideSettingViewModel = mapper.Map(updatedSlideSetting);
                    return Ok(slideSettingViewModel);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    _logger.LogCritical((int)SecurityId.UpdateSlideSetting, ex, ex.Message);
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
