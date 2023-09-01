using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Hrevert.Common.Constants.Security;
using Hrevert.Common.Security;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using HrevertCRM.Data.Mapper;
using PagedTaskDataInquiryResponse = HrevertCRM.Data.Common.PagedDataInquiryResponse<HrevertCRM.Data.ViewModels.SecurityGroupViewModel>; 

namespace HrevertCRM.Data.QueryProcessors
{
    public class SecurityGroupQueryProcessor : QueryBase<SecurityGroup>, ISecurityGroupQueryProcessor
    {
        public SecurityGroupQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
        {
        }

        public SecurityGroup Save(SecurityGroup securtiyGroup)
        {
            securtiyGroup.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<SecurityGroup>().Add(securtiyGroup);
            _dbContext.SaveChanges();
            return securtiyGroup;
        }

        public bool Exists(Expression<Func<SecurityGroup, bool>> @where)
        {
            return _dbContext.Set<SecurityGroup>().Any(@where);
        }

        public SecurityGroup GetSecurityGroup(int securityGroupId)
        {
            var securityGroup = _dbContext.Set<SecurityGroup>().FirstOrDefault(x => x.Id == securityGroupId);
            return securityGroup;
        }

        public PagedTaskDataInquiryResponse GetSecurityGroups(PagedDataRequest requestInfo, Expression<Func<SecurityGroup, bool>> @where = null)
        {
            var query = _dbContext.Set<SecurityGroup>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            //var enumerable = query as ProductCategory[] ?? query.ToArray();
            var totalItemCount = query.Count();
            var startIndex = ResultsPagingUtility.CalculateStartIndex(requestInfo.PageNumber, requestInfo.PageSize);
            var mapper = new SecurityGroupToSecurityGroupViewModelMapper();
            var docs = query.OrderBy(x => x.DateCreated).Skip(startIndex).Take(requestInfo.PageSize).Select(s => mapper.Map(s)).ToList();
            var queryResult = new QueryResult<SecurityGroupViewModel>(docs, totalItemCount, requestInfo.PageSize);

            var inquiryResponse = new PagedTaskDataInquiryResponse()
            {
                Items = docs,
                TotalRecords = totalItemCount,
                PageCount = queryResult.TotalPageCount,
                PageNumber = requestInfo.PageNumber,
                PageSize = requestInfo.PageSize
            };
            return inquiryResponse;
        }

        public SecurityGroup[] GetSecurityGroup(Expression<Func<SecurityGroup, bool>> @where = null)
        {
            var query = _dbContext.Set<SecurityGroup>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);
            var security = new List<SecurityGroup>();
            var enumerable = query.ToArray();
            return enumerable;
        }

     
        public SecurityGroup Update(SecurityGroup securityGroup)
        {
            var original = GetValidSecurityGroup(securityGroup.Id);
            ValidateAuthorization(securityGroup);
            CheckVersionMismatch(securityGroup, original);

            original.GroupName = securityGroup.GroupName;
            original.GroupDescription = securityGroup.GroupDescription;
            original.Members = securityGroup.Members;
            original.Rights = securityGroup.Rights;
            original.Active = securityGroup.Active;
            original.WebActive = securityGroup.WebActive;

            _dbContext.Set<SecurityGroup>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        public virtual SecurityGroup GetValidSecurityGroup(int securityGroupId)
        {
            var securityGroup = _dbContext.Set<SecurityGroup>().FirstOrDefault(sc => sc.Id == securityGroupId);
            if (securityGroup == null)
            {
                throw new RootObjectNotFoundException(SecurityConstants.SecurityGroupQueryProcessorConstants.SecurityGroupNotFound);
            }
            return securityGroup;
        }

        public bool Delete(int securityGroupId)
        {
            var doc = GetSecurityGroup(securityGroupId);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<SecurityGroup>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }

        public IQueryable<SecurityGroup> GetActiveSecurityGroups()
        {
            return _dbContext.Set<SecurityGroup>().Where(x => x.CompanyId == LoggedInUser.CompanyId && x.Active);
        }

        public IQueryable<SecurityGroup> GetDeletedSecurityGroups()
        {
            return _dbContext.Set<SecurityGroup>().Where(x => x.CompanyId == LoggedInUser.CompanyId && x.Active == false);
        }

        public SecurityGroup CheckIfSecurityGroupExistsOrSave(SecurityGroup securityGroup)
        {
            var @group = securityGroup;
            securityGroup = _dbContext.Set<SecurityGroup>().Any(row => row.GroupName == @group.GroupName) ? _dbContext.Set<SecurityGroup>().FirstOrDefault(x => x.GroupName == @group.GroupName) : Save(securityGroup);
            return securityGroup;
        }

        public List<SecurityGroupViewModel> GetSecurityGroupsByLoggedInUserId()
        {
            var securityGroups =
                _dbContext.Set<SecurityGroup>().Where(x => x.CompanyId == LoggedInUser.CompanyId && x.Active).ToList();
            var securityGroupMapper = new SecurityGroupToSecurityGroupViewModelMapper();
            return securityGroups == null ? null : securityGroupMapper.Map(securityGroups);
        }

        public int SaveAll(List<SecurityGroup> securityGroups)
        {
            _dbContext.Set<SecurityGroup>().AddRange(securityGroups);
            return _dbContext.SaveChanges();
        }
    }
}
