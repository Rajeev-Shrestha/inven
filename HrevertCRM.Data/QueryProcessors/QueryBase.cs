using System;
using Hrevert.Common.Security;
using HrevertCRM.Data.Common;
using HrevertCRM.Entities;
using System.Linq;
using System.Linq.Expressions;
using Hrevert.Common.Constants.Others;
using Hrevert.Common.Constants.User;
using Microsoft.EntityFrameworkCore;

namespace HrevertCRM.Data.QueryProcessors
{
    public class QueryBase<T> where T : BaseEntity
    {
        private  IUserSession _userSession;
        protected IDbContext _dbContext;
        protected ApplicationUser LoggedInUser;

        public QueryBase(IUserSession userSession, IDbContext dbContext)
        {
            string defaultUser = "admin@hrevertcrm.com";
            _dbContext = dbContext;
            _userSession = userSession;
            string username;
            try
            {
                username = _userSession.Username ?? defaultUser; //TODO: backup plan for seeding, should be removed in production
            }
            catch (Exception)
            {
                username = defaultUser; //TODO: backup plan for seeding, should be removed in production
            }
           LoggedInUser = _dbContext.Set<ApplicationUser>().AsNoTracking().Single(x => x.UserName == username);
            if (LoggedInUser == null)
            {
                throw new RootObjectNotFoundException(UserConstants.UserQueryProcessorConstants.UserNotFound);
            }
        }
        public Expression<Func<T, bool>> FilterByActiveTrueAndCompany
        {
            get { return x => x.CompanyId == LoggedInUser.CompanyId && x.Active; }
        }
        public Expression<Func<T, bool>> FilterByActiveFalseAndCompany
        {
            get { return x => x.CompanyId == LoggedInUser.CompanyId && x.Active == false; }
        }

        public void CheckVersionMismatch(BaseEntity changed, BaseEntity original)
        {
            var isVersionCorrect = changed.Version.SequenceEqual(original.Version);

            if (!isVersionCorrect)
            {
                throw new VersionMismatchException(OtherConstants.VersionMismatchExceptions.VersionMismatchException);
            }
        }

        public ApplicationUser ActiveUser => LoggedInUser;

        protected void ValidateAuthorization(BaseEntity entity)
        {
            if (LoggedInUser.CompanyId != entity.CompanyId)
            {
                throw new AuthorizationValidationException(OtherConstants.AuthorizationValidationExceptions.AuthorizationValidationException);
            }
        }
    }
}