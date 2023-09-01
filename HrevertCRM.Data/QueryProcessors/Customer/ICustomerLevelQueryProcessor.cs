using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface ICustomerLevelQueryProcessor
    {
        CustomerLevel Update(CustomerLevel customerLevel);
        CustomerLevel GetCustomerLevel(int customerLevelId);
        void SaveAllCustomerLevel(List<CustomerLevel> customerLevels);
        bool Delete(int customerLevelId);
        bool Exists(Expression<Func<CustomerLevel, bool>> @where);
        PagedDataInquiryResponse<CustomerLevelViewModel> GetCustomerLevels(PagedDataRequest requestInfo, Expression<Func<CustomerLevel, bool>> @where = null);
        CustomerLevel[] GetCustomerLevels(Expression<Func<CustomerLevel, bool>> @where = null);
        IQueryable<CustomerLevel> GetActiveCustomerLevels();
        IQueryable<CustomerLevel> GetDeletedCustomerLevels();
        CustomerLevel Save(CustomerLevel customerLevel);
        int SaveAll(List<CustomerLevel> customerLevels);
    }
}
