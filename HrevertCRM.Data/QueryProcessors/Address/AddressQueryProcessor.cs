using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Hrevert.Common.Constants.Address;
using Hrevert.Common.Security;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.Mapper;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore;

namespace HrevertCRM.Data.QueryProcessors
{
    public class AddressQueryProcessor : QueryBase<Address>, IAddressQueryProcessor
    {
        public AddressQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
        {
        }
        public Address Update(Address address)
        {
            var original = GetValidAddress(address.Id);
            ValidateAuthorization(address);
            CheckVersionMismatch(address, original);   //TODO: to test this method comment this out

            original.UserId = address.UserId;
            original.AddressType = address.AddressType;
            original.Fax = address.Fax;
            original.Title = address.Title;
            original.Suffix = address.Suffix;
            original.FirstName = address.FirstName;
            original.LastName = address.LastName;
            original.MiddleName = address.MiddleName;
            original.AddressLine1 = address.AddressLine1;
            original.AddressLine2 = address.AddressLine2;
            original.City = address.City;
            original.State = address.State;
            original.ZipCode = address.ZipCode;
            original.CountryId = address.CountryId;
            original.Telephone = address.Telephone;
            original.MobilePhone = address.MobilePhone;
            original.Email = address.Email;
            original.Website = address.Website;
            original.DeliveryZoneId = address.DeliveryZoneId;
            original.Active = address.Active;
            original.WebActive = address.WebActive;
            original.IsDefault = address.IsDefault;
            _dbContext.Set<Address>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        public void UpdateAddresses(List<Address> addresses)
        {
            foreach (var address in addresses)
            {
                var original = GetValidAddress(address.Id);

                //UserId = address.UserId,
                //AddressType = address.AddressType,
                original.FirstName = address.FirstName;
                original.LastName = address.LastName;
                original.MiddleName = address.MiddleName;
                original.AddressLine1 = address.AddressLine1;
                original.AddressLine2 = address.AddressLine2;
                original.City = address.City;
                //original.CustomerId = address.CustomerId;
                original.State = address.State;
                original.CountryId = address.CountryId;
                original.Telephone = address.Telephone;
                original.MobilePhone = address.MobilePhone;
                original.Email = address.Email;
                original.DeliveryZoneId = address.DeliveryZoneId;
                original.IsDefault = address.IsDefault;
                original.WebActive = address.WebActive;
                //CompanyId = address.CompanyId,

                _dbContext.Set<Address>().AsNoTracking();
                _dbContext.Set<Address>().Update(original);
                _dbContext.SaveChanges();
            }
        }

        public virtual Address GetValidAddress(int addressId)
        {
            var address = _dbContext.Set<Address>().AsNoTracking().FirstOrDefault(sc => sc.Id == addressId);
            if (address == null)
            {
                throw new RootObjectNotFoundException(AddressConstants.AddressQueryProcessorConstants.AddressNotFound);
            }
            return address;
        }

        public Address GetAddress(int addressId)
        {
            var address = _dbContext.Set<Address>().FirstOrDefault(d => d.Id == addressId);
            return address;
        }

        public void SaveAll(List<AddressViewModel> addressViewModels)
        {
            var mapper = new AddressToAddressViewModelMapper();
            var addresses = addressViewModels.Select(x => mapper.Map(x)).ToList();
            addresses.ForEach(x => x.CompanyId = LoggedInUser.CompanyId);
            _dbContext.Set<Address>().AddRange(addresses);
            _dbContext.SaveChanges();
        }


        public void SaveAllAddresses(List<Address> addresses)
       {
            addresses.ForEach(x => x.IsDefault = true);
            _dbContext.Set<Address>().AddRange(addresses);
            _dbContext.SaveChanges();
        }

        public Address Save(Address address)
        {
            address.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<Address>().Add(address);
            _dbContext.SaveChanges();
            return address;
        }

        public bool Delete(int addressId)
        {
            var doc = GetAddress(addressId);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<Address>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }

        public bool Exists(Expression<Func<Address, bool>> @where)
        {
            return _dbContext.Set<Address>().Any(@where);
        }

        public QueryResult<Address> GetAddresses(PagedDataRequest requestInfo, Expression<Func<Address, bool>> @where = null)
        {
            var query = _dbContext.Set<Address>().Where(where);
            query = @where == null ? query : query.Where(@where);

            var enumerable = query.ToArray();
            var totalItemCount = enumerable.Count();
            var startIndex = ResultsPagingUtility.CalculateStartIndex(requestInfo.PageNumber, requestInfo.PageSize);
            var docs = enumerable.OrderBy(x => x.DateCreated).Skip(startIndex).Take(requestInfo.PageSize).ToList();
            var queryResult = new QueryResult<Address>(docs, totalItemCount, requestInfo.PageSize);
            return queryResult;
        }

        public Address[] GetAddresses(Expression<Func<Address, bool>> @where = null)
        {
            var query = _dbContext.Set<Address>().Where(where);
            query = @where == null ? query : query.Where(@where);

            var enumerable = query.ToArray();
            return enumerable;
        }

        public IQueryable<Address> GetActiveAddresses()
        {
            return _dbContext.Set<Address>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active);
        }

        public IQueryable<Address> GetDeletedAddresses()
        {
            return _dbContext.Set<Address>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active == false);
        }


        public Address ActivateAddress(int addressId)
        {
            var original = GetValidAddress(addressId);
            ValidateAuthorization(original);
            original.Active = true;
            _dbContext.Set<Address>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
    }
}
