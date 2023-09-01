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
    public class AddressTypesQueryProcessor : QueryBase<AddressTypes>, IAddressTypesQueryProcessor
    {
        public AddressTypesQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
        {
        }
        public AddressTypes Update(AddressTypes addressTypes)
        {
            var original = GetValidAddressTypes(addressTypes.Id);
            ValidateAuthorization(addressTypes);
            CheckVersionMismatch(addressTypes, original);

            original.Value = addressTypes.Value;
            original.Active = addressTypes.Active;
            original.CompanyId = addressTypes.CompanyId;

            _dbContext.Set<AddressTypes>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
        public virtual AddressTypes GetValidAddressTypes(int addressTypesId)
        {
            var addressTypes = _dbContext.Set<AddressTypes>().FirstOrDefault(sc => sc.Id == addressTypesId);
            if (addressTypes == null)
            {
                throw new RootObjectNotFoundException("Address Types not found");
            }
            return addressTypes;
        }
        public AddressTypes GetAddressTypes(int addressTypesId)
        {
            var addressTypes = _dbContext.Set<AddressTypes>().FirstOrDefault(d => d.Id == addressTypesId);
            return addressTypes;
        }
        public void SaveAllAddressTypes(List<AddressTypes> addressTypes)
        {
            _dbContext.Set<AddressTypes>().AddRange(addressTypes);
            _dbContext.SaveChanges();
        }
        public AddressTypes Save(AddressTypes addressTypes)
        {
            addressTypes.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<AddressTypes>().Add(addressTypes);
            _dbContext.SaveChanges();
            return addressTypes;
        }
        public int SaveAll(List<AddressTypes> addressTypes)
        {
            _dbContext.Set<AddressTypes>().AddRange(addressTypes);
            return _dbContext.SaveChanges();
        }
        public AddressTypes ActivateAddressTypes(int id)
        {
            var original = GetValidAddressTypes(id);
            ValidateAuthorization(original);

            original.Active = true;
            _dbContext.Set<AddressTypes>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
        public AddressTypeViewModel GetAddressTypesViewModel(int id)
        {
            var addressTypes = _dbContext.Set<AddressTypes>().Single(s => s.Id == id);
            var mapper = new AddressTypeToAddressTypeViewModelMapper();
            return mapper.Map(addressTypes);
        }
        public bool Delete(int addressTypesId)
        {
            var doc = GetAddressTypes(addressTypesId);
            ValidateAuthorization(doc);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<AddressTypes>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }
        public bool Exists(Expression<Func<AddressTypes, bool>> @where)
        {
            return _dbContext.Set<AddressTypes>().Any(@where);
        }
        public AddressTypes[] GetAddressTypes(Expression<Func<AddressTypes, bool>> @where = null)
        {

            var query = _dbContext.Set<AddressTypes>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            var enumerable = query.ToArray();
            return enumerable;
        }
        public IQueryable<AddressTypes> GetActiveAddressTypes()
        {
            return _dbContext.Set<AddressTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active);
        }
        public IQueryable<AddressTypes> GetDeletedAddressTypes()
        {
            return _dbContext.Set<AddressTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active == false);
        }
        public IQueryable<AddressTypes> GetAllAddressTypes()
        {
            var result = _dbContext.Set<AddressTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId);
            return result;
        }
    }
}
