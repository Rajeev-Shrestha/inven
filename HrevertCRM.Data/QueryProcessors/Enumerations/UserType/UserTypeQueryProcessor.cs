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
    public class UserTypesQueryProcessor : QueryBase<UserTypes>, IUserTypesQueryProcessor
    {
        public UserTypesQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
        {
        }
        public UserTypes Update(UserTypes userTypes)
        {
            var original = GetValidUserTypes(userTypes.Id);
            ValidateAuthorization(userTypes);
            CheckVersionMismatch(userTypes, original);

            original.Value = userTypes.Value;
            original.Active = userTypes.Active;
            original.CompanyId = userTypes.CompanyId;

            _dbContext.Set<UserTypes>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
        public virtual UserTypes GetValidUserTypes(int userTypesId)
        {
            var userTypes = _dbContext.Set<UserTypes>().FirstOrDefault(sc => sc.Id == userTypesId);
            if (userTypes == null)
            {
                throw new RootObjectNotFoundException("User Types not found");
            }
            return userTypes;
        }
        public UserTypes GetUserTypes(int userTypesId)
        {
            var userTypes = _dbContext.Set<UserTypes>().FirstOrDefault(d => d.Id == userTypesId);
            return userTypes;
        }
        public void SaveAllUserTypes(List<UserTypes> userTypes)
        {
            _dbContext.Set<UserTypes>().AddRange(userTypes);
            _dbContext.SaveChanges();
        }
        public UserTypes Save(UserTypes userTypes)
        {
            userTypes.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<UserTypes>().Add(userTypes);
            _dbContext.SaveChanges();
            return userTypes;
        }
        public int SaveAll(List<UserTypes> userTypes)
        {
            _dbContext.Set<UserTypes>().AddRange(userTypes);
            return _dbContext.SaveChanges();
        }
        public UserTypes ActivateUserTypes(int id)
        {
            var original = GetValidUserTypes(id);
            ValidateAuthorization(original);

            original.Active = true;
            _dbContext.Set<UserTypes>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
        public UserTypeViewModel GetUserTypesViewModel(int id)
        {
            var userTypes = _dbContext.Set<UserTypes>().Single(s => s.Id == id);
            var mapper = new UserTypeToUserTypeViewModelMapper();
            return mapper.Map(userTypes);
        }
        public bool Delete(int userTypesId)
        {
            var doc = GetUserTypes(userTypesId);
            ValidateAuthorization(doc);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<UserTypes>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }
        public bool Exists(Expression<Func<UserTypes, bool>> @where)
        {
            return _dbContext.Set<UserTypes>().Any(@where);
        }
        public UserTypes[] GetUserTypes(Expression<Func<UserTypes, bool>> @where = null)
        {

            var query = _dbContext.Set<UserTypes>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            var enumerable = query.ToArray();
            return enumerable;
        }
        public IQueryable<UserTypes> GetActiveUserTypes()
        {
            return _dbContext.Set<UserTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active);
        }
        public IQueryable<UserTypes> GetDeletedUserTypes()
        {
            return _dbContext.Set<UserTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active == false);
        }
        public IQueryable<UserTypes> GetAllUserTypes()
        {
            var result = _dbContext.Set<UserTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId);
            return result;
        }
    }
}
