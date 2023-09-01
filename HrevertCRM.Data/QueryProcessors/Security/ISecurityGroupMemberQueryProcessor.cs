using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HrevertCRM.Data.Common;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface ISecurityGroupMemberQueryProcessor
    {
        //SecurityGroupMember UpdateSecurityGroupMember(SecurityGroupMember securityGroupMember);
        SecurityGroupMember Save(SecurityGroupMember security);
        int SaveAllSeed(List<SecurityGroupMember> allGroupMembers); //TODO: This is for seeding
        bool Exists(Expression<Func<SecurityGroupMember, bool>> @where);
        SecurityGroupMember GetSecurityGroupMember(string memberId, int securityGroupId);
        SecurityGroupMember[] GetSecurityGroupMembers(Expression<Func<SecurityGroupMember, bool>> @where);
        QueryResult<SecurityGroupMember> GetSecurityGroupMembers(PagedDataRequest requestInfo,
            Expression<Func<SecurityGroupMember, bool>> @where = null);
        bool Delete(string memberId, int securityGroupId);
        IQueryable<SecurityGroupMember> GetActiveSecurityGroupMembers();
        IQueryable<SecurityGroupMember> GetDeletedSecurityGroupMembers();
        int SaveAll(List<SecurityGroupMember> allGroupMembers);
        //This method is only written for the purpose of reducing the repeating codes while seeding in DbContextExtensions
        SecurityGroupMember CheckIfSecurityGroupMemberExistsOrSave(SecurityGroupMember securityGroupMember);
        IQueryable<ApplicationUser> GetMembers(int id);
        List<string> GetExistingMembersOfGroup(int securityGroupId);
        SecurityGroupMember SaveSecurityGroupMember(SecurityGroupMember newSecurityGroupMember);
        List<int> GetExistingRolesOfUser(string id);
        List<int> GetSecurityGroupsOfMemberId(string userId);
    }
}
