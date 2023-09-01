using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface IShoppingCartDetailQueryProcessor
    {
        ShoppingCartDetail Update(ShoppingCartDetail shoppingCartDetail);
        bool Delete(int shoppingCartDetailId);
        bool Exists(Expression<Func<ShoppingCartDetail, bool>> @where);
        PagedDataInquiryResponse<ShoppingCartDetailViewModel> GetActiveShoppingCartDetails(PagedDataRequest requestInfo, Expression<Func<ShoppingCartDetail, bool>> @where = null);
        PagedDataInquiryResponse<ShoppingCartDetailViewModel> GetDeletedShoppingCartDetails(PagedDataRequest requestInfo, Expression<Func<ShoppingCartDetail, bool>> @where = null);
        PagedDataInquiryResponse<ShoppingCartDetailViewModel> GetAllShoppingCartDetails(PagedDataRequest requestInfo, Expression<Func<ShoppingCartDetail, bool>> @where = null);
        ShoppingCartDetail[] GetShoppingCartDetails(Expression<Func<ShoppingCartDetail, bool>> @where = null);
        ShoppingCartDetail Save(ShoppingCartDetail shoppingCartDetail);
        int SaveAll(List<ShoppingCartDetail> shoppingCartDetails);
        //List<ShoppingCartDetail> SearchForShoppingCartDetails(string searchString);
        //PagedDataInquiryResponse<ShoppingCartDetailViewModel> SearchForShoppingCartDetails(PagedDataRequest requestInfo, Expression<Func<ShoppingCartDetail, bool>> @where = null);
        ShoppingCartDetail ActivateShoppingCartDetail(int id);
        //EditShoppingCartDetailViewModel GetShoppingCartDetailViewModel(int ShoppingCartDetailId);
        List<ProductViewModel> GetTrendingProducts();

        List<ProductCategoryViewModel> GetTopCategories();
        List<ProductViewModel> GetHotThisWeek();
    }
}