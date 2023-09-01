using System;
using System.Collections.Generic;
using System.Linq;
using Hrevert.Common.Constants.Others;
using Hrevert.Common.Constants.Product;
using HrevertCRM.Data;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.Mapper;
using HrevertCRM.Data.QueryProcessors;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HrevertCRM.Web.Api
{
    [Route("api/[controller]")]
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductQueryProcessor _productQueryProcessor;
        private readonly IPagedDataRequestFactory _pagedDataRequestFactory;
        private readonly IUserSettingQueryProcessor _userSettingQueryProcessor;
        private readonly IProductMetadataQueryProcessor _productMetadataQueryProcessor;
        private readonly ISecurityQueryProcessor _securityQueryProcessor;
        private readonly ITaxesInProductQueryProcessor _taxesInProductQueryProcessor;
        private readonly IProductInCategoryQueryProcessor _productInCategoryQueryProcessor;
        private readonly IDbContext _context;
        private readonly ILogger<ProductController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProductController(IProductQueryProcessor productQueryProcessor,
            IProductInCategoryQueryProcessor productInCategoryQueryProcessor,
            IProductCategoryQueryProcessor productCategoryQueryProcessor,
            IDbContext context, ILoggerFactory factory, 
            IPagedDataRequestFactory pagedDataRequestFactory,
            IUserSettingQueryProcessor userSettingQueryProcessor,
            IAddressQueryProcessor addressQueryProcessor,
            IProductMetadataQueryProcessor productMetadataQueryProcessor,
            IHostingEnvironment env,
            ISecurityQueryProcessor securityQueryProcessor,
            ISalesOrderLineQueryProcessor salesOrderLineQueryProcessor,
            ITaxesInProductQueryProcessor taxesInProductQueryProcessor,
            UserManager<ApplicationUser> userManager
        )
        {
            _productQueryProcessor = productQueryProcessor;
            _productInCategoryQueryProcessor = productInCategoryQueryProcessor;
            _context = context;
            _pagedDataRequestFactory = pagedDataRequestFactory;
            _userSettingQueryProcessor = userSettingQueryProcessor;
            _productMetadataQueryProcessor = productMetadataQueryProcessor;
            _securityQueryProcessor = securityQueryProcessor;
            _taxesInProductQueryProcessor = taxesInProductQueryProcessor;
            _userManager = userManager;
            _logger = factory.CreateLogger<ProductController>();
        }

        [HttpGet]
        [Route("lastestproducts")]
        public ObjectResult GetLatestProductsSortedByDate()
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewProducts))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            try
            {
                var requestInfo = _pagedDataRequestFactory.Create(Request.GetUri());
                return Ok(_productQueryProcessor.GetLatestProductsBySortedByDate(requestInfo));
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewProducts, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getallactiveproducts")]
        public ObjectResult GetActiveProductsWithoutPaging()
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewProducts))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            try
            {
                var mapper = new ProductToProductViewModelMapper();
                var result = _productQueryProcessor.GetAllActiveProducts().Select(f => mapper.Map(f)).ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewProducts, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getProductsByCategoryId/{id}")]
        public List<ProductViewModel> GetProductsByCategoryId(int id)
        {
            var mapper = new ProductToProductViewModelMapper();
            var result = _productQueryProcessor.GetProductsByCategoryId(id).Select(x => mapper.Map(x)).ToList();
            return result;
        }

        [HttpGet]
        [Route("getProductsByCategoryIdList")]
        public ObjectResult GetProductsByCategoryIdList([FromQuery] List<int> listOfCategoryId)
        {
            var mapper = new ProductToProductViewModelMapper();
            var products = new List<Product>();
            if (listOfCategoryId == null || listOfCategoryId.Count <= 0) return BadRequest("Category List Cannot be empty");
            foreach (var id in listOfCategoryId)
            {
                var result = _productQueryProcessor.GetProductsByCategoryId(id).ToList();
                products.AddRange(result);
            }
            return Ok(mapper.Map(products));
        }

        [HttpGet]
        [Route("GetProduct/{id}")]
        public ObjectResult Get(int id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewProducts))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var result = _productQueryProcessor.GetProductViewModel(id);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetProductByIdForStore/{id}")]
        public ObjectResult GetProductByIdForStore(int id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewProducts))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var result = _productQueryProcessor.GetProductViewModelByIdForStore(id);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetProductForStoreFront/{customerId}/{productId}")]
        public ObjectResult Get(int customerId, int productId)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewProducts))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var result = _productQueryProcessor.GetProductViewModelForStoreFront(customerId, productId);
            return Ok(result.First());
        }

        [HttpGet]
        [Route("activateproduct/{id}")]
        public ObjectResult ActivateProduct(int id)
        {
            var mapper = new ProductToProductViewModelMapper();
            return Ok(mapper.Map(_productQueryProcessor.ActivateProduct(id)));
        }

        [HttpPost]
        [Route("createproduct")]
        public ObjectResult Create([FromBody] ProductViewModel productViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.AddProduct))
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
                    productViewModel.CompanyId = currentUserCompanyId;
                    productViewModel.Code = productViewModel.Code.Trim();
                    productViewModel.Name = productViewModel.Name.Trim();
                    productViewModel.ShortDescription = productViewModel.ShortDescription?.Trim();
                    productViewModel.LongDescription = productViewModel.LongDescription?.Trim();
                    var model = productViewModel;

                    if (_productQueryProcessor.Exists(p => p.Code == model.Code && p.CompanyId == model.CompanyId))
                        return BadRequest(ProductConstants.ProductControllerConstants.ProductCodeExists);

                    if (_productQueryProcessor.Exists(p => p.Name == model.Name && p.CompanyId == model.CompanyId))
                        return BadRequest(ProductConstants.ProductControllerConstants.ProductNameExists);

                    var mapper = new ProductToProductViewModelMapper();

                    var mappedProduct = mapper.Map(productViewModel);

                    //Save Product
                    var savedProduct = _productQueryProcessor.Save(mappedProduct);

                    //Save Product In Categories
                    productViewModel.Id = savedProduct.Id;
                    if (productViewModel.Categories != null && productViewModel.Categories.Count > 0)
                        SaveProductInCategories(productViewModel);

                    //Save Taxes In Products
                    if (productViewModel.Taxes !=null && productViewModel.Taxes.Count > 0)
                        SaveTaxesInProducts(productViewModel);

                    //Save Products Referenced By Kit and Assembled Types
                    if(productViewModel.ProductsReferencedByKitAndAssembledType != null && productViewModel.ProductsReferencedByKitAndAssembledType.Count > 0)                    
                        SaveProductsRefByKitAndAssembledType(productViewModel);

                    //Save Images
                    if (productViewModel.Images.Any())
                        SaveProductMetadatas(savedProduct.Id, productViewModel.Images);

                    trans.Commit();
                    productViewModel = mapper.Map(savedProduct);
                    return Ok(productViewModel);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    _logger.LogCritical((int)SecurityId.AddProduct, ex, ex.Message);
                    ModelState.AddModelError("", ex.Message);
                    return BadRequest(new
                    {
                        errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                    });
                }
            }
        }

        private void SaveTaxesInProducts(ProductViewModel productViewModel)
        {
            var taxesInProductList = new List<TaxesInProduct>();
            if (productViewModel.Taxes != null && productViewModel.Taxes.Count > 0)
            {
                taxesInProductList.AddRange(from taxId in productViewModel.Taxes
                    where productViewModel.Id != null
                    select new TaxesInProduct
                    {
                        ProductId = (int) productViewModel.Id,
                        TaxId = taxId
                    });
            }
            _taxesInProductQueryProcessor.SaveAllTaxesInProducts(taxesInProductList);
        }

        private void SaveProductsRefByKitAndAssembledType(ProductViewModel productViewModel)
        {
            var productsRefList = new List<ProductsRefByKitAndAssembledType>();
            if (productViewModel.ProductsReferencedByKitAndAssembledType != null && productViewModel.ProductsReferencedByKitAndAssembledType.Count > 0)
            {
                productsRefList.AddRange(from productId in productViewModel.ProductsReferencedByKitAndAssembledType
                    where productViewModel.Id != null
                    select new ProductsRefByKitAndAssembledType
                    {
                        ProductId = (int) productViewModel.Id,
                        ProductRefId = productId
                    });
            }
            _productQueryProcessor.SaveAllRefProducts(productsRefList);
        }

        private void SaveProductMetadatas(int productId, List<Image> images)
        {
            _productMetadataQueryProcessor.SaveProductMetadatas(productId, images);
        }

        private void SaveProductInCategories(ProductViewModel productViewModel)
        {                               
            var productInCategoriesList = new List<ProductInCategory>();
            if (productViewModel.Categories != null && productViewModel.Categories.Count > 0)
            {
                productInCategoriesList.AddRange(from categoryId in productViewModel.Categories
                    where productViewModel.Id != null
                    select new ProductInCategory
                    {
                        ProductId = (int) productViewModel.Id,
                        CategoryId = categoryId
                    });
            }
            _productInCategoryQueryProcessor.SaveAllProductInCategories(productInCategoriesList);
        }

        [HttpDelete("{id}")]
        public ObjectResult Delete(int id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteProduct))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            var isDeleted = false;
            try
            {
                isDeleted = _productQueryProcessor.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeleteProduct, ex, ex.Message);
            }
            return Ok(isDeleted);
        }

        [HttpDelete("deleteImage/{productId}/{imageUri}")]
        public ObjectResult DeleteProductImage(int productId, string imageUri)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteProduct))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            try
            {
                isDeleted = _productMetadataQueryProcessor.DeleteProductImage(productId, imageUri);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeleteProduct, ex, ex.Message);
            }
            return Ok(isDeleted);
        }

        [HttpPut]
        public ObjectResult Put([FromBody] ProductViewModel productViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.UpdateProduct))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    productViewModel.Code = productViewModel.Code.Trim();
                    productViewModel.Name = productViewModel.Name.Trim();
                    productViewModel.ShortDescription = productViewModel.ShortDescription?.Trim();
                    productViewModel.LongDescription = productViewModel.LongDescription?.Trim();
                    var model = productViewModel;
                    if (_productQueryProcessor.Exists(p => p.Name == model.Name && p.Id != model.Id && p.CompanyId == _productQueryProcessor.ActiveUser.CompanyId))
                        return BadRequest(ProductConstants.ProductControllerConstants.ProductCodeExists);
                    if (_productQueryProcessor.Exists(p => p.Code == model.Code && p.Id != model.Id
                                 && p.CompanyId == _productQueryProcessor.ActiveUser.CompanyId))
                        return BadRequest(ProductConstants.ProductControllerConstants.ProductNameExists);

                    //This method deals with update of categoryId's in Product In Categories
                    DetermineCategoriesToAdd(productViewModel);
                    //This method saves the ProductId and CategoryId in Product In Categories
                    SaveProductInCategories(productViewModel);

                    //This method deals with the update of taxId's in Taxes In Products
                    DetermineTaxesToAdd(productViewModel);
                    //This method saves the ProductId and TaxId in Taxes In Products
                    SaveTaxesInProducts(productViewModel);

                    DetermineRefToAdd(productViewModel);
                    SaveProductsRefByKitAndAssembledType(productViewModel);

                    var mapper = new ProductToProductViewModelMapper();
                    var newProduct = mapper.Map(productViewModel);
                    var updatedProduct = _productQueryProcessor.Update(newProduct);
                    
                    //Save Images
                    if (productViewModel.Images.Any())
                    {
                        DetermineImagesToAdd(productViewModel);
                        SaveProductMetadatas(updatedProduct.Id, productViewModel.Images);
                    }

                    transaction.Commit();
                    productViewModel = mapper.Map(updatedProduct);
                    return Ok(productViewModel);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _logger.LogCritical((int)SecurityId.UpdateProduct, ex, ex.Message);
                    ModelState.AddModelError("", ex.Message);
                    return BadRequest(new
                    {
                        errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                    });
                }
            }
        }

        private void DetermineTaxesToAdd(ProductViewModel productViewModel)
        {
            if (productViewModel.Id == null) return;
            var existingTaxes = _taxesInProductQueryProcessor.GetExistingTaxesOfProduct((int) productViewModel.Id);
            foreach (var existingTaxId in existingTaxes)
            {
                if (!productViewModel.Taxes.Contains(existingTaxId))
                    _taxesInProductQueryProcessor.Delete((int) productViewModel.Id, existingTaxId);
                else
                    productViewModel.Taxes.Remove(existingTaxId);
            }
        }

        private void DetermineRefToAdd(ProductViewModel productViewModel)
        {
            if (productViewModel.Id == null) return;
            var existingReferences =
                _productQueryProcessor.GetExistingreferencesOfProduct((int)productViewModel.Id);

            foreach (var referenceProductId in existingReferences)
            {
                if (!productViewModel.ProductsReferencedByKitAndAssembledType.Contains(referenceProductId))
                    _productQueryProcessor.DeleteRefOfKitAndAssembledType((int)productViewModel.Id, referenceProductId);
                else
                    productViewModel.ProductsReferencedByKitAndAssembledType.Remove(referenceProductId);
            }
        }

        private void DetermineCategoriesToAdd(ProductViewModel productViewModel)
        {
            if (productViewModel.Id == null) return;
            var existingCategories =
                _productInCategoryQueryProcessor.GetExistingCategoriesOfProduct((int)productViewModel.Id);
            foreach (var categoryId in existingCategories)
            {
                if (!productViewModel.Categories.Contains(categoryId))
                    _productInCategoryQueryProcessor.Delete((int)productViewModel.Id, categoryId);
                else
                    productViewModel.Categories.Remove(categoryId);
            }
        }

        private void DetermineImagesToAdd(ProductViewModel productViewModel)
        {
            if (productViewModel.Id == null) return;
            var existingImageUrls =
                _productMetadataQueryProcessor.GetProductMetadatasByProductId((int)productViewModel.Id).ToList();
            var imageurlList = new List<string>();
            foreach (var existingImageUrl in existingImageUrls)
            {
                var value = existingImageUrl.Substring(existingImageUrl.LastIndexOf('/') + 1);
                imageurlList.Add(productViewModel.Name + "_" + value);
            }
            foreach (var imageUrl in imageurlList.Distinct())
            {
                if (productViewModel.Images.Select(x => x.FileName).Any(x => x.Equals(imageUrl)))
                    productViewModel.Images.Remove(productViewModel.Images.FirstOrDefault(x => x.FileName == imageUrl));
                else
                    _productMetadataQueryProcessor.DeleteProductImage((int)productViewModel.Id, imageUrl);
            }
        }


        [HttpGet]
        [Route("getstorefrontproducts")]
        public ObjectResult GetProductsForStoreFront()
        {
            var result = _productQueryProcessor.GetProductsForStoreFront();
            return Ok(result);
        }

        [HttpGet]
        [Route("GetProductsForStore/{customerId}/{categoryId}/{level}")]
        public ObjectResult GetProductsWithDiscountForStore(int customerId, int categoryId, int level)
        {
            var productViewModels = _productQueryProcessor.GetProductsByCategoryIdAndLevel(categoryId, level);
            return Ok(productViewModels);
        }

        [HttpGet]
        [Route("SearchProductByCategoryIdAndText/{id}/{text}")]
        public ObjectResult SearchProductByCategoryIdAndText(int id, string text)
        {
            var result = _productQueryProcessor.SearchProductByCategoryIdAndText(id, text);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetRegularProductsOnly")]
        public ObjectResult GetRegularItems()
        {
            var result = _productQueryProcessor.GetRegularItemsOnly();
            return Ok(result);
        }

        [HttpGet]
        [Route("CheckIfDeletedProducWithSameNameExists/{name}")]
        public ObjectResult CheckIfDeletedProductWithSameNameExists(string name)
        {
            var product = _productQueryProcessor.CheckIfDeletedProductWithSameNameExists(name);
            var productMapper = new ProductToProductViewModelMapper();
            if (product != null)
                productMapper.Map(product);
            return Ok(product);
        }

        [HttpGet]
        [Route("CheckIfDeletedProducWithSameCodeExists/{code}")]
        public ObjectResult CheckIfDeletedProductWithSameCodeExists(string code)
        {
            var product = _productQueryProcessor.CheckIfDeletedProductWithSameCodeExists(code);
            var productMapper = new ProductToProductViewModelMapper();
            if (product != null)
                productMapper.Map(product);
            return Ok(product);
        }

        [HttpGet]
        [Route("GetPageSize")]
        public ObjectResult GetPageSize()
        {
            var pageSize = _userSettingQueryProcessor.GetPageSize(EntityId.Product);
            return Ok(pageSize);
        }

        [HttpGet]
        [Route("searchProducts")]
        public ObjectResult SearchProducts()
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewProducts))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            try
            {
                var requestInfo = _pagedDataRequestFactory.CreateWithSearchStringCompanyActive(Request.GetUri());
                requestInfo.PageSize = _userSettingQueryProcessor.ForPaging(requestInfo, EntityId.Product);
                var result = _productQueryProcessor.SearchProducts(requestInfo);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewProducts, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("bulkDelete")]
        public ObjectResult BulkDelete([FromBody] List<int?> productsId)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteProduct))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            if (productsId == null || productsId.Count <= 0) return Ok(false);
            var isDeleted = false;
            try
            {
                isDeleted = _productQueryProcessor.DeleteRange(productsId);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeleteProduct, ex, ex.Message);
            }
            return Ok(isDeleted);
        }

        [HttpPost]
        [Route("assignToCategory")]
        public ObjectResult AssignToCategory([FromBody] List<ProductViewModel> productsList)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteProduct))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            if (productsList == null || productsList.Count <= 0) return Ok(false);
            try
            {
                foreach (var productViewModel in productsList)
                {
                    if (productViewModel.NewlyAssignedCats == null ||
                        productViewModel.NewlyAssignedCats.Count <= 0) continue;
                    foreach (var newlyAssignedCat in productViewModel.NewlyAssignedCats)
                    {
                        if (!productViewModel.Categories.Contains(newlyAssignedCat))
                            productViewModel.Categories.Add(newlyAssignedCat);
                    }
                    DetermineCategoriesToAdd(productViewModel);
                    SaveProductInCategories(productViewModel);
                }
                return Ok(productsList);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeleteProduct, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
