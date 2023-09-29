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
using Microsoft.EntityFrameworkCore;
using PagedTaskDataInquiryResponse = HrevertCRM.Data.Common.PagedDataInquiryResponse<HrevertCRM.Data.ViewModels.SecurityViewModel>;
using PagedTaskDataInquiryResponseForSearch = HrevertCRM.Data.Common.PagedDataInquiryResponse<HrevertCRM.Data.ViewModels.SecurityViewModel>;


namespace HrevertCRM.Data.QueryProcessors
{
    public class SecurityQueryProcessor : QueryBase<Security>, ISecurityQueryProcessor
    {
        public SecurityQueryProcessor(IUserSession userSession, IDbContext dbContext) 
            : base(userSession, dbContext)
        {
        }
    
        public bool VerifyUserHasRight(long securityCode)
        {
            var right =
                _dbContext.Set<SecurityRight>().Include(x => x.Security)
                    .FirstOrDefault(u => u.UserId == LoggedInUser.Id && u.Security.SecurityCode == securityCode);

            if (right != null)
                if (!right.Allowed)
                {
                    return false;
                }
                else if (right.Allowed)
                    return true;

            var hasRight = _dbContext.Set<SecurityGroupMember>().Include(x => x.SecurityGroup).ThenInclude(x => x.Rights).Where(u => u.MemberId == LoggedInUser.Id)
                .Select(t => t.SecurityGroup.Rights.Select(x => x.Security.SecurityCode == securityCode && x.Allowed).First()).FirstOrDefault();
            //var isDenied = securityRights.Any(r => !r.Allowed);
            //if (!hasRight)
            //{
            //    return false;
            //}
            return hasRight;
        }

        public Security Save(Security securtiy)
        {
            securtiy.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<Security>().Add(securtiy);
            _dbContext.SaveChanges();
            return securtiy;
        }

        public int  SaveAll(List<Security> securities)
        {
            securities.ForEach(x => x.CompanyId = LoggedInUser.CompanyId);
            _dbContext.Set<Security>().AddRange(securities);
            return  _dbContext.SaveChanges();
        }

        public PagedTaskDataInquiryResponse SearchSecurities(PagedDataRequest requestInfo, Expression<Func<Security, bool>> @where = null)
        {
            var query = _dbContext.Set<Security>().AsQueryable();

            if (requestInfo.Active)
                query = query.Where(x => x.Active);
            var result = string.IsNullOrEmpty(requestInfo.SearchText)
                ? query
                : query.Where(x => x.SecurityCode.ToString().Contains(requestInfo.SearchText) ||
                                   x.SecurityDescription.ToUpper().Contains(requestInfo.SearchText.ToUpper())); 

            var totalItemCount = result.Count();
            var startIndex = ResultsPagingUtility.CalculateStartIndex(requestInfo.PageNumber, requestInfo.PageSize);

            var mapper = new SecurityToSecurityViewModelMapper();
            var docs = result.OrderBy(x => x.DateCreated).Skip(startIndex).Take(requestInfo.PageSize).Select(s => mapper.Map(s)).ToList();

            var queryResult = new QueryResult<SecurityViewModel>(docs, totalItemCount, requestInfo.PageSize);
            var inquiryResponse = new PagedTaskDataInquiryResponse
            {
                Items = docs,
                TotalRecords = totalItemCount,
                PageCount = queryResult.TotalPageCount,
                PageNumber = requestInfo.PageNumber,
                PageSize = requestInfo.PageSize
            };
            return inquiryResponse;
        }

        public List<int?> GetGroupsWithAuthorityToAssignRight()
        {
            var groups = _dbContext.Set<SecurityRight>().Include(x => x.Security)
                .Where(x => x.Security.SecurityCode == 100001 && x.CompanyId == LoggedInUser.CompanyId && x.Active && x.Allowed)
                .Select(x => x.SecurityGroupId).ToList();

            return groups;
        }

        public bool Exists(Expression<Func<Security, bool>> @where)
        {
            return _dbContext.Set<Security>().Any(@where);
        }

        public PagedTaskDataInquiryResponse GetSecurities(PagedDataRequest requestInfo, Expression<Func<Security, bool>> @where = null)
        {
            var query = _dbContext.Set<Security>().Where(s=> s.Active);
            query = @where == null ? query : query.Where(@where);

            var totalItemCount = query.Count();
            var startIndex = ResultsPagingUtility.CalculateStartIndex(requestInfo.PageNumber, requestInfo.PageSize);

            var mapper = new SecurityToSecurityViewModelMapper();
            var docs = query.OrderBy(x => x.DateCreated).Skip(startIndex).Take(requestInfo.PageSize).Select(s => mapper.Map(s)).ToList();

            var queryResult = new QueryResult<SecurityViewModel>(docs, totalItemCount, requestInfo.PageSize);
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

        public Security Update(Security security)
        {
            var original = GetValidSecurity(security.Id);
            ValidateAuthorization(security);
            CheckVersionMismatch(security, original);

            original.SecurityCode = security.SecurityCode;
            original.SecurityDescription = security.SecurityDescription;
            original.SecurityRights = security.SecurityRights;
            original.TransactionLogs = security.TransactionLogs;
            original.Active = security.Active;
            original.WebActive = security.WebActive;

            _dbContext.Set<Security>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        public Security GetSecurity(int securityId)
        {
            var security = _dbContext.Set<Security>().FirstOrDefault(x => x.Id == securityId);
            return security;
        }

        public IQueryable<Security> GetActiveSecurities()
        {
            return _dbContext.Set<Security>().Where(x => x.Active);
        }

        public IQueryable<Security> GetDeletedSecurities()
        {
            return _dbContext.Set<Security>().Where(x => x.Active == false);
        }

        public virtual Security GetValidSecurity(int securityId)
        {
            var security = _dbContext.Set<Security>().FirstOrDefault(sc => sc.Id == securityId);
            if (security == null)
            {
                throw new RootObjectNotFoundException(SecurityConstants.SecurityQueryProcessorConstants.SecurityNotFound);
            }
            return security;
        }

        public bool Delete(int securityId)
        {
            var doc = GetSecurity(securityId);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<Security>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }


        public PagedTaskDataInquiryResponseForSearch SearchActive(PagedDataRequest requestInfo, Expression<Func<Security, bool>> @where = null)
        {
            //We need to eliminate companyId checking since therer is no companyid in security model
            var filteredSecurity = _dbContext.Set<Security>().Include(x => x.SecurityRights).Where(x => x.Active);
            var query = string.IsNullOrEmpty(requestInfo.SearchText) ? filteredSecurity : filteredSecurity.Where(s
                                                                  => s.SecurityDescription.ToUpper().Contains(requestInfo.SearchText.ToUpper()));

            //TODO: Make this LINQ query precompiled, using the method 

            var totalItemCount = query.Count();
            var startIndex = ResultsPagingUtility.CalculateStartIndex(requestInfo.PageNumber, requestInfo.PageSize);
            var mapper = new SecurityToSecurityViewModelMapper();
            var docs =
                query.OrderBy(x => x.DateCreated)
                    .Skip(startIndex)
                    .Take(requestInfo.PageSize)
                    .Select(
                        s => mapper.Map(s)).ToList();

            var queryResult = new QueryResult<SecurityViewModel>(docs, totalItemCount, requestInfo.PageSize);
            var inquiryResponse = new PagedTaskDataInquiryResponseForSearch
            {
                Items = docs,
                TotalRecords = totalItemCount,
                PageCount = queryResult.TotalPageCount,
                PageNumber = requestInfo.PageNumber,
                PageSize = requestInfo.PageSize
            };
            return inquiryResponse;
        }

        public PagedTaskDataInquiryResponseForSearch SearchAll(PagedDataRequest requestInfo, Expression<Func<Security, bool>> @where = null)
        {
            var filteredSecurity = _dbContext.Set<Security>().Include(s => s.SecurityRights);
            var query = String.IsNullOrEmpty(requestInfo.SearchText) ? filteredSecurity : filteredSecurity.Where(s
                                                                  => s.SecurityDescription.ToUpper().Contains(requestInfo.SearchText.ToUpper()));

            //TODO: Make this LINQ query precompiled, using the method 

            var totalItemCount = query.Count();
            var startIndex = ResultsPagingUtility.CalculateStartIndex(requestInfo.PageNumber, requestInfo.PageSize);
            var mapper = new SecurityToSecurityViewModelMapper();
            var docs =
                query.OrderBy(x => x.DateCreated)
                    .Skip(startIndex)
                    .Take(requestInfo.PageSize)
                    .Select(
                        s => mapper.Map(s)).ToList();

            var queryResult = new QueryResult<SecurityViewModel>(docs, totalItemCount, requestInfo.PageSize);
            var inquiryResponse = new PagedTaskDataInquiryResponseForSearch
            {
                Items = docs,
                TotalRecords = totalItemCount,
                PageCount = queryResult.TotalPageCount,
                PageNumber = requestInfo.PageNumber,
                PageSize = requestInfo.PageSize
            };
            return inquiryResponse;
        }

    }
}

