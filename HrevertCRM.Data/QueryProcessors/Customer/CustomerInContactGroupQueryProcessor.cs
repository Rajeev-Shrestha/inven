using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Hrevert.Common.Constants.Customer;
using Hrevert.Common.Security;
using HrevertCRM.Data.Common;
using HrevertCRM.Entities;


namespace HrevertCRM.Data.QueryProcessors
{
    public class CustomerInContactGroupQueryProcessor : QueryBase<CustomerInContactGroup>, ICustomerInContactGroupQueryProcessor
    {
        public CustomerInContactGroupQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
        {
        }
        public CustomerInContactGroup Update(CustomerInContactGroup customerInContactGroup)
        {
            var original = GetValidCustomerInContactGroup(customerInContactGroup.Id);
            ValidateAuthorization(customerInContactGroup);
            CheckVersionMismatch(customerInContactGroup, original);

            original.ContactGroupId = customerInContactGroup.ContactGroupId;
            original.CustomerId = customerInContactGroup.CustomerId;
            original.Active = customerInContactGroup.Active;
            _dbContext.Set<CustomerInContactGroup>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        public virtual CustomerInContactGroup GetValidCustomerInContactGroup(int customerInContactGroupId)
        {
            var customerInContactGroup = _dbContext.Set<CustomerInContactGroup>().FirstOrDefault(sc => sc.Id == customerInContactGroupId);
            if (customerInContactGroup == null)
            {
                throw new RootObjectNotFoundException(CustomerConstants.CustomerInContactGroupQueryProcessorConstants.CustomerInContactGroupNotFound);
            }
            return customerInContactGroup;
        }

        public CustomerInContactGroup GetCustomerInContactGroup(int customerId, int groupId)
        {
            var customerInContactGroup = _dbContext.Set<CustomerInContactGroup>().FirstOrDefault(d => d.CustomerId == customerId && d.ContactGroupId == groupId);
            return customerInContactGroup;
        }

        public void SaveAllCustomerInContactGroup(List<CustomerInContactGroup> customerInContactGroups)
        {
            _dbContext.Set<CustomerInContactGroup>().AddRange(customerInContactGroups);
            _dbContext.SaveChanges();
        }

        public CustomerInContactGroup Save(CustomerInContactGroup customerInContactGroup)
        {
            _dbContext.Set<CustomerInContactGroup>().Add(customerInContactGroup);
            _dbContext.SaveChanges();
            return customerInContactGroup;
        }

        public int SaveAll(List<CustomerInContactGroup> customerInContactGroups)
        {
            _dbContext.Set<CustomerInContactGroup>().AddRange(customerInContactGroups);
            return _dbContext.SaveChanges();

        }

        public bool Delete(int customerId, int groupId)
        {
            var doc = GetCustomerInContactGroup(customerId, groupId);
            var result = 0;
            if (doc == null) return result > 0;
            _dbContext.Set<CustomerInContactGroup>().Remove(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }

        public bool Exists(Expression<Func<CustomerInContactGroup, bool>> @where)
        {
            return _dbContext.Set<CustomerInContactGroup>().Any(@where);
        }

        public QueryResult<CustomerInContactGroup> GetCustomerInContactGroups(PagedDataRequest requestInfo, Expression<Func<CustomerInContactGroup, bool>> @where = null)
        {
            var query = _dbContext.Set<CustomerInContactGroup>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

//            var enumerable = query as CustomerInContactGroup[] ?? query.ToArray();
            var totalItemCount = query.Count();
            var startIndex = ResultsPagingUtility.CalculateStartIndex(requestInfo.PageNumber, requestInfo.PageSize);
            var docs = query.OrderBy(x => x.DateCreated).Skip(startIndex).Take(requestInfo.PageSize).ToList();
            var queryResult = new QueryResult<CustomerInContactGroup>(docs, totalItemCount, requestInfo.PageSize);
            return queryResult;
        }

        public CustomerInContactGroup[] GetCustomerInContactGroups(Expression<Func<CustomerInContactGroup, bool>> @where = null)
        {
            var query = _dbContext.Set<CustomerInContactGroup>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            var enumerable =  query.ToArray();
            return enumerable;
        }

        public IQueryable<CustomerInContactGroup> GetActiveCustomerInContactGroups()
        {
            return _dbContext.Set<CustomerInContactGroup>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active);
        }

        public IQueryable<CustomerInContactGroup> GetDeletedCustomerInContactGroups()
        {
            return _dbContext.Set<CustomerInContactGroup>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active == false);
        }

        public List<int> GetExistingCustomersOfGroup(int groupId)
        {
            var existingCustomers =
                _dbContext.Set<CustomerInContactGroup>()
                    .Where(c => c.ContactGroupId == groupId)
                    .Select(c => c.CustomerId)
                    .ToList();
            return existingCustomers;
        }
    }
}
