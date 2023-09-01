using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Hrevert.Common.Enums;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface IPurchaseOrderLineQueryProcessor
    {
        PurchaseOrderLine Update(PurchaseOrderLine purchaseOrderLine);
        void SaveAllPurchaseOrderLine(List<PurchaseOrderLine> purchaseOrderLines);
        bool Delete(int purchaseOrderLineId);
        bool Exists(Expression<Func<PurchaseOrderLine, bool>> @where);
        PurchaseOrderLine[] GetPurchaseOrderLines(Expression<Func<PurchaseOrderLine, bool>> @where = null);
        PurchaseOrderLine Save(PurchaseOrderLine purchaseOrderLine);
        int SaveAll(ICollection<PurchaseOrderLine> purchaseOrderLines);
        void SaveAllViewModels(ICollection<PurchaseOrderLineViewModel> purchaseOrderLineViewModels);
        PurchaseOrderLine ActivatePurchaseOrderLine(int id);
        PurchaseOrderLineViewModel GetPurchaseOrderLineViewModel(int id);
        PagedDataInquiryResponse<PurchaseOrderLineViewModel> SearchPurchaseOrderLines(PagedDataRequest requestInfo, 
            Expression<Func<PurchaseOrderLine, bool>> @where = null);

        void UpdateQuantityOnOrderInProduct(PurchaseOrderLineViewModel purchaseOrderLineViewModel, PurchaseOrderType orderType);
    }
}