using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Hrevert.Common.Enums;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface ISalesOrderQueryProcessor
    {
        string GenerateSalesOrderCode();
        SalesOrder Update(SalesOrder salesOrder);
        void SaveAllSalesOrder(List<SalesOrder> salesOrders);
        bool Delete(int salesOrderId);
        bool Exists(Expression<Func<SalesOrder, bool>> where);

        SalesOrder[] GetSalesOrders(Expression<Func<SalesOrder, bool>> where = null);
        SalesOrder Save(SalesOrder salesOrder);
        int SaveAll(List<SalesOrder> salesOrders);
        SalesOrder ActivateSalesOrder(int id);
        SalesOrderViewModel GetSalesOrderViewModel(int id);
        SalesOrderDefaultValuesViewModel GetDefaultValues(int customerId);
        DateTime GetDueDate(DateTime? date, int termId);
        List<TaskDocIdViewModel> GetSalesOrderNumbers();

        PagedDataInquiryResponse<SalesOrderViewModel> SearchSalesOrders(PagedDataRequest requestInfo, 
            Expression<Func<SalesOrder, bool>> @where = null);

        SalesOrderType GetOrderType(int? id);
        bool DeleteRange(List<int?> salesOrderId);
    }
}