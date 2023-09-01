using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface IProductQueryProcessor
    {
        Product Update(Product product);
        Product GetProduct(int productId);
        ProductViewModel GetProductViewModel(int productId);
        void SaveAllProduct(List<Product> products);
        bool Delete(int productId);
        bool DeleteRange(List<int?> productsId);
        bool Exists(Expression<Func<Product, bool>> where);
        Product ActivateProduct(int id);
        Product[] GetProducts(Expression<Func<Product, bool>> where = null);
        int GetCompanyIdByProductId(int productId);
        ApplicationUser ActiveUser { get; }
        Product Save(Product product);
        List<Product> GetProductsByCategoryId(int productCategoryId);
        IQueryable<Product> GetAllActiveProducts();
        List<ProductViewModel> GetProductsForStoreFront();
        PagedDataInquiryResponse<ProductViewModel> SearchProducts(PagedDataRequest requestInfo, Expression<Func<Product, bool>> @where = null);

        // Products With Discount
        List<ProductViewModel> GetProductsByCategoryIdAndLevel(int categoryId, int level);
        List<ProductViewModel> GetProductsWithDiscount(int customerId, List<ProductViewModel> productViewModels);
        List<ProductViewModel> SearchProductByCategoryIdAndText(int id, string text);
        PagedDataInquiryResponse<ProductViewModel> GetLatestProductsBySortedByDate(PagedDataRequest requestInfo, Expression<Func<Product, bool>> @where = null);
        List<ProductViewModel> GetProductViewModelForStoreFront(int customerId, int productId);

        int SaveAllRefProducts(List<ProductsRefByKitAndAssembledType> productsRefList);
        List<int> GetExistingreferencesOfProduct(int productId);
        bool DeleteRefOfKitAndAssembledType(int productId, int referenceProductId);
        List<ProductViewModel> GetRegularItemsOnly();
        Product CheckIfDeletedProductWithSameNameExists(string productName);
        Product CheckIfDeletedProductWithSameCodeExists(string productCode);

        IQueryable<ProductsRefByKitAndAssembledType> GetProductsRefByKitAndAssembledTypes(int distributorId);
        IQueryable<Product> GetActiveProducts(int distributorId);
        List<ProductViewModel> SearchActiveWithoutPaging(string searchText);
        ProductViewModelForStore GetProductViewModelByIdForStore(int id);
    }
}
