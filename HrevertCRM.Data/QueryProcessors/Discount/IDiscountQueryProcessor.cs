using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface IDiscountQueryProcessor
    {
        Discount Update(Discount discount);
        DiscountViewModel GetDiscountViewModel(int discountId);
        void SaveAllDiscount(List<Discount> discounts);
        bool Delete(int discountId);
        bool Exists(Expression<Func<Discount, bool>> @where);
        PagedDataInquiryResponse<DiscountViewModel> GetDiscounts(PagedDataRequest requestInfo, Expression<Func<Discount, bool>> @where = null);
        Discount[] GetDiscounts(Expression<Func<Discount, bool>> @where = null);
        Discount Save(Discount discount);
        int SaveAll(List<Discount> discounts);
        Discount ActivateDiscount(int id);
        IQueryable<Discount> GetActiveDiscountsWithoutPaging();
        IQueryable<Discount> GetDeletedDiscountsWithoutPaging();

        bool CheckDiscountPriceWithProductUnitPrice(int productId, double discountValue);
        bool DeleteRange(List<int?> discountsId);
        List<Discount> GetDiscountsProductWise();
        List<Discount> GetDiscountsCategoryWise();
        Discount GetDiscountOfProductForSlide(int id);
        Discount GetDiscountOfCategoryForSlide(int categoryId);
    }
}