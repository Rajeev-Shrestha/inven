using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Hrevert.Common.Constants.Customer;
using Hrevert.Common.Security;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.Mapper;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore;
using PagedTaskDataInquiryResponse = HrevertCRM.Data.Common.PagedDataInquiryResponse<HrevertCRM.Data.ViewModels.CustomerContactGroupViewModel>;


namespace HrevertCRM.Data.QueryProcessors
{
    public class CustomerContactGroupQueryProcessor : QueryBase<CustomerContactGroup>, ICustomerContactGroupQueryProcessor
    {
        public CustomerContactGroupQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
        {
        }
        public CustomerContactGroup Update(CustomerContactGroup customerContactGroup)
        {
            var original = GetValidCustomerContactGroup(customerContactGroup.Id);
            ValidateAuthorization(customerContactGroup);
            CheckVersionMismatch(customerContactGroup, original);  //TODO: to test this method this commented out.

            original.GroupName = customerContactGroup.GroupName;
            original.Description = customerContactGroup.Description;

            _dbContext.Set<CustomerContactGroup>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        //public IQueryable<CustomerContactGroup> GetAllCustomerContactGroups()
        //{
        //    return _dbContext.Set<CustomerContactGroup>().Include(x => x.CustomerAndContactGroupList).Where(f => f.CompanyId == LoggedInUser.CompanyId);
        //}

        //public IQueryable<CustomerContactGroup> GetActiveCustomerContactGroups()
        //{
        //    var value =
        //        _dbContext.Set<CustomerContactGroup>()
        //            .Include(x => x.CustomerAndContactGroupList)
        //            .Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active);
        //    return value;
        //}

        //public IQueryable<CustomerContactGroup> GetDeletedCustomerContactGroups()
        //{
        //    return _dbContext.Set<CustomerContactGroup>().Include(x => x.CustomerAndContactGroupList).Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active == false);
        //}

        public virtual CustomerContactGroup GetValidCustomerContactGroup(int customerContactGroupId)
        {
            var customerContactGroup = _dbContext.Set<CustomerContactGroup>().FirstOrDefault(sc => sc.Id == customerContactGroupId);
            if (customerContactGroup == null)
            {
                throw new RootObjectNotFoundException(CustomerConstants.CustomerContactGroupQueryProcessorConstants.ContactGroupNotFound);
            }
            return customerContactGroup;
        }

        public CustomerContactGroup GetCustomerContactGroup(int customerContactGroupId)
        {
            var customerContactGroup = _dbContext.Set<CustomerContactGroup>().Include(x => x.CustomerAndContactGroupList).FirstOrDefault(d => d.Id == customerContactGroupId);
            customerContactGroup.CustomerIdsInContactGroup =
                customerContactGroup.CustomerAndContactGroupList.Select(x => x.CustomerId).ToList();
            return customerContactGroup;
        }

        public void SaveAllCustomerContactGroup(List<CustomerContactGroup> customerContactGroups)
        {
            _dbContext.Set<CustomerContactGroup>().AddRange(customerContactGroups);
            _dbContext.SaveChanges();
        }

        public CustomerContactGroup Save(CustomerContactGroup customerContactGroup)
        {
            customerContactGroup.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<CustomerContactGroup>().Add(customerContactGroup);
            _dbContext.SaveChanges();
            return customerContactGroup;
        }

        public int SaveAll(List<CustomerContactGroup> customerContactGroups)
        {
            _dbContext.Set<CustomerContactGroup>().AddRange(customerContactGroups);
            return _dbContext.SaveChanges();

        }

        public bool Delete(int customerContactGroupId)
        {
            var doc = GetCustomerContactGroup(customerContactGroupId);
            int result = 0;
            if (doc != null)
            {
                doc.Active = false;
                _dbContext.Set<CustomerContactGroup>().Update(doc);
                result = _dbContext.SaveChanges();
            }
            return result > 0;
        }

        public bool Exists(Expression<Func<CustomerContactGroup, bool>> @where)
        {
            return _dbContext.Set<CustomerContactGroup>().Where(p=>p.CompanyId==LoggedInUser.CompanyId).Any(@where);
        }

        public QueryResult<CustomerContactGroup> GetCustomerContactGroups(PagedDataRequest requestInfo, Expression<Func<CustomerContactGroup, bool>> @where = null)
        {
            var query = _dbContext.Set<CustomerContactGroup>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            //var enumerable = query as CustomerContactGroup[] ?? query.ToArray();
            var totalItemCount = query.Count();
            var startIndex = ResultsPagingUtility.CalculateStartIndex(requestInfo.PageNumber, requestInfo.PageSize);
            var docs = query.OrderBy(x => x.DateCreated).Skip(startIndex).Take(requestInfo.PageSize).ToList();
            var queryResult = new QueryResult<CustomerContactGroup>(docs, totalItemCount, requestInfo.PageSize);
            return queryResult;
        }

        public CustomerContactGroup[] GetCustomerContactGroups(Expression<Func<CustomerContactGroup, bool>> @where = null)
        {
            var query = _dbContext.Set<CustomerContactGroup>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            var enumerable = query.ToArray();
            return enumerable;
        }


        public List<CustomerContactGroup> SearchActive(string searchText)
        {
            var contactGroups = _dbContext.Set<CustomerContactGroup>().Include(x => x.CustomerAndContactGroupList).Where(FilterByActiveTrueAndCompany);

            if (string.IsNullOrEmpty(searchText)) return contactGroups.ToList();
            var result = new List<CustomerContactGroup>();
            if (!string.IsNullOrEmpty(searchText))
            {
                result = contactGroups.Where(s => s.GroupName.ToUpper().Contains(searchText.ToUpper())).ToList();
            }
            return result;
        }

        public List<CustomerContactGroup> SearchAll(string searchText)
        {
            var contactGroups = _dbContext.Set<CustomerContactGroup>().Include(x => x.CustomerAndContactGroupList).Where(x => x.CompanyId == LoggedInUser.CompanyId);
            if (string.IsNullOrEmpty(searchText)) return contactGroups.ToList();
            var result = new List<CustomerContactGroup>();
            if (!string.IsNullOrEmpty(searchText))
            {
                result = contactGroups.Where(s => s.GroupName.ToUpper().Contains(searchText.ToUpper())).ToList();

            }
            return result;
        }

       public CustomerContactGroup ActivateCustomerContactGroup(int id)
        {
            var original = GetValidCustomerContactGroup(id);
            ValidateAuthorization(original);

            original.Active = true;
            _dbContext.Set<CustomerContactGroup>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        public PagedTaskDataInquiryResponse GetActiveCustomerContactGroups(PagedDataRequest requestInfo, Expression<Func<CustomerContactGroup, bool>> where = null)
        {
            var query = _dbContext.Set<CustomerContactGroup>().Include(x => x.CustomerAndContactGroupList).Where(FilterByActiveTrueAndCompany);
            return FormatResultForPaging(requestInfo, query);
        }

        private static PagedTaskDataInquiryResponse FormatResultForPaging(PagedDataRequest requestInfo, IQueryable<CustomerContactGroup> query)
        {
            var totalItemCount = query.Count();

            var startIndex = ResultsPagingUtility.CalculateStartIndex(requestInfo.PageNumber, requestInfo.PageSize);
            var mapper = new CustomerContactGroupToCustomerContactGroupViewModelMapper();
            var docs = query.OrderBy(x => x.DateCreated).Skip(startIndex).Take(requestInfo.PageSize).Select(
                s => mapper.Map(s)).ToList();

            var queryResult = new QueryResult<CustomerContactGroupViewModel>(docs, totalItemCount, requestInfo.PageSize);
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

        public PagedTaskDataInquiryResponse GetDeletedCustomerContactGroups(PagedDataRequest requestInfo, Expression<Func<CustomerContactGroup, bool>> where = null)
        {
            var query = _dbContext.Set<CustomerContactGroup>().Include(x => x.CustomerAndContactGroupList).Where(FilterByActiveFalseAndCompany);
            return FormatResultForPaging(requestInfo, query);
        }

        public PagedTaskDataInquiryResponse GetAllCustomerContactGroups(PagedDataRequest requestInfo, Expression<Func<CustomerContactGroup, bool>> where = null)
        {
            var query = _dbContext.Set<CustomerContactGroup>().Include(x => x.CustomerAndContactGroupList).Where(c => c.CompanyId == LoggedInUser.CompanyId);
            return FormatResultForPaging(requestInfo, query);
        }

        public CustomerContactGroup CheckIfDeletedCustomerContactWithSameNameExists(string name)
        {
            var contactGroup =
                _dbContext.Set<CustomerContactGroup>()
                    .FirstOrDefault(
                        x =>
                            x.GroupName == name && x.CompanyId == LoggedInUser.CompanyId && (x.Active ||
                            x.Active == false));
            return contactGroup;

        }

        public PagedTaskDataInquiryResponse GetContactGroups(PagedDataRequest requestInfo, Expression<Func<CustomerContactGroup, bool>> @where = null)
        {
            var query = _dbContext.Set<CustomerContactGroup>().Include(x => x.CustomerAndContactGroupList).Where(x=>x.CompanyId==LoggedInUser.CompanyId);
            if (requestInfo.Active)
                query = query.Where(req => req.Active);
            var result = string.IsNullOrEmpty(requestInfo.SearchText)
                ? query
                : query.Where(x => x.GroupName.ToUpper().Contains(requestInfo.SearchText.ToUpper()));
            return FormatResultForPaging(requestInfo, result);
        }
    }
}
