using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;

namespace HrevertCRM.Data.QueryProcessors.Enumerations
{
    public interface IEncryptionTypesQueryProcessor
    {
        EncryptionTypes Update(EncryptionTypes encryptionTypes);
        EncryptionTypes GetValidEncryptionTypes(int encryptionTypesId);
        EncryptionTypes GetEncryptionTypes(int encryptionTypesId);
        void SaveAllEncryptionTypes(List<EncryptionTypes> encryptionTypes);
        EncryptionTypes Save(EncryptionTypes encryptionTypes);
        int SaveAll(List<EncryptionTypes> encryptionTypes);
        EncryptionTypes ActivateEncryptionTypes(int id);
        EncryptionTypeViewModel GetEncryptionTypesViewModel(int id);
        bool Delete(int encryptionTypesId);
        bool Exists(Expression<Func<EncryptionTypes, bool>> @where);
        EncryptionTypes[] GetEncryptionTypes(Expression<Func<EncryptionTypes, bool>> @where = null);
        IQueryable<EncryptionTypes> GetActiveEncryptionTypes();
        IQueryable<EncryptionTypes> GetDeletedEncryptionTypes();
        IQueryable<EncryptionTypes> GetAllEncryptionTypes();
    }
}