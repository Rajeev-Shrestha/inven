using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using HrevertCRM.Entities;
using System.Linq;
using HrevertCRM.Entities.Enumerations;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface ICompanyWebSettingQueryProcessor
    {
        int SaveAll(List<CompanyWebSetting> companies);
        int GetSerialNo(int companyId);
        CompanyWebSetting Get(int companyId);

        CompanyWebSetting GetCompanyWebSetting();
        CompanyWebSetting Update(CompanyWebSetting companyWebSetting);
        CompanyWebSetting Save(CompanyWebSetting companyWebSetting);
        bool Exists(Expression<Func<CompanyWebSetting, bool>> @where);

        IQueryable<ShippingCalculationTypes> GetShippingDiscountCalculationTypes();

        IQueryable<DiscountCalculationTypes> GetDiscountCalculationTypes();
        bool Delete(int id);
        bool CheckIfEstoreIsInitialized(string userId);
        CompanyWebSetting SaveCompanyWebSetting(CompanyWebSetting newCompanyWebSetting);
        CompanyWebSetting ActivateCompanyWebSetting(int id);
        CompanyWebSetting CheckIfDeletedCompanyWebSettingExists();
    }
}