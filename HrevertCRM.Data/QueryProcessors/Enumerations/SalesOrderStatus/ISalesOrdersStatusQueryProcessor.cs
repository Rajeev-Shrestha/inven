using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;

namespace HrevertCRM.Data.QueryProcessors.Enumerations
{
    public interface ISalesOrdersStatusQueryProcessor
    {
        SalesOrdersStatus Update(SalesOrdersStatus salesOrdersStatus);
        SalesOrdersStatus GetValidSalesOrdersStatus(int salesOrdersStatusId);
        SalesOrdersStatus GetSalesOrdersStatus(int salesOrdersStatusId);
        void SaveAllSalesOrdersStatus(List<SalesOrdersStatus> salesOrdersStatus);
        SalesOrdersStatus Save(SalesOrdersStatus salesOrdersStatus);
        int SaveAll(List<SalesOrdersStatus> salesOrdersStatus);
        SalesOrdersStatus ActivateSalesOrdersStatus(int id);
        SalesOrderStatusViewModel GetSalesOrdersStatusViewModel(int id);
        bool Delete(int salesOrdersStatusId);
        bool Exists(Expression<Func<SalesOrdersStatus, bool>> @where);
        SalesOrdersStatus[] GetSalesOrdersStatus(Expression<Func<SalesOrdersStatus, bool>> @where = null);
        IQueryable<SalesOrdersStatus> GetActiveSalesOrdersStatus();
        IQueryable<SalesOrdersStatus> GetDeletedSalesOrdersStatus();
        IQueryable<SalesOrdersStatus> GetAllSalesOrdersStatus();
    }
}