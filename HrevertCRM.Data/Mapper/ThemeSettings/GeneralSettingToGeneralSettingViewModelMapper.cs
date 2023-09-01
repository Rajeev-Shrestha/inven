using HrevertCRM.Data.ViewModels.ThemeSettings;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper.ThemeSettings
{
    public class GeneralSettingToGeneralSettingViewModelMapper : MapperBase<GeneralSetting, GeneralSettingViewModel>
    {
        public override GeneralSetting Map(GeneralSettingViewModel viewModel)
        {
            return new GeneralSetting
            {
                Id = viewModel.Id ?? 0,
                SelectedTheme = viewModel.SelectedTheme,
                LogoUrl = viewModel.LogoUrl,
                StoreName = viewModel.StoreName,
                FaviconLogoUrl = viewModel.FaviconLogoUrl,

                EnableSlides = viewModel.EnableSlides,
                EnableTopCategories = viewModel.EnableTopCategories,
                EnableTopTrendingProducts = viewModel.EnableTopTrendingProducts,
                EnableHotThisWeek = viewModel.EnableHotThisWeek,
                EnableLatestProducts = viewModel.EnableLatestProducts,
                EnableGetInspired = viewModel.EnableGetInspired,

                CompanyId = viewModel.CompanyId,
                Active = viewModel.Active,
                Version = viewModel.Version
            };
        }

        public override GeneralSettingViewModel Map(GeneralSetting entity)
        {
            if (entity == null) return null;
            return new GeneralSettingViewModel
            {
                Id = entity.Id,
                SelectedTheme = entity.SelectedTheme,
                LogoUrl = entity.LogoUrl,
                StoreName = entity.StoreName,
                FaviconLogoUrl = entity.FaviconLogoUrl,

                EnableSlides = entity.EnableSlides,
                EnableTopCategories = entity.EnableTopCategories,
                EnableTopTrendingProducts = entity.EnableTopTrendingProducts,
                EnableHotThisWeek = entity.EnableHotThisWeek,
                EnableLatestProducts = entity.EnableLatestProducts,
                EnableGetInspired = entity.EnableGetInspired,

                CompanyId = entity.CompanyId,
                Active = entity.Active,
                Version = entity.Version
            };
        }
    }
}
