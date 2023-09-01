using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface ISecurityRightQueryProcessor
    {
        SecurityRight Save(SecurityRight securityRight);
        SecurityRight AssignRemoveSecurityRightToGroup(AssignSecurityToGroupViewModel securityInfo);
        bool Exists(Expression<Func<SecurityRight, bool>> @where);
        SecurityRight GetSecurityRight(int securityRightId);
        List<SecurityRightViewModel> GetAssignedGroupSecurity(int securityGroupId);
        QueryResult<SecurityRight> GetSecurityRights(PagedDataRequest requestInfo,
            Expression<Func<SecurityRight, bool>> @where = null);
        SecurityRight[] GetSecurityRight(Expression<Func<SecurityRight, bool>> @where = null);
        bool Delete(int securityRightId);
        SecurityRight Update(SecurityRight securityRight);
        IQueryable<SecurityRight> GetActiveSecurityRights();
        IQueryable<SecurityRight> GetDeletedSecurityRights();
        int SaveAll(List<SecurityRight> securityRights);
        List<SecurityRightViewModel> GetAssignedUserSecurity(string userId);
        SecurityRight AssignRemoveSecurityRightToUser(AssignSecurityToUserViewModel securityInfo);
    }
}
