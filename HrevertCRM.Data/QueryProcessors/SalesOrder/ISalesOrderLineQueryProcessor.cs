using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Hrevert.Common.Enums;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface ISalesOrderLineQueryProcessor
    {
        SalesOrderLine Update(SalesOrderLine salesOrderLine);
        void SaveAllSalesOrderLine(ICollection<SalesOrderLine> salesOrderLines);
        bool Delete(int salesOrderLineId);
        bool Exists(Expression<Func<SalesOrderLine, bool>> @where);
        SalesOrderLine[] GetSalesOrderLines(Expression<Func<SalesOrderLine, bool>> @where = null);
        SalesOrderLine Save(SalesOrderLine salesOrderLine);
        void SaveAll(ICollection<SalesOrderLineViewModel> salesOrderLines);
        SalesOrderLine ActivateSalesOrderLine(int id);
        SalesOrderLineViewModel GetSalesOrderLineViewModel(int id);
        void SaveAllSeed(List<SalesOrderLine> salesOrderLines);
        void UpdateQuantityInProduct(IEnumerable<SalesOrderLineViewModel> salesOrderLines);

        //void SaveItemCount();
        PagedDataInquiryResponse<SalesOrderLineViewModel> SearchSalesOrderLines(PagedDataRequest requestInfo, 
            Expression<Func<SalesOrderLine, bool>> @where = null);

        void UpdateQuantityOnOrderInProduct(SalesOrderLineViewModel salesOrderLineViewModel, SalesOrderType orderType);
    }
}