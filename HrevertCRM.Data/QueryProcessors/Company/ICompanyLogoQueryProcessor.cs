using HrevertCRM.Data.Common;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface ICompanyLogoQueryProcessor
    {
        CompanyLogo Update(CompanyLogo companyLogo);
        CompanyLogo GetCompanyLogo(int companyLogoId);
        CompanyLogoViewModel GetCompanyLogoViewModel(int companyLogoId);
        void SaveAllCompanyLogos(List<CompanyLogo> companyLogos);
        bool Delete(int companyLogoId);
        bool Exists(Expression<Func<CompanyLogo, bool>> where);
        CompanyLogo GetActiveCompanyLogos();
        CompanyLogo GetDeletedCompanyLogos();
        CompanyLogo GetAllCompanyLogos();
        PagedDataInquiryResponse<CompanyLogoViewModel> SearchActive(PagedDataRequest requestInfo, Expression<Func<CompanyLogo, bool>> @where = null);
        PagedDataInquiryResponse<CompanyLogoViewModel> SearchAll(PagedDataRequest requestInfo, Expression<Func<CompanyLogo, bool>> @where = null);
        CompanyLogo ActivateCompanyLogo(int id);
        CompanyLogo[] GetCompanyLogos(Expression<Func<CompanyLogo, bool>> where = null);
        int GetCompanyIdByCompnayLogoId(int carouselId);
        CompanyLogo Save(CompanyLogo companyLogo);
        string SaveImage(int savedCompanylogoId, Image image);
        string GetActiveCompanyLogo();
    }
}
