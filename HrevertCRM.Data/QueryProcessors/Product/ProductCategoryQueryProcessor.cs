using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Hrevert.Common.Constants.Product;
using Hrevert.Common.Enums;
using Hrevert.Common.Security;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.DTO;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using HrevertCRM.Data.Mapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using PagedTaskDataInquiryResponse = HrevertCRM.Data.Common.PagedDataInquiryResponse<HrevertCRM.Data.ViewModels.ProductCategoryViewModel>;


namespace HrevertCRM.Data.QueryProcessors
{
    public class ProductCategoryQueryProcessor : QueryBase<ProductCategory>, IProductCategoryQueryProcessor
    {
        private readonly IHostingEnvironment _env;

        public ProductCategoryQueryProcessor(IUserSession userSession, 
            IDbContext dbContext,
            IHostingEnvironment env)
            : base(userSession, dbContext)
        {
            _env = env;
        }
        public ProductCategory Update(ProductCategory productCategory)
        {
            var original = GetValidProductCategory(productCategory.Id);
            ValidateAuthorization(original);
            CheckVersionMismatch(productCategory, original);

            //TODO: map updated category to original //DONE

            original.Name = productCategory.Name;
            original.Description = productCategory.Description;
            original.WebActive = productCategory.WebActive;
            original.CategoryRank = productCategory.CategoryRank;
            original.ParentId = productCategory.ParentId;
            original.Active = productCategory.Active;
            original.CompanyId = productCategory.CompanyId;

            _dbContext.Set<ProductCategory>().Update(original);
            _dbContext.SaveChanges();
            return productCategory;
        }

        public virtual ProductCategory GetValidProductCategory(int categoryId)
        {
            var product = _dbContext.Set<ProductCategory>().FirstOrDefault(sc => sc.Id == categoryId);
            if (product == null)
            {
                throw new RootObjectNotFoundException(ProductConstants.ProductCategoryQueryProcessorConstants.ProductCategoryNotFound);
            }
            return product;
        }
        public ProductCategory Save(ProductCategory category)
        {
            category.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<ProductCategory>().Add(category);
            _dbContext.SaveChanges();
            return category;
        }

        public List<string> GetCategoryNamesByCategoryId(List<int> categoryIdList)
        {
            return categoryIdList.Select(categoryId => _dbContext.Set<ProductCategory>().Where(x => x.Id == categoryId).Select(p => p.Name).SingleOrDefault()).ToList();
        }

        public int GetCategoryIdByCategoryName(string categoryName)
        {
            return _dbContext.Set<ProductCategory>().Where(p => p.Name == categoryName).Select(x => x.Id).Single();
        }

        public string GetCategoryNameByCategoryId(int categoryId)
        {
            return _dbContext.Set<ProductCategory>().Where(p => p.Id == categoryId).Select(x => x.Name).Single();
        }


        public ProductCategory GetProductCategory(int productCategoryId)
        {
            var productCategory = _dbContext.Set<ProductCategory>().Include(x => x.ProductInCategories).FirstOrDefault(d => d.Id == productCategoryId);
            return productCategory;
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

        public IEnumerable<CategoryNode> GetProductCategoriesInTree()
        {
            var lookup = new Dictionary<int?, CategoryNode>();
            var productCategories = _dbContext.Set<ProductCategory>().Include(x => x.ProductInCategories)
                .ThenInclude(x => x.Product).ThenInclude(x => x.ProductMetadatas)
                .Where(x => x.CompanyId == LoggedInUser.CompanyId)
                .OrderBy(o => o.CategoryRank).ToList().Select(x => new ProductCategory
                {
                    ProductInCategories = x.ProductInCategories.Where(pin => pin.Product.Active && pin.Product.WebActive).ToList(),
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    CategoryRank = x.CategoryRank,
                    ParentId = x.ParentId,
                    WebActive = x.WebActive,
                    ParentCategory = x.ParentCategory
                }).ToList();

            productCategories.ForEach(x => lookup.Add(x.Id, new CategoryNode { Source = x }));

            foreach (var item in lookup.Values)
            {
                if (item.Source.ParentId == null) continue;
                CategoryNode proposedParent;
                if (!lookup.TryGetValue(item.Source.ParentId, out proposedParent)) continue;
                item.Parent = proposedParent;
                proposedParent.Children.Add(item);
            }

            return lookup.Values.Where(x => x.Parent == null);
        }

        public IEnumerable<CategoryNode> GetAllProductCategoriesInTree()
        {
            var lookup = new Dictionary<int?, CategoryNode>();

            var productCategories = _dbContext.Set<ProductCategory>().Include(x => x.ProductInCategories)
                .ThenInclude(x => x.Product).ThenInclude(x => x.ProductMetadatas)
                .Where(x => x.CompanyId == LoggedInUser.CompanyId)
                .OrderBy(o => o.CategoryRank).ToList();

            productCategories.ForEach(x => lookup.Add(x.Id, new CategoryNode { Source = x }));

            foreach (var item in lookup.Values)
            {
                if (item.Source.ParentId == null) continue;
                CategoryNode proposedParent;
                if (!lookup.TryGetValue(item.Source.ParentId, out proposedParent)) continue;
                item.Parent = proposedParent;
                proposedParent.Children.Add(item);
            }

            return lookup.Values.Where(x => x.Parent == null);
        }

        public IQueryable<ProductCategory> GetActiveProductCategories()
        {
            return _dbContext.Set<ProductCategory>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active);
        }

        public IQueryable<ProductCategory> GetDeletedProductCategories()
        {
            return _dbContext.Set<ProductCategory>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active == false);
        }

        public List<ProductCategoryViewModel> GetFullProductCategories()
        {
            var productCategories = GetAllProductCategories().ToList();

            var mapper = new ProductCategoryToProductCategoryViewModelMapper();
            var productMapper = new ProductToProductViewModelMapper();

            var categoriesList = mapper.Map(productCategories);
            var productIdList = new List<int>();

            foreach (var productCategory in categoriesList)
            {
                if (productCategory.ParentId == null)
                {
                    productIdList.AddRange(_dbContext.Set<ProductInCategory>().Where(x => x.CategoryId == productCategory.Id).Select(x => x.ProductId).Distinct().ToList());
                    var products =
                        productIdList.Distinct().Select(
                            productId => _dbContext.Set<Product>().Include(inc => inc.ProductMetadatas).Distinct().SingleOrDefault(x => x.Id == productId)).ToList();
                    productCategory.Products = productMapper.Map(products);
                }
                else
                {
                    var categories = _dbContext.Set<ProductCategory>().Where(d => d.ParentId == productCategory.ParentId).Select(x => x.Id).ToList();
                    foreach (var category in categories)
                    {
                        productIdList.AddRange(_dbContext.Set<ProductInCategory>().Where(x => x.CategoryId == category).Select(x => x.ProductId).Distinct().ToList());
                    }
                    var products =
                            productIdList.Distinct().Select(
                                productId => _dbContext.Set<Product>().Include(inc => inc.ProductMetadatas).Distinct().SingleOrDefault(x => x.Id == productId)).ToList();
                    productCategory.Products = productMapper.Map(products);
                }
            }

            return categoriesList;
        }


        public ProductCategory CheckIfCategoryExistsOrSave(ProductCategory productCategory) //for seeding
        {
            var category = productCategory;
            productCategory = _dbContext.Set<ProductCategory>().Any(row => row.Name == category.Name) ? _dbContext.Set<ProductCategory>().FirstOrDefault(row => row.Name == productCategory.Name) : Save(productCategory);
            return productCategory;
        }

        public int SaveAll(List<ProductCategory> productCategories)
        {
            _dbContext.Set<ProductCategory>().AddRange(productCategories);
            return _dbContext.SaveChanges();
        }

        public bool DeleteOnlyCategory(int productCategoryId)
        {
            var doc = GetProductCategory(productCategoryId);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<ProductCategory>().Update(doc);
            _dbContext.SaveChanges();
            return result > 0;
        }

        public bool Delete(int productCategoryId)
        {
            var doc = GetProductCategory(productCategoryId);
            var result = 0;
            if (doc == null) return result > 0;
            DeleteCategoryTree(doc);

            for (var i = 0; i < doc.ProductInCategories.ToArray().Length; i++)
            {
                doc.Active = false;

            }
            //var categories = _dbContext.Set<ProductCategory>().Where(x=>x.CompanyId==LoggedInUser.CompanyId);
            //foreach (var item in categories)
            //{
            //    if (item.ParentId == productCategoryId)
            //    {
            //        item.Active = false;
            //        _dbContext.Set<ProductCategory>().Update(doc);
            //    }
            //}
            _dbContext.Set<ProductCategory>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }

        public bool Exists(Expression<Func<ProductCategory, bool>> @where)
        {
            return _dbContext.Set<ProductCategory>().Any(@where);
        }

        public PagedTaskDataInquiryResponse GetProductCategories(PagedDataRequest requestInfo, Expression<Func<ProductCategory, bool>> @where = null)
        {
            var query = _dbContext.Set<ProductCategory>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            //var enumerable = query as ProductCategory[] ?? query.ToArray();
            var totalItemCount = query.Count();
            var startIndex = ResultsPagingUtility.CalculateStartIndex(requestInfo.PageNumber, requestInfo.PageSize);

            var mapper = new ProductCategoryToProductCategoryViewModelMapper();
            var docs = query.OrderBy(x => x.DateCreated).Skip(startIndex).Take(requestInfo.PageSize).Select(s => mapper.Map(s)).ToList();
            var queryResult = new QueryResult<ProductCategoryViewModel>(docs, totalItemCount, requestInfo.PageSize);
            var inquiryResponse = new PagedTaskDataInquiryResponse()
            {
                Items = docs,
                TotalRecords = totalItemCount,
                PageCount = queryResult.TotalPageCount,
                PageNumber = requestInfo.PageNumber,
                PageSize = requestInfo.PageSize
            };
            return inquiryResponse;
        }

        public ProductCategory[] GetProductCategories(Expression<Func<ProductCategory, bool>> @where = null)
        {
            var query = _dbContext.Set<ProductCategory>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            var enumerable = query.ToArray();
            return enumerable;
        }

        public IQueryable<ProductCategory> GetAllProductCategories()
        {
            return _dbContext.Set<ProductCategory>().Where(x => x.CompanyId == LoggedInUser.CompanyId);
        }
        private void DeleteCategoryTree(ProductCategory categoryTreeViewModel)
        {
            categoryTreeViewModel.Active = false;
            //find children product
            var child = _dbContext.Set<ProductCategory>().Where(x => x.ParentId == categoryTreeViewModel.Id).ToList();
            foreach (var categoryNode in child)
            {
                DeleteCategoryTree(categoryNode);
            }
               
                    
            
            _dbContext.SaveChanges();
        }

        public string GetCategoryName(List<int> categoryList)
        {
            string stringToBeReturned = null;

            foreach (var categoryId in categoryList)
            {
                stringToBeReturned =
                    _dbContext.Set<ProductCategory>().Where(x => x.Id == categoryId).Select(p => p.Name).Single();
                stringToBeReturned = stringToBeReturned + ", ";
            }
            return stringToBeReturned;
        }

        public List<string> CategoryNamesBySplittingString(string value)
        {
            var splitedString = Regex.Split(value, "[ ]*,[ ]*").ToList();
            return splitedString;
        }

        public List<int> GetCategoryIdByCategoryNames(List<string> categoryNames)
        {
            return categoryNames.Select(categoryName => _dbContext.Set<ProductCategory>().Where(x => x.Name == categoryName).Select(p => p.Id).Single()).ToList();
        }

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
                CompanyId = category.CompanyId,
                Childrens = categoriesViewModels
            };

            return categoryWithChildren;
        }

        public ProductCategoryWithChildren GetProductByCategoryIdLevelAndNoofProduct(int categoryId, int levelId, int noofProducts)
        {

            var category = _dbContext.Set<ProductCategory>().SingleOrDefault(x => x.Id == categoryId);
            var categories = _dbContext.Set<ProductCategory>().Where(x => x.ParentId == categoryId && x.Active).ToList();

            var mapper = new ProductCategoryToProductCategoryViewModelMapper();
            var categoriesViewModels = categories.Select(x => mapper.Map(x)).ToList();
            var productMapper = new ProductToProductViewModelMapper();

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
                CompanyId =  category.CompanyId,
                Childrens = categoriesViewModels
            };
           
            if (categoryWithChildren.Childrens.Count>0)
            {
                foreach (var item in categoryWithChildren.Childrens)
                {
                    var product = _dbContext.Set<ProductInCategory>().Where(x => x.CategoryId == item.Id).Include(p => p.Product).Select(y => y.Product);
                    var productViewModel = product.Select(x => productMapper.Map(x)).ToList();
                    item.Products = productViewModel;
                }

            }

            return categoryWithChildren;
        }

        // for searching of products by categoryId and text
       public List<Product> GetProductsByCategoryIdAndSearchText(int productCategoryId, string text)
       {
           var products = _dbContext.Set<ProductInCategory>()
                .Where(x => x.Active && x.CompanyId == LoggedInUser.CompanyId && x.CategoryId == productCategoryId);
           var prods = products.Include(x => x.Product).ThenInclude(x => x.ProductMetadatas);
           var newProducts = prods.Where(x => x.Product.Code.ToUpper().Contains(text.ToUpper()) 
           || x.Product.Name.ToUpper().Contains(text.ToUpper())).Select(x => x.Product).ToList();
           return newProducts;
       }


        // parent product categories

        public IQueryable<ProductCategory> GetAllParentProductCategories()
        {
            return _dbContext.Set<ProductCategory>().Where(x => x.CompanyId == LoggedInUser.CompanyId && x.ParentId==null);
        }
        // for parent category
        public IQueryable<ProductCategory> GetAllActiveProductCategories()
        {
            return _dbContext.Set<ProductCategory>().Where(x => x.CompanyId == LoggedInUser.CompanyId && x.Active);
        }


        public ProductCategory ActivateProductCategory(int categoryId)
        {
            var category = _dbContext.Set<ProductCategory>().FirstOrDefault(x => x.CompanyId == LoggedInUser.CompanyId && x.Id == categoryId);

            var categories = _dbContext.Set<ProductCategory>().Where(x => x.CompanyId == LoggedInUser.CompanyId);

            foreach (var item in categories)
            {
                if (item.ParentId == categoryId)
                {
                    item.Active = true;
                    _dbContext.Set<ProductCategory>().Update(item);
                }
            }
            category.Active = true;
            _dbContext.Set<ProductCategory>().Update(category);

            _dbContext.SaveChanges();
            return category;
        }

        public IQueryable<ProductCategory> GetActiveProductCategories(int distributorId)
        {
            return _dbContext.Set<ProductCategory>().Where(x => x.CompanyId == distributorId && x.Active);
        }

        public ProductCategory CheckIfDeletedProductCategoryWithSameNameExists(string name)
        {
            var productCategory =
                _dbContext.Set<ProductCategory>()
                    .FirstOrDefault(
                        x =>
                            x.Name == name && x.CompanyId == LoggedInUser.CompanyId && (x.Active ||
                            x.Active == false));
            return productCategory;
        }

        public short GetLastCategoryRank()
        {
            var categoryRank =
                _dbContext.Set<ProductCategory>()
                    .Where(x => x.CompanyId == LoggedInUser.CompanyId)
                    .Max(x => x.CategoryRank);
            return categoryRank;
        }

        public string SaveCategoryImage(Image image)
        {
            string mediaUrl;
            string copyFileName = null;
            try
            {
                if (!CheckFileExtesions(image)) return "Please choose only Image and Pdf files.";

                //Check if the directory exists or create a new one
                var bytes = Convert.FromBase64String(image.ImageBase64);
                var filename = Regex.Replace(Path.GetFileNameWithoutExtension(image.FileName), @"\s+", "");
                var fileExtension = Path.GetExtension(image.FileName);
                var imagepath = @"companyMM\" + LoggedInUser.CompanyId + @"\ProductCategoryImages\";
                var path = Path.Combine(_env.WebRootPath, @"companyMM\" + LoggedInUser.CompanyId + @"\ProductCategoryImages");

                CreateDirectory(path);
                //Create path with filename
                var fileWritten = false;
                var i = 0;
                while (!fileWritten)
                {
                    if (i > 0)
                    {
                        if (!filename.Contains("_") && i == 1)
                        {
                            filename = filename + "_" + i;
                        }
                        else
                        {
                            filename = filename.Remove(filename.LastIndexOf("_", StringComparison.Ordinal)) + "_" + i;
                        }
                    }

                    copyFileName = filename + fileExtension;
                    var filePath = Path.Combine(path, copyFileName);

                    if (File.Exists(filePath)) i++;
                    else
                    {
                        using (var imageFile = new FileStream(filePath, FileMode.Create))
                        {
                            imageFile.Write(bytes, 0, bytes.Length);
                            imageFile.Flush();
                            fileWritten = true;
                        }
                    }
                }
                mediaUrl = imagepath + copyFileName;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return mediaUrl;
        }
        private static void CreateDirectory(string path)
        {
            var directory = new DirectoryInfo(path);
            if (directory.Exists == false)
                directory.Create();
        }

        private static bool CheckFileExtesions(Image image)
        {
            var allowedExtensions = new[] { ".JPG", ".PNG", ".JPEG" };
            var ext = Path.GetExtension(image.FileName);
            return allowedExtensions.Contains(ext.ToUpper());
        }
    }
}