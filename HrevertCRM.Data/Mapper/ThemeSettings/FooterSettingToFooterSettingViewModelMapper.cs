using System.Linq;
using HrevertCRM.Data.ViewModels.ThemeSettings;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper.ThemeSettings
{
    public class FooterSettingToFooterSettingViewModelMapper : MapperBase<FooterSetting, FooterSettingViewModel>
    {
        public override FooterSetting Map(FooterSettingViewModel viewModel)
        {
            return new FooterSetting
            {
                Id = viewModel.Id ?? 0,
                EnableFooterMenu = viewModel.EnableFooterMenu,
                EnableBrands = viewModel.EnableBrands,

                ShowFooterLogo = viewModel.ShowFooterLogo,
                FooterLogoUrl = viewModel.FooterLogoUrl,
                AboutStore = viewModel.AboutStore,

                EnableAbout = viewModel.EnableAbout,
                EnableContact = viewModel.EnableContact,
                EnableStoreLocator = viewModel.EnableStoreLocator,
                EnablePolicies = viewModel.EnablePolicies,
                EnableFaq = viewModel.EnableFaq,

                ShowUserLoginLink = viewModel.ShowUserLoginLink,
                ShowOrderHistoryLink = viewModel.ShowOrderHistoryLink,
                ShowWishlistLink = viewModel.ShowWishlistLink,

                ShowTrendingOrLastest = viewModel.ShowTrendingOrLastest,

                WhereToFindUs = viewModel.WhereToFindUs,
                EnableNewsLetter = viewModel.EnableNewsLetter,

                EnableCopyright = viewModel.EnableCopyright,
                CopyrightText = viewModel.CopyrightText,
                CompanyId = viewModel.CompanyId,
                Active = viewModel.Active,
                Version = viewModel.Version
            };
        }

        public override FooterSettingViewModel Map(FooterSetting entity)
        {
            if (entity == null) return null;
            var footerSettingVm = new FooterSettingViewModel
            {
                Id = entity.Id,
                EnableFooterMenu = entity.EnableFooterMenu,
                EnableBrands = entity.EnableBrands,

                ShowFooterLogo = entity.ShowFooterLogo,
                FooterLogoUrl = entity.FooterLogoUrl,
                AboutStore = entity.AboutStore,

                EnableAbout = entity.EnableAbout,
                EnableContact = entity.EnableContact,
                EnableStoreLocator = entity.EnableStoreLocator,
                EnablePolicies = entity.EnablePolicies,
                EnableFaq = entity.EnableFaq,

                ShowUserLoginLink = entity.ShowUserLoginLink,
                ShowOrderHistoryLink = entity.ShowOrderHistoryLink,
                ShowWishlistLink = entity.ShowWishlistLink,

                ShowTrendingOrLastest = entity.ShowTrendingOrLastest,

                WhereToFindUs = entity.WhereToFindUs,
                EnableNewsLetter = entity.EnableNewsLetter,

                EnableCopyright = entity.EnableCopyright,
                CopyrightText = entity.CopyrightText,
                CompanyId = entity.CompanyId,
                Active = entity.Active,
                Version = entity.Version
            };
            if (entity.BrandImages == null || entity.BrandImages.Count <= 0) return footerSettingVm;
            footerSettingVm.ThemeBrandImageUrls = entity.BrandImages.Select(o => o.ImageUrl).ToList();
            return footerSettingVm;
        }
    }
}

