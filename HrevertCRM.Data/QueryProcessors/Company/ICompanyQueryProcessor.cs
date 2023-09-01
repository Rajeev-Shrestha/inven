using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface ICompanyQueryProcessor
    {
        Company Update(Company company);
        Company GetCompany(int companyId);
        void SaveAllCompany(List<Company> companies );
        bool Delete(int companyId);
        bool Exists(Expression<Func<Company, bool>> @where);
        PagedDataInquiryResponse<CompanyViewModel> GetCompanies(PagedDataRequest requestInfo, Expression<Func<Company, bool>> @where = null);
        Company[] GetCompanies(Expression<Func<Company, bool>> @where = null);
        IQueryable<Company> GetActiveCompanies();
        IQueryable<Company> GetDeletedCompanies();
        IQueryable<Company> GetAllCompanies();
        Company Save(Company company);
        int SaveAll(List<Company> companies);
        bool CheckIfCompanyIsInitialized(string userId);
        byte[] GetVersion(string userVmId);
        CompanyViewModel GetCompanyByUserId(string userId);
        CompanyViewModel GetCompanyByLoggedInUserId();
        IQueryable<CompanyViewModel> SearchActive(string searchText);
    }
}
