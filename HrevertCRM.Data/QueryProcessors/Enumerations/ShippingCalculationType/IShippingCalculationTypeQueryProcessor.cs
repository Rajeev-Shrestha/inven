using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;
using System.Linq.Expressions;

namespace HrevertCRM.Data.QueryProcessors.Enumerations
{
    public interface IShippingCalculationTypeQueryProcessor
    {
        ShippingCalculationTypes Update(ShippingCalculationTypes shippingCalculationType);
        ShippingCalculationTypes GetValidShippingCalculationType(int shippingCalculationTypeId);
        ShippingCalculationTypes GetShippingCalculationType(int shippingCalculationTypeId);
        void SaveAllShippingCalculationType(List<ShippingCalculationTypes> shippingCalculationType);
        ShippingCalculationTypes Save(ShippingCalculationTypes shippingCalculationType);
        int SaveAll(List<ShippingCalculationTypes> ShippingCalculationType);
        ShippingCalculationTypes ActivateShippingCalculationType(int id);
        ShippingCalculationTypeViewModel GetShippingCalculationTypeViewModel(int id);
        bool Delete(int ShippingCalculationTypeId);
        bool Exists(Expression<Func<ShippingCalculationTypes, bool>> @where);
        ShippingCalculationTypes[] GetShippingCalculationType(Expression<Func<ShippingCalculationTypes, bool>> @where = null);
        IQueryable<ShippingCalculationTypes> GetActiveShippingCalculationType();
        IQueryable<ShippingCalculationTypes> GetDeletedShippingCalculationType();
        IQueryable<ShippingCalculationTypes> GetAllShippingCalculationType();
    }
}
