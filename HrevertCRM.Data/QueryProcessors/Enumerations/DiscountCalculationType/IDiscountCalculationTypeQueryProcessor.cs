using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace HrevertCRM.Data.QueryProcessors.Enumerations
{
    public interface IDiscountCalculationTypeQueryProcessor
    {
        DiscountCalculationTypes Update(DiscountCalculationTypes discountCalculationType);
        DiscountCalculationTypes GetValidDiscountCalculationType(int discountCalculationTypeId);
        DiscountCalculationTypes GetDiscountCalculationType(int discountCalculationTypeId);
        void SaveAllDiscountCalculationType(List<DiscountCalculationTypes> discountCalculationType);
        DiscountCalculationTypes Save(DiscountCalculationTypes discountCalculationType);
        int SaveAll(List<DiscountCalculationTypes> discountCalculationType);
        DiscountCalculationTypes ActivateDiscountCalculationType(int id);
        DiscountCalculationTypeViewModel GetDiscountCalculationTypeViewModel(int id);
        bool Delete(int DiscountCalculationTypeId);
        bool Exists(Expression<Func<DiscountCalculationTypes, bool>> @where);
        DiscountCalculationTypes[] GetDiscountCalculationType(Expression<Func<DiscountCalculationTypes, bool>> @where = null);
        IQueryable<DiscountCalculationTypes> GetActiveDiscountCalculationType();
        IQueryable<DiscountCalculationTypes> GetDeletedDiscountCalculationType();
        IQueryable<DiscountCalculationTypes> GetAllDiscountCalculationType();
    }
}
