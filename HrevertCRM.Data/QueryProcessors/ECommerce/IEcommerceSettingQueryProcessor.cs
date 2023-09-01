using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface IEcommerceSettingQueryProcessor
    {
        EcommerceSetting Update(EcommerceSetting ecommerceSetting);
        EcommerceSetting GetEcommerceSetting(int ecommerceSettingId);
        EcommerceSettingViewModel GetEcommerceSettingViewModel(int ecommerceSettingId);
        void SaveAllEcommerceSetting(List<EcommerceSetting> ecommerceSettings);
        bool Delete(int ecommerceSettingId);
        bool Exists(Expression<Func<EcommerceSetting, bool>> where);
        EcommerceSetting GetActiveEcommerceSettings();
        EcommerceSetting ActivateEcommerceSetting(int id);
        EcommerceSetting[] GetEcommerceSettings(Expression<Func<EcommerceSetting, bool>> where = null);
        int GetCompanyIdByEcommerceSettingId(int ecommerceSettingId);
        EcommerceSetting Save(EcommerceSetting ecommerceSetting);
        bool IsExist();
        string SaveEcommerceLogo(Image image);
        bool DeleteLogo(string imageUri);
    }
}