using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Hrevert.Common.Constants.Security;
using Hrevert.Common.Security;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.Mapper.Security;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.QueryProcessors
{
    public class SecurityRightQueryProcessor : QueryBase<SecurityRight>, ISecurityRightQueryProcessor
    {
        public SecurityRightQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
        {
        }

        public List<SecurityRightViewModel> GetAssignedGroupSecurity(int securityGroupId)
        {
            var securities = _dbContext.Set<Security>().Where(x => x.Active).ToList();
            var securityRights = _dbContext.Set<SecurityRight>()
                .Where(x => x.Active && x.SecurityGroupId == securityGroupId && x.CompanyId == LoggedInUser.CompanyId)
                .ToList();
            var securityToRightMapper = new SecurityToSecurityRightViewModelMapper();
            var securityRightViewModels = securityToRightMapper.Map(securities);
            foreach (var securityRight in securityRights)
            {
                var right = securityRightViewModels.FirstOrDefault(s => s.SecurityId == securityRight.SecurityId);
                if (right != null)
                {
                    right.Allowed = securityRight.Allowed;
                }
            }
            return securityRightViewModels;
        }

        public List<SecurityRightViewModel> GetAssignedUserSecurity(string userId)
        {
            var securities = _dbContext.Set<Security>().Where(x => x.Active).ToList();
            var groupsUserIsIn = _dbContext.Set<SecurityGroupMember>()
                .Where(x => x.MemberId == userId && x.Active && x.CompanyId == LoggedInUser.CompanyId)
                .Select(x => x.SecurityGroupId).ToList();
            var securityRights = new List<SecurityRight>();
            if (groupsUserIsIn != null && groupsUserIsIn.Count > 0)
            {
                foreach (var groupId in groupsUserIsIn)
                {
                    securityRights.AddRange(_dbContext.Set<SecurityRight>().Where(x => x.Active && x.SecurityGroupId == groupId && x.CompanyId == LoggedInUser.CompanyId).ToList());
                }
            }
            var securityToRightMapper = new SecurityToSecurityRightViewModelMapper();
            var securityRightViewModels = securityToRightMapper.Map(securities);
            foreach (var securityRight in securityRightViewModels)
            {
                var right = securityRights.FirstOrDefault(s => s.SecurityId == securityRight.SecurityId && s.Allowed);
                if (right != null)
                {
                    securityRight.Allowed = right.Allowed;
                }
            }
            return securityRightViewModels;
        }


        public SecurityRight AssignRemoveSecurityRightToGroup(AssignSecurityToGroupViewModel securityInfo)
        {
            var security =
               _dbContext.Set<SecurityRight>().FirstOrDefault(s => s.SecurityGroupId == securityInfo.GroupId && s.SecurityId == securityInfo.SecurityId);

            if (security == null)
            {
                security = new SecurityRight
                {
                    SecurityGroupId = securityInfo.GroupId,
                    SecurityId = securityInfo.SecurityId,
                    CompanyId = LoggedInUser.CompanyId,
                    Allowed = securityInfo.IsAssigned
                };
                _dbContext.Set<SecurityRight>().Add(security);
            }
            else
            {
                security.Allowed = securityInfo.IsAssigned;
                _dbContext.Set<SecurityRight>().Update(security);
            }
            _dbContext.SaveChanges();
            return security;
        }

        public SecurityRight AssignRemoveSecurityRightToUser(AssignSecurityToUserViewModel securityInfo)
        {
            var security =
                _dbContext.Set<SecurityRight>().FirstOrDefault(s => s.UserId == securityInfo.UserId && s.SecurityId == securityInfo.SecurityId);

            if (security == null)
            {
                security = new SecurityRight
                {
                    UserId = securityInfo.UserId,
                    SecurityId = securityInfo.SecurityId,
                    CompanyId = LoggedInUser.CompanyId,
                    Allowed = securityInfo.IsAssigned
                };
                _dbContext.Set<SecurityRight>().Add(security);
            }
            else
            {
                security.UserId = securityInfo.UserId;
                security.Allowed = securityInfo.IsAssigned;
                _dbContext.Set<SecurityRight>().Update(security);
            }
            _dbContext.SaveChanges();
            return security;
        }

        public SecurityRight Save(SecurityRight security)
        {
            security.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<SecurityRight>().Add(security);
            _dbContext.SaveChanges();
            return security;
        }

        public bool Exists(Expression<Func<SecurityRight, bool>> @where)
        {
            return _dbContext.Set<SecurityRight>().Any(@where);
        }

        public SecurityRight GetSecurityRight(int securityRightId)
        {
            var securityRight = _dbContext.Set<SecurityRight>().FirstOrDefault(x => x.Id == securityRightId);
            return securityRight;
        }

        public QueryResult<SecurityRight> GetSecurityRights(PagedDataRequest requestInfo, Expression<Func<SecurityRight, bool>> @where = null)
        {
            var query = _dbContext.Set<SecurityRight>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            //var enumerable = query as ProductCategory[] ?? query.ToArray();
            var totalItemCount = query.Count();
            var startIndex = ResultsPagingUtility.CalculateStartIndex(requestInfo.PageNumber, requestInfo.PageSize);
            var docs = query.OrderBy(x => x.DateCreated).Skip(startIndex).Take(requestInfo.PageSize).ToList();
            var queryResult = new QueryResult<SecurityRight>(docs, totalItemCount, requestInfo.PageSize);
            return queryResult;
        }

        public SecurityRight[] GetSecurityRight(Expression<Func<SecurityRight, bool>> @where = null)
        {
            var query = _dbContext.Set<SecurityRight>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            var enumerable = query.ToArray();
            return enumerable;
        }

        public bool Delete(int securityRightId)
        {
            var doc = GetSecurityRight(securityRightId);
            var result = 0;
            if (doc == null) return result > 0;
            _dbContext.Set<SecurityRight>().Remove(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }

        public SecurityRight Update(SecurityRight securityRight)
        {
            var original = GetValidSecurityRight(securityRight.Id);
            ValidateAuthorization(securityRight);
            CheckVersionMismatch(securityRight, original);

            original.Allowed = securityRight.Allowed;
            original.SecurityGroupId = securityRight.SecurityGroupId;
            original.SecurityId = securityRight.SecurityId;
            original.Active = securityRight.Active;
            original.WebActive = securityRight.WebActive;

            _dbContext.Set<SecurityRight>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        public IQueryable<SecurityRight> GetActiveSecurityRights()
        {
            return _dbContext.Set<SecurityRight>().Where(x => x.CompanyId == LoggedInUser.CompanyId && x.Active);
        }

        public IQueryable<SecurityRight> GetDeletedSecurityRights()
        {
            return _dbContext.Set<SecurityRight>().Where(x => x.CompanyId == LoggedInUser.CompanyId && x.Active == false);
        }

        public virtual SecurityRight GetValidSecurityRight(int securityRightId)
        {
            var securityRight = _dbContext.Set<SecurityRight>().FirstOrDefault(sc => sc.Id == securityRightId);
            if (securityRight == null)
            {
                throw new RootObjectNotFoundException(SecurityConstants.SecurityRightQueryProcessorConstants.SecurityRightNotFound);
            }
            return securityRight;
        }

        public int SaveAll(List<SecurityRight> securityRights)
        {
            _dbContext.Set<SecurityRight>().AddRange(securityRights);
            return _dbContext.SaveChanges();

        }
    }
}
