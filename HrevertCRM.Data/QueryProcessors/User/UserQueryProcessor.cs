using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Hrevert.Common.Constants.Others;
using Hrevert.Common.Constants.User;
using HrevertCRM.Entities;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.Mapper;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Web.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace HrevertCRM.Data.QueryProcessors
{
    public class UserQueryProcessor : IUserQueryProcessor
    {
        //private readonly IUserSession _userSession;
        private readonly IDbContext _dbContext;

      //  protected ApplicationUser LoggedInUser;

        public UserQueryProcessor(IDbContext dbContext)
        {
          //  _userSession = userSession;
            _dbContext = dbContext;
        }
        public ApplicationUser UpdateUser(ApplicationUser user, int loggedInUserCompanyId)
        {
            var validUser = GetValidUser(user.Id, user.CompanyId);
            ValidateAuthorization(user, loggedInUserCompanyId);
            CheckVersionMismatch(user);
            
            validUser.FirstName = user.FirstName;
            validUser.MiddleName = user.MiddleName;
            validUser.LastName = user.LastName;
            validUser.Email = user.Email;
            validUser.Phone = user.Phone;
            validUser.UserName = user.UserName;
            validUser.Gender = user.Gender;
            validUser.UserType = user.UserType;
            validUser.Address = user.Address;
            validUser.CompanyName = user.CompanyName;
            validUser.CompanyId = user.CompanyId;
            validUser.Active = user.Active;
            validUser.WebActive = user.WebActive;

            validUser.DateModified = DateTime.Now;
            _dbContext.Set<ApplicationUser>().Update(validUser);
            _dbContext.SaveChanges();
            return validUser;
        }

        protected void ValidateAuthorization(ApplicationUser entity, int loggedInUserCompanyId)
        {
            if (loggedInUserCompanyId != entity.CompanyId)
            {
                throw new AuthorizationValidationException(OtherConstants.AuthorizationValidationExceptions.AuthorizationValidationException);
            }
        }

        public void CheckVersionMismatch(ApplicationUser user)
        {
            var original = GetValidUser(user.Id, user.CompanyId);  
            var isVersionCorrect = user.Version.SequenceEqual(original.Version);
            if (!isVersionCorrect)
            {
                throw new VersionMismatchException(OtherConstants.VersionMismatchExceptions.VersionMismatchException);
            }
        }

        public virtual ApplicationUser GetValidUser(string userId, int companyId)
        {
            var user = _dbContext.Set<ApplicationUser>().AsNoTracking().FirstOrDefault(sc => sc.Id == userId && sc.CompanyId == companyId);
            if (user == null)
            {
                throw new RootObjectNotFoundException(UserConstants.UserQueryProcessorConstants.UserNotFound);
            }
            return user;
        }

        public ApplicationUser GetUser(string userId, int companyId)
        {
            var user = _dbContext.Set<ApplicationUser>().Include(x => x.SecurityGroupMemberUsers).AsNoTracking().FirstOrDefault(d => d.Id == userId && d.Active && d.CompanyId == companyId);
            return user;
        }

        public void SaveAllUser(List<ApplicationUser> users)
        {
            _dbContext.Set<ApplicationUser>().AddRange(users);
            _dbContext.SaveChanges();
        }
        public bool Save(ApplicationUser user)
        {
            var result = _dbContext.Set<ApplicationUser>().Add(user);
            _dbContext.SaveChanges();
            var isSaved = result!= null;
            return isSaved;
        }

        public bool DeleteUser(string userId, int companyId)
        {
            var doc = GetUser(userId, companyId);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<ApplicationUser>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }

        public bool UserExists(Expression<Func<ApplicationUser, bool>> @where)
        {
            return _dbContext.Set<ApplicationUser>().Any(@where);
        }

        public QueryResult<ApplicationUser> GetUser(PagedDataRequest requestInfo, Func<ApplicationUser, bool> @where = null)
        {
            var query = @where == null
                ? _dbContext.Set<ApplicationUser>()
                : _dbContext.Set<ApplicationUser>().Where(@where);
            var enumerable = query as ApplicationUser[] ?? query.ToArray();
            var totalItemCount = enumerable.Count();
            var startIndex = ResultsPagingUtility.CalculateStartIndex(requestInfo.PageNumber, requestInfo.PageSize);
            var docs = enumerable.OrderBy(x => x.DateCreated).Skip(startIndex).Take(requestInfo.PageSize).ToList();
            var queryResult = new QueryResult<ApplicationUser>(docs, totalItemCount, requestInfo.PageSize);
            return queryResult;
        }

        public ApplicationUser[] GetUser(Func<ApplicationUser, bool> @where = null)
        {
            var query = @where == null
                ? _dbContext.Set<ApplicationUser>()
                : _dbContext.Set<ApplicationUser>().Where(@where);
            var enumerable = query as ApplicationUser[] ?? query.ToArray();
            return enumerable;
        }

        public IQueryable<ApplicationUser> GetActiveUsers()
        {
            return _dbContext.Set<ApplicationUser>().Where(x => x.Active);
        }

        public IQueryable<ApplicationUser> GetDeletedUsers()
        {
            return _dbContext.Set<ApplicationUser>().Where(x => x.Active == false);
        }

        public List<string> GetGroupNamesFromGroupIds(List<int> securityGroupIds)
        {
            return securityGroupIds.Select(groupId => _dbContext.Set<SecurityGroup>().Where(x => x.Id == groupId).Select(x => x.GroupName).Single()).ToList();
        }

        public PagedDataInquiryResponse<UserViewModel> SearchActive(PagedDataRequest requestInfo, Expression<Func<ApplicationUser, bool>> where = null)
        {
            var filteredUser = _dbContext.Set<ApplicationUser>().Where(user => user.Active);
            var query = string.IsNullOrEmpty(requestInfo.SearchText) ? filteredUser : filteredUser.Where(s
                                                                  => s.Address.ToUpper().Contains(requestInfo.SearchText.ToUpper())
                                                                  || s.FirstName.ToUpper().Contains(requestInfo.SearchText.ToUpper())
                                                                  || s.MiddleName.ToUpper().Contains(requestInfo.SearchText.ToUpper())
                                                                  || s.LastName.ToUpper().Contains(requestInfo.SearchText.ToUpper())
                                                                  || s.Phone.ToUpper().Contains(requestInfo.SearchText.ToUpper())
                                                                  );
            //TODO: Make this LINQ query precompiled, using the method 
            return FormatResultForPaging(requestInfo, query);
        }

        private static PagedDataInquiryResponse<UserViewModel> FormatResultForPaging(PagedDataRequest requestInfo, IQueryable<ApplicationUser> query)
        {
            var totalItemCount = query.Count();
            var startIndex = ResultsPagingUtility.CalculateStartIndex(requestInfo.PageNumber, requestInfo.PageSize);
            var mapper = new UserToUserViewModelMapper();
            var docs =
                query.Where(s => s.CompanyId == 1) //TODO: we have assign the CompanyId dynamically here instead of 1
                    .OrderBy(x => x.DateCreated)
                    .Skip(startIndex)
                    .Take(requestInfo.PageSize)
                    .Select(
                        s => mapper.Map(s)).ToList();

            var queryResult = new QueryResult<UserViewModel>(docs, totalItemCount, requestInfo.PageSize);
            var inquiryResponse = new PagedDataInquiryResponse<UserViewModel>
            {
                Items = docs,
                TotalRecords = totalItemCount,
                PageCount = queryResult.TotalPageCount,
                PageNumber = requestInfo.PageNumber,
                PageSize = requestInfo.PageSize
            };
            return inquiryResponse;
        }

        public PagedDataInquiryResponse<UserViewModel> SearchAll(PagedDataRequest requestInfo, Expression<Func<ApplicationUser, bool>> @where = null)
        {
            var filteredUser = _dbContext.Set<ApplicationUser>();
            var query = string.IsNullOrEmpty(requestInfo.SearchText) ? filteredUser : filteredUser.Where(s
                                                                  => s.Address.ToUpper().Contains(requestInfo.SearchText.ToUpper())
                                                                  || s.FirstName.ToUpper().Contains(requestInfo.SearchText.ToUpper())
                                                                  || s.MiddleName.ToUpper().Contains(requestInfo.SearchText.ToUpper())
                                                                  || s.LastName.ToUpper().Contains(requestInfo.SearchText.ToUpper())
                                                                  || s.Phone.ToUpper().Contains(requestInfo.SearchText.ToUpper())
                                                                  );
            //TODO: Make this LINQ query precompiled, using the method 
            return FormatResultForPaging(requestInfo, query);
        }

        public ApplicationUser ActivateUser(string id, int companyId)
        {
            var original = GetValidUser(id, companyId);
            //ValidateAuthorization(original);

            original.Active = true;
            _dbContext.Set<ApplicationUser>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        public ApplicationUser Get(string email)
        {
            return _dbContext.Set<ApplicationUser>().SingleOrDefault(x => x.Email == email);
        }

       public IQueryable<ApplicationUser> GetActiveUsersWithoutPaging(int companyId)
        {
            var users = _dbContext.Set<ApplicationUser>().Where(x=>x.Active && x.CompanyId == companyId);   //TODO: we have assign the CompanyId dynamically here instead of 1
            return users;
        }

        public List<TaskDocIdViewModelForUser> GetUserNames(int companyId)
        {
            var userNamesList = new List<TaskDocIdViewModelForUser>();
            var userNames = _dbContext.Set<ApplicationUser>().Where(x => x.Active && x.CompanyId == companyId).Select(x => new {x.Id, x.FirstName, x.MiddleName, x.LastName});   //TODO: we have assign the CompanyId dynamically here instead of 1
            foreach (var userName in userNames)
            {
                var name = userName.MiddleName == null
                    ? userName.FirstName + " " + userName.LastName
                    : userName.FirstName + " " + userName.MiddleName + " " + userName.LastName;
                userNamesList.Add(new TaskDocIdViewModelForUser
                {
                    Id = userName.Id,
                    Name = name
                });
            }
            return userNamesList;
        }

        public ApplicationUser CheckIfDeletedUserWithSameEmailExists(string email, int companyId)
        {
            var user = _dbContext.Set<ApplicationUser>().FirstOrDefault( x => x.CompanyId == companyId &&
                            x.Email == email && (x.Active || x.Active == false));
            return user;
        }
        public ApplicationUser GetUserByUserId(string userId)
        {
            var user= _dbContext.Set<ApplicationUser>().FirstOrDefault(x=>x.Id==userId);
            return user;
        }

        public bool DeleteRange(List<string> usersId, int companyId)
        {
            var userList = usersId.Select(userId => _dbContext.Set<ApplicationUser>().FirstOrDefault(x => x.Id == userId && x.CompanyId == companyId)).ToList();
            userList.ForEach(x => x.Active = false);
            _dbContext.Set<ApplicationUser>().UpdateRange(userList);
            _dbContext.SaveChanges();
            return true;
        }
    }
}
