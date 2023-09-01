using System;
using System.Collections.Generic;
using System.Linq;
using Hrevert.Common.Constants.Others;
using Hrevert.Common.Constants.Product;
using HrevertCRM.Data;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.DTO;
using HrevertCRM.Data.Mapper;
using Microsoft.AspNetCore.Mvc;
using HrevertCRM.Data.QueryProcessors;
using HrevertCRM.Data.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using HrevertCRM.Entities;

namespace HrevertCRM.Web.Api
{

    [Route("api/[controller]")]
    public class ProductCategoryController : Controller
    {
        private readonly IProductCategoryQueryProcessor _productCategoryQueryProcessor;
        private readonly ISecurityQueryProcessor _securityQueryProcessor;
        private readonly IProductQueryProcessor _productQueryProcessor;
        private readonly ILogger<ProductCategoryController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        public ProductCategoryController(IProductCategoryQueryProcessor productCategoryQueryProcessor,
            IDbContext context, IPagedDataRequestFactory pagedDataRequestFactory,
            ILoggerFactory factory, ISecurityQueryProcessor securityQueryProcessor,
            IProductQueryProcessor productQueryProcessor,
            UserManager<ApplicationUser> userManager)
        {
            _productCategoryQueryProcessor = productCategoryQueryProcessor;
            _securityQueryProcessor = securityQueryProcessor;
            _productQueryProcessor = productQueryProcessor;
            _userManager = userManager;
            _logger = factory.CreateLogger<ProductCategoryController>();

        }
        [HttpGet]
        [Route("getactivecategories")]
        public ObjectResult GetActiveProductCategories()
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewProductCategories))

                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            try
            {
                var mapper = new ProductCategoryToProductCategoryViewModelMapper();
                return Ok(_productCategoryQueryProcessor.GetActiveProductCategories().Select(c => mapper.Map(c)).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewProductCategories, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getdeletedcategories")]
        public ObjectResult GetDeletedProductCategories()
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewProductCategories))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            try
            {
                var mapper = new ProductCategoryToProductCategoryViewModelMapper();
                return Ok(_productCategoryQueryProcessor.GetDeletedProductCategories().Select(c => mapper.Map(c)).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewProductCategories, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getallcategories")]
        public ObjectResult GetAllProductCategories()
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewProductCategories))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            try
            {
                var mapper = new ProductCategoryToProductCategoryViewModelMapper();
                return Ok(_productCategoryQueryProcessor.GetAllProductCategories().Select(c => mapper.Map(c)).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewProductCategories, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("categorytree")]
        public ObjectResult GetCategoryTreeViewModel()
        {
            var catNodes = _productCategoryQueryProcessor.GetProductCategoriesInTree();
            var categoriesTreeViewModel = new List<ProductCategoryTreeViewModel>();
            var mapper = new ProductCategoryAndProductCategoryTreeViewModelMapper();

            foreach (var parent in catNodes)
            {
                var productCategory = parent.Source;
                var vm = mapper.Map(productCategory);
                categoriesTreeViewModel.Add(vm);
                WalkCategoryTree(vm, parent.Children);
            }

            return Ok(categoriesTreeViewModel);
        }

        [HttpGet]
        [Route("allcategorytree")]
        public ObjectResult GetAllCategoryTreeViewModel()
        {
            var catNodes = _productCategoryQueryProcessor.GetAllProductCategoriesInTree();
            var categoriesTreeViewModel = new List<ProductCategoryTreeViewModel>();
            var mapper = new ProductCategoryAndProductCategoryTreeViewModelMapper();

            foreach (var parent in catNodes)
            {
                var productCategory = parent.Source;
                var vm = mapper.Map(productCategory);
                categoriesTreeViewModel.Add(vm);
                WalkCategoryTree(vm, parent.Children);
            }

            return Ok(categoriesTreeViewModel);
        }

        [HttpPost]
        //[Route("categorytree")]
        public ObjectResult SaveCategoryTree([FromBody] List<ProductCategoryTreeViewModel> productCategoies)
        {
            short categoryRank = 1;
            var root = new ProductCategoryTreeViewModel();
            root.Children.AddRange(productCategoies);
            SaveCategoryTreeChildren(root, categoryRank);
            return Ok(categoryRank);
        }

        private void SaveCategoryTreeChildren(ProductCategoryTreeViewModel categoryTreeViewModel, short categoryRank)
        {
            var parentId = categoryTreeViewModel.Id == 0 ? null : categoryTreeViewModel.Id;
            var mapper = new ProductCategoryAndProductCategoryTreeViewModelMapper();
            foreach (var categoryVm in categoryTreeViewModel.Children)
            {
                categoryVm.ParentId = parentId;
                categoryVm.CategoryRank = categoryRank;
                var updatedCategory = mapper.Map(categoryVm);

                if (updatedCategory.Id == 0)
                    _productCategoryQueryProcessor.Save(updatedCategory);
                else
                    _productCategoryQueryProcessor.Update(updatedCategory);

                categoryRank++;
                SaveCategoryTreeChildren(categoryVm, categoryRank);
            }
        }

        private void WalkCategoryTree(ProductCategoryTreeViewModel categoryTreeViewModel, List<CategoryNode> parentChildren)
        {
            var mapper = new CategoryNodeAndProductCategoryTreeViewModelMapper();
            foreach (var categoryNode in parentChildren)
            {

                var vm = mapper.Map(categoryNode);
                categoryTreeViewModel.Children.Add(vm);
                WalkCategoryTree(vm, categoryNode.Children);
            }
        }

        [HttpGet]
        [Route("getproductcategory/{id}")]
        public ObjectResult Get(int id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewProductCategories))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            var mapper = new ProductCategoryToProductCategoryViewModelMapper();
            var productCategory = mapper.Map(_productCategoryQueryProcessor.GetProductCategory(id));
            return Ok(productCategory);
        }


        [HttpGet]
        [Route("getCategoriesByCategoryIdList")]
        public ObjectResult GetCategoriesByCategoryIdList([FromQuery] List<int> listOfCategoryId)
        {
            var mapper = new ProductCategoryToProductCategoryViewModelMapper();
            if (listOfCategoryId == null || listOfCategoryId.Count <= 0) return BadRequest("Category List Cannot be empty");
            var productCategories = listOfCategoryId.Select(id => _productCategoryQueryProcessor.GetProductCategory(id)).ToList();
            return Ok(mapper.Map(productCategories));
        }

        [HttpPost]
        [Route("createcategory")]
        public ObjectResult Create([FromBody] ProductCategoryViewModel productCategoryViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.AddProductCategory))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });
            var categoryImage = productCategoryViewModel.CategoryImage;
            var currentUserCompanyId = _userManager.FindByEmailAsync(User.Identity.Name).Result.CompanyId;
            productCategoryViewModel.CompanyId = currentUserCompanyId;
            productCategoryViewModel.Name = productCategoryViewModel.Name.Trim();
            if (_productCategoryQueryProcessor.Exists(p => p.Name == productCategoryViewModel.Name && p.CompanyId == productCategoryViewModel.CompanyId))
            {
                return BadRequest(ProductConstants.ProductCategoryControllerConstants.ProductCategoryExists);
            }

            var mapper = new ProductCategoryToProductCategoryViewModelMapper();
            var newProductCategory = mapper.Map(productCategoryViewModel);
            try
            {
                if (productCategoryViewModel.ParentId == null)
                {
                    newProductCategory.CategoryRank = (short)(_productCategoryQueryProcessor.GetLastCategoryRank() + 1);
                }
                if (categoryImage != null)
                {
                    var categoryImageUrl = _productCategoryQueryProcessor.SaveCategoryImage(categoryImage);
                    newProductCategory.CategoryImageUrl = categoryImageUrl;
                }
                var savedProductCategory = _productCategoryQueryProcessor.Save(newProductCategory);
                var mappedProductCategory = mapper.Map(savedProductCategory);
                return Ok(mappedProductCategory);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.AddProductCategory, ex, ex.Message);
                ModelState.AddModelError("", ex.Message);
                return BadRequest(new
                {
                    errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                });
            }
        }

        [HttpDelete()]
        [Route("deleteOnlyCategory/{id}")]
        public ObjectResult DeleteOnlyCategory(int id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteProductCategory))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            var isDeleted = false;
            try
            {
                isDeleted = _productCategoryQueryProcessor.DeleteOnlyCategory(id);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeleteProductCategory, ex, ex.Message);
            }
            return Ok(isDeleted);
        }

        [HttpDelete("{id}")]
        public ObjectResult Delete(int id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteProductCategory))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            var isDeleted = false;
            try
            {
                isDeleted = _productCategoryQueryProcessor.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.DeleteProductCategory, ex, ex.Message);
            }
            return Ok(isDeleted);
        }

        [HttpPut]
        [Route("updatecategory")]
        public ObjectResult Put([FromBody] ProductCategoryViewModel productCategoryViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.UpdateProductCategory))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });

            if (_productCategoryQueryProcessor.Exists(p => p.Name == productCategoryViewModel.Name && p.Id != productCategoryViewModel.Id  && p.ParentId == productCategoryViewModel.ParentId && p.CompanyId == productCategoryViewModel.CompanyId))
            {
                return BadRequest(ProductConstants.ProductCategoryControllerConstants.ProductCategoryExists);
            }

            var categoryImage = productCategoryViewModel.CategoryImage;
            var mapper = new ProductCategoryToProductCategoryViewModelMapper();
            var newProductCategory = mapper.Map(productCategoryViewModel);
            try
            {
                if (categoryImage == null) return Ok(_productCategoryQueryProcessor.Update(newProductCategory));
                var categoryImageUrl = _productCategoryQueryProcessor.SaveCategoryImage(categoryImage);
                newProductCategory.CategoryImageUrl = categoryImageUrl;
                return Ok(_productCategoryQueryProcessor.Update(newProductCategory));
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.UpdateProductCategory, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getcategories")]
        public ObjectResult GetFullProductCategories()
        {
            var result = _productCategoryQueryProcessor.GetFullProductCategories();
            return Ok(result);
        }

        [HttpGet]
        [Route("getcategorywithproducts/{categoryId}")]
        public ObjectResult GetCategoryWithProducts(int categoryId)
        {
            // There is a method in ProductController that does the same thing as this method
            // This method should be removed in time. 
            var result = _productCategoryQueryProcessor.GetCategoryWithProducts(categoryId);
            return Ok(result);
        }

        [HttpGet]
        [Route("getcategorywithchildren/{id}")]
        public ObjectResult GetCategoryWithChildren(int id)
        {
            var result = _productCategoryQueryProcessor.GetCategoryWithChildren(id);
            return Ok(result);
        }

        [HttpPost]
        [Route("getproductbycategoryidlevelandnoofproducts")]
        public ObjectResult GetProductByCategoryIdLevelAndNoofProducts([FromBody] ProductByCategoryIdLevelIdNoofProductsViewModel viewModel)
        {
            var result = _productCategoryQueryProcessor.GetProductByCategoryIdLevelAndNoofProduct(viewModel.CategoryId, viewModel.LevelId, viewModel.NoofProducts);
            return Ok(result);
        }

        [HttpPost]
        [Route("searchbycategoryIdAndText")]
        public ObjectResult Search([FromBody] ProductCategorySearchViewModel model)
        {
            if (model == null) return Ok("Please select a category or Enter text to search for products");
            var productMapper = new ProductToProductViewModelMapper();

            if ((model.CategoryId == null || model.CategoryId == 0) && !string.IsNullOrEmpty(model.Text))
            {
                return Ok(_productQueryProcessor.SearchActiveWithoutPaging(model.Text));
            }
            if (string.IsNullOrEmpty(model.Text) && model.CategoryId != null)
            {
                var result = _productQueryProcessor.GetProductsByCategoryId((int) model.CategoryId);
                return Ok(productMapper.Map(result));
            }
            if (string.IsNullOrEmpty(model.Text) || model.CategoryId == null || model.CategoryId == 0)
            {
                return Ok(_productQueryProcessor.GetProductsForStoreFront());
            }
            {
                var result = _productQueryProcessor.SearchProductByCategoryIdAndText(
                    (int) model.CategoryId, model.Text);
                return Ok(result);
            }
        }

        [HttpGet]
        [Route("getallparentcategories")]
        public ObjectResult GetAllParentProductCategories()
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewProductCategories))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            try
            {
                var mapper = new ProductCategoryToProductCategoryViewModelMapper();
                return Ok(_productCategoryQueryProcessor.GetAllParentProductCategories().Select(c => mapper.Map(c)).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewProductCategories, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getallactivecategories")]
        public ObjectResult GetAllActiveProductCategories()
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewProductCategories))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            try
            {
                var mapper = new ProductCategoryToProductCategoryViewModelMapper();
                return Ok(_productCategoryQueryProcessor.GetAllActiveProductCategories().Select(c => mapper.Map(c)).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewProductCategories, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("activatecategory/{categoryId}")]
        public ObjectResult ActivateProductCategory(int categoryId)
        {
            var category = _productCategoryQueryProcessor.ActivateProductCategory(categoryId);
            var mapper = new ProductCategoryToProductCategoryViewModelMapper();
            var mappedData = mapper.Map(category); 
            return Ok(mappedData);
        }


        [HttpGet]
        [Route("CheckIfDeletedProductCategoryWithSameNameExists/{name}")]
        public ObjectResult CheckIfDeletedProductCategoryWithSameNameExists(string name)
        {
            var productCategory = _productCategoryQueryProcessor.CheckIfDeletedProductCategoryWithSameNameExists(name);
            var productCategoryMapper = new ProductCategoryToProductCategoryViewModelMapper();
            if (productCategory != null)
            {
                productCategoryMapper.Map(productCategory);
            }
            return Ok(productCategory);
        }
        [HttpPost]
        [Route("moveToCategories")]
        public ObjectResult MoveToCategory([FromBody]List<ProductViewModel> prouductViewModel)
        {
            return null;
        }
    }
}
