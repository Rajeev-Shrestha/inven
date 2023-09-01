using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;


namespace HrevertCRM.Data.QueryProcessors
{
    public interface IDeliveryMethodQueryProcessor
    {
        DeliveryMethod Update(DeliveryMethod deliveryMethod);
        DeliveryMethod GetDeliveryMethod(int deliveryMethodId);
        void SaveAllDeliveryMethod(List<DeliveryMethod> deliveryMethods);
        bool Delete(int deliveryMethodId);
        bool Exists(Expression<Func<DeliveryMethod, bool>> @where);
        QueryResult<DeliveryMethod> GetDeliveryMethods(PagedDataRequest requestInfo, Expression<Func<DeliveryMethod, bool>> @where = null);
        DeliveryMethod[] GetDeliveryMethods(Expression<Func<DeliveryMethod, bool>> @where = null);
        DeliveryMethod Save(DeliveryMethod deliveryMethod);
        int SaveAll(List<DeliveryMethod> deliveryMethods);
        DeliveryMethod ActivateDeliveryMethod(int id);
        DeliveryMethod CheckIfDeletedDeliveryMethodWithSameCodeExists(string code);
        IQueryable<DeliveryMethod> SearchDeliveryMethods(bool active, string searchText);
        bool DeleteRange(List<int?> deliveryMethodsId);
    }
}
