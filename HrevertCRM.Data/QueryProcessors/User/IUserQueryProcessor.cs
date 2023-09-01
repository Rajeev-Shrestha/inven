using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using HrevertCRM.Web.ViewModels;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface IUserQueryProcessor
    {
        ApplicationUser UpdateUser(ApplicationUser user, int loggedInUserCompanyId);
        ApplicationUser GetUser(string userId, int companyId);
        void SaveAllUser(List<ApplicationUser> users);
        bool Save(ApplicationUser user);
        bool DeleteUser(string userId, int companyId);
        bool UserExists(Expression<Func<ApplicationUser, bool>> @where);
        QueryResult<ApplicationUser> GetUser(PagedDataRequest requestInfo, Func<ApplicationUser, bool> @where = null);
        ApplicationUser[] GetUser(Func<ApplicationUser, bool> @where = null);
        IQueryable<ApplicationUser> GetActiveUsers();
        IQueryable<ApplicationUser> GetDeletedUsers();
        List<string> GetGroupNamesFromGroupIds(List<int> securityGroupIds);
        PagedDataInquiryResponse<UserViewModel> SearchActive(PagedDataRequest requestInfo, Expression<Func<ApplicationUser, bool>> @where = null);
        PagedDataInquiryResponse<UserViewModel> SearchAll(PagedDataRequest requestInfo, Expression<Func<ApplicationUser, bool>> @where = null);
        ApplicationUser ActivateUser(string id, int companyId);
        ApplicationUser Get(string email);
        IQueryable<ApplicationUser> GetActiveUsersWithoutPaging(int companyId);
        List<TaskDocIdViewModelForUser> GetUserNames(int companyId);
        ApplicationUser CheckIfDeletedUserWithSameEmailExists(string email, int companyId);
        ApplicationUser GetUserByUserId(string userId);
        bool DeleteRange(List<string> usersId, int companyId);

    }
}
