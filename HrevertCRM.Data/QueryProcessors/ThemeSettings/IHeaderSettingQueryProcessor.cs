using HrevertCRM.Data.ViewModels.ThemeSettings;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface IHeaderSettingQueryProcessor
    {
        HeaderSetting Update(HeaderSetting headerSetting);
        HeaderSetting Get(int headerSettingId);
        HeaderSetting Save(HeaderSetting headerSetting);
        HeaderSetting GetHeaderSetting();
        bool DeleteStoreLocator(int id);
    }
}
