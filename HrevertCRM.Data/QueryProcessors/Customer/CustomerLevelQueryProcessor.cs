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
using PagedTaskDataInquiryResponse = HrevertCRM.Data.Common.PagedDataInquiryResponse<HrevertCRM.Data.ViewModels.CustomerLevelViewModel>;

namespace HrevertCRM.Data.QueryProcessors
{
    public class CustomerLevelQueryProcessor : QueryBase<CustomerLevel>, ICustomerLevelQueryProcessor
    {
        public CustomerLevelQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
        {
        }
        public CustomerLevel Update(CustomerLevel customerLevel)
        {
            var original = GetValidCustomerLevel(customerLevel.Id);
            ValidateAuthorization(customerLevel);
            CheckVersionMismatch(customerLevel, original);   //TODO: to test the this method this is commented

            original.Name = customerLevel.Name;

            _dbContext.Set<CustomerLevel>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        public virtual CustomerLevel GetValidCustomerLevel(int customerLevelId)
        {
            var customerLevel = _dbContext.Set<CustomerLevel>().FirstOrDefault(sc => sc.Id == customerLevelId);
            if (customerLevel == null)
            {
                throw new RootObjectNotFoundException(CustomerConstants.CustomerLevelQueryProcessorConstants.CustomerLevelNotFound);
            }
            return customerLevel;
        }

        public CustomerLevel GetCustomerLevel(int customerLevelId)
        {
            var customerLevel = _dbContext.Set<CustomerLevel>().FirstOrDefault(d => d.Id == customerLevelId);
            return customerLevel;
        }

        public void SaveAllCustomerLevel(List<CustomerLevel> customerLevels)
        {
            _dbContext.Set<CustomerLevel>().AddRange(customerLevels);
            _dbContext.SaveChanges();
        }

        public CustomerLevel Save(CustomerLevel customerLevel)
        {
            _dbContext.Set<CustomerLevel>().Add(customerLevel);
            _dbContext.SaveChanges();
            return customerLevel;
        }

        public int SaveAll(List<CustomerLevel> customerLevels)
        {
            _dbContext.Set<CustomerLevel>().AddRange(customerLevels);
            return _dbContext.SaveChanges();

        }

        public bool Delete(int customerLevelId)
        {
            var doc = GetCustomerLevel(customerLevelId);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<CustomerLevel>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }

        public bool Exists(Expression<Func<CustomerLevel, bool>> @where)
        {
            return _dbContext.Set<CustomerLevel>().Any(@where);
        }

        public PagedTaskDataInquiryResponse GetCustomerLevels(PagedDataRequest requestInfo, Expression<Func<CustomerLevel, bool>> @where = null)
        {
            var query = _dbContext.Set<CustomerLevel>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            //var enumerable = query as CustomerLevel[] ?? query.ToArray();
            var totalItemCount = query.Count();
            var startIndex = ResultsPagingUtility.CalculateStartIndex(requestInfo.PageNumber, requestInfo.PageSize);
            var mapper = new CustomerLevelToCustomerLevelViewModelMapper();
            var docs = query.OrderBy(x => x.DateCreated).Skip(startIndex).Take(requestInfo.PageSize).Select(s => mapper.Map(s)).ToList();
            var queryResult = new QueryResult<CustomerLevelViewModel>(docs, totalItemCount, requestInfo.PageSize);

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

        public CustomerLevel[] GetCustomerLevels(Expression<Func<CustomerLevel, bool>> @where = null)
        {
            var query = _dbContext.Set<CustomerLevel>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            var enumerable = query.ToArray();
            return enumerable;
        }

        public IQueryable<CustomerLevel> GetActiveCustomerLevels()
        {
            return _dbContext.Set<CustomerLevel>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active);
        }
        public IQueryable<CustomerLevel> GetDeletedCustomerLevels()
        {
            return _dbContext.Set<CustomerLevel>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active == false);
        }


    }
}
