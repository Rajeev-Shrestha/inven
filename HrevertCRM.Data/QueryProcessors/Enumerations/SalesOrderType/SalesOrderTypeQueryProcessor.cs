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
    public class SalesOrderTypesQueryProcessor : QueryBase<SalesOrderTypes>, ISalesOrderTypesQueryProcessor
    {
        public SalesOrderTypesQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
        {
        }
        public SalesOrderTypes Update(SalesOrderTypes salesOrderTypes)
        {
            var original = GetValidSalesOrderTypes(salesOrderTypes.Id);
            ValidateAuthorization(salesOrderTypes);
            CheckVersionMismatch(salesOrderTypes, original);

            original.Value = salesOrderTypes.Value;
            original.Active = salesOrderTypes.Active;
            original.CompanyId = salesOrderTypes.CompanyId;

            _dbContext.Set<SalesOrderTypes>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
        public virtual SalesOrderTypes GetValidSalesOrderTypes(int salesOrderTypesId)
        {
            var salesOrderTypes = _dbContext.Set<SalesOrderTypes>().FirstOrDefault(sc => sc.Id == salesOrderTypesId);
            if (salesOrderTypes == null)
            {
                throw new RootObjectNotFoundException("Sales Order Types not found");
            }
            return salesOrderTypes;
        }
        public SalesOrderTypes GetSalesOrderTypes(int salesOrderTypesId)
        {
            var salesOrderTypes = _dbContext.Set<SalesOrderTypes>().FirstOrDefault(d => d.Id == salesOrderTypesId);
            return salesOrderTypes;
        }
        public void SaveAllSalesOrderTypes(List<SalesOrderTypes> salesOrderTypes)
        {
            _dbContext.Set<SalesOrderTypes>().AddRange(salesOrderTypes);
            _dbContext.SaveChanges();
        }
        public SalesOrderTypes Save(SalesOrderTypes salesOrderTypes)
        {
            salesOrderTypes.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<SalesOrderTypes>().Add(salesOrderTypes);
            _dbContext.SaveChanges();
            return salesOrderTypes;
        }
        public int SaveAll(List<SalesOrderTypes> salesOrderTypes)
        {
            _dbContext.Set<SalesOrderTypes>().AddRange(salesOrderTypes);
            return _dbContext.SaveChanges();
        }
        public SalesOrderTypes ActivateSalesOrderTypes(int id)
        {
            var original = GetValidSalesOrderTypes(id);
            ValidateAuthorization(original);

            original.Active = true;
            _dbContext.Set<SalesOrderTypes>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
        public SalesOrderTypeViewModel GetSalesOrderTypesViewModel(int id)
        {
            var salesOrderTypes = _dbContext.Set<SalesOrderTypes>().Single(s => s.Id == id);
            var mapper = new SalesOrderTypeToSalesOrderTypeViewModelMapper();
            return mapper.Map(salesOrderTypes);
        }
        public bool Delete(int salesOrderTypesId)
        {
            var doc = GetSalesOrderTypes(salesOrderTypesId);
            ValidateAuthorization(doc);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<SalesOrderTypes>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }
        public bool Exists(Expression<Func<SalesOrderTypes, bool>> @where)
        {
            return _dbContext.Set<SalesOrderTypes>().Any(@where);
        }
        public SalesOrderTypes[] GetSalesOrderTypes(Expression<Func<SalesOrderTypes, bool>> @where = null)
        {

            var query = _dbContext.Set<SalesOrderTypes>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            var enumerable = query.ToArray();
            return enumerable;
        }
        public IQueryable<SalesOrderTypes> GetActiveSalesOrderTypes()
        {
            return _dbContext.Set<SalesOrderTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active);
        }
       
        public IQueryable<SalesOrderTypes> GetDeletedSalesOrderTypes()
        {
            return _dbContext.Set<SalesOrderTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active == false);
        }
        public IQueryable<SalesOrderTypes> GetAllSalesOrderTypes()
        {
            var result = _dbContext.Set<SalesOrderTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId);
            return result;
        }
    }
}
