using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;

namespace HrevertCRM.Data.QueryProcessors.Enumerations
{
    public interface IPurchaseOrdersStatusQueryProcessor
    {
        PurchaseOrdersStatus Update(PurchaseOrdersStatus purchaseOrdersStatus);
        PurchaseOrdersStatus GetValidPurchaseOrdersStatus(int purchaseOrdersStatusId);
        PurchaseOrdersStatus GetPurchaseOrdersStatus(int purchaseOrdersStatusId);
        void SaveAllPurchaseOrdersStatus(List<PurchaseOrdersStatus> purchaseOrdersStatus);
        PurchaseOrdersStatus Save(PurchaseOrdersStatus purchaseOrdersStatus);
        int SaveAll(List<PurchaseOrdersStatus> purchaseOrdersStatus);
        PurchaseOrdersStatus ActivatePurchaseOrdersStatus(int id);
        PurchaseOrderStatusViewModel GetPurchaseOrdersStatusViewModel(int id);
        bool Delete(int purchaseOrdersStatusId);
        bool Exists(Expression<Func<PurchaseOrdersStatus, bool>> @where);
        PurchaseOrdersStatus[] GetPurchaseOrdersStatus(Expression<Func<PurchaseOrdersStatus, bool>> @where = null);
        IQueryable<PurchaseOrdersStatus> GetActivePurchaseOrdersStatus();
        IQueryable<PurchaseOrdersStatus> GetDeletedPurchaseOrdersStatus();
        IQueryable<PurchaseOrdersStatus> GetAllPurchaseOrdersStatus();
    }
}