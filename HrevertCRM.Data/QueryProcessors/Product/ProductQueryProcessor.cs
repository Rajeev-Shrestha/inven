using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Hrevert.Common.Constants.Product;
using Hrevert.Common.Enums;
using Hrevert.Common.Security;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.Mapper;
using HrevertCRM.Data.Mapper.Product;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore;
using PagedTaskDataInquiryResponse = HrevertCRM.Data.Common.PagedDataInquiryResponse<HrevertCRM.Data.ViewModels.ProductViewModel>;

namespace HrevertCRM.Data.QueryProcessors
{
    public class ProductQueryProcessor : QueryBase<Product>, IProductQueryProcessor
    {
        private readonly ICompanyWebSettingQueryProcessor _companyWebSettingQueryProcessor;

        public ProductQueryProcessor(IUserSession userSession, IDbContext dbContext,
            ICompanyWebSettingQueryProcessor companyWebSettingQueryProcessor)
            : base(userSession, dbContext)
        {
            _companyWebSettingQueryProcessor = companyWebSettingQueryProcessor;
        }
        public Product Update(Product product)
        {
            var original = GetValidProduct(product.Id);
            ValidateAuthorization(original);
            CheckVersionMismatch(product, original);
            //pass value to original
            original.Code = product.Code;
            original.Name = product.Name;
            original.UnitPrice = product.UnitPrice;
            original.ShortDescription = product.ShortDescription;
            original.LongDescription = product.LongDescription;
            original.QuantityOnHand = product.QuantityOnHand;
            original.QuantityOnOrder = product.QuantityOnOrder;
            original.CompanyId = LoggedInUser.CompanyId;
            original.Active = product.Active;
            original.Commissionable = product.Commissionable;
            original.CommissionRate = product.CommissionRate;
            original.WebActive = product.WebActive;
            original.ProductType = product.ProductType;
            original.AllowBackOrder = product.AllowBackOrder;

            //validProduct.DateModified = DateTime.Now;
            _dbContext.Set<Product>().Update(original);
            var newUpdatedCategory =
                _dbContext
                    .Set<Product>()
                    .Include(x => x.ProductInCategories)
                    .FirstOrDefault(x => x.Id == original.Id);

            _dbContext.SaveChanges();
            return newUpdatedCategory;
        }
        public virtual Product GetValidProduct(int productId)
        {
            var product = _dbContext.Set<Product>().FirstOrDefault(sc => sc.Id == productId);
            if (product == null)
            {
                throw new RootObjectNotFoundException(ProductConstants.ProductQueryProcessorConstants.ProductNotFound);
            }
            return product;
        }

        public Product GetProduct(int productId)
        {
            var product = _dbContext.Set<Product>().Include(x => x.ProductMetadatas).Include(x => x.TaxesInProducts).FirstOrDefault(d => d.Id == productId);
            return product;
        }

        public ProductViewModel GetProductViewModel(int productId)
        {
            //This mapper is for sending productmetadatas without medium images
            var mapper = new ProductToProductViewModelMapper();
            var product = _dbContext.Set<Product>()
                .Include(pc => pc.ProductInCategories).
                Include(x => x.ProductMetadatas).
                Include(x => x.ProductsReferencedByKitAndAssembledTypes).
                Include(x => x.TaxesInProducts).FirstOrDefault(d => d.Id == productId && d.CompanyId == LoggedInUser.CompanyId);
            var metadatas = product.ProductMetadatas;
            product.ProductMetadatas = null;
            product.ProductMetadatas = metadatas.Where(x => x.CompanyId == LoggedInUser.CompanyId && x.Active).ToList();
            return mapper.Map(product);
        }

        public void SaveAllProduct(List<Product> products)
        {
            //This method is for seeding
            _dbContext.Set<Product>().AddRange(products);
            _dbContext.SaveChanges();
        }

        public int GetCompanyIdByProductId(int productId)
        {
            return _dbContext.Set<Product>().Where(p => p.Id == productId).Select(p => p.CompanyId).Single();

        }

        public Product Save(Product product)
        {
            product.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<Product>().Add(product);
            _dbContext.SaveChanges();
            return product;
        }

        public List<Product> GetProductsByCategoryId(int productCategoryId)
        {
            var productCategories = _dbContext.Set<ProductCategory>().Where(d => d.ParentId == productCategoryId && d.CompanyId == LoggedInUser.CompanyId && d.Active).Select(x => x.Id).ToList();
            productCategories.Add(productCategoryId);
            var productIdList = new List<int>();
            if (productCategories.Count == 0)
            {
                productIdList.AddRange(_dbContext.Set<ProductInCategory>().Where(x => x.CategoryId == productCategoryId && x.CompanyId == LoggedInUser.CompanyId && x.Active).Select(x => x.ProductId).Distinct().ToList());
            }
            else
            {
                foreach (var category in productCategories)
                {
                    productIdList.AddRange(_dbContext.Set<ProductInCategory>().Where(x => x.CategoryId == category && x.CompanyId == LoggedInUser.CompanyId && x.Active).Select(x => x.ProductId).Distinct().ToList());
                }
            }

            return productIdList.Select(productId => _dbContext.Set<Product>().Include(x => x.ProductMetadatas).SingleOrDefault(x => x.Id == productId && x.CompanyId == LoggedInUser.CompanyId && x.Active)).ToList();
        }

        public IQueryable<Product> GetAllActiveProducts()
        {
            return _dbContext.Set<Product>().Where(p => p.Active && p.CompanyId == LoggedInUser.CompanyId);
        }

        public Product CheckIfProductExistsOrSave(Product product)
        {
            var product2 = product;
            product = _dbContext.Set<Product>().Any(row => row.Code == product2.Code) ? _dbContext.Set<Product>().FirstOrDefault(row => row.Code == product2.Code) : Save(product);
            return product;
        }

        public bool Delete(int productId)
        {
            var doc = GetProduct(productId);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<Product>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }

        public bool DeleteRange(List<int?> productsId)
        {
            var productsList = productsId.Select(productId => _dbContext.Set<Product>().FirstOrDefault(x => x.Id == productId && x.CompanyId == LoggedInUser.CompanyId)).ToList();
            productsList.ForEach(x => x.Active = false);
            _dbContext.Set<Product>().UpdateRange(productsList);
            _dbContext.SaveChanges();
            return true;
        }

        public bool Exists(Expression<Func<Product, bool>> where)
        {
            return _dbContext.Set<Product>().Any(where);
        }

        public IQueryable<Product> GetActiveProducts()
        {
            var products = _dbContext.Set<Product>().Include(a => a.ProductInCategories).ThenInclude(t => t.ProductCategory).Where(p => p.Active && p.CompanyId == LoggedInUser.CompanyId);
            return products;
        }
        public IQueryable<Product> GetDeletedProducts()
        {
            return _dbContext.Set<Product>().Include(p => p.ProductInCategories).Where(p => p.Active == false && p.CompanyId == LoggedInUser.CompanyId);
        }

        private PagedTaskDataInquiryResponse FormatResultForPaging(PagedDataRequest requestInfo, IQueryable<Product> query)
        {
            var totalItemCount = query.Count();
            var startIndex = ResultsPagingUtility.CalculateStartIndex(requestInfo.PageNumber, requestInfo.PageSize);
            var mapper = new ProductToProductViewModelMapper();
            var docs = query.OrderBy(x => x.DateCreated).Skip(startIndex).Take(requestInfo.PageSize).Select(
                s => mapper.Map(s)).ToList();

            var docsWithDiscountsAndTaxes = GetProductsWithDiscount(0, docs);

            var queryResult = new QueryResult<ProductViewModel>(docsWithDiscountsAndTaxes, totalItemCount, requestInfo.PageSize);
            var inquiryResponse = new PagedTaskDataInquiryResponse
            {
                Items = docs,
                TotalRecords = totalItemCount,
                PageCount = queryResult.TotalPageCount,
                PageNumber = requestInfo.PageNumber,
                PageSize = requestInfo.PageSize
            };

            return inquiryResponse;
        }

        public Product ActivateProduct(int id)
        {
            var original = GetValidProduct(id);
            ValidateAuthorization(original);

            original.Active = true;
            _dbContext.Set<Product>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        #region Get Products By CategoryId and Level : For StoreFront
        public List<ProductViewModel> GetProductsByCategoryIdAndLevel(int categoryId, int level)
        {
            var categoriesList = new List<int> { categoryId };
            var existingCat = new List<int> { categoryId };
            var categories = new List<int>();
            var productsList = new List<Product>();
            if (level == 0)
            {
                while (true)
                {
                    categories.Clear();
                    foreach (var catId in existingCat)
                    {
                        categories.AddRange(_dbContext.Set<ProductCategory>().Where(x => x.ParentId == catId && x.CompanyId == LoggedInUser.CompanyId && x.Active).Select(x => x.Id).ToList());
                    }
                    if (!categories.Except(categoriesList).Any())
                        break;

                    existingCat = categories.Except(existingCat).ToList();
                    categoriesList.AddRange(existingCat);
                }
            }
            else if (level > 0)
            {
                while (level > 0)
                {
                    categories.Clear();
                    foreach (var catId in existingCat)
                    {
                        categories.AddRange(_dbContext.Set<ProductCategory>().Where(x => x.ParentId == catId && x.CompanyId == LoggedInUser.CompanyId && x.Active).Select(x => x.Id).ToList());
                    }
                    existingCat = categories.Except(existingCat).ToList();
                    categoriesList.AddRange(existingCat);
                    level--;
                }
            }
            foreach (var catId in categoriesList.Distinct())
            {
                productsList.AddRange(GetProductsByCategoryId(catId).Distinct());
            }
            var mapper = new ProductToProductViewModelMapper();
            var productsViewModels = productsList.Distinct().Where(x => x != null).Select(x => mapper.Map(x));
            return productsViewModels.ToList();
        }
        #endregion

        #region Calcualte Discount On Products By Product, Category, Customer and Customer Level

        public List<ProductViewModel> GetProductsWithDiscount(int customerId, List<ProductViewModel> productViewModels)
        {
            var productViewModelList = new List<ProductViewModel>();
            var discountCalculationType = _companyWebSettingQueryProcessor.Get(LoggedInUser.CompanyId).DiscountCalculationType;
            var compareDiscountForDifferentType = new List<double>();
            foreach (var productViewModel in productViewModels)
            {
                var productWiseDiscount = CalculateDiscountByProduct(productViewModel);
                var categoryWiseDiscount = CalculateDiscountByProductCategory(discountCalculationType, productViewModel);
                var customerWiseDiscount = 0.0;
                var customerLevelWiseDiscount = 0.0;
                if (customerId != 0)
                {
                    customerWiseDiscount = CalculateDiscountByCustomer(customerId, productViewModel);
                    customerLevelWiseDiscount = CalculateDiscountByCustomerLevel(customerId, discountCalculationType, productViewModel);
                }
                compareDiscountForDifferentType.Add(productWiseDiscount);
                compareDiscountForDifferentType.Add(categoryWiseDiscount);
                compareDiscountForDifferentType.Add(customerWiseDiscount);
                compareDiscountForDifferentType.Add(customerLevelWiseDiscount);
                // Find the maximum or minimum of Discount
                switch (discountCalculationType)
                {
                    case DiscountCalculationType.Minimum:
                        var minDiscount = compareDiscountForDifferentType.Min();
                        productViewModel.DiscountPrice = minDiscount;
                        break;
                    case DiscountCalculationType.Maximum:
                        var maxDiscount = compareDiscountForDifferentType.Max();
                        productViewModel.DiscountPrice = maxDiscount;
                        break;
                    default:
                        productViewModel.DiscountPrice = 0.0;
                        break;
                }
                productViewModelList.Add(productViewModel);
            }
            GetTaxesOfProducts(productViewModelList);
            return productViewModelList;
        }

        private void GetTaxesOfProducts(List<ProductViewModel> productViewModelList)
        {
            if (productViewModelList == null || productViewModelList.Count <= 0) return;
            foreach (var productVm in productViewModelList)
            {
                switch (productVm.ProductType)
                {
                    case ProductType.Assembled:
                        {
                            productVm.ProductsRefByAssembledAndKit = new List<ProductViewModel>();
                            if (productVm.ProductsReferencedByKitAndAssembledType == null) break;
                            foreach (var productId in productVm.ProductsReferencedByKitAndAssembledType)
                            {
                                var product = GetProductViewModel(productId);
                                if (product == null) continue;
                                productVm.ProductsRefByAssembledAndKit.Add(product);
                            }
                            foreach (var productViewModel in productVm.ProductsRefByAssembledAndKit)
                            {
                                productViewModel.Name = productVm.Name;
                                productViewModel.Code = productVm.Code;
                                productViewModel.ProductType = productVm.ProductType;
                                if (productVm.TaxesInProducts != null)
                                    productVm.TaxViewModelList = GetTaxViewModels(productVm.TaxesInProducts.Select(x => x.TaxId).ToList());
                                else if (productVm.Taxes != null)
                                    productVm.TaxViewModelList = GetTaxViewModels(productVm.Taxes);
                                if (productViewModel.TaxViewModelList == null) continue;
                                {
                                    productViewModel.TaxAndAmounts = new List<TaxNameAndAmountList>();
                                    foreach (var taxViewModel in productViewModel.TaxViewModelList)
                                    {
                                        productViewModel.TaxAndAmounts.Add(new TaxNameAndAmountList
                                        {
                                            TaxName = taxViewModel.TaxCode,
                                            TaxAmount = taxViewModel.TaxType == TaxCaculationType.Fixed ? taxViewModel.TaxRate : (taxViewModel.TaxRate * (decimal)productVm.UnitPrice / 100)
                                        });
                                    }
                                }
                            }
                            break;
                        }
                    case ProductType.Kit:
                        {
                            productVm.ProductsRefByAssembledAndKit = new List<ProductViewModel>();
                            if (productVm.ProductsReferencedByKitAndAssembledType == null) break;
                            foreach (var productId in productVm.ProductsReferencedByKitAndAssembledType)
                            {
                                var product = GetProductViewModel(productId);
                                if (product == null) continue;
                                productVm.ProductsRefByAssembledAndKit.Add(product);
                            }
                            foreach (var productViewModel in productVm.ProductsRefByAssembledAndKit)
                            {
                                productViewModel.Name = productVm.Name;
                                productViewModel.Code = productVm.Code;
                                productViewModel.ProductType = productVm.ProductType;
                                if (productVm.TaxesInProducts != null)
                                    productVm.TaxViewModelList = GetTaxViewModels(productVm.TaxesInProducts.Select(x => x.TaxId).ToList());
                                else if (productVm.Taxes != null)
                                    productVm.TaxViewModelList = GetTaxViewModels(productVm.Taxes);
                                if (productViewModel.TaxViewModelList == null) continue;
                                {
                                    productViewModel.TaxAndAmounts = new List<TaxNameAndAmountList>();
                                    foreach (var taxViewModel in productViewModel.TaxViewModelList)
                                    {
                                        productViewModel.TaxAndAmounts.Add(new TaxNameAndAmountList
                                        {
                                            TaxName = taxViewModel.TaxCode,
                                            TaxAmount = taxViewModel.TaxType == TaxCaculationType.Fixed ? taxViewModel.TaxRate : (taxViewModel.TaxRate * (decimal)productVm.UnitPrice / 100)
                                        });
                                    }
                                }
                            }
                            break;
                        }
                    case ProductType.Regular:
                        {
                            if (productVm.TaxesInProducts != null)
                                productVm.TaxViewModelList = GetTaxViewModels(productVm.TaxesInProducts.Select(x => x.TaxId).ToList());
                            else if (productVm.Taxes != null)
                                productVm.TaxViewModelList = GetTaxViewModels(productVm.Taxes);
                            if (productVm.TaxViewModelList == null) break;

                            productVm.TaxAndAmounts = new List<TaxNameAndAmountList>();
                            foreach (var taxViewModel in productVm.TaxViewModelList)
                            {
                                productVm.TaxAndAmounts.Add(new TaxNameAndAmountList
                                {
                                    TaxName = taxViewModel.TaxCode,
                                    TaxAmount = taxViewModel.TaxType == TaxCaculationType.Fixed ? taxViewModel.TaxRate : (taxViewModel.TaxRate * (decimal)productVm.UnitPrice / 100)
                                });
                            }
                            break;
                        }
                    default:
                        // Yaha samma na aaune parne ho
                        throw new ArgumentOutOfRangeException("Product Type Error");
                }
            }
        }

        private List<ProductViewModel> GetProductsRefByAssembledAndKit(ProductViewModel productViewModel)
        {
            var mapper = new ProductToProductViewModelMapper();
            var refProductsViewModelList = new List<ProductViewModel>();
            var refProductIdList =
                        _dbContext.Set<ProductsRefByKitAndAssembledType>()
                            .Where(x => x.ProductId == productViewModel.Id)
                            .Select(x => x.ProductRefId);
            foreach (var productId in refProductIdList)
            {
                var product = _dbContext.Set<Product>().Include(x => x.ProductMetadatas).FirstOrDefault(x => x.Id == productId);
                if (product != null)
                {
                    refProductsViewModelList.Add(mapper.Map(product));
                }
            }
            return refProductsViewModelList;
        }

        public IQueryable<ProductsRefByKitAndAssembledType> GetProductsRefByKitAndAssembledTypes(int distributorId)
        {
            return
                _dbContext.Set<ProductsRefByKitAndAssembledType>().Where(x => x.CompanyId == distributorId && x.Active);
        }

        public IQueryable<Product> GetActiveProducts(int distributorId)
        {
            return
               _dbContext.Set<Product>().Where(x => x.CompanyId == distributorId && x.Active);
        }

        public List<ProductViewModel> SearchActiveWithoutPaging(string searchText)
        {
            var products = _dbContext.Set<Product>().Include(x => x.ProductMetadatas).Where(FilterByActiveTrueAndCompany).ToList();
            var filteredProducts = string.IsNullOrEmpty(searchText) ? products : products.Where(s
                                                                  => s.Code.ToUpper().Contains(searchText.ToUpper())
                                                                  || s.Name.ToUpper().Contains(searchText.ToUpper())).ToList();
            var productMapper = new ProductToProductViewModelMapper();
            return productMapper.Map(filteredProducts);
        }

        public ProductViewModelForStore GetProductViewModelByIdForStore(int id)
        {
            var mapper = new ProductToProductViewModelForStoreMapper();
            var product = _dbContext.Set<Product>()
                .Include(pc => pc.ProductInCategories).
                Include(x => x.ProductMetadatas).
                Include(x => x.ProductsReferencedByKitAndAssembledTypes).
                Include(x => x.TaxesInProducts).FirstOrDefault(d => d.Id == id && d.CompanyId == LoggedInUser.CompanyId);
            var metadatas = product.ProductMetadatas;
            product.ProductMetadatas = null;
            product.ProductMetadatas = metadatas.Where(x => x.CompanyId == LoggedInUser.CompanyId && x.Active).ToList();
            return mapper.Map(product);
        }

        private List<TaxViewModel> GetTaxViewModels(List<int> taxIds)
        {
            var taxMapper = new TaxToTaxViewModelMapper();
            var taxViewModels = new List<TaxViewModel>();
            foreach (var taxId in taxIds)
            {
                var tax =
                    _dbContext.Set<Tax>().FirstOrDefault(x => x.Active && x.CompanyId == LoggedInUser.CompanyId && x.Id == taxId);
                if (tax == null) continue;
                taxViewModels.Add(taxMapper.Map(tax));
            }
            return taxViewModels;
        }

        public List<ProductViewModel> SearchProductByCategoryIdAndText(int categoryId, string text)
        {
            var categoriesList = new List<int> { categoryId };
            var existingCat = new List<int> { categoryId };
            var categories = new List<int>();
            var productsList = new List<Product>();

            if (categoryId == 0)
            {
                productsList = _dbContext.Set<Product>().Include(x => x.ProductMetadatas).Where(x => x.CompanyId == LoggedInUser.CompanyId && x.Active).ToList();
            }
            else
            {
                while (true)
                {
                    categories.Clear();
                    foreach (var catId in existingCat)
                    {
                        categories.AddRange(_dbContext.Set<ProductCategory>().Where(x => x.ParentId == catId && x.CompanyId == LoggedInUser.CompanyId && x.Active).Select(x => x.Id).ToList());
                    }
                    if (!categories.Except(categoriesList).Any())
                        break;

                    existingCat = categories.Except(existingCat).ToList();
                    categoriesList.AddRange(existingCat);
                }
                foreach (var catId in categoriesList.Distinct())
                {
                    productsList.AddRange(GetProductsByCategoryId(catId).Distinct());
                }
            }
            var searchResultProductsList = productsList.Distinct().Where(product => product.Code.ToUpper().Contains(text.ToUpper()) || product.Name.ToUpper().Contains(text.ToUpper())).ToList();
            var mapper = new ProductToProductViewModelMapper();
            var productsViewModels = searchResultProductsList.Distinct().Select(x => mapper.Map(x));
            return productsViewModels.ToList();
        }

        public PagedTaskDataInquiryResponse GetLatestProductsBySortedByDate(PagedDataRequest requestInfo, Expression<Func<Product, bool>> @where = null)
        {
            var products = _dbContext.Set<Product>().Where(x => x.CompanyId == LoggedInUser.CompanyId && x.Active && x.WebActive)
                .OrderByDescending(x => x.DateCreated).Take(5).ToList();
            foreach (var product in products)
            {
                var metadatas = _dbContext.Set<ProductMetadata>()
                        .Where(
                            x =>
                                x.Active && x.WebActive && x.CompanyId == LoggedInUser.CompanyId &&
                                x.ProductId == product.Id).ToList();
                product.ProductMetadatas = new List<ProductMetadata>();
                product.ProductMetadatas = metadatas;
            }
            var totalItemCount = products.Count;

            //var startIndex = ResultsPagingUtility.CalculateStartIndex(requestInfo.PageNumber, requestInfo.PageSize);
            var mapper = new ProductToProductViewModelMapper();
            var docs = mapper.Map(products);
            var queryResult = new QueryResult<ProductViewModel>(docs, totalItemCount, requestInfo.PageSize);
            var inquiryResponse = new PagedTaskDataInquiryResponse
            {
                Items = docs.ToList(),
                TotalRecords = totalItemCount,
                PageCount = queryResult.TotalPageCount,
                PageNumber = requestInfo.PageNumber,
                PageSize = requestInfo.PageSize
            };

            return inquiryResponse;
        }

        public List<ProductViewModel> GetProductViewModelForStoreFront(int customerId, int productId)
        {
            var productList = new List<ProductViewModel>();
            // This mapper is for sending productmetadatas without medium images
            // productList kina cha vanda kheri Assembled ra Kit products ko ek vanda badhi products haru huna sakcha
            var mapper = new ProductToProductViewModelMapper();
            var products = _dbContext.Set<Product>().
                Include(pc => pc.ProductInCategories).Include(x => x.ProductMetadatas).
                Include(x => x.ProductsReferencedByKitAndAssembledTypes).
                Include(x => x.TaxesInProducts).FirstOrDefault(d => d.Id == productId && d.Active && d.WebActive);
            var productMetaData = products.ProductMetadatas;
            products.ProductMetadatas = null;
            products.ProductMetadatas = productMetaData.Where(a => a.CompanyId == LoggedInUser.CompanyId && a.Active).ToList();
            productList.Add(mapper.Map(products));
            //productList.Add(mapper.Map(_dbContext.Set<Product>().
            //    Include(pc => pc.ProductInCategories).Include(x => x.ProductMetadatas).
            //    Include(x => x.ProductsReferencedByKitAndAssembledTypes).
            //    Include(x => x.TaxesInProducts).FirstOrDefault(d => d.Id == productId)));
            return GetProductsWithDiscount(customerId, productList);
        }

        public Product[] GetProducts(Expression<Func<Product, bool>> where = null)
        {
            var query = _dbContext.Set<Product>().Where(FilterByActiveTrueAndCompany). Where(x => x.WebActive);

            query = where == null ? query : query.Where(where);
            var enumerable = query.ToArray();
            return enumerable;
        }


        public int SaveAllRefProducts(List<ProductsRefByKitAndAssembledType> productsRefList)
        {
            productsRefList.ForEach(x => x.CompanyId = LoggedInUser.CompanyId);
            _dbContext.Set<ProductsRefByKitAndAssembledType>().AddRange(productsRefList);
            return _dbContext.SaveChanges();
        }

        public List<int> GetExistingreferencesOfProduct(int productId)
        {
            var existingReferences = _dbContext.Set<ProductsRefByKitAndAssembledType>().Where(p => p.ProductId == productId).Select(p => p.ProductRefId).ToList();
            return existingReferences;
        }

        public bool DeleteRefOfKitAndAssembledType(int productId, int referenceProductId)
        {
            var doc = GetProductInCategory(productId, referenceProductId);
            var result = 0;
            if (doc == null) return result > 0;
            _dbContext.Set<ProductsRefByKitAndAssembledType>().Remove(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }

        public List<ProductViewModel> GetRegularItemsOnly()
        {
            var products =
                _dbContext.Set<Product>()
                    .Where(FilterByActiveTrueAndCompany)
                    .Where(x => x.ProductType == ProductType.Regular)
                    .ToList();
            var mapper = new ProductToProductViewModelMapper();
            return mapper.Map(products);
        }

        public Product CheckIfDeletedProductWithSameNameExists(string productName)
        {
            var product =
                _dbContext.Set<Product>()
                    .FirstOrDefault(
                        x =>
                            x.Name == productName && x.CompanyId == LoggedInUser.CompanyId && (x.Active ||
                            x.Active == false));
            return product;
        }

        public Product CheckIfDeletedProductWithSameCodeExists(string productCode)
        {
            var product =
               _dbContext.Set<Product>()
                   .FirstOrDefault(
                       x =>
                           x.Code == productCode && x.CompanyId == LoggedInUser.CompanyId && (x.Active ||
                           x.Active == false));
            return product;
        }

        private ProductsRefByKitAndAssembledType GetProductInCategory(int productId, int referenceProductId)
        {
            var productInCategory = _dbContext.Set<ProductsRefByKitAndAssembledType>().FirstOrDefault(d => d.ProductId == productId && d.ProductRefId == referenceProductId);
            return productInCategory;
        }

        private double CalculateDiscountByProduct(ProductViewModel productViewModel)
        {
            var productWiseDiscount = 0.0;
            var discounts = _dbContext.Set<Discount>().Where(d => d.ItemId == productViewModel.Id 
            && d.CompanyId == LoggedInUser.CompanyId && d.DiscountStartDate <= DateTime.Now && d.DiscountEndDate >= DateTime.Now 
            && d.Active).ToList();
            if (discounts != null && discounts.Count > 0)
            {
                productWiseDiscount += discounts.Sum(discount => CalculateDiscount(productViewModel, discount));
            }
            else
            {
                productWiseDiscount =  0.0;
            }
            return productWiseDiscount;
        }
        private double CalculateDiscountByProductCategory(DiscountCalculationType discountCalculationType, ProductViewModel productViewModel)
        {
            var listOfCategoryId = _dbContext.Set<ProductInCategory>()
                                    .Where(pic => pic.ProductId == productViewModel.Id && pic.CompanyId == LoggedInUser.CompanyId && pic.Active)
                                    .Select(x => x.CategoryId).ToList();
            var discountListForEachCategory = new List<double>();
            if (listOfCategoryId != null && listOfCategoryId.Count > 0)
            {
                foreach (var categoryId in listOfCategoryId)
                {
                    double totalDiscount;
                    var discounts = _dbContext.Set<Discount>()
                        .Where(x => x.CategoryId == categoryId && x.CompanyId == LoggedInUser.CompanyId
                                    && x.DiscountStartDate <= DateTime.Now && x.DiscountEndDate >= DateTime.Now &&
                                    x.Active).ToList();

                    if (discounts != null && discounts.Count > 0)
                    {
                        totalDiscount = discounts.Sum(discount => CalculateDiscount(productViewModel, discount));
                    }
                    else
                    {
                        totalDiscount = 0.0;
                    }
                    discountListForEachCategory.Add(totalDiscount);
                }
            }
            else
            {
                return 0.0;
            }

            switch (discountCalculationType)
            {
                case DiscountCalculationType.Minimum:
                    var minDiscount = discountListForEachCategory.Min();
                    return minDiscount;
                case DiscountCalculationType.Maximum:
                    var maxDiscount = discountListForEachCategory.Max();
                    return maxDiscount;
                default:
                    return 0.0;
            }
        }

        private double CalculateDiscountByCustomer(int customerId, ProductViewModel productViewModel)
        {
            var totalDiscount = 0.0;
            var discounts = _dbContext.Set<Discount>().Where(x => x.CustomerId == customerId 
            && x.CompanyId == LoggedInUser.CompanyId && x.DiscountStartDate <= DateTime.Now 
            && x.DiscountEndDate >= DateTime.Now && x.Active).ToList();
            if (discounts != null && discounts.Count > 0)
            {
                totalDiscount += discounts.Sum(discount => CalculateDiscount(productViewModel, discount));
            }
            else
            {
                totalDiscount = 0.0;
            }
            return totalDiscount;
        }
        private double CalculateDiscountByCustomerLevel(int customerId,  DiscountCalculationType discountCalculationType, ProductViewModel productViewModel)
        {
            var discountListForEachLevel = new List<double>();
            var listOfCustomerLevelId =
                _dbContext.Set<Customer>().Where(x => x.Id == customerId && x.Active 
                && x.CompanyId == LoggedInUser.CompanyId).Select(x => x.CustomerLevelId).ToList();
            if (listOfCustomerLevelId != null && listOfCustomerLevelId.Count > 0)
            {
                foreach (var customerLevelId in listOfCustomerLevelId)
                {
                    double totalDiscount;
                    var discounts = _dbContext.Set<Discount>().Where(x => x.CustomerLevelId == customerLevelId
                    && x.CompanyId == LoggedInUser.CompanyId && x.DiscountStartDate <= DateTime.Now
                    && x.DiscountEndDate >= DateTime.Now && x.Active).ToList();
                    if (discounts != null && discounts.Count > 0)
                    {
                        totalDiscount = discounts.Sum(discount => CalculateDiscount(productViewModel, discount));
                    }
                    else
                    {
                        totalDiscount = 0.0;
                    }
                    discountListForEachLevel.Add(totalDiscount);
                }
            }
            else
            {
                return 0.0;
            }
            switch (discountCalculationType)
            {
                case DiscountCalculationType.Minimum:
                    var minDiscount = discountListForEachLevel.Min();
                    return minDiscount;
                case DiscountCalculationType.Maximum:
                    var maxDiscount = discountListForEachLevel.Max();
                    return maxDiscount;
                default:
                    return 0.0;
            }
        }

        #endregion

        public ProductCategoryWithChildren GetCategoryWithChildren(int categoryId)
        {
            var category = _dbContext.Set<ProductCategory>().SingleOrDefault(x => x.Id == categoryId);
            var categories = _dbContext.Set<ProductCategory>().Where(x => x.ParentId == categoryId && x.Active).ToList();

            var mapper = new ProductCategoryToProductCategoryViewModelMapper();
            var categoriesViewModels = categories.Select(x => mapper.Map(x)).ToList();

            var categoryWithChildren = new ProductCategoryWithChildren
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                CategoryRank = category.CategoryRank,
                ParentId = category.ParentId,
                Version = category.Version,
                WebActive = category.WebActive,
                Active = category.Active,
                Childrens = categoriesViewModels
            };

            return categoryWithChildren;
        }

        public ProductCategoryViewModel GetCategoryWithProducts(int productCategoryId)
        {
            var productCategory = _dbContext.Set<ProductCategory>().Include(x => x.ProductInCategories).FirstOrDefault(d => d.Id == productCategoryId);
            var productIdList = new List<int>();
            var mapper = new ProductCategoryToProductCategoryViewModelMapper();
            var productMapper = new ProductToProductViewModelMapper();

            var productCategoryViewModel = mapper.Map(productCategory);

            if (productCategoryViewModel.ParentId == null)
            {
                productIdList.AddRange(_dbContext.Set<ProductInCategory>().Where(x => x.CategoryId == productCategoryViewModel.Id).Select(x => x.ProductId).Distinct().ToList());
                var products =
                    productIdList.Distinct().Select(
                        productId => _dbContext.Set<Product>().Include(inc => inc.ProductMetadatas).Distinct().SingleOrDefault(x => x.Id == productId && x.Active)).ToList();
                products.RemoveAll(p => p == null);
                productCategoryViewModel.Products = productMapper.Map(products);
            }
            else
            {
                var categories = _dbContext.Set<ProductCategory>().Where(d => d.ParentId == productCategoryViewModel.ParentId).Select(x => x.Id).ToList();
                foreach (var category in categories)
                {
                    productIdList.AddRange(_dbContext.Set<ProductInCategory>().Where(x => x.CategoryId == category).Select(x => x.ProductId).Distinct().ToList());
                }
                var products =
                        productIdList.Distinct().Select(
                            productId => _dbContext.Set<Product>().Include(inc => inc.ProductMetadatas).Distinct().SingleOrDefault(x => x.Id == productId && x.Active)).ToList();
                products.RemoveAll(p => p == null);
                productCategoryViewModel.Products = productMapper.Map(products);
            }

            return productCategoryViewModel;
        }

        public double CalculateDiscount(ProductViewModel productViewModel, Discount discount)
        {
            if (discount == null) return 0;
            switch (discount.DiscountType)
            {
                case DiscountType.Fixed:
                    return discount.DiscountValue;
                case DiscountType.Percent:
                    var discountValue = productViewModel.UnitPrice * discount.DiscountValue / 100;
                    return discountValue;
                case DiscountType.None:
                    return 0;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public List<ProductViewModel> GetProductsForStoreFront()
        {
            var productsInStock = _dbContext.Set<Product>().Include(x => x.ProductMetadatas).Where(x => x.CompanyId == LoggedInUser.CompanyId && x.Active && x.WebActive && x.QuantityOnHand > 0);
            var productsOutOfStock = _dbContext.Set<Product>().Include(x => x.ProductMetadatas).Where(x => x.CompanyId == LoggedInUser.CompanyId && x.Active && x.WebActive && x.QuantityOnHand <= 0);

            var mapper = new ProductToProductViewModelMapper();
            var productViewModelsInStock = productsInStock.Select(x => mapper.Map(x));
            var productViewModelsOutOfStock = productsOutOfStock.Select(x => mapper.Map(x));
            var allProductViewModels = GetAllProductViewModels(productViewModelsInStock, productViewModelsOutOfStock);

            return GetProductViewModels(productViewModelsInStock, allProductViewModels);
        }

        private List<ProductViewModel> GetProductViewModels(IQueryable<ProductViewModel> productViewModelsInStock, List<ProductViewModel> allProductViewModels)
        {
            var eCommerceSetting =
                            _dbContext.Set<EcommerceSetting>().FirstOrDefault(x => x.CompanyId == LoggedInUser.CompanyId);

            if (eCommerceSetting.DisplayOutOfStockItems)
            {
                if (!eCommerceSetting.IncludeQuantityInSalesOrder) return allProductViewModels.ToList();
                foreach (var productViewModel in allProductViewModels)
                {
                    var itemCount = _dbContext.Set<ItemCount>().FirstOrDefault(x => x.CompanyId == LoggedInUser.CompanyId && x.ItemId == productViewModel.Id);
                    if (itemCount == null) continue;
                    var productQuantity = productViewModel.QuantityOnHand + itemCount.QuantityOnInvoice + itemCount.QuantityOnOrder;
                    productViewModel.QuantityOnHand = productQuantity;
                }
                return allProductViewModels.ToList();
            }
            if (!eCommerceSetting.IncludeQuantityInSalesOrder) return productViewModelsInStock.ToList();
            {
                foreach (var productViewModel in productViewModelsInStock)
                {
                    var itemCount = _dbContext.Set<ItemCount>().FirstOrDefault(x => x.CompanyId == LoggedInUser.CompanyId && x.ItemId == productViewModel.Id);
                    if (itemCount != null)
                    {
                        productViewModel.QuantityOnHand = productViewModel.QuantityOnHand + itemCount.QuantityOnInvoice +
                                                          itemCount.QuantityOnOrder;
                    }
                }
                return productViewModelsInStock.ToList();
            }
        }

        private static List<ProductViewModel> GetAllProductViewModels(IQueryable<ProductViewModel> productViewModelsInStock, IQueryable<ProductViewModel> productViewModelsOutOfStock)
        {
            var allProductViewModels = new List<ProductViewModel>(productViewModelsInStock.Count() + productViewModelsOutOfStock.Count());
            allProductViewModels.AddRange(productViewModelsInStock);
            allProductViewModels.AddRange(productViewModelsOutOfStock);
            return allProductViewModels;
        }
        public PagedDataInquiryResponse<ProductViewModel> SearchProducts(PagedDataRequest requestInfo, Expression<Func<Product, bool>> where = null)
        {
            var filteredProduct = @where != null ? _dbContext.Set<Product>().Include(x => x.TaxesInProducts).Include(c => c.ProductInCategories).Where(@where).Where(w => w.CompanyId == LoggedInUser.CompanyId) :
                _dbContext.Set<Product>().Include(x => x.TaxesInProducts).Include(c=>c.ProductInCategories).Where(w => w.CompanyId == LoggedInUser.CompanyId);

            if (requestInfo.Active)
                filteredProduct = filteredProduct.Where(req => req.Active);
            var query = string.IsNullOrEmpty(requestInfo.SearchText) ? filteredProduct : filteredProduct.Where(s
                                                                  => s.Name.ToUpper().Contains(requestInfo.SearchText.ToUpper()));
            return FormatResultForPaging(requestInfo, query);
        }
    }

}

