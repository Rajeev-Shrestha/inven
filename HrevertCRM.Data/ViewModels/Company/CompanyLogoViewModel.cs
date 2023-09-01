using HrevertCRM.Entities;

namespace HrevertCRM.Data.ViewModels
{
    public class CompanyLogoViewModel: IWebItem
    {
        public int? Id { get; set; }
        public string CompanyName { get; set; }
        public bool WebActive { get; set; }
        public byte[] Version { get; set; }
        public bool Active { get; set; }
        public int CompanyId { get; set; }
        public string MediaUrl { get; set; }
        public Image LogoImage { get; set; }
    }
}
