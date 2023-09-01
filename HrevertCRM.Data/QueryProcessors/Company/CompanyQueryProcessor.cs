using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Hrevert.Common.Constants.Company;
using Hrevert.Common.Security;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.Mapper;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using PagedTaskDataInquiryResponse = HrevertCRM.Data.Common.PagedDataInquiryResponse<HrevertCRM.Data.ViewModels.CompanyViewModel>;

namespace HrevertCRM.Data.QueryProcessors
{
    public class CompanyQueryProcessor : QueryBase<Company>, ICompanyQueryProcessor
    {
        public CompanyQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
        {
        }

        public Company Update(Company company)
        {
            var original = GetValidCompany(company.Id);
            ValidateAuthorization(company);
            CheckVersionMismatch(company, original); //TODO: to test this method comment this out.
            
            company.CompanyId = company.Id;
            company.MasterId = company.MasterId;
            //original.Name = company.Name; // Buy garne bela ma nai company name aayeko hunchha, which can't be changed
            original.GpoBoxNumber = company.GpoBoxNumber;
            original.Address = company.Address;
            original.PhoneNumber = company.PhoneNumber;
            original.FaxNo = company.FaxNo;
            original.Email = company.Email;
            original.WebsiteUrl = company.WebsiteUrl;
            original.VatRegistrationNo = company.VatRegistrationNo;
            original.PanRegistrationNo = company.PanRegistrationNo;
            original.FiscalYearFormat = company.FiscalYearFormat;
            original.IsCompanyInitialized = company.IsCompanyInitialized;

            original.Active = company.Active;
            original.WebActive = company.WebActive;

            _dbContext.Set<Company>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        public virtual Company GetValidCompany(int companyId)
        {
            var company = _dbContext.Set<Company>().FirstOrDefault(sc => sc.Id == companyId);
            if (company == null)
            {
                throw new RootObjectNotFoundException(CompanyConstants.CompanyQueryProcessorConstants.CompanyNotFound);
            }
            return company;
        }

        public Company GetCompany(int companyId)
        {
            var company = _dbContext.Set<Company>().FirstOrDefault(d => d.Id == companyId);
            return company;
        }

        public void SaveAllCompany(List<Company> companies)
        {
            _dbContext.Set<Company>().AddRange(companies);
            _dbContext.SaveChanges();
        }

        public Company Save(Company company)
        {
            _dbContext.Set<Company>().Add(company);
            _dbContext.SaveChanges();
            return company;
        }

        public int SaveAll(List<Company> companies)
        {
            _dbContext.Set<Company>().AddRange(companies);
            return _dbContext.SaveChanges();
        }

        public bool CheckIfCompanyIsInitialized(string userId)
        {
            var companyId = _dbContext.Set<ApplicationUser>().FirstOrDefault(x => x.Id == userId && x.Active).CompanyId;
            if (companyId == 0) return false;
            var isInitialized =
                _dbContext.Set<Company>().FirstOrDefault(x => x.Id == companyId && x.Active).IsCompanyInitialized;
            return isInitialized;
        }

        public byte[] GetVersion(string userId)
        {
            var companyId = _dbContext.Set<ApplicationUser>().FirstOrDefault(x => x.Id == userId && x.Active).CompanyId;
            var companyVersion =
                _dbContext.Set<Company>().FirstOrDefault(x => x.Id == companyId && x.Active).Version;
            return companyVersion;
        }

        public CompanyViewModel GetCompanyByUserId(string userId)
        {
            var companyId = _dbContext.Set<ApplicationUser>().FirstOrDefault(x => x.Id == userId && x.Active).CompanyId;
            if (companyId == 0) return null;
            var company = _dbContext.Set<Company>().FirstOrDefault(x => x.Id == companyId && x.Active);
            var companyMapper = new CompanyToCompanyViewModelMapper();
            return company == null ? null : companyMapper.Map(company);
        }

        public CompanyViewModel GetCompanyByLoggedInUserId()
        {
            var company = _dbContext.Set<Company>().FirstOrDefault(x => x.Id == LoggedInUser.CompanyId && x.Active);
            var companyMapper = new CompanyToCompanyViewModelMapper();
            return company == null ? null : companyMapper.Map(company);
        }

        public IQueryable<CompanyViewModel> SearchActive(string searchText)
        {
            var filteredCompany = _dbContext.Set<Company>().Where(x => x.Id != LoggedInUser.CompanyId && x.Active);
            if (filteredCompany == null) return null;
            var query = string.IsNullOrEmpty(searchText) ? filteredCompany : filteredCompany.Where(s =>
                                                                   s.Name.ToUpper().Contains(searchText.ToUpper()));
            var mapper = new CompanyToCompanyViewModelMapper();
            var docs = query.OrderBy(x => x.DateCreated).Select(s => mapper.Map(s));
            return docs;
        }

        public bool Delete(int companyId)
        {
            var doc = GetCompany(companyId);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<Company>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }

        public bool Exists(Expression<Func<Company, bool>> @where)
        {
            return _dbContext.Set<Company>().Any(@where);
        }

        public PagedTaskDataInquiryResponse GetCompanies(PagedDataRequest requestInfo, Expression<Func<Company, bool>> @where = null)
        {
            var query = _dbContext.Set<Company>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            var totalItemCount = query.Count();
            var startIndex = ResultsPagingUtility.CalculateStartIndex(requestInfo.PageNumber, requestInfo.PageSize);
            var mapper = new CompanyToCompanyViewModelMapper();
            var docs = query.OrderBy(x => x.DateCreated).Skip(startIndex).Take(requestInfo.PageSize).
                Select(s => mapper.Map(s)).ToList();

            var queryResult = new QueryResult<CompanyViewModel>(docs, totalItemCount, requestInfo.PageSize);
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

        public Company[] GetCompanies(Expression<Func<Company, bool>> @where = null)
        {
            var query = _dbContext.Set<Company>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            var enumerable = query.ToArray();
            return enumerable;
        }

        public IQueryable<Company> GetActiveCompanies()
        {
            return _dbContext.Set<Company>().Where(f => f.Active && f.Id != LoggedInUser.CompanyId);
        }
        public IQueryable<Company> GetDeletedCompanies()
        {
            return _dbContext.Set<Company>().Where(f => f.Active == false && f.Id != LoggedInUser.CompanyId);
        }
        public IQueryable<Company> GetAllCompanies()
        {
            return _dbContext.Set<Company>().Where(f => f.Id != LoggedInUser.CompanyId);
        }
    }
}
