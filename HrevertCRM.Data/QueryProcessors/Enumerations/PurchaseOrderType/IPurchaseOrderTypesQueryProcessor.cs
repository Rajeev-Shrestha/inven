using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;

namespace HrevertCRM.Data.QueryProcessors.Enumerations
{
    public interface IPurchaseOrderTypesQueryProcessor
    {
        PurchaseOrderTypes Update(PurchaseOrderTypes purchaseOrderTypes);
        PurchaseOrderTypes GetValidPurchaseOrderTypes(int purchaseOrderTypesId);
        PurchaseOrderTypes GetPurchaseOrderTypes(int purchaseOrderTypesId);
        void SaveAllPurchaseOrderTypes(List<PurchaseOrderTypes> purchaseOrderTypes);
        PurchaseOrderTypes Save(PurchaseOrderTypes purchaseOrderTypes);
        int SaveAll(List<PurchaseOrderTypes> purchaseOrderTypes);
        PurchaseOrderTypes ActivatePurchaseOrderTypes(int id);
        PurchaseOrderTypeViewModel GetPurchaseOrderTypesViewModel(int id);
        bool Delete(int purchaseOrderTypesId);
        bool Exists(Expression<Func<PurchaseOrderTypes, bool>> @where);
        PurchaseOrderTypes[] GetPurchaseOrderTypes(Expression<Func<PurchaseOrderTypes, bool>> @where = null);
        IQueryable<PurchaseOrderTypes> GetActivePurchaseOrderTypes();
        IQueryable<PurchaseOrderTypes> GetDeletedPurchaseOrderTypes();
        IQueryable<PurchaseOrderTypes> GetAllPurchaseOrderTypes();
    }
}