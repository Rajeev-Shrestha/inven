namespace HrevertCRM.Entities
{
    public class CompanyLogo : BaseEntity, IWebItem
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public Image Image { get; set; }
        public bool WebActive{ get; set; }

        public string MediaUrl { get; set; }
    }
}
