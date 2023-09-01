using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface IAddressQueryProcessor
    {
        Address Update(Address address);
        void SaveAllAddresses(List<Address> addresses);
        void UpdateAddresses(List<Address> addresses);
        Address GetAddress(int addressId);
        void SaveAll(List<AddressViewModel> addresses);
        bool Delete(int addressId);
        bool Exists(Expression<Func<Address, bool>> @where);
        QueryResult<Address> GetAddresses(PagedDataRequest requestInfo, Expression<Func<Address, bool>> @where = null);
        Address[] GetAddresses(Expression<Func<Address, bool>> @where = null);
        IQueryable<Address> GetActiveAddresses();
        IQueryable<Address> GetDeletedAddresses();
        Address Save(Address address);

        Address ActivateAddress(int addressId);
    }
}
