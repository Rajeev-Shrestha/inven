using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper
{
    public class PersonnelSettingToPersonnelSettingViewModelMapper : MapperBase<PersonnelSetting, PersonnelSettingViewModel>
    {
        public override PersonnelSetting Map(PersonnelSettingViewModel viewModel)
        {
            return new PersonnelSetting
            {
                Id = viewModel.Id ?? 0,
                PersonnelImageUrl = viewModel.PersonnelImageUrl,
                RecommendationText = viewModel.RecommendationText,
                RecommendingPersonName = viewModel.RecommendingPersonName,
                RecommendingPersonAddress = viewModel.RecommendingPersonAddress,
                LayoutSettingId = viewModel.LayoutSettingId,
                Version = viewModel.Version,
                CompanyId = viewModel.CompanyId,
                Active = viewModel.Active
            };
        }

        public override PersonnelSettingViewModel Map(PersonnelSetting entity)
        {
            return new PersonnelSettingViewModel
            {
                Id = entity.Id,
                PersonnelImageUrl = entity.PersonnelImageUrl,
                RecommendationText = entity.RecommendationText,
                RecommendingPersonName = entity.RecommendingPersonName,
                RecommendingPersonAddress = entity.RecommendingPersonAddress,
                LayoutSettingId = entity.LayoutSettingId,
                Version = entity.Version,
                CompanyId = entity.CompanyId,
                Active = entity.Active
            };
        }
    }
}
