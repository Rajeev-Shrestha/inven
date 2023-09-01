using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface IShoppingCartQueryProcessor
    {

        ShoppingCart Save(ShoppingCart shoppingCart);
        ShoppingCart Get(int shoppingCartId);
        ShoppingCart GetAll(int customerId);
        ShoppingCart Update(ShoppingCart shoppingCart);

        ShoppingCartViewModel AddProductToCart(ShoppingCartDetail detailLine);
        bool RemoveProductFromCart(int id);
        ShoppingCartDetailViewModel UpdateCartDetail(ShoppingCartDetail detailLine);

        bool Delete(int shoppingCartId);

        bool Exists(Expression<Func<ShoppingCart, bool>> @where);

        //ShoppingCart Update(ShoppingCart shoppingCart);
        //bool Delete(int shoppingCartId);
        //bool Exists(Func<ShoppingCart, bool> @where);
        ShoppingCart[] GetShoppingCarts(Expression<Func<ShoppingCart, bool>> @where = null);
        //ShoppingCart Save(ShoppingCart shoppingCart);
        int SaveAll(List<ShoppingCart> shoppingCarts);
        //List<ShoppingCart> SearchForShoppingCarts(string searchString);
        //PagedDataInquiryResponse<ShoppingCartViewModel> SearchForShoppingCarts(PagedDataRequest requestInfo, Expression<Func<ShoppingCart, bool>> @where = null);
        ShoppingCart ActivateShoppingCart(int id);
        ShoppingCartViewModel GetShoppingCartViewModel(int shoppingCartId, int customerId);
        SalesOrderViewModel ConvertCartToOrder(ConvertCartToOrderViewModel convertCartToOrderViewModel);
        ShoppingCartViewModel UpdateCartWithCustomerDetails(int cartId, int customerId);
        ShoppingCartViewModel GetCartViewModel(int id, int customerId, Guid guid);
        void SendEmailToCustomer(SendOrderToCustomerViaEmailViewModel sendOrderToCustomerViaEmailViewModel);
        PagedDataInquiryResponse<ShoppingCartViewModel> GetShoppingCarts(PagedDataRequest requestInfo, 
            Expression<Func<ShoppingCart, bool>> @where = null);
    }
}