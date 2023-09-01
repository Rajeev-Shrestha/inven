using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;

namespace HrevertCRM.Data.QueryProcessors.Enumerations
{
    public interface IDiscountTypesQueryProcessor
    {
        DiscountTypes Update(DiscountTypes discountTypes);
        DiscountTypes GetValidDiscountTypes(int discountTypesId);
        DiscountTypes GetDiscountTypes(int discountTypesId);
        void SaveAllDiscountTypes(List<DiscountTypes> discountTypes);
        DiscountTypes Save(DiscountTypes discountTypes);
        int SaveAll(List<DiscountTypes> discountTypes);
        DiscountTypes ActivateDiscountTypes(int id);
        DiscountTypeViewModel GetDiscountTypesViewModel(int id);
        bool Delete(int discountTypesId);
        bool Exists(Expression<Func<DiscountTypes, bool>> @where);
        DiscountTypes[] GetDiscountTypes(Expression<Func<DiscountTypes, bool>> @where = null);
        IQueryable<DiscountTypes> GetActiveDiscountTypes();
        IQueryable<DiscountTypes> GetDeletedDiscountTypes();
        IQueryable<DiscountTypes> GetAllDiscountTypes();
    }
}