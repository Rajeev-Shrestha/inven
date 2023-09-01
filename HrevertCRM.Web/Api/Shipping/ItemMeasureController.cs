using System;
using System.Linq;
using Hrevert.Common.Constants;
using Hrevert.Common.Constants.Others;
using HrevertCRM.Data;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.Mapper;
using HrevertCRM.Data.QueryProcessors;
using HrevertCRM.Data.QueryProcessors.Enumerations;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace HrevertCRM.Web.Api
{
    [Route("api/[controller]")]
    public class ItemMeasureController : Controller
    {
        private readonly IItemMeasureQueryProcessor _itemMeasureQueryProcessor;
        private readonly ISecurityQueryProcessor _securityQueryProcessor;
        private readonly ILogger<ItemMeasureController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public ItemMeasureController(IItemMeasureQueryProcessor itemMeasureQueryProcessor,
            IDbContext context, IPagedDataRequestFactory pagedDataRequestFactory,
            ILoggerFactory factory,
            IEncryptionTypesQueryProcessor encryptionTypesQueryProcessor,
            ISecurityQueryProcessor securityQueryProcessor,
            IProductQueryProcessor productQueryProcessor, UserManager<ApplicationUser> userManager)
        {
            _itemMeasureQueryProcessor = itemMeasureQueryProcessor;
            _securityQueryProcessor = securityQueryProcessor;
            _userManager = userManager;
            _logger = factory.CreateLogger<ItemMeasureController>();
        }
        
        [HttpGet]
        [Route("getItemMeasurebyid/{id}")]
        public ObjectResult Get(int id) //Get Includes Full ItemMeasure data
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewItemMeasures))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            return Ok(_itemMeasureQueryProcessor.GetItemMeasureViewModel(id));
        }


        [HttpGet]
        [Route("activateItemMeasure/{id}")]
        public ObjectResult ActivateItemMeasure(int id)
        {
            var mapper = new ItemMeasureToItemMeasureViewModelMapper();
            return Ok(mapper.Map(_itemMeasureQueryProcessor.ActivateItemMeasure(id)));
        }

        [HttpPost]
        [Route("createitemmeasure")]
        public ObjectResult Create([FromBody] ItemMeasureViewModel itemMeasureViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.AddItemMeasure))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });
            var model = itemMeasureViewModel;
            var currentUserCompanyId = _userManager.FindByEmailAsync(User.Identity.Name).Result.CompanyId;
            model.CompanyId = currentUserCompanyId;
            if (_itemMeasureQueryProcessor.Exists(p => p.ProductId == model.ProductId && p.CompanyId == model.CompanyId))
            {
                return BadRequest(ItemMeasureConstants.ItemMeasureControllerConstants.ItemMeasureAlreadyExists);
            }
            try
            {
                var mapper = new ItemMeasureToItemMeasureViewModelMapper();
                var mappedItemMeasure = mapper.Map(itemMeasureViewModel);
                var savedItemMeasure = _itemMeasureQueryProcessor.Save(mappedItemMeasure);
                return Ok(savedItemMeasure);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.AddItemMeasure, ex, ex.Message);
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
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteItemMeasure))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            try
            {
                isDeleted = _itemMeasureQueryProcessor.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeleteItemMeasure, ex, ex.Message);
            }
            return Ok(isDeleted);
        }

        [HttpPut]
        [Route("updateitemmeasure")]
        public ObjectResult Put([FromBody] ItemMeasureViewModel itemMeasureViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.UpdateItemMeasure))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });
            var model = itemMeasureViewModel;
            if (_itemMeasureQueryProcessor.Exists(p => p.ProductId == model.ProductId && p.MeasureUnitId == model.MeasureUnitId && p.Id != model.Id && p.CompanyId == model.CompanyId))
            {
                return BadRequest(ItemMeasureConstants.ItemMeasureControllerConstants.ItemMeasureAlreadyExists);
            }
            var mapper = new ItemMeasureToItemMeasureViewModelMapper();
            var newItemMeasure = mapper.Map(itemMeasureViewModel);
            try
            {
                var updatedItemMeasure = _itemMeasureQueryProcessor.Update(newItemMeasure);
                itemMeasureViewModel = mapper.Map(updatedItemMeasure);
                return Ok(itemMeasureViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.UpdateItemMeasure, ex, ex.Message);
                ModelState.AddModelError("", ex.Message);
                return BadRequest(new
                {
                    errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                });
            }
        }

        [HttpGet]
        [Route("CheckIfDeletedItemMeasureWithSameProductIdExists/{productId}")]
        public ObjectResult CheckIfDeletedItemMeasureWithSameProductIdExists(int productId)
        {
            var itemMeasure = _itemMeasureQueryProcessor.CheckIfDeletedItemMeasureWithSameProductIdExists(productId);
            var itemMeasureMapper = new ItemMeasureToItemMeasureViewModelMapper();
            if (itemMeasure != null)
            {
                itemMeasureMapper.Map(itemMeasure);
            }
            return Ok(itemMeasure);
        }

        [HttpGet]
        [Route("searchItemMeasures/{active}/{searchText}")]
        public ObjectResult SearchItemMeasures(bool active, string searchText)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewItemMeasures))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            try
            {
                var mapper = new ItemMeasureToItemMeasureViewModelMapper();
                return Ok(_itemMeasureQueryProcessor.SearchItemMeasures(active, searchText).Select(x => mapper.Map(x)));
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewItemMeasures, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("bulkDelete")]
        public ObjectResult BulkDelete([FromBody] List<int?> itemMeasuresId)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteItemMeasure))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            if (itemMeasuresId == null || itemMeasuresId.Count <= 0) return Ok(false);
            try
            {
                isDeleted = _itemMeasureQueryProcessor.DeleteRange(itemMeasuresId);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeleteItemMeasure, ex, ex.Message);
            }
            return Ok(isDeleted);
        }
    }
}
