using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HrevertCRM.Data.Common;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface ICustomerInContactGroupQueryProcessor
    {
        CustomerInContactGroup Update(CustomerInContactGroup customerInContactGroup);
        CustomerInContactGroup GetValidCustomerInContactGroup(int customerInContactGroupId);
        CustomerInContactGroup GetCustomerInContactGroup(int customerId, int groupId);
        void SaveAllCustomerInContactGroup(List<CustomerInContactGroup> customerInContactGroups);
        CustomerInContactGroup Save(CustomerInContactGroup customerInContactGroup);
        int SaveAll(List<CustomerInContactGroup> customerInContactGroups);
        bool Exists(Expression<Func<CustomerInContactGroup, bool>> @where);
        QueryResult<CustomerInContactGroup> GetCustomerInContactGroups(PagedDataRequest requestInfo,
            Expression<Func<CustomerInContactGroup, bool>> @where = null);
        CustomerInContactGroup[] GetCustomerInContactGroups(Expression<Func<CustomerInContactGroup, bool>> @where = null);
        IQueryable<CustomerInContactGroup> GetActiveCustomerInContactGroups();
        IQueryable<CustomerInContactGroup> GetDeletedCustomerInContactGroups();
        bool Delete(int customerId, int groupId);
        List<int> GetExistingCustomersOfGroup(int groupId);
     
    }
}