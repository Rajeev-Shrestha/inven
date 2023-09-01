using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;

namespace HrevertCRM.Data.QueryProcessors.Enumerations
{
    public interface IAddressTypesQueryProcessor
    {
        AddressTypes Update(AddressTypes addressTypes);
        AddressTypes GetValidAddressTypes(int addressTypesId);
        AddressTypes GetAddressTypes(int addressTypesId);
        void SaveAllAddressTypes(List<AddressTypes> addressTypes);
        AddressTypes Save(AddressTypes addressTypes);
        int SaveAll(List<AddressTypes> addressTypes);
        AddressTypes ActivateAddressTypes(int id);
        AddressTypeViewModel GetAddressTypesViewModel(int id);
        bool Delete(int addressTypesId);
        bool Exists(Expression<Func<AddressTypes, bool>> @where);
        AddressTypes[] GetAddressTypes(Expression<Func<AddressTypes, bool>> @where = null);
        IQueryable<AddressTypes> GetActiveAddressTypes();
        IQueryable<AddressTypes> GetDeletedAddressTypes();
        IQueryable<AddressTypes> GetAllAddressTypes();
    }
}