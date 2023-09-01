using System.Linq;
using HrevertCRM.Data.ViewModels.ThemeSettings;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper.ThemeSettings
{
    public class LayoutSettingToLayoutSettingViewModelMapper : MapperBase<LayoutSetting, LayoutSettingViewModel>
    {
        public override LayoutSetting Map(LayoutSettingViewModel viewModel)
        {
            if (viewModel == null) return null;
            var layoutSetting = new LayoutSetting
            {
                Id = viewModel.Id ?? 0,
                ShowLayoutTitle = viewModel.ShowLayoutTitle,

                CategoryOne = viewModel.CategoryOne ?? 0,
                CategoryTwo = viewModel.CategoryTwo ?? 0,
                CategoryThree = viewModel.CategoryThree ?? 0,
                CategoryFour = viewModel.CategoryFour ?? 0,

                ShowTrendingItemsLayoutTitle = viewModel.ShowTrendingItemsLayoutTitle,
                TrendingItemsTitleStyle = viewModel.TrendingItemsTitleStyle,
                TrendingItemsImageUrl = viewModel.TrendingItemsImageUrl,
                TrendingItemsColor = viewModel.TrendingItemsColor,

                ShowHotThisWeekLayoutTitle = viewModel.ShowHotThisWeekLayoutTitle,
                HotThisWeekTitleStyle= viewModel.HotThisWeekTitleStyle,
                HotThisWeekImageUrl = viewModel.HotThisWeekImageUrl,
                HotThisWeekColor = viewModel.HotThisWeekColor,

                ShowLatestProductsLayoutTitle = viewModel.ShowLatestProductsLayoutTitle,
                LatestProductsTitleStyle = viewModel.LatestProductsTitleStyle,
                LatestProductsImageUrl = viewModel.LatestProductsImageUrl,
                LatestProductsColor = viewModel.LatestProductsColor,

                BackgroundImageOrColor = viewModel.BackgroundImageOrColor,
                BackgroundImageUrl = viewModel.BackgroundImageUrl,
                ColorCode = viewModel.ColorCode,

                EnableSeparator = viewModel.EnableSeparator,
                HotThisWeekSeparatorUrl = viewModel.HotThisWeekSeparatorUrl,
                TrendingItemsSeparatorUrl = viewModel.TrendingItemsSeparatorUrl,
                LatestProductsSeparatorUrl = viewModel.LatestProductsSeparatorUrl,

                CompanyId = viewModel.CompanyId,
                Active = viewModel.Active,
                Version = viewModel.Version
            };
            if (viewModel.PersonnelSettingViewModels == null || viewModel.PersonnelSettingViewModels.Count <= 0)
                return layoutSetting;
            var personnelSettingMapper = new PersonnelSettingToPersonnelSettingViewModelMapper();
            layoutSetting.PersonnelSettings = personnelSettingMapper.Map(viewModel.PersonnelSettingViewModels);
            return layoutSetting;
        }

        public override LayoutSettingViewModel Map(LayoutSetting entity)
        {
            if (entity == null) return null;
            var layoutSettingVm = new LayoutSettingViewModel
            {
                Id = entity.Id,
                ShowLayoutTitle = entity.ShowLayoutTitle,

                CategoryOne = entity.CategoryOne,
                CategoryTwo = entity.CategoryTwo,
                CategoryThree = entity.CategoryThree,
                CategoryFour = entity.CategoryFour,

                ShowTrendingItemsLayoutTitle = entity.ShowTrendingItemsLayoutTitle,
                TrendingItemsTitleStyle = entity.TrendingItemsTitleStyle,
                TrendingItemsImageUrl = entity.TrendingItemsImageUrl,
                TrendingItemsColor = entity.TrendingItemsColor,

                ShowHotThisWeekLayoutTitle = entity.ShowHotThisWeekLayoutTitle,
                HotThisWeekTitleStyle = entity.HotThisWeekTitleStyle,
                HotThisWeekImageUrl = entity.HotThisWeekImageUrl,
                HotThisWeekColor = entity.HotThisWeekColor,

                ShowLatestProductsLayoutTitle = entity.ShowLatestProductsLayoutTitle,
                LatestProductsTitleStyle = entity.LatestProductsTitleStyle,
                LatestProductsImageUrl = entity.LatestProductsImageUrl,
                LatestProductsColor = entity.LatestProductsColor,

                BackgroundImageOrColor = entity.BackgroundImageOrColor,
                BackgroundImageUrl = entity.BackgroundImageUrl,
                ColorCode = entity.ColorCode,

                EnableSeparator = entity.EnableSeparator,
                HotThisWeekSeparatorUrl = entity.HotThisWeekSeparatorUrl,
                TrendingItemsSeparatorUrl = entity.TrendingItemsSeparatorUrl,
                LatestProductsSeparatorUrl = entity.LatestProductsSeparatorUrl,

                CompanyId = entity.CompanyId,
                Active = entity.Active,
                Version = entity.Version
            };
            if (entity.PersonnelSettings == null || entity.PersonnelSettings.Count <= 0) return layoutSettingVm;
            var personnelSettingMapper = new PersonnelSettingToPersonnelSettingViewModelMapper();
            layoutSettingVm.PersonnelSettingViewModels = entity.PersonnelSettings.Select(x => personnelSettingMapper.Map(x)).ToList();
            return layoutSettingVm;
        }
    }
}
