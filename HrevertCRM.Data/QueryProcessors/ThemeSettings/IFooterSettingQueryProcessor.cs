using HrevertCRM.Data.ViewModels.ThemeSettings;
using HrevertCRM.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface IFooterSettingQueryProcessor
    {
        FooterSetting Update(FooterSetting footerSetting);
        FooterSetting GetFooterSetting(int footerSettingId);
        FooterSetting Get();
        void SaveAll(List<FooterSetting> footerSettings);
        bool Delete(int footerSettingId);
        FooterSetting ActivateFooterSetting(int footerSettingId);
        FooterSetting Save(FooterSetting footerSetting);
        bool Exists(Expression<Func<FooterSetting, bool>> @where);
        List<string> GetBrandImageUrls(int footerSettingId);
        string GetFooterImagePath(int footerSettingId);
        bool DeleteImage(FooterSettingViewModel footerSettingViewModel, string imageUrl);
        bool DeleteBrandImage(int footerSettingId, string imageUrl);
        bool RemoveFooterLogo(int footerSettingId);
    }
}
