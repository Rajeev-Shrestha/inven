using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;

namespace HrevertCRM.Data.QueryProcessors.Enumerations
{
    public interface IUserTypesQueryProcessor
    {
        UserTypes Update(UserTypes userTypes);
        UserTypes GetValidUserTypes(int userTypesId);
        UserTypes GetUserTypes(int userTypesId);
        void SaveAllUserTypes(List<UserTypes> userTypes);
        UserTypes Save(UserTypes userTypes);
        int SaveAll(List<UserTypes> userTypes);
        UserTypes ActivateUserTypes(int id);
        UserTypeViewModel GetUserTypesViewModel(int id);
        bool Delete(int userTypesId);
        bool Exists(Expression<Func<UserTypes, bool>> @where);
        UserTypes[] GetUserTypes(Expression<Func<UserTypes, bool>> @where = null);
        IQueryable<UserTypes> GetActiveUserTypes();
        IQueryable<UserTypes> GetDeletedUserTypes();
        IQueryable<UserTypes> GetAllUserTypes();
    }
}