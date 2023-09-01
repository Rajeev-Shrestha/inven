using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Hrevert.Common.Security;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.Mapper.Enumerations;
using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;

namespace HrevertCRM.Data.QueryProcessors.Enumerations
{
    public class SalesOrdersStatusQueryProcessor : QueryBase<SalesOrdersStatus>, ISalesOrdersStatusQueryProcessor
    {
        public SalesOrdersStatusQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
        {
        }
        public SalesOrdersStatus Update(SalesOrdersStatus salesOrdersStatus)
        {
            var original = GetValidSalesOrdersStatus(salesOrdersStatus.Id);
            ValidateAuthorization(salesOrdersStatus);
            CheckVersionMismatch(salesOrdersStatus, original);

            original.Value = salesOrdersStatus.Value;
            original.Active = salesOrdersStatus.Active;
            original.CompanyId = salesOrdersStatus.CompanyId;

            _dbContext.Set<SalesOrdersStatus>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
        public virtual SalesOrdersStatus GetValidSalesOrdersStatus(int salesOrdersStatusId)
        {
            var salesOrdersStatus = _dbContext.Set<SalesOrdersStatus>().FirstOrDefault(sc => sc.Id == salesOrdersStatusId);
            if (salesOrdersStatus == null)
            {
                throw new RootObjectNotFoundException("Sales Order Status not found");
            }
            return salesOrdersStatus;
        }
        public SalesOrdersStatus GetSalesOrdersStatus(int salesOrdersStatusId)
        {
            var salesOrdersStatus = _dbContext.Set<SalesOrdersStatus>().FirstOrDefault(d => d.Id == salesOrdersStatusId);
            return salesOrdersStatus;
        }
        public void SaveAllSalesOrdersStatus(List<SalesOrdersStatus> salesOrdersStatus)
        {
            _dbContext.Set<SalesOrdersStatus>().AddRange(salesOrdersStatus);
            _dbContext.SaveChanges();
        }
        public SalesOrdersStatus Save(SalesOrdersStatus salesOrdersStatus)
        {
            salesOrdersStatus.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<SalesOrdersStatus>().Add(salesOrdersStatus);
            _dbContext.SaveChanges();
            return salesOrdersStatus;
        }
        public int SaveAll(List<SalesOrdersStatus> salesOrdersStatus)
        {
            _dbContext.Set<SalesOrdersStatus>().AddRange(salesOrdersStatus);
            return _dbContext.SaveChanges();
        }
        public SalesOrdersStatus ActivateSalesOrdersStatus(int id)
        {
            var original = GetValidSalesOrdersStatus(id);
            ValidateAuthorization(original);

            original.Active = true;
            _dbContext.Set<SalesOrdersStatus>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
        public SalesOrderStatusViewModel GetSalesOrdersStatusViewModel(int id)
        {
            var salesOrdersStatus = _dbContext.Set<SalesOrdersStatus>().Single(s => s.Id == id);
            var mapper = new SalesOrderStatusToSalesOrderStatusViewModelMapper();
            return mapper.Map(salesOrdersStatus);
        }
        public bool Delete(int salesOrdersStatusId)
        {
            var doc = GetSalesOrdersStatus(salesOrdersStatusId);
            ValidateAuthorization(doc);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<SalesOrdersStatus>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }
        public bool Exists(Expression<Func<SalesOrdersStatus, bool>> @where)
        {
            return _dbContext.Set<SalesOrdersStatus>().Any(@where);
        }
        public SalesOrdersStatus[] GetSalesOrdersStatus(Expression<Func<SalesOrdersStatus, bool>> @where = null)
        {

            var query = _dbContext.Set<SalesOrdersStatus>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            var enumerable = query.ToArray();
            return enumerable;
        }
        public IQueryable<SalesOrdersStatus> GetActiveSalesOrdersStatus()
        {
            return _dbContext.Set<SalesOrdersStatus>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active);
        }
        public IQueryable<SalesOrdersStatus> GetDeletedSalesOrdersStatus()
        {
            return _dbContext.Set<SalesOrdersStatus>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active == false);
        }
        public IQueryable<SalesOrdersStatus> GetAllSalesOrdersStatus()
        {
            var result = _dbContext.Set<SalesOrdersStatus>().Where(f => f.CompanyId == LoggedInUser.CompanyId);
            return result;
        }
    }
}
