using Hrevert.Common.Enums;
using HrevertCRM.Data.ViewModels.ThemeSettings;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface ILayoutSettingQueryProcessor
    {
        LayoutSetting Update(LayoutSetting layoutSetting);
        LayoutSetting Get(int getLayoutSettingId);
        LayoutSetting Save(LayoutSetting layoutSetting);
        string GetImageUrl(int layoutSettingId, ThemeSettingImageType img);
        bool DeleteImage(LayoutSettingViewModel layoutSettingViewModel, string imageUrl, ThemeSettingImageType img);
        PersonnelSetting SavePersonnelSetting(PersonnelSetting personnelSetting);
        LayoutSetting GetLayoutSetting();
        bool RemoveLayoutImage(string imageUrl, int layoutId);
    }
}
