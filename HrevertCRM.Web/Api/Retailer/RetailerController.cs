using System;
using System.Collections.Generic;
using System.Linq;
using Hrevert.Common.Constants.Others;
using HrevertCRM.Data;
using HrevertCRM.Data.Mapper;
using HrevertCRM.Data.Mapper.Sales;
using HrevertCRM.Data.QueryProcessors;
using HrevertCRM.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HrevertCRM.Web.Api
{
    [Route("api/[controller]")]
    [Authorize]
    public class RetailerController : Controller
    {
        private readonly IRetailerQueryProcessor _retailerQueryProcessor;
        private readonly IDbContext _context;
        private readonly ILogger<RetailerController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISecurityQueryProcessor _securityQueryProcessor;
        private readonly IProductCategoryQueryProcessor _productCategoryQueryProcessor;
        private readonly ITaxQueryProcessor _taxQueryProcessor;
        private readonly IProductMetadataQueryProcessor _productMetadataQueryProcessor;
        private readonly IDeliveryRateQueryProcessor _deliveryRateQueryProcessor;
        private readonly IItemMeasureQueryProcessor _itemMeasureQueryProcessor;
        private readonly IProductQueryProcessor _productQueryProcessor;

        public RetailerController(IRetailerQueryProcessor retailerQueryProcessor,
            IDbContext context,
            ILoggerFactory factory,
            UserManager<ApplicationUser> userManager,
            ISecurityQueryProcessor securityQueryProcessor,
            IProductCategoryQueryProcessor productCategoryQueryProcessor,
            ITaxQueryProcessor taxQueryProcessor,
            IProductMetadataQueryProcessor productMetadataQueryProcessor,
            IDeliveryRateQueryProcessor deliveryRateQueryProcessor,
            IItemMeasureQueryProcessor itemMeasureQueryProcessor,
            IProductQueryProcessor productQueryProcessor
        )
        {
            _retailerQueryProcessor = retailerQueryProcessor;
            _context = context;
            _userManager = userManager;
            _securityQueryProcessor = securityQueryProcessor;
            _productCategoryQueryProcessor = productCategoryQueryProcessor;
            _taxQueryProcessor = taxQueryProcessor;
            _productMetadataQueryProcessor = productMetadataQueryProcessor;
            _deliveryRateQueryProcessor = deliveryRateQueryProcessor;
            _itemMeasureQueryProcessor = itemMeasureQueryProcessor;
            _productQueryProcessor = productQueryProcessor;
            _logger = factory.CreateLogger<RetailerController>();
        }

        [HttpPost]
        [Route("GetRetailers/{id}")]
        public ObjectResult GetRetailers(int id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewRetailers))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            try
            {
                var result = _retailerQueryProcessor.GetRetailers(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewRetailers, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("GetDistributors/{id}")]
        public ObjectResult GetDistributors(int id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewRetailers))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            try
            {
                var result = _retailerQueryProcessor.GetDistributors(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewRetailers, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("SaveRetailers")]
        public ObjectResult Create([FromBody] RetailerViewModel retailerViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.AddRetailer))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                });

            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    var currentUserCompanyId = _userManager.FindByEmailAsync(User.Identity.Name).Result.CompanyId;
                    retailerViewModel.CompanyId = currentUserCompanyId;
                    retailerViewModel.DistibutorId = currentUserCompanyId;

                    //Save Product In Categories
                    if (retailerViewModel.Retailers != null && retailerViewModel.Retailers.Count > 0)
                    {
                        DetermineRetailersToAdd(retailerViewModel);
                        SaveRetailers(retailerViewModel);
                    }

                    trans.Commit();
                    return Ok(retailerViewModel);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    _logger.LogCritical((int)SecurityId.AddRetailer, ex, ex.Message);
                    ModelState.AddModelError("", ex.Message);
                    return BadRequest(new
                    {
                        errors =
                        ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                    });
                }
            }
        }

        private void DetermineRetailersToAdd(RetailerViewModel retailerViewModel)
        {
            var existingRetailers =
                _retailerQueryProcessor.GetRetailers((int)retailerViewModel.DistibutorId);
            foreach (var retailerId in existingRetailers)
            {
                if (!retailerViewModel.Retailers.Contains(retailerId))
                {
                    //remove from db
                    _retailerQueryProcessor.Delete((int)retailerViewModel.DistibutorId, retailerId);
                }
                else
                {
                    //leave as it is
                    retailerViewModel.Retailers.Remove(retailerId);
                }
            }
        }

        private void SaveRetailers(RetailerViewModel retailerViewModel)
        {
            var currentUserCompanyId = _userManager.FindByEmailAsync(User.Identity.Name).Result.CompanyId;
            var retailersOfDistributor = new List<Retailer>();
            if (retailerViewModel.Retailers != null && retailerViewModel.Retailers.Count > 0)
            {
                foreach (var retailer in retailerViewModel.Retailers)
                {
                    retailersOfDistributor.Add(new Retailer
                    {
                        DistibutorId = currentUserCompanyId,
                        RetailerId = retailer
                    });
                }
            }
            _retailerQueryProcessor.SaveAllRetailers(retailersOfDistributor);
        }

        [HttpDelete("{id}")]
        public ObjectResult Delete(int id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteRetailer))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            var isDeleted = false;
            try
            {
                isDeleted = _retailerQueryProcessor.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeleteRetailer, ex, ex.Message);
            }
            return Ok(isDeleted);
        }

        [HttpPost]
        [Route("SyncNewProductsOnly/{id}")]
        public ObjectResult SyncNewProductsOnly(int id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.AddRetailer))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    var currentUserCompanyId = _userManager.FindByEmailAsync(User.Identity.Name).Result.CompanyId;
                    var categories = _productCategoryQueryProcessor.GetActiveProductCategories(id);
                    var productCategoryMapper = new ProductCategoryMapperToGetNewEntity();
                    var mappedCategories = productCategoryMapper.Map(categories.ToList());
                    var newCategories = productCategoryMapper.Map(mappedCategories);
                    newCategories.ForEach(x => x.CompanyId = currentUserCompanyId);
                    _productCategoryQueryProcessor.SaveAll(newCategories.ToList());

                    var taxes = _taxQueryProcessor.GetActiveTaxesWithoutPaging(id);
                    var taxMapper = new TaxMapperToGetNewEntity();
                    var mappedTaxes = taxMapper.Map(taxes.ToList());
                    var newTaxes = taxMapper.Map(mappedTaxes);
                    newTaxes.ForEach(x => x.CompanyId = currentUserCompanyId);
                    _taxQueryProcessor.SaveAll(newTaxes.ToList());

                    var productMetadatas = _productMetadataQueryProcessor.GetActiveProductMetadatas(id);
                    var productMetadataMapper = new ProductMetadataMapperToGetNewEntity();
                    var mappedProductMetadatas = productMetadataMapper.Map(productMetadatas.ToList());
                    var newProductMetadatas = productMetadataMapper.Map(mappedProductMetadatas);
                    newProductMetadatas.ForEach(x => x.CompanyId = currentUserCompanyId);
                    _productMetadataQueryProcessor.SaveAll(newProductMetadatas.ToList());

                    var deliveryRates = _deliveryRateQueryProcessor.GetActiveDeliveryRates(id);
                    var deliveryRateMapper = new DeliveryRateMapperToGetNewEntity();
                    var mappedDeliveryRates = deliveryRateMapper.Map(deliveryRates.ToList());
                    var newDeliveryRates = deliveryRateMapper.Map(mappedDeliveryRates);
                    newDeliveryRates.ForEach(x => x.CompanyId = currentUserCompanyId);
                    _deliveryRateQueryProcessor.SaveAllDeliveryRate(newDeliveryRates.ToList());

                    var itemMeasures = _itemMeasureQueryProcessor.GetActiveItemMeasures(id);
                    var itemMeasureMapper = new ItemMeasureMapperToGetNewEntity();
                    var mappedItemMeasures = itemMeasureMapper.Map(itemMeasures.ToList());
                    var newItemMeasures = itemMeasureMapper.Map(mappedItemMeasures);
                    newItemMeasures.ForEach(x => x.CompanyId = currentUserCompanyId);
                    _itemMeasureQueryProcessor.SaveAll(newItemMeasures.ToList());

                    var productsRefByKitAndAssembledTypes = _productQueryProcessor.GetProductsRefByKitAndAssembledTypes(id);
                    var productRefByKitAndAssembledTypeMapper = new ProductRefByKitAndAssembledTypeMapperToGetNewEntity();
                    var mappedProductRefByKitAndAssembledTypes = productRefByKitAndAssembledTypeMapper.Map(productsRefByKitAndAssembledTypes.ToList());
                    var newProductRefByKitAndAssembledTypes = productRefByKitAndAssembledTypeMapper.Map(mappedProductRefByKitAndAssembledTypes);
                    newProductRefByKitAndAssembledTypes.ForEach(x => x.CompanyId = currentUserCompanyId);
                    _productQueryProcessor.SaveAllRefProducts(newProductRefByKitAndAssembledTypes.ToList());

                    var products = _productQueryProcessor.GetActiveProducts(id);
                    var productMapper = new ProductMapperToGetNewEntity();
                    var mappedProducts = productMapper.Map(products.ToList());
                    var newProducts = productMapper.Map(mappedProducts);
                    newProducts.ForEach(x => x.CompanyId = currentUserCompanyId);
                    _productQueryProcessor.SaveAllProduct(newProducts.ToList());

                    trans.Commit();
                    return Ok("New Products Sync Finished");
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    _logger.LogCritical((int)SecurityId.AddRetailer, ex, ex.Message);
                    ModelState.AddModelError("", ex.Message);
                    return BadRequest(new
                    {
                        errors =
                        ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                    });
                }
            }
        }
    }
}
