using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface ISecurityGroupQueryProcessor
    {
        SecurityGroup Save(SecurityGroup securityGroup);
        bool Exists(Expression<Func<SecurityGroup, bool>> @where);
        SecurityGroup GetSecurityGroup(int securityGroupId);
        PagedDataInquiryResponse<SecurityGroupViewModel> GetSecurityGroups(PagedDataRequest requestInfo,
            Expression<Func<SecurityGroup, bool>> @where = null);
        SecurityGroup[] GetSecurityGroup(Expression<Func<SecurityGroup, bool>> @where = null);
        SecurityGroup Update(SecurityGroup securityGroupMember);
        bool Delete(int securityGroupId);
        IQueryable<SecurityGroup> GetActiveSecurityGroups();
        IQueryable<SecurityGroup> GetDeletedSecurityGroups();
        int SaveAll(List<SecurityGroup> securityGroups);
        //This method is only written for the purpose of reducing the repeating codes while seeding in DbContextExtensions
        SecurityGroup CheckIfSecurityGroupExistsOrSave(SecurityGroup securityGroup);
        List<SecurityGroupViewModel> GetSecurityGroupsByLoggedInUserId();
    }
}
