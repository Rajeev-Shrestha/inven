using System;
using System.Collections.Generic;
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
    public class CarouselController : Controller
    {
        private readonly ICarouselQueryProcessor _carouselQueryProcessor;
        private readonly IPagedDataRequestFactory _pagedDataRequestFactory;
        private readonly ISecurityQueryProcessor _securityQueryProcessor;
        private readonly IProductQueryProcessor _productQueryProcessor;
        private readonly ILogger<CarouselController> _logger;

        public CarouselController(ICarouselQueryProcessor carouselQueryProcessor,
            IDbContext context, IPagedDataRequestFactory pagedDataRequestFactory,
            ILoggerFactory factory,
            IEncryptionTypesQueryProcessor encryptionTypesQueryProcessor,
            ISecurityQueryProcessor securityQueryProcessor, 
            IProductQueryProcessor productQueryProcessor)
        {
            _carouselQueryProcessor = carouselQueryProcessor;
            _pagedDataRequestFactory = pagedDataRequestFactory;
            _securityQueryProcessor = securityQueryProcessor;
            _productQueryProcessor = productQueryProcessor;
            _logger = factory.CreateLogger<CarouselController>();
        }
        
        [HttpGet]
        [Route("getCarouselbyid/{id}")]
        public ObjectResult Get(int id) //Get Includes Full Carousel data
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewCarousels))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            return Ok(_carouselQueryProcessor.GetCarouselViewModel(id));
        }


        [HttpGet]
        [Route("activateCarousel/{id}")]
        public ObjectResult ActivateCarousel(int id)
        {
            return Ok(_carouselQueryProcessor.ActivateCarousel(id));
        }

        [HttpPost]
        public ObjectResult Create([FromBody] CarouselViewModel carouselViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.AddCarousel))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });
            try
            {
                var mapper = new CarouselToCarouselViewModelMapper();
                var mappedCarousel = mapper.Map(carouselViewModel);
                mappedCarousel.Image = null;

                var savedCarousel = _carouselQueryProcessor.Save(mappedCarousel);
                SaveImage(savedCarousel, carouselViewModel.Image);
                return Ok(savedCarousel);
            }
            catch (Exception ex)
            { 
                _logger.LogCritical((int) SecurityId.AddCarousel, ex, ex.Message);
                ModelState.AddModelError("", ex.Message);
                return BadRequest(new
                {
                    errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                });
            }
        }

        private string SaveImage(Carousel savedCarousel, Image image)
        {
            return _carouselQueryProcessor.SaveImage(savedCarousel, image);
        }

        [HttpDelete("{id}")]
        public ObjectResult Delete(int id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteCarousel))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            try
            {
                isDeleted = _carouselQueryProcessor.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeleteCarousel, ex, ex.Message);
            }
            return Ok(isDeleted);
        }

        [HttpPut]
        public ObjectResult Put([FromBody] CarouselViewModel carouselViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.UpdateCarousel))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });

            var mapper = new CarouselToCarouselViewModelMapper();
            var newCarousel = mapper.Map(carouselViewModel);
            try
            {
                var updatedCarousel = _carouselQueryProcessor.Update(newCarousel);
                carouselViewModel = mapper.Map(updatedCarousel);
                return Ok(carouselViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.UpdateCarousel, ex, ex.Message);
                ModelState.AddModelError("", ex.Message);
                return BadRequest(new
                {
                    errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                });
            }
        }

        [HttpGet]
        [Route("searchCarousels/{active}/{searchText}")]
        public ObjectResult SearchCarousels(bool active, string searchText)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewCarousels))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            try
            {
                var mapper = new CarouselToCarouselViewModelMapper();
                return Ok(_carouselQueryProcessor.SearchCarousels(active, searchText).Select(x => mapper.Map(x)));
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewCarousels, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
