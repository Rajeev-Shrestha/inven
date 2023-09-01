using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Hrevert.Common.Security;
using HrevertCRM.Data.Common;
using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore;

namespace HrevertCRM.Data.QueryProcessors
{
    public class SecurityGroupMemberQueryProcessor : QueryBase<SecurityGroupMember>, ISecurityGroupMemberQueryProcessor
    {
        public SecurityGroupMemberQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
        {
        }

        public SecurityGroupMember Save(SecurityGroupMember securityGroupMember)
        {
            securityGroupMember.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<SecurityGroupMember>().Add(securityGroupMember);
            _dbContext.SaveChanges();
            return securityGroupMember;
        }

        public bool Exists(Expression<Func<SecurityGroupMember, bool>> @where)
        {
            return _dbContext.Set<SecurityGroupMember>().Any(@where);
        }

        public SecurityGroupMember GetSecurityGroupMember(string memberId, int securityGroupId)
        {
            var groupMember = _dbContext.Set<SecurityGroupMember>().FirstOrDefault(x => x.MemberId == memberId && x.SecurityGroupId == securityGroupId);
            return groupMember;
        }

        public SecurityGroupMember[] GetSecurityGroupMembers(Expression<Func<SecurityGroupMember, bool>> @where)
        {
            var query = _dbContext.Set<SecurityGroupMember>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            var enumerable = query.ToArray();
            return enumerable;
        }

        public QueryResult<SecurityGroupMember> GetSecurityGroupMembers(PagedDataRequest requestInfo, Expression<Func<SecurityGroupMember, bool>> @where = null)
        {
            var query = _dbContext.Set<SecurityGroupMember>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            //var enumerable = query as ProductCategory[] ?? query.ToArray();
            var totalItemCount = query.Count();
            var startIndex = ResultsPagingUtility.CalculateStartIndex(requestInfo.PageNumber, requestInfo.PageSize);
            var docs = query.OrderBy(x => x.DateCreated).Skip(startIndex).Take(requestInfo.PageSize).ToList();
            var queryResult = new QueryResult<SecurityGroupMember>(docs, totalItemCount, requestInfo.PageSize);
            return queryResult;
        }

        public bool Delete(string memberId, int securityGroupId)
        {
            var doc = GetSecurityGroupMember(memberId, securityGroupId);
            var result = 0;
            if (doc == null) return result > 0;
            _dbContext.Set<SecurityGroupMember>().Remove(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }

        public IQueryable<SecurityGroupMember> GetActiveSecurityGroupMembers()
        {
            return _dbContext.Set<SecurityGroupMember>().Where(x => x.CompanyId == LoggedInUser.CompanyId && x.Active);
        }

        public IQueryable<SecurityGroupMember> GetDeletedSecurityGroupMembers()
        {
            return _dbContext.Set<SecurityGroupMember>().Where(x => x.CompanyId == LoggedInUser.CompanyId && x.Active == false);
        }

        public SecurityGroupMember CheckIfSecurityGroupMemberExistsOrSave(SecurityGroupMember securityGroupMember)
        {
            var member = securityGroupMember;
            securityGroupMember = _dbContext.Set<SecurityGroupMember>().Any(row => row.MemberId == member.MemberId && row.SecurityGroupId == member.SecurityGroupId) ? _dbContext.Set<SecurityGroupMember>().FirstOrDefault(x => x.MemberId == securityGroupMember.MemberId && x.SecurityGroupId == securityGroupMember.SecurityGroupId) : Save(securityGroupMember);
            return securityGroupMember;
        }

        public IQueryable<ApplicationUser> GetMembers(int id)
        {
            var members =
                _dbContext.Set<SecurityGroupMember>()
                    .Where(x => x.SecurityGroupId == id)
                    .Include(x=>x.MemberUser)
                    .Select(x => x.MemberUser);
                    
            return members;
        }

        public List<string> GetExistingMembersOfGroup(int securityGroupId)
        {
            var existingMembers = _dbContext.Set<SecurityGroupMember>().Where(p => p.SecurityGroupId == securityGroupId).Select(p => p.MemberId).ToList();

            return existingMembers;
        }

        public SecurityGroupMember SaveSecurityGroupMember(SecurityGroupMember securityGroupMember)
        {
            securityGroupMember.Active = true;
            _dbContext.Set<SecurityGroupMember>().Add(securityGroupMember);
            _dbContext.SaveChanges();
            return securityGroupMember;
        }

        public List<int> GetExistingRolesOfUser(string id)
        {
            var existingRoles =
                _dbContext.Set<SecurityGroupMember>()
                    .Where(x => x.MemberId == id && x.CompanyId == LoggedInUser.CompanyId && x.Active)
                    .Select(x => x.SecurityGroupId)
                    .ToList();
            return existingRoles;
        }

        public List<int> GetSecurityGroupsOfMemberId(string userId)
        {
            return _dbContext.Set<SecurityGroupMember>()
                .Where(x => x.MemberId == userId && x.Active && x.CompanyId == LoggedInUser.CompanyId)
                .Select(x => x.SecurityGroupId).ToList();
        }

        public int SaveAll(List<SecurityGroupMember> allGroupMembers)
        {
            allGroupMembers.ForEach(g => g.CompanyId = LoggedInUser.CompanyId);
            _dbContext.Set<SecurityGroupMember>().AddRange(allGroupMembers);
            return _dbContext.SaveChanges();
        }

        public int SaveAllSeed(List<SecurityGroupMember> allGroupMembers)
        {
            _dbContext.Set<SecurityGroupMember>().AddRange(allGroupMembers);
            return _dbContext.SaveChanges();
        }
    }
}
