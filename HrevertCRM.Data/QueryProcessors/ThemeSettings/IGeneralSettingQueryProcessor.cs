using HrevertCRM.Data.ViewModels.ThemeSettings;
using HrevertCRM.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface IGeneralSettingQueryProcessor
    {
        GeneralSetting Update(GeneralSetting generalSetting);
        GeneralSetting GetGeneralSetting(int generalSettingId);
        GeneralSetting Get();
        void SaveAll(List<GeneralSetting> generalSettings);
        bool Delete(int generalSettingId);
        GeneralSetting ActivateBrand(int generalSettingId);
        GeneralSetting Save(GeneralSetting generalSetting);
        string SaveThemeSettingImage(ThemeImage image);
        bool Exists(Expression<Func<GeneralSetting, bool>> @where);
        string GetLogoImageUrl(int id);
        string GetGetFaviconImageUrl(int id);
        List<object> GetThemeColor();
        //bool DeleteImage(GeneralSettingViewModel generalSettingViewModel, string imageUrl, ThemeImage img);
        bool RemoveLogo(int id);
        bool RemoveFavicon(int id);

    }
}
