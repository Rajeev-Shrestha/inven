using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface ICustomerContactGroupQueryProcessor
    {
        bool Delete(int customerContactGroupId);
        bool Exists(Expression<Func<CustomerContactGroup, bool>> where);
        //IQueryable<CustomerContactGroup> GetActiveCustomerContactGroups();
        //IQueryable<CustomerContactGroup> GetDeletedCustomerContactGroups();
        //IQueryable<CustomerContactGroup> GetAllCustomerContactGroups();
        CustomerContactGroup GetCustomerContactGroup(int customerContactGroupId);
        CustomerContactGroup[] GetCustomerContactGroups(Expression<Func<CustomerContactGroup, bool>> where = null);
        QueryResult<CustomerContactGroup> GetCustomerContactGroups(PagedDataRequest requestInfo, Expression<Func<CustomerContactGroup, bool>> where = null);
        CustomerContactGroup GetValidCustomerContactGroup(int customerContactGroupId);
        CustomerContactGroup Save(CustomerContactGroup customerContactGroup);
        int SaveAll(List<CustomerContactGroup> customerContactGroups);
        void SaveAllCustomerContactGroup(List<CustomerContactGroup> customerContactGroups);
        CustomerContactGroup Update(CustomerContactGroup customerContactGroup);

        List<CustomerContactGroup> SearchActive(string searchText);
        List<CustomerContactGroup> SearchAll(string searchText);
        CustomerContactGroup ActivateCustomerContactGroup(int id);
        CustomerContactGroup CheckIfDeletedCustomerContactWithSameNameExists(string name);
        PagedDataInquiryResponse<CustomerContactGroupViewModel> GetContactGroups(PagedDataRequest requestInfo, Expression<Func<CustomerContactGroup, bool>> @where = null);

    }
}