using System.Linq;
using HrevertCRM.Data.ViewModels.ThemeSettings;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper.ThemeSettings
{
    public class SlideSettingToSlideSettingViewModelMapper : MapperBase<SlideSetting, SlideSettingViewModel>
    {
        public override SlideSetting Map(SlideSettingViewModel viewModel)
        {
            var slideSetting = new SlideSetting
            {
                Id = viewModel.Id ?? 0,
                NumberOfSlides = viewModel.NumberOfSlides ?? 0,

                CompanyId = viewModel.CompanyId,
                Active = viewModel.Active,
                Version = viewModel.Version
            };
            if (viewModel.IndividualSlideSettingViewModels == null ||
                viewModel.IndividualSlideSettingViewModels.Count <= 0) return slideSetting;
            var mapper = new IndividualSlideSettingToIndividualSlideSettingViewModelMapper();
            slideSetting.IndividualSlideSettings = viewModel.IndividualSlideSettingViewModels.Select(o => mapper.Map(o))
                .ToList();
            return slideSetting;
        }

        public override SlideSettingViewModel Map(SlideSetting entity)
        {
            if (entity == null) return null;
            var slideSettingVm = new SlideSettingViewModel()
            {
                Id = entity.Id,
                NumberOfSlides = entity.NumberOfSlides,

                CompanyId = entity.CompanyId,
                Active = entity.Active,
                Version = entity.Version
            };
            if (entity.IndividualSlideSettings == null ||
                entity.IndividualSlideSettings.Count <= 0) return slideSettingVm;
            var mapper = new IndividualSlideSettingToIndividualSlideSettingViewModelMapper();
            slideSettingVm.IndividualSlideSettingViewModels = entity.IndividualSlideSettings.Select(o => mapper.Map(o))
                .ToList();
            return slideSettingVm;
        }
    }
}
