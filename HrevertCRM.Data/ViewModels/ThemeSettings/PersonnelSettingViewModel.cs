using HrevertCRM.Entities;

namespace HrevertCRM.Data.ViewModels
{
    public class PersonnelSettingViewModel
    {
        public int? Id { get; set; }
        public ThemeImage PersonnelImage { get; set; }
        public string PersonnelImageUrl { get; set; }
        public string RecommendationText { get; set; }
        public string RecommendingPersonName { get; set; }
        public string RecommendingPersonAddress { get; set; }

        public int LayoutSettingId { get; set; }

        public int CompanyId { get; set; }
        public bool Active { get; set; }
        public byte[] Version { get; set; }
    }
}
