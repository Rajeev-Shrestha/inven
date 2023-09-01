namespace HrevertCRM.Entities
{
    public class BrandImage: BaseEntity, IWebItem
    {
        public int Id { get; set; }
        public  string ImageUrl{ get; set; }
        public int FooterSettingId { get; set; }
        public bool WebActive { get; set; }
        public virtual FooterSetting FooterSetting { get; set; }
    }
}
