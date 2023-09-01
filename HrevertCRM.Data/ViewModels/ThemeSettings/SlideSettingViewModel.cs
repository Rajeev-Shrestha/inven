using System.Collections.Generic;

namespace HrevertCRM.Data.ViewModels.ThemeSettings
{
    public class SlideSettingViewModel
    {
        public int? Id { get; set; }
        public int? NumberOfSlides { get; set; }
        public List<IndividualSlideSettingViewModel> IndividualSlideSettingViewModels { get; set; }

        public int CompanyId { get; set; }
        public bool Active { get; set; }
        public byte[] Version { get; set; }
    }
}
