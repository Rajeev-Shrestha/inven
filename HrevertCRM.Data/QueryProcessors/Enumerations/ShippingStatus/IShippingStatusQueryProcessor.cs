using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HrevertCRM.Data.QueryProcessors.Enumerations
{
    public interface IShippingStatusQueryProcessor
    {
        ShippingStatus Update(ShippingStatus shippingStatus);
        ShippingStatus GetValidShippingStatus(int shippingStatusId);
        ShippingStatus GetShippingStatus(int shippingStatusId);
        void SaveAllShippingStatus(List<ShippingStatus> shippingStatus);
        ShippingStatus Save(ShippingStatus shippingStatus);
        int SaveAll(List<ShippingStatus> shippingStatus);
        ShippingStatus ActivateShippingStatus(int id);
        ShippingStatusViewModel GetShippingStatusViewModel(int id);
        bool Delete(int shippingStatusId);
        bool Exists(Expression<Func<ShippingStatus, bool>> @where);
        ShippingStatus[] GetShippingStatus(Expression<Func<ShippingStatus, bool>> @where = null);
        IQueryable<ShippingStatus> GetActiveShippingStatus();
        IQueryable<ShippingStatus> GetDeletedShippingStatus();
        IQueryable<ShippingStatus> GetAllShippingStatus();
    }
}
