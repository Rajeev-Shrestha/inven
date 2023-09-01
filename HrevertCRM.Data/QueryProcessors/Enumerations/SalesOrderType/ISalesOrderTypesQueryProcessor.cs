using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;

namespace HrevertCRM.Data.QueryProcessors.Enumerations
{
    public interface ISalesOrderTypesQueryProcessor
    {
        SalesOrderTypes Update(SalesOrderTypes salesOrderTypes);
        SalesOrderTypes GetValidSalesOrderTypes(int salesOrderTypesId);
        SalesOrderTypes GetSalesOrderTypes(int salesOrderTypesId);
        void SaveAllSalesOrderTypes(List<SalesOrderTypes> salesOrderTypes);
        SalesOrderTypes Save(SalesOrderTypes salesOrderTypes);
        int SaveAll(List<SalesOrderTypes> salesOrderTypes);
        SalesOrderTypes ActivateSalesOrderTypes(int id);
        SalesOrderTypeViewModel GetSalesOrderTypesViewModel(int id);
        bool Delete(int salesOrderTypesId);
        bool Exists(Expression<Func<SalesOrderTypes, bool>> @where);
        SalesOrderTypes[] GetSalesOrderTypes(Expression<Func<SalesOrderTypes, bool>> @where = null);
        IQueryable<SalesOrderTypes> GetActiveSalesOrderTypes();
        IQueryable<SalesOrderTypes> GetDeletedSalesOrderTypes();
        IQueryable<SalesOrderTypes> GetAllSalesOrderTypes();
    }
}