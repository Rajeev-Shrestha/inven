using Hrevert.Common.Enums;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.ViewModels
{
    public class CarouselViewModel
    {
        public int? Id { get; set; }
        public ProductOrCategory ProductOrCategory { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public Image Image { get; set; }
        public string ImageUrl { get; set; }

        public int CompanyId { get; set; }
        public bool Active { get; set; }
        public bool WebActive { get; set; }
        public byte[] Version { get; set; }
    }
}
