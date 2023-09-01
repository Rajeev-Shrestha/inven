using Hrevert.Common.Enums;
using HrevertCRM.Data.ViewModels.ThemeSettings;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface IBrandImageQueryProcessor
    {
        BrandImage Save(BrandImage brandImage);
        BrandImage Update(BrandImage brandImage);
        BrandImage GetBrandImage(int id);
        void DeleteBrandImage(int footerSettingId, string imageUrl);
    }
}
