namespace HrevertCRM.Entities
{
    public class PersonnelSetting : BaseEntity
    {
        public int Id { get; set; }
        public string PersonnelImageUrl { get; set; }
        public string RecommendationText { get; set; }
        public string RecommendingPersonName { get; set; }
        public string RecommendingPersonAddress { get; set; }

        public int LayoutSettingId { get; set; }
        public LayoutSetting LayoutSetting { get; set; }
    }
}
