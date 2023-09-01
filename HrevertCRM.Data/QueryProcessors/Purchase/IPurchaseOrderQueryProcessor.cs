using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface IPurchaseOrderQueryProcessor
    {
        string GeneratePurchaseOrderCode();
        PurchaseOrder Update(PurchaseOrder purchaseOrder);
        void SaveAllPurchaseOrder(List<PurchaseOrder> purchaseOrders);
        bool Delete(int purchaseOrderId);
        bool Exists(Expression<Func<PurchaseOrder, bool>> @where);
        PurchaseOrder[] GetPurchaseOrders(Expression<Func<PurchaseOrder, bool>> @where = null);
        PurchaseOrder Save(PurchaseOrder purchaseOrder);
        int SaveAll(List<PurchaseOrder> purchaseOrders);
        PurchaseOrder ActivatePurchaseOrder(int id);
        PurchaseOrderViewModel GetPurchaseOrderViewModel(int id);
        PurchaseOrderDefaultValuesViewModel GetDefaultValues(int customerId);
        List<TaskDocIdViewModel> GetPurchaseOrderNumbers();
        DateTime GetDueDate(DueDateViewModel dueDateViewModel);
        PagedDataInquiryResponse<PurchaseOrderViewModel> SearchPurchaseOrders(PagedDataRequest requestInfo, 
            Expression<Func<PurchaseOrder, bool>> @where = null);

        bool DeleteRange(List<int?> purchaseTermsId);
    }
}