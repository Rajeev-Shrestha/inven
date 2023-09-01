using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface ICustomerQueryProcessor
    {
        Customer Update(Customer customer);
        EditCustomerViewModel GetCustomerViewModel(int customerId);
        void SaveAllCustomer(List<Customer> customers);
        bool Delete(int customerId);
        bool Exists(Expression<Func<Customer, bool>> @where);
        Customer[] GetCustomers(Expression<Func<Customer, bool>> @where = null);
        Customer Save(Customer customer);
        int SaveAll(List<Customer> customers);
        //List<Customer> SearchForCustomers(string searchString);
        Customer ActivateCustomer(int id);
        Address SaveBillingAddress(Address address);
        string GenerateCustomerCode();
        bool CheckIfCustomerCodeExistsOrNot(string customerCode);
        CustomerLoginResultViewModel CustomerLogin(string email, string password);
        List<SalesOrderViewModel> GetOrdersSummary(int customerId);
        IQueryable<Customer> GetActiveCustomersWithoutPaging();
        IQueryable<Customer> GetDeletedCustomersWithoutPaging();
        void ImportCustomers(List<Customer> customers, bool updateExisting);
        IQueryable<Address> GetCustomerAllAddresses(int customerId);
        Customer CheckIfDeletedCustomerWithSameCodeExists(string customerCode);
        PagedDataInquiryResponse<EditCustomerViewModel> SearchCustomers(PagedDataRequest requestInfo, Expression<Func<Customer, bool>> @where = null);
        bool DeleteRange(List<int?> customersId);
    }
}
